using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{
    public class TezcatlipocaChainBoltJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MagnetSphere;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 1;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.Blue;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/44aaff:East Offense Socket - Obsidian Mirror Discharge]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "+14% Magic Damage & +10% Cast Speed.\nMagic hits discharge Chain Lightning that arcs from the struck target to up to 3 nearby enemies."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetDamage(DamageClass.Magic) += 0.14f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.10f;
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_LightningSorcererOffense = true;
        }
    }
}