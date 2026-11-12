using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using WarpedAtlas.Content.Players;

namespace WarpedAtlas.Content.Systems
{
    public enum NodeType { Origin, Small, Keystone }

    public class PassiveNode 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Point GridCoord { get; set; }
        public Vector2 Position => new Vector2(GridCoord.X * 80f, GridCoord.Y * 80f);
        public int IconItemId { get; set; }
        public NodeType Type { get; set; }
        public Action<Player> ApplyHealthAndDefense { get; set; }
        public Action<Player> ApplyOffensiveStats { get; set; }
        public List<int> ConnectedNodes { get; set; }

        public PassiveNode(int id, string name, string desc, Point gridCoord, int iconId, NodeType type, Action<Player> healthAndDef, Action<Player> offense) {
            Id = id; Name = name; Description = desc; GridCoord = gridCoord; IconItemId = iconId; Type = type;
            ApplyHealthAndDefense = healthAndDef; ApplyOffensiveStats = offense; ConnectedNodes = new List<int>();
        }
    }

    public class BranchBlueprint {
        public string R1BaseName;   public int[] R1BaseIcons;   public Action<Player> R1BaseStats;
        public string R1LeftName;   public int[] R1LeftIcons;   public Action<Player> R1LeftStats;
        public string R1RightName;  public int[] R1RightIcons;  public Action<Player> R1RightStats;
        public string MinorKeyName; public string MinorKeyDesc; public int MinorKeyIcon; public Action<Player> MinorKeyStats;
        public string R2BaseName;   public int[] R2BaseIcons;   public Action<Player> R2BaseStats;
        public string R2LeftName;   public int[] R2LeftIcons;   public Action<Player> R2LeftStats;
        public string R2RightName;  public int[] R2RightIcons;  public Action<Player> R2RightStats;
        public string MajorKeyName; public string MajorKeyDesc; public int MajorKeyIcon; public Action<Player> MajorKeyStats;
    }

    public static class PassiveTreeRegistry 
    {
        public static Dictionary<int, PassiveNode> Nodes = new Dictionary<int, PassiveNode>();

        public static void Initialize() {
            Nodes.Clear();
            WarriorPassiveTree.Build();
            RangerPassiveTree.Build();
            SorcererPassiveTree.Build();
            SummonerPassiveTree.Build();
        }

        public static void AddNode(int id, string name, string desc, Point gridCoord, int iconId, NodeType type, Action<Player> def, Action<Player> off) {
            if (!Nodes.ContainsKey(id)) {
                Main.instance.LoadItem(iconId); // PRELOAD TEXTURE TO FIX UI STUTTERING
                Nodes.Add(id, new PassiveNode(id, name, desc, gridCoord, iconId, type, def, off));
            }
        }

        public static void Link(int id1, int id2) {
            if (Nodes.ContainsKey(id1) && Nodes.ContainsKey(id2)) {
                if (!Nodes[id1].ConnectedNodes.Contains(id2)) Nodes[id1].ConnectedNodes.Add(id2);
                if (!Nodes[id2].ConnectedNodes.Contains(id1)) Nodes[id2].ConnectedNodes.Add(id1);
            }
        }

        private static Point TransformOffset(Point p, int dir) {
            if (dir % 2 == 0) { 
                return dir switch {
                    0 => new Point(p.X, -p.Y),  2 => new Point(p.Y, -p.X),  
                    4 => new Point(p.X, p.Y),   6 => new Point(-p.Y, p.X), _ => new Point(p.X, p.Y)
                };
            } else { 
                return dir switch {
                    1 => new Point(p.X, -p.Y),   3 => new Point(p.X, p.Y),    
                    5 => new Point(-p.X, p.Y),   7 => new Point(-p.X, -p.Y), _ => new Point(p.X, p.Y)
                };
            }
        }

        private static readonly (Point pos, int[] links)[] CardinalTemplate = new (Point, int[])[] {
            (new Point(0, 1), new int[] { 1 }), (new Point(0, 2), new int[] { 2, 3 }),      
            (new Point(-1, 3), new int[] { 4 }), (new Point(1, 3), new int[] { 5 }),         
            (new Point(-1, 4), new int[] { 6 }), (new Point(1, 4), new int[] { 7 }),         
            (new Point(-1, 5), new int[] { 8 }), (new Point(1, 5), new int[] { 8 }),         
            (new Point(0, 6), new int[] { 9 }), (new Point(0, 7), new int[] { 10 }),        
            (new Point(0, 8), new int[] { 11 }), (new Point(0, 9), new int[] { 12, 13 }),    
            (new Point(-1, 10), new int[] { 14 }), (new Point(1, 10), new int[] { 15 }),       
            (new Point(-2, 11), new int[] { 16 }), (new Point(2, 11), new int[] { 17 }),       
            (new Point(-2, 12), new int[] { 18 }), (new Point(2, 12), new int[] { 19 }),       
            (new Point(-1, 13), new int[] { 20 }), (new Point(1, 13), new int[] { 20 }),       
            (new Point(0, 14), new int[] { 21 }), (new Point(0, 15), new int[] {})            
        };

        private static readonly (Point pos, int[] links)[] DiagonalTemplate = new (Point, int[])[] {
            (new Point(1, 1), new int[] { 1 }), (new Point(2, 2), new int[] { 2, 3 }),      
            (new Point(2, 3), new int[] { 4 }), (new Point(3, 2), new int[] { 5 }),         
            (new Point(2, 4), new int[] { 6 }), (new Point(4, 2), new int[] { 7 }),         
            (new Point(3, 4), new int[] { 8 }), (new Point(4, 3), new int[] { 8 }),         
            (new Point(4, 4), new int[] { 9 }), (new Point(5, 5), new int[] { 10 }),        
            (new Point(6, 6), new int[] { 11 }), (new Point(7, 7), new int[] { 12, 13 }),    
            (new Point(7, 8), new int[] { 14 }), (new Point(8, 7), new int[] { 15 }),        
            (new Point(7, 9), new int[] { 16 }), (new Point(9, 7), new int[] { 17 }),       
            (new Point(8, 10), new int[] { 18 }), (new Point(10, 8), new int[] { 19 }),       
            (new Point(9, 10), new int[] { 20 }), (new Point(10, 9), new int[] { 20 }),       
            (new Point(10, 10), new int[] { 21 }), (new Point(11, 11), new int[] {})       
        };

        // NOW RETURNS THE OUTER-EDGE MAJOR KEYSTONE ID TO ALLOW INFINITE CHAINING/FORKING
        public static int BuildClassBranch(int parentId, ref int currentId, int dir, BranchBlueprint bp) {
            int startX = Nodes[parentId].GridCoord.X;
            int startY = Nodes[parentId].GridCoord.Y;
            int[] allocatedIds = new int[22];
            var template = (dir % 2 == 0) ? CardinalTemplate : DiagonalTemplate;
            
            int r1c = 0, r1l = 0, r1r = 0, r2c = 0, r2l = 0, r2r = 0;

            for (int i = 0; i < 22; i++) {
                int thisId = currentId++;
                allocatedIds[i] = thisId;
                
                Point offset = TransformOffset(template[i].pos, dir);
                Point gridPos = new Point(startX + offset.X, startY + offset.Y);

                if (i == 10) AddNode(thisId, bp.MinorKeyName, bp.MinorKeyDesc, gridPos, bp.MinorKeyIcon, NodeType.Keystone, bp.MinorKeyStats, p => {});
                else if (i == 21) AddNode(thisId, bp.MajorKeyName, bp.MajorKeyDesc, gridPos, bp.MajorKeyIcon, NodeType.Keystone, bp.MajorKeyStats, p => {});
                else if (i == 0 || i == 1 || i == 8 || i == 9) { 
                    int icon = bp.R1BaseIcons[r1c++ % bp.R1BaseIcons.Length];
                    AddNode(thisId, bp.R1BaseName, "Minor Passive", gridPos, icon, NodeType.Small, bp.R1BaseStats, p => {});
                }
                else if (i == 2 || i == 4 || i == 6) { 
                    int icon = bp.R1LeftIcons[r1l++ % bp.R1LeftIcons.Length];
                    AddNode(thisId, bp.R1LeftName, "Minor Passive", gridPos, icon, NodeType.Small, bp.R1LeftStats, p => {});
                }
                else if (i == 3 || i == 5 || i == 7) { 
                    int icon = bp.R1RightIcons[r1r++ % bp.R1RightIcons.Length];
                    AddNode(thisId, bp.R1RightName, "Minor Passive", gridPos, icon, NodeType.Small, bp.R1RightStats, p => {});
                }
                else if (i == 11 || i == 20) { 
                    int icon = bp.R2BaseIcons[r2c++ % bp.R2BaseIcons.Length];
                    AddNode(thisId, bp.R2BaseName, "Minor Passive", gridPos, icon, NodeType.Small, bp.R2BaseStats, p => {});
                }
                else if (i == 12 || i == 14 || i == 16 || i == 18) { 
                    int icon = bp.R2LeftIcons[r2l++ % bp.R2LeftIcons.Length];
                    AddNode(thisId, bp.R2LeftName, "Minor Passive", gridPos, icon, NodeType.Small, bp.R2LeftStats, p => {});
                }
                else if (i == 13 || i == 15 || i == 17 || i == 19) { 
                    int icon = bp.R2RightIcons[r2r++ % bp.R2RightIcons.Length];
                    AddNode(thisId, bp.R2RightName, "Minor Passive", gridPos, icon, NodeType.Small, bp.R2RightStats, p => {});
                }
            }

            Link(parentId, allocatedIds[0]); 
            for (int i = 0; i < 22; i++) {
                foreach (int linkTarget in template[i].links) Link(allocatedIds[i], allocatedIds[linkTarget]);
            }

            return allocatedIds[21];
        }
    }
}