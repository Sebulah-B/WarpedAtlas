using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
    public class EhecatlGaleJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Emerald;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 0;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.Cyan;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/50ffaa:North Ultimate Socket - Breath of Ehecatl]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Press 'V' to channel a Sand Elemental Cyclone around your body for 45s.\nSuspends you in the eye of the storm, firing Weather Pain gales and shredding nearby foes."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.moveSpeed += 0.12f;
        }
    }
}