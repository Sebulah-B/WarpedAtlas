using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items; 

namespace WarpedAtlas.Content.Items.Ascendancies
{       
 public class EhecatlTempestBladeJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.BookStaff;
        public override bool CanSocketInSlot(int slotIndex) => slotIndex == 1;

        public override void SetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.rare = ItemRarityID.Cyan;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Slot", "[c/50ffaa:East Offense Socket - Gale of the Feathered Serpent]"));
            tooltips.Add(new TooltipLine(Mod, "Desc", "+15% Melee Damage & +12% Melee Attack Speed.\nSwinging a Melee weapon unleashes a full Tome of Infinite Wisdom Cyclone & Sand Elemental Gale that carves through enemies."));
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            player.GetDamage(DamageClass.Melee) += 0.15f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
            player.GetModPlayer<AtlasProgressionPlayer>().Asc_WindOffense = true;
        }
    }   
}