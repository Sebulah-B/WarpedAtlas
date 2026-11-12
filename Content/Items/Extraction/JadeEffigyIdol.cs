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
    public class JadeEffigyIdol : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LihzahrdPowerCell;

        public override void SetDefaults() {
            Item.damage = 135;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 12;
            Item.width = 28; Item.height = 28;
            Item.useTime = 25; Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.NPCDeath14;
            Item.shoot = ModContent.ProjectileType<MartyrPayload>();
            Item.shootSpeed = 14f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Tier", "[c/ff5555:Tier IV Extraction Exclusive Idol]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Launches a homing Martyr Payload and erupts 3 Blood-Thorn Briar Spikes at your cursor while restoring 25 Spirit Ward."));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            int finalDmg = (int)(damage * prog.ToolkitDamageMult);
            prog.CurrentWard = Math.Min(prog.MaxWard, prog.CurrentWard + (prog.Toolkit_IdolWardLeech ? 50f : 25f));

            Projectile.NewProjectile(source, player.Center, velocity, type, finalDmg, knockback, player.whoAmI);
            for (int i = -1; i <= 1; i++) {
                Projectile.NewProjectile(source, Main.MouseWorld + new Vector2(i * 28f, 20f), Vector2.Zero, ModContent.ProjectileType<XochiquetzalBriarSpikeProj>(), finalDmg, 3f, player.whoAmI, i * 0.15f);
            }
            return false;
        }
    }
}