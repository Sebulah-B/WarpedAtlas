using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using System.Collections.Generic;

namespace WarpedAtlas.Content.Systems
{
    public class HubSpawnerSystem : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            tasks.Clear();
            
            tasks.Add(new PassLegacy("Generating Ilhuicatl-Tonatiuh", (progress, configuration) => {
                progress.Message = "Constructing the House of the Sun...";

                int centerX = Main.maxTilesX / 2;
                int centerY = Main.maxTilesY / 2;

                // 1. Safe Clear 
                for (int x = 0; x < Main.maxTilesX; x++) {
                    for (int y = 0; y < Main.maxTilesY; y++) {
                        Tile tile = Main.tile[x, y];
                        tile.HasTile = false;
                        tile.WallType = 0;
                    }
                }

                // 2. New Compact, Climbable Proportions
                int baseRadius = 60; // 120 blocks wide total
                int steps = 5;
                int tierHeight = 4; // Exactly 4 blocks high (easily jumpable)
                int tierWidth = 10;
                
                // Establish the bottom floor of the top-side Ziggurat
                int bottomY = centerY + 10; 
                
                // 3. Build the Steps (Stacking from Bottom to Top)
                for (int step = 0; step < steps; step++) {
                    int currentRadius = baseRadius - (step * tierWidth);
                    int topY = bottomY - ((step + 1) * tierHeight);
                    int currentBottomY = bottomY - (step * tierHeight);

                    for (int x = centerX - currentRadius; x <= centerX + currentRadius; x++) {
                        for (int y = topY; y < currentBottomY; y++) {
                            
                            // Strict bounds check to prevent array crashes
                            if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY) continue;

                            Tile tile = Main.tile[x, y];
                            tile.HasTile = true;

                            // Aesthetic decoration logic
                            if (y == topY || x == centerX - currentRadius || x == centerX + currentRadius) {
                                tile.TileType = TileID.GoldBrick;
                            } else if (x % 5 == 0) {
                                tile.TileType = TileID.ObsidianBrick;
                            } else {
                                tile.TileType = TileID.LihzahrdBrick;
                            }
                        }
                    }
                }

                // 4. Build a tapered underbelly so it looks like a floating island
                for (int y = bottomY; y < bottomY + 12; y++) {
                    int depth = y - bottomY;
                    int underbellyRadius = baseRadius - (depth * 5);
                    
                    for (int x = centerX - underbellyRadius; x <= centerX + underbellyRadius; x++) {
                        if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY) continue;
                        
                        Tile tile = Main.tile[x, y];
                        tile.HasTile = true;
                        tile.TileType = TileID.LihzahrdBrick;
                    }
                }

                int plazaY = bottomY - (steps * tierHeight);

                // 5. Place Defender's Forge (Stash)
                WorldGen.PlaceObject(centerX + 12, plazaY - 1, TileID.DefendersForge, mute: true);

                // 6. Place Target Dummies and ACTIVATE their Tile Entities
                int[] dummyXPositions = { centerX - 18, centerX - 24 };
                foreach (int dx in dummyXPositions) {
                    WorldGen.PlaceObject(dx, plazaY - 1, TileID.TargetDummy, mute: true);
                    
                    // Wake up the dummy by finding its top-left frame and planting the TileEntity
                    for (int i = dx - 2; i <= dx + 2; i++) {
                        for (int j = plazaY - 4; j <= plazaY; j++) {
                            if (i >= 0 && i < Main.maxTilesX && j >= 0 && j < Main.maxTilesY) {
                                Tile t = Main.tile[i, j];
                                if (t.HasTile && t.TileType == TileID.TargetDummy && t.TileFrameX == 0 && t.TileFrameY == 0) {
                                    Terraria.GameContent.Tile_Entities.TETrainingDummy.Place(i, j);
                                }
                            }
                        }
                    }
                }

                // 7. Safe Texture Framing 
                for (int x = centerX - baseRadius - 5; x <= centerX + baseRadius + 5; x++) {
                    for (int y = plazaY - 5; y < bottomY + 15; y++) {
                        if (x >= 0 && x < Main.maxTilesX && y >= 0 && y < Main.maxTilesY) {
                            if (Main.tile[x, y].HasTile) {
                                WorldGen.SquareTileFrame(x, y, true);
                            }
                        }
                    }
                }

                // 8. Set Spawn Point
                Main.spawnTileX = centerX;
                Main.spawnTileY = plazaY - 3;
            }));
        }
    }
}