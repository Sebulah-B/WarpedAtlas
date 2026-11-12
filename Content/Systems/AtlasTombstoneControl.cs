using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;
using WarpedAtlas.Content.Subworlds;

namespace WarpedAtlas.Content.Systems
{
    public class AtlasTombstoneControl : GlobalProjectile
    {
        // We do not need InstancePerEntity since we are not storing custom data per projectile
        
        public override void OnSpawn(Projectile projectile, Terraria.DataStructures.IEntitySource source)
        {
            // aiStyle 17 is universally assigned to all Tombstone projectiles in Terraria
            if (projectile.aiStyle == 17)
            {
                // Vaporize the tombstone if we are in the Dungeon, Boss Arena, or the Sky Hub
                if (SubworldSystem.IsActive<BaseDungeonSubworld>() || 
                    SubworldSystem.IsActive<BossDomainSubworld>() || 
                    SubworldSystem.Current == null)
                {
                    projectile.active = false;
                }
            }
        }
    }
}