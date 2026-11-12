using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Projectiles
{
    public class ShrapnelTrapProjectile : ModProjectile
    {
        // FIX: Points the projectile's sprite to the vanilla Landmine ITEM texture so it is actually visible
        public override string Texture => "Terraria/Images/Item_" + ItemID.LandMine;

        // State Machine tracking
        public ref float State => ref Projectile.ai[0];
        public ref float Timer => ref Projectile.ai[1];

        private const float STATE_AIRBORNE = 0f;
        private const float STATE_ARMED = 1f;

        public override void SetDefaults() {
            Projectile.width = 24; // Matched to the size of the item sprite
            Projectile.height = 24;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = true;
        }

        public override void AI() {
            if (State == STATE_AIRBORNE) {
                // Apply standard gravity and friction
                Projectile.velocity.Y += 0.2f; 
                if (Projectile.velocity.Y > 16f) Projectile.velocity.Y = 16f;
                Projectile.velocity.X *= 0.98f;

                // FIX: Add visual traveling feedback for aiming
                Projectile.rotation += Projectile.velocity.X * 0.05f; // Spin while flying
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f); // Dust trail

                // Arm the trap if it settles on the ground naturally
                if (Projectile.velocity.Length() < 0.2f) {
                    Projectile.velocity = Vector2.Zero;
                    Projectile.rotation = 0f; // Snap flat to the ground
                    State = STATE_ARMED;
                    SoundEngine.PlaySound(SoundID.MaxMana, Projectile.Center); 
                }
            } 
            else if (State == STATE_ARMED) {
                // Emit a pulsing visual indicator to show it is armed
                Timer++;
                if (Timer % 30 == 0) {
                    Dust.NewDustPerfect(Projectile.Center, DustID.MartianSaucerSpark, Vector2.Zero, 0, Color.White, 1.5f);
                }

                // Proximity Scanner
                float detectionRadius = 120f;
                for (int i = 0; i < Main.maxNPCs; i++) {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage) {
                        if (Vector2.Distance(Projectile.Center, npc.Center) < detectionRadius) {
                            Projectile.Kill();
                            break;
                        }
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            // Instantly stop and plant flat upon hitting a wall or floor
            Projectile.velocity = Vector2.Zero;
            Projectile.rotation = 0f; // Snap flat to the surface
            State = STATE_ARMED;
            SoundEngine.PlaySound(SoundID.MaxMana, Projectile.Center);
            return false; 
        }

        public override void OnKill(int timeLeft) {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            // Visual explosion dust
            for (int i = 0; i < 20; i++) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), 100, default, 2f);
            }

            // Spawn 8 high-velocity bullets in a perfect circle
            if (Main.myPlayer == Projectile.owner) {
                int shrapnelCount = 8;
                float rotationInterval = MathHelper.TwoPi / shrapnelCount;
                
                for (int i = 0; i < shrapnelCount; i++) {
                    Vector2 shootVelocity = new Vector2(0, 12f).RotatedBy(rotationInterval * i);
                    
                    Projectile.NewProjectile(
                        Projectile.GetSource_Death(), 
                        Projectile.Center, 
                        shootVelocity, 
                        ProjectileID.BulletHighVelocity, 
                        Projectile.damage, 
                        Projectile.knockBack, 
                        Projectile.owner
                    );
                }
            }
        }
    }
}