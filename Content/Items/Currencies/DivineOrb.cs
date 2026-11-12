using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Items.Currencies
{
    public class DivineOrb : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Ectoplasm;
        public override void SetDefaults() {
            Item.width = 24; Item.height = 24; Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 5, 0, 0); Item.rare = ItemRarityID.Yellow; Item.consumable = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "CurrencyDesc", "Randomizes the numerical values of all properties on an item."));
            tooltips.Add(new TooltipLine(Mod, "CurrencyUse", "Right-Click this orb while holding a Magic or Rare equipment piece."));
        }

        public override bool CanRightClick() {
            var global = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            return AtlasGlobalItem.IsValidForAffixes(Main.mouseItem) && (global.Prefixes.Count > 0 || global.Suffixes.Count > 0);
        }

        public override void RightClick(Player player) {
            var global = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            
            foreach (var prefix in global.Prefixes) prefix.RollValues();
            foreach (var suffix in global.Suffixes) suffix.RollValues();

            SoundEngine.PlaySound(SoundID.Item37, player.Center);
            Main.NewText($"  Divine intervention! The properties of {Main.mouseItem.Name} have been randomized.", 255, 255, 255);
        }
    }
}