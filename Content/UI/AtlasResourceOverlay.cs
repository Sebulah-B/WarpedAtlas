using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using WarpedAtlas.Content.Players;

namespace WarpedAtlas.Content.UI
{
    public class AtlasResourceOverlay : ModSystem
    {
        // Internal tracking values for the smooth lerping animation
        private float _lerpedES = 0f;
        private float _lerpedWard = 0f;

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "WarpedAtlas: ARPG Resource Bar",
                    delegate
                    {
                        DrawResourceBarsAligned();
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        private void DrawResourceBarsAligned()
        {
            Player player = Main.LocalPlayer;
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();

            // Only draw if the player has these defenses active
            if (prog.MaxEnergyShield <= 0 && prog.MaxWard <= 0) return;

            // --- SMOOTH LERPING ANIMATION ---
            // Interpolate the visual fill towards the actual resource value by 10% each frame
            _lerpedES = MathHelper.Lerp(_lerpedES, prog.CurrentEnergyShield, 0.1f);
            _lerpedWard = MathHelper.Lerp(_lerpedWard, prog.CurrentWard, 0.1f);

            // Snap directly to the final value if it gets close enough (prevents infinite micro-decimals)
            if (Math.Abs(_lerpedES - prog.CurrentEnergyShield) < 0.5f) _lerpedES = prog.CurrentEnergyShield;
            if (Math.Abs(_lerpedWard - prog.CurrentWard) < 0.5f) _lerpedWard = prog.CurrentWard;

            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            
            // --- ALIGNMENT LOGIC ---
            int barWidth = 260; // Matches the exact width of a 10-heart vanilla row
            int barHeight = 20;
            
            // Shift the bar up to Y = 70 if the player only has 1 row of hearts (<= 200 Max Life)
            // Shift down to Y = 100 if they have 2 rows of hearts (> 200 Max Life)
            float yPos = player.statLifeMax2 > 200 ? 100f : 70f;
            Vector2 position = new Vector2(Main.screenWidth - 300f, yPos); // -300 matches the leftmost heart's X coordinate

            // 1. Draw Energy Shield Bar
            if (prog.MaxEnergyShield > 0)
            {
                float esPercent = MathHelper.Clamp(_lerpedES / prog.MaxEnergyShield, 0f, 1f);
                Color esColor = prog.CurrentEnergyShield > 0 ? new Color(0, 200, 255) : Color.DarkSlateGray;
                string esText = prog.CurrentEnergyShield > 0 ? $"ES: {(int)prog.CurrentEnergyShield} / {prog.MaxEnergyShield}" : "BROKEN";
                
                DrawThematicBar(spriteBatch, pixel, position, barWidth, barHeight, esPercent, esColor, esText);
            }

            // 2. Draw Spirit Ward Bar
            if (prog.MaxWard > 0)
            {
                // Offset the Ward bar vertically if Energy Shield is also currently drawn
                int yOffset = prog.MaxEnergyShield > 0 ? 30 : 0; 
                Vector2 wardPos = new Vector2(position.X, position.Y + yOffset);
                
                float wardPercent = MathHelper.Clamp(_lerpedWard / prog.MaxWard, 0f, 1f);
                Color wardColor = prog.CurrentWard > 0 ? new Color(200, 0, 200) : Color.DarkSlateGray;
                string wardText = prog.CurrentWard > 0 ? $"Ward: {(int)prog.CurrentWard} / {prog.MaxWard}" : "DEPLETED";

                DrawThematicBar(spriteBatch, pixel, wardPos, barWidth, barHeight, wardPercent, wardColor, wardText);
            }
        }

        private void DrawThematicBar(SpriteBatch spriteBatch, Texture2D pixel, Vector2 position, int width, int height, float percent, Color fillColor, string text)
        {
            // Atlas UI Theme Colors
            Color borderColor = new Color(200, 160, 60, 255); // Gold border
            Color bgColor = new Color(15, 20, 35, 245);       // Deep space blue

            // 1. Draw Outer Gold Border
            spriteBatch.Draw(pixel, new Rectangle((int)position.X - 2, (int)position.Y - 2, width + 4, height + 4), borderColor);
            
            // 2. Draw Inner Background
            spriteBatch.Draw(pixel, new Rectangle((int)position.X, (int)position.Y, width, height), bgColor);

            // 3. Draw Fill with Pseudo-3D Shading
            int fillWidth = (int)(width * percent);
            if (fillWidth > 0)
            {
                // Base fill color
                spriteBatch.Draw(pixel, new Rectangle((int)position.X, (int)position.Y, fillWidth, height), fillColor * 0.8f);
                
                // Top highlight (simulates light hitting the top edge)
                spriteBatch.Draw(pixel, new Rectangle((int)position.X, (int)position.Y, fillWidth, 3), Color.White * 0.3f);
                
                // Bottom shadow (simulates depth)
                spriteBatch.Draw(pixel, new Rectangle((int)position.X, (int)position.Y + height - 3, fillWidth, 3), Color.Black * 0.4f);
            }

            // 4. Center the Text Inside the Bar
            Vector2 textSize = FontAssets.MouseText.Value.MeasureString(text) * 0.8f;
            Vector2 textPos = new Vector2(position.X + (width / 2f) - (textSize.X / 2f), position.Y + (height / 2f) - (textSize.Y / 2f) + 2f);
            
            Utils.DrawBorderString(spriteBatch, text, textPos, Color.White, 0.8f);
        }
    }
}