using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Projectiles
{
    public class AncientDoomProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ShadowBeamHostile; 

        private const int ArmLength = 400; // 400px in all 4 directions (800px total span)
        private const int ArmWidth = 40;   // The thickness of the damaging cross
        private const int DetonationDelay = 90; // 1.5 seconds (90 ticks)

        public override void SetDefaults() {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1; // Infinite piercing through the horde
            Projectile.timeLeft = DetonationDelay + 5; 
            Projectile.tileCollide = false; 
            Projectile.alpha = 255; // Fully transparent, visually driven by dust
            
            // Prevents multihit shredding; hits every enemy exactly once upon eruption
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1; 
        }

        public override void AI() {
            Projectile.ai[0]++;
            
            if (Projectile.ai[0] < DetonationDelay) {
                // 1. Telegraph Phase: Draw the warning cross on the terrain
                for (int i = 0; i < 2; i++) {
                    float offset = Main.rand.NextFloat(-ArmLength, ArmLength);
                    Vector2 spawnPos = Main.rand.NextBool() 
                        ? Projectile.Center + new Vector2(offset, 0)  // Horizontal line
                        : Projectile.Center + new Vector2(0, offset); // Vertical line
                        
                    Dust d = Dust.NewDustPerfect(spawnPos, DustID.PurpleCrystalShard, Vector2.Zero, 100, default, 1.2f);
                    d.noGravity = true;
                }
            } 
            else if (Projectile.ai[0] == DetonationDelay) {
                // 2. Eruption Phase: Audio and heavy visual execution
                SoundEngine.PlaySound(SoundID.Item74 with { Pitch = -0.4f, Volume = 1.2f }, Projectile.Center); 
                
                // Flood the axes with dense shadowflame
                for (int i = -ArmLength; i <= ArmLength; i += 12) {
                    Dust.NewDustPerfect(Projectile.Center + new Vector2(i, 0), DustID.Shadowflame, Main.rand.NextVector2Circular(2f, 10f), 0, default, 2.5f).noGravity = true;
                    Dust.NewDustPerfect(Projectile.Center + new Vector2(0, i), DustID.Shadowflame, Main.rand.NextVector2Circular(10f, 2f), 0, default, 2.5f).noGravity = true;
                }
            }
        }

        // 3. Custom Geometry Collision
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            // Suppress all damage until the exact moment of eruption
            if (Projectile.ai[0] >= DetonationDelay) {
                float point = 0f;
                Vector2 center = Projectile.Center;
                
                // Check Horizontal Arm Intersection
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), center + new Vector2(-ArmLength, 0), center + new Vector2(ArmLength, 0), ArmWidth, ref point)) 
                    return true;
                
                // Check Vertical Arm Intersection
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), center + new Vector2(0, -ArmLength), center + new Vector2(0, ArmLength), ArmWidth, ref point)) 
                    return true;
            }
            return false;
        }
    }
}