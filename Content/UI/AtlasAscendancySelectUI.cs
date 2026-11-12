using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using WarpedAtlas.Content.Items;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Systems;
using WarpedAtlas.Content.Items.Ascendancies;

namespace WarpedAtlas.Content.UI
{
    public class AscendancyOption
    {
        public string Id;
        public string Title;
        public string ParentClass;
        public string Subtitle;
        public string Description;
        public int IconItemId;
        public Color ThemeColor;
        public bool IsPlaceholder;
    }

    public class AtlasAscendancySelectUI : UIState
    {
        private int selectedIndex = 0;

        public static readonly List<AscendancyOption> Options = new List<AscendancyOption>()
        {
            // 1. WARRIOR: ICE (Itztli)
            new AscendancyOption {
                Id = "IceWarrior",
                Title = "Champion of Itztli",
                ParentClass = "Warrior",
                Subtitle = "Avatar of the Obsidian Frost Paladin",
                Description = "Channels the freezing obsidian winds of Mictlan to fortify armor and shatter the earth.\n" +
                              "  Ultimate (North): Press 'V' to transform into the Ice Paladin and unleash Deerclops Icicle Crashes.\n" +
                              "  Offense (East): +15% Melee Damage, +8 Armor Pen, and Frostburn + Homing Ice Sickles.\n" +
                              "  Utility (South): +15% Move Speed, Ice Skates mobility, Chilled/Frozen immunity.\n" +
                              "  Defense (West): +12 Defense, +6% Endurance, and +40 Max Life.",
                IconItemId = ItemID.Frostbrand,
                ThemeColor = new Color(80, 210, 255),
                IsPlaceholder = false
            },

            // 2. WARRIOR: WIND (Ehecatl)
            new AscendancyOption {
                Id = "WindWarrior",
                Title = "Vortex of Ehecatl",
                ParentClass = "Warrior",
                Subtitle = "Dervish of the Feathered Tempest",
                Description = "Embodies Ehecatl, the Mesoamerican god of wind, becoming a living cyclone.\n" +
                              "  Ultimate (North - Breath of Ehecatl): Press 'V' to encase yourself in a compact, swirling Tornado while suspended at its center. Gently levitates and shreds nearby foes.\n" +
                              "  Offense (East - Gale of the Feathered Serpent): +15% Melee Damage & +12% Attack Speed. Melee swings launch Tome of Infinite Wisdom Whirlwind Tornadoes.",
                IconItemId = ItemID.WeatherPain,
                ThemeColor = new Color(90, 255, 190),
                IsPlaceholder = false
            },

            // 3. RANGER: FIRE TRAPPER (Xiuhtecuhtli)
            new AscendancyOption {
                Id = "FireRanger",
                Title = "Artificer of Xiuhtecuhtli",
                ParentClass = "Ranger",
                Subtitle = "Master of Volcanic Munitions & Pyre Traps",
                Description = "Consecrates traps and explosives in the volcanic fires of Xiuhtecuhtli.\n" +
                              "  Ultimate (North - Xiuhtecuhtli's Crucible): When your Shrapnel Trap explodes, it detonates a second time in a solar blast and scatters 4 cluster firebombs.\n" +
                              "  Offense (East - Eruption of Xiuhtecuhtli): +15% Ranged Damage & +10 Armor Pen. Every 4th Ranged hit erupts a fiery Volcanic Geyser beneath the target.",
                IconItemId = ItemID.LandMine,
                ThemeColor = new Color(255, 95, 50),
                IsPlaceholder = false
            },

            // 4. RANGER: TIME VOLLEY (Ollin)
            new AscendancyOption {
                Id = "TimeRanger",
                Title = "Chrono-Stalker of Ollin",
                ParentClass = "Ranger",
                Subtitle = "Weaver of the Fifth Sun's Motion",
                Description = "Bends the sacred calendar of Ollin to fracture projectiles across timelines.\n" +
                              "  Ultimate (North - Pulse of Ollin): Firing a Ranged weapon spawns an additional echoed projectile in a synchronized volley.\n" +
                              "  Offense (East - Velocity of the Fifth Sun): +12% Ranged Crit. Ranged Crits grant Chrono-Surge (+30% Attack Speed) and fire a high-velocity Temporal Piercer.",
                IconItemId = ItemID.Tsunami,
                ThemeColor = new Color(255, 215, 70),
                IsPlaceholder = false
            },

            // 5. SORCERER: FLAME PYRE (Xiuhcoatl)
            new AscendancyOption {
                Id = "FlameSorcerer",
                Title = "Scion of Xiuhcoatl",
                ParentClass = "Sorcerer",
                Subtitle = "Conduit of the Fire Serpent",
                Description = "Replaces arcane exhaustion with the self-immolating power of the Fire Serpent.\n" +
                              "  Ultimate (North - Blood of Xiuhcoatl): Replaces Mana Sickness with Solar Immolation (+25% Magic Damage, ignites foes, and burns Energy Shield before Life).\n" +
                              "  Offense (East - Fangs of Xiuhcoatl): +18% Magic Damage. Magic hits against burning enemies trigger a violent Solar Combustion blast.",
                IconItemId = ItemID.FragmentSolar,
                ThemeColor = new Color(255, 110, 40),
                IsPlaceholder = false
            },

            // 6. SORCERER: LIGHTNING (Tezcatlipoca)
            new AscendancyOption {
                Id = "LightningSorcerer",
                Title = "Oracle of Tezcatlipoca",
                ParentClass = "Sorcerer",
                Subtitle = "Caller of the Smoking Mirror's Storm",
                Description = "Commands living fulgurite lightning born from the obsidian mirror of Tezcatlipoca.\n" +
                              "  Ultimate (North - Tempest of Tezcatlipoca): +8% Magic Crit. Magic Critical Strikes manifest a Homing Living Lightning Orb.\n" +
                              "  Offense (East - Obsidian Mirror Discharge): +14% Magic Damage & +10% Cast Speed. Magic hits arc Chain Lightning to up to 3 nearby enemies.",
                IconItemId = ItemID.NebulaArcanum,
                ThemeColor = new Color(70, 175, 255),
                IsPlaceholder = false
            },

            // 7. SUMMONER: PLANT / LIFE FRUIT (Xochiquetzal)
            new AscendancyOption {
                Id = "PlantSummoner",
                Title = "Cultivator of Xochiquetzal",
                ParentClass = "Summoner",
                Subtitle = "Keeper of the Sacred World-Tree",
                Description = "Nurtures the lush vitality of Xochiquetzal from the fallen husks of enemies.\n" +
                              "  Ultimate (North - Garden of Xochiquetzal): Kills and boss hits sprout Sacred Life Fruit (+40 Spirit Ward, +35% Summon Damage & +25% Speed on pickup).\n" +
                              "  Offense (East - Briars of Xochiquetzal): +15% Summon Damage & +1 Max Minion. Minion and Whip hits erupt impaling Blood-Thorn Spikes beneath foes.",
                IconItemId = ItemID.LifeFruit,
                ThemeColor = new Color(110, 245, 90),
                IsPlaceholder = false
            },

            // 8. SUMMONER: LIGHTNING ARC (Tlaloc)
            new AscendancyOption {
                Id = "LightningSummoner",
                Title = "Herald of Tlaloc",
                ParentClass = "Summoner",
                Subtitle = "Sovereign of the Storm-Tethered Swarm",
                Description = "Links master and minion with the high-voltage fury of Tlaloc, god of rain and thunder.\n" +
                              "  Ultimate (North - Conduit of Tlaloc): +15% Summon Crit. Minion Crits surge a Lightning Arc between you and the Minion, frying all enemies between you.\n" +
                              "  Offense (East - Wrath of Tlalocan): +15% Summon Damage & +12% Speed. Minion hits call down vertical Sky-Thunderbolts onto the target.",
                IconItemId = ItemID.ThunderStaff,
                ThemeColor = new Color(0, 240, 255),
                IsPlaceholder = false
            }
        };

        public void AutoSelectPlayerClass() {
            if (Main.LocalPlayer == null || !Main.LocalPlayer.active) return;

            var prog = Main.LocalPlayer.GetModPlayer<AtlasProgressionPlayer>();
            for (int i = 0; i < Options.Count; i++) {
                if (string.IsNullOrEmpty(prog.ChosenClass) || prog.ChosenClass == Options[i].ParentClass) {
                    selectedIndex = i;
                    break;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Player player = Main.LocalPlayer;
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            Main.LocalPlayer.mouseInterface = true;

            int panelWidth = 920;
            int panelHeight = 580;
            int panelX = (Main.screenWidth - panelWidth) / 2;
            int panelY = (Main.screenHeight - panelHeight) / 2;

            Rectangle mainRect = new Rectangle(panelX, panelY, panelWidth, panelHeight);
            DrawBox(spriteBatch, mainRect, new Color(16, 20, 32, 245), new Color(90, 140, 200), 3);

            Utils.DrawBorderStringBig(spriteBatch, "ALTAR OF THE FIVE SUNS: ASCENSION", new Vector2(panelX + panelWidth / 2f, panelY + 34), new Color(180, 235, 255), 0.65f, 0.5f, 0.5f);
            Utils.DrawBorderString(spriteBatch, $"Current Class: {(string.IsNullOrEmpty(prog.ChosenClass) ? "Unassigned" : prog.ChosenClass)} - Choose your Divine Devotion", new Vector2(panelX + panelWidth / 2f, panelY + 66), Color.LightGray, 0.9f, 0.5f, 0.5f);

            Rectangle closeBtn = new Rectangle(panelX + panelWidth - 42, panelY + 12, 30, 30);
            bool hoverClose = closeBtn.Contains(Main.MouseScreen.ToPoint());
            DrawBox(spriteBatch, closeBtn, hoverClose ? new Color(180, 50, 50) : new Color(90, 30, 30), Color.White, 1);
            Utils.DrawBorderString(spriteBatch, "X", closeBtn.Center.ToVector2(), Color.White, 0.9f, 0.5f, 0.45f);

            if (hoverClose && Main.mouseLeft && Main.mouseLeftRelease) {
                SoundEngine.PlaySound(SoundID.MenuClose);
                ModContent.GetInstance<AtlasUISystem>().CloseAscendancySelectUI();
                return;
            }

            int gridStartX = panelX + 28;
            int gridStartY = panelY + 95;
            int cardWidth = 210;
            int cardHeight = 96;
            int gapX = 14;
            int gapY = 14;

            for (int i = 0; i < Options.Count; i++) {
                var opt = Options[i];
                int col = i % 2;
                int row = i / 2;

                Rectangle cardRect = new Rectangle(
                    gridStartX + col * (cardWidth + gapX),
                    gridStartY + row * (cardHeight + gapY),
                    cardWidth,
                    cardHeight
                );

                bool isSelected = (selectedIndex == i);
                bool isHovered = cardRect.Contains(Main.MouseScreen.ToPoint());
                bool matchesClass = string.IsNullOrEmpty(prog.ChosenClass) || prog.ChosenClass == opt.ParentClass;

                Color bg = isSelected
                    ? opt.ThemeColor * 0.35f
                    : (isHovered ? new Color(45, 55, 80, 230) : new Color(26, 32, 48, 220));
                Color border = isSelected
                    ? opt.ThemeColor
                    : (matchesClass ? opt.ThemeColor * 0.65f : new Color(70, 75, 90));

                if (!matchesClass) bg *= 0.4f;

                DrawBox(spriteBatch, cardRect, bg, border, isSelected ? 3 : 2);

                Main.instance.LoadItem(opt.IconItemId);
                Texture2D itemTex = TextureAssets.Item[opt.IconItemId].Value;
                Vector2 iconPos = new Vector2(cardRect.X + 28, cardRect.Y + cardRect.Height / 2f);
                float scale = Math.Min(1f, 36f / Math.Max(itemTex.Width, itemTex.Height));
                spriteBatch.Draw(itemTex, iconPos, null, matchesClass ? Color.White : Color.Gray, 0f, itemTex.Size() * 0.5f, scale, SpriteEffects.None, 0f);

                Utils.DrawBorderString(spriteBatch, opt.Title, new Vector2(cardRect.X + 54, cardRect.Y + 18), Color.White, 0.72f);
                Utils.DrawBorderString(spriteBatch, $"[{opt.ParentClass}]", new Vector2(cardRect.X + 54, cardRect.Y + 44), matchesClass ? opt.ThemeColor : Color.Gray, 0.72f);
                Utils.DrawBorderString(spriteBatch, "North + East Ready", new Vector2(cardRect.X + 54, cardRect.Y + 68), Color.LimeGreen, 0.68f);

                if (isHovered && Main.mouseLeft && Main.mouseLeftRelease) {
                    if (!matchesClass) {
                        Main.NewText($"You must be a {opt.ParentClass} to select this Ascendancy!", 255, 80, 80);
                    } else {
                        selectedIndex = i;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }
            }

            var activeOpt = Options[selectedIndex];
            Rectangle detailRect = new Rectangle(panelX + 476, gridStartY, panelWidth - 504, 426);
            DrawBox(spriteBatch, detailRect, new Color(22, 26, 40, 240), activeOpt.ThemeColor, 2);

            Utils.DrawBorderStringBig(spriteBatch, activeOpt.Title, new Vector2(detailRect.X + 24, detailRect.Y + 24), activeOpt.ThemeColor, 0.48f);
            Utils.DrawBorderString(spriteBatch, $"{activeOpt.Subtitle} ({activeOpt.ParentClass})", new Vector2(detailRect.X + 24, detailRect.Y + 62), Color.Gold, 0.82f);

            string[] lines = activeOpt.Description.Split('\n');
            int textY = detailRect.Y + 104;
            foreach (string rawLine in lines) {
                string wrapped = FontAssets.MouseText.Value.CreateWrappedText(rawLine, detailRect.Width - 48);
                foreach (string subLine in wrapped.Split('\n')) {
                    Utils.DrawBorderString(spriteBatch, subLine, new Vector2(detailRect.X + 24, textY), Color.White, 0.80f);
                    textY += 23;
                }
                textY += 6;
            }

            Rectangle ascendBtn = new Rectangle(detailRect.X + 40, detailRect.Bottom - 68, detailRect.Width - 80, 48);
            bool hoverAscend = ascendBtn.Contains(Main.MouseScreen.ToPoint());
            Color btnColor = hoverAscend ? activeOpt.ThemeColor * 0.75f : activeOpt.ThemeColor * 0.45f;
            DrawBox(spriteBatch, ascendBtn, btnColor, Color.White, 2);

            string btnText = $"ASCEND AS {activeOpt.Title.ToUpper()}";
            Utils.DrawBorderString(spriteBatch, btnText, ascendBtn.Center.ToVector2(), Color.White, 0.85f, 0.5f, 0.45f);

            if (hoverAscend && Main.mouseLeft && Main.mouseLeftRelease) {
                ConfirmAscendancy(player, prog, activeOpt);
            }
        }

        private void ConfirmAscendancy(Player player, AtlasProgressionPlayer prog, AscendancyOption chosen) {
            if (prog.ChosenClass != chosen.ParentClass && !string.IsNullOrEmpty(prog.ChosenClass)) {
                Main.NewText($"You cannot ascend as a {chosen.ParentClass} because your primary class is {prog.ChosenClass}!", 255, 80, 80);
                return;
            }

            int relicType = ModContent.ItemType<AscendancyRelic>();
            bool consumed = false;
            for (int i = 0; i < player.inventory.Length; i++) {
                if (player.inventory[i].type == relicType && player.inventory[i].stack > 0) {
                    player.inventory[i].stack--;
                    if (player.inventory[i].stack <= 0) player.inventory[i].TurnToAir();
                    consumed = true;
                    break;
                }
            }

            if (!consumed) {
                Main.NewText("You require an Ascendancy Relic in your inventory to ascend!", 255, 80, 80);
                return;
            }

            prog.ChosenAscendancy = chosen.Id;

            int northJewel = chosen.Id switch {
                "IceWarrior" => ModContent.ItemType<FrigidGlimmer>(),
                "WindWarrior" => ModContent.ItemType<EhecatlGaleJewel>(),
                "FireRanger" => ModContent.ItemType<XiuhtecuhtliTrapJewel>(),
                "TimeRanger" => ModContent.ItemType<OllinChronoJewel>(),
                "FlameSorcerer" => ModContent.ItemType<XiuhcoatlPyreJewel>(),
                "LightningSorcerer" => ModContent.ItemType<TezcatlipocaStormJewel>(),
                "PlantSummoner" => ModContent.ItemType<XochiquetzalBloomJewel>(),
                "LightningSummoner" => ModContent.ItemType<TlalocConduitJewel>(),
                _ => ModContent.ItemType<PlaceholderUltimateJewel>()
            };

            int eastJewel = chosen.Id switch {
                "IceWarrior" => ModContent.ItemType<GlacialEdgeJewel>(),
                "WindWarrior" => ModContent.ItemType<EhecatlTempestBladeJewel>(),
                "FireRanger" => ModContent.ItemType<XiuhtecuhtliMagmaArrowJewel>(),
                "TimeRanger" => ModContent.ItemType<OllinChronoBurstJewel>(),
                "FlameSorcerer" => ModContent.ItemType<XiuhcoatlCinderBrandJewel>(),
                "LightningSorcerer" => ModContent.ItemType<TezcatlipocaChainBoltJewel>(),
                "PlantSummoner" => ModContent.ItemType<XochiquetzalThornLashJewel>(),
                "LightningSummoner" => ModContent.ItemType<TlalocStormCallJewel>(),
                _ => ModContent.ItemType<PlaceholderOffenseJewel>()
            };

            int[] jewelTypes = chosen.Id == "IceWarrior"
                ? new int[] {
                    northJewel,
                    eastJewel,
                    ModContent.ItemType<RimeStepJewel>(),
                    ModContent.ItemType<PermafrostCarapaceJewel>()
                }
                : new int[] {
                    northJewel,
                    eastJewel,
                    ModContent.ItemType<PlaceholderUtilityJewel>(),
                    ModContent.ItemType<PlaceholderDefenseJewel>()
                };

            var source = player.GetSource_Misc("WarpedAtlas_Ascendancy");
            for (int slot = 0; slot < 4; slot++) {
                if (prog.AscendancyLoadout[slot] == null || prog.AscendancyLoadout[slot].IsAir) {
                    Item jewelItem = new Item();
                    jewelItem.SetDefaults(jewelTypes[slot]);
                    prog.AscendancyLoadout[slot] = jewelItem;
                } else {
                    player.QuickSpawnItem(source, jewelTypes[slot]);
                }
            }

            SoundEngine.PlaySound(SoundID.Item29, player.Center);
            SoundEngine.PlaySound(SoundID.Roar, player.Center);
            for (int i = 0; i < 50; i++) {
                Dust d = Dust.NewDustDirect(player.position, player.width, player.height, DustID.IceTorch, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 100, chosen.ThemeColor, 2.2f);
                d.noGravity = true;
            }

            Main.NewText($"[c/50d2ff:ASCENSION COMPLETE!] You have ascended as the {chosen.Title}! Press 'B' to view your Ascendancy Matrix.", 80, 210, 255);

            var uiSystem = ModContent.GetInstance<AtlasUISystem>();
            uiSystem.CloseAscendancySelectUI();
            uiSystem.AtlasAscendancyInterface.SetState(uiSystem.AscendancyUIState);
            Main.playerInventory = true;
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