using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace WarpedAtlas.Content.Subworlds
{
    public static class MapPrefab
    {
        // Removed all solid block barriers. 
        // This leaves the center completely empty for vertical ropes and horizontal corridors[cite: 1].
        public static readonly int[,] BalconyChamber = new int[,] {
            { 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };

        public static readonly int[,] PillaredVault = new int[,] {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };

        public static void StampPrefab(Rectangle roomBounds, int[,] matrix, ushort tileType, ushort wallType) {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int startX = roomBounds.X + (roomBounds.Width - cols) / 2;
            int startY = roomBounds.Bottom - rows; 

            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < cols; c++) {
                    int worldX = startX + c;
                    int worldY = startY + r;
                    if (worldX < 0 || worldX >= Main.maxTilesX || worldY < 0 || worldY >= Main.maxTilesY) continue;
                    
                    Tile tile = Main.tile[worldX, worldY];
                    
                    if (tile.HasTile && (tile.TileType == TileID.Platforms || tile.TileType == TileID.Rope)) {
                        continue;
                    }

                    int cell = matrix[r, c];
                    if (cell == 2) {
                        tile.HasTile = true;
                        tile.TileType = TileID.Platforms;
                    }
                }
            }
        }
    }
}