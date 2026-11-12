using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace WarpedAtlas.Content.Items.Currencies
{
    public class RegalOrb : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.FragmentNebula;
        
        public override void SetDefaults() {
            Item.width = 22;
            Item.height = 22;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.consumable = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "CurrencyDesc", "Upgrades a Magic item to a Rare item, adding one new property."));
            tooltips.Add(new TooltipLine(Mod, "CurrencyUse", "Right-Click this orb while holding a Magic (Blue) equipment piece."));
        }

        public override bool CanRightClick() {
            var globalItem = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            return AtlasGlobalItem.IsValidForAffixes(Main.mouseItem) && globalItem.CurrentTier == ItemTier.Magic;
        }

        public override void RightClick(Player player) {
            var globalItem = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            globalItem.CurrentTier = ItemTier.Rare;
            
            var prePool = AtlasGlobalItem.GetPool(Main.mouseItem, "Prefix");
            var sufPool = AtlasGlobalItem.GetPool(Main.mouseItem, "Suffix");
            
            var availablePrefixes = prePool.Where(p => !globalItem.Prefixes.Any(e => e.Name == p.Name)).ToList();
            var availableSuffixes = sufPool.Where(s => !globalItem.Suffixes.Any(e => e.Name == s.Name)).ToList();
            
            bool canAddPrefix = globalItem.Prefixes.Count < 3 && availablePrefixes.Count > 0;
            bool canAddSuffix = globalItem.Suffixes.Count < 3 && availableSuffixes.Count > 0;
            
            bool addPrefix = canAddPrefix && (!canAddSuffix || Main.rand.NextBool());
            
            if (addPrefix) {
                globalItem.Prefixes.Add(availablePrefixes[Main.rand.Next(availablePrefixes.Count)].CloneAndRoll());
            } else if (canAddSuffix) {
                globalItem.Suffixes.Add(availableSuffixes[Main.rand.Next(availableSuffixes.Count)].CloneAndRoll());
            }
            
            SoundEngine.PlaySound(SoundID.Item37, player.Center);
            Main.NewText($"  Regal blessing applied! {Main.mouseItem.Name} has been upgraded.", 255, 255, 102);
        }
    }
}