using Terraria;
using Terraria.ModLoader;
using SubworldLibrary;
using WarpedAtlas.Content.Subworlds;
using Microsoft.Xna.Framework;

namespace WarpedAtlas.Content.NPCs
{
	public class DungeonAggroControl : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		private static Vector2 spawnPosition = Vector2.Zero;
		private static bool hasMovedOrAttacked = false;

		public static void ResetGraceState() {
			spawnPosition = Vector2.Zero;
			hasMovedOrAttacked = false;
		}

		public override bool PreAI(NPC npc) {
			// Only apply this logic while inside our custom dungeon run instances
			if (!SubworldSystem.IsActive<BaseDungeonSubworld>()) {
				return true;
			}

			Player player = Main.LocalPlayer;

			// Monitor player actions if loading shroud window is processing
			if (BaseDungeonSubworld.GracePeriodTimer > 0 && !hasMovedOrAttacked) {
				if (spawnPosition == Vector2.Zero) {
					spawnPosition = player.position;
				}

				// Break shroud cover if player runs away, swings a weapon, or uses an item
				if (Vector2.Distance(player.position, spawnPosition) > 16f || 
					player.itemAnimation > 0 || player.controlUseItem || player.controlUseTile) {
					hasMovedOrAttacked = true;
					BaseDungeonSubworld.GracePeriodTimer = 0;
					Main.NewText("⚔️ Spatial Shroud broken! Combat engaged.", 255, 100, 100);
				}
			}

			// Active Shroud Lock: Freeze everything while timer counts down
			if (BaseDungeonSubworld.GracePeriodTimer > 0 && !hasMovedOrAttacked) {
				FreezeNPC(npc);
				return false;
			}

			if (BaseDungeonSubworld.GracePeriodTimer <= 0 && !hasMovedOrAttacked) {
				hasMovedOrAttacked = true; 
			}

			// --- Original Linear Room Proximity Activation Code ---
			// Calculate the absolute distance in blocks between this monster and the player
			float blockDistanceX = System.Math.Abs(npc.Center.X - player.Center.X) / 16f;

			// If the player hasn't progressed to their room yet (more than 45 blocks away)
			if (blockDistanceX > 45f) {
				FreezeNPC(npc);
				return false; 
			}

			// Player is inside their room trigger threshold! Wake up and attack!
			if (npc.target == -1 || Main.player[npc.target].dead) {
				npc.TargetClosest(true);
			}
			return true;
		}

		private void FreezeNPC(NPC npc) {
			npc.target = -1; // Clear aggro target completely
			npc.velocity = Vector2.Zero; // Freeze all movement vectors completely
		}
	}
}