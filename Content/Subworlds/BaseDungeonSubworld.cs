using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader; 
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using SubworldLibrary;
using WarpedAtlas.Content.NPCs;

namespace WarpedAtlas.Content.Subworlds
{
    public class BaseDungeonSubworld : Subworld
    {
        public override int Width => 4200; 
        public override int Height => 1200;
        public override bool ShouldSave => false;
        public override bool NoPlayerSaving => false;

        public static int CurrentMapTier = 1;
        public static MapProfile ActiveProfile;
        public static int CurrentZoneIndex = 1;
        public static int TotalMonstersSpawned = 0;
        public static int RequiredKillCount = 0;

        public static int GateWallX = 0;
        public static int GateFloorY = 0;
        public static bool GateHasOpened = false;
        public static bool BossSummoned = false;
        public static int GracePeriodTimer = 0;
        public static Terraria.Utilities.UnifiedRandom MapRNG;

        // NEW: Store the penultimate room coordinates for boss respawns
        public static int PenultimateRoomX = 0;
        public static int PenultimateRoomY = 0;

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new PassLegacy("Generating Map", (progress, configuration) => {
                
                MapRNG = new Terraria.Utilities.UnifiedRandom(System.Environment.TickCount);

                if (ActiveProfile == null) ActiveProfile = new DungeonProfile();
                
                MapTraversability.DroppedLootTracker.Clear();
                
                progress.Message = $"Carving {ActiveProfile.DisplayName}...";

                ushort floorPalette = ActiveProfile.TileId;
                ushort wallPalette = ActiveProfile.WallId;  

                int originX = 200;
                int originY = Height / 2;
                
                GateHasOpened = false;
                BossSummoned = false;

                MapGridGraph graph = MapGridGraph.BuildGraph(originX, originY);
                
                MapCell[,] blueprint = new MapCell[Width, Height];

                foreach (var node in graph.Nodes) {
                    Rectangle shell = node.TileBounds;
                    shell.Inflate(4, 4); 
                    MapTraversability.FillBounds(shell, MapCell.Shell, blueprint);
                }

                foreach (var node in graph.Nodes) {
                    foreach (var target in node.ConnectedNodes) {
                        MapTraversability.MapCorridor(node.CenterTile, target.CenterTile, blueprint, true);
                    }
                }

                foreach (var node in graph.Nodes) {
                    MapTraversability.FillBounds(node.TileBounds, MapCell.Hollow, blueprint);
                }

                HashSet<string> carved = new HashSet<string>();
                foreach (var node in graph.Nodes) {
                    foreach (var target in node.ConnectedNodes) {
                        string key = node.GetHashCode() < target.GetHashCode() ? $"{node.GetHashCode()}_{target.GetHashCode()}" : $"{target.GetHashCode()}_{node.GetHashCode()}";
                        if (!carved.Contains(key)) {
                            carved.Add(key);
                            MapTraversability.MapCorridor(node.CenterTile, target.CenterTile, blueprint, false);
                        }
                    }
                }


                MapTraversability.FinalizeTraversability(graph, blueprint);

                for (int x = 0; x < Width; x++) {
                    for (int y = 0; y < Height; y++) {
                        MapCell cell = blueprint[x, y];
                        Tile tile = Main.tile[x, y];

                        if (cell != MapCell.Void) {
                            tile.WallType = wallPalette; 
                        }

                        switch (cell) {
                            case MapCell.Shell:
                                tile.HasTile = true;
                                tile.TileType = floorPalette;
                                break;
                            case MapCell.Hollow:
                                tile.HasTile = false;
                                break;
                            case MapCell.Rope:
                                tile.HasTile = true;
                                tile.TileType = TileID.Rope;
                                break;
                            case MapCell.Platform:
                                tile.HasTile = true;
                                tile.TileType = TileID.Platforms;
                                break;
                        }
                    }
                }

                for (int x = 0; x < Width; x++) {
                    for (int y = 0; y < Height; y++) {
                        if (blueprint[x, y] != MapCell.Void) {
                            WorldGen.SquareTileFrame(x, y, true);
                        }
                    }
                }

                foreach (var node in graph.Nodes) {
                    MapTraversability.BuildProceduralInterior(node.TileBounds, node.Type, floorPalette, wallPalette);
                }

                RoomNode entrance = graph.EntranceNode;
                Main.spawnTileX = entrance.CenterTile.X;
                Main.spawnTileY = entrance.TileBounds.Bottom - 3;

                RoomNode bossNode = graph.BossNode;

                // NEW: Capture the penultimate room's coordinates for respawning
                if (bossNode.ConnectedNodes.Count > 0) {
                    RoomNode penultimate = bossNode.ConnectedNodes[0];
                    PenultimateRoomX = penultimate.CenterTile.X;
                    PenultimateRoomY = penultimate.TileBounds.Bottom - 3;
                } else {
                    PenultimateRoomX = entrance.CenterTile.X;
                    PenultimateRoomY = entrance.TileBounds.Bottom - 3;
                }

                int altarX = bossNode.CenterTile.X;
                int altarY = bossNode.CenterTile.Y;

                // 1. Build the Pedestal
                for (int px = altarX - 3; px <= altarX + 3; px++) {
                    for (int py = altarY; py <= altarY + 1; py++) {
                        Tile floorTile = Main.tile[px, py];
                        floorTile.HasTile = true;
                        floorTile.TileType = floorPalette;
                        floorTile.IsHalfBlock = false;
                        floorTile.Slope = SlopeType.Solid;
                    }
                }
                for (int px = altarX - 3; px <= altarX + 3; px++) {
                    for (int py = altarY; py <= altarY + 1; py++) {
                        WorldGen.SquareTileFrame(px, py, true);
                    }
                }
                
                // 2. Drop the Rope
                for (int py = altarY + 2; py < bossNode.TileBounds.Bottom; py++) {
                    Tile rope = Main.tile[altarX, py];
                    if (!rope.HasTile) {
                        rope.HasTile = true;
                        rope.TileType = TileID.Rope;
                    }
                }

                // 3. Build the ActiveStoneBlock Box Seal
                GateWallX = altarX;
                GateFloorY = altarY;
                for (int gx = altarX - 4; gx <= altarX + 4; gx++) {
                    for (int gy = altarY - 5; gy <= altarY + 1; gy++) {
                        if (gx == altarX - 4 || gx == altarX + 4 || gy == altarY - 5 || gy == altarY + 1) {
                            if (gy == altarY + 1 && gx >= altarX - 3 && gx <= altarX + 3) continue; 
                            
                            Tile tile = Main.tile[gx, gy];
                            tile.HasTile = true;
                            tile.TileType = TileID.ActiveStoneBlock;
                        }
                    }
                }
                
                for (int gx = altarX - 4; gx <= altarX + 4; gx++) {
                    for (int gy = altarY - 5; gy <= altarY + 1; gy++) {
                        if (gx == altarX - 4 || gx == altarX + 4 || gy == altarY - 5 || gy == altarY + 1) {
                            WorldGen.SquareTileFrame(gx, gy, true);
                        }
                    }
                }

                // 4. Forcefully clear the exact 3x4 cavity needed for the Altar
                for (int cx = altarX - 3; cx <= altarX + 3; cx++) {
                    for (int cy = altarY - 4; cy <= altarY - 1; cy++) {
                        Tile clearTile = Main.tile[cx, cy];
                        clearTile.HasTile = false;
                    }
                }

                // 5. Bruteforce Altar Injection
                bool altarPlaced = WorldGen.PlaceObject(altarX, altarY - 1, ModContent.TileType<Tiles.MapAltarTile>(), mute: true);
                
                if (!altarPlaced) {
                    ushort altarType = (ushort)ModContent.TileType<Tiles.MapAltarTile>();
                    for (int x = 0; x < 3; x++) {
                        for (int y = 0; y < 4; y++) {
                            Tile tile = Main.tile[altarX - 1 + x, altarY - 4 + y];
                            tile.HasTile = true;
                            tile.TileType = altarType;
                            tile.TileFrameX = (short)(x * 18);
                            tile.TileFrameY = (short)(y * 18);
                        }
                    }
                }

                // 6. DUNGEON MOB SPAWNING & ENCOUNTER INJECTION
                int monstersSpawned = 0;
                var spawnSource = new Terraria.DataStructures.EntitySource_WorldGen();

                foreach (var node in graph.Nodes) {
                    if (node.Type == RoomType.Entrance) continue;

                    // INJECT THE ESCALATION ANCHOR
                    if (node.Encounter == "Escalation") {
                        // Spawn the anchor perfectly in the bottom center of the room
                        int anchorX = node.TileBounds.Center.X * 16;
                        int anchorY = (node.TileBounds.Bottom - 3) * 16;
                        
                        NPC.NewNPC(spawnSource, anchorX, anchorY, ModContent.NPCType<EscalationAnchorNPC>());
                    }

                    // Standard Room Mob Generation (Existing Code)
                    int mobsInThisRoom = MapRNG.Next(6, 11);
                    for (int m = 0; m < mobsInThisRoom; m++) {
                        int randX = MapRNG.Next(node.TileBounds.X + 3, node.TileBounds.X + node.TileBounds.Width - 3);
                        int randY = node.TileBounds.Bottom - 3;
                        
                        int npcType = ActiveProfile.EnemyPool[MapRNG.Next(ActiveProfile.EnemyPool.Count)];
                        int npcIndex = NPC.NewNPC(spawnSource, randX * 16, randY * 16, npcType);
                        if (npcIndex >= 0 && npcIndex < Main.maxNPCs) {
                            Main.npc[npcIndex].netUpdate = true;
                            monstersSpawned++;
                        }
                    }
                }

                TotalMonstersSpawned = monstersSpawned;
                RequiredKillCount = (int)(TotalMonstersSpawned * 0.75f);
            })
        };

        // Inside WarpedAtlas.Content.Subworlds.BaseDungeonSubworld
        public static int CurrentPressureLevel = 0;

        public override void OnEnter() {
            GracePeriodTimer = 600; 
            DungeonAggroControl.ResetGraceState();
            GateHasOpened = false;
            BossSummoned = false;
            CurrentPressureLevel = 0; // Reset juicing on entry

            string name = ActiveProfile != null ? ActiveProfile.DisplayName : "Map Area";
            Main.NewText($"[c/ff5555:Entered {name} (Tier {CurrentMapTier})]", 255, 255, 255);
        }

        public override void OnExit() {
            GracePeriodTimer = 0;
            DungeonAggroControl.ResetGraceState();
            ActiveProfile = null; 
            CurrentMapTier = 1;
            PenultimateRoomX = 0;
            PenultimateRoomY = 0;
            CurrentPressureLevel = 0; // Reset juicing on exit
        }

        public override void Update() {
            Main.dayTime = true;
            Main.time = 27000.0;
            Main.bloodMoon = false;
            Main.eclipse = false;
            Main.invasionType = 0;
        }
    }
}