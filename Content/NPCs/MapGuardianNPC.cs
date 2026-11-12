using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using WarpedAtlas.Content.Subworlds;
using WarpedAtlas.Content.Items.Currencies;

namespace WarpedAtlas.Content.NPCs
{
    public abstract class MapGuardianNPC : ModNPC
    {
        public virtual int ProjectileType => ProjectileID.None;
        public virtual float ProjectileSpeed => 8f;
        public virtual int ProjectileDamage => 25;
        public virtual float ProjectileSpread => 0f;

        public override void SetDefaults() {
            NPC.width = 40;
            NPC.height = 56;
            NPC.damage = 35;
            NPC.defense = 18;
            NPC.lifeMax = 1200;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 50000f;
            NPC.knockBackResist = 0.1f;
            NPC.aiStyle = -1; 
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.boss = true; 
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment) {
            int tier = BaseDungeonSubworld.CurrentMapTier;
            float hpMult = 1f + ((tier - 1) * 0.35f);
            float dmgMult = 1f + ((tier - 1) * 0.20f);
            NPC.lifeMax = (int)(NPC.lifeMax * hpMult * balance * bossAdjustment);
            NPC.life = NPC.lifeMax; 
            NPC.damage = (int)(NPC.damage * dmgMult);
        }

        public override void FindFrame(int frameHeight) {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6) {
                NPC.frame.Y = (NPC.frame.Y + frameHeight) % (Main.npcFrameCount[Type] * frameHeight);
                NPC.frameCounter = 0;
            }
        }

        public override void AI() {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead) return;
            
            // --- TIER-SCALED AGGRESSION & PHASE TRACKING ---
            int tier = BaseDungeonSubworld.CurrentMapTier;
            bool isPhase2 = NPC.life < (NPC.lifeMax / 2);
            
            // Base speed scales gently: Tier 1 is a manageable 3f, Tier 8 pushes 6.5f
            float baseSpeed = 2.5f + (tier * 0.5f); 
            float speedMult = isPhase2 ? 1.3f : 1.0f; // Softened Phase 2 boost from 1.5x to 1.3x
            float currentSpeed = baseSpeed * speedMult;
            
            int attackCooldown = isPhase2 ? 70 : 120;
            
            Vector2 direction = player.Center - NPC.Center;
            NPC.spriteDirection = direction.X > 0 ? 1 : -1;
            
            // --- MOVEMENT & JUMP COOLDOWN ---
            if (NPC.velocity.Y == 0 && Math.Abs(direction.X) > 40) {
                NPC.velocity.X = direction.X > 0 ? currentSpeed : -currentSpeed;
                
                // Jump trigger: Player is far above OR boss is stuck on a wall
                if (direction.Y < -50 || (NPC.collideX && Math.Abs(NPC.velocity.X) < 0.5f)) {
                    if (NPC.ai[1] <= 0) { // ai[1] acts as our jump cooldown timer
                        NPC.velocity.Y = -7f - (tier * 0.2f); // Jump height scales slightly with tier
                        NPC.ai[1] = 45; // 45-tick delay before they can jump again
                    }
                }
            }
            
            // Only decrement the jump cooldown while grounded so they don't spam jump in corners
            if (NPC.velocity.Y == 0 && NPC.ai[1] > 0) {
                NPC.ai[1]--; 
            }

            // --- HOSTILE PROJECTILE ATTACKS ---
            if (ProjectileType != ProjectileID.None && Main.netMode != NetmodeID.MultiplayerClient) {
                NPC.ai[0]++; 
                if (NPC.ai[0] >= attackCooldown) {
                    NPC.ai[0] = 0;
                    
                    Vector2 shootVelocity = direction;
                    shootVelocity.Normalize();
                    shootVelocity *= ProjectileSpeed;

                    int shots = isPhase2 ? 3 : 1; 
                    for (int i = 0; i < shots; i++) {
                        Vector2 perturbedSpeed = shootVelocity.RotatedByRandom(MathHelper.ToRadians(isPhase2 ? 25 : 0));
                        
                        // Spawn the projectile and capture its index
                        int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, perturbedSpeed, ProjectileType, ProjectileDamage, 0f, Main.myPlayer);
                        
                        // Force the projectile to be hostile regardless of its vanilla defaults
                        Main.projectile[proj].friendly = false;
                        Main.projectile[proj].hostile = true;
                    }
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ChaosOrb>(), 1)); 
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OrbOfAlchemy>(), 1, 1, 2)); 
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RegalOrb>(), 1, 1, 2)); 
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.ApexCatalystKey>(), 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OrbOfScouring>(), 1, 1, 3)); // Guaranteed 1-3
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OrbOfAnnulment>(), 2)); // 50% chance
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DivineOrb>(), 3)); // 33% chance
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ExaltedOrb>(), 5)); // 20% chance 
        }
    }

    // --- TIER 1 TO 4 GUARDIANS ---
    public class DungeonGuardianNPC : MapGuardianNPC {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.Paladin;
        public override int ProjectileType => ProjectileID.WaterBolt;
        public override void SetStaticDefaults() { Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Paladin]; }
    }
    public class LihzahrdGuardianNPC : MapGuardianNPC {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.Lihzahrd;
        public override int ProjectileType => ProjectileID.Fireball;
        public override void SetStaticDefaults() { Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Lihzahrd]; }
    }
    public class SandstoneGuardianNPC : MapGuardianNPC {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.SandElemental;
        public override int ProjectileType => ProjectileID.DesertDjinnCurse;
        public override void SetStaticDefaults() { Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.SandElemental]; }
    }
    public class HallowedGuardianNPC : MapGuardianNPC {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.ChaosElemental;
        public override int ProjectileType => ProjectileID.FairyQueenMagicItemShot;
        public override void SetStaticDefaults() { Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.ChaosElemental]; }
    }

    // --- TIER 5 TO 8 GUARDIANS ---
    public class CryoFrostGuardianNPC : MapGuardianNPC {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.IceGolem;
        public override int ProjectileType => ProjectileID.FrostBlastHostile;
        public override void SetStaticDefaults() { Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.IceGolem]; }
    }
    public class HellforgeGuardianNPC : MapGuardianNPC {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.RedDevil;
        public override int ProjectileType => ProjectileID.UnholyTridentHostile;
        public override void SetStaticDefaults() { Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.RedDevil]; }
    }
    public class GraniteGuardianNPC : MapGuardianNPC {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.GraniteGolem;
        public override int ProjectileType => ProjectileID.Geode;
        public override void SetStaticDefaults() { Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.GraniteGolem]; }
    }
    public class FungalGuardianNPC : MapGuardianNPC {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.FungiBulb;
        public override int ProjectileType => ProjectileID.Mushroom;
        public override void SetStaticDefaults() { Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.FungiBulb]; }
    }
}