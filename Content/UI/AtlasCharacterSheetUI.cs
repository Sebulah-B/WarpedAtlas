using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using WarpedAtlas.Content.Players;

namespace WarpedAtlas.Content.UI
{
    public class AtlasCharacterSheetUI : UIState
    {
        private UIPanel panel;
        private UIList statList;
        private UIScrollbar scrollbar;
        
        private string hoveredTooltip = "";
        private string lastClass = "";

        // UI Text Elements
        private UIText classText;
        
        // General / Defensive
        private UIText lifeText;
        private UIText defText;
        private UIText drText;
        private UIText moveText;
        
        // ARPG Custom
        private UIText esText;
        private UIText wardText;
        private UIText evadeText;
        private UIText penText;
        
        // Offensive Class-Specific
        private UIText dmgText;
        private UIText critText;
        private UIText speedText;
        private UIText subClassText; 
        
        // Summoner Specific
        private UIText minionText;
        private UIText sentryText;

        public override void OnInitialize() {
            panel = new UIPanel();
            panel.Width.Set(360f, 0f);
            panel.Height.Set(480f, 0f);
            panel.HAlign = 0.85f; 
            panel.VAlign = 0.5f;
            panel.BackgroundColor = new Color(20, 30, 50, 230);
            Append(panel);

            UIText header = new UIText("[c/ffcc00:Atlas Record]", 1.1f, true);
            header.HAlign = 0.5f;
            header.Top.Set(5f, 0f);
            panel.Append(header);

            classText = new UIText("Archetype: None", 0.95f);
            classText.HAlign = 0.5f;
            classText.Top.Set(35f, 0f);
            panel.Append(classText);

            // Setup Scrolling List
            statList = new UIList();
            statList.Top.Set(65f, 0f);
            statList.Width.Set(-20f, 1f);
            statList.Height.Set(-110f, 1f);
            statList.ListPadding = 8f;
            panel.Append(statList);

            scrollbar = new UIScrollbar();
            scrollbar.SetView(100f, 1000f);
            scrollbar.Height.Set(-110f, 1f);
            scrollbar.Top.Set(65f, 0f);
            scrollbar.HAlign = 1f;
            panel.Append(scrollbar);
            statList.SetScrollbar(scrollbar);

            // Initialize Text Elements
            lifeText = CreateStatText("Health & Regen", "Total health and natural life regeneration per second.");
            defText = CreateStatText("Defense", "Flat damage reduction subtracted from incoming hits.");
            drText = CreateStatText("Damage Reduction", "Percentage reduction applied to all incoming damage.");
            moveText = CreateStatText("Movement Speed", "Movement speed multiplier.");
            
            esText = CreateStatText("Energy Shield", "Recharges automatically if you avoid damage for 3 seconds.");
            wardText = CreateStatText("Spirit Ward", "Generated via Minion hits and specific modifiers. Does not recharge natively.");
            evadeText = CreateStatText("Evasion Chance", "Grants a chance to completely negate a hit and grant brief invulnerability. Capped at 75%.");
            penText = CreateStatText("Armor Pen", "Bypasses enemy defense to increase overall DPS.");

            dmgText = CreateStatText("Class Damage", "Overall damage multiplier for your archetype.");
            critText = CreateStatText("Class Crit", "Critical strike chance for your archetype.");
            speedText = CreateStatText("Class Speed", "Attack speed multiplier for your archetype.");
            subClassText = CreateStatText("Sub-Class Bonus", "Damage bonus applied to specific weapon types within your archetype.");

            minionText = CreateStatText("Active Minions", "Current active minion slots vs maximum capacity.");
            sentryText = CreateStatText("Active Sentries", "Current active sentries vs maximum capacity.");

            UIText closeBtn = new UIText("[ Close ]", 0.95f);
            closeBtn.HAlign = 0.5f;
            closeBtn.Top.Set(-30f, 1f);
            closeBtn.OnLeftClick += (evt, element) => ModContent.GetInstance<Systems.AtlasUISystem>().HideCharacterSheet();
            closeBtn.OnMouseOver += (evt, element) => closeBtn.TextColor = Color.Yellow;
            closeBtn.OnMouseOut += (evt, element) => closeBtn.TextColor = Color.White;
            panel.Append(closeBtn);
        }

        private UIText CreateStatText(string defaultText, string tooltip) {
            UIText text = new UIText(defaultText, 0.85f);
            text.OnMouseOver += (evt, element) => hoveredTooltip = tooltip;
            text.OnMouseOut += (evt, element) => hoveredTooltip = "";
            return text;
        }

        private UIText CreateDivider(string title) {
            UIText div = new UIText($"--- {title} ---", 0.85f);
            div.HAlign = 0.5f;
            div.TextColor = Color.Gray;
            return div;
        }

        // Dynamically rebuilds the list layout based on the current class
        private void BuildStatList(string className) {
            statList.Clear();

            statList.Add(CreateDivider("Custom ARPG Vectors"));
            statList.Add(esText);
            statList.Add(wardText);
            statList.Add(evadeText);
            statList.Add(penText);

            statList.Add(CreateDivider("Offensive Stats"));
            statList.Add(dmgText);
            statList.Add(critText);
            statList.Add(speedText);
            if (className != "") statList.Add(subClassText);

            if (className == "Summoner") {
                statList.Add(CreateDivider("Summoning Assets"));
                statList.Add(minionText);
                statList.Add(sentryText);
            }

            statList.Add(CreateDivider("Core Defenses"));
            statList.Add(lifeText);
            statList.Add(defText);
            statList.Add(drText);
            statList.Add(moveText);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            Player player = Main.LocalPlayer;
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();

            if (lastClass != prog.ChosenClass) {
                BuildStatList(prog.ChosenClass);
                lastClass = prog.ChosenClass;
            }

            classText.SetText($"Archetype: {(prog.ChosenClass == "" ? "None" : prog.ChosenClass)} (Lv. {prog.Level})");
            
            // Core Defenses
            lifeText.SetText($"Max Life: {player.statLifeMax2} | Regen: {player.lifeRegen}");
            defText.SetText($"Defense: {player.statDefense}");
            drText.SetText($"Endurance (DR): {player.endurance * 100f:0.#}%");
            moveText.SetText($"Movement Speed: {player.moveSpeed * 100f:0.#}%");

            // ARPG Vectors
            esText.SetText($"Energy Shield: {(int)prog.CurrentEnergyShield} / {prog.MaxEnergyShield}");
            wardText.SetText($"Spirit Ward: {(int)prog.CurrentWard} / {prog.MaxWard}");
            evadeText.SetText($"Evasion Chance: {MathHelper.Clamp(prog.EvasionChance * 100f, 0, 75):0.#}%");
            penText.SetText($"Armor Pen: {player.GetArmorPenetration(DamageClass.Generic)}");

            // Offensive Scaling
            DamageClass dmgClass = prog.ChosenClass switch {
                "Warrior" => DamageClass.Melee,
                "Ranger" => DamageClass.Ranged,
                "Sorcerer" => DamageClass.Magic,
                "Summoner" => DamageClass.Summon,
                _ => DamageClass.Generic
            };

            float dmgMult = player.GetDamage(dmgClass).Multiplicative - 1f + player.GetDamage(dmgClass).Additive - 1f;
            dmgText.SetText($"Class Damage: {dmgMult * 100f:+0.#;-0.#;0}%");
            critText.SetText($"Crit Chance: {player.GetCritChance(dmgClass)}%");
            speedText.SetText($"Attack Speed: {player.GetAttackSpeed(dmgClass) * 100f:0.#}%");

            // Sub-Class Bonuses
            if (prog.ChosenClass == "Warrior") subClassText.SetText($"Bonus Yoyo Damage: {prog.BonusYoyoDamage * 100f:+0.#;-0.#;0}%");
            else if (prog.ChosenClass == "Ranger") subClassText.SetText($"Bonus Bow Damage: {prog.BonusBowDamage * 100f:+0.#;-0.#;0}%");
            else if (prog.ChosenClass == "Sorcerer") subClassText.SetText($"Bonus Tome Damage: {prog.BonusTomeDamage * 100f:+0.#;-0.#;0}%");
            else if (prog.ChosenClass == "Summoner") subClassText.SetText($"Bonus Whip Damage: {prog.BonusWhipDamage * 100f:+0.#;-0.#;0}%");

            // Live Minion & Sentry Tracker
            if (prog.ChosenClass == "Summoner") {
                float activeMinions = 0;
                int activeSentries = 0;
                for (int i = 0; i < Main.maxProjectiles; i++) {
                    Projectile p = Main.projectile[i];
                    if (p.active && p.owner == Main.myPlayer) {
                        if (p.minion) activeMinions += p.minionSlots;
                        if (p.sentry) activeSentries++;
                    }
                }
                minionText.SetText($"Active Minions: {activeMinions:0.#} / {player.maxMinions}");
                sentryText.SetText($"Active Sentries: {activeSentries} / {player.maxTurrets}");
            }
        }

        // Custom Tooltip Bubble Rendering Engine
        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch); // Draw the panel and text first

            if (!string.IsNullOrEmpty(hoveredTooltip)) {
                var font = Terraria.GameContent.FontAssets.MouseText.Value;
                Vector2 mousePos = new Vector2(Main.mouseX, Main.mouseY) + new Vector2(16, 16);
                Vector2 textSize = font.MeasureString(hoveredTooltip);
                
                // Screen edge clamp to prevent tooltip clipping
                if (mousePos.X + textSize.X + 16 > Main.screenWidth) 
                    mousePos.X = Main.screenWidth - textSize.X - 16;
                if (mousePos.Y + textSize.Y + 16 > Main.screenHeight) 
                    mousePos.Y = Main.screenHeight - textSize.Y - 16;

                // Draw the Dark Blue background box
                Rectangle bgRect = new Rectangle((int)mousePos.X - 8, (int)mousePos.Y - 4, (int)textSize.X + 16, (int)textSize.Y + 8);
                Terraria.Utils.DrawInvBG(spriteBatch, bgRect, new Color(20, 25, 45, 240));
                
                // Draw the actual text over the box
                Terraria.Utils.DrawBorderString(spriteBatch, hoveredTooltip, mousePos, Color.White);
            }
        }
    }
}