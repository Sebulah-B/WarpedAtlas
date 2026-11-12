using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
   public class TlalocConduitJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Amethyst;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 0;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.Cyan;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/00ffff:North Ultimate Socket - Conduit of Tlaloc]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Grants Minions +15% Crit Chance. When a Minion scores a Critical Strike, a high-voltage Lightning Arc surges between you and the Minion, frying all enemies in the path."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_LightningSummoner = true;
            player.GetCritChance(DamageClass.Summon) += 15f;
        }
    } 
}