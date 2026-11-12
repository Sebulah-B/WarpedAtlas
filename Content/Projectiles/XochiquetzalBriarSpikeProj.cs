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
    public class XochiquetzalBriarSpikeProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.SharpTears;

        public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults() {
            Projectile.width = 32;
            Projectile.height = 72;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 36;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;
        }

        public override void AI() {
            Projectile.velocity *= 0.88f;

            // Progress smoothly through the 6 emergence/retraction frames
            float progress = 1f - (Projectile.timeLeft / 36f);
            Projectile.frame = Math.Clamp((int)(progress * 6f), 0, 5);

            if (Projectile.timeLeft == 34) {
                SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
                for (int i = 0; i < 12; i++) {
                    Dust d = Dust.NewDustDirect(Projectile.Bottom - new Vector2(14, 10), 28, 16, DustID.JungleSpore, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-6f, -1f), 80, Color.LimeGreen, 1.5f);
                    d.noGravity = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            target.AddBuff(BuffID.Poisoned, 240);
            target.AddBuff(BuffID.Venom, 120);
        }

        public override bool PreDraw(ref Color lightColor) {
            Main.instance.LoadProjectile(ProjectileID.SharpTears);
            Texture2D tex = TextureAssets.Projectile[ProjectileID.SharpTears].Value;
            Rectangle frame = tex.Frame(1, 6, 0, Projectile.frame);
            Vector2 origin = new Vector2(frame.Width * 0.5f, frame.Height * 0.85f);
            Vector2 drawPos = Projectile.Bottom - Main.screenPosition;

            float rot = Projectile.ai[0];
            Main.spriteBatch.Draw(tex, drawPos, frame, new Color(90, 255, 110), rot, origin, Projectile.scale * 1.15f, SpriteEffects.None, 0f);
            return false;
        }
    }
}