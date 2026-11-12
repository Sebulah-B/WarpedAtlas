using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
    public class XiuhcoatlPyreJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.FragmentSolar;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 0;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.LightRed;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/ff6622:North Ultimate Socket - Blood of Xiuhcoatl]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Replaces Mana Sickness with Solar Immolation (+25% Magic Damage, ignites enemies, and burns Energy Shield before Life)."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_FlameSorcerer = true;
        }
    }
}