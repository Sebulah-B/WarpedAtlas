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
    public class ChronoSingularityTrap : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.ProximityMineLauncher;

        public override void SetDefaults() {
            Item.damage = 210;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32; Item.height = 32;
            Item.useTime = 20; Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<ShrapnelTrapProjectile>();
            Item.shootSpeed = 11f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Tier", "[c/ff5555:Tier VIII Extraction Exclusive Trap]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Launches 3 Chrono-Singularity Mines accompanied by a Vortex Tempest that pulls enemies onto the mines."));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            int finalDmg = (int)(damage * prog.ToolkitDamageMult);
            int count = prog.Toolkit_ExtraTraps ? 5 : 3;
            for (int i = 0; i < count; i++) {
                float angle = (i - (count - 1) * 0.5f) * 0.14f;
                Projectile.NewProjectile(source, position, velocity.RotatedBy(angle), type, finalDmg, knockback, player.whoAmI);
            }
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<EhecatlTempestCycloneProj>(), finalDmg / 2, 2f, player.whoAmI);
            return false;
        }
    }
}