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
    public class OllinSingularityConduit : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LunarFlareBook;

        public override void SetDefaults() {
            Item.damage = 195;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 24;
            Item.width = 30; Item.height = 30;
            Item.useTime = 24; Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item117;
            Item.shoot = ModContent.ProjectileType<LivingLightningOrbProj>();
            Item.shootSpeed = 12f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Tier", "[c/ff5555:Tier VIII Extraction Exclusive Conduit]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", " collapses a Solar-Storm Singularity at your cursor, detonating a Solar Blast and releasing 4 Living Lightning Orbs."));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            int finalDmg = (int)(damage * prog.ToolkitDamageMult);
            prog.CurrentEnergyShield = Math.Min(prog.MaxEnergyShield, prog.CurrentEnergyShield + (prog.Toolkit_ConduitOvercharge ? 45f : 25f));

            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ProjectileID.SolarWhipSwordExplosion, finalDmg, 6f, player.whoAmI, 0f, 1.3f);
            int count = prog.Toolkit_ConduitOvercharge ? 5 : 3;
            for (int i = 0; i < count; i++) {
                Vector2 spawnVel = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / count * i) * 9f;
                Projectile.NewProjectile(source, Main.MouseWorld, spawnVel, type, finalDmg, knockback, player.whoAmI);
            }
            return false;
        }
    }
}