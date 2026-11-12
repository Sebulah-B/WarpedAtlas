using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Items.Currencies
{
    public class OrbOfAlteration : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.OrichalcumOre;
        
        public override void SetDefaults() {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.consumable = true; 
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "CurrencyDesc", "Reforges a Magic item with a new random property."));
            tooltips.Add(new TooltipLine(Mod, "CurrencyUse", "Right-Click this orb while holding a Magic (Blue) equipment piece."));
        }

        public override bool CanRightClick() {
            var globalItem = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            return AtlasGlobalItem.IsValidForAffixes(Main.mouseItem) && globalItem.CurrentTier == ItemTier.Magic;
        }

        public override void RightClick(Player player) {
            var globalItem = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            
            // Clear existing affixes
            globalItem.Prefixes.Clear();
            globalItem.Suffixes.Clear();
            
            // Roll exactly 1 Prefix OR 1 Suffix for Magic tier
            if (Main.rand.NextBool()) {
                var pool = AtlasGlobalItem.GetPool(Main.mouseItem, "Prefix");
                globalItem.Prefixes.Add(pool[Main.rand.Next(pool.Count)].CloneAndRoll());
            } else {
                var pool = AtlasGlobalItem.GetPool(Main.mouseItem, "Suffix");
                globalItem.Suffixes.Add(pool[Main.rand.Next(pool.Count)].CloneAndRoll());
            }
            
            SoundEngine.PlaySound(SoundID.Item37, player.Center);
            Main.NewText($"  Alteration successful! {Main.mouseItem.Name} has shifted.", 136, 136, 255);
        }
    }
}