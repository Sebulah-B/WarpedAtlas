using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace WarpedAtlas.Content.Subworlds
{
    public enum RoomType { Entrance, Standard, Puzzle, Vault, BossArena, Transition }

    public class RoomNode
    {
        public Point GridCoords;
        public Rectangle TileBounds;
        public RoomType Type;
        public List<RoomNode> ConnectedNodes = new List<RoomNode>();
        public Point CenterTile => TileBounds.Center;
        public string Encounter = "None";
    }

    public class MapGridGraph
    {
        public List<RoomNode> Nodes = new List<RoomNode>();
        public RoomNode EntranceNode;
        public RoomNode BossNode;

        public static MapGridGraph BuildGraph(int originX, int originY) {
            MapGridGraph graph = new MapGridGraph();
            var profile = BaseDungeonSubworld.ActiveProfile;

            // Stable baseline spacing
            int sectorSpacingX = 110;
            int sectorSpacingY = 70;

            int totalRooms = BaseDungeonSubworld.MapRNG.Next(9, 13);
            int currentGridX = 0;
            int currentGridY = 0;
            Dictionary<string, RoomNode> occupiedGrid = new Dictionary<string, RoomNode>();
            RoomNode previousNode = null;

            int guaranteedVaultIndex = BaseDungeonSubworld.MapRNG.Next(1, totalRooms - 1);
            
            // Map boundary constraints
            int maxWorldTilesX = 3900; 
            int maxWorldTilesY = 1000;

            for (int i = 0; i < totalRooms; i++) {
                
                // Stable room sizes
                int minW = profile != null ? profile.RoomSizeMinMax.X : 32;
                int maxW = profile != null ? profile.RoomSizeMinMax.Y : 46;
                int rWidth = BaseDungeonSubworld.MapRNG.Next(minW, maxW);
                int rHeight = BaseDungeonSubworld.MapRNG.Next(20, 28);

                int worldX = originX + (currentGridX * sectorSpacingX);
                int worldY = originY + (currentGridY * sectorSpacingY);

                RoomNode node = new RoomNode {
                    GridCoords = new Point(currentGridX, currentGridY),
                    TileBounds = new Rectangle(worldX, worldY, rWidth, rHeight),
                    Type = RoomType.Standard,
                    Encounter = "None"
                };

                if (i == 0) {
                    node.Type = RoomType.Entrance;
                    graph.EntranceNode = node;
                } else if (i == totalRooms - 1) {
                    node.Type = RoomType.BossArena;
                    graph.BossNode = node;
                } else if (i == guaranteedVaultIndex) {
                    node.Type = RoomType.Vault;
                } else {
                    int roll = BaseDungeonSubworld.MapRNG.Next(10);
                    if (roll < 3) {
                        node.Type = RoomType.Puzzle;
                    } else {
                        node.Type = RoomType.Standard;
                        if (BaseDungeonSubworld.MapRNG.NextBool(3)) node.Encounter = "Escalation";
                    }
                }

                if (previousNode != null) {
                    previousNode.ConnectedNodes.Add(node);
                    node.ConnectedNodes.Add(previousNode);
                }

                occupiedGrid[$"{currentGridX}_{currentGridY}"] = node;
                graph.Nodes.Add(node);
                previousNode = node;

                int nextX = currentGridX;
                int nextY = currentGridY;
                bool validPlacement = false;
                int attempts = 0;
                GenerationAxis genAxis = profile != null ? profile.Axis : GenerationAxis.Free;

                // Guardrail Loop
                while (!validPlacement && attempts < 15) {
                    int dirRoll = BaseDungeonSubworld.MapRNG.Next(4);
                    int testX = currentGridX;
                    int testY = currentGridY;

                    if (genAxis == GenerationAxis.Vertical) testY += 1;
                    else if (genAxis == GenerationAxis.Horizontal) testX += 1;
                    else {
                        if (dirRoll == 0) testY -= 1;
                        else if (dirRoll == 1) testY += 1;
                        else if (dirRoll == 2) testX -= 1;
                        else testX += 1;
                    }

                    int projectedWorldX = originX + (testX * sectorSpacingX);
                    int projectedWorldY = originY + (testY * sectorSpacingY);

                    if (projectedWorldX > 200 && projectedWorldX < maxWorldTilesX && 
                        projectedWorldY > 200 && projectedWorldY < maxWorldTilesY &&
                        !occupiedGrid.ContainsKey($"{testX}_{testY}")) 
                    {
                        nextX = testX;
                        nextY = testY;
                        validPlacement = true;
                    }
                    
                    attempts++;
                    if (!validPlacement && attempts > 10 && genAxis != GenerationAxis.Free) break; 
                }

                currentGridX = nextX;
                currentGridY = nextY;
            }
            return graph;
        }
    }
}