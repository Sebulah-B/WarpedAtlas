using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Projectiles;
using WarpedAtlas.Content.Buffs;

namespace WarpedAtlas.Content.Items.Extraction
{
    public class TzitzimitlEclipseIdol : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.CelestialSigil;

        public override void SetDefaults() {
            Item.damage = 280;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 16;
            Item.width = 32; Item.height = 32;
            Item.useTime = 22; Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item119;
            Item.shoot = ModContent.ProjectileType<MartyrPayload>();
            Item.shootSpeed = 16f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Tier", "[c/ff5555:Tier VIII Extraction Exclusive Idol]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Unleashes twin Eclipse Martyr Payloads, sprouts a Sacred Life Fruit, and surges a Tlaloc Lightning Arc to your cursor."));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            int finalDmg = (int)(damage * prog.ToolkitDamageMult);
            prog.CurrentWard = Math.Min(prog.MaxWard, prog.CurrentWard + (prog.Toolkit_IdolWardLeech ? 75f : 40f));

            Projectile.NewProjectile(source, player.Center, velocity.RotatedBy(0.12f), type, finalDmg, knockback, player.whoAmI);
            Projectile.NewProjectile(source, player.Center, velocity.RotatedBy(-0.12f), type, finalDmg, knockback, player.whoAmI);
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<TlalocArcVisualProj>(), 0, 0f, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
            Projectile.NewProjectile(source, Main.MouseWorld, new Vector2(0, -4f), ModContent.ProjectileType<XochiquetzalFruitProj>(), 0, 0f, player.whoAmI);
            return false;
        }
    }
}