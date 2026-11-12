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
    public class CataclysmBellowCry : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.SweetheartNecklace;

        public override void SetDefaults() {
            Item.damage = 240;
            Item.DamageType = DamageClass.Melee;
            Item.width = 32; Item.height = 32;
            Item.useTime = 30; Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 12f;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Roar;
            Item.shoot = ModContent.ProjectileType<SeismicShockwaveProjectile>();
            Item.shootSpeed = 0f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Tier", "[c/ff5555:Tier VIII Extraction Exclusive Warcry]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Shatters the screen with a Cataclysmic Bellow, erupting 8 Solar Explosions around you and maxing out Seismic Might stacks."));
        }

        public override bool CanUseItem(Player player) => !player.HasBuff(ModContent.BuffType<WarcryCooldownDebuff>());

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            int cd = prog.Toolkit_WarcryCDR ? 210 : 360;
            player.AddBuff(ModContent.BuffType<WarcryCooldownDebuff>(), cd);
            player.AddBuff(ModContent.BuffType<SeismicMightBuff>(), 720);
            prog.WarcryPowerStacks = 10;

            int finalDmg = (int)(damage * prog.ToolkitDamageMult);
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, finalDmg, knockback, player.whoAmI);
            for (int i = 0; i < 6; i++) {
                Vector2 offset = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 6f * i) * 140f;
                Projectile.NewProjectile(source, player.Center + offset, Vector2.Zero, ProjectileID.SolarWhipSwordExplosion, finalDmg, 8f, player.whoAmI, 0f, 1.2f);
            }
            return false;
        }
    }
}