using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using static WarpedAtlas.Content.Systems.PassiveTreeRegistry; 

namespace WarpedAtlas.Content.Systems
{
    public static class RangerPassiveTree
    {
        public static void Build() {
            int rootId = 2000;
            AddNode(rootId, "Ranger's Origin", "+5% Ranged Damage, +2% Evasion", new Point(0, 0), ItemID.WoodenBow, NodeType.Origin, p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.02f, p => p.GetDamage(DamageClass.Ranged) += 0.05f);
            
            int id = 2001;
            
            // Icon Pools
            int[] rngDmgIcons = { ItemID.MusketBall, ItemID.MeteorShot, ItemID.SilverBullet, ItemID.RangerEmblem };
            int[] rngCritIcons = { ItemID.RifleScope, ItemID.SniperScope, ItemID.ReconScope };
            int[] atkSpeedIcons = { ItemID.Minishark, ItemID.Musket };
            int[] moveIcons = { ItemID.HermesBoots, ItemID.LightningBoots, ItemID.FrostsparkBoots };
            int[] lifeIcons = { ItemID.LifeCrystal, ItemID.LifeFruit };
            int[] bowDmgIcons = { ItemID.WoodenArrow, ItemID.FlamingArrow, ItemID.JestersArrow, ItemID.HellfireArrow };
            int[] evadeIcons = { ItemID.Tabi, ItemID.NinjaShirt, ItemID.NinjaPants };
            int[] penIcons = { ItemID.SharkToothNecklace, ItemID.StingerNecklace };

            // --- DIR 0: SNIPER (NORTH) ---
            int t0 = BuildClassBranch(rootId, ref id, 0, new BranchBlueprint { 
                R1BaseName = "+2% Ranged Damage", R1BaseIcons = rngDmgIcons, R1BaseStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R1LeftName = "+1% Ranged Crit Chance", R1LeftIcons = rngCritIcons, R1LeftStats = p => p.GetCritChance(DamageClass.Ranged) += 1f,
                R1RightName = "+2 Armor Penetration", R1RightIcons = penIcons, R1RightStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                MinorKeyName = "Deadeye", MinorKeyDesc = "+15% Ranged Crit Chance. Bows deal 0 damage.", MinorKeyIcon = ItemID.MagicQuiver, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Deadeye = true,
                R2BaseName = "+2% Ranged Damage", R2BaseIcons = rngDmgIcons, R2BaseStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R2LeftName = "+1% Ranged Crit Chance", R2LeftIcons = rngCritIcons, R2LeftStats = p => p.GetCritChance(DamageClass.Ranged) += 1f,
                R2RightName = "+2 Armor Penetration", R2RightIcons = penIcons, R2RightStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                MajorKeyName = "Sniper's Veil", MajorKeyDesc = "+25% Ranged Crit Chance, -20% Max Life.", MajorKeyIcon = ItemID.SniperRifle, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_SnipersVeil = true
            });

            // Outer Fork 0: The Anchor
            BuildClassBranch(t0, ref id, 0, new BranchBlueprint {
                R1BaseName = "+2% Ranged Damage", R1BaseIcons = rngDmgIcons, R1BaseStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R1LeftName = "+2% Ranged Damage", R1LeftIcons = rngDmgIcons, R1LeftStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R1RightName = "+2% Ranged Damage", R1RightIcons = rngDmgIcons, R1RightStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                MinorKeyName = "Hold the Line", MinorKeyDesc = "+15% Ranged Damage.", MinorKeyIcon = ItemID.RangerEmblem, MinorKeyStats = p => p.GetDamage(DamageClass.Ranged) += 0.15f,
                R2BaseName = "+2 Armor Penetration", R2BaseIcons = penIcons, R2BaseStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R2LeftName = "+2 Armor Penetration", R2LeftIcons = penIcons, R2LeftStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R2RightName = "+2 Armor Penetration", R2RightIcons = penIcons, R2RightStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                MajorKeyName = "Entrenched", MajorKeyDesc = "If stationary, gain +50% Ranged Damage and +20 Armor Penetration.", MajorKeyIcon = ItemID.CobaltShield, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Entrenched = true
            });

            // --- DIR 1: GUNNER (NORTHEAST) ---
            int t1 = BuildClassBranch(rootId, ref id, 1, new BranchBlueprint { 
                R1BaseName = "+1% Ranged Attack Speed", R1BaseIcons = atkSpeedIcons, R1BaseStats = p => p.GetAttackSpeed(DamageClass.Ranged) += 0.01f,
                R1LeftName = "+2% Movement Speed", R1LeftIcons = moveIcons, R1LeftStats = p => p.moveSpeed += 0.02f,
                R1RightName = "+2% Ranged Damage", R1RightIcons = rngDmgIcons, R1RightStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                MinorKeyName = "Trigger Happy", MinorKeyDesc = "+40% Ranged Attack Speed, -20% Ranged Damage.", MinorKeyIcon = ItemID.Boomstick, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_TriggerHappy = true,
                R2BaseName = "+1% Ranged Attack Speed", R2BaseIcons = atkSpeedIcons, R2BaseStats = p => p.GetAttackSpeed(DamageClass.Ranged) += 0.01f,
                R2LeftName = "+2% Movement Speed", R2LeftIcons = moveIcons, R2LeftStats = p => p.moveSpeed += 0.02f,
                R2RightName = "+2% Ranged Damage", R2RightIcons = rngDmgIcons, R2RightStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                MajorKeyName = "Acrobat", MajorKeyDesc = "+15% Flat Evasion, +25% Movement Speed, -10 Defense.", MajorKeyIcon = ItemID.Tabi, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Acrobat = true
            });

            // Outer Fork 1: The Shredder
            BuildClassBranch(t1, ref id, 1, new BranchBlueprint {
                R1BaseName = "+2 Armor Penetration", R1BaseIcons = penIcons, R1BaseStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R1LeftName = "+2 Armor Penetration", R1LeftIcons = penIcons, R1LeftStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R1RightName = "+2 Armor Penetration", R1RightIcons = penIcons, R1RightStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                MinorKeyName = "Shrapnel", MinorKeyDesc = "+10 Armor Penetration.", MinorKeyIcon = ItemID.StingerNecklace, MinorKeyStats = p => p.GetArmorPenetration(DamageClass.Generic) += 10f,
                R2BaseName = "+1% Ranged Attack Speed", R2BaseIcons = atkSpeedIcons, R2BaseStats = p => p.GetAttackSpeed(DamageClass.Ranged) += 0.01f,
                R2LeftName = "+1% Ranged Attack Speed", R2LeftIcons = atkSpeedIcons, R2LeftStats = p => p.GetAttackSpeed(DamageClass.Ranged) += 0.01f,
                R2RightName = "+1% Ranged Attack Speed", R2RightIcons = atkSpeedIcons, R2RightStats = p => p.GetAttackSpeed(DamageClass.Ranged) += 0.01f,
                MajorKeyName = "Armorbane", MajorKeyDesc = "Ranged Damage increases by 1% for every 2 points of Armor Penetration.", MajorKeyIcon = ItemID.Megashark, MajorKeyStats = p => { float pen = p.GetArmorPenetration(DamageClass.Generic); if (pen > 0) p.GetDamage(DamageClass.Ranged) += (pen / 2f) * 0.01f; }
            });

            // --- DIR 2: DEMOLITIONIST (EAST) ---
            int t2 = BuildClassBranch(rootId, ref id, 2, new BranchBlueprint { 
                R1BaseName = "+2% Ranged Damage", R1BaseIcons = new int[] { ItemID.Grenade, ItemID.StickyGrenade }, R1BaseStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R1LeftName = "+10 Max Life", R1LeftIcons = lifeIcons, R1LeftStats = p => p.statLifeMax2 += 10,
                R1RightName = "+1% Ranged Attack Speed", R1RightIcons = atkSpeedIcons, R1RightStats = p => p.GetAttackSpeed(DamageClass.Ranged) += 0.01f,
                MinorKeyName = "Demolitionist", MinorKeyDesc = "+40% Ranged Damage, -15% Damage Reduction.", MinorKeyIcon = ItemID.Bomb, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Demolitionist = true,
                R2BaseName = "+2% Ranged Damage", R2BaseIcons = new int[] { ItemID.Grenade, ItemID.StickyGrenade }, R2BaseStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R2LeftName = "+10 Max Life", R2LeftIcons = lifeIcons, R2LeftStats = p => p.statLifeMax2 += 10,
                R2RightName = "+1% Ranged Attack Speed", R2RightIcons = atkSpeedIcons, R2RightStats = p => p.GetAttackSpeed(DamageClass.Ranged) += 0.01f,
                MajorKeyName = "Siege Engineer", MajorKeyDesc = "Grants Immunity to Knockback.", MajorKeyIcon = ItemID.RocketLauncher, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_SiegeEngineer = true
            });

            // Outer Fork 2: The Bombardier
            BuildClassBranch(t2, ref id, 2, new BranchBlueprint {
                R1BaseName = "+2% Ranged Damage", R1BaseIcons = rngDmgIcons, R1BaseStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R1LeftName = "+2% Ranged Damage", R1LeftIcons = rngDmgIcons, R1LeftStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R1RightName = "+2% Ranged Damage", R1RightIcons = rngDmgIcons, R1RightStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                MinorKeyName = "Heavy Payload", MinorKeyDesc = "+15% Ranged Damage.", MinorKeyIcon = ItemID.ExplosivePowder, MinorKeyStats = p => p.GetDamage(DamageClass.Ranged) += 0.15f,
                R2BaseName = "+2% Ranged Damage", R2BaseIcons = rngDmgIcons, R2BaseStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R2LeftName = "+2% Ranged Damage", R2LeftIcons = rngDmgIcons, R2LeftStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R2RightName = "+2% Ranged Damage", R2RightIcons = rngDmgIcons, R2RightStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                MajorKeyName = "Artillery", MajorKeyDesc = "+75% Ranged Damage, -30% Ranged Attack Speed.", MajorKeyIcon = ItemID.SnowmanCannon, MajorKeyStats = p => { p.GetDamage(DamageClass.Ranged) += 0.75f; p.GetAttackSpeed(DamageClass.Ranged) -= 0.30f; }
            });

            // --- DIR 3: BOWMAN (SOUTHEAST) ---
            int t3 = BuildClassBranch(rootId, ref id, 3, new BranchBlueprint { 
                R1BaseName = "+2% Bow Damage", R1BaseIcons = bowDmgIcons, R1BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusBowDamage += 0.02f,
                R1LeftName = "+1% Ranged Crit Chance", R1LeftIcons = new int[] { ItemID.ArcheryPotion, ItemID.MagicQuiver }, R1LeftStats = p => p.GetCritChance(DamageClass.Ranged) += 1f,
                R1RightName = "+2% Movement Speed", R1RightIcons = moveIcons, R1RightStats = p => p.moveSpeed += 0.02f,
                MinorKeyName = "Fletcher's Pride", MinorKeyDesc = "+30% Bow Damage. Guns deal 0 damage.", MinorKeyIcon = ItemID.Tsunami, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_FletchersPride = true,
                R2BaseName = "+2% Bow Damage", R2BaseIcons = bowDmgIcons, R2BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusBowDamage += 0.02f,
                R2LeftName = "+1% Ranged Crit Chance", R2LeftIcons = new int[] { ItemID.ArcheryPotion, ItemID.MagicQuiver }, R2LeftStats = p => p.GetCritChance(DamageClass.Ranged) += 1f,
                R2RightName = "+2% Movement Speed", R2RightIcons = moveIcons, R2RightStats = p => p.moveSpeed += 0.02f,
                MajorKeyName = "Endless Quiver", MajorKeyDesc = "-15% Movement Speed.", MajorKeyIcon = ItemID.EndlessQuiver, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_EndlessQuiver = true
            });

            // Outer Fork 3: The Windrunner
            BuildClassBranch(t3, ref id, 3, new BranchBlueprint {
                R1BaseName = "+2% Movement Speed", R1BaseIcons = moveIcons, R1BaseStats = p => p.moveSpeed += 0.02f,
                R1LeftName = "+2% Movement Speed", R1LeftIcons = moveIcons, R1LeftStats = p => p.moveSpeed += 0.02f,
                R1RightName = "+2% Movement Speed", R1RightIcons = moveIcons, R1RightStats = p => p.moveSpeed += 0.02f,
                MinorKeyName = "Swiftness", MinorKeyDesc = "+10% Movement Speed.", MinorKeyIcon = ItemID.SwiftnessPotion, MinorKeyStats = p => p.moveSpeed += 0.10f,
                R2BaseName = "+2% Movement Speed", R2BaseIcons = moveIcons, R2BaseStats = p => p.moveSpeed += 0.02f,
                R2LeftName = "+2% Movement Speed", R2LeftIcons = moveIcons, R2LeftStats = p => p.moveSpeed += 0.02f,
                R2RightName = "+2% Movement Speed", R2RightIcons = moveIcons, R2RightStats = p => p.moveSpeed += 0.02f,
                MajorKeyName = "Tailwind", MajorKeyDesc = "Converts all Movement Speed bonuses directly into Ranged Attack Speed.", MajorKeyIcon = ItemID.FairyBoots, MajorKeyStats = p => { float bonus = p.moveSpeed - 1f; if(bonus > 0) { p.GetAttackSpeed(DamageClass.Ranged) += bonus; p.moveSpeed = 1f; } }
            });

            // --- DIR 4: EVASION (SOUTH) ---
            int t4 = BuildClassBranch(rootId, ref id, 4, new BranchBlueprint { 
                R1BaseName = "+1% Flat Evasion", R1BaseIcons = evadeIcons, R1BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.01f,
                R1LeftName = "+2% Movement Speed", R1LeftIcons = moveIcons, R1LeftStats = p => p.moveSpeed += 0.02f,
                R1RightName = "+2 Defense", R1RightIcons = new int[] { ItemID.IronskinPotion, ItemID.CobaltShield }, R1RightStats = p => p.statDefense += 2,
                MinorKeyName = "Wind Walker", MinorKeyDesc = "+10% Movement Speed.", MinorKeyIcon = ItemID.HermesBoots, MinorKeyStats = p => p.moveSpeed += 0.10f,
                R2BaseName = "+1% Flat Evasion", R2BaseIcons = evadeIcons, R2BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.01f,
                R2LeftName = "+2% Movement Speed", R2LeftIcons = moveIcons, R2LeftStats = p => p.moveSpeed += 0.02f,
                R2RightName = "+2 Defense", R2RightIcons = new int[] { ItemID.IronskinPotion, ItemID.CobaltShield }, R2RightStats = p => p.statDefense += 2,
                MajorKeyName = "Phantom", MajorKeyDesc = "+15% Flat Evasion, -10 Defense.", MajorKeyIcon = ItemID.MasterNinjaGear, MajorKeyStats = p => { p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.15f; p.statDefense -= 10; }
            });

            // Outer Fork 4: The Ghost
            BuildClassBranch(t4, ref id, 4, new BranchBlueprint {
                R1BaseName = "+1% Flat Evasion", R1BaseIcons = evadeIcons, R1BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.01f,
                R1LeftName = "+1% Flat Evasion", R1LeftIcons = evadeIcons, R1LeftStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.01f,
                R1RightName = "+1% Flat Evasion", R1RightIcons = evadeIcons, R1RightStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.01f,
                MinorKeyName = "Elusive Stance", MinorKeyDesc = "+5% Flat Evasion.", MinorKeyIcon = ItemID.NinjaHood, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.05f,
                R2BaseName = "+1% Flat Evasion", R2BaseIcons = evadeIcons, R2BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.01f,
                R2LeftName = "+1% Flat Evasion", R2LeftIcons = evadeIcons, R2LeftStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.01f,
                R2RightName = "+1% Flat Evasion", R2RightIcons = evadeIcons, R2RightStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.01f,
                MajorKeyName = "Lethal Mirage", MajorKeyDesc = "Trades all Evasion Chance for an equal flat Ranged Damage percentage.", MajorKeyIcon = ItemID.PsychoKnife, MajorKeyStats = p => { var prog = p.GetModPlayer<AtlasProgressionPlayer>(); if (prog.EvasionChance > 0) { p.GetDamage(DamageClass.Ranged) += prog.EvasionChance; prog.EvasionChance = 0f; } }
            });

            // --- DIR 5: SKIRMISHER (SOUTHWEST) ---
            int t5 = BuildClassBranch(rootId, ref id, 5, new BranchBlueprint { 
                R1BaseName = "+2% Movement Speed", R1BaseIcons = moveIcons, R1BaseStats = p => p.moveSpeed += 0.02f,
                R1LeftName = "+2% Ranged Damage", R1LeftIcons = rngDmgIcons, R1LeftStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R1RightName = "+1% Ranged Crit Chance", R1RightIcons = rngCritIcons, R1RightStats = p => p.GetCritChance(DamageClass.Ranged) += 1f,
                MinorKeyName = "Run and Gun", MinorKeyDesc = "+10% Movement Speed.", MinorKeyIcon = ItemID.Aglet, MinorKeyStats = p => p.moveSpeed += 0.10f,
                R2BaseName = "+2% Movement Speed", R2BaseIcons = moveIcons, R2BaseStats = p => p.moveSpeed += 0.02f,
                R2LeftName = "+2% Ranged Damage", R2LeftIcons = rngDmgIcons, R2LeftStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R2RightName = "+1% Ranged Crit Chance", R2RightIcons = rngCritIcons, R2RightStats = p => p.GetCritChance(DamageClass.Ranged) += 1f,
                MajorKeyName = "Momentum", MajorKeyDesc = "+20% Ranged Damage while moving.", MajorKeyIcon = ItemID.LightningBoots, MajorKeyStats = p => { if (p.velocity != Vector2.Zero) p.GetDamage(DamageClass.Ranged) += 0.20f; }
            });

            // Outer Fork 5: Kinetic Sniper
            BuildClassBranch(t5, ref id, 5, new BranchBlueprint {
                R1BaseName = "+2% Movement Speed", R1BaseIcons = moveIcons, R1BaseStats = p => p.moveSpeed += 0.02f,
                R1LeftName = "+2% Movement Speed", R1LeftIcons = moveIcons, R1LeftStats = p => p.moveSpeed += 0.02f,
                R1RightName = "+2% Movement Speed", R1RightIcons = moveIcons, R1RightStats = p => p.moveSpeed += 0.02f,
                MinorKeyName = "Velocity", MinorKeyDesc = "+10% Movement Speed.", MinorKeyIcon = ItemID.FrostsparkBoots, MinorKeyStats = p => p.moveSpeed += 0.10f,
                R2BaseName = "+2% Movement Speed", R2BaseIcons = moveIcons, R2BaseStats = p => p.moveSpeed += 0.02f,
                R2LeftName = "+2% Movement Speed", R2LeftIcons = moveIcons, R2LeftStats = p => p.moveSpeed += 0.02f,
                R2RightName = "+2% Movement Speed", R2RightIcons = moveIcons, R2RightStats = p => p.moveSpeed += 0.02f,
                MajorKeyName = "Railgun", MajorKeyDesc = "X-Velocity is converted directly into Ranged Damage.", MajorKeyIcon = ItemID.SniperRifle, MajorKeyStats = p => p.GetDamage(DamageClass.Ranged) += (System.Math.Abs(p.velocity.X) * 0.05f)
            });

            // --- DIR 6: PENETRATOR (WEST) ---
            int t6 = BuildClassBranch(rootId, ref id, 6, new BranchBlueprint { 
                R1BaseName = "+2 Armor Penetration", R1BaseIcons = penIcons, R1BaseStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R1LeftName = "+2% Ranged Damage", R1LeftIcons = rngDmgIcons, R1LeftStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R1RightName = "+1% Ranged Crit Chance", R1RightIcons = rngCritIcons, R1RightStats = p => p.GetCritChance(DamageClass.Ranged) += 1f,
                MinorKeyName = "Hollow Point", MinorKeyDesc = "+10 Armor Penetration.", MinorKeyIcon = ItemID.SharkToothNecklace, MinorKeyStats = p => p.GetArmorPenetration(DamageClass.Generic) += 10f,
                R2BaseName = "+2 Armor Penetration", R2BaseIcons = penIcons, R2BaseStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R2LeftName = "+2% Ranged Damage", R2LeftIcons = rngDmgIcons, R2LeftStats = p => p.GetDamage(DamageClass.Ranged) += 0.02f,
                R2RightName = "+1% Ranged Crit Chance", R2RightIcons = rngCritIcons, R2RightStats = p => p.GetCritChance(DamageClass.Ranged) += 1f,
                MajorKeyName = "Piercing Strikes", MajorKeyDesc = "+20 Armor Penetration, -5% Ranged Crit Chance.", MajorKeyIcon = ItemID.NailGun, MajorKeyStats = p => { p.GetArmorPenetration(DamageClass.Generic) += 20f; p.GetCritChance(DamageClass.Ranged) -= 5f; }
            });

            // Outer Fork 6: The Executioner
            BuildClassBranch(t6, ref id, 6, new BranchBlueprint {
                R1BaseName = "+2 Armor Penetration", R1BaseIcons = penIcons, R1BaseStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R1LeftName = "+2 Armor Penetration", R1LeftIcons = penIcons, R1LeftStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R1RightName = "+2 Armor Penetration", R1RightIcons = penIcons, R1RightStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                MinorKeyName = "Shatter", MinorKeyDesc = "+5 Armor Penetration.", MinorKeyIcon = ItemID.StingerNecklace, MinorKeyStats = p => p.GetArmorPenetration(DamageClass.Generic) += 5f,
                R2BaseName = "+2 Armor Penetration", R2BaseIcons = penIcons, R2BaseStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R2LeftName = "+2 Armor Penetration", R2LeftIcons = penIcons, R2LeftStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R2RightName = "+2 Armor Penetration", R2RightIcons = penIcons, R2RightStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                MajorKeyName = "Lethality", MajorKeyDesc = "Converts all Armor Penetration directly into Ranged Critical Strike Chance.", MajorKeyIcon = ItemID.VenusMagnum, MajorKeyStats = p => { float pen = p.GetArmorPenetration(DamageClass.Generic); if (pen > 0) { p.GetCritChance(DamageClass.Ranged) += pen; p.GetArmorPenetration(DamageClass.Generic) -= pen; } }
            });

            // --- DIR 7: SURVIVALIST (NORTHWEST) ---
            int t7 = BuildClassBranch(rootId, ref id, 7, new BranchBlueprint { 
                R1BaseName = "+10 Max Life", R1BaseIcons = lifeIcons, R1BaseStats = p => p.statLifeMax2 += 10,
                R1LeftName = "+1% Ranged Crit Chance", R1LeftIcons = rngCritIcons, R1LeftStats = p => p.GetCritChance(DamageClass.Ranged) += 1f,
                R1RightName = "+2% Movement Speed", R1RightIcons = moveIcons, R1RightStats = p => p.moveSpeed += 0.02f,
                MinorKeyName = "Camouflage", MinorKeyDesc = "+15% Ranged Crit Chance while stationary.", MinorKeyIcon = ItemID.ShroomiteHeadgear, MinorKeyStats = p => { if (p.velocity == Vector2.Zero) p.GetCritChance(DamageClass.Ranged) += 15f; },
                R2BaseName = "+10 Max Life", R2BaseIcons = lifeIcons, R2BaseStats = p => p.statLifeMax2 += 10,
                R2LeftName = "+1% Ranged Crit Chance", R2LeftIcons = rngCritIcons, R2LeftStats = p => p.GetCritChance(DamageClass.Ranged) += 1f,
                R2RightName = "+2% Movement Speed", R2RightIcons = moveIcons, R2RightStats = p => p.moveSpeed += 0.02f,
                MajorKeyName = "Scavenger", MajorKeyDesc = "+50 Max Life.", MajorKeyIcon = ItemID.LifeFruit, MajorKeyStats = p => p.statLifeMax2 += 50
            });

            // Outer Fork 7: Apex Hunter
            BuildClassBranch(t7, ref id, 7, new BranchBlueprint {
                R1BaseName = "+10 Max Life", R1BaseIcons = lifeIcons, R1BaseStats = p => p.statLifeMax2 += 10,
                R1LeftName = "+10 Max Life", R1LeftIcons = lifeIcons, R1LeftStats = p => p.statLifeMax2 += 10,
                R1RightName = "+10 Max Life", R1RightIcons = lifeIcons, R1RightStats = p => p.statLifeMax2 += 10,
                MinorKeyName = "Vitality", MinorKeyDesc = "+20 Max Life.", MinorKeyIcon = ItemID.LifeCrystal, MinorKeyStats = p => p.statLifeMax2 += 20,
                R2BaseName = "+10 Max Life", R2BaseIcons = lifeIcons, R2BaseStats = p => p.statLifeMax2 += 10,
                R2LeftName = "+10 Max Life", R2LeftIcons = lifeIcons, R2LeftStats = p => p.statLifeMax2 += 10,
                R2RightName = "+10 Max Life", R2RightIcons = lifeIcons, R2RightStats = p => p.statLifeMax2 += 10,
                MajorKeyName = "Bloodlust", MajorKeyDesc = "Grants +50% Ranged Damage while at maximum health.", MajorKeyIcon = ItemID.VampireKnives, MajorKeyStats = p => { if (p.statLife >= p.statLifeMax2) p.GetDamage(DamageClass.Ranged) += 0.50f; }
            });
        }
    }
}