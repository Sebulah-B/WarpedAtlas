using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace WarpedAtlas.Content.Subworlds
{
    public enum GenerationAxis { Free, Vertical, Horizontal }
    public enum CorridorStyle { Solid, FloatingBridge, Chokepoint }

    public abstract class MapProfile
    {
        public abstract string DisplayName { get; }
        public abstract ushort TileId { get; }
        public abstract ushort WallId { get; }
        public abstract List<int> EnemyPool { get; }
        
        // --- SPATIAL CONFIGURATIONS ---
        public virtual Point RoomSizeMinMax => new Point(32, 46); 
        public virtual GenerationAxis Axis => GenerationAxis.Free;
        public virtual CorridorStyle Style => CorridorStyle.Solid;
        public virtual bool Submerged => false; 

        public virtual int PlatformStyle => TileId switch
        {
            TileID.BlueDungeonBrick => 2,     
            TileID.LihzahrdBrick => 12,       
            TileID.SandstoneBrick => 11,      
            TileID.PearlstoneBrick => 33,     
            TileID.IceBrick => 19,            
            TileID.ObsidianBrick => 13,       
            TileID.Granite => 28,             
            TileID.MushroomBlock => 18,       
            _ => 0                            
        };
    }

    // Act I: Mictlan Crypts (Free layout)
    public class DungeonProfile : MapProfile {
        public override string DisplayName => "Mictlan Crypts";
        public override ushort TileId => TileID.BlueDungeonBrick; 
        public override ushort WallId => WallID.BlueDungeonUnsafe; 
        public override List<int> EnemyPool => new List<int> { 21, 32, 2 }; 
    }

    // Act II: Tepeyollotl's Heart (Standard)
    public class LihzahrdProfile : MapProfile {
        public override string DisplayName => "Tepeyollotl's Heart";
        public override ushort TileId => TileID.LihzahrdBrick; 
        public override ushort WallId => WallID.LihzahrdBrickUnsafe; 
        public override List<int> EnemyPool => new List<int> { 198, 199, 201 }; 
    }

    // Act III: Toltec Ruins (Forced Vertical layout, flooded floors for Cenote feel)
    public class SandstoneProfile : MapProfile {
        public override string DisplayName => "Toltec Ruins";
        public override ushort TileId => TileID.SandstoneBrick; 
        public override ushort WallId => WallID.Sandstone; 
        public override List<int> EnemyPool => new List<int> { 78, 533, 541 };
        
        public override GenerationAxis Axis => GenerationAxis.Vertical;
        public override bool Submerged => true;
    }

    // Act IV: Quetzal's Domain
    public class HallowedProfile : MapProfile {
        public override string DisplayName => "Quetzal's Domain";
        public override ushort TileId => TileID.PearlstoneBrick; 
        public override ushort WallId => WallID.HallowUnsafe1; 
        public override List<int> EnemyPool => new List<int> { 75, 79, 84 }; 
    }

    // Act V: Itztli Frost Peaks
    public class CryoFrostProfile : MapProfile {
        public override string DisplayName => "Itztli Frost Peaks";
        public override ushort TileId => TileID.IceBrick; 
        public override ushort WallId => WallID.IceUnsafe; 
        public override List<int> EnemyPool => new List<int> { 243, 127, 147 }; 
    }

    // Act VI: Xiuhtecuhtli's Core (Horizontal layout)
    public class HellforgeProfile : MapProfile {
        public override string DisplayName => "Xiuhtecuhtli's Core";
        public override ushort TileId => TileID.ObsidianBrick; 
        public override ushort WallId => WallID.HellstoneBrickUnsafe; 
        public override List<int> EnemyPool => new List<int> { 59, 62, 273 }; 
        public override GenerationAxis Axis => GenerationAxis.Horizontal;
    }

    // Act VII: Ollin Tectonic Fault
    public class GraniteProfile : MapProfile {
        public override string DisplayName => "Ollin Tectonic Fault";
        public override ushort TileId => TileID.Granite; 
        public override ushort WallId => WallID.GraniteUnsafe; 
        public override List<int> EnemyPool => new List<int> { 410, 430, 164 }; 
    }

    // Act VIII: Tzitzimitl Abyss
    public class FungalProfile : MapProfile {
        public override string DisplayName => "Tzitzimitl Abyss";
        public override ushort TileId => TileID.MushroomBlock; 
        public override ushort WallId => WallID.MushroomUnsafe; 
        public override List<int> EnemyPool => new List<int> { 258, 260, 259 }; 
    }
}