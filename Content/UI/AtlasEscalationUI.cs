using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.NPCs;
using WarpedAtlas.Content.Subworlds;
using WarpedAtlas.Content.Items.Currencies;

namespace WarpedAtlas.Content.UI
{
    public class AtlasEscalationUI : UIState
    {
        private UIPanel mainPanel;
        private UIText titleText;
        private UIText pressureText;
        private EscalationAnchorNPC linkedAnchor;

        public void SetAnchor(EscalationAnchorNPC anchor) {
            linkedAnchor = anchor;
        }

        public override void OnInitialize() {
            mainPanel = new UIPanel();
            mainPanel.SetPadding(20);
            mainPanel.Width.Set(400, 0);
            mainPanel.Height.Set(220, 0); 
            mainPanel.HAlign = 0.5f;
            mainPanel.VAlign = 0.8f; 
            mainPanel.BackgroundColor = new Color(20, 20, 40, 220);
            mainPanel.BorderColor = new Color(255, 150, 255, 255);
            Append(mainPanel);

            titleText = new UIText("Temporal Escalation", 0.6f, true);
            titleText.HAlign = 0.5f;
            titleText.VAlign = 0.05f;
            titleText.TextColor = new Color(255, 150, 255);
            mainPanel.Append(titleText);

            pressureText = new UIText("Pressure Level: 0", 1f);
            pressureText.HAlign = 0.5f;
            pressureText.VAlign = 0.30f; 
            mainPanel.Append(pressureText);

            // Rebuilt the button using a colored UIPanel and standard text
            UIPanel extractButton = new UIPanel();
            extractButton.Width.Set(120, 0);
            extractButton.Height.Set(40, 0);
            extractButton.HAlign = 0.5f;
            extractButton.VAlign = 0.95f;
            extractButton.BackgroundColor = new Color(73, 94, 171); // Terraria UI Blue
            
            // Add hover color feedback
            extractButton.OnMouseOver += (evt, element) => extractButton.BackgroundColor = new Color(100, 118, 184);
            extractButton.OnMouseOut += (evt, element) => extractButton.BackgroundColor = new Color(73, 94, 171);
            
            extractButton.OnLeftClick += (evt, element) => {
                if (linkedAnchor != null && linkedAnchor.NPC.active) {
                    linkedAnchor.ExtractAndReward();
                    ModContent.GetInstance<Systems.AtlasUISystem>().CloseEscalationUI();
                }
            };
            mainPanel.Append(extractButton);

            UIText extractText = new UIText("Extract", 1f);
            extractText.HAlign = 0.5f;
            extractText.VAlign = 0.5f;
            extractButton.Append(extractText);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if (linkedAnchor == null || !linkedAnchor.NPC.active || Main.LocalPlayer.Distance(linkedAnchor.NPC.Center) > 300f) {
                ModContent.GetInstance<Systems.AtlasUISystem>().CloseEscalationUI();
                return;
            }

            pressureText.SetText($"Pressure Level: {BaseDungeonSubworld.CurrentPressureLevel}");
            
            // Automatically start the event if it hasn't begun
            if (linkedAnchor.State == 0) {
                linkedAnchor.State = 1;
                SoundEngine.PlaySound(SoundID.Roar, Main.LocalPlayer.Center);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {
            base.DrawSelf(spriteBatch);

            // Custom Drawing for the Threshold Progress Bar
            CalculatedStyle dimensions = mainPanel.GetDimensions();
            float barWidth = 320f;
            float barHeight = 16f;
            float barX = dimensions.X + (dimensions.Width - barWidth) / 2;
            float barY = dimensions.Y + 110f; 

            // Draw Background Track
            Rectangle bgRect = new Rectangle((int)barX, (int)barY, (int)barWidth, (int)barHeight);
            spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, bgRect, new Color(80, 80, 100, 200));

            // Calculate Fill based on Max Pressure of 15
            int currentPressure = BaseDungeonSubworld.CurrentPressureLevel;
            float fillPercent = MathHelper.Clamp(currentPressure / 15f, 0f, 1f);
            
            Rectangle fillRect = new Rectangle((int)barX, (int)barY, (int)(barWidth * fillPercent), (int)barHeight);
            spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, fillRect, new Color(255, 100, 255, 255));

            // Draw the Reward Pips & Icons at specific thresholds
            DrawThresholdPip(spriteBatch, barX, barY, barWidth, 5, 15, ModContent.ItemType<OrbOfAlchemy>(), currentPressure >= 5);
            DrawThresholdPip(spriteBatch, barX, barY, barWidth, 10, 15, ModContent.ItemType<ChaosOrb>(), currentPressure >= 10);
            DrawThresholdPip(spriteBatch, barX, barY, barWidth, 15, 15, ModContent.ItemType<ExaltedOrb>(), currentPressure >= 15);
        }

        private void DrawThresholdPip(SpriteBatch spriteBatch, float barX, float barY, float barWidth, int targetLevel, int maxLevel, int itemType, bool unlocked) {
            float positionX = barX + (barWidth * ((float)targetLevel / maxLevel));
            
            // Draw Pip Line
            Rectangle pipRect = new Rectangle((int)positionX - 2, (int)barY - 4, 4, 24);
            spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, pipRect, unlocked ? Color.Gold : Color.DarkGray);

            // Draw Item Icon above the pip
            Main.instance.LoadItem(itemType);
            Texture2D icon = Terraria.GameContent.TextureAssets.Item[itemType].Value;
            Vector2 iconPos = new Vector2(positionX - (icon.Width / 2), barY - 26); 
            
            Color iconColor = unlocked ? Color.White : Color.White * 0.4f;
            spriteBatch.Draw(icon, iconPos, iconColor);
        }
    }
}