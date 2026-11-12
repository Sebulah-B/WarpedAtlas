using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace WarpedAtlas.Content.Items.Currencies
{
    public class OrbOfAlchemy : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.StrangePlant1;
        
        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.consumable = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "CurrencyDesc", "Upgrades a normal item to Rare, infusing it with up to 6 properties."));
            tooltips.Add(new TooltipLine(Mod, "CurrencyUse", "Right-Click this orb while holding a normal equipment piece."));
        }

        public override bool CanRightClick() {
            var globalItem = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            return AtlasGlobalItem.IsValidForAffixes(Main.mouseItem) && globalItem.CurrentTier == ItemTier.Normal;
        }

        public override void RightClick(Player player) {
            var globalItem = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            
            // Promote tier
            globalItem.CurrentTier = ItemTier.Rare;
            
            var prePool = AtlasGlobalItem.GetPool(Main.mouseItem, "Prefix");
            var sufPool = AtlasGlobalItem.GetPool(Main.mouseItem, "Suffix");
            
            // Roll 1 to 3 Prefixes and Suffixes
            int preCount = Main.rand.Next(1, 4);
            int sufCount = Main.rand.Next(1, 4);
            
            for (int i = 0; i < preCount; i++) {
                var available = prePool.Where(p => !globalItem.Prefixes.Any(existing => existing.Name == p.Name)).ToList();
                if (available.Count > 0) globalItem.Prefixes.Add(available[Main.rand.Next(available.Count)].CloneAndRoll());
            }
            for (int i = 0; i < sufCount; i++) {
                var available = sufPool.Where(s => !globalItem.Suffixes.Any(existing => existing.Name == s.Name)).ToList();
                if (available.Count > 0) globalItem.Suffixes.Add(available[Main.rand.Next(available.Count)].CloneAndRoll());
            }
            
            SoundEngine.PlaySound(SoundID.Item37, player.Center);
            Main.NewText($"  Alchemy successful! {Main.mouseItem.Name} has been infused.", 255, 200, 100);
        }
    }
}