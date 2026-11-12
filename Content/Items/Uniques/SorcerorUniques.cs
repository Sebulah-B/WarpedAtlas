using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;

namespace WarpedAtlas.Content.Items.Uniques
{
    // --- TIER 1: ASTRAL BATTERY ---
    public class AstralBattery : AtlasUniqueItem
    {
        public override int BossTier => 1;
        public override string Archetype => "Sorcerer";
        public override string Texture => "Terraria/Images/Item_" + ItemID.MagnetSphere;
        public override string EffectText => "Grants +1 Max Energy Shield per 10 Max Mana.\nEnergy Shield recharge cannot be interrupted by damage.";
        public override string FlavorText => "Limitless power, caged in mortal binding. The shield never falters.";

        public override void SafeSetDefaults() {
            Item.width = 28; Item.height = 30;
            Item.damage = 55;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            Item.useTime = 20; Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.shoot = ProjectileID.MagnetSphereBolt;
            Item.shootSpeed = 10f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            prog.MaxEnergyShield += player.statManaMax2 / 10;
            prog.EnergyShieldRechargeDelay = 0;
        }
    }

    // --- TIER 2: GEOMANCER'S OMEN ---
    public class GeomancersOmen : AtlasUniqueItem
    {
        public override int BossTier => 2;
        public override string Archetype => "Sorcerer";
        public override string Texture => "Terraria/Images/Item_" + ItemID.MeteorStaff;
        public override string EffectText => "While Energy Shield is active, gain +25% Damage Reduction but -40% Movement Speed.";
        public override string FlavorText => "Root yourself in the arcane bedrock.";

        public override void SafeSetDefaults() {
            Item.width = 48; Item.height = 48;
            Item.damage = 50;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.useTime = 28; Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.shoot = ProjectileID.Meteor1;
            Item.shootSpeed = 14f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.CurrentEnergyShield > 0) {
                player.endurance += 0.25f;
                player.moveSpeed -= 0.40f;
            }
        }
    }

    // --- TIER 3: MIRAGE WEAVER ---
    public class MirageWeaver : AtlasUniqueItem
    {
        public override int BossTier => 3;
        public override string Archetype => "Sorcerer";
        public override string Texture => "Terraria/Images/Item_" + ItemID.MagicMissile;
        public override string EffectText => "Grants +1% Flat Evasion Chance for every 10 Max Mana.";
        public override string FlavorText => "They cannot kill what isn't truly there.";

        public override void SafeSetDefaults() {
            Item.width = 38; Item.height = 38;
            Item.damage = 60;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            Item.useTime = 22; Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shoot = ProjectileID.MagicMissile;
            Item.shootSpeed = 10f;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (player.statManaMax2 > 0) {
                float evasionBonus = (player.statManaMax2 / 10) * 0.01f;
                prog.EvasionChance += evasionBonus;
            }
        }
    }

    // --- TIER 4: SERAPH'S GAZE ---
    public class SeraphsGaze : AtlasUniqueItem
    {
        public override int BossTier => 4;
        public override string Archetype => "Sorcerer";
        public override string Texture => "Terraria/Images/Item_" + ItemID.RainbowRod;
        public override string EffectText => "Grants up to +40% Magic Critical Strike Chance based on missing Energy Shield.";
        public override string FlavorText => "Judgment requires no incantation. Only sight.";

        public override void SafeSetDefaults() {
            Item.width = 40; Item.height = 40;
            Item.damage = 85;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 18;
            Item.useTime = 24; Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shoot = ProjectileID.PhantasmalBolt;
            Item.shootSpeed = 15f;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.MaxEnergyShield > 0) {
                float missingEsPercent = 1f - (prog.CurrentEnergyShield / prog.MaxEnergyShield);
                player.GetCritChance(DamageClass.Magic) += (missingEsPercent * 40f);
            }
        }
    }

    // --- TIER 5: ABSOLUTE ZERO ---
    public class AbsoluteZero : AtlasUniqueItem
    {
        public override int BossTier => 5;
        public override string Archetype => "Sorcerer";
        public override string Texture => "Terraria/Images/Item_" + ItemID.FrostStaff;
        public override string EffectText => "+50% Magic Damage while Energy Shield is active, but Energy Shield cannot recharge.";
        public override string FlavorText => "Power preserved perfectly in ice, at the cost of blood.";

        public override void SafeSetDefaults() {
            Item.width = 46; Item.height = 46;
            Item.damage = 95;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 14;
            Item.useTime = 20; Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shoot = ProjectileID.FrostBlastFriendly;
            Item.shootSpeed = 11f;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.CurrentEnergyShield > 0) {
                player.GetDamage(DamageClass.Magic) += 0.50f;
                prog.EnergyShieldRechargeDelay = 180;
            }
        }
    }

    // --- TIER 6: PYROMANCER'S BRAND ---
    public class PyromancersBrand : AtlasUniqueItem
    {
        public override int BossTier => 6;
        public override string Archetype => "Sorcerer";
        public override string Texture => "Terraria/Images/Item_" + ItemID.DemonScythe;
        public override string EffectText => "Converts all Defense directly into Magic Critical Strike Chance.";
        public override string FlavorText => "A scorched brand that offers no protection, only devastation.";

        public override void SafeSetDefaults() {
            Item.width = 28; Item.height = 32;
            Item.damage = 105;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 14;
            Item.useTime = 18; Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shoot = ProjectileID.DemonScythe;
            Item.shootSpeed = 12f;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            int currentDef = player.statDefense;
            if (currentDef > 0) {
                player.GetCritChance(DamageClass.Magic) += currentDef;
                player.statDefense -= currentDef;
            }
        }
    }

    // --- TIER 7: GEODE CHANNELER ---
    public class GeodeChanneler : AtlasUniqueItem
    {
        public override int BossTier => 7;
        public override string Archetype => "Sorcerer";
        public override string Texture => "Terraria/Images/Item_" + ItemID.MeteorStaff;
        public override string EffectText => "Grants +1% Magic Damage for every 10 Max Mana.";
        public override string FlavorText => "Channel the resonance of the planet's core.";

        public override void SafeSetDefaults() {
            Item.width = 46; Item.height = 46;
            Item.damage = 110;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.useTime = 10; Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shoot = ProjectileID.ShadowBeamFriendly;
            Item.shootSpeed = 16f;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            float manaBonus = (player.statManaMax2 / 10) * 0.01f;
            player.GetDamage(DamageClass.Magic) += manaBonus;
        }
    }

    // --- TIER 8: VOID BLIGHT ---
    public class VoidBlight : AtlasUniqueItem
    {
        public override int BossTier => 8;
        public override string Archetype => "Sorcerer";
        public override string Texture => "Terraria/Images/Item_" + ItemID.MagnetSphere;
        public override string EffectText => "Doubles Max Energy Shield and grants +50% Magic Damage, but continuously drains Energy Shield.";
        public override string FlavorText => "An anomaly that consumes everything, including its master's barriers.";

        public override void SafeSetDefaults() {
            Item.width = 28; Item.height = 28;
            Item.damage = 140;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 25;
            Item.useTime = 35; Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shoot = ProjectileID.MagnetSphereBolt;
            Item.shootSpeed = 10f;
            Item.value = Item.sellPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.MaxEnergyShield > 0) {
                prog.MaxEnergyShield *= 2;
                player.GetDamage(DamageClass.Magic) += 0.50f;

                prog.CurrentEnergyShield -= 1.5f;
                if (prog.CurrentEnergyShield < 0) prog.CurrentEnergyShield = 0;
            }
        }
    }
}