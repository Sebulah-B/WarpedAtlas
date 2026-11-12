using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;
using WarpedAtlas.Content.Items;
using WarpedAtlas.Content.Subworlds;
using WarpedAtlas.Content.NPCs;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Tiles;

namespace WarpedAtlas.Content.Systems
{
    public class GlobalMapDropper : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            if (npc.friendly || npc.lifeMax <= 5) return;
            var dropRule = ItemDropRule.Common(ModContent.ItemType<WaystoneBase>(), chanceDenominator: 20);
            var hardmodeCondition = new Conditions.IsHardmode();
            npcLoot.Add(new LeadingConditionRule(hardmodeCondition));
            npcLoot.Add(dropRule);
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
            if (SubworldSystem.IsActive<BaseDungeonSubworld>() ||
                SubworldSystem.IsActive<BossDomainSubworld>() ||
                SubworldSystem.Current == null) {
                spawnRate = 0;
                maxSpawns = 0;
            }
        }

        public override bool CheckActive(NPC npc) {
            if (SubworldSystem.IsActive<BaseDungeonSubworld>() || SubworldSystem.IsActive<BossDomainSubworld>()) {
                return false;
            }
            return base.CheckActive(npc);
        }

        public override void OnKill(NPC npc) {
            int defeatedBossTier = 0;
            if (npc.type == ModContent.NPCType<ApexSovereignNPC>()) defeatedBossTier = 1;
            else if (npc.type == ModContent.NPCType<ApexColossusNPC>()) defeatedBossTier = 2;
            else if (npc.type == ModContent.NPCType<ApexPharaohNPC>()) defeatedBossTier = 3;
            else if (npc.type == ModContent.NPCType<ApexSeraphNPC>()) defeatedBossTier = 4;
            else if (npc.type == ModContent.NPCType<ApexCryoTitanNPC>()) defeatedBossTier = 5;
            else if (npc.type == ModContent.NPCType<ApexInfernalNPC>()) defeatedBossTier = 6;
            else if (npc.type == ModContent.NPCType<ApexGraniteCoreNPC>()) defeatedBossTier = 7;
            else if (npc.type == ModContent.NPCType<ApexFungalLordNPC>()) defeatedBossTier = 8;

            if (defeatedBossTier > 0) {
                for (int p = 0; p < Main.maxPlayers; p++) {
                    Player plr = Main.player[p];
                    if (plr != null && plr.active) {
                        var prog = plr.GetModPlayer<AtlasProgressionPlayer>();
                        prog.CompleteZone(defeatedBossTier, 4);
                    }
                }

                // Spawn Extraction Portal on the Boss Arena floor so the player can extract & save cleanly
                int portalX = Main.maxTilesX / 2;
                int portalY = (Main.maxTilesY / 2) + 40 - 1;
                if (npc.ai[2] > 0 && npc.ai[3] > 0) {
                    portalX = (int)npc.ai[2] + 1;
                    portalY = (int)npc.ai[3] + 1;
                }

                // Clear space and place the 3x4 Extraction Portal
                for (int x = portalX - 2; x <= portalX + 2; x++) {
                    for (int y = portalY - 4; y <= portalY; y++) {
                        if (WorldGen.InWorld(x, y)) {
                            Tile t = Main.tile[x, y];
                            if (t.TileType != TileID.Platforms && y < portalY) {
                                t.HasTile = false;
                            }
                        }
                    }
                }

                bool placed = WorldGen.PlaceObject(portalX, portalY, ModContent.TileType<ExtractionPortalTile>(), mute: false);
                if (!placed) {
                    ushort portalType = (ushort)ModContent.TileType<ExtractionPortalTile>();
                    for (int x = 0; x < 3; x++) {
                        for (int y = 0; y < 4; y++) {
                            int tx = portalX - 1 + x;
                            int ty = portalY - 3 + y;
                            if (WorldGen.InWorld(tx, ty)) {
                                Tile t = Main.tile[tx, ty];
                                t.HasTile = true;
                                t.TileType = portalType;
                                t.TileFrameX = (short)(x * 18);
                                t.TileFrameY = (short)(y * 18);
                            }
                        }
                    }
                }

                Main.NewText("  An Extraction Rift has materialized in the center of the arena!", 0, 191, 255);
            }
        }
    }
}