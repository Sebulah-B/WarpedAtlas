using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;
using WarpedAtlas.Content.Systems;
using WarpedAtlas.Content.Players;

namespace WarpedAtlas.Content.Items
{
    public class AscendancyRelic : ModItem
    {
        // Uses the vanilla Celestial Sigil sprite as an ornate PoE-style Trial Relic
        public override string Texture => "Terraria/Images/Item_" + ItemID.CelestialSigil;

        public override void SetDefaults() {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item119;
            Item.consumable = false; // Consumed explicitly upon confirming an Ascendancy in the UI
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "RelicDesc", "An ancient Trial Relic humming with Ascendant power.\nUse within the Sky Hub to choose your Ascendancy specialization.") {
                OverrideColor = new Color(120, 235, 255)
            });
        }

        public override bool? UseItem(Player player) {
            if (player.whoAmI == Main.myPlayer) {
                if (SubworldSystem.Current != null) {
                    Main.NewText("The Ascendancy Altar can only be communed with safely in the Sky Hub!", 230, 80, 80);
                    return false;
                }

                var prog = player.GetModPlayer<AtlasProgressionPlayer>();
                if (!string.IsNullOrEmpty(prog.ChosenAscendancy)) {
                    Main.NewText($"You have already ascended as a {prog.ChosenAscendancy}!", 255, 180, 80);
                    return false;
                }

                var uiSystem = ModContent.GetInstance<AtlasUISystem>();
                if (uiSystem != null) {
                    uiSystem.OpenAscendancySelectUI();
                }
                return true;
            }
            return true;
        }
    }
}