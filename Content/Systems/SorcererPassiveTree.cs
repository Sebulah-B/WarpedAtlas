using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using static WarpedAtlas.Content.Systems.PassiveTreeRegistry;

namespace WarpedAtlas.Content.Systems
{
    public static class SorcererPassiveTree
    {
        public static void Build() {
            int rootId = 3000;
            AddNode(rootId, "Sorcerer's Origin", "+5% Magic Damage, +10 Energy Shield", new Point(0, 0), ItemID.AmethystStaff, NodeType.Origin, p => p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 10, p => p.GetDamage(DamageClass.Magic) += 0.05f);
            
            int id = 3001;
            
            int[] manaIcons = { ItemID.ManaCrystal, ItemID.BandofStarpower, ItemID.ManaFlower, ItemID.Star };
            int[] magDmgIcons = { ItemID.SorcererEmblem, ItemID.FallenStar, ItemID.AmethystStaff, ItemID.SapphireStaff };
            int[] magCritIcons = { ItemID.WizardHat, ItemID.MagicPowerPotion, ItemID.CelestialMagnet };
            int[] castSpdIcons = { ItemID.LaserMachinegun, ItemID.SpaceGun, ItemID.Meteorite };
            int[] esIcons = { ItemID.CobaltShield, ItemID.ObsidianShield, ItemID.AnkhShield };
            int[] lifeIcons = { ItemID.LifeCrystal, ItemID.LifeFruit, ItemID.Heart };
            int[] regenIcons = { ItemID.BandofRegeneration, ItemID.CharmofMyths };
            int[] defIcons = { ItemID.IronskinPotion, ItemID.Shackle };
            int[] penIcons = { ItemID.SharkToothNecklace, ItemID.StingerNecklace };

            // --- DIR 0: ARCHMAGE (NORTH) ---
            int t0 = BuildClassBranch(rootId, ref id, 0, new BranchBlueprint { 
                R1BaseName = "+10 Max Mana", R1BaseIcons = manaIcons, R1BaseStats = p => p.statManaMax2 += 10,
                R1LeftName = "-2% Mana Cost", R1LeftIcons = new int[] { ItemID.NaturesGift, ItemID.ManaPotion }, R1LeftStats = p => p.manaCost -= 0.02f,
                R1RightName = "+2% Magic Damage", R1RightIcons = magDmgIcons, R1RightStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                MinorKeyName = "Arcane Battery", MinorKeyDesc = "+100 Max Mana, +20% Magic Damage, -10 Defense.", MinorKeyIcon = ItemID.ManaFlower, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_ArcaneBattery = true,
                R2BaseName = "+10 Max Mana", R2BaseIcons = manaIcons, R2BaseStats = p => p.statManaMax2 += 10,
                R2LeftName = "-2% Mana Cost", R2LeftIcons = new int[] { ItemID.NaturesGift, ItemID.ManaPotion }, R2LeftStats = p => p.manaCost -= 0.02f,
                R2RightName = "+2% Magic Damage", R2RightIcons = magDmgIcons, R2RightStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                MajorKeyName = "Archmage", MajorKeyDesc = "+1% Magic Damage per 10 Max Mana.", MajorKeyIcon = ItemID.CelestialCuffs, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Archmage = true
            });

            // Outer Fork 0: The Arcane Well
            BuildClassBranch(t0, ref id, 0, new BranchBlueprint {
                R1BaseName = "+10 Max Mana", R1BaseIcons = manaIcons, R1BaseStats = p => p.statManaMax2 += 10,
                R1LeftName = "+10 Max Mana", R1LeftIcons = manaIcons, R1LeftStats = p => p.statManaMax2 += 10,
                R1RightName = "+10 Max Mana", R1RightIcons = manaIcons, R1RightStats = p => p.statManaMax2 += 10,
                MinorKeyName = "Clarity", MinorKeyDesc = "+50 Max Mana.", MinorKeyIcon = ItemID.ManaCrystal, MinorKeyStats = p => p.statManaMax2 += 50,
                R2BaseName = "+10 Max Mana", R2BaseIcons = manaIcons, R2BaseStats = p => p.statManaMax2 += 10,
                R2LeftName = "+10 Max Mana", R2LeftIcons = manaIcons, R2LeftStats = p => p.statManaMax2 += 10,
                R2RightName = "+10 Max Mana", R2RightIcons = manaIcons, R2RightStats = p => p.statManaMax2 += 10,
                MajorKeyName = "Mind Over Matter", MajorKeyDesc = "50% of incoming damage is deducted from Mana before Life.", MajorKeyIcon = ItemID.ArcaneRuneWall, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_MindOverMatter = true
            });

            // --- DIR 1: GLASS CANNON (NORTHEAST) ---
            int t1 = BuildClassBranch(rootId, ref id, 1, new BranchBlueprint { 
                R1BaseName = "+2% Magic Damage", R1BaseIcons = magDmgIcons, R1BaseStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                R1LeftName = "+1% Magic Crit Chance", R1LeftIcons = magCritIcons, R1LeftStats = p => p.GetCritChance(DamageClass.Magic) += 1f,
                R1RightName = "+5 Max Energy Shield", R1RightIcons = esIcons, R1RightStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 5,
                MinorKeyName = "Glass Cannon", MinorKeyDesc = "+40% Magic Damage, -50% Max Life.", MinorKeyIcon = ItemID.DemonScythe, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_GlassCannon = true,
                R2BaseName = "+2% Magic Damage", R2BaseIcons = magDmgIcons, R2BaseStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                R2LeftName = "+1% Magic Crit Chance", R2LeftIcons = magCritIcons, R2LeftStats = p => p.GetCritChance(DamageClass.Magic) += 1f,
                R2RightName = "+5 Max Energy Shield", R2RightIcons = esIcons, R2RightStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 5,
                MajorKeyName = "Mana Shield", MajorKeyDesc = "Max Mana is added to Max Energy Shield.", MajorKeyIcon = ItemID.MagicCuffs, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_ManaShield = true
            });

            // Outer Fork 1: The Ascendant
            BuildClassBranch(t1, ref id, 1, new BranchBlueprint {
                R1BaseName = "+2% Magic Damage", R1BaseIcons = magDmgIcons, R1BaseStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                R1LeftName = "+2% Magic Damage", R1LeftIcons = magDmgIcons, R1LeftStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                R1RightName = "+2% Magic Damage", R1RightIcons = magDmgIcons, R1RightStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                MinorKeyName = "Evocation", MinorKeyDesc = "+20% Magic Damage, -10 Defense.", MinorKeyIcon = ItemID.SorcererEmblem, MinorKeyStats = p => { p.GetDamage(DamageClass.Magic) += 0.20f; p.statDefense -= (int)p.statDefense; },
                R2BaseName = "+2% Magic Damage", R2BaseIcons = magDmgIcons, R2BaseStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                R2LeftName = "+2% Magic Damage", R2LeftIcons = magDmgIcons, R2LeftStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                R2RightName = "+2% Magic Damage", R2RightIcons = magDmgIcons, R2RightStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                MajorKeyName = "Apotheosis", MajorKeyDesc = "Max Life becomes 1. Grants +100% Magic Damage.", MajorKeyIcon = ItemID.LastPrism, MajorKeyStats = p => { p.statLifeMax2 = 1; p.GetDamage(DamageClass.Magic) += 1.00f; }
            });

            // --- DIR 2: TIME WEAVER (EAST) ---
            int t2 = BuildClassBranch(rootId, ref id, 2, new BranchBlueprint { 
                R1BaseName = "+1% Magic Attack Speed", R1BaseIcons = castSpdIcons, R1BaseStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                R1LeftName = "+2% Magic Damage", R1LeftIcons = magDmgIcons, R1LeftStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                R1RightName = "+1% Magic Crit Chance", R1RightIcons = magCritIcons, R1RightStats = p => p.GetCritChance(DamageClass.Magic) += 1f,
                MinorKeyName = "Channeler", MinorKeyDesc = "+30% Magic Attack Speed. Spell Tomes deal 0 damage.", MinorKeyIcon = ItemID.SpaceGun, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Channeler = true,
                R2BaseName = "+1% Magic Attack Speed", R2BaseIcons = castSpdIcons, R2BaseStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                R2LeftName = "+2% Magic Damage", R2LeftIcons = magDmgIcons, R2LeftStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                R2RightName = "+1% Magic Crit Chance", R2RightIcons = magCritIcons, R2RightStats = p => p.GetCritChance(DamageClass.Magic) += 1f,
                MajorKeyName = "Time Warp", MajorKeyDesc = "+30% Magic Attack Speed.", MajorKeyIcon = ItemID.CelestialEmblem, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_TimeWarp = true
            });

            // Outer Fork 2: The Chronomancer
            BuildClassBranch(t2, ref id, 2, new BranchBlueprint {
                R1BaseName = "+1% Magic Attack Speed", R1BaseIcons = castSpdIcons, R1BaseStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                R1LeftName = "+1% Magic Attack Speed", R1LeftIcons = castSpdIcons, R1LeftStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                R1RightName = "+1% Magic Attack Speed", R1RightIcons = castSpdIcons, R1RightStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                MinorKeyName = "Quickcast", MinorKeyDesc = "+15% Magic Attack Speed.", MinorKeyIcon = ItemID.Stopwatch, MinorKeyStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.15f,
                R2BaseName = "+1% Magic Attack Speed", R2BaseIcons = castSpdIcons, R2BaseStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                R2LeftName = "+1% Magic Attack Speed", R2LeftIcons = castSpdIcons, R2LeftStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                R2RightName = "+1% Magic Attack Speed", R2RightIcons = castSpdIcons, R2RightStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                MajorKeyName = "Haste", MajorKeyDesc = "Converts all Movement Speed bonuses directly into Magic Attack Speed.", MajorKeyIcon = ItemID.FastClock, MajorKeyStats = p => { float bonus = p.moveSpeed - 1f; if(bonus > 0) { p.GetAttackSpeed(DamageClass.Magic) += bonus; p.moveSpeed = 1f; } }
            });

            // --- DIR 3: LIBRARIAN (SOUTHEAST) ---
            int t3 = BuildClassBranch(rootId, ref id, 3, new BranchBlueprint { 
                R1BaseName = "+2% Tome Damage", R1BaseIcons = new int[] { ItemID.Book, ItemID.WaterBolt, ItemID.DemonScythe }, R1BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusTomeDamage += 0.02f,
                R1LeftName = "+2 Armor Penetration", R1LeftIcons = penIcons, R1LeftStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R1RightName = "+10 Max Mana", R1RightIcons = manaIcons, R1RightStats = p => p.statManaMax2 += 10,
                MinorKeyName = "Librarian", MinorKeyDesc = "Non-Tome Magic weapons deal 0 damage.", MinorKeyIcon = ItemID.BookofSkulls, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Librarian = true,
                R2BaseName = "+2% Tome Damage", R2BaseIcons = new int[] { ItemID.Book, ItemID.WaterBolt, ItemID.DemonScythe }, R2BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusTomeDamage += 0.02f,
                R2LeftName = "+2 Armor Penetration", R2LeftIcons = penIcons, R2LeftStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R2RightName = "+10 Max Mana", R2RightIcons = manaIcons, R2RightStats = p => p.statManaMax2 += 10,
                MajorKeyName = "Elementalist", MajorKeyDesc = "Magic attacks inflict Burn.", MajorKeyIcon = ItemID.InfernoFork, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Elementalist = true
            });

            // Outer Fork 3: The Scholar
            BuildClassBranch(t3, ref id, 3, new BranchBlueprint {
                R1BaseName = "+2% Tome Damage", R1BaseIcons = new int[] { ItemID.Book }, R1BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusTomeDamage += 0.02f,
                R1LeftName = "+2 Armor Penetration", R1LeftIcons = penIcons, R1LeftStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R1RightName = "+2 Armor Penetration", R1RightIcons = penIcons, R1RightStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                MinorKeyName = "Piercing Insight", MinorKeyDesc = "+15 Armor Penetration.", MinorKeyIcon = ItemID.CrystalStorm, MinorKeyStats = p => p.GetArmorPenetration(DamageClass.Generic) += 15f,
                R2BaseName = "+2% Tome Damage", R2BaseIcons = new int[] { ItemID.Book }, R2BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusTomeDamage += 0.02f,
                R2LeftName = "+2 Armor Penetration", R2LeftIcons = penIcons, R2LeftStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R2RightName = "+2 Armor Penetration", R2RightIcons = penIcons, R2RightStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                MajorKeyName = "Forbidden Knowledge", MajorKeyDesc = "+50% Spell Tome Damage, -20 Defense.", MajorKeyIcon = ItemID.CursedFlames, MajorKeyStats = p => { p.GetModPlayer<AtlasProgressionPlayer>().BonusTomeDamage += 0.50f; p.statDefense -= 20; }
            });

            // --- DIR 4: BULWARK (SOUTH) ---
            int t4 = BuildClassBranch(rootId, ref id, 4, new BranchBlueprint { 
                R1BaseName = "+10 Max Energy Shield", R1BaseIcons = esIcons, R1BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 10,
                R1LeftName = "+2 Defense", R1LeftIcons = defIcons, R1LeftStats = p => p.statDefense += 2,
                R1RightName = "+1% Damage Reduction", R1RightIcons = new int[] { ItemID.WormScarf, ItemID.EndurancePotion }, R1RightStats = p => p.endurance += 0.01f,
                MinorKeyName = "Aegis Dome", MinorKeyDesc = "+50 Max Energy Shield.", MinorKeyIcon = ItemID.ObsidianShield, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 50,
                R2BaseName = "+10 Max Energy Shield", R2BaseIcons = esIcons, R2BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 10,
                R2LeftName = "+2 Defense", R2LeftIcons = defIcons, R2LeftStats = p => p.statDefense += 2,
                R2RightName = "+1% Damage Reduction", R2RightIcons = new int[] { ItemID.WormScarf, ItemID.EndurancePotion }, R2RightStats = p => p.endurance += 0.01f,
                MajorKeyName = "Bulwark", MajorKeyDesc = "+100 Max Energy Shield, -10% Magic Damage.", MajorKeyIcon = ItemID.AnkhShield, MajorKeyStats = p => { p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 100; p.GetDamage(DamageClass.Magic) -= 0.10f; }
            });

            // Outer Fork 4: The Void Walker
            BuildClassBranch(t4, ref id, 4, new BranchBlueprint {
                R1BaseName = "+10 Max Energy Shield", R1BaseIcons = esIcons, R1BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 10,
                R1LeftName = "+10 Max Energy Shield", R1LeftIcons = esIcons, R1LeftStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 10,
                R1RightName = "+10 Max Energy Shield", R1RightIcons = esIcons, R1RightStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 10,
                MinorKeyName = "Volatile Shields", MinorKeyDesc = "+2% Magic Damage per 10 Energy Shield.", MinorKeyIcon = ItemID.MagnetSphere, MinorKeyStats = p => p.GetDamage(DamageClass.Magic) += (p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield / 10) * 0.02f,
                R2BaseName = "+10 Max Energy Shield", R2BaseIcons = esIcons, R2BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 10,
                R2LeftName = "+10 Max Energy Shield", R2LeftIcons = esIcons, R2LeftStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 10,
                R2RightName = "+10 Max Energy Shield", R2RightIcons = esIcons, R2RightStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxEnergyShield += 10,
                MajorKeyName = "Overload", MajorKeyDesc = "Casting spells at Max Energy Shield shatters the shield into explosive damage.", MajorKeyIcon = ItemID.NebulaArcanum, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Overload = true
            });

            // --- DIR 5: BLOOD MAGE (SOUTHWEST) ---
            int t5 = BuildClassBranch(rootId, ref id, 5, new BranchBlueprint { 
                R1BaseName = "+10 Max Life", R1BaseIcons = lifeIcons, R1BaseStats = p => p.statLifeMax2 += 10,
                R1LeftName = "+2% Magic Damage", R1LeftIcons = magDmgIcons, R1LeftStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                R1RightName = "+1 Life Regeneration", R1RightIcons = regenIcons, R1RightStats = p => p.lifeRegen += 1,
                MinorKeyName = "Blood Sacrifice", MinorKeyDesc = "+15% Magic Damage, -20 Max Life.", MinorKeyIcon = ItemID.VampireKnives, MinorKeyStats = p => { p.GetDamage(DamageClass.Magic) += 0.15f; p.statLifeMax2 -= 20; },
                R2BaseName = "+10 Max Life", R2BaseIcons = lifeIcons, R2BaseStats = p => p.statLifeMax2 += 10,
                R2LeftName = "+2% Magic Damage", R2LeftIcons = magDmgIcons, R2LeftStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                R2RightName = "+1 Life Regeneration", R2RightIcons = regenIcons, R2RightStats = p => p.lifeRegen += 1,
                MajorKeyName = "Blood Magic", MajorKeyDesc = "+50% Magic Damage, Halves Max Mana.", MajorKeyIcon = ItemID.CrimsonHeart, MajorKeyStats = p => { p.GetDamage(DamageClass.Magic) += 0.50f; p.statManaMax2 /= 2; }
            });

            // Outer Fork 5: The Hemomancer
            BuildClassBranch(t5, ref id, 5, new BranchBlueprint {
                R1BaseName = "+10 Max Life", R1BaseIcons = lifeIcons, R1BaseStats = p => p.statLifeMax2 += 10,
                R1LeftName = "+10 Max Life", R1LeftIcons = lifeIcons, R1LeftStats = p => p.statLifeMax2 += 10,
                R1RightName = "+10 Max Life", R1RightIcons = lifeIcons, R1RightStats = p => p.statLifeMax2 += 10,
                MinorKeyName = "Sanguine Flow", MinorKeyDesc = "+10 Flat Life Regeneration.", MinorKeyIcon = ItemID.CharmofMyths, MinorKeyStats = p => p.lifeRegen += 10,
                R2BaseName = "+10 Max Life", R2BaseIcons = lifeIcons, R2BaseStats = p => p.statLifeMax2 += 10,
                R2LeftName = "+10 Max Life", R2LeftIcons = lifeIcons, R2LeftStats = p => p.statLifeMax2 += 10,
                R2RightName = "+10 Max Life", R2RightIcons = lifeIcons, R2RightStats = p => p.statLifeMax2 += 10,
                MajorKeyName = "Sanguine Pact", MajorKeyDesc = "Life Regeneration is disabled, converting entirely into Magic Damage.", MajorKeyIcon = ItemID.BloodWater, MajorKeyStats = p => { if(p.lifeRegen > 0) { p.GetDamage(DamageClass.Magic) += (p.lifeRegen * 0.02f); p.lifeRegen = 0; } }
            });

            // --- DIR 6: CHANNELER (WEST) ---
            int t6 = BuildClassBranch(rootId, ref id, 6, new BranchBlueprint { 
                R1BaseName = "+1% Magic Attack Speed", R1BaseIcons = castSpdIcons, R1BaseStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                R1LeftName = "+10 Max Mana", R1LeftIcons = manaIcons, R1LeftStats = p => p.statManaMax2 += 10,
                R1RightName = "+2% Magic Damage", R1RightIcons = magDmgIcons, R1RightStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                MinorKeyName = "Static Charge", MinorKeyDesc = "+15% Magic Damage while stationary.", MinorKeyIcon = ItemID.LightningBug, MinorKeyStats = p => { if(p.velocity == Vector2.Zero) p.GetDamage(DamageClass.Magic) += 0.15f; },
                R2BaseName = "+1% Magic Attack Speed", R2BaseIcons = castSpdIcons, R2BaseStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                R2LeftName = "+10 Max Mana", R2LeftIcons = manaIcons, R2LeftStats = p => p.statManaMax2 += 10,
                R2RightName = "+2% Magic Damage", R2RightIcons = magDmgIcons, R2RightStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                MajorKeyName = "Focus", MajorKeyDesc = "+40% Magic Damage while stationary.", MajorKeyIcon = ItemID.CrystalBall, MajorKeyStats = p => { if(p.velocity == Vector2.Zero) p.GetDamage(DamageClass.Magic) += 0.40f; }
            });

            // Outer Fork 6: The Conduit (Leyline)
            BuildClassBranch(t6, ref id, 6, new BranchBlueprint {
                R1BaseName = "+10 Max Mana", R1BaseIcons = manaIcons, R1BaseStats = p => p.statManaMax2 += 10,
                R1LeftName = "+1% Magic Attack Speed", R1LeftIcons = castSpdIcons, R1LeftStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                R1RightName = "+1% Magic Attack Speed", R1RightIcons = castSpdIcons, R1RightStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                MinorKeyName = "Deep Pool", MinorKeyDesc = "+50 Max Mana.", MinorKeyIcon = ItemID.ManaCrystal, MinorKeyStats = p => p.statManaMax2 += 50,
                R2BaseName = "+10 Max Mana", R2BaseIcons = manaIcons, R2BaseStats = p => p.statManaMax2 += 10,
                R2LeftName = "+1% Magic Attack Speed", R2LeftIcons = castSpdIcons, R2LeftStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                R2RightName = "+1% Magic Attack Speed", R2RightIcons = castSpdIcons, R2RightStats = p => p.GetAttackSpeed(DamageClass.Magic) += 0.01f,
                MajorKeyName = "Leyline Conduit", MajorKeyDesc = "Massive Mana Regeneration while stationary.", MajorKeyIcon = ItemID.CrystalVileShard, MajorKeyStats = p => { if(p.velocity == Vector2.Zero) p.manaRegen += 50; }
            });

            // --- DIR 7: GEOMANCER (NORTHWEST) ---
            int t7 = BuildClassBranch(rootId, ref id, 7, new BranchBlueprint { 
                R1BaseName = "+2 Defense", R1BaseIcons = defIcons, R1BaseStats = p => p.statDefense += 2,
                R1LeftName = "+2 Armor Penetration", R1LeftIcons = penIcons, R1LeftStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R1RightName = "+2% Magic Damage", R1RightIcons = magDmgIcons, R1RightStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                MinorKeyName = "Stone Skin", MinorKeyDesc = "+10 Defense, -10% Movement Speed.", MinorKeyIcon = ItemID.StoneBlock, MinorKeyStats = p => { p.statDefense += 10; p.moveSpeed -= 0.10f; },
                R2BaseName = "+2 Defense", R2BaseIcons = defIcons, R2BaseStats = p => p.statDefense += 2,
                R2LeftName = "+2 Armor Penetration", R2LeftIcons = penIcons, R2LeftStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R2RightName = "+2% Magic Damage", R2RightIcons = magDmgIcons, R2RightStats = p => p.GetDamage(DamageClass.Magic) += 0.02f,
                MajorKeyName = "Earthbind", MajorKeyDesc = "Defense is added to Armor Penetration.", MajorKeyIcon = ItemID.StaffofEarth, MajorKeyStats = p => { p.GetArmorPenetration(DamageClass.Generic) += (int)p.statDefense; }
            });

            // Outer Fork 7: The Meteor
            BuildClassBranch(t7, ref id, 7, new BranchBlueprint {
                R1BaseName = "+1% Magic Crit Chance", R1BaseIcons = magCritIcons, R1BaseStats = p => p.GetCritChance(DamageClass.Magic) += 1f,
                R1LeftName = "+1% Magic Crit Chance", R1LeftIcons = magCritIcons, R1LeftStats = p => p.GetCritChance(DamageClass.Magic) += 1f,
                R1RightName = "+2 Armor Penetration", R1RightIcons = penIcons, R1RightStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                MinorKeyName = "Crushing Blows", MinorKeyDesc = "+15 Armor Penetration.", MinorKeyIcon = ItemID.SharpeningStation, MinorKeyStats = p => p.GetArmorPenetration(DamageClass.Generic) += 15f,
                R2BaseName = "+1% Magic Crit Chance", R2BaseIcons = magCritIcons, R2BaseStats = p => p.GetCritChance(DamageClass.Magic) += 1f,
                R2LeftName = "+1% Magic Crit Chance", R2LeftIcons = magCritIcons, R2LeftStats = p => p.GetCritChance(DamageClass.Magic) += 1f,
                R2RightName = "+2 Armor Penetration", R2RightIcons = penIcons, R2RightStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                MajorKeyName = "Meteor Strike", MajorKeyDesc = "Converts all Defense into Magic Critical Strike Chance.", MajorKeyIcon = ItemID.MeteorStaff, MajorKeyStats = p => { if(p.statDefense > 0) { p.GetCritChance(DamageClass.Magic) += (int)p.statDefense; p.statDefense -= (int)p.statDefense; } }
            });
        }
    }
}