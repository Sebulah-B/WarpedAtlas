using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
    public class XiuhcoatlCinderBrandJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.HelFire;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 1;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.LightRed;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/ff6622:East Offense Socket - Fangs of Xiuhcoatl]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "+18% Magic Damage.\nStriking a burning enemy with Magic weapons triggers a violent Solar Combustion blast on the target."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetDamage(DamageClass.Magic) += 0.18f;
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_FlameSorcererOffense = true;
        }
    }
}