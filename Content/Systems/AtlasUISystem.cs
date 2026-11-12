using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using SubworldLibrary;
using WarpedAtlas.Content.UI;
using WarpedAtlas.Content.Subworlds;
using WarpedAtlas.Content.NPCs;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Buffs;
using WarpedAtlas.Content.Items;

namespace WarpedAtlas.Content.Systems
{
    public class AtlasUISystem : ModSystem
    {
        internal UserInterface AtlasMapInterface;
        internal UserInterface AtlasPassiveInterface;
        internal UserInterface AtlasAscendancyInterface;
        internal UserInterface AtlasAscendancySelectInterface;
        internal UserInterface AtlasIntroInterface;
        internal UserInterface AtlasHUDInterface;
        public UserInterface AtlasSheetInterface;
        internal UserInterface AtlasEscalationInterface;
        internal UserInterface AtlasStashInterface;

        internal AtlasMapUI MapUIState;
        internal AtlasPassiveUI PassiveUIState;
        internal AtlasAscendancyUI AscendancyUIState;
        internal AtlasAscendancySelectUI AscendancySelectUIState;
        internal AtlasIntroUI IntroUIState;
        internal AtlasMonsterHUD MonsterHUDState;
        internal AtlasCharacterSheetUI SheetUIState;
        internal AtlasEscalationUI EscalationUIState;
        internal AtlasStashUI StashUIState;

        public static ModKeybind ToggleMapKey;
        public static ModKeybind ToggleTreeKey;
        public static ModKeybind ToggleSheetKey;
        public static ModKeybind ToggleAscendancyKey;
        public static ModKeybind ActivateAscendancyUltimateKey;

        public override void Load() {
            if (!Main.dedServ) {
                ToggleMapKey = KeybindLoader.RegisterKeybind(Mod, "Open Atlas Map", "[");
                ToggleTreeKey = KeybindLoader.RegisterKeybind(Mod, "Open Atlas Passives", "]");
                ToggleSheetKey = KeybindLoader.RegisterKeybind(Mod, "Open Character Sheet", "\\");
                ToggleAscendancyKey = KeybindLoader.RegisterKeybind(Mod, "Open Ascendancy Matrix", "B");
                ActivateAscendancyUltimateKey = KeybindLoader.RegisterKeybind(Mod, "Activate Ascendancy Ultimate", "V");

                AtlasMapInterface = new UserInterface();
                AtlasPassiveInterface = new UserInterface();
                AtlasAscendancyInterface = new UserInterface();
                AtlasAscendancySelectInterface = new UserInterface();
                AtlasIntroInterface = new UserInterface();
                AtlasHUDInterface = new UserInterface();
                AtlasSheetInterface = new UserInterface();
                AtlasEscalationInterface = new UserInterface();
                AtlasStashInterface = new UserInterface();

                MapUIState = new AtlasMapUI();
                PassiveUIState = new AtlasPassiveUI();
                AscendancyUIState = new AtlasAscendancyUI();
                AscendancySelectUIState = new AtlasAscendancySelectUI();
                IntroUIState = new AtlasIntroUI();
                MonsterHUDState = new AtlasMonsterHUD();
                SheetUIState = new AtlasCharacterSheetUI();
                EscalationUIState = new AtlasEscalationUI();
                StashUIState = new AtlasStashUI();

                MonsterHUDState.Activate();
                AtlasHUDInterface.SetState(MonsterHUDState);

                EscalationUIState.Activate();
                AscendancyUIState.Activate();
                AscendancySelectUIState.Activate();
                StashUIState.Activate();
            }
        }

        public override void Unload() {
            ToggleMapKey = null;
            ToggleTreeKey = null;
            ToggleSheetKey = null;
            ToggleAscendancyKey = null;
            ActivateAscendancyUltimateKey = null;

            MapUIState = null;
            PassiveUIState = null;
            AscendancyUIState = null;
            AscendancySelectUIState = null;
            IntroUIState = null;
            MonsterHUDState = null;
            SheetUIState = null;
            EscalationUIState = null;
            StashUIState = null;

            AtlasMapInterface = null;
            AtlasPassiveInterface = null;
            AtlasAscendancyInterface = null;
            AtlasAscendancySelectInterface = null;
            AtlasIntroInterface = null;
            AtlasHUDInterface = null;
            AtlasSheetInterface = null;
            AtlasEscalationInterface = null;
            AtlasStashInterface = null;
        }

        public override void PostSetupContent() {
            PassiveTreeRegistry.Initialize();
        }

        public void ShowIntroMenu() => AtlasIntroInterface?.SetState(IntroUIState);
        public void HideIntroMenu() => AtlasIntroInterface?.SetState(null);
        public void HideCharacterSheet() => AtlasSheetInterface?.SetState(null);

        public void OpenEscalationUI(EscalationAnchorNPC anchor) {
            EscalationUIState.SetAnchor(anchor);
            AtlasEscalationInterface.SetState(EscalationUIState);
        }

        public void CloseEscalationUI() {
            AtlasEscalationInterface.SetState(null);
        }

        public void OpenNativeStashUI() {
            AtlasMapInterface?.SetState(null);
            AtlasPassiveInterface?.SetState(null);
            AtlasAscendancyInterface?.SetState(null);
            AtlasAscendancySelectInterface?.SetState(null);
            AtlasSheetInterface?.SetState(null);

            Main.playerInventory = true;
            AtlasStashInterface?.SetState(StashUIState);
        }

        public void CloseNativeStashUI() {
            AtlasStashInterface?.SetState(null);
        }

        public void OpenAscendancySelectUI() {
            AtlasMapInterface?.SetState(null);
            AtlasPassiveInterface?.SetState(null);
            AtlasAscendancyInterface?.SetState(null);
            AtlasSheetInterface?.SetState(null);
            Main.playerInventory = false;

            AscendancySelectUIState?.AutoSelectPlayerClass();
            AtlasAscendancySelectInterface?.SetState(AscendancySelectUIState);
        }

        public void CloseAscendancySelectUI() {
            AtlasAscendancySelectInterface?.SetState(null);
        }

        public override void UpdateUI(GameTime gameTime) {
            if (AtlasIntroInterface?.CurrentState != null) {
                AtlasIntroInterface.Update(gameTime);
                return;
            }

            if (AtlasMapInterface?.CurrentState != null) AtlasMapInterface.Update(gameTime);
            if (AtlasPassiveInterface?.CurrentState != null) AtlasPassiveInterface.Update(gameTime);
            if (AtlasAscendancyInterface?.CurrentState != null) AtlasAscendancyInterface.Update(gameTime);
            if (AtlasAscendancySelectInterface?.CurrentState != null) AtlasAscendancySelectInterface.Update(gameTime);
            if (AtlasSheetInterface?.CurrentState != null) AtlasSheetInterface.Update(gameTime);
            if (AtlasEscalationInterface?.CurrentState != null) AtlasEscalationInterface.Update(gameTime);
            if (AtlasStashInterface?.CurrentState != null) AtlasStashInterface.Update(gameTime);

            if (SubworldSystem.IsActive<BaseDungeonSubworld>()) {
                AtlasHUDInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1) {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "WarpedAtlas: Custom UI Engine",
                    delegate {
                        if (AtlasIntroInterface?.CurrentState != null) AtlasIntroInterface.Draw(Main.spriteBatch, new GameTime());
                        if (AtlasMapInterface?.CurrentState != null) AtlasMapInterface.Draw(Main.spriteBatch, new GameTime());
                        if (AtlasPassiveInterface?.CurrentState != null) AtlasPassiveInterface.Draw(Main.spriteBatch, new GameTime());
                        if (AtlasAscendancyInterface?.CurrentState != null) AtlasAscendancyInterface.Draw(Main.spriteBatch, new GameTime());
                        if (AtlasAscendancySelectInterface?.CurrentState != null) AtlasAscendancySelectInterface.Draw(Main.spriteBatch, new GameTime());
                        if (AtlasSheetInterface?.CurrentState != null) AtlasSheetInterface.Draw(Main.spriteBatch, new GameTime());
                        if (AtlasEscalationInterface?.CurrentState != null) AtlasEscalationInterface.Draw(Main.spriteBatch, new GameTime());
                        if (AtlasStashInterface?.CurrentState != null) AtlasStashInterface.Draw(Main.spriteBatch, new GameTime());

                        if (SubworldSystem.IsActive<BaseDungeonSubworld>() && AtlasHUDInterface?.CurrentState != null) {
                            AtlasHUDInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }

    public class UIKeybindPlayer : ModPlayer
    {
        public override void ProcessTriggers(Terraria.GameInput.TriggersSet triggersSet) {
            var uiSystem = ModContent.GetInstance<AtlasUISystem>();
            if (uiSystem.AtlasIntroInterface?.CurrentState != null) return;

            if (AtlasUISystem.ToggleMapKey.JustPressed) {
                if (uiSystem.AtlasMapInterface.CurrentState == null) {
                    uiSystem.AtlasPassiveInterface.SetState(null);
                    uiSystem.AtlasAscendancyInterface.SetState(null);
                    uiSystem.AtlasAscendancySelectInterface.SetState(null);
                    uiSystem.AtlasSheetInterface.SetState(null);
                    uiSystem.AtlasStashInterface.SetState(null);
                    uiSystem.AtlasMapInterface.SetState(uiSystem.MapUIState);
                    Main.playerInventory = false;
                } else {
                    uiSystem.AtlasMapInterface.SetState(null);
                }
            }

            if (AtlasUISystem.ToggleTreeKey.JustPressed) {
                if (uiSystem.AtlasPassiveInterface.CurrentState == null) {
                    uiSystem.AtlasMapInterface.SetState(null);
                    uiSystem.AtlasAscendancyInterface.SetState(null);
                    uiSystem.AtlasAscendancySelectInterface.SetState(null);
                    uiSystem.AtlasSheetInterface.SetState(null);
                    uiSystem.AtlasStashInterface.SetState(null);
                    uiSystem.AtlasPassiveInterface.SetState(uiSystem.PassiveUIState);
                    Main.playerInventory = false;
                } else {
                    uiSystem.AtlasPassiveInterface.SetState(null);
                }
            }

            if (AtlasUISystem.ToggleAscendancyKey.JustPressed) {
                if (uiSystem.AtlasAscendancyInterface.CurrentState == null) {
                    uiSystem.AtlasMapInterface.SetState(null);
                    uiSystem.AtlasPassiveInterface.SetState(null);
                    uiSystem.AtlasAscendancySelectInterface.SetState(null);
                    uiSystem.AtlasSheetInterface.SetState(null);
                    uiSystem.AtlasStashInterface.SetState(null);
                    uiSystem.AtlasAscendancyInterface.SetState(uiSystem.AscendancyUIState);
                    Main.playerInventory = true;
                } else {
                    uiSystem.AtlasAscendancyInterface.SetState(null);
                }
            }

            if (AtlasUISystem.ToggleSheetKey.JustPressed) {
                if (uiSystem.AtlasSheetInterface.CurrentState == null) {
                    uiSystem.AtlasMapInterface.SetState(null);
                    uiSystem.AtlasPassiveInterface.SetState(null);
                    uiSystem.AtlasAscendancyInterface.SetState(null);
                    uiSystem.AtlasAscendancySelectInterface.SetState(null);
                    uiSystem.AtlasStashInterface.SetState(null);
                    uiSystem.AtlasSheetInterface.SetState(uiSystem.SheetUIState);
                    Main.playerInventory = false;
                } else {
                    uiSystem.AtlasSheetInterface.SetState(null);
                }
            }

            if (AtlasUISystem.ActivateAscendancyUltimateKey.JustPressed) {
                var prog = Player.GetModPlayer<AtlasProgressionPlayer>();
                int cooldownId = ModContent.BuffType<FrigidGlimmerCooldownDebuff>();

                if (!Player.HasBuff(cooldownId)) {
                    if (prog.HasFrigidGlimmerInUltimateSlot() && !Player.HasBuff(ModContent.BuffType<FrigidGlimmerBuff>())) {
                        Player.AddBuff(ModContent.BuffType<FrigidGlimmerBuff>(), 60 * 60);
                        SoundEngine.PlaySound(SoundID.Item29, Player.Center);
                        for (int i = 0; i < 30; i++) {
                            Dust d = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.IceTorch, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 100, default, 2f);
                            d.noGravity = true;
                        }
                    } else if (prog.HasEhecatlGaleInUltimateSlot() && !Player.HasBuff(ModContent.BuffType<EhecatlVortexBuff>())) {
                        Player.AddBuff(ModContent.BuffType<EhecatlVortexBuff>(), 45 * 60);
                        SoundEngine.PlaySound(SoundID.Item122, Player.Center);
                        for (int i = 0; i < 35; i++) {
                            Dust d = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Cloud, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 100, Color.MediumSpringGreen, 2f);
                            d.noGravity = true;
                        }
                    }
                }
            }
        }
    }
}