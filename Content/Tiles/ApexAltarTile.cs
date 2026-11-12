using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Audio;
using WarpedAtlas.Content.Subworlds;
using WarpedAtlas.Content.NPCs;

namespace WarpedAtlas.Content.Tiles
{
    public class ApexAltarTile : ModTile
    {
        public override string Texture => "Terraria/Images/Tiles_" + TileID.DemonAltar;
                 
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(255, 50, 50), CreateMapEntryName());
        }

        public override bool RightClick(int i, int j) {
            for (int n = 0; n < Main.maxNPCs; n++) {
                if (Main.npc[n].active && Main.npc[n].boss) return true;
            }

            // Mathematically calculate the top-left corner of the 3x2 Altar
            Tile tile = Main.tile[i, j];
            int left = i - (tile.TileFrameX / 18);
            int top = j - (tile.TileFrameY / 18);

            if (Main.netMode != NetmodeID.MultiplayerClient) {
                int tier = BaseDungeonSubworld.CurrentMapTier;
                int bossId = tier switch {
                    1 => ModContent.NPCType<ApexSovereignNPC>(),
                    2 => ModContent.NPCType<ApexColossusNPC>(),
                    3 => ModContent.NPCType<ApexPharaohNPC>(),
                    4 => ModContent.NPCType<ApexSeraphNPC>(),
                    5 => ModContent.NPCType<ApexCryoTitanNPC>(),
                    6 => ModContent.NPCType<ApexInfernalNPC>(),
                    7 => ModContent.NPCType<ApexGraniteCoreNPC>(),
                    8 => ModContent.NPCType<ApexFungalLordNPC>(),
                    _ => ModContent.NPCType<ApexSovereignNPC>()
                };
                
                int npcIndex = NPC.NewNPC(new Terraria.DataStructures.EntitySource_TileInteraction(Main.LocalPlayer, left, top), left * 16, (top - 10) * 16, bossId);
                if (npcIndex >= 0 && npcIndex < Main.maxNPCs) {
                    Main.npc[npcIndex].netUpdate = true;
                    
                    // Store the uniform top-left anchor in the Boss's memory
                    Main.npc[npcIndex].ai[2] = left;
                    Main.npc[npcIndex].ai[3] = top;
                }
            }
            
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, new Microsoft.Xna.Framework.Vector2(i * 16, j * 16));
            Main.NewText("  The Apex Boss has been awakened! There is no escape.", 255, 50, 50);

            // Instantly shatter the altar to clear the space for the fight and future portal
            WorldGen.KillTile(i, j, false, false, true);
            if (Main.netMode == NetmodeID.Server) {
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
            }

            return true;
        }
    }
}