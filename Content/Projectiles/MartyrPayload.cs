using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Projectiles
{
    public class MartyrPayload : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.LostSoulFriendly; // Invisible base, visually driven by dust
        
        private const int MaxRadius = 180; // Explosion radius

        public override void SetDefaults() {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = 1; 
            Projectile.timeLeft = 120; // 2 seconds to find a target or fizzle
            Projectile.tileCollide = false; // Phases through walls to reach targets
            Projectile.alpha = 255; // Fully transparent, drawn purely via dust trails
        }

        public override void AI() {
            // 1. Violent Trailing Visuals
            if (Main.rand.NextBool(2)) {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 1.8f);
                d.noGravity = true;
                d.velocity *= 0.2f;
                
                Dust shadow = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 100, default, 1.5f);
                shadow.noGravity = true;
            }

            // 2. Aggressive Homing Logic
            NPC closestTarget = null;
            float closestDist = 800f;

            for (int i = 0; i < Main.maxNPCs; i++) {
                NPC npc = Main.npc[i];
                if (npc.active && npc.CanBeChasedBy() && !npc.friendly) {
                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < closestDist) {
                        closestDist = dist;
                        closestTarget = npc;
                    }
                }
            }

            if (closestTarget != null) {
                Vector2 targetDir = (closestTarget.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                // Heavy steering factor (0.15f) for sharp, erratic turns
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetDir * 18f, 0.15f);
            }
        }

        public override void OnKill(int timeLeft) {
            // 1. High-Impact Audio (Flesh tear + Heavy Explosion)
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);

            // 2. Visceral Gore/Bone Explosion
            for (int i = 0; i < 40; i++) {
                float angle = (i / 40f) * MathHelper.TwoPi;
                Vector2 dustVel = angle.ToRotationVector2() * Main.rand.NextFloat(4f, 12f);
                
                Dust.NewDustPerfect(Projectile.Center, DustID.Blood, dustVel, 0, default, 2.5f).noGravity = true;
                if (i % 3 == 0) {
                    Dust.NewDustPerfect(Projectile.Center, DustID.Bone, dustVel * 0.8f, 0, default, 1.5f);
                }
            }

            // 3. Manual AoE Damage Execution
            for (int i = 0; i < Main.maxNPCs; i++) {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage) {
                    if (Vector2.Distance(Projectile.Center, npc.Center) <= MaxRadius) {
                        // Clears iFrames so players can rapidly chain-detonate multiple minions on the same boss
                        npc.immune[Projectile.owner] = 0;
                        // Insert 'false' as the 3rd argument to satisfy the 'bool crit' parameter
                        npc.SimpleStrikeNPC(Projectile.damage, npc.Center.X < Projectile.Center.X ? 1 : -1, false, Projectile.knockBack);
                    }
                }
            }
        }
    }
}