using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using SubworldLibrary;

namespace WarpedAtlas.Content.Subworlds
{
    public class BossDomainSubworld : Subworld
    {
        public override int Width => 800; 
        public override int Height => 800;
        public override bool ShouldSave => false;
        public override bool NoPlayerSaving => false;

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new PassLegacy("Building Apex Arena", (progress, configuration) => {
                progress.Message = "Constructing Boss Domain...";
                
                if (BaseDungeonSubworld.ActiveProfile == null) 
                    BaseDungeonSubworld.ActiveProfile = new DungeonProfile();

                ushort floorTile = BaseDungeonSubworld.ActiveProfile.TileId;
                ushort wallTile = BaseDungeonSubworld.ActiveProfile.WallId;

                int centerX = Width / 2;
                int centerY = Height / 2;
                int arenaRadius = 60;
                int arenaHeight = 40;

                // 1. Clear everything and place background walls
                for (int x = 0; x < Width; x++) {
                    for (int y = 0; y < Height; y++) {
                        Tile tile = Main.tile[x, y]; 
                        tile.HasTile = false;
                        if (x >= centerX - arenaRadius && x <= centerX + arenaRadius && y >= centerY - arenaHeight && y <= centerY + arenaHeight) {
                            tile.WallType = wallTile;
                        }
                    }
                }

                // 2. Build the solid arena floor and ceiling
                for (int x = centerX - arenaRadius; x <= centerX + arenaRadius; x++) {
                    for (int y = centerY + arenaHeight; y <= centerY + arenaHeight + 3; y++) {
                        Tile floorTileRef = Main.tile[x, y]; 
                        floorTileRef.HasTile = true;
                        floorTileRef.TileType = floorTile;
                    }
                    for (int y = centerY - arenaHeight - 3; y <= centerY - arenaHeight; y++) {
                        Tile ceilTileRef = Main.tile[x, y]; 
                        ceilTileRef.HasTile = true;
                        ceilTileRef.TileType = floorTile;
                    }
                }

                // 3. Build side walls
                for (int y = centerY - arenaHeight; y <= centerY + arenaHeight; y++) {
                    for (int thickness = 0; thickness < 4; thickness++) {
                        Tile leftWallTile = Main.tile[centerX - arenaRadius - thickness, y]; 
                        leftWallTile.HasTile = true;
                        leftWallTile.TileType = floorTile;
                        
                        Tile rightWallTile = Main.tile[centerX + arenaRadius + thickness, y]; 
                        rightWallTile.HasTile = true;
                        rightWallTile.TileType = floorTile;
                    }
                }

                // 4. Place 3 rows of platforms
                for (int p = 1; p <= 3; p++) {
                    int platformY = (centerY + arenaHeight) - (p * 15);
                    for (int x = centerX - arenaRadius + 5; x <= centerX + arenaRadius - 5; x++) {
                        Tile platformTile = Main.tile[x, platformY]; 
                        platformTile.HasTile = true;
                        platformTile.TileType = TileID.Platforms;
                        platformTile.TileFrameY = (short)(BaseDungeonSubworld.ActiveProfile.PlatformStyle * 18);
                    }
                }

                // 5. Place the Apex Altar in the center
                WorldGen.PlaceObject(centerX, centerY + arenaHeight - 1, ModContent.TileType<Tiles.ApexAltarTile>(), mute: true);

                // 6. Illuminate the Arena (Torch Grid)
                for (int x = centerX - arenaRadius + 10; x <= centerX + arenaRadius - 10; x += 12) {
                    for (int p = 1; p <= 3; p++) {
                        int platformY = (centerY + arenaHeight) - (p * 15);
                        WorldGen.PlaceObject(x, platformY - 1, TileID.Torches, mute: true, style: 8); // Style 8 = Ice/White Torch
                    }
                }
                
                for (int x = 0; x < Width; x++) {
                    for (int y = 0; y < Height; y++) {
                        if (Main.tile[x, y].HasTile) WorldGen.SquareTileFrame(x, y, true);
                    }
                }

                Main.spawnTileX = centerX;
                Main.spawnTileY = centerY + arenaHeight - 3;
            })
        };

        public override void Update() {
            Main.dayTime = true;
            Main.time = 27000.0;
            Main.bloodMoon = false;
            Main.eclipse = false;
            Main.invasionType = 0;
        }

        public override void OnEnter() {
            Main.NewText($"[c/ff5555:Entered Apex Boss Domain: {BaseDungeonSubworld.ActiveProfile.DisplayName}]", 255, 255, 255);
        }
    }
}