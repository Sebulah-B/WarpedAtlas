using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
    public class XochiquetzalBloomJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LifeFruit;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 0;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.Lime;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/77ff44:North Ultimate Socket - Garden of Xochiquetzal]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "Slaying enemies or striking bosses has a chance to sprout Sacred Life Fruit.\nPicking up a fruit restores 40 Spirit Ward and empowers Minions with +35% Damage & +25% Speed."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_PlantSummoner = true;
        }
    }
}