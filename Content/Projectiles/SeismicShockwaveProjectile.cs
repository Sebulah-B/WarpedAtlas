using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Projectiles
{
    public class SeismicShockwaveProjectile : ModProjectile
    {
        // Uses the vanilla Nebula explosion ring texture as a base for the shockwave
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.NebulaArcanumExplosionShot;

        private const int MaxDuration = 25;
        private const float MaxRadius = 350f; // 350 pixels (~22 blocks) radius

        public override void SetDefaults() {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1; // Infinite piercing so it hits the whole room
            Projectile.timeLeft = MaxDuration;
            Projectile.tileCollide = false; // Passes through walls
            Projectile.ignoreWater = true;

            // Uses local immunity so it hits every enemy in the room exactly ONCE per shout
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            
            // Lock the center of the earthquake to the player's body
            Projectile.Center = player.Center;

            // Calculate progress from 0.0f (start) to 1.0f (end)
            float progress = 1f - ((float)Projectile.timeLeft / MaxDuration);
            float currentRadius = MaxRadius * progress;

            // Generate a 360-degree expanding ring of stone and magma dust
            int dustPoints = 36;
            for (int i = 0; i < dustPoints; i++) {
                float angle = (i / (float)dustPoints) * MathHelper.TwoPi;
                Vector2 offset = angle.ToRotationVector2() * currentRadius;
                Vector2 dustPos = Projectile.Center + offset;

                int dustType = Main.rand.NextBool(3) ? DustID.SolarFlare : DustID.Stone;
                Dust d = Dust.NewDustPerfect(dustPos, dustType, offset.SafeNormalize(Vector2.Zero) * 2f, 100, default, 1.8f * (1f - progress * 0.5f));
                d.noGravity = true;
            }
        }

        // Dynamically expands the actual damage hitbox outward from the center
        public override void ModifyDamageHitbox(ref Rectangle hitbox) {
            float progress = 1f - ((float)Projectile.timeLeft / MaxDuration);
            int currentRadius = (int)(MaxRadius * progress);
            
            hitbox = new Rectangle(
                (int)Projectile.Center.X - currentRadius,
                (int)Projectile.Center.Y - currentRadius,
                currentRadius * 2,
                currentRadius * 2
            );
        }

        // Custom rendering to draw an expanding, fading shockwave aura
        public override bool PreDraw(ref Color lightColor) {
            Main.instance.LoadProjectile(ProjectileID.NebulaArcanumExplosionShot);
            Texture2D texture = TextureAssets.Projectile[ProjectileID.NebulaArcanumExplosionShot].Value;

            float progress = 1f - ((float)Projectile.timeLeft / MaxDuration);
            
            // Fade out as it reaches the edge
            float alpha = (float)Math.Sin(progress * MathHelper.Pi); 
            Color drawColor = new Color(255, 120, 40, 0) * alpha * 0.8f; // Additive fiery orange

            // Scale the sprite to match the 350f radius
            float scale = (MaxRadius * 2f / texture.Width) * progress;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation + (progress * 2f),
                texture.Size() / 2f,
                scale,
                SpriteEffects.None,
                0
            );

            return false; // Skip default drawing
        }
    }
}