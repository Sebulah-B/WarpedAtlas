using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader; 
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Items;
using WarpedAtlas.Content.Subworlds;

namespace WarpedAtlas.Content.UI
{
    public class AtlasIntroUI : UIState
    {
        private UIPanel panel;

        public override void OnInitialize() {
            panel = new UIPanel();
            panel.Width.Set(620f, 0f);
            panel.Height.Set(460f, 0f); 
            panel.HAlign = 0.5f;
            panel.VAlign = 0.4f;
            Append(panel);

            UIText header = new UIText("Welcome to the Warped Atlas Engine", 1.2f, large: false);
            header.HAlign = 0.5f;
            header.Top.Set(15f, 0f);
            panel.Append(header);

            string instructions = "Sandbox digging tools have been removed. Progression relies purely on combat sweeps.\n\n" +
                                    "Press '[' inside the Sky Hub to open coordinate selections. Launch using your core Waystone item.\n\n" +
                                    "Purge 75% of room monsters to clear the vault stone wall block, break the altar, and harvest weapon rewards.\n\n" +
                                   "Select your starting archetype path to manifest baseline weaponry:";
            
            UIText info = new UIText(instructions, 0.85f);
            info.Width.Set(-40f, 1f);
            info.Height.Set(180f, 0f);
            info.Left.Set(20f, 0f);
            info.Top.Set(55f, 0f);
            info.IsWrapped = true;
            panel.Append(info);

            CreateClassNode(25f,  305f, ItemID.FieryGreatsword, "Warrior",  "Warrior");
            CreateClassNode(165f, 305f, ItemID.MoltenFury,      "Ranger",   "Ranger");
            CreateClassNode(305f, 305f, ItemID.DemonScythe,    "Sorcerer", "Sorcerer");
            CreateClassNode(445f, 305f, ItemID.ImpStaff,       "Summoner", "Summoner");
        }

        private void CreateClassNode(float x, float y, int itemId, string label, string className) {
            if (itemId < TextureAssets.Item.Length) {
                Main.instance.LoadItem(itemId);
            }

            UIPanel slotBackground = new UIPanel();
            slotBackground.Left.Set(x, 0f);
            slotBackground.Top.Set(y, 0f);
            slotBackground.Width.Set(110f, 0f);
            slotBackground.Height.Set(110f, 0f);
            slotBackground.OnLeftClick += (evt, element) => SelectArchetype(className, itemId, ModContent.ItemType<WaystoneBase>());
            panel.Append(slotBackground);

            if (itemId < TextureAssets.Item.Length) {
                UIImage itemImage = new UIImage(TextureAssets.Item[itemId]);
                itemImage.HAlign = 0.5f;
                itemImage.VAlign = 0.3f;
                itemImage.IgnoresMouseInteraction = true; 
                slotBackground.Append(itemImage);
            }

            UIText labelText = new UIText(label, 0.9f);
            labelText.HAlign = 0.5f;
            labelText.Top.Set(72f, 0f); 
            labelText.IgnoresMouseInteraction = true;
            slotBackground.Append(labelText);
        }

        private void SelectArchetype(string className, int starterWeaponId, int waystoneItemId) {
            Player player = Main.LocalPlayer;
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            prog.ChosenClass = className;
            prog.HasOnboarded = true;
            BaseDungeonSubworld.ActiveProfile = null;

            player.QuickSpawnItem(player.GetSource_Misc("Onboarding"), starterWeaponId, 1);
            player.QuickSpawnItem(player.GetSource_Misc("Onboarding"), waystoneItemId, 1);

            if (className == "Ranger") {
                player.QuickSpawnItem(player.GetSource_Misc("Onboarding"), ItemID.EndlessQuiver, 1);
                player.QuickSpawnItem(player.GetSource_Misc("Onboarding"), ItemID.EndlessMusketPouch, 1);
                player.QuickSpawnItem(player.GetSource_Misc("Onboarding"), ModContent.ItemType<EndlessRocketPouch>(), 1);
            } 
            else if (className == "Summoner") {
                player.QuickSpawnItem(player.GetSource_Misc("Onboarding"), ItemID.FireWhip, 1);
            }

            Main.NewText($"  Path initialized: Welcomed to the ranks of the {className}!", 255, 215, 0);
            ModContent.GetInstance<Systems.AtlasUISystem>().HideIntroMenu();
        }
    }
}