using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Systems;
using WarpedAtlas.Content.Buffs;

namespace WarpedAtlas.Content.Items
{
    public abstract class AscendancyJewelItem : ModItem
    {
        public virtual int RequiredSlot => 0; // 0: North/Ultimate, 1: East/Offense, 2: South/Utility, 3: West/Defense
        public virtual string SlotName => RequiredSlot switch {
            0 => "North (Ultimate)",
            1 => "East (Offense)",
            2 => "South (Utility)",
            3 => "West (Defense)",
            _ => "Any"
        };

        public virtual bool CanSocketInSlot(int slotIndex) => slotIndex == RequiredSlot;
        public virtual void ApplyAscendancyPassive(Player player, int slotIndex) { }
    }

    // =========================================================================
    // ICE WARRIOR (CRYO PALADIN) 4-JEWEL SET
    // =========================================================================

    // 0: NORTH (ULTIMATE)
    public class FrigidGlimmer : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Diamond;
        public override int RequiredSlot => 0;

        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            if (slotIndex == 0) {
                player.statDefense += 8;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "JewelSlot", $"Ascendancy Ultimate Jewel - Socket into {SlotName} (Open Matrix with 'B')") {
                OverrideColor = new Color(120, 235, 255)
            });
            tooltips.Add(new TooltipLine(Mod, "JewelEffect",
                "Ascendancy: Ice Warrior (Diamond)\n" +
                "Passive: +8 Defense\n" +
                "Active ('V'): Transform into a Blue Paladin for 60 seconds.\n" +
                "While transformed, attacks unleash waves of Deerclops Ice Spikes.\n" +
                "180-second cooldown after expiration.") {
                OverrideColor = new Color(200, 240, 255)
            });
        }
    }

    // 1: EAST (OFFENSE)
    public class GlacialEdgeJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Ruby;
        public override int RequiredSlot => 1;

        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            if (slotIndex == 1) {
                player.GetDamage(DamageClass.Melee) += 0.15f;
                player.GetArmorPenetration(DamageClass.Melee) += 8f;
                player.frostBurn = true; // Vanilla Frostburn melee/ranged imbue
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "JewelSlot", $"Ascendancy Offense Jewel - Socket into {SlotName} (Open Matrix with 'B')") {
                OverrideColor = new Color(255, 120, 120)
            });
            tooltips.Add(new TooltipLine(Mod, "JewelEffect",
                "Ascendancy: Ice Warrior (Ruby)\n" +
                "+15% Melee Damage\n" +
                "+8 Melee Armor Penetration\n" +
                "Attacks inflict Frostburn on hit.") {
                OverrideColor = new Color(200, 240, 255)
            });
        }
    }

    // 2: SOUTH (UTILITY)
    public class RimeStepJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Emerald;
        public override int RequiredSlot => 2;

        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            if (slotIndex == 2) {
                player.moveSpeed += 0.15f;
                player.iceSkate = true;
                player.buffImmune[BuffID.Chilled] = true;
                player.buffImmune[BuffID.Frozen] = true;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "JewelSlot", $"Ascendancy Utility Jewel - Socket into {SlotName} (Open Matrix with 'B')") {
                OverrideColor = new Color(120, 255, 140)
            });
            tooltips.Add(new TooltipLine(Mod, "JewelEffect",
                "Ascendancy: Ice Warrior (Emerald)\n" +
                "+15% Movement Speed\n" +
                "Grants Ice Skates mobility and immunity to Chilled and Frozen.") {
                OverrideColor = new Color(200, 240, 255)
            });
        }
    }

    // 3: WEST (DEFENSE)
    public class PermafrostCarapaceJewel : AscendancyJewelItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Sapphire;
        public override int RequiredSlot => 3;

        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            if (slotIndex == 3) {
                player.statDefense += 12;
                player.endurance += 0.06f;
                player.statLifeMax2 += 40;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "JewelSlot", $"Ascendancy Defense Jewel - Socket into {SlotName} (Open Matrix with 'B')") {
                OverrideColor = new Color(120, 175, 255)
            });
            tooltips.Add(new TooltipLine(Mod, "JewelEffect",
                "Ascendancy: Ice Warrior (Sapphire)\n" +
                "+12 Defense\n" +
                "+6% Endurance (Damage Reduction)\n" +
                "+40 Max Life") {
                OverrideColor = new Color(200, 240, 255)
            });
        }
    }

    // =========================================================================
    // PLACEHOLDER ASCENDANCY JEWELS (FOR THE OTHER 7 ASCENDANCIES)
    // =========================================================================
    public abstract class BasePlaceholderJewel : AscendancyJewelItem
    {
        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "JewelSlot", $"Placeholder Ascendancy Jewel - Socket into {SlotName}") {
                OverrideColor = new Color(255, 200, 110)
            });
            tooltips.Add(new TooltipLine(Mod, "JewelEffect",
                "Placeholder Ascendancy node. Grants baseline stats until this subclass is finalized.") {
                OverrideColor = new Color(190, 190, 190)
            });
        }
    }

    public class PlaceholderUltimateJewel : BasePlaceholderJewel {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Amber;
        public override int RequiredSlot => 0;
        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            if (slotIndex == 0) player.GetDamage(DamageClass.Generic) += 0.10f;
        }
    }

    public class PlaceholderOffenseJewel : BasePlaceholderJewel {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Ruby;
        public override int RequiredSlot => 1;
        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            if (slotIndex == 1) player.GetCritChance(DamageClass.Generic) += 8f;
        }
    }

    public class PlaceholderUtilityJewel : BasePlaceholderJewel {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Topaz;
        public override int RequiredSlot => 2;
        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            if (slotIndex == 2) player.moveSpeed += 0.10f;
        }
    }

    public class PlaceholderDefenseJewel : BasePlaceholderJewel {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Amethyst;
        public override int RequiredSlot => 3;
        public override void ApplyAscendancyPassive(Player player, int slotIndex) {
            if (slotIndex == 3) player.statDefense += 10;
        }
    }
}

namespace WarpedAtlas.Content.Buffs
{
    public class FrigidGlimmerBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Frostburn;

        public override void SetStaticDefaults() {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.statDefense += 25;
            player.endurance += 0.15f;
            player.GetDamage(DamageClass.Melee) += 0.35f;
            player.GetCritChance(DamageClass.Melee) += 15f;
            player.noKnockback = true;
            player.jumpSpeedBoost += 2.5f;
            Lighting.AddLight(player.Center, 0.2f, 0.7f, 1.0f);

            if (Main.rand.NextBool(3)) {
                Dust d = Dust.NewDustDirect(player.position, player.width, player.height, DustID.IceTorch, 0f, -1.5f, 100, Color.Cyan, 1.4f);
                d.noGravity = true;
            }

            // Apply 30s cooldown when the 60s Paladin transformation expires
            if (player.buffTime[buffIndex] == 1) {
                player.AddBuff(ModContent.BuffType<FrigidGlimmerCooldownDebuff>(), 30 * 60);
            }
        }
    }

    public class FrigidGlimmerCooldownDebuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Chilled;

        public override void SetStaticDefaults() {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
    }
}