using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
    public class XochiquetzalThornLashJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Stinger;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 1;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.Lime;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/77ff44:East Offense Socket - Briars of Xochiquetzal]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "+15% Summon Damage & +1 Max Minion.\nMinion and Whip strikes erupt 3 animated Blood-Thorn Briar Spikes from the ground beneath your target."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetDamage(DamageClass.Summon) += 0.15f;
            player.maxMinions += 1;
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_PlantSummonerOffense = true;
        }
    }
}