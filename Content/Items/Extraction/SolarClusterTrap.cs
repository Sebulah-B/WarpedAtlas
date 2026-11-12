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
    public class SolarClusterTrap : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LandMine;

        public override void SetDefaults() {
            Item.damage = 95;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 28; Item.height = 28;
            Item.useTime = 24; Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<ShrapnelTrapProjectile>();
            Item.shootSpeed = 9.5f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Tier", "[c/ff5555:Tier IV Extraction Exclusive Trap]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Deploys a twin spread of high-yield Solar Shrapnel Traps that arm on contact."));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            int finalDmg = (int)(damage * prog.ToolkitDamageMult);
            int count = prog.Toolkit_ExtraTraps ? 4 : 2;
            for (int i = 0; i < count; i++) {
                float angle = (i - (count - 1) * 0.5f) * 0.16f;
                Projectile.NewProjectile(source, position, velocity.RotatedBy(angle), type, finalDmg, knockback, player.whoAmI);
            }
            return false;
        }
    }
}