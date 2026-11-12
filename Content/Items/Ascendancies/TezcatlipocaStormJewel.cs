using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
    public class TezcatlipocaStormJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Sapphire;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 0;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.Blue;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/44aaff:North Ultimate Socket - Tempest of Tezcatlipoca]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Magic Critical Strikes manifest Homing Living Lightning orbs that relentlessly electrocute nearby targets."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_LightningSorcerer = true;
            player.GetCritChance(DamageClass.Magic) += 8f;
        }
    }
}