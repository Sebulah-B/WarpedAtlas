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
    public class XochiquetzalFruitProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LifeFruit;

        public override void SetDefaults() {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.damage = 0;
        }

        public override void AI() {
            Projectile.velocity.Y = Math.Min(8f, Projectile.velocity.Y + 0.25f);
            Projectile.velocity.X *= 0.96f;
            Projectile.rotation = (float)Math.Sin(Main.GameUpdateCount * 0.08f) * 0.2f;

            Lighting.AddLight(Projectile.Center, 0.4f, 0.9f, 0.2f);
            if (Main.rand.NextBool(3)) {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.JungleSpore, 0f, -1f, 100, Color.LimeGreen, 1.2f);
                d.noGravity = true;
            }

            Player owner = Main.player[Projectile.owner];
            if (owner != null && owner.active && !owner.dead) {
                float dist = Vector2.Distance(owner.Center, Projectile.Center);
                if (dist < 130f) {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(owner.Center - Projectile.Center) * 9.5f, 0.15f);
                }
                if (dist < 34f && Projectile.timeLeft < 585) {
                    var prog = owner.GetModPlayer<AtlasProgressionPlayer>();
                    prog.CurrentWard = Math.Min(prog.MaxWard, prog.CurrentWard + 40f);
                    owner.AddBuff(ModContent.BuffType<XochiquetzalBloomBuff>(), 8 * 60);
                    CombatText.NewText(owner.getRect(), Color.LimeGreen, "+40 Ward (Xochiquetzal's Bloom!)");
                    SoundEngine.PlaySound(SoundID.Item4, owner.Center);
                    Projectile.Kill();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (Projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 2f) {
                Projectile.velocity.Y = -oldVelocity.Y * 0.45f;
            }
            return false;
        }
    }
}