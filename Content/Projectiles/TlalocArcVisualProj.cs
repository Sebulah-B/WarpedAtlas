using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Buffs;

namespace WarpedAtlas.Content.Projectiles
{
    public class TlalocArcVisualProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CultistBossLightningOrbArc;

        public override void SetDefaults() {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 18;
            Projectile.penetrate = -1;
        }

        public override bool PreDraw(ref Color lightColor) {
            Vector2 start = Projectile.Center;
            Vector2 end = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            float alpha = Projectile.timeLeft / 18f;

            int segments = 8;
            Vector2 prev = start;
            Vector2 diff = end - start;
            if (diff.LengthSquared() < 4f) return false;
            Vector2 normal = Vector2.Normalize(diff).RotatedBy(MathHelper.PiOver2);
            for (int i = 1; i <= segments; i++) {
                float t = i / (float)segments;
                Vector2 next = Vector2.Lerp(start, end, t);
                if (i < segments) {
                    float jitter = (float)Math.Sin(Main.GameUpdateCount * 0.8f + i * 2.3f) * 14f;
                    next += normal * jitter;
                }
                Utils.DrawLine(Main.spriteBatch, prev - Main.screenPosition, next - Main.screenPosition, Color.Cyan * alpha, Color.White * alpha, 4f);
                prev = next;
            }
            return false;
        }
    }

    public class LivingLightningOrbProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CultistBossLightningOrb;

        public override void SetDefaults() {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;
        }

        public override void AI() {
            Projectile.rotation += 0.2f;
            for (int i = 0; i < 2; i++) {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 1.1f);
                d.noGravity = true;
            }

            NPC target = null;
            float bestDist = 550f;
            for (int i = 0; i < Main.maxNPCs; i++) {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage) {
                    float d = Vector2.Distance(npc.Center, Projectile.Center);
                    if (d < bestDist) {
                        bestDist = d;
                        target = npc;
                    }
                }
            }

            if (target != null) {
                Vector2 desired = Vector2.Normalize(target.Center - Projectile.Center) * 12f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, 0.12f);
            }
        }
    }
}