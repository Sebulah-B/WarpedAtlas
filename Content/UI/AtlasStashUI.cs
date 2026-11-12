using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Systems;

namespace WarpedAtlas.Content.UI
{
    public class AtlasStashUI : UIState
    {
        private const int Columns = 10;
        private const int Rows = 10;
        private const int SlotSize = 46;
        private const int SlotGap = 4;

        public override void Draw(SpriteBatch spriteBatch) {
            Player player = Main.LocalPlayer;
            if (player == null || !player.active) return;

            if (player.chest != -1) player.chest = -1;

            if (!Main.playerInventory) {
                ModContent.GetInstance<AtlasUISystem>().CloseNativeStashUI();
                return;
            }

            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.StashItems == null || prog.StashItems.Length < 100) {
                prog.StashItems = new Item[100];
            }
            for (int i = 0; i < 100; i++) {
                if (prog.StashItems[i] == null) {
                    prog.StashItems[i] = new Item();
                    prog.StashItems[i].TurnToAir();
                }
            }

            int gridWidth = Columns * SlotSize + (Columns - 1) * SlotGap;
            int gridHeight = Rows * SlotSize + (Rows - 1) * SlotGap;
            int panelWidth = gridWidth + 40;
            int panelHeight = gridHeight + 96;

            int panelX = (Main.screenWidth - panelWidth) / 2;
            int panelY = Math.Max(260, (Main.screenHeight - panelHeight) / 2 + 40);

            Rectangle panelRect = new Rectangle(panelX, panelY, panelWidth, panelHeight);
            Point mousePoint = Main.MouseScreen.ToPoint();

            if (panelRect.Contains(mousePoint)) {
                player.mouseInterface = true;
            }

            DrawBox(spriteBatch, panelRect, new Color(15, 20, 35, 245), new Color(200, 160, 60, 255), 3);

            Utils.DrawBorderStringBig(spriteBatch, "THE SACRED VAULT (100 SLOTS)", new Vector2(panelX + panelWidth / 2f - 40f, panelY + 28f), new Color(255, 215, 0), 0.52f, 0.5f, 0.5f);

            Rectangle sortBtn = new Rectangle(panelX + panelWidth - 190, panelY + 12, 135, 28);
            bool hoverSort = sortBtn.Contains(mousePoint);
            DrawBox(spriteBatch, sortBtn, hoverSort ? new Color(60, 110, 160) : new Color(35, 65, 100), Color.Gold, 1);
            Utils.DrawBorderString(spriteBatch, "Deposit Orbs", sortBtn.Center.ToVector2(), Color.White, 0.78f, 0.5f, 0.45f);
            if (hoverSort && Main.mouseLeft && Main.mouseLeftRelease) {
                DepositCraftingCurrencies(player, prog);
            }

            Rectangle closeBtn = new Rectangle(panelX + panelWidth - 44, panelY + 12, 28, 28);
            bool hoverClose = closeBtn.Contains(mousePoint);
            DrawBox(spriteBatch, closeBtn, hoverClose ? new Color(190, 50, 50) : new Color(100, 30, 30), Color.White, 1);
            Utils.DrawBorderString(spriteBatch, "X", closeBtn.Center.ToVector2(), Color.White, 0.85f, 0.5f, 0.45f);
            if (hoverClose && Main.mouseLeft && Main.mouseLeftRelease) {
                SoundEngine.PlaySound(SoundID.MenuClose);
                ModContent.GetInstance<AtlasUISystem>().CloseNativeStashUI();
                return;
            }

            int startX = panelX + 20;
            int startY = panelY + 52;
            int hoveredSlot = -1;

            for (int i = 0; i < 100; i++) {
                int col = i % Columns;
                int row = i / Columns;

                Rectangle slotRect = new Rectangle(
                    startX + col * (SlotSize + SlotGap),
                    startY + row * (SlotSize + SlotGap),
                    SlotSize,
                    SlotSize
                );

                bool isHovered = slotRect.Contains(mousePoint);
                if (isHovered) hoveredSlot = i;

                Item item = prog.StashItems[i];
                Color slotBg = isHovered ? new Color(55, 75, 115, 240) : new Color(26, 34, 56, 230);
                Color slotBorder = isHovered ? Color.Gold : new Color(80, 105, 150, 200);

                if (item != null && !item.IsAir) {
                    if (item.TryGetGlobalItem<Items.AtlasGlobalItem>(out var globalItem)) {
                        if (globalItem.CurrentTier == Items.ItemTier.Rare) slotBorder = isHovered ? Color.White : new Color(255, 215, 0);
                        else if (globalItem.CurrentTier == Items.ItemTier.Magic) slotBorder = isHovered ? Color.White : new Color(80, 160, 255);
                    }
                }

                DrawBox(spriteBatch, slotRect, slotBg, slotBorder, isHovered ? 2 : 1);

                if (item != null && !item.IsAir) {
                    Main.instance.LoadItem(item.type);
                    Texture2D itemTex = TextureAssets.Item[item.type].Value;
                    Rectangle frame = Main.itemAnimations[item.type] != null
                        ? Main.itemAnimations[item.type].GetFrame(itemTex)
                        : itemTex.Frame();

                    float scale = 1f;
                    int maxDim = Math.Max(frame.Width, frame.Height);
                    if (maxDim > 34) scale = 34f / maxDim;

                    Vector2 drawCenter = slotRect.Center.ToVector2();
                    spriteBatch.Draw(itemTex, drawCenter, frame, item.GetAlpha(Color.White), 0f, frame.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                    if (item.color != Color.Transparent) {
                        spriteBatch.Draw(itemTex, drawCenter, frame, item.GetColor(Color.White), 0f, frame.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                    }

                    if (item.stack > 1) {
                        Utils.DrawBorderString(spriteBatch, item.stack.ToString(), new Vector2(slotRect.X + 6, slotRect.Bottom - 20), Color.White, 0.75f);
                    }
                }
            }

            if (hoveredSlot != -1) {
                Item slotItem = prog.StashItems[hoveredSlot];

                if (Main.mouseLeft && Main.mouseLeftRelease) {
                    HandleLeftClick(player, prog, hoveredSlot);
                } else if (Main.mouseRight && Main.mouseRightRelease) {
                    HandleRightClick(prog, hoveredSlot);
                }

                if (slotItem != null && !slotItem.IsAir) {
                    Main.HoverItem = slotItem.Clone();
                    Main.hoverItemName = slotItem.Name;
                }
            }

            Utils.DrawBorderString(spriteBatch, "Left-Click: Place/Swap | Shift-Click: Quick Transfer to Inventory | Right-Click: Take 1", new Vector2(panelX + panelWidth / 2f, panelRect.Bottom - 18f), Color.LightGray, 0.78f, 0.5f, 0.5f);
        }

        private void HandleLeftClick(Player player, AtlasProgressionPlayer prog, int index) {
            Item slotItem = prog.StashItems[index];
            Item mouseItem = Main.mouseItem;

            if (ItemSlot.ShiftInUse && slotItem != null && !slotItem.IsAir && mouseItem.IsAir) {
                Item leftover = player.GetItem(player.whoAmI, slotItem.Clone(), GetItemSettings.InventoryEntityToPlayerInventorySettings);
                if (leftover.IsAir || leftover.stack <= 0) {
                    prog.StashItems[index].TurnToAir();
                } else {
                    prog.StashItems[index] = leftover;
                }
                SoundEngine.PlaySound(SoundID.Grab);
                return;
            }

            if (!mouseItem.IsAir && !slotItem.IsAir && mouseItem.type == slotItem.type && slotItem.stack < slotItem.maxStack) {
                int space = slotItem.maxStack - slotItem.stack;
                int toMove = Math.Min(space, mouseItem.stack);
                slotItem.stack += toMove;
                mouseItem.stack -= toMove;
                if (mouseItem.stack <= 0) mouseItem.TurnToAir();
                SoundEngine.PlaySound(SoundID.Grab);
                return;
            }

            Item temp = slotItem != null && !slotItem.IsAir ? slotItem.Clone() : new Item();
            prog.StashItems[index] = mouseItem != null && !mouseItem.IsAir ? mouseItem.Clone() : new Item();
            Main.mouseItem = temp;
            SoundEngine.PlaySound(SoundID.Grab);
        }

        private void HandleRightClick(AtlasProgressionPlayer prog, int index) {
            Item slotItem = prog.StashItems[index];
            if (slotItem == null || slotItem.IsAir) return;

            if (Main.mouseItem.IsAir) {
                Main.mouseItem = slotItem.Clone();
                Main.mouseItem.stack = 1;
                slotItem.stack--;
                if (slotItem.stack <= 0) slotItem.TurnToAir();
                SoundEngine.PlaySound(SoundID.MenuTick);
            } else if (Main.mouseItem.type == slotItem.type && Main.mouseItem.stack < Main.mouseItem.maxStack) {
                Main.mouseItem.stack++;
                slotItem.stack--;
                if (slotItem.stack <= 0) slotItem.TurnToAir();
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
        }

        private void DepositCraftingCurrencies(Player player, AtlasProgressionPlayer prog) {
            bool movedAny = false;
            for (int inv = 0; inv < 50; inv++) {
                Item item = player.inventory[inv];
                if (item != null && !item.IsAir && item.ModItem != null && item.ModItem.Mod.Name == "WarpedAtlas" && item.maxStack > 1) {
                    for (int s = 0; s < 100 && item.stack > 0; s++) {
                        if (prog.StashItems[s].type == item.type && prog.StashItems[s].stack < prog.StashItems[s].maxStack) {
                            int transfer = Math.Min(item.stack, prog.StashItems[s].maxStack - prog.StashItems[s].stack);
                            prog.StashItems[s].stack += transfer;
                            item.stack -= transfer;
                            movedAny = true;
                        }
                    }
                    if (item.stack > 0) {
                        for (int s = 0; s < 100; s++) {
                            if (prog.StashItems[s].IsAir) {
                                prog.StashItems[s] = item.Clone();
                                item.TurnToAir();
                                movedAny = true;
                                break;
                            }
                        }
                    }
                    if (item.stack <= 0) item.TurnToAir();
                }
            }
            if (movedAny) SoundEngine.PlaySound(SoundID.Grab);
        }

        private void DrawBox(SpriteBatch sb, Rectangle rect, Color bg, Color border, int thickness) {
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            sb.Draw(pixel, rect, bg);
            sb.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), border);
            sb.Draw(pixel, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), border);
            sb.Draw(pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), border);
            sb.Draw(pixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), border);
        }
    }
}