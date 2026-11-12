using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content; 
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.UI
{
    public class AtlasBossBar : ModBossBar
    {
        public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame) {
            Main.instance.LoadItem(ItemID.TempleKey);
            return TextureAssets.Item[ItemID.TempleKey];
        }

        // Changed return type from bool to bool?
        public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax) {
            NPC npc = Main.npc[info.npcIndexToAimAt];
            
            if (!npc.active || npc.lifeMax <= 5) return false;

            life = npc.life;
            lifeMax = npc.lifeMax;

            return true;
        }
    }
}