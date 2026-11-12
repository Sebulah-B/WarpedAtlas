using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items.Uniques;
using WarpedAtlas.Content.Tiles;

namespace WarpedAtlas.Content.NPCs
{
    // ==========================================
    // CUSTOM PROJECTILES (NESTED IN THIS FILE)
    // ==========================================
    public class BloodRootHazard : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CursedFlameHostile;

        public override void SetDefaults() {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 540;
            Projectile.penetrate = -1;
        }

        public override void AI() {
            if (Projectile.velocity.Y != 0f) {
                Projectile.velocity.Y = Math.Min(12f, Projectile.velocity.Y + 0.45f);
            }

            if (Projectile.timeLeft % 120 == 0) {
                SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
                for (int i = 0; i < 24; i++) {
                    Vector2 dustVel = Main.rand.NextVector2Circular(7f, 7f);
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CrimsonTorch, dustVel.X, dustVel.Y, 100, default, 2.2f);
                    d.noGravity = true;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -7f), ProjectileID.GreekFire1, Projectile.damage, 0f, Main.myPlayer);
                }
            }

            if (Main.rand.NextBool(3)) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, -2f, 100, default, 1.3f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }

    public class TectonicShockwaveProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Boulder;

        public override void SetDefaults() {
            Projectile.width = 36;
            Projectile.height = 48;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 150;
            Projectile.penetrate = -1;
        }

        public override void AI() {
            Projectile.rotation += Projectile.velocity.X * 0.05f;
            for (int i = 0; i < 3; i++) {
                Dust d = Dust.NewDustDirect(Projectile.Bottom - new Vector2(16, 12), 32, 12, DustID.Lihzahrd, 0f, -3f, 100, default, 1.8f);
                d.noGravity = true;
            }
        }
    }

    public class GlacialShatterBoulderProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CultistBossIceMist;

        public override void SetDefaults() {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
        }

        public override void AI() {
            Projectile.rotation += 0.12f;
            if (Main.rand.NextBool(2)) {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, Color.Cyan, 1.6f);
                d.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            if (Main.netMode != NetmodeID.MultiplayerClient) {
                for (int i = 0; i < 8; i++) {
                    Vector2 shardVel = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 8f * i) * 7.5f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shardVel, ProjectileID.FrostShard, Projectile.damage, 0f, Main.myPlayer);
                }
            }
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            target.AddBuff(BuffID.Chilled, 180);
            target.AddBuff(BuffID.Frostburn, 180);
        }
    }

    public class RisingMagmaGeyserProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.DD2BetsyFlameBreath;

        public override void SetDefaults() {
            Projectile.width = 48;
            Projectile.height = 64;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 240;
            Projectile.penetrate = -1;
        }

        public override void AI() {
            Projectile.velocity = Vector2.Zero;
            for (int i = 0; i < 4; i++) {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-6f, -1f), 100, default, 2.2f);
                d.noGravity = true;
            }
        }
    }

    public class ExpandingVoidCloudProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.SporeCloud;

        public override void SetDefaults() {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 420;
            Projectile.penetrate = -1;
        }

        public override void AI() {
            Projectile.velocity *= 0.96f;
            Projectile.scale = Math.Min(3.5f, Projectile.scale + 0.006f);
            Projectile.width = (int)(48 * Projectile.scale);
            Projectile.height = (int)(48 * Projectile.scale);

            if (Main.rand.NextBool(2)) {
                Vector2 offset = Main.rand.NextVector2Circular(Projectile.width * 0.5f, Projectile.height * 0.5f);
                Dust d = Dust.NewDustPerfect(Projectile.Center + offset, DustID.Shadowflame, Vector2.Zero, 150, Color.Purple, 1.8f);
                d.noGravity = true;
            }
        }
    }

    // ==========================================
    // BASE CLASS: Universal Scaling, Drops & AI
    // ==========================================
    public abstract class ApexBossBase : ModNPC
    {
        public ref float AITimer => ref NPC.ai[0];
        public ref float AttackState => ref NPC.ai[1];
        public abstract int BossTier { get; }

        public override void SetStaticDefaults() {
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment) {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * balance);
            NPC.damage = (int)(NPC.damage * 0.65f);
        }

        public override bool CheckActive() => false;

        protected Player GetTarget() {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active) {
                NPC.TargetClosest();
            }
            return Main.player[NPC.target];
        }

        protected Vector2 GetArenaCenter() {
            if (NPC.ai[2] > 0 && NPC.ai[3] > 0) {
                return new Vector2(NPC.ai[2] * 16f, (NPC.ai[3] - 18f) * 16f);
            }
            return new Vector2(400 * 16f, 380 * 16f);
        }

        protected float GetArenaFloorY() {
            if (NPC.ai[3] > 0) return NPC.ai[3] * 16f;
            return 440 * 16f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
            int wItem = BossTier switch {
                1 => ModContent.ItemType<TheLeviathan>(),
                2 => ModContent.ItemType<Faultline>(),
                3 => ModContent.ItemType<SunscorchedKhopesh>(),
                4 => ModContent.ItemType<PrismaticEdge>(),
                5 => ModContent.ItemType<Avalanche>(),
                6 => ModContent.ItemType<CruciblesWrath>(),
                7 => ModContent.ItemType<KineticCrusher>(),
                _ => ModContent.ItemType<MycelialGlaive>()
            };
            int rItem = BossTier switch {
                1 => ModContent.ItemType<WindripperEcho>(),
                2 => ModContent.ItemType<RubbleMaker>(),
                3 => ModContent.ItemType<DuneShredder>(),
                4 => ModContent.ItemType<HeavensMandate>(),
                5 => ModContent.ItemType<Hailstorm>(),
                6 => ModContent.ItemType<BrimstoneSalvo>(),
                7 => ModContent.ItemType<RailgunCore>(),
                _ => ModContent.ItemType<ToxicProliferator>()
            };
            int mItem = BossTier switch {
                1 => ModContent.ItemType<AstralBattery>(),
                2 => ModContent.ItemType<GeomancersOmen>(),
                3 => ModContent.ItemType<MirageWeaver>(),
                4 => ModContent.ItemType<SeraphsGaze>(),
                5 => ModContent.ItemType<AbsoluteZero>(),
                6 => ModContent.ItemType<PyromancersBrand>(),
                7 => ModContent.ItemType<GeodeChanneler>(),
                _ => ModContent.ItemType<VoidBlight>()
            };
            int sItem = BossTier switch {
                1 => ModContent.ItemType<SovereignChain>(),
                2 => ModContent.ItemType<MonolithCore>(),
                3 => ModContent.ItemType<PharaohsCurse>(),
                4 => ModContent.ItemType<SeraphimChime>(),
                5 => ModContent.ItemType<CryoHeart>(),
                6 => ModContent.ItemType<AshenLash>(),
                7 => ModContent.ItemType<MatrixOverlord>(),
                _ => ModContent.ItemType<FungalSymbiote>()
            };

            npcLoot.Add(ItemDropRule.OneFromOptions(1, wItem, rItem, mItem, sItem));
        }
    }

    // ==========================================
    // TIER 1: Warden of Mictlan (Full Mourning Wood)
    // Mobility Test: Basic Boots & Platform Hopping
    // ==========================================
    public class ApexSovereignNPC : ApexBossBase
    {
        public override int BossTier => 1;
        public override string Texture => "Terraria/Images/NPC_" + NPCID.MourningWood;
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.MourningWood];
        }

        public override void SetDefaults() {
            NPC.width = 120; NPC.height = 160;
            NPC.damage = 42; NPC.defense = 16; NPC.lifeMax = 4500;
            NPC.HitSound = SoundID.NPCHit7; NPC.DeathSound = SoundID.NPCDeath10;
            NPC.boss = true; NPC.noGravity = true; NPC.noTileCollide = true;
            AnimationType = NPCID.MourningWood;
        }

        public override void AI() {
            Player player = GetTarget();
            if (player.dead) { NPC.velocity.Y -= 0.5f; return; }
            NPC.spriteDirection = player.Center.X > NPC.Center.X ? 1 : -1;

            AITimer++;
            int phase = (int)(AttackState % 3);

            if (phase == 0) {
                // Stalking ground-level march + Cinder volleys
                Vector2 marchTarget = new Vector2(player.Center.X + (player.Center.X > NPC.Center.X ? -180f : 180f), GetArenaFloorY() - 90f);
                NPC.velocity = Vector2.Lerp(NPC.velocity, (marchTarget - NPC.Center) * 0.04f, 0.1f);

                if (AITimer % 65 == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
                    SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                    for (int i = -1; i <= 1; i++) {
                        Vector2 vel = Vector2.Normalize(player.Center - NPC.Center).RotatedBy(i * 0.22f) * 6.5f;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel, ProjectileID.GreekFire1, NPC.damage / 2, 0f, Main.myPlayer);
                    }
                }
            } else if (phase == 1) {
                // Overhead Canopy Sweep + Planting Blood-Root Platform Hazards
                Vector2 sweepTarget = player.Center + new Vector2((float)Math.Sin(AITimer * 0.05f) * 260f, -240f);
                NPC.velocity = Vector2.Lerp(NPC.velocity, (sweepTarget - NPC.Center) * 0.06f, 0.12f);

                if (AITimer % 55 == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
                    SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 9f), ModContent.ProjectileType<BloodRootHazard>(), NPC.damage / 2, 0f, Main.myPlayer);
                }
            } else {
                // Bull Rush Charge across the floor
                if (AITimer == 1) {
                    SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                    float dir = player.Center.X > NPC.Center.X ? 1f : -1f;
                    NPC.velocity = new Vector2(dir * 11f, 0f);
                }
                NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, (player.Center.Y - NPC.Center.Y) * 0.02f, 0.1f);
            }

            if (AITimer >= 180) {
                AITimer = 0;
                AttackState++;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Main.instance.LoadNPC(NPCID.MourningWood);
            Texture2D tex = TextureAssets.Npc[NPCID.MourningWood].Value;
            Vector2 origin = new Vector2(tex.Width * 0.5f, (tex.Height / Main.npcFrameCount[Type]) * 0.5f);
            SpriteEffects fx = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(tex, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, new Color(210, 45, 45), NPC.rotation, origin, NPC.scale, fx, 0f);
            return false;
        }
    }

    // ==========================================
    // TIER 2: Earth-Shaker Tepeyollotl (Full Golem Body + Fists)
    // Mobility Test: Hook Timing & Jump Suppression
    // ==========================================
    public class ApexColossusNPC : ApexBossBase
    {
        public override int BossTier => 2;
        public override string Texture => "Terraria/Images/NPC_" + NPCID.Golem;
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Golem];
        }

        public override void SetDefaults() {
            NPC.width = 110; NPC.height = 140;
            NPC.damage = 58; NPC.defense = 28; NPC.lifeMax = 8500;
            NPC.HitSound = SoundID.NPCHit41; NPC.DeathSound = SoundID.NPCDeath43;
            NPC.boss = true; NPC.noGravity = false; NPC.noTileCollide = false;
            NPC.scale = 1.25f;
            AnimationType = NPCID.Golem;
        }

        public override void AI() {
            Player player = GetTarget();
            if (player.dead) return;
            NPC.spriteDirection = player.Center.X > NPC.Center.X ? 1 : -1;

            // Tectonic Gravity Aura: Weighs down jump height slightly, encouraging hook usage
            if (player.velocity.Y < 0f) player.velocity.Y += 0.18f;

            AITimer++;

            // Grounded pursuit between leaps
            if (NPC.velocity.Y == 0f) {
                float targetSpeed = Math.Sign(player.Center.X - NPC.Center.X) * 3.8f;
                NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, targetSpeed, 0.08f);
            }

            // Tectonic Leap every 3.5 seconds
            if (AITimer == 140 && NPC.velocity.Y == 0f) {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                NPC.velocity.Y = -14.5f;
                NPC.velocity.X = MathHelper.Clamp((player.Center.X - NPC.Center.X) * 0.045f, -11f, 11f);
            }

            // Eye Beam Volley mid-cycle
            if (AITimer == 75 && Main.netMode != NetmodeID.MultiplayerClient) {
                SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                Vector2 eyeDir = Vector2.Normalize(player.Center - (NPC.Top + new Vector2(0, 24)));
                for (int i = -1; i <= 1; i++) {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Top + new Vector2(0, 24), eyeDir.RotatedBy(i * 0.15f) * 8.5f, ProjectileID.EyeBeam, NPC.damage / 2, 0f, Main.myPlayer);
                }
            }

            if (AITimer >= 200) AITimer = 0;

            // Slam Shockwave upon hitting the ground
            if (NPC.velocity.Y == 0f && NPC.oldVelocity.Y > 3f) {
                SoundEngine.PlaySound(SoundID.Item14, NPC.Bottom);
                for (int i = 0; i < 35; i++) {
                    Dust.NewDust(NPC.Bottom - new Vector2(50, 8), 100, 16, DustID.Lihzahrd, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-5f, 0f), 100, default, 2f);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Bottom + new Vector2(0, -16), new Vector2(8.5f, 0f), ModContent.ProjectileType<TectonicShockwaveProj>(), NPC.damage / 2, 0f, Main.myPlayer);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Bottom + new Vector2(0, -16), new Vector2(-8.5f, 0f), ModContent.ProjectileType<TectonicShockwaveProj>(), NPC.damage / 2, 0f, Main.myPlayer);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Main.instance.LoadNPC(NPCID.Golem);
            Main.instance.LoadNPC(NPCID.GolemFistLeft);
            Texture2D bodyTex = TextureAssets.Npc[NPCID.Golem].Value;
            Texture2D fistTex = TextureAssets.Npc[NPCID.GolemFistLeft].Value;

            Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY);
            Vector2 bodyOrigin = new Vector2(bodyTex.Width * 0.5f, (bodyTex.Height / Main.npcFrameCount[Type]) * 0.5f);

            // Draw oscillating Golem Fists flanking the full Golem Body
            float fistOffset = (float)Math.Sin(Main.GameUpdateCount * 0.08f) * 18f;
            Rectangle fistRect = new Rectangle(0, 0, fistTex.Width, fistTex.Height / Main.npcFrameCount[NPCID.GolemFistLeft]);
            spriteBatch.Draw(fistTex, drawPos + new Vector2(-78f - fistOffset, -10f), fistRect, drawColor, 0f, fistRect.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(fistTex, drawPos + new Vector2(78f + fistOffset, -10f), fistRect, drawColor, 0f, fistRect.Size() * 0.5f, NPC.scale, SpriteEffects.FlipHorizontally, 0f);

            spriteBatch.Draw(bodyTex, drawPos, NPC.frame, drawColor, NPC.rotation, bodyOrigin, NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
    }

    // ==========================================
    // TIER 3: Toltec Sun-Priest (Full Sand Elemental)
    // Mobility Test: Multi-Tier Platform Swapping & Mirage Tracking
    // ==========================================
    public class ApexPharaohNPC : ApexBossBase
    {
        public override int BossTier => 3;
        public override string Texture => "Terraria/Images/NPC_" + NPCID.SandElemental;
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.SandElemental];
        }

        public override void SetDefaults() {
            NPC.width = 70; NPC.height = 110;
            NPC.damage = 74; NPC.defense = 24; NPC.lifeMax = 13000;
            NPC.HitSound = SoundID.NPCHit23; NPC.DeathSound = SoundID.NPCDeath26;
            NPC.boss = true; NPC.noGravity = true; NPC.noTileCollide = true;
            AnimationType = NPCID.SandElemental;
        }

        public override void AI() {
            Player player = GetTarget();
            if (player.dead) return;
            NPC.spriteDirection = player.Center.X > NPC.Center.X ? 1 : -1;

            // Figure-8 Orbit around the player
            float angle = Main.GameUpdateCount * 0.03f;
            Vector2 orbitOffset = new Vector2((float)Math.Cos(angle) * 280f, (float)Math.Sin(angle * 2f) * 140f - 80f);
            NPC.velocity = Vector2.Lerp(NPC.velocity, ((player.Center + orbitOffset) - NPC.Center) * 0.07f, 0.12f);

            AITimer++;
            if (AITimer == 90 && Main.netMode != NetmodeID.MultiplayerClient) {
                // Tri-angle Djinn Curse Barrage
                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                Vector2 dir = Vector2.Normalize(player.Center - NPC.Center);
                for (int i = -1; i <= 1; i++) {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, dir.RotatedBy(i * 0.35f) * 9f, ProjectileID.DesertDjinnCurse, NPC.damage / 2, 0f, Main.myPlayer);
                }
            }

            if (AITimer >= 160) {
                AITimer = 0;
                AttackState++;
                // Sandstorm Teleport + Ancient Storm Pillars
                NPC.Center = GetArenaCenter() + new Vector2(Main.rand.NextBool() ? -280f : 280f, Main.rand.NextFloat(-140f, 60f));
                SoundEngine.PlaySound(SoundID.Item122, NPC.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(Main.rand.NextFloat(-120f, 120f), 80f), Vector2.Zero, ProjectileID.SandnadoHostileMark, NPC.damage / 2, 0f, Main.myPlayer);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Main.instance.LoadNPC(NPCID.SandElemental);
            Texture2D tex = TextureAssets.Npc[NPCID.SandElemental].Value;
            Vector2 origin = new Vector2(tex.Width * 0.5f, (tex.Height / Main.npcFrameCount[Type]) * 0.5f);
            SpriteEffects fx = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // Draw 2 Orbiting Sun-Mirages
            float rot = Main.GameUpdateCount * 0.05f;
            Vector2 m1 = NPC.Center + rot.ToRotationVector2() * 140f - screenPos;
            Vector2 m2 = NPC.Center + (rot + MathHelper.Pi).ToRotationVector2() * 140f - screenPos;
            spriteBatch.Draw(tex, m1, NPC.frame, Color.Gold * 0.35f, NPC.rotation, origin, NPC.scale * 0.9f, fx, 0f);
            spriteBatch.Draw(tex, m2, NPC.frame, Color.Gold * 0.35f, NPC.rotation, origin, NPC.scale * 0.9f, fx, 0f);

            // Real Boss glows bright gold prior to teleport
            Color realColor = AITimer > 110 ? Color.Gold : new Color(255, 220, 150) * 0.85f;
            spriteBatch.Draw(tex, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, realColor, NPC.rotation, origin, NPC.scale, fx, 0f);
            return false;
        }
    }

    // ==========================================
    // TIER 4: Feathered Arbiter Quetzal (Full Empress of Light)
    // Mobility Test: Grappling / Wing Gliding through Geometric Lattices
    // ==========================================
    public class ApexSeraphNPC : ApexBossBase
    {
        public override int BossTier => 4;
        public override string Texture => "Terraria/Images/NPC_" + NPCID.HallowBoss;
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.HallowBoss];
        }

        public override void SetDefaults() {
            NPC.width = 110; NPC.height = 130;
            NPC.damage = 92; NPC.defense = 35; NPC.lifeMax = 19000;
            NPC.HitSound = SoundID.NPCHit5; NPC.DeathSound = SoundID.NPCDeath7;
            NPC.boss = true; NPC.noGravity = true; NPC.noTileCollide = true;
            AnimationType = NPCID.HallowBoss;
        }

        public override void AI() {
            Player player = GetTarget();
            if (player.dead) return;

            Vector2 center = GetArenaCenter();
            NPC.velocity = Vector2.Lerp(NPC.velocity, (center - NPC.Center) * 0.08f, 0.15f);

            AITimer++;
            if (AITimer % 75 == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
                SoundEngine.PlaySound(SoundID.Item163, NPC.Center);
                AttackState++;

                if (AttackState % 2 == 0) {
                    // Horizontal & Vertical Lattice Grid across the Arena
                    for (int offset = -320; offset <= 320; offset += 160) {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), center + new Vector2(offset, -360f), new Vector2(0f, 8.5f), ProjectileID.FairyQueenLance, NPC.damage / 2, 0f, Main.myPlayer, MathHelper.PiOver2, AttackState * 0.1f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), center + new Vector2(-460f, offset * 0.6f), new Vector2(8.5f, 0f), ProjectileID.FairyQueenLance, NPC.damage / 2, 0f, Main.myPlayer, 0f, AttackState * 0.1f);
                    }
                } else {
                    // 10-Point Rotating Prismatic Starburst
                    for (int i = 0; i < 10; i++) {
                        Vector2 dir = Vector2.UnitY.RotatedBy(MathHelper.TwoPi / 10f * i + AttackState * 0.35f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, dir * 7.5f, ProjectileID.PhantasmalBolt, NPC.damage / 2, 0f, Main.myPlayer);
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Main.instance.LoadNPC(NPCID.HallowBoss);
            Texture2D tex = TextureAssets.Npc[NPCID.HallowBoss].Value;
            Vector2 origin = new Vector2(tex.Width * 0.5f, (tex.Height / Main.npcFrameCount[Type]) * 0.5f);
            spriteBatch.Draw(tex, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, Main.DiscoColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
    }

    // ==========================================
    // TIER 5: Itztli Frost Titan (Full Ice Golem)
    // Mobility Test: Mid-Air Wing Hovering & Shatter Wall Avoidance
    // ==========================================
    public class ApexCryoTitanNPC : ApexBossBase
    {
        public override int BossTier => 5;
        public override string Texture => "Terraria/Images/NPC_" + NPCID.IceGolem;
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.IceGolem];
        }

        public override void SetDefaults() {
            NPC.width = 90; NPC.height = 130;
            NPC.damage = 115; NPC.defense = 46; NPC.lifeMax = 26000;
            NPC.HitSound = SoundID.NPCHit5; NPC.DeathSound = SoundID.NPCDeath7;
            NPC.boss = true; NPC.noGravity = false; NPC.noTileCollide = false;
            NPC.scale = 1.25f;
            AnimationType = NPCID.IceGolem;
        }

        public override void AI() {
            Player player = GetTarget();
            if (player.dead) return;
            NPC.spriteDirection = player.Center.X > NPC.Center.X ? 1 : -1;

            float moveDir = Math.Sign(player.Center.X - NPC.Center.X);
            NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, moveDir * 4.2f, 0.06f);

            if (NPC.velocity.Y == 0f && player.Center.Y < NPC.Center.Y - 120f && AITimer % 90 == 0) {
                NPC.velocity.Y = -13f;
            }

            AITimer++;
            if (AITimer % 85 == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
                SoundEngine.PlaySound(SoundID.Item28, NPC.Center);
                Vector2 shootDir = Vector2.Normalize(player.Center - NPC.Center);
                // Launch wall-shattering Glacial Boulders
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shootDir * 9f, ModContent.ProjectileType<GlacialShatterBoulderProj>(), NPC.damage / 2, 0f, Main.myPlayer);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shootDir.RotatedBy(0.25f) * 11f, ProjectileID.FrostBeam, NPC.damage / 2, 0f, Main.myPlayer);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shootDir.RotatedBy(-0.25f) * 11f, ProjectileID.FrostBeam, NPC.damage / 2, 0f, Main.myPlayer);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Main.instance.LoadNPC(NPCID.IceGolem);
            Texture2D tex = TextureAssets.Npc[NPCID.IceGolem].Value;
            Vector2 origin = new Vector2(tex.Width * 0.5f, (tex.Height / Main.npcFrameCount[Type]) * 0.5f);
            SpriteEffects fx = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // Draw Rotating Hexagonal Ice Barrier
            float baseAngle = Main.GameUpdateCount * 0.03f;
            for (int i = 0; i < 6; i++) {
                Vector2 p1 = NPC.Center + (baseAngle + MathHelper.TwoPi / 6f * i).ToRotationVector2() * 95f - screenPos;
                Vector2 p2 = NPC.Center + (baseAngle + MathHelper.TwoPi / 6f * ((i + 1) % 6)).ToRotationVector2() * 95f - screenPos;
                Utils.DrawLine(spriteBatch, p1, p2, Color.Cyan * 0.8f, Color.DeepSkyBlue * 0.8f, 4f);
            }

            spriteBatch.Draw(tex, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, new Color(180, 240, 255), NPC.rotation, origin, NPC.scale, fx, 0f);
            return false;
        }
    }

    // ==========================================
    // TIER 6: Volcanic Core Xiuhtecuhtli (Full Betsy Dragon)
    // Mobility Test: Sustained Flight / Upper-Platform Aerial Combat (Floor Lava Flood)
    // ==========================================
    public class ApexInfernalNPC : ApexBossBase
    {
        public override int BossTier => 6;
        public override string Texture => "Terraria/Images/NPC_" + NPCID.DD2Betsy;
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.DD2Betsy];
        }

        public override void SetDefaults() {
            NPC.width = 140; NPC.height = 120;
            NPC.damage = 140; NPC.defense = 45; NPC.lifeMax = 35000;
            NPC.HitSound = SoundID.NPCHit1; NPC.DeathSound = SoundID.NPCDeath1;
            NPC.boss = true; NPC.noGravity = true; NPC.noTileCollide = true;
            AnimationType = NPCID.DD2Betsy;
        }

        public override void AI() {
            Player player = GetTarget();
            if (player.dead) return;
            NPC.spriteDirection = player.Center.X > NPC.Center.X ? -1 : 1;

            AttackState++; // Compounding Enrage Timer
            float enrageMult = 1f + Math.Min(1.2f, AttackState / 3600f);

            // Swooping aerial figure-eight above the player
            Vector2 targetPos = player.Center + new Vector2((float)Math.Cos(AttackState * 0.04f) * 320f, -200f + (float)Math.Sin(AttackState * 0.08f) * 70f);
            NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Normalize(targetPos - NPC.Center) * (7.5f * enrageMult), 0.08f);

            if (Main.rand.NextBool(2)) {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Lava, 0f, 2f, 100, default, 2f);
            }

            // Periodically flood the arena floor with Rising Magma Geysers
            if ((int)AttackState % 180 == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
                float floorY = GetArenaFloorY();
                float centerX = GetArenaCenter().X;
                for (int xOff = -450; xOff <= 450; xOff += 150) {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(centerX + xOff, floorY - 32f), Vector2.Zero, ModContent.ProjectileType<RisingMagmaGeyserProj>(), NPC.damage / 2, 0f, Main.myPlayer);
                }
            }

            if (++AITimer >= (int)(75f / enrageMult)) {
                AITimer = 0;
                SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, NPC.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    Vector2 aim = Vector2.Normalize(player.Center - NPC.Center) * (9f * enrageMult);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, aim, ProjectileID.CultistBossFireBall, (int)(NPC.damage * enrageMult) / 2, 0f, Main.myPlayer);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 9f), ProjectileID.GreekFire1, NPC.damage / 2, 0f, Main.myPlayer);
                }
            }
        }
    }

    // ==========================================
    // TIER 7: Apex Ollin (Full Granite Golem + Orbiting Tectonic Shields)
    // Mobility Test: High-Velocity Momentum Check & Laser Weaving
    // ==========================================
    public class ApexGraniteCoreNPC : ApexBossBase
    {
        public override int BossTier => 7;
        public override string Texture => "Terraria/Images/NPC_" + NPCID.GraniteGolem;
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.GraniteGolem];
        }

        public override void SetDefaults() {
            NPC.width = 90; NPC.height = 120;
            NPC.damage = 170; NPC.defense = 62; NPC.lifeMax = 46000;
            NPC.HitSound = SoundID.NPCHit4; NPC.DeathSound = SoundID.NPCDeath14;
            NPC.boss = true; NPC.noGravity = true; NPC.noTileCollide = true;
            NPC.scale = 1.6f;
            AnimationType = NPCID.GraniteGolem;
        }

        public override void AI() {
            Player player = GetTarget();
            if (player.dead) return;
            NPC.spriteDirection = player.Center.X > NPC.Center.X ? 1 : -1;

            NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Normalize(player.Center - NPC.Center) * 5.2f, 0.05f);

            AITimer++;
            // Sweeping Laser Barrages
            if (AITimer % 65 == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
                SoundEngine.PlaySound(SoundID.Item12, NPC.Center);
                Vector2 aim = Vector2.Normalize(player.Center - NPC.Center);
                for (int i = -2; i <= 2; i++) {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, aim.RotatedBy(i * 0.14f) * 11f, ProjectileID.SaucerLaser, NPC.damage / 2, 0f, Main.myPlayer);
                }
            }

            // Kinetic Repulsion Field Check every 3.5 seconds
            if (AITimer >= 210) {
                AITimer = 0;
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                if (player.velocity.Length() < 4.0f) {
                    player.velocity = Vector2.Normalize(player.Center - NPC.Center) * 20f;
                    Main.NewText("  Momentum insufficient! Ollin Repulsion Field triggered!", 80, 180, 255);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Main.instance.LoadNPC(NPCID.GraniteGolem);
            Main.instance.LoadItem(ItemID.GraniteBlock);
            Texture2D tex = TextureAssets.Npc[NPCID.GraniteGolem].Value;
            Texture2D rockTex = TextureAssets.Item[ItemID.GraniteBlock].Value;

            Vector2 origin = new Vector2(tex.Width * 0.5f, (tex.Height / Main.npcFrameCount[Type]) * 0.5f);
            SpriteEffects fx = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(tex, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, Color.White, NPC.rotation, origin, NPC.scale, fx, 0f);

            // Draw 4 Orbiting Tectonic Chunks
            float rot = Main.GameUpdateCount * 0.06f;
            for (int i = 0; i < 4; i++) {
                Vector2 chunkPos = NPC.Center + (rot + MathHelper.PiOver2 * i).ToRotationVector2() * 110f - screenPos;
                spriteBatch.Draw(rockTex, chunkPos, null, Color.Cyan, rot * 2f, rockTex.Size() * 0.5f, 2.2f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }

    // ==========================================
    // TIER 8: Eclipse Lord Tzitzimitl (Full Duke Fishron)
    // Mobility Test: Pinnacle 3D Arena Kiting, Dash-Dodging & Void Cloud Management
    // ==========================================
    public class ApexFungalLordNPC : ApexBossBase
    {
        public override int BossTier => 8;
        public override string Texture => "Terraria/Images/NPC_" + NPCID.DukeFishron;
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.DukeFishron];
        }

        public override void SetDefaults() {
            NPC.width = 150; NPC.height = 110;
            NPC.damage = 210; NPC.defense = 72; NPC.lifeMax = 65000;
            NPC.HitSound = SoundID.NPCHit14; NPC.DeathSound = SoundID.NPCDeath20;
            NPC.boss = true; NPC.noGravity = true; NPC.noTileCollide = true;
            AnimationType = NPCID.DukeFishron;
        }

        public override void AI() {
            Player player = GetTarget();
            if (player.dead) return;
            NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1;

            AITimer++;
            if (AITimer < 120) {
                // High-speed predatory tracking
                NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Normalize(player.Center - NPC.Center) * 8.5f, 0.08f);
            } else if (AITimer == 120 || AITimer == 150 || AITimer == 180) {
                // Triple Void Lunge Dash
                SoundEngine.PlaySound(SoundID.Zombie20, NPC.Center);
                NPC.velocity = Vector2.Normalize(player.Center - NPC.Center) * 18f;

                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextVector2Circular(3f, 3f), ModContent.ProjectileType<ExpandingVoidCloudProj>(), NPC.damage / 2, 0f, Main.myPlayer);
                }
            } else if (AITimer >= 210) {
                AITimer = 0;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
            var prog = target.GetModPlayer<AtlasProgressionPlayer>();
            int drainAmount = hurtInfo.Damage * 3;

            if (prog.CurrentEnergyShield > 0) {
                prog.CurrentEnergyShield = Math.Max(0, prog.CurrentEnergyShield - drainAmount);
            } else if (prog.CurrentWard > 0) {
                prog.CurrentWard = Math.Max(0, prog.CurrentWard - drainAmount);
            } else if (target.statMana > 0) {
                target.statMana = Math.Max(0, target.statMana - drainAmount);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Main.instance.LoadNPC(NPCID.DukeFishron);
            Texture2D tex = TextureAssets.Npc[NPCID.DukeFishron].Value;
            Vector2 origin = new Vector2(tex.Width * 0.5f, (tex.Height / Main.npcFrameCount[Type]) * 0.5f);
            SpriteEffects fx = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // Shadowflame Void Aura + Pitch-Black Leviathan Body
            spriteBatch.Draw(tex, NPC.Center - screenPos - NPC.velocity * 1.5f, NPC.frame, new Color(140, 30, 220, 120), NPC.rotation, origin, NPC.scale * 1.06f, fx, 0f);
            spriteBatch.Draw(tex, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, new Color(25, 10, 40), NPC.rotation, origin, NPC.scale, fx, 0f);
            return false;
        }
    }
}