using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SubworldLibrary;
using WarpedAtlas.Content.Subworlds;

namespace WarpedAtlas.Content.Systems
{
    public class AtlasLootFilter : GlobalItem
    {
        // The definitive list of vanilla items permitted to drop from monsters in the Atlas
        public static HashSet<int> ApprovedVanillaDrops = new HashSet<int>() {
            ItemID.Heart, ItemID.CandyApple, ItemID.CandyCane,
            ItemID.Star, ItemID.SugarPlum, ItemID.SoulCake,
            ItemID.CopperCoin, ItemID.SilverCoin, ItemID.GoldCoin, ItemID.PlatinumCoin,
            ItemID.LesserHealingPotion, ItemID.HealingPotion, ItemID.GreaterHealingPotion, ItemID.SuperHealingPotion,
            ItemID.LesserManaPotion, ItemID.ManaPotion, ItemID.GreaterManaPotion, ItemID.SuperManaPotion
        };

        public override void OnSpawn(Item item, IEntitySource source)
        {
            // Only enforce this strict economy inside our custom subworlds
            if (SubworldSystem.IsActive<BaseDungeonSubworld>() || SubworldSystem.IsActive<BossDomainSubworld>())
            {
                // Only intercept items dropping directly from monster kills.
                // This ensures we DO NOT delete items players drop from their own inventory (EntitySource_ItemDrop),
                // items from shattered Vault Chests (EntitySource_TileBreak), or Altar rewards (EntitySource_TileInteraction).
                if (source is EntitySource_Loot)
                {
                    // 1. Always allow our custom mod items (Orbs, Waystones, Uniques, Catalyst Keys)
                    if (item.ModItem != null && item.ModItem.Mod.Name == "WarpedAtlas")
                        return;

                    // 2. Always allow authorized vanilla ARPG essentials (Hearts, Mana, Coins, Potions)
                    if (ApprovedVanillaDrops.Contains(item.type))
                        return;

                    // 3. Vaporize everything else (Power Cells, Solar Tablets, Bones, Banners, Lenses, Gel, etc.)
                    item.active = false;
                    item.type = ItemID.None;
                }
            }
        }
    }
}