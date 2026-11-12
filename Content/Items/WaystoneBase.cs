using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;
using WarpedAtlas.Content.Subworlds;
using WarpedAtlas.Content.Systems;

namespace WarpedAtlas.Content.Items
{
	public class WaystoneBase : ModItem
	{
		public override string Texture => "Terraria/Images/Item_3828";

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = false;
		}

		public override bool CanRightClick() {
			return false;
		}

		public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				
				// 1. Safety Block: Do not allow mapping loops if already inside a combat sector
				if (SubworldSystem.Current != null) {
					Main.NewText("❌ Waystones can only resonate within the safety of the Sky Hub town!", 230, 80, 80);
					return false;
				}

				// 2. SMART INTERCEPT: Instead of spamming chat error indicators, directly manifest the Atlas UI map screen!
				if (BaseDungeonSubworld.ActiveProfile == null) {
					var uiSystem = ModContent.GetInstance<AtlasUISystem>();
					if (uiSystem != null && uiSystem.AtlasMapInterface.CurrentState == null) {
						// Open up the map layout panel instantly
						uiSystem.AtlasPassiveInterface.SetState(null); 
						uiSystem.AtlasMapInterface.SetState(uiSystem.MapUIState);
						Main.playerInventory = true; // Open their inventory frames to visually match UI interactions
						
						Main.NewText("🌀 Map coordinates unselected. Initializing Atlas Navigation System...", 0, 191, 255);
					}
					return false; // Intercepts item usage so the item stack is NOT consumed!
				}

				// 3. Clear Check Passed -> Close the UI immediately before starting subworld load threads
				if (Main.netMode != NetmodeID.Server) {
					var uiSystem = ModContent.GetInstance<Systems.AtlasUISystem>();
					if (uiSystem != null) {
						uiSystem.AtlasMapInterface.SetState(null); // Force closes the map window
					}
				}

				// 4. Consume 1 item out of the active bag slot stack layout
				Item.stack--;
				if (Item.stack <= 0) {
					Item.TurnToAir();
				}

				// 5. Fire the warp gate parameters
				Main.NewText($"🌀 Waystone consumed! Ripping open a spatial rift to {BaseDungeonSubworld.ActiveProfile.DisplayName}...", 0, 191, 255);
				SubworldSystem.Enter<BaseDungeonSubworld>();
				return true;
			}
			return base.UseItem(player);
		}
	}
}