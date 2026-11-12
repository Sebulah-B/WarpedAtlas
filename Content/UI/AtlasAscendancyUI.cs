using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items;

namespace WarpedAtlas.Content.UI
{
    public class AtlasAscendancyUI : UIState
    {
        private readonly string[] SlotLabels = { "Ultimate (North)", "Offense (East)", "Utility (South)", "Defense (West)" };

        public override void Draw(SpriteBatch spriteBatch) {
            Player player = Main.LocalPlayer;
            var modPlayer = player.GetModPlayer<AtlasProgressionPlayer>();

            Rectangle panelRect = new Rectangle(Main.screenWidth / 2 - 280, Main.screenHeight / 2 - 230, 560, 460);

            if (panelRect.Contains(Main.MouseScreen.ToPoint())) {
                player.mouseInterface = true;
            }

            // 1. Draw Gradient Backdrop matching Cold/Diamond or Class Theme
            Color topGradient = modPlayer.HasFrigidGlimmerInUltimateSlot() ? new Color(15, 45, 85, 245) : new Color(25, 20, 45, 245);
            Color bottomGradient = modPlayer.HasFrigidGlimmerInUltimateSlot() ? new Color(5, 12, 28, 250) : new Color(10, 10, 18, 250);
            DrawGradientBox(spriteBatch, panelRect, topGradient, bottomGradient, new Color(100, 200, 255));

            // 2. Title & Instructions
            Vector2 center = panelRect.Center.ToVector2();
            Utils.DrawBorderStringBig(spriteBatch, "ASCENDANCY MATRIX", new Vector2(center.X, panelRect.Y + 38), new Color(150, 230, 255), 0.6f, 0.5f, 0.5f);
            Utils.DrawBorderString(spriteBatch, "Slot Ascendancy Jewels | Press 'V' to Unleash North Ultimate", new Vector2(center.X, panelRect.Bottom - 28), Color.LightGray, 0.85f, 0.5f, 0.5f);

            // 3. 4-Jewel Socket Positions (0: North, 1: East, 2: South, 3: West)
            Vector2[] slotCenters = {
                center + new Vector2(0, -110), // 0: North (Ultimate)
                center + new Vector2(130, 10), // 1: East (Offense)
                center + new Vector2(0, 130),  // 2: South (Utility)
                center + new Vector2(-130, 10) // 3: West (Defense)
            };

            // Connecting lines to center crest
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            foreach (Vector2 pos in slotCenters) {
                Utils.DrawLine(spriteBatch, center + new Vector2(0, 10), pos, new Color(80, 160, 220, 120), new Color(140, 220, 255, 180), 3f);
            }

            // Center Class Crest
            Rectangle crestRect = new Rectangle((int)center.X - 28, (int)center.Y - 18, 56, 56);
            DrawGradientBox(spriteBatch, crestRect, new Color(40, 80, 120), new Color(20, 40, 70), new Color(180, 240, 255));

            // 4. Draw and Handle the 4 Jewel Sockets
            int hoveredSlot = -1;
            for (int i = 0; i < 4; i++) {
                Rectangle slotRect = new Rectangle((int)slotCenters[i].X - 30, (int)slotCenters[i].Y - 30, 60, 60);
                bool isHovered = slotRect.Contains(Main.MouseScreen.ToPoint());
                if (isHovered) hoveredSlot = i;

                Color borderColor = (i == 0) ? new Color(120, 235, 255) : new Color(120, 140, 180);
                if (isHovered) borderColor = Color.Gold;

                DrawGradientBox(spriteBatch, slotRect, new Color(20, 35, 60, 230), new Color(10, 18, 32, 230), borderColor);
                Utils.DrawBorderString(spriteBatch, SlotLabels[i], new Vector2(slotCenters[i].X, slotRect.Y - 14), borderColor, 0.75f, 0.5f, 0.5f);

                Item slottedItem = modPlayer.AscendancyLoadout[i];
                if (slottedItem != null && !slottedItem.IsAir) {
                    Main.instance.LoadItem(slottedItem.type);
                    Texture2D itemTex = TextureAssets.Item[slottedItem.type].Value;
                    spriteBatch.Draw(itemTex, slotCenters[i], null, Color.White, 0f, itemTex.Size() / 2f, 1.35f, SpriteEffects.None, 0f);
                }

                // Handle Left-Click Socketing / Unsocketing
                if (isHovered && Main.mouseLeft && Main.mouseLeftRelease) {
                    HandleSlotClick(modPlayer, i);
                }
            }

            // 5. Render Tooltip on Hover
            if (hoveredSlot != -1) {
                Item item = modPlayer.AscendancyLoadout[hoveredSlot];
                if (item != null && !item.IsAir) {
                    Main.HoverItem = item.Clone();
                    Main.hoverItemName = item.Name;
                } else {
                    Main.instance.MouseText($"Empty {SlotLabels[hoveredSlot]} Socket\nClick with an Ascendancy Jewel while inventory is open");
                }
            }
        }

        private void HandleSlotClick(AtlasProgressionPlayer modPlayer, int slotIndex) {
            Item mouseItem = Main.mouseItem;
            Item currentSlotted = modPlayer.AscendancyLoadout[slotIndex];

            // Allow placing an AscendancyJewelItem or swapping/removing with an empty cursor
            if (mouseItem.IsAir && currentSlotted != null && !currentSlotted.IsAir) {
                Main.playerInventory = true;
                Main.mouseItem = currentSlotted.Clone();
                modPlayer.AscendancyLoadout[slotIndex].TurnToAir();
                SoundEngine.PlaySound(SoundID.Grab);
            }
            else if (!mouseItem.IsAir && mouseItem.ModItem is AscendancyJewelItem jewel && jewel.CanSocketInSlot(slotIndex)) {
                Item temp = currentSlotted != null && !currentSlotted.IsAir ? currentSlotted.Clone() : new Item();
                modPlayer.AscendancyLoadout[slotIndex] = mouseItem.Clone();
                Main.mouseItem = temp;
                SoundEngine.PlaySound(SoundID.Item37);
            }
        }

        private void DrawGradientBox(SpriteBatch sb, Rectangle rect, Color topColor, Color bottomColor, Color borderColor) {
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            int strips = 32;
            float stripHeight = (float)rect.Height / strips;

            for (int i = 0; i < strips; i++) {
                float t = (float)i / (strips - 1);
                Color c = Color.Lerp(topColor, bottomColor, t);
                Rectangle slice = new Rectangle(rect.X, rect.Y + (int)(i * stripHeight), rect.Width, (int)stripHeight + 1);
                sb.Draw(pixel, slice, c);
            }

            // 2px Border
            sb.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, 2), borderColor);
            sb.Draw(pixel, new Rectangle(rect.X, rect.Bottom - 2, rect.Width, 2), borderColor);
            sb.Draw(pixel, new Rectangle(rect.X, rect.Y, 2, rect.Height), borderColor);
            sb.Draw(pixel, new Rectangle(rect.Right - 2, rect.Y, 2, rect.Height), borderColor);
        }
    }
}