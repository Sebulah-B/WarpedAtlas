using Terraria;
using Terraria.ModLoader;
using SubworldLibrary;
using WarpedAtlas.Content.Subworlds;

namespace WarpedAtlas
{
	public class MapIlluminationBuff : ModBuff
	{
		// Points tML to its internal missing texture fallback asset
		public override string Texture => "ModLoader/UnloadedItem";

		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			if (SubworldSystem.IsActive<BaseDungeonSubworld>()) {
				// Jacked up the brightness values significantly (from 2.5 to 5.5) for a crisp, wide sight radius
				Lighting.AddLight(player.Center, 5.5f, 5.5f, 6.0f);
				
				player.buffTime[buffIndex] = 10; 
			} else {
				player.DelBuff(buffIndex); 
			}
		}
	}
}