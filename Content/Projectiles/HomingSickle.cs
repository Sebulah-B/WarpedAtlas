using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Projectiles
{
    public class HomingIceSickle : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.IceSickle;

        public override void SetDefaults() {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3; 
            Projectile.timeLeft = 240;
            Projectile.tileCollide = true;
        }

        public override void AI() {
            Projectile.rotation += 0.4f * Projectile.direction;

            if (Main.rand.NextBool(3)) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, default, 1f);
            }

            // Delay homing slightly so the projectile flies outward first
            if (Projectile.timeLeft < 210) {
                NPC closestTarget = null;
                float closestDist = 600f;
                
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
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetDir * 14f, 0.08f);
                }
            }
        }

        public override void OnKill(int timeLeft) {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center); 
            for (int i = 0; i < 15; i++) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3), 100, default, 1.2f);
            }
        }
    }
}