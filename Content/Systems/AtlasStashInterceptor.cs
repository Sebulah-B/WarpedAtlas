using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Systems
{
    public class AtlasStashInterceptor : GlobalTile
    {
        public override void RightClick(int i, int j, int type) {
            if (type == TileID.DefendersForge) {
                Player player = Main.LocalPlayer;

                // Prevent vanilla Defender's Forge bank from overriding our UI
                Main.mouseRightRelease = false;
                player.chest = -1;
                player.tileEntityAnchor.Clear();
                player.CloseSign();
                player.SetTalkNPC(-1);
                Main.npcChatCornerItem = 0;
                Main.npcChatText = "";
                Recipe.FindRecipes();

                SoundEngine.PlaySound(SoundID.MenuOpen);
                var uiSystem = ModContent.GetInstance<AtlasUISystem>();
                uiSystem.OpenNativeStashUI();
            }
        }

        public override void MouseOver(int i, int j, int type) {
            if (type == TileID.DefendersForge) {
                Player player = Main.LocalPlayer;
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = ItemID.DefendersForge;
                player.noThrow = 2;
                player.cursorItemIconText = "Open Sacred Vault (100 Slots)";
            }
        }
    }
}