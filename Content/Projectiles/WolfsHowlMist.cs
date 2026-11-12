using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Projectiles
{
    public class WolfsHowlMist : ModProjectile
    {
        // Uses the Cultist's icy snowflake texture
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CultistBossIceMist;

        public override void SetDefaults() {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            
            Projectile.penetrate = 1; // Dies and explodes on the first enemy hit
            Projectile.timeLeft = 300;
            
            // KEY: Setting this to false makes it phase right through the floor/ceiling
            Projectile.tileCollide = false; 
        }

        public override void AI() {
            // Spin the mist as it falls
            Projectile.rotation += 0.15f * Projectile.direction;

            // Emit a trailing icy aura
            if (Main.rand.NextBool(2)) {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }

        public override void OnKill(int timeLeft) {
            // Explosive shatter sound and visual burst ONLY when it hits an enemy
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center); 
            
            for (int i = 0; i < 20; i++) {
                Dust.NewDustPerfect(
                    Projectile.Center, 
                    DustID.Ice, 
                    Main.rand.NextVector2Circular(6f, 6f), 
                    100, 
                    default, 
                    1.5f
                ).noGravity = true;
            }
        }
    }
}