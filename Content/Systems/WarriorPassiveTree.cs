using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using static WarpedAtlas.Content.Systems.PassiveTreeRegistry; 

namespace WarpedAtlas.Content.Systems
{
    public static class WarriorPassiveTree
    {
        public static void Build() {
            int rootId = 1000;
            AddNode(rootId, "Warrior's Origin", "+5% Melee Damage, +4 Defense", new Point(0, 0), ItemID.CopperBroadsword, NodeType.Origin, p => p.statDefense += 4, p => p.GetDamage(DamageClass.Melee) += 0.05f);
            
            int id = 1001;
            int[] lifeIcons = { ItemID.LifeCrystal, ItemID.LifeFruit, ItemID.Heart };
            int[] dmgRedIcons = { ItemID.WormScarf, ItemID.EndurancePotion };
            int[] defIcons = { ItemID.IronskinPotion, ItemID.CobaltShield, ItemID.ObsidianShield };
            int[] meleeDmgIcons = { ItemID.WarriorEmblem, ItemID.DestroyerEmblem, ItemID.CopperBroadsword, ItemID.IronBroadsword };
            int[] atkSpeedIcons = { ItemID.FeralClaws, ItemID.PowerGlove, ItemID.MechanicalGlove };
            int[] meleeCritIcons = { ItemID.Ale, ItemID.TitanGlove, ItemID.ThrowingKnife };
            int[] moveIcons = { ItemID.HermesBoots, ItemID.FlurryBoots, ItemID.SailfishBoots, ItemID.SpectreBoots };

            // --- DIR 0: JUGGERNAUT (NORTH) ---
            int t0 = BuildClassBranch(rootId, ref id, 0, new BranchBlueprint { 
                R1BaseName = "+10 Max Life", R1BaseIcons = lifeIcons, R1BaseStats = p => p.statLifeMax2 += 10,
                R1LeftName = "+1% Damage Reduction", R1LeftIcons = dmgRedIcons, R1LeftStats = p => p.endurance += 0.01f,
                R1RightName = "+2 Defense", R1RightIcons = defIcons, R1RightStats = p => p.statDefense += 2,
                MinorKeyName = "Aegis Wall", MinorKeyDesc = "+20 Defense, -30% Movement Speed.", MinorKeyIcon = ItemID.PaladinsShield, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_AegisWall = true,
                R2BaseName = "+10 Max Life", R2BaseIcons = lifeIcons, R2BaseStats = p => p.statLifeMax2 += 10,
                R2LeftName = "+1% Damage Reduction", R2LeftIcons = dmgRedIcons, R2LeftStats = p => p.endurance += 0.01f,
                R2RightName = "+2 Defense", R2RightIcons = defIcons, R2RightStats = p => p.statDefense += 2,
                MajorKeyName = "Juggernaut", MajorKeyDesc = "+100 Max Life, -25% Movement Speed.", MajorKeyIcon = ItemID.FleshKnuckles, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Juggernaut = true
            });

            // Outer Fork 0: The Ironclad
            BuildClassBranch(t0, ref id, 0, new BranchBlueprint {
                R1BaseName = "+2 Defense", R1BaseIcons = defIcons, R1BaseStats = p => p.statDefense += 2,
                R1LeftName = "+2 Defense", R1LeftIcons = defIcons, R1LeftStats = p => p.statDefense += 2,
                R1RightName = "+2 Defense", R1RightIcons = defIcons, R1RightStats = p => p.statDefense += 2,
                MinorKeyName = "Bulwark Plating", MinorKeyDesc = "+15 Defense.", MinorKeyIcon = ItemID.PaladinsShield, MinorKeyStats = p => p.statDefense += 15,
                R2BaseName = "+1% Damage Reduction", R2BaseIcons = dmgRedIcons, R2BaseStats = p => p.endurance += 0.01f,
                R2LeftName = "+1% Damage Reduction", R2LeftIcons = dmgRedIcons, R2LeftStats = p => p.endurance += 0.01f,
                R2RightName = "+1% Damage Reduction", R2RightIcons = dmgRedIcons, R2RightStats = p => p.endurance += 0.01f,
                MajorKeyName = "Retaliation Matrix", MajorKeyDesc = "Melee damage increases by 1% of your total Defense.", MajorKeyIcon = ItemID.TurtleScaleMail, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_RetaliationMatrix = true
            });

            // --- DIR 1: BERSERKER (NORTHEAST) ---
            int t1 = BuildClassBranch(rootId, ref id, 1, new BranchBlueprint { 
                R1BaseName = "+2% Melee Damage", R1BaseIcons = meleeDmgIcons, R1BaseStats = p => p.GetDamage(DamageClass.Melee) += 0.02f,
                R1LeftName = "+1% Melee Attack Speed", R1LeftIcons = atkSpeedIcons, R1LeftStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                R1RightName = "+1% Melee Crit Chance", R1RightIcons = meleeCritIcons, R1RightStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                MinorKeyName = "Blood Pact", MinorKeyDesc = "+50% Max Life, +5 Flat Life Regeneration.", MinorKeyIcon = ItemID.VampireKnives, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_BloodPact = true,
                R2BaseName = "+2% Melee Damage", R2BaseIcons = meleeDmgIcons, R2BaseStats = p => p.GetDamage(DamageClass.Melee) += 0.02f,
                R2LeftName = "+1% Melee Attack Speed", R2LeftIcons = atkSpeedIcons, R2LeftStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                R2RightName = "+1% Melee Crit Chance", R2RightIcons = meleeCritIcons, R2RightStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                MajorKeyName = "Berserker", MajorKeyDesc = "+40% Melee Damage, -20% Damage Reduction.", MajorKeyIcon = ItemID.EyeoftheGolem, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Berserker = true
            });

            // Outer Fork 1: The Executioner
            BuildClassBranch(t1, ref id, 1, new BranchBlueprint {
                R1BaseName = "+2% Melee Damage", R1BaseIcons = meleeDmgIcons, R1BaseStats = p => p.GetDamage(DamageClass.Melee) += 0.02f,
                R1LeftName = "+2% Melee Damage", R1LeftIcons = meleeDmgIcons, R1LeftStats = p => p.GetDamage(DamageClass.Melee) += 0.02f,
                R1RightName = "+2% Melee Damage", R1RightIcons = meleeDmgIcons, R1RightStats = p => p.GetDamage(DamageClass.Melee) += 0.02f,
                MinorKeyName = "Reckless Abandon", MinorKeyDesc = "+20% Melee Damage, -10 Defense.", MinorKeyIcon = ItemID.WarriorEmblem, MinorKeyStats = p => { p.GetDamage(DamageClass.Melee) += 0.20f; p.statDefense -= 10; },
                R2BaseName = "+1% Melee Crit Chance", R2BaseIcons = meleeCritIcons, R2BaseStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                R2LeftName = "+1% Melee Crit Chance", R2LeftIcons = meleeCritIcons, R2LeftStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                R2RightName = "+1% Melee Crit Chance", R2RightIcons = meleeCritIcons, R2RightStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                MajorKeyName = "Executioner", MajorKeyDesc = "Gain up to +100% Melee Damage based on your missing Life.", MajorKeyIcon = ItemID.PsychoKnife, MajorKeyStats = p => { float missing = 1f - ((float)p.statLife / p.statLifeMax2); p.GetDamage(DamageClass.Melee) += missing; }
            });

            // --- DIR 2: GLADIATOR (EAST) ---
            int t2 = BuildClassBranch(rootId, ref id, 2, new BranchBlueprint { 
                R1BaseName = "+1% Melee Crit Chance", R1BaseIcons = meleeCritIcons, R1BaseStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                R1LeftName = "+2% Movement Speed", R1LeftIcons = moveIcons, R1LeftStats = p => p.moveSpeed += 0.02f,
                R1RightName = "+1% Melee Attack Speed", R1RightIcons = atkSpeedIcons, R1RightStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                MinorKeyName = "Gladiator", MinorKeyDesc = "+20% Melee Crit Chance, -10% Damage Reduction.", MinorKeyIcon = ItemID.Gladius, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Gladiator = true,
                R2BaseName = "+1% Melee Crit Chance", R2BaseIcons = meleeCritIcons, R2BaseStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                R2LeftName = "+2% Movement Speed", R2LeftIcons = moveIcons, R2LeftStats = p => p.moveSpeed += 0.02f,
                R2RightName = "+1% Melee Attack Speed", R2RightIcons = atkSpeedIcons, R2RightStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                MajorKeyName = "Flurry", MajorKeyDesc = "+30% Melee Attack Speed, -15% Melee Damage.", MajorKeyIcon = ItemID.Muramasa, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Flurry = true
            });

            // Outer Fork 2: The Blade Dancer
            BuildClassBranch(t2, ref id, 2, new BranchBlueprint {
                R1BaseName = "+1% Flat Evasion", R1BaseIcons = new int[] { ItemID.Tabi }, R1BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.01f,
                R1LeftName = "+1% Flat Evasion", R1LeftIcons = new int[] { ItemID.Tabi }, R1LeftStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.01f,
                R1RightName = "+1% Flat Evasion", R1RightIcons = new int[] { ItemID.Tabi }, R1RightStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.01f,
                MinorKeyName = "Elusive Stance", MinorKeyDesc = "+5% Flat Evasion.", MinorKeyIcon = ItemID.MasterNinjaGear, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.05f,
                R2BaseName = "+1% Melee Attack Speed", R2BaseIcons = atkSpeedIcons, R2BaseStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                R2LeftName = "+1% Melee Attack Speed", R2LeftIcons = atkSpeedIcons, R2LeftStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                R2RightName = "+1% Melee Attack Speed", R2RightIcons = atkSpeedIcons, R2RightStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                MajorKeyName = "Blade Dancer", MajorKeyDesc = "Converts all Movement Speed bonuses into Evasion Chance.", MajorKeyIcon = ItemID.CobaltNaginata, MajorKeyStats = p => { float bonus = p.moveSpeed - 1f; if (bonus > 0) { p.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += bonus; p.moveSpeed = 1f; } }
            });

            // --- DIR 3: HEAVY BLADE (SOUTHEAST) ---
            int t3 = BuildClassBranch(rootId, ref id, 3, new BranchBlueprint { 
                R1BaseName = "+2 Armor Penetration", R1BaseIcons = new int[] { ItemID.SharkToothNecklace, ItemID.StingerNecklace }, R1BaseStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R1LeftName = "+2% Melee Damage", R1LeftIcons = meleeDmgIcons, R1LeftStats = p => p.GetDamage(DamageClass.Melee) += 0.02f,
                R1RightName = "Increased Weapon Size", R1RightIcons = new int[] { ItemID.BreakerBlade, ItemID.SlapHand }, R1RightStats = p => {},
                MinorKeyName = "Warlord's Reach", MinorKeyDesc = "+30% Melee Damage, -10 Defense.", MinorKeyIcon = ItemID.Keybrand, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_WarlordsReach = true,
                R2BaseName = "+2 Armor Penetration", R2BaseIcons = new int[] { ItemID.SharkToothNecklace, ItemID.StingerNecklace }, R2BaseStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R2LeftName = "+2% Melee Damage", R2LeftIcons = meleeDmgIcons, R2LeftStats = p => p.GetDamage(DamageClass.Melee) += 0.02f,
                R2RightName = "Increased Weapon Size", R2RightIcons = new int[] { ItemID.BreakerBlade, ItemID.SlapHand }, R2RightStats = p => {},
                MajorKeyName = "Titan's Grip", MajorKeyDesc = "Grants Knockback Immunity, -15% Melee Attack Speed.", MajorKeyIcon = ItemID.TitanGlove, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_TitansGrip = true
            });

            // Outer Fork 3: The Colossus
            BuildClassBranch(t3, ref id, 3, new BranchBlueprint {
                R1BaseName = "Increased Weapon Size", R1BaseIcons = new int[] { ItemID.BreakerBlade }, R1BaseStats = p => {},
                R1LeftName = "Increased Weapon Size", R1LeftIcons = new int[] { ItemID.BreakerBlade }, R1LeftStats = p => {},
                R1RightName = "Increased Weapon Size", R1RightIcons = new int[] { ItemID.BreakerBlade }, R1RightStats = p => {},
                MinorKeyName = "Giant's Reach", MinorKeyDesc = "+20% Melee Damage.", MinorKeyIcon = ItemID.WarriorEmblem, MinorKeyStats = p => p.GetDamage(DamageClass.Melee) += 0.20f,
                R2BaseName = "Increased Weapon Size", R2BaseIcons = new int[] { ItemID.BreakerBlade }, R2BaseStats = p => {},
                R2LeftName = "Increased Weapon Size", R2LeftIcons = new int[] { ItemID.BreakerBlade }, R2LeftStats = p => {},
                R2RightName = "Increased Weapon Size", R2RightIcons = new int[] { ItemID.BreakerBlade }, R2RightStats = p => {},
                MajorKeyName = "Colossus", MajorKeyDesc = "+50% Melee Damage, but completely disables Critical Strikes.", MajorKeyIcon = ItemID.PaladinsHammer, MajorKeyStats = p => { p.GetDamage(DamageClass.Melee) += 0.50f; p.GetCritChance(DamageClass.Melee) = -1000f; }
            });

            // --- DIR 4: CENTRIFUGAL (SOUTH) ---
            int t4 = BuildClassBranch(rootId, ref id, 4, new BranchBlueprint { 
                R1BaseName = "+2% Yoyo Damage", R1BaseIcons = new int[] { ItemID.WoodYoyo, ItemID.Rally, ItemID.Chik }, R1BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusYoyoDamage += 0.02f,
                R1LeftName = "+1% Melee Attack Speed", R1LeftIcons = atkSpeedIcons, R1LeftStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                R1RightName = "+1% Melee Crit Chance", R1RightIcons = meleeCritIcons, R1RightStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                MinorKeyName = "Heavy Blade", MinorKeyDesc = "+50% Melee Damage, -20% Movement Speed.", MinorKeyIcon = ItemID.Amarok, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_HeavyBlade = true,
                R2BaseName = "+2% Yoyo Damage", R2BaseIcons = new int[] { ItemID.WoodYoyo, ItemID.Rally, ItemID.Chik }, R2BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusYoyoDamage += 0.02f,
                R2LeftName = "+1% Melee Attack Speed", R2LeftIcons = atkSpeedIcons, R2LeftStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                R2RightName = "+1% Melee Crit Chance", R2RightIcons = meleeCritIcons, R2RightStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                MajorKeyName = "Centrifugal Master", MajorKeyDesc = "+20% Yoyo Damage.", MajorKeyIcon = ItemID.Terrarian, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_CentrifugalForce = true
            });

            // Outer Fork 4: Kinetic Dynamo
            BuildClassBranch(t4, ref id, 4, new BranchBlueprint {
                R1BaseName = "+2% Movement Speed", R1BaseIcons = moveIcons, R1BaseStats = p => p.moveSpeed += 0.02f,
                R1LeftName = "+2% Movement Speed", R1LeftIcons = moveIcons, R1LeftStats = p => p.moveSpeed += 0.02f,
                R1RightName = "+2% Movement Speed", R1RightIcons = moveIcons, R1RightStats = p => p.moveSpeed += 0.02f,
                MinorKeyName = "Agility Boost", MinorKeyDesc = "+10% Movement Speed.", MinorKeyIcon = ItemID.LightningBoots, MinorKeyStats = p => p.moveSpeed += 0.10f,
                R2BaseName = "+2% Movement Speed", R2BaseIcons = moveIcons, R2BaseStats = p => p.moveSpeed += 0.02f,
                R2LeftName = "+2% Movement Speed", R2LeftIcons = moveIcons, R2LeftStats = p => p.moveSpeed += 0.02f,
                R2RightName = "+2% Movement Speed", R2RightIcons = moveIcons, R2RightStats = p => p.moveSpeed += 0.02f,
                MajorKeyName = "Inertia", MajorKeyDesc = "Converts movement speed directly into massive Melee Damage multipliers.", MajorKeyIcon = ItemID.MasterNinjaGear, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Inertia = true
            });

            // --- DIR 5: EARTHSHAKER (SOUTHWEST) ---
            int t5 = BuildClassBranch(rootId, ref id, 5, new BranchBlueprint { 
                R1BaseName = "+10 Max Life", R1BaseIcons = lifeIcons, R1BaseStats = p => p.statLifeMax2 += 10,
                R1LeftName = "+2% Melee Damage", R1LeftIcons = meleeDmgIcons, R1LeftStats = p => p.GetDamage(DamageClass.Melee) += 0.02f,
                R1RightName = "+2 Defense", R1RightIcons = defIcons, R1RightStats = p => p.statDefense += 2,
                MinorKeyName = "Mountain Stance", MinorKeyDesc = "+30% Melee Damage while standing perfectly still.", MinorKeyIcon = ItemID.StoneBlock, MinorKeyStats = p => { if(p.velocity == Vector2.Zero) p.GetDamage(DamageClass.Melee) += 0.30f; },
                R2BaseName = "+10 Max Life", R2BaseIcons = lifeIcons, R2BaseStats = p => p.statLifeMax2 += 10,
                R2LeftName = "+2% Melee Damage", R2LeftIcons = meleeDmgIcons, R2LeftStats = p => p.GetDamage(DamageClass.Melee) += 0.02f,
                R2RightName = "+2 Defense", R2RightIcons = defIcons, R2RightStats = p => p.statDefense += 2,
                MajorKeyName = "Earthshaker", MajorKeyDesc = "Removes Knockback. Slower swings.", MajorKeyIcon = ItemID.PaladinsHammer, MajorKeyStats = p => p.noKnockback = true
            });

            // Outer Fork 5: The Vanguard
            BuildClassBranch(t5, ref id, 5, new BranchBlueprint {
                R1BaseName = "+2 Defense", R1BaseIcons = defIcons, R1BaseStats = p => p.statDefense += 2,
                R1LeftName = "+10 Max Life", R1LeftIcons = lifeIcons, R1LeftStats = p => p.statLifeMax2 += 10,
                R1RightName = "+1% Damage Reduction", R1RightIcons = dmgRedIcons, R1RightStats = p => p.endurance += 0.01f,
                MinorKeyName = "Phalanx", MinorKeyDesc = "+15 Defense while moving.", MinorKeyIcon = ItemID.CobaltShield, MinorKeyStats = p => { if(p.velocity != Vector2.Zero) p.statDefense += 15; },
                R2BaseName = "+2 Defense", R2BaseIcons = defIcons, R2BaseStats = p => p.statDefense += 2,
                R2LeftName = "+10 Max Life", R2LeftIcons = lifeIcons, R2LeftStats = p => p.statLifeMax2 += 10,
                R2RightName = "+1% Damage Reduction", R2RightIcons = dmgRedIcons, R2RightStats = p => p.endurance += 0.01f,
                MajorKeyName = "Vanguard", MajorKeyDesc = "Converts all Endurance (Damage Reduction) into Armor Penetration.", MajorKeyIcon = ItemID.SquireShield, MajorKeyStats = p => { p.GetArmorPenetration(DamageClass.Generic) += (p.endurance * 100f); p.endurance = 0f; }
            });

            // --- DIR 6: ZEALOT (WEST) ---
            int t6 = BuildClassBranch(rootId, ref id, 6, new BranchBlueprint { 
                R1BaseName = "+1% Melee Attack Speed", R1BaseIcons = atkSpeedIcons, R1BaseStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                R1LeftName = "+1% Melee Attack Speed", R1LeftIcons = atkSpeedIcons, R1LeftStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                R1RightName = "+1% Melee Attack Speed", R1RightIcons = atkSpeedIcons, R1RightStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                MinorKeyName = "Adrenaline", MinorKeyDesc = "+15% Melee Attack Speed.", MinorKeyIcon = ItemID.PanicNecklace, MinorKeyStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.15f,
                R2BaseName = "+1% Melee Attack Speed", R2BaseIcons = atkSpeedIcons, R2BaseStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                R2LeftName = "+1% Melee Attack Speed", R2LeftIcons = atkSpeedIcons, R2LeftStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                R2RightName = "+1% Melee Attack Speed", R2RightIcons = atkSpeedIcons, R2RightStats = p => p.GetAttackSpeed(DamageClass.Melee) += 0.01f,
                MajorKeyName = "Zealotry", MajorKeyDesc = "Taking damage spikes attack speed.", MajorKeyIcon = ItemID.StarCloak, MajorKeyStats = p => { if(p.immune) p.GetAttackSpeed(DamageClass.Melee) += 0.50f; }
            });

            // Outer Fork 6: The Blood Knight
            BuildClassBranch(t6, ref id, 6, new BranchBlueprint {
                R1BaseName = "+1 Life Regeneration", R1BaseIcons = new int[] { ItemID.BandofRegeneration }, R1BaseStats = p => p.lifeRegen += 1,
                R1LeftName = "+10 Max Life", R1LeftIcons = lifeIcons, R1LeftStats = p => p.statLifeMax2 += 10,
                R1RightName = "+1 Life Regeneration", R1RightIcons = new int[] { ItemID.BandofRegeneration }, R1RightStats = p => p.lifeRegen += 1,
                MinorKeyName = "Sanguine Flow", MinorKeyDesc = "+10 Flat Life Regeneration.", MinorKeyIcon = ItemID.CharmofMyths, MinorKeyStats = p => p.lifeRegen += 10,
                R2BaseName = "+1 Life Regeneration", R2BaseIcons = new int[] { ItemID.BandofRegeneration }, R2BaseStats = p => p.lifeRegen += 1,
                R2LeftName = "+10 Max Life", R2LeftIcons = lifeIcons, R2LeftStats = p => p.statLifeMax2 += 10,
                R2RightName = "+1 Life Regeneration", R2RightIcons = new int[] { ItemID.BandofRegeneration }, R2RightStats = p => p.lifeRegen += 1,
                MajorKeyName = "Blood Knight", MajorKeyDesc = "Life Regeneration is doubled while attacking, but you have 0 Defense.", MajorKeyIcon = ItemID.VampireKnives, MajorKeyStats = p => { if (p.itemAnimation > 0) p.lifeRegen *= 2; p.statDefense -= (int)p.statDefense; }
            });

            // --- DIR 7: DUELIST (NORTHWEST) ---
            int t7 = BuildClassBranch(rootId, ref id, 7, new BranchBlueprint { 
                R1BaseName = "+1% Melee Crit Chance", R1BaseIcons = meleeCritIcons, R1BaseStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                R1LeftName = "+2 Armor Penetration", R1LeftIcons = new int[] { ItemID.SharkToothNecklace, ItemID.StingerNecklace }, R1LeftStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R1RightName = "+1% Melee Crit Chance", R1RightIcons = meleeCritIcons, R1RightStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                MinorKeyName = "Precision", MinorKeyDesc = "+10 Armor Penetration.", MinorKeyIcon = ItemID.SharpeningStation, MinorKeyStats = p => p.GetArmorPenetration(DamageClass.Generic) += 10f,
                R2BaseName = "+1% Melee Crit Chance", R2BaseIcons = meleeCritIcons, R2BaseStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                R2LeftName = "+2 Armor Penetration", R2LeftIcons = new int[] { ItemID.SharkToothNecklace, ItemID.StingerNecklace }, R2LeftStats = p => p.GetArmorPenetration(DamageClass.Generic) += 2f,
                R2RightName = "+1% Melee Crit Chance", R2RightIcons = meleeCritIcons, R2RightStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                MajorKeyName = "Duelist", MajorKeyDesc = "Armor Penetration converts into Crit Multiplier.", MajorKeyIcon = ItemID.Katana, MajorKeyStats = p => { }
            });

            // Outer Fork 7: The Assassin
            BuildClassBranch(t7, ref id, 7, new BranchBlueprint {
                R1BaseName = "+2% Melee Damage", R1BaseIcons = meleeDmgIcons, R1BaseStats = p => p.GetDamage(DamageClass.Melee) += 0.02f,
                R1LeftName = "+1% Melee Crit Chance", R1LeftIcons = meleeCritIcons, R1LeftStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                R1RightName = "+2% Movement Speed", R1RightIcons = moveIcons, R1RightStats = p => p.moveSpeed += 0.02f,
                MinorKeyName = "First Strike", MinorKeyDesc = "+20% Melee Damage.", MinorKeyIcon = ItemID.WarriorEmblem, MinorKeyStats = p => p.GetDamage(DamageClass.Melee) += 0.20f,
                R2BaseName = "+2% Melee Damage", R2BaseIcons = meleeDmgIcons, R2BaseStats = p => p.GetDamage(DamageClass.Melee) += 0.02f,
                R2LeftName = "+1% Melee Crit Chance", R2LeftIcons = meleeCritIcons, R2LeftStats = p => p.GetCritChance(DamageClass.Melee) += 1f,
                R2RightName = "+2% Movement Speed", R2RightIcons = moveIcons, R2RightStats = p => p.moveSpeed += 0.02f,
                MajorKeyName = "Assassin", MajorKeyDesc = "Melee weapons deal double damage to enemies at full health.", MajorKeyIcon = ItemID.PsychoKnife, MajorKeyStats = p => { }
            });
        }
    }
}