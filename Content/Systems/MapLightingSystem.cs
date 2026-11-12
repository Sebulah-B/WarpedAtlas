using Terraria;
using Terraria.ModLoader;
using SubworldLibrary;
using WarpedAtlas.Content.Subworlds;

namespace WarpedAtlas.Content.Systems
{
    public class MapLightingSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            // Check if we are inside our map subworld or the apex arena
            bool inDungeon = SubworldSystem.IsActive<BaseDungeonSubworld>();
            bool inBossDomain = SubworldSystem.IsActive<BossDomainSubworld>();
            
            // Check if we are inside our sky-island hubworld
            bool inHub = SubworldSystem.Current == null; 

            if (inDungeon || inBossDomain || inHub)
            {
                Player player = Main.LocalPlayer;

                // Define a large bounding window of blocks surrounding the player's current coordinate block
                int startX = (int)(player.position.X / 16f) - 60;
                int endX = (int)(player.position.X / 16f) + 60;
                int startY = (int)(player.position.Y / 16f) - 45;
                int endY = (int)(player.position.Y / 16f) + 45;

                // Clamp window sizes cleanly within safe structural engine parameters
                if (startX < 0) startX = 0;
                if (endX > Main.maxTilesX) endX = Main.maxTilesX;
                if (startY < 0) startY = 0;
                if (endY > Main.maxTilesY) endY = Main.maxTilesY;

                // Rapidly fire a low-intensity, clean ambient white light vector across the viewport window
                for (int x = startX; x < endX; x++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                        Lighting.AddLight(x, y, 0.45f, 0.45f, 0.45f);
                    }
                }
            }
        }
    }
}