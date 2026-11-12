using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Items.Uniques
{
    public abstract class AtlasUniqueItem : ModItem
    {
        // PoE/Diablo Unique Orange Color
        public static readonly Color UniqueColor = new Color(175, 96, 37);
        public static readonly Color MechanicColor = new Color(255, 190, 110);

        public virtual int BossTier => 1;
        public virtual string Archetype => "Generic";
        public virtual string EffectText => "";
        public virtual string FlavorText => "";

        public override void SetDefaults() {
            SafeSetDefaults();
        }

        public virtual void SafeSetDefaults() { }

        public override void UpdateInventory(Player player) {
            var globalItem = Item.GetGlobalItem<AtlasGlobalItem>();
            globalItem.CurrentTier = ItemTier.Unique;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            var nameLine = tooltips.Find(l => l.Name == "ItemName" && l.Mod == "Terraria");
            if (nameLine != null) {
                nameLine.OverrideColor = UniqueColor;
            }

            tooltips.Insert(1, new TooltipLine(Mod, "UniqueTier", $"Unique {Archetype} Item (Tier {BossTier})") { 
                OverrideColor = UniqueColor 
            });

            if (!string.IsNullOrEmpty(EffectText)) {
                tooltips.Add(new TooltipLine(Mod, "UniqueEffect", EffectText) { 
                    OverrideColor = MechanicColor 
                });
            }

            if (!string.IsNullOrEmpty(FlavorText)) {
                tooltips.Add(new TooltipLine(Mod, "FlavorText", $"\"{FlavorText}\"") { 
                    OverrideColor = Color.Gray 
                });
            }
        }
    }
}