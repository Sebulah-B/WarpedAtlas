using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SubworldLibrary;

namespace WarpedAtlas.Content.Tiles
{
	public class ExtractionPortalTile : ModTile
	{
		
		public override string Texture => "Terraria/Images/Tiles_" + TileID.ElderCrystalStand;

		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;

			// Maintain a clean 3x4 layout matching the exact scale of the broken altar
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.Origin = new Point16(1, 3);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
			TileObjectData.addTile(Type);

			// Adds a distinct cyan marker indicator on the player's minimap screen
			AddMapEntry(new Color(0, 191, 255));
		}

		public override bool RightClick(int i, int j) {
			Main.NewText("Stepping through the rift back to the Atlas Hub.", 0, 191, 255);
			SubworldSystem.Exit();
			return true;
		}

		public override void MouseOver(int i, int j) {
			Player player = Main.LocalPlayer;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ItemID.CellPhone;
			player.noThrow = 2;
		}
	}
}