using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Audio;
using SubworldLibrary;
using WarpedAtlas.Content.Items;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Subworlds;
using WarpedAtlas.Content.NPCs;

namespace WarpedAtlas.Content.Tiles
{
    public class MapAltarTile : ModTile
    {
        public override string Texture => "Terraria/Images/Tiles_" + TileID.ElderCrystalStand;

        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Origin = new Point16(1, 3);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(255, 215, 0), name);
        }

        public override bool RightClick(int i, int j) {
            Player player = Main.LocalPlayer;

            int enemiesLeft = 0;
            for (int n = 0; n < Main.maxNPCs; n++) {
                NPC npc = Main.npc[n];
                if (npc.active && !npc.friendly && npc.damage > 0 && npc.lifeMax > 5 &&
                    npc.chaseable && !npc.dontTakeDamage && !npc.immortal) {
                    enemiesLeft++;
                }
            }
            int currentKills = System.Math.Max(0, BaseDungeonSubworld.TotalMonstersSpawned - enemiesLeft);

            if (currentKills < BaseDungeonSubworld.RequiredKillCount) {
                Main.NewText($"  The Altar remains heavily sealed by anomalous entities! Purge more threats first ({currentKills}/{BaseDungeonSubworld.RequiredKillCount} Slain).", 230, 80, 80);
                return true;
            }

            bool guardianActive = false;
            for (int n = 0; n < Main.maxNPCs; n++) {
                if (Main.npc[n].active && Main.npc[n].ModNPC is MapGuardianNPC) {
                    guardianActive = true;
                    break;
                }
            }

            if (guardianActive) {
                Main.NewText("  The Map Guardian blocks access to the Altar! Defeat it to claim your reward!", 255, 100, 100);
                return true;
            }

            if (!BaseDungeonSubworld.BossSummoned) {
                BaseDungeonSubworld.BossSummoned = true;
                Main.spawnTileX = BaseDungeonSubworld.PenultimateRoomX;
                Main.spawnTileY = BaseDungeonSubworld.PenultimateRoomY;

                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    Vector2 spawnPos = new Vector2(i * 16, (j - 8) * 16);
                    int guardianType = ModContent.NPCType<DungeonGuardianNPC>();
                    if (BaseDungeonSubworld.ActiveProfile != null) {
                        string profileKey = BaseDungeonSubworld.ActiveProfile.GetType().Name;
                        guardianType = profileKey switch {
                            "LihzahrdProfile" => ModContent.NPCType<LihzahrdGuardianNPC>(),
                            "SandstoneProfile" => ModContent.NPCType<SandstoneGuardianNPC>(),
                            "HallowedProfile" => ModContent.NPCType<HallowedGuardianNPC>(),
                            "CryoFrostProfile" => ModContent.NPCType<CryoFrostGuardianNPC>(),
                            "HellforgeProfile" => ModContent.NPCType<HellforgeGuardianNPC>(),
                            "GraniteProfile" => ModContent.NPCType<GraniteGuardianNPC>(),
                            "FungalProfile" => ModContent.NPCType<FungalGuardianNPC>(),
                            _ => ModContent.NPCType<DungeonGuardianNPC>()
                        };
                    }

                    int npcIndex = NPC.NewNPC(new EntitySource_TileInteraction(player, i, j), (int)spawnPos.X, (int)spawnPos.Y, guardianType);
                    if (npcIndex >= 0 && npcIndex < Main.maxNPCs) {
                        Main.npc[npcIndex].netUpdate = true;
                        Main.npc[npcIndex].velocity = new Vector2(0f, 4f);
                    }
                }

                SoundEngine.PlaySound(SoundID.Roar, player.Center);
                SoundEngine.PlaySound(SoundID.Item74, player.Center);
                for (int d = 0; d < 36; d++) {
                    Vector2 dustVel = Main.rand.NextVector2Circular(6f, 6f);
                    Dust.NewDust(new Vector2(i * 16, (j - 8) * 16), 32, 32, DustID.PurpleTorch, dustVel.X, dustVel.Y);
                }
                Main.NewText("  The Altar surges with kinetic energy! A Map Guardian descends from above!", 255, 50, 50);
                return true;
            }

            // Mark Zone Completion & Advance Progression
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            int activeAct = prog.ActiveRunAct > 0 ? prog.ActiveRunAct : BaseDungeonSubworld.CurrentMapTier;
            int activeZone = prog.ActiveRunZone > 0 ? prog.ActiveRunZone : BaseDungeonSubworld.CurrentZoneIndex;
            prog.CompleteZone(activeAct, activeZone);

            Main.NewText("  Techcatl Purified! Harvesting loot...", 255, 215, 0);

            Tile tile = Main.tile[i, j];
            int left = i - tile.TileFrameX / 18;
            int top = j - tile.TileFrameY / 18;
            int dropSourceX = left * 16;
            int dropSourceY = top * 16;

            var source = player.GetSource_TileInteraction(i, j);
            Item.NewItem(source, dropSourceX, dropSourceY, 32, 32, ModContent.ItemType<WaystoneBase>());
            Item.NewItem(source, dropSourceX, dropSourceY, 32, 32, ItemID.GoldCoin, Main.rand.Next(3, 8));

            if (Main.rand.NextBool()) {
                Item.NewItem(source, dropSourceX, dropSourceY, 32, 32, ItemID.GreaterHealingPotion, 5);
            }

            for (int x = left; x < left + 3; x++) {
                for (int y = top; y < top + 4; y++) {
                    WorldGen.KillTile(x, y, false, false, true);
                }
            }

            int portalX = left + 1;
            int portalY = top + 3;
            WorldGen.PlaceObject(portalX, portalY, ModContent.TileType<ExtractionPortalTile>(), mute: false);

            for (int x = portalX - 2; x <= portalX + 2; x++) {
                for (int y = portalY - 4; y <= portalY; y++) {
                    WorldGen.SquareTileFrame(x, y, true);
                }
            }

            Main.NewText("  An extraction rift has stabilized nearby. Step through when ready.", 0, 191, 255);
            return true;
        }

        public override void MouseOver(int i, int j) {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<WaystoneBase>();
            player.noThrow = 2;
        }
    }
}