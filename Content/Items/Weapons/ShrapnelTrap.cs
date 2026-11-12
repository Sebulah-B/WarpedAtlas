using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Projectiles;

namespace WarpedAtlas.Content.Items.Weapons
{
    public class ShrapnelTrap : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LandMine;

        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 24;
            Item.damage = 45; 
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true; 
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            
            Item.shoot = ModContent.ProjectileType<ShrapnelTrapProjectile>();
            Item.shootSpeed = 10f; // Slightly increased toss velocity for a better arc
            
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Green;
        }
    }
}