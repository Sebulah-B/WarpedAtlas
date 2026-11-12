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
    public class AethericRiftConduit : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.NebulaArcanum;

        public override void SetDefaults() {
            Item.damage = 85;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 18;
            Item.width = 30; Item.height = 30;
            Item.useTime = 26; Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item117;
            Item.shoot = ModContent.ProjectileType<LivingLightningOrbProj>();
            Item.shootSpeed = 10f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Tier", "[c/ff5555:Tier IV Extraction Exclusive Conduit]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Tears open a spatial rift at your cursor, manifesting 2 Homing Living Lightning Orbs and restoring 15 Energy Shield."));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            int finalDmg = (int)(damage * prog.ToolkitDamageMult);
            prog.CurrentEnergyShield = Math.Min(prog.MaxEnergyShield, prog.CurrentEnergyShield + (prog.Toolkit_ConduitOvercharge ? 30f : 15f));

            int count = prog.Toolkit_ConduitOvercharge ? 3 : 2;
            for (int i = 0; i < count; i++) {
                Vector2 spawnVel = Main.rand.NextVector2CircularEdge(8f, 8f);
                Projectile.NewProjectile(source, Main.MouseWorld, spawnVel, type, finalDmg, knockback, player.whoAmI);
            }
            return false;
        }
    }
}