using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Items.Currencies
{
    public class OrbOfAnnulment : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.SoulofLight;
        public override void SetDefaults() {
            Item.width = 24; Item.height = 24; Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 3, 0, 0); Item.rare = ItemRarityID.Pink; Item.consumable = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "CurrencyDesc", "Fractures and removes one random property from an item."));
            tooltips.Add(new TooltipLine(Mod, "CurrencyUse", "Right-Click this orb while holding a Magic or Rare equipment piece."));
        }

        public override bool CanRightClick() {
            var global = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            return AtlasGlobalItem.IsValidForAffixes(Main.mouseItem) && (global.Prefixes.Count > 0 || global.Suffixes.Count > 0);
        }

        public override void RightClick(Player player) {
            var global = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            int totalAffixes = global.Prefixes.Count + global.Suffixes.Count;
            int target = Main.rand.Next(totalAffixes);

            if (target < global.Prefixes.Count) global.Prefixes.RemoveAt(target);
            else global.Suffixes.RemoveAt(target - global.Prefixes.Count);

            // If it removes the last stat, downgrade the tier natively
            if (global.Prefixes.Count == 0 && global.Suffixes.Count == 0) global.CurrentTier = ItemTier.Normal;

            SoundEngine.PlaySound(SoundID.Item37, player.Center);
            Main.NewText($"  Annulled. A property was fractured from {Main.mouseItem.Name}.", 180, 180, 180);
        }
    }
}