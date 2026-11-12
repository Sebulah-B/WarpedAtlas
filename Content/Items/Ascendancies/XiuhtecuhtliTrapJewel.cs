using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
    public class XiuhtecuhtliTrapJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Ruby;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 0;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.Red;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/ff5533:North Ultimate Socket - Xiuhtecuhtli's Crucible]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Shrapnel Traps detonate a second time in a solar blast and scatter 4 secondary cluster explosions."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_FireTrapper = true;
        }
    }
}