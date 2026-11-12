using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Audio;
using WarpedAtlas.Content.Subworlds;
using WarpedAtlas.Content.Players;
using SubworldLibrary;

namespace WarpedAtlas.Content.UI
{
    public enum NodeState { Locked, Unlocked, Completed }

    public class MapLineCanvas : UIElement
    {
        public List<UIMapNode> Nodes = new List<UIMapNode>();

        protected override void DrawSelf(SpriteBatch spriteBatch) {
            base.DrawSelf(spriteBatch);
            for (int i = 0; i < Nodes.Count - 1; i++) {
                Vector2 start = Nodes[i].GetDimensions().Center();
                Vector2 end = Nodes[i + 1].GetDimensions().Center();

                Color lineColor = Nodes[i + 1].State == NodeState.Locked
                    ? new Color(60, 60, 60, 150)
                    : new Color(200, 160, 60, 255);
                Terraria.Utils.DrawLine(spriteBatch, start, end, lineColor, lineColor, 4f);
            }
        }
    }

    public class UIMapNode : UIPanel
    {
        public MapProfile Profile;
        public int Tier;
        public int ZoneIndex;
        public string LabelText;
        public bool IsBossNode;
        public NodeState State;
        private UIText tooltipReference;
        private UIImage iconImage;

        public UIMapNode(int itemId, MapProfile profile, int tier, int zoneIndex, string label, bool isBossNode, NodeState state, UIText tooltip) {
            Profile = profile;
            Tier = tier;
            ZoneIndex = zoneIndex;
            LabelText = label;
            IsBossNode = isBossNode;
            State = state;
            tooltipReference = tooltip;
            Width.Set(56f, 0f);
            Height.Set(56f, 0f);

            Main.instance.LoadItem(itemId);
            iconImage = new UIImage(TextureAssets.Item[itemId]);
            iconImage.HAlign = 0.5f;
            iconImage.VAlign = 0.5f;
            iconImage.IgnoresMouseInteraction = true;
            Append(iconImage);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (Main.LocalPlayer != null && Main.LocalPlayer.active) {
                var prog = Main.LocalPlayer.GetModPlayer<AtlasProgressionPlayer>();
                if (prog.HighestUnlockedAct > Tier || (prog.HighestUnlockedAct == Tier && prog.HighestUnlockedZone > ZoneIndex)) {
                    State = NodeState.Completed;
                } else if (prog.HighestUnlockedAct == Tier && prog.HighestUnlockedZone == ZoneIndex) {
                    State = NodeState.Unlocked;
                } else {
                    State = NodeState.Locked;
                }
            }

            if (iconImage != null) {
                iconImage.Color = State == NodeState.Locked ? Color.DarkGray * 0.5f : Color.White;
            }

            if (State == NodeState.Completed) {
                BorderColor = IsBossNode ? new Color(255, 50, 50, 255) : new Color(255, 215, 0, 255);
                BackgroundColor = new Color(60, 50, 20, 230);
            } else if (State == NodeState.Unlocked) {
                BorderColor = IsMouseHovering ? Color.White : new Color(0, 255, 255, 255);
                BackgroundColor = new Color(20, 50, 90, 230);
            } else {
                BorderColor = new Color(60, 60, 60, 255);
                BackgroundColor = new Color(20, 20, 20, 230);
            }
        }

        public override void MouseOver(UIMouseEvent evt) {
            base.MouseOver(evt);
            SoundEngine.PlaySound(SoundID.MenuTick);
            string status = State switch {
                NodeState.Completed => " [Completed - Click to Replay]",
                NodeState.Unlocked => " [Deploy Ready]",
                _ => " [Locked]"
            };
            tooltipReference.SetText(LabelText + status);
        }

        public override void MouseOut(UIMouseEvent evt) {
            base.MouseOut(evt);
            tooltipReference.SetText("Select a destination on the sacred web...");
        }
    }

    public class AtlasMapUI : UIState
    {
        private UIPanel panel;
        private MapLineCanvas lineCanvas;
        private UIText descriptionText;
        private UIText actTitleText;
        private UIText prevButton;
        private UIText nextButton;

        public int CurrentActTab = 1;

        public override void OnInitialize() {
            panel = new UIPanel();
            panel.Width.Set(660f, 0f);
            panel.Height.Set(340f, 0f);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.5f;
            Append(panel);

            actTitleText = new UIText("Campaign: Act I", 1.3f, large: false);
            actTitleText.Top.Set(20f, 0f);
            actTitleText.HAlign = 0.5f;
            panel.Append(actTitleText);

            descriptionText = new UIText("Select a destination on the sacred web...", 0.95f);
            descriptionText.Top.Set(280f, 0f);
            descriptionText.HAlign = 0.5f;
            descriptionText.TextColor = Color.LightGray;
            panel.Append(descriptionText);

            lineCanvas = new MapLineCanvas();
            lineCanvas.Width.Set(0f, 1f);
            lineCanvas.Height.Set(200f, 0f);
            lineCanvas.Top.Set(60f, 0f);
            panel.Append(lineCanvas);

            prevButton = new UIText("< Previous Act", 1f);
            prevButton.Top.Set(20f, 0f);
            prevButton.Left.Set(20f, 0f);
            prevButton.OnMouseOver += (evt, el) => prevButton.TextColor = Color.Gold;
            prevButton.OnMouseOut += (evt, el) => prevButton.TextColor = Color.White;
            prevButton.OnLeftClick += (evt, element) => {
                if (CurrentActTab > 1) {
                    CurrentActTab--;
                    BuildActNodes();
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            };

            nextButton = new UIText("Next Act >", 1f);
            nextButton.Top.Set(20f, 0f);
            nextButton.Left.Set(-120f, 1f);
            nextButton.OnMouseOver += (evt, el) => nextButton.TextColor = Color.Gold;
            nextButton.OnMouseOut += (evt, el) => nextButton.TextColor = Color.White;
            nextButton.OnLeftClick += (evt, element) => {
                var prog = Main.LocalPlayer.GetModPlayer<AtlasProgressionPlayer>();
                if (CurrentActTab < 8 && prog.HighestUnlockedAct > CurrentActTab) {
                    CurrentActTab++;
                    BuildActNodes();
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            };
        }

        public override void OnActivate() {
            base.OnActivate();
            if (Main.LocalPlayer != null && Main.LocalPlayer.active) {
                var prog = Main.LocalPlayer.GetModPlayer<AtlasProgressionPlayer>();
                CurrentActTab = Math.Clamp(prog.HighestUnlockedAct, 1, 8);
                BuildActNodes();
            }
        }

        public void BuildActNodes() {
            if (lineCanvas == null || Main.LocalPlayer == null || !Main.LocalPlayer.active) return;

            lineCanvas.RemoveAllChildren();
            lineCanvas.Nodes.Clear();

            var prog = Main.LocalPlayer.GetModPlayer<AtlasProgressionPlayer>();
            int activeTab = CurrentActTab;

            bool actCompleted = prog.HighestUnlockedAct > activeTab;
            panel.BackgroundColor = actCompleted ? new Color(30, 25, 15, 245) : new Color(15, 20, 35, 245);
            panel.BorderColor = actCompleted ? new Color(255, 180, 50, 255) : new Color(100, 140, 200, 255);
            actTitleText.TextColor = actCompleted ? new Color(255, 215, 0) : Color.White;
            actTitleText.SetText($"Campaign: Act {activeTab} (Unlocked: Act {prog.HighestUnlockedAct}, Zone {prog.HighestUnlockedZone})");

            prevButton.Remove();
            nextButton.Remove();

            if (activeTab > 1) {
                panel.Append(prevButton);
            }
            if (activeTab < 8 && actCompleted) {
                panel.Append(nextButton);
            }

            MapProfile actProfile = activeTab switch {
                1 => new DungeonProfile(),
                2 => new LihzahrdProfile(),
                3 => new SandstoneProfile(),
                4 => new HallowedProfile(),
                5 => new CryoFrostProfile(),
                6 => new HellforgeProfile(),
                7 => new GraniteProfile(),
                8 => new FungalProfile(),
                _ => new DungeonProfile()
            };

            string actName = actProfile.DisplayName;

            var node1 = CreateNode(60f, 100f, GetIcon(activeTab, 1), actProfile, activeTab, 1, $"{actName} - Outskirts", false);
            var node2 = CreateNode(200f, 30f, GetIcon(activeTab, 2), actProfile, activeTab, 2, $"{actName} - Depths", false);
            var node3 = CreateNode(340f, 100f, GetIcon(activeTab, 3), actProfile, activeTab, 3, $"{actName} - Sanctum", false);
            var bossNode = CreateNode(480f, 30f, GetIcon(activeTab, 4), actProfile, activeTab, 4, $"{actName} - Apex Domain", true);

            lineCanvas.Append(node1);
            lineCanvas.Append(node2);
            lineCanvas.Append(node3);
            lineCanvas.Append(bossNode);

            lineCanvas.Nodes.Add(node1);
            lineCanvas.Nodes.Add(node2);
            lineCanvas.Nodes.Add(node3);
            lineCanvas.Nodes.Add(bossNode);
        }

        private int GetIcon(int act, int zone) {
            if (act == 1) return zone switch { 1 => ItemID.Tombstone, 2 => ItemID.Bone, 3 => ItemID.Skull, _ => ItemID.DemonHeart };
            if (act == 2) return zone switch { 1 => ItemID.JungleSpores, 2 => ItemID.Vine, 3 => ItemID.LihzahrdPowerCell, _ => ItemID.EyeoftheGolem };
            if (act == 3) return zone switch { 1 => ItemID.SandBlock, 2 => ItemID.AntlionMandible, 3 => ItemID.FossilOre, _ => ItemID.PharaohsMask };
            if (act == 4) return zone switch { 1 => ItemID.CrystalShard, 2 => ItemID.PixieDust, 3 => ItemID.UnicornHorn, _ => ItemID.HallowedBar };
            if (act == 5) return zone switch { 1 => ItemID.IceBlock, 2 => ItemID.Shiverthorn, 3 => ItemID.FrostCore, _ => ItemID.IceSickle };
            if (act == 6) return zone switch { 1 => ItemID.AshBlock, 2 => ItemID.Hellstone, 3 => ItemID.ObsidianRose, _ => ItemID.LivingFireBlock };
            if (act == 7) return zone switch { 1 => ItemID.Granite, 2 => ItemID.Geode, 3 => ItemID.NightVisionHelmet, _ => ItemID.ThunderSpear };
            if (act == 8) return zone switch { 1 => ItemID.GlowingMushroom, 2 => ItemID.TruffleWorm, 3 => ItemID.ShroomiteBar, _ => ItemID.Autohammer };
            return ItemID.DirtBlock;
        }

        private UIMapNode CreateNode(float x, float y, int itemId, MapProfile profile, int tier, int zoneIndex, string label, bool isBoss) {
            var prog = Main.LocalPlayer.GetModPlayer<AtlasProgressionPlayer>();

            NodeState initialState = NodeState.Locked;
            if (prog.HighestUnlockedAct > tier || (prog.HighestUnlockedAct == tier && prog.HighestUnlockedZone > zoneIndex)) {
                initialState = NodeState.Completed;
            } else if (prog.HighestUnlockedAct == tier && prog.HighestUnlockedZone == zoneIndex) {
                initialState = NodeState.Unlocked;
            }

            var node = new UIMapNode(itemId, profile, tier, zoneIndex, label, isBoss, initialState, descriptionText);
            node.Left.Set(x, 0f);
            node.Top.Set(y, 0f);

            node.OnLeftClick += (evt, element) => {
                var liveProg = Main.LocalPlayer.GetModPlayer<AtlasProgressionPlayer>();
                bool isUnlocked = liveProg.HighestUnlockedAct > tier || (liveProg.HighestUnlockedAct == tier && liveProg.HighestUnlockedZone >= zoneIndex);

                if (!isUnlocked) {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    Main.NewText("This zone is sealed. Complete the preceding zones first.", 255, 100, 100);
                    return;
                }

                SoundEngine.PlaySound(SoundID.Chat);
                BaseDungeonSubworld.ActiveProfile = profile;
                BaseDungeonSubworld.CurrentMapTier = tier;
                BaseDungeonSubworld.CurrentZoneIndex = zoneIndex;

                liveProg.ActiveRunAct = tier;
                liveProg.ActiveRunZone = zoneIndex;

                Main.NewText($"  Deploying to: {label} (Act {tier}, Zone {zoneIndex})", 0, 255, 100);
                ModContent.GetInstance<Systems.AtlasUISystem>().AtlasMapInterface.SetState(null);

                if (isBoss) SubworldSystem.Enter<BossDomainSubworld>();
                else SubworldSystem.Enter<BaseDungeonSubworld>();
            };
            return node;
        }
    }
}