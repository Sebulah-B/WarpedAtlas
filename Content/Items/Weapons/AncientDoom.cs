using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Projectiles;

namespace WarpedAtlas.Content.Items.Weapons
{
    public class AncientDoom : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.SpellTome;
        
        public override void SetDefaults() {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 300; 
            Item.DamageType = DamageClass.Magic;
            Item.mana = 40;
            
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.autoReuse = true;
            
            Item.shoot = ModContent.ProjectileType<AncientDoomProjectile>();
            Item.shootSpeed = 0f; // Spawns stationary at the cursor
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            position = Main.MouseWorld; // Forces the cast to appear exactly at the cursor
        }
    }
}