using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using SubworldLibrary;
using WarpedAtlas.Content.Subworlds;

namespace WarpedAtlas.Content.UI
{
    public class AtlasMonsterHUD : UIState
    {
        private UIText counterText;

        public override void OnInitialize() {
            counterText = new UIText("Threats Remaining: 0/0", 1f, false);
            counterText.Left.Set(-350f, 1f); 
            counterText.Top.Set(150f, 0f);  
            counterText.TextColor = Color.Salmon;
            Append(counterText);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (!SubworldSystem.IsActive<BaseDungeonSubworld>()) 
                return;

            int enemiesLeft = 0;
            for (int i = 0; i < Main.maxNPCs; i++) {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.damage > 0 && npc.lifeMax > 5 && 
                    npc.chaseable && !npc.dontTakeDamage && !npc.immortal) 
                {
                    enemiesLeft++;
                }
            }

            int currentKills = Math.Max(0, BaseDungeonSubworld.TotalMonstersSpawned - enemiesLeft);
            int required = BaseDungeonSubworld.RequiredKillCount;

            if (currentKills < required) {
                counterText.SetText($"  Purging Vault: {currentKills}/{required} Slain");
                counterText.TextColor = Color.OrangeRed;
            } else {
                counterText.SetText("  Clearance Secured! Altar Stabilized.");
                counterText.TextColor = Color.Cyan;

                // DYNAMIC SEAL REMOVAL MECHANIC
                if (!BaseDungeonSubworld.GateHasOpened && BaseDungeonSubworld.GateWallX > 0) {
                    BaseDungeonSubworld.GateHasOpened = true;

                    int aX = BaseDungeonSubworld.GateWallX;
                    int aY = BaseDungeonSubworld.GateFloorY;

                    // Expanded deletion radius to cleanly shatter the entire box structure[cite: 1]
                    for (int gx = aX - 6; gx <= aX + 6; gx++) {
                        for (int gy = aY - 7; gy <= aY + 3; gy++) {
                            Tile tile = Main.tile[gx, gy];
                            if (tile.HasTile && tile.TileType == TileID.ActiveStoneBlock) {
                                tile.HasTile = false;
                            }
                        }
                    }

                    if (Main.netMode != NetmodeID.Server) {
                        SoundEngine.PlaySound(SoundID.Unlock, new Vector2(aX * 16, aY * 16));
                        WorldGen.SectionTileFrame(aX - 7, aY - 8, aX + 7, aY + 4);
                        Main.NewText("  An echo resonates through the vault... The sealed barrier stone has crumbled!", 100, 255, 100);
                    }
                }
            }
        }
    }
}