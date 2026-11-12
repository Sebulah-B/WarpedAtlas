using Terraria;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Players
{
	public class AtlasMapTracker : ModPlayer
	{
		// Persists natively on the character data layout
		public bool HasActiveMap = false;

		public override void OnEnterWorld() {
			// Optional: Clears map locking state if the player logs out completely 
			// Delete this line if you want the lockout to persist across game sessions!
			HasActiveMap = false; 
		}
	}
}