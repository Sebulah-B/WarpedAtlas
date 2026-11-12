using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Items;
using WarpedAtlas.Content.Items.Currencies;
using WarpedAtlas.Content.Items.Weapons;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Subworlds;
using WarpedAtlas.Content.Systems;
using WarpedAtlas.Content.Items.Extraction;

namespace WarpedAtlas.Content.NPCs
{
    public class EscalationAnchorNPC : ModNPC
    {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.LunarTowerNebula;

        public int State {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        public ref float PressureTimer => ref NPC.ai[1];
        public ref float WaveSpawnTimer => ref NPC.ai[2];

        public override void SetStaticDefaults() {
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.LunarTowerNebula];
            NPCID.Sets.ActsLikeTownNPC[Type] = true;
            NPCID.Sets.NoTownNPCHappiness[Type] = true;
        }

        public override void SetDefaults() {
            NPC.width = 48;
            NPC.height = 96;
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.defense = 9999;
            NPC.lifeMax = 10000;
            NPC.friendly = true;
            NPC.dontTakeDamage = true;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.scale = 0.75f;
        }

        public override bool CanChat() => true;

        public override bool CheckActive() => false;

        public override void AI() {
            NPC.velocity.X = 0f;
            NPC.velocity.Y = (float)Math.Sin(Main.GameUpdateCount * 0.05f) * 0.35f;

            Lighting.AddLight(NPC.Center, 0.8f, 0.3f, 0.9f);

            Player localPlayer = Main.LocalPlayer;
            if (localPlayer != null && localPlayer.active && localPlayer.talkNPC == NPC.whoAmI) {
                localPlayer.SetTalkNPC(-1);
                Main.npcChatText = "";
                ModContent.GetInstance<AtlasUISystem>().OpenEscalationUI(this);
            }

            if (localPlayer != null && localPlayer.active && NPC.Hitbox.Contains(Main.MouseWorld.ToPoint())) {
                localPlayer.cursorItemIconEnabled = true;
                localPlayer.cursorItemIconID = ModContent.ItemType<ChaosOrb>();
                localPlayer.noThrow = 2;
            }

            if (State == 1) {
                PressureTimer++;
                if (PressureTimer >= 600f) {
                    PressureTimer = 0f;
                    if (BaseDungeonSubworld.CurrentPressureLevel < 15) {
                        BaseDungeonSubworld.CurrentPressureLevel++;
                        SoundEngine.PlaySound(SoundID.Item119, NPC.Center);
                        Main.NewText($"  [Temporal Escalation] Pressure Level increased to {BaseDungeonSubworld.CurrentPressureLevel}!", 255, 120, 255);
                    }
                }

                WaveSpawnTimer++;
                int spawnInterval = Math.Max(75, 180 - (BaseDungeonSubworld.CurrentPressureLevel * 6));
                if (WaveSpawnTimer >= spawnInterval) {
                    WaveSpawnTimer = 0f;
                    SpawnEscalationWave();
                }

                if (Main.rand.NextBool(2)) {
                    Vector2 offset = Main.rand.NextVector2CircularEdge(56f, 56f);
                    Dust d = Dust.NewDustPerfect(NPC.Center + offset, DustID.Shadowflame, -offset * 0.06f, 100, Color.Violet, 1.6f);
                    d.noGravity = true;
                }
            }
        }

        private void SpawnEscalationWave() {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            var profile = BaseDungeonSubworld.ActiveProfile ?? new DungeonProfile();
            if (profile.EnemyPool == null || profile.EnemyPool.Count == 0) return;

            int activeHostilesNearby = 0;
            for (int i = 0; i < Main.maxNPCs; i++) {
                if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].Distance(NPC.Center) < 600f) {
                    activeHostilesNearby++;
                }
            }
            if (activeHostilesNearby >= 14) return;

            int countToSpawn = Main.rand.Next(2, 4);
            var source = NPC.GetSource_FromAI();

            for (int s = 0; s < countToSpawn; s++) {
                float offsetX = Main.rand.NextBool() ? Main.rand.NextFloat(-240f, -90f) : Main.rand.NextFloat(90f, 240f);
                Vector2 spawnPos = NPC.Center + new Vector2(offsetX, -16f);

                Point tilePt = spawnPos.ToTileCoordinates();
                for (int y = tilePt.Y; y < tilePt.Y + 12; y++) {
                    if (WorldGen.InWorld(tilePt.X, y) && Main.tile[tilePt.X, y].HasTile && Main.tileSolid[Main.tile[tilePt.X, y].TileType]) {
                        spawnPos.Y = (y - 2) * 16f;
                        break;
                    }
                }

                int npcType = profile.EnemyPool[Main.rand.Next(profile.EnemyPool.Count)];
                int idx = NPC.NewNPC(source, (int)spawnPos.X, (int)spawnPos.Y, npcType);
                if (idx >= 0 && idx < Main.maxNPCs) {
                    Main.npc[idx].netUpdate = true;
                    BaseDungeonSubworld.TotalMonstersSpawned++;
                    for (int d = 0; d < 15; d++) {
                        Dust.NewDust(Main.npc[idx].position, Main.npc[idx].width, Main.npc[idx].height, DustID.PurpleTorch, 0f, -2f, 100, default, 1.5f);
                    }
                }
            }
        }

        public void ExtractAndReward() {
            int achievedPressure = BaseDungeonSubworld.CurrentPressureLevel;
            BaseDungeonSubworld.CurrentPressureLevel = 0;
            State = 0;

            Player player = Main.LocalPlayer;
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            int tier = prog.ActiveRunAct > 0 ? prog.ActiveRunAct : BaseDungeonSubworld.CurrentMapTier;
            string pClass = prog.ChosenClass;

            int toolkitWeaponId = pClass switch {
                "Warrior" => tier >= 7
                    ? ModContent.ItemType<CataclysmBellowCry>()
                    : (tier >= 4 ? ModContent.ItemType<ObsidianRoarCry>() : ModContent.ItemType<SeismicCry>()),
                "Ranger" => tier >= 7
                    ? ModContent.ItemType<ChronoSingularityTrap>()
                    : (tier >= 4 ? ModContent.ItemType<SolarClusterTrap>() : ModContent.ItemType<ShrapnelTrap>()),
                "Sorcerer" => tier >= 7
                    ? ModContent.ItemType<OllinSingularityConduit>()
                    : ModContent.ItemType<AethericRiftConduit>(),
                "Summoner" => tier >= 7
                    ? ModContent.ItemType<TzitzimitlEclipseIdol>()
                    : (tier >= 4 ? ModContent.ItemType<JadeEffigyIdol>() : ModContent.ItemType<MartyrsIdol>()),
                _ => ModContent.ItemType<SeismicCry>()
            };

            int utilityItemId = tier switch {
                1 => ItemID.HermesBoots,
                2 => ItemID.AmethystHook,
                3 => ItemID.IvyWhip,
                4 => ItemID.CreativeWings,
                5 => ItemID.HarpyWings,
                6 => ItemID.FlameWings,
                7 => ItemID.Hoverboard,
                _ => ItemID.FishronWings
            };

            Point anchorTile = NPC.Center.ToTileCoordinates();
            int chestX = anchorTile.X;
            int floorY = anchorTile.Y;

            for (int y = anchorTile.Y; y < anchorTile.Y + 25; y++) {
                if (WorldGen.InWorld(chestX, y) && Main.tile[chestX, y].HasTile && Main.tileSolid[Main.tile[chestX, y].TileType]) {
                    floorY = y;
                    break;
                }
            }

            for (int x = chestX; x <= chestX + 1; x++) {
                if (WorldGen.InWorld(x, floorY)) {
                    Tile ft = Main.tile[x, floorY];
                    ft.HasTile = true;
                    ft.TileType = BaseDungeonSubworld.ActiveProfile != null ? BaseDungeonSubworld.ActiveProfile.TileId : TileID.BlueDungeonBrick;
                    ft.IsHalfBlock = false;
                    ft.Slope = SlopeType.Solid;
                }
                for (int y = floorY - 2; y <= floorY - 1; y++) {
                    if (WorldGen.InWorld(x, y)) {
                        Tile ct = Main.tile[x, y];
                        ct.HasTile = false;
                    }
                }
            }

            int chestIndex = WorldGen.PlaceChest(chestX, floorY - 1, TileID.Containers, false, 1);

            if (chestIndex == -1) {
                for (int x = 0; x < 2; x++) {
                    for (int y = 0; y < 2; y++) {
                        int tx = chestX + x;
                        int ty = floorY - 2 + y;
                        if (WorldGen.InWorld(tx, ty)) {
                            Tile t = Main.tile[tx, ty];
                            t.HasTile = true;
                            t.TileType = TileID.Containers;
                            t.TileFrameX = (short)(1 * 36 + x * 18);
                            t.TileFrameY = (short)(y * 18);
                        }
                    }
                }
                chestIndex = Chest.CreateChest(chestX, floorY - 2);
            }

            var dropSource = NPC.GetSource_Loot();
            if (chestIndex >= 0 && chestIndex < Main.maxChests && Main.chest[chestIndex] != null) {
                Chest chest = Main.chest[chestIndex];
                int slot = 0;

                chest.item[slot].SetDefaults(toolkitWeaponId);
                slot++;

                chest.item[slot].SetDefaults(utilityItemId);
                slot++;

                chest.item[slot].SetDefaults(ModContent.ItemType<AscendancyRelic>());
                slot++;

                if (achievedPressure >= 5) {
                    chest.item[slot].SetDefaults(ModContent.ItemType<OrbOfAlchemy>());
                    chest.item[slot].stack = Math.Max(1, achievedPressure / 3);
                    slot++;
                }
                if (achievedPressure >= 10) {
                    chest.item[slot].SetDefaults(ModContent.ItemType<ChaosOrb>());
                    chest.item[slot].stack = Math.Max(1, achievedPressure / 4);
                    slot++;
                }
                if (achievedPressure >= 15) {
                    chest.item[slot].SetDefaults(ModContent.ItemType<ExaltedOrb>());
                    chest.item[slot].stack = 2;
                    slot++;
                }
            } else {
                Item.NewItem(dropSource, NPC.getRect(), toolkitWeaponId);
                Item.NewItem(dropSource, NPC.getRect(), utilityItemId);
                Item.NewItem(dropSource, NPC.getRect(), ModContent.ItemType<AscendancyRelic>());
                if (achievedPressure >= 5) Item.NewItem(dropSource, NPC.getRect(), ModContent.ItemType<OrbOfAlchemy>(), Math.Max(1, achievedPressure / 3));
                if (achievedPressure >= 10) Item.NewItem(dropSource, NPC.getRect(), ModContent.ItemType<ChaosOrb>(), Math.Max(1, achievedPressure / 4));
                if (achievedPressure >= 15) Item.NewItem(dropSource, NPC.getRect(), ModContent.ItemType<ExaltedOrb>(), 2);
            }

            SoundEngine.PlaySound(SoundID.Item4, NPC.Center);
            for (int i = 0; i < 40; i++) {
                Vector2 v = Main.rand.NextVector2Circular(6f, 6f);
                Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.GoldFlame, v.X, v.Y, 100, default, 2f);
                d.noGravity = true;
            }

            Main.NewText($"  [Temporal Escalation] Extraction Cache manifested! (Cleared at Pressure {achievedPressure})", 255, 215, 0);
            NPC.active = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Main.instance.LoadNPC(NPCID.LunarTowerNebula);
            Texture2D tex = TextureAssets.Npc[NPCID.LunarTowerNebula].Value;
            int totalFrames = Main.npcFrameCount[NPCID.LunarTowerNebula];
            int frameHeight = tex.Height / totalFrames;
            int currentFrame = (int)(Main.GameUpdateCount / 6) % totalFrames;
            Rectangle sourceRect = new Rectangle(0, currentFrame * frameHeight, tex.Width, frameHeight);

            Vector2 origin = sourceRect.Size() * 0.5f;
            Vector2 drawPos = NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY);

            Color auraColor = State == 1 ? new Color(255, 80, 255, 160) : new Color(160, 100, 255, 120);
            spriteBatch.Draw(tex, drawPos, sourceRect, auraColor, 0f, origin, NPC.scale * 0.55f, SpriteEffects.None, 0f);
            spriteBatch.Draw(tex, drawPos, sourceRect, Color.White, 0f, origin, NPC.scale * 0.48f, SpriteEffects.None, 0f);
            return false;
        }
    }
}