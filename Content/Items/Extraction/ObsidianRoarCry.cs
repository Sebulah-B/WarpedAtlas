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
    public class ObsidianRoarCry : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.PanicNecklace;

        public override void SetDefaults() {
            Item.damage = 110;
            Item.DamageType = DamageClass.Melee;
            Item.width = 32; Item.height = 32;
            Item.useTime = 35; Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 9f;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Roar;
            Item.shoot = ModContent.ProjectileType<SeismicShockwaveProjectile>();
            Item.shootSpeed = 0f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Tier", "[c/ff5555:Tier IV Extraction Exclusive Warcry]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Unleashes a twin Obsidian Warcry shockwave and launches 4 homing Tempest Gales.\nGrants Seismic Might and +15% Endurance. Scales with the Tecciztli Warcaller passive branch."));
        }

        public override bool CanUseItem(Player player) => !player.HasBuff(ModContent.BuffType<WarcryCooldownDebuff>());

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            int cd = prog.Toolkit_WarcryCDR ? 240 : 390;
            player.AddBuff(ModContent.BuffType<WarcryCooldownDebuff>(), cd);
            player.AddBuff(ModContent.BuffType<SeismicMightBuff>(), 600);

            Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, (int)(damage * prog.ToolkitDamageMult), knockback, player.whoAmI);
            for (int i = -1; i <= 1; i += 2) {
                Projectile.NewProjectile(source, player.Center, new Vector2(i * 8f, 0f), ModContent.ProjectileType<EhecatlTempestCycloneProj>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}