using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Systems;

namespace WarpedAtlas.Content.UI
{
    public class AtlasPassiveUI : UIState
    {
        private Vector2 panOffset;
        private Vector2 dragStart;
        private bool isDragging;
        private bool wasClick;
        private PassiveNode hoveredNode;
        public static float Zoom = 1f;

        public override void OnInitialize() {
            panOffset = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
        }

        public override void OnActivate() {
            base.OnActivate();
            Main.playerInventory = false; 
            Zoom = 1f;

            var prog = Main.LocalPlayer.GetModPlayer<AtlasProgressionPlayer>();
            int rootId = prog.ChosenClass switch {
                "Warrior" => 1000,
                "Ranger" => 2000,
                "Sorcerer" => 3000,
                "Summoner" => 4000,
                _ => 0
            };

            // Anchor camera directly to the active class origin to prevent 3-screen drag punishment
            if (PassiveTreeRegistry.Nodes.TryGetValue(rootId, out var rootNode)) {
                Vector2 screenCenter = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
                panOffset = screenCenter - (rootNode.Position * Zoom);
            } else {
                panOffset = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
            }
        }

        public override void ScrollWheel(UIScrollWheelEvent evt) {
            base.ScrollWheel(evt);
            float oldZoom = Zoom;
            Zoom += evt.ScrollWheelValue > 0 ? 0.1f : -0.1f;
            Zoom = MathHelper.Clamp(Zoom, 0.15f, 2.0f); // Expanded minimum zoom to 0.15f for 4800px canvases
            
            Vector2 mousePos = new Vector2(Main.mouseX, Main.mouseY);
            Vector2 mouseWorld = (mousePos - panOffset) / oldZoom;
            panOffset = mousePos - (mouseWorld * Zoom);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            Main.LocalPlayer.mouseInterface = true;
            if (Main.LocalPlayer.controlInv) {
                ModContent.GetInstance<AtlasUISystem>().AtlasPassiveInterface.SetState(null);
                SoundEngine.PlaySound(SoundID.MenuClose);
                Main.LocalPlayer.controlInv = false; 
            }
        }

        private void DrawConnectingLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color, float thickness) {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            Vector2 scale = new Vector2(edge.Length(), thickness);
            sb.Draw(TextureAssets.MagicPixel.Value, start, new Rectangle(0, 0, 1, 1), color, angle, new Vector2(0, 0.5f), scale, SpriteEffects.None, 0f);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            Main.LocalPlayer.mouseInterface = true;
            
            Player player = Main.LocalPlayer;
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();

            if (Main.mouseLeft) {
                if (!isDragging) {
                    isDragging = true;
                    wasClick = true; 
                    dragStart = Main.MouseScreen - panOffset;
                }
                panOffset = Main.MouseScreen - dragStart;
                if (Vector2.Distance(Main.MouseScreen, dragStart + panOffset) > 5f) wasClick = false;
            } else {
                if (isDragging && wasClick && hoveredNode != null) AttemptAllocateNode(prog, hoveredNode);
                isDragging = false;
            }

            if (Main.mouseRight && Main.mouseRightRelease) {
                ModContent.GetInstance<AtlasUISystem>().AtlasPassiveInterface.SetState(null);
                SoundEngine.PlaySound(SoundID.MenuClose);
                return;
            }

            hoveredNode = null;
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            spriteBatch.Draw(pixel, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(10, 15, 20, 240));

            // Shifted visibility constraints to the new 1,000-block scale
            int minId = 0, maxId = 9999;
            switch (prog.ChosenClass) {
                case "Warrior": minId = 1000; maxId = 1999; break;
                case "Ranger": minId = 2000; maxId = 2999; break;
                case "Sorcerer": minId = 3000; maxId = 3999; break;
                case "Summoner": minId = 4000; maxId = 4999; break;
            }

            // 1. DRAW CONNECTIONS
            foreach (var kvp in PassiveTreeRegistry.Nodes) {
                PassiveNode node = kvp.Value;
                if (node.Id < minId || node.Id > maxId) continue;

                Vector2 nodePos = (node.Position * Zoom) + panOffset;
                foreach (int connectedId in node.ConnectedNodes) {
                    if (connectedId < minId || connectedId > maxId) continue;
                    if (connectedId > node.Id) { 
                        PassiveNode targetNode = PassiveTreeRegistry.Nodes[connectedId];
                        Vector2 targetPos = (targetNode.Position * Zoom) + panOffset;

                        Color lineColor = (prog.AllocatedNodeIds.Contains(node.Id) && prog.AllocatedNodeIds.Contains(targetNode.Id)) 
                            ? new Color(200, 160, 50, 255) : new Color(60, 60, 60, 120);
                        DrawConnectingLine(spriteBatch, nodePos, targetPos, lineColor, 6f * Zoom);
                    }
                }
            }

            // 2. DRAW SOLID NODES
            foreach (var kvp in PassiveTreeRegistry.Nodes) {
                PassiveNode node = kvp.Value;
                if (node.Id < minId || node.Id > maxId) continue;
                
                Vector2 nodePos = (node.Position * Zoom) + panOffset;
                if (nodePos.X < -50 || nodePos.X > Main.screenWidth + 50 || nodePos.Y < -50 || nodePos.Y > Main.screenHeight + 50) continue;

                // LoadItem has been shifted to AddNode() for efficiency.
                Texture2D iconTex = TextureAssets.Item[node.IconItemId].Value;
                
                int frameCount = Main.itemAnimations[node.IconItemId] != null ? Main.itemAnimations[node.IconItemId].FrameCount : 1;
                Rectangle sourceRect = iconTex.Frame(1, frameCount, 0, 0);

                bool isAllocated = prog.AllocatedNodeIds.Contains(node.Id);
                bool canAllocate = CanAllocate(prog, node.Id);

                Color iconColor; Color borderColor;
                if (isAllocated) { iconColor = Color.White; borderColor = new Color(255, 215, 0, 255); } 
                else if (canAllocate) { iconColor = new Color(200, 200, 200, 255); borderColor = new Color(100, 255, 100, 255); } 
                else { iconColor = new Color(80, 80, 80, 100); borderColor = new Color(40, 40, 40, 100); }

                float baseNodeRadius = node.Type == NodeType.Keystone ? 24f : 16f;
                float nodeSize = baseNodeRadius * Zoom;
                Rectangle nodeRect = new Rectangle((int)(nodePos.X - nodeSize), (int)(nodePos.Y - nodeSize), (int)(nodeSize * 2), (int)(nodeSize * 2));

                if (nodeRect.Contains(Main.mouseX, Main.mouseY)) {
                    hoveredNode = node;
                    borderColor = Color.Yellow; 
                }

                Rectangle bgRect = new Rectangle((int)(nodePos.X - nodeSize - (4 * Zoom)), (int)(nodePos.Y - nodeSize - (4 * Zoom)), (int)((nodeSize * 2) + (8 * Zoom)), (int)((nodeSize * 2) + (8 * Zoom)));
                spriteBatch.Draw(pixel, bgRect, borderColor); 
                spriteBatch.Draw(pixel, new Rectangle(bgRect.X + 2, bgRect.Y + 2, bgRect.Width - 4, bgRect.Height - 4), new Color(15, 20, 30, 255)); 
                
                float maxIconDim = baseNodeRadius * 1.5f; 
                float largestTexDim = Math.Max(sourceRect.Width, sourceRect.Height);
                float iconScale = (maxIconDim / largestTexDim) * Zoom;
                if (iconScale > 1.3f * Zoom) iconScale = 1.3f * Zoom; 
                Vector2 origin = sourceRect.Size() / 2f;
                
                spriteBatch.Draw(iconTex, nodePos, sourceRect, iconColor, 0f, origin, iconScale, SpriteEffects.None, 0f);
            }

            // 3. DRAW HUD
            string pointText = $"Class: {prog.ChosenClass} | Passive Points: {prog.TotalPassivePoints}";
            Utils.DrawBorderStringBig(spriteBatch, pointText, new Vector2(20, 20), prog.TotalPassivePoints > 0 ? Color.Gold : Color.Gray);
            Utils.DrawBorderString(spriteBatch, "Right-Click to Exit | Scroll to Zoom", new Vector2(20, 60), Color.LightGray);

            // 4. DRAW TOOLTIP
            if (hoveredNode != null) {
                string status = prog.AllocatedNodeIds.Contains(hoveredNode.Id) ? "[Allocated]" : CanAllocate(prog, hoveredNode.Id) ? "[Click to Allocate]" : "[Locked]";
                string tooltip = $"{hoveredNode.Name}\n{hoveredNode.Description}\n{status}";
                Utils.DrawBorderString(spriteBatch, tooltip, new Vector2(Main.mouseX + 20, Main.mouseY + 20), Color.White);
            }
        }

        private bool CanAllocate(AtlasProgressionPlayer prog, int nodeId) {
            if (prog.AllocatedNodeIds.Contains(nodeId)) return false; 
            foreach (int connectedId in PassiveTreeRegistry.Nodes[nodeId].ConnectedNodes) {
                if (prog.AllocatedNodeIds.Contains(connectedId)) return true;
            }
            return false;
        }

        private void AttemptAllocateNode(AtlasProgressionPlayer prog, PassiveNode node) {
            if (prog.TotalPassivePoints > 0 && CanAllocate(prog, node.Id)) {
                prog.AllocatedNodeIds.Add(node.Id);
                prog.TotalPassivePoints--;
                SoundEngine.PlaySound(SoundID.Unlock);
            } else if (prog.TotalPassivePoints <= 0 && CanAllocate(prog, node.Id)) {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Main.NewText("Not enough Passive Points!", Color.Red);
            }
        }
    }
}