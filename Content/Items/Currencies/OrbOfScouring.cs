using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Items.Currencies
{
    public class OrbOfScouring : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Bone;
        public override void SetDefaults() {
            Item.width = 24; Item.height = 24; Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 1, 0, 0); Item.rare = ItemRarityID.White; Item.consumable = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "CurrencyDesc", "Removes all properties from an item, returning it to Normal tier."));
            tooltips.Add(new TooltipLine(Mod, "CurrencyUse", "Right-Click this orb while holding an altered equipment piece."));
        }

        public override bool CanRightClick() {
            var global = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            return AtlasGlobalItem.IsValidForAffixes(Main.mouseItem) && global.CurrentTier != ItemTier.Normal;
        }

        public override void RightClick(Player player) {
            var global = Main.mouseItem.GetGlobalItem<AtlasGlobalItem>();
            global.Prefixes.Clear();
            global.Suffixes.Clear();
            global.CurrentTier = ItemTier.Normal;

            SoundEngine.PlaySound(SoundID.Item37, player.Center);
            Main.NewText($"  Scoured clean! {Main.mouseItem.Name} has returned to normal.", 200, 200, 200);
        }
    }
}