using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Items
{
    public class EndlessRocketPouch : ModItem
    {
        // Points directly to the vanilla Rocket I sprite instead of the Musket Pouch
        public override string Texture => "Terraria/Images/Item_" + ItemID.RocketI;

        public override void SetDefaults()
        {
            // Safely clone all native properties of the Rocket I ammo.
            // This guarantees we get the correct Projectile ID (fixing the Web Spit bug), 
            // Ammo ID, Damage, and Knockback without risking enum naming collisions.
            Item.CloneDefaults(ItemID.RocketI);

            // Overwrite specific fields to make it endless and fit for the endgame
            Item.maxStack = 1;
            Item.consumable = false; 
            
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Lime;
        }
    }
}