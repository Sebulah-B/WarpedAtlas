using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
    public class OllinChronoBurstJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.FastClock;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 1;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.Yellow;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/ffdd44:East Offense Socket - Velocity of the Fifth Sun]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "+12% Ranged Crit Chance.\nScoring a Ranged Critical Strike grants Chrono-Surge (+30% Ranged Attack Speed for 3s) and fires a high-velocity Temporal Piercer through the target."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetCritChance(DamageClass.Ranged) += 12f;
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_TimeRangerOffense = true;
        }
    }
}