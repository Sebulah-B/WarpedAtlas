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
    public class EhecatlTempestCycloneProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.DD2ApprenticeStorm;

        public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults() {
            Projectile.width = 68;
            Projectile.height = 110;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 135;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }

        public override void AI() {
            if (++Projectile.frameCounter >= 3) {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 6;
            }

            // Pull non-boss enemies slightly into the traveling cyclone
            for (int i = 0; i < Main.maxNPCs; i++) {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && !npc.boss && npc.lifeMax > 5 && !npc.dontTakeDamage && npc.knockBackResist > 0f) {
                    if (Vector2.Distance(npc.Center, Projectile.Center) < 120f) {
                        npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Normalize(Projectile.Center - npc.Center) * 5f, 0.15f);
                    }
                }
            }

            // Swirl Sand Elemental & Wind particles
            for (int i = 0; i < 2; i++) {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Sandnado, Projectile.velocity.X * 0.4f, -2f, 100, new Color(150, 255, 220), 1.3f);
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Main.instance.LoadProjectile(ProjectileID.DD2ApprenticeStorm);
            Main.instance.LoadProjectile(ProjectileID.SandnadoFriendly);

            Texture2D stormTex = TextureAssets.Projectile[ProjectileID.DD2ApprenticeStorm].Value;
            Texture2D sandTex = TextureAssets.Projectile[ProjectileID.SandnadoFriendly].Value;

            Rectangle stormFrame = stormTex.Frame(1, 6, 0, Projectile.frame);
            Rectangle sandFrame = sandTex.Frame(1, 6, 0, Projectile.frame);

            float fade = MathHelper.Clamp(Projectile.timeLeft / 25f, 0f, 1f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            // Layer 1: Sand Elemental Golden-Emerald Funnel
            Main.spriteBatch.Draw(sandTex, drawPos, sandFrame, new Color(120, 255, 190, 140) * fade, 0f, sandFrame.Size() * 0.5f, Projectile.scale * 1.05f, SpriteEffects.None, 0f);
            // Layer 2: Tome of Infinite Wisdom Tempest Core
            Main.spriteBatch.Draw(stormTex, drawPos, stormFrame, Color.White * fade, 0f, stormFrame.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}