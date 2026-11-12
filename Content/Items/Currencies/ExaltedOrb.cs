using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace WarpedAtlas.Content.Items.Currencies
{
    public class ExaltedOrb : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LunarBar;
        public override void SetDefaults() {
            Item.width = 24; Item.height = 24; Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 10, 0, 0); Item.rare = ItemRarityID.Red; Item.consumable = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "CurrencyDesc", "Adds one new random property to a Rare item."));
            tooltips.Add(new TooltipLine(Mod, "CurrencyUse", "Right-Click this orb while holding a Rare (Yellow) equipment piece with open property slots."));
        }

        public override bool CanRightClick() {
            var global = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            if (!AtlasGlobalItem.IsValidForAffixes(Main.mouseItem) || global.CurrentTier != ItemTier.Rare) return false;
            
            var prePool = AtlasGlobalItem.GetPool(Main.mouseItem, "Prefix");
            var sufPool = AtlasGlobalItem.GetPool(Main.mouseItem, "Suffix");
            
            // Only allow usage if there is at least one unused affix left in a pool that hasn't hit the slot cap
            bool hasOpenPrefix = global.Prefixes.Count < 3 && prePool.Any(p => !global.Prefixes.Any(e => e.Name == p.Name));
            bool hasOpenSuffix = global.Suffixes.Count < 3 && sufPool.Any(s => !global.Suffixes.Any(e => e.Name == s.Name));
            
            return hasOpenPrefix || hasOpenSuffix;
        }

        public override void RightClick(Player player) {
            var global = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            
            var prePool = AtlasGlobalItem.GetPool(Main.mouseItem, "Prefix");
            var sufPool = AtlasGlobalItem.GetPool(Main.mouseItem, "Suffix");
            
            var availablePrefixes = prePool.Where(p => !global.Prefixes.Any(e => e.Name == p.Name)).ToList();
            var availableSuffixes = sufPool.Where(s => !global.Suffixes.Any(e => e.Name == s.Name)).ToList();
            
            bool canAddPrefix = global.Prefixes.Count < 3 && availablePrefixes.Count > 0;
            bool canAddSuffix = global.Suffixes.Count < 3 && availableSuffixes.Count > 0;
            
            // Pick randomly, but force the other option if one pool is completely exhausted
            bool addPrefix = canAddPrefix && (!canAddSuffix || Main.rand.NextBool());
            
            if (addPrefix) {
                global.Prefixes.Add(availablePrefixes[Main.rand.Next(availablePrefixes.Count)].CloneAndRoll());
            } else if (canAddSuffix) {
                global.Suffixes.Add(availableSuffixes[Main.rand.Next(availableSuffixes.Count)].CloneAndRoll());
            }
            
            SoundEngine.PlaySound(SoundID.Item37, player.Center);
            Main.NewText($"  Exalted! A new property has manifested on {Main.mouseItem.Name}.", 255, 215, 0);
        }
    }
}