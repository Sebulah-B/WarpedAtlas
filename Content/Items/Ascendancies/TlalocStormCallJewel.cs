using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
    public class TlalocStormCallJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.NimbusRod;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 1;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.Cyan;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/00ffff:East Offense Socket - Wrath of Tlalocan]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "+15% Summon Damage & +12% Minion Attack Speed.\nMinion attacks call down vertical Sky-Thunderbolts from above the struck enemy."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetDamage(DamageClass.Summon) += 0.15f;
            player.GetAttackSpeed(DamageClass.Summon) += 0.12f;
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_LightningSummonerOffense = true;
        }
    }
}