using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
    public class OllinChronoJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Amber;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 0;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.Yellow;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/ffdd44:North Ultimate Socket - Pulse of Ollin]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Ranged weapon attacks fracture time, firing an additional echoed projectile in a tight volley."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_TimeRanger = true;
        }
    }   
}