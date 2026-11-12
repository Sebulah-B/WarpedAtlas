using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
    public class XiuhtecuhtliMagmaArrowJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.HellfireArrow;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 1;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.Red;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/ff5533:East Offense Socket - Eruption of Xiuhtecuhtli]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "+15% Ranged Damage & +10 Armor Penetration.\nEvery 3rd Ranged hit erupts a fiery Volcanic Geyser & Solar Explosion beneath the target."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetDamage(DamageClass.Ranged) += 0.15f;
            player.GetArmorPenetration(DamageClass.Generic) += 10f;
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_FireRangerOffense = true;
        }
    }
}