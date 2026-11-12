using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;
using WarpedAtlas.Content.Subworlds;
using WarpedAtlas.Content.Systems;

namespace WarpedAtlas.Content.Items
{
    public class ApexCatalystKey : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.TempleKey;

        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player) {
            if (player.whoAmI == Main.myPlayer) {
                if (SubworldSystem.Current != null) {
                    Main.NewText("  The Catalyst can only be used safely from the Sky Hub!", 230, 80, 80);
                    return false;
                }

                if (BaseDungeonSubworld.ActiveProfile == null) {
                    var uiSystem = ModContent.GetInstance<AtlasUISystem>();
                    if (uiSystem != null && uiSystem.AtlasMapInterface.CurrentState == null) {
                        uiSystem.AtlasPassiveInterface.SetState(null); 
                        uiSystem.AtlasMapInterface.SetState(uiSystem.MapUIState);
                        Main.playerInventory = true; 
                        Main.NewText("  Select an Apex Domain coordinate...", 255, 215, 0);
                    }
                    return false;
                }

                if (Main.netMode != NetmodeID.Server) {
                    var uiSystem = ModContent.GetInstance<AtlasUISystem>();
                    if (uiSystem != null) uiSystem.AtlasMapInterface.SetState(null);
                }

                Item.stack--;
                if (Item.stack <= 0) Item.TurnToAir(); 

                Main.NewText($"  Catalyst accepted. Opening the Apex Domain for {BaseDungeonSubworld.ActiveProfile.DisplayName}...", 255, 100, 100);
                SubworldSystem.Enter<BossDomainSubworld>();
                return true;
            }
            return base.UseItem(player);
        }
    }
}