using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using static WarpedAtlas.Content.Systems.PassiveTreeRegistry;

namespace WarpedAtlas.Content.Systems
{
    public static class SummonerPassiveTree
    {
        public static void Build() {
            int rootId = 4000;
            AddNode(rootId, "Summoner's Origin", "+5% Summon Damage, +10 Spirit Ward", new Point(0, 0), ItemID.SlimeStaff, NodeType.Origin, p => p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 10, p => p.GetDamage(DamageClass.Summon) += 0.05f);
            
            int id = 4001;
            
            int[] sumDmgIcons = { ItemID.SummonerEmblem, ItemID.PapyrusScarab, ItemID.HerculesBeetle };
            int[] minSpdIcons = { ItemID.BeeWax, ItemID.SummoningPotion };
            int[] moveIcons = { ItemID.SwiftnessPotion, ItemID.HermesBoots };
            int[] lifeIcons = { ItemID.LifeFruit, ItemID.LifeCrystal };
            int[] whipIcons = { ItemID.BlandWhip, ItemID.ThornWhip, ItemID.BoneWhip };
            int[] wardIcons = { ItemID.SpectreRobe, ItemID.SpectrePants, ItemID.Ectoplasm };
            int[] sentryIcons = { ItemID.QueenSpiderStaff, ItemID.StaffoftheFrostHydra, ItemID.DefenderMedal };
            int[] defIcons = { ItemID.IronskinPotion, ItemID.CobaltShield };
            int[] regenIcons = { ItemID.BandofRegeneration, ItemID.CharmofMyths };

            // --- DIR 0: THE SWARM (NORTH) ---
            int t0 = BuildClassBranch(rootId, ref id, 0, new BranchBlueprint { 
                R1BaseName = "+2% Summon Damage", R1BaseIcons = sumDmgIcons, R1BaseStats = p => p.GetDamage(DamageClass.Summon) += 0.02f,
                R1LeftName = "+1% Minion Attack Speed", R1LeftIcons = minSpdIcons, R1LeftStats = p => p.GetAttackSpeed(DamageClass.Summon) += 0.01f,
                R1RightName = "+2% Movement Speed", R1RightIcons = moveIcons, R1RightStats = p => p.moveSpeed += 0.02f,
                MinorKeyName = "The Hive", MinorKeyDesc = "+3 Max Minions, -30% Summon Damage.", MinorKeyIcon = ItemID.PygmyNecklace, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_TheHive = true,
                R2BaseName = "+2% Summon Damage", R2BaseIcons = sumDmgIcons, R2BaseStats = p => p.GetDamage(DamageClass.Summon) += 0.02f,
                R2LeftName = "+1% Minion Attack Speed", R2LeftIcons = minSpdIcons, R2LeftStats = p => p.GetAttackSpeed(DamageClass.Summon) += 0.01f,
                R2RightName = "+2% Movement Speed", R2RightIcons = moveIcons, R2RightStats = p => p.moveSpeed += 0.02f,
                MajorKeyName = "Feral Stampede", MajorKeyDesc = "+20% Summon Critical Strike Chance, -20% Movement Speed.", MajorKeyIcon = ItemID.HerculesBeetle, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_FeralStampede = true
            });

            // Outer Fork 0: The Apex Entity
            BuildClassBranch(t0, ref id, 0, new BranchBlueprint {
                R1BaseName = "+1 Max Minion", R1BaseIcons = new int[] { ItemID.BewitchingTable }, R1BaseStats = p => p.maxMinions += 1,
                R1LeftName = "+1 Max Minion", R1LeftIcons = new int[] { ItemID.BewitchingTable }, R1LeftStats = p => p.maxMinions += 1,
                R1RightName = "+1 Max Minion", R1RightIcons = new int[] { ItemID.BewitchingTable }, R1RightStats = p => p.maxMinions += 1,
                MinorKeyName = "Legion", MinorKeyDesc = "+2 Max Minions.", MinorKeyIcon = ItemID.NecromanticScroll, MinorKeyStats = p => p.maxMinions += 2,
                R2BaseName = "+1 Max Minion", R2BaseIcons = new int[] { ItemID.BewitchingTable }, R2BaseStats = p => p.maxMinions += 1,
                R2LeftName = "+1 Max Minion", R2LeftIcons = new int[] { ItemID.BewitchingTable }, R2LeftStats = p => p.maxMinions += 1,
                R2RightName = "+1 Max Minion", R2RightIcons = new int[] { ItemID.BewitchingTable }, R2RightStats = p => p.maxMinions += 1,
                MajorKeyName = "The Singularity", MajorKeyDesc = "Limits Max Minions to 1. Excess Minion capacity compounds into massive Damage multipliers.", MajorKeyIcon = ItemID.StardustCellStaff, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_TheSingularity = true
            });

            // --- DIR 1: LONE SOVEREIGN (NORTHEAST) ---
            int t1 = BuildClassBranch(rootId, ref id, 1, new BranchBlueprint { 
                R1BaseName = "+3% Summon Damage", R1BaseIcons = sumDmgIcons, R1BaseStats = p => p.GetDamage(DamageClass.Summon) += 0.03f,
                R1LeftName = "+10 Max Life", R1LeftIcons = lifeIcons, R1LeftStats = p => p.statLifeMax2 += 10,
                R1RightName = "+2% Movement Speed", R1RightIcons = moveIcons, R1RightStats = p => p.moveSpeed += 0.02f,
                MinorKeyName = "Puppeteer", MinorKeyDesc = "+20% Summon Damage. Whips deal 0 damage.", MinorKeyIcon = ItemID.NecromanticScroll, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Puppeteer = true,
                R2BaseName = "+3% Summon Damage", R2BaseIcons = sumDmgIcons, R2BaseStats = p => p.GetDamage(DamageClass.Summon) += 0.03f,
                R2LeftName = "+10 Max Life", R2LeftIcons = lifeIcons, R2LeftStats = p => p.statLifeMax2 += 10,
                R2RightName = "+2% Movement Speed", R2RightIcons = moveIcons, R2RightStats = p => p.moveSpeed += 0.02f,
                MajorKeyName = "Lone Sovereign", MajorKeyDesc = "Max 1 Minion, +100% Summon Damage.", MajorKeyIcon = ItemID.StardustDragonStaff, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_LoneSovereign = true
            });

            // Outer Fork 1: The Titan
            BuildClassBranch(t1, ref id, 1, new BranchBlueprint {
                R1BaseName = "+3% Summon Damage", R1BaseIcons = sumDmgIcons, R1BaseStats = p => p.GetDamage(DamageClass.Summon) += 0.03f,
                R1LeftName = "+3% Summon Damage", R1LeftIcons = sumDmgIcons, R1LeftStats = p => p.GetDamage(DamageClass.Summon) += 0.03f,
                R1RightName = "+3% Summon Damage", R1RightIcons = sumDmgIcons, R1RightStats = p => p.GetDamage(DamageClass.Summon) += 0.03f,
                MinorKeyName = "Colossal Minions", MinorKeyDesc = "+25% Summon Damage, -10% Minion Attack Speed.", MinorKeyIcon = ItemID.HerculesBeetle, MinorKeyStats = p => { p.GetDamage(DamageClass.Summon) += 0.25f; p.GetAttackSpeed(DamageClass.Summon) -= 0.10f; },
                R2BaseName = "+3% Summon Damage", R2BaseIcons = sumDmgIcons, R2BaseStats = p => p.GetDamage(DamageClass.Summon) += 0.03f,
                R2LeftName = "+3% Summon Damage", R2LeftIcons = sumDmgIcons, R2LeftStats = p => p.GetDamage(DamageClass.Summon) += 0.03f,
                R2RightName = "+3% Summon Damage", R2RightIcons = sumDmgIcons, R2RightStats = p => p.GetDamage(DamageClass.Summon) += 0.03f,
                MajorKeyName = "Gigantism", MajorKeyDesc = "+100% Summon Damage, but halves Minion Attack Speed.", MajorKeyIcon = ItemID.MaceWhip, MajorKeyStats = p => { p.GetDamage(DamageClass.Summon) += 1.00f; p.GetAttackSpeed(DamageClass.Summon) -= 0.50f; }
            });

            // --- DIR 2: WHIPMASTER (EAST) ---
            int t2 = BuildClassBranch(rootId, ref id, 2, new BranchBlueprint { 
                R1BaseName = "+2% Whip Damage", R1BaseIcons = whipIcons, R1BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusWhipDamage += 0.02f,
                R1LeftName = "+1% Summon Attack Speed", R1LeftIcons = new int[] { ItemID.JungleSpores, ItemID.FeralClaws }, R1LeftStats = p => p.GetAttackSpeed(DamageClass.Summon) += 0.01f,
                R1RightName = "+2 Defense", R1RightIcons = new int[] { ItemID.MonkBelt, ItemID.SquireShield }, R1RightStats = p => p.statDefense += 2,
                MinorKeyName = "Whipmaster", MinorKeyDesc = "+30% Whip Damage, -20% Summon Damage.", MinorKeyIcon = ItemID.FireWhip, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Whipmaster = true,
                R2BaseName = "+2% Whip Damage", R2BaseIcons = whipIcons, R2BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusWhipDamage += 0.02f,
                R2LeftName = "+1% Summon Attack Speed", R2LeftIcons = new int[] { ItemID.JungleSpores, ItemID.FeralClaws }, R2LeftStats = p => p.GetAttackSpeed(DamageClass.Summon) += 0.01f,
                R2RightName = "+2 Defense", R2RightIcons = new int[] { ItemID.MonkBelt, ItemID.SquireShield }, R2RightStats = p => p.statDefense += 2,
                MajorKeyName = "Frontline", MajorKeyDesc = "+3 Defense per Max Minion, -20% Movement Speed.", MajorKeyIcon = ItemID.ApprenticeScarf, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_Frontline = true
            });

            // Outer Fork 2: The Taskmaster
            BuildClassBranch(t2, ref id, 2, new BranchBlueprint {
                R1BaseName = "+1% Summon Attack Speed", R1BaseIcons = minSpdIcons, R1BaseStats = p => p.GetAttackSpeed(DamageClass.Summon) += 0.01f,
                R1LeftName = "+2% Whip Damage", R1LeftIcons = whipIcons, R1LeftStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusWhipDamage += 0.02f,
                R1RightName = "+2% Whip Damage", R1RightIcons = whipIcons, R1RightStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusWhipDamage += 0.02f,
                MinorKeyName = "Frenzy", MinorKeyDesc = "+15% Summon Attack Speed.", MinorKeyIcon = ItemID.ScytheWhip, MinorKeyStats = p => p.GetAttackSpeed(DamageClass.Summon) += 0.15f,
                R2BaseName = "+1% Summon Attack Speed", R2BaseIcons = minSpdIcons, R2BaseStats = p => p.GetAttackSpeed(DamageClass.Summon) += 0.01f,
                R2LeftName = "+2% Whip Damage", R2LeftIcons = whipIcons, R2LeftStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusWhipDamage += 0.02f,
                R2RightName = "+2% Whip Damage", R2RightIcons = whipIcons, R2RightStats = p => p.GetModPlayer<AtlasProgressionPlayer>().BonusWhipDamage += 0.02f,
                MajorKeyName = "Cruel Lash", MajorKeyDesc = "Converts Summon Attack Speed bonuses into Whip Damage.", MajorKeyIcon = ItemID.RainbowWhip, MajorKeyStats = p => { float bonus = p.GetAttackSpeed(DamageClass.Summon) - 1f; if (bonus > 0) { p.GetModPlayer<AtlasProgressionPlayer>().BonusWhipDamage += bonus; p.GetAttackSpeed(DamageClass.Summon) = 1f; } }
            });

            // --- DIR 3: SENTRY COMMANDER (SOUTHEAST) ---
            int t3 = BuildClassBranch(rootId, ref id, 3, new BranchBlueprint { 
                R1BaseName = "+2% Summon Damage", R1BaseIcons = new int[] { ItemID.DefenderMedal, ItemID.Ale }, R1BaseStats = p => p.GetDamage(DamageClass.Summon) += 0.02f,
                R1LeftName = "+1% Damage Reduction", R1LeftIcons = new int[] { ItemID.WormScarf, ItemID.EndurancePotion }, R1LeftStats = p => p.endurance += 0.01f,
                R1RightName = "+2 Defense", R1RightIcons = new int[] { ItemID.CobaltShield, ItemID.ApprenticeScarf }, R1RightStats = p => p.statDefense += 2,
                MinorKeyName = "Sentry Commander", MinorKeyDesc = "+3 Max Sentries, Max 0 Minions.", MinorKeyIcon = ItemID.SquireShield, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_SentryCommander = true,
                R2BaseName = "+2% Summon Damage", R2BaseIcons = new int[] { ItemID.DefenderMedal, ItemID.Ale }, R2BaseStats = p => p.GetDamage(DamageClass.Summon) += 0.02f,
                R2LeftName = "+1% Damage Reduction", R2LeftIcons = new int[] { ItemID.WormScarf, ItemID.EndurancePotion }, R2LeftStats = p => p.endurance += 0.01f,
                R2RightName = "+2 Defense", R2RightIcons = new int[] { ItemID.CobaltShield, ItemID.ApprenticeScarf }, R2RightStats = p => p.statDefense += 2,
                MajorKeyName = "Soul Link", MajorKeyDesc = "+15% Damage Reduction, -10% Summon Damage.", MajorKeyIcon = ItemID.StarVeil, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_SoulLink = true
            });

            // Outer Fork 3: The Siege Master
            BuildClassBranch(t3, ref id, 3, new BranchBlueprint {
                R1BaseName = "+1 Max Sentry", R1BaseIcons = sentryIcons, R1BaseStats = p => p.maxTurrets += 1,
                R1LeftName = "+2 Defense", R1LeftIcons = defIcons, R1LeftStats = p => p.statDefense += 2,
                R1RightName = "+2 Defense", R1RightIcons = defIcons, R1RightStats = p => p.statDefense += 2,
                MinorKeyName = "Turret Plating", MinorKeyDesc = "+2 Defense per Max Sentry.", MinorKeyIcon = ItemID.HuntressBuckler, MinorKeyStats = p => p.statDefense += (p.maxTurrets * 2),
                R2BaseName = "+1 Max Sentry", R2BaseIcons = sentryIcons, R2BaseStats = p => p.maxTurrets += 1,
                R2LeftName = "+2 Defense", R2LeftIcons = defIcons, R2LeftStats = p => p.statDefense += 2,
                R2RightName = "+2 Defense", R2RightIcons = defIcons, R2RightStats = p => p.statDefense += 2,
                MajorKeyName = "Fortification", MajorKeyDesc = "Gain +5 Defense for every Max Sentry, but Max Minions is set to 0.", MajorKeyIcon = ItemID.MonkBelt, MajorKeyStats = p => { p.statDefense += (p.maxTurrets * 5); p.maxMinions = 0; }
            });

            // --- DIR 4: SPIRIT WARD (SOUTH) ---
            int t4 = BuildClassBranch(rootId, ref id, 4, new BranchBlueprint { 
                R1BaseName = "+10 Max Spirit Ward", R1BaseIcons = wardIcons, R1BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 10,
                R1LeftName = "+2% Summon Damage", R1LeftIcons = new int[] { ItemID.SpookyWood, ItemID.Pumpkin }, R1LeftStats = p => p.GetDamage(DamageClass.Summon) += 0.02f,
                R1RightName = "+1% Minion Attack Speed", R1RightIcons = minSpdIcons, R1RightStats = p => p.GetAttackSpeed(DamageClass.Summon) += 0.01f,
                MinorKeyName = "Spirit Mantle", MinorKeyDesc = "+50 Max Spirit Ward.", MinorKeyIcon = ItemID.SpectreHood, MinorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 50,
                R2BaseName = "+10 Max Spirit Ward", R2BaseIcons = wardIcons, R2BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 10,
                R2LeftName = "+2% Summon Damage", R2LeftIcons = new int[] { ItemID.SpookyWood, ItemID.Pumpkin }, R2LeftStats = p => p.GetDamage(DamageClass.Summon) += 0.02f,
                R2RightName = "+1% Minion Attack Speed", R2RightIcons = minSpdIcons, R2RightStats = p => p.GetAttackSpeed(DamageClass.Summon) += 0.01f,
                MajorKeyName = "Vampiric Souls", MajorKeyDesc = "+100 Max Spirit Ward, -10% Summon Damage.", MajorKeyIcon = ItemID.Phantasm, MajorKeyStats = p => { p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 100; p.GetDamage(DamageClass.Summon) -= 0.10f; }
            });

            // Outer Fork 4: The Blood Priest
            BuildClassBranch(t4, ref id, 4, new BranchBlueprint {
                R1BaseName = "+10 Max Spirit Ward", R1BaseIcons = wardIcons, R1BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 10,
                R1LeftName = "+10 Max Spirit Ward", R1LeftIcons = wardIcons, R1LeftStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 10,
                R1RightName = "+10 Max Spirit Ward", R1RightIcons = wardIcons, R1RightStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 10,
                MinorKeyName = "Spirit Overflow", MinorKeyDesc = "+2% Summon Damage per 10 Spirit Ward.", MinorKeyIcon = ItemID.Ectoplasm, MinorKeyStats = p => p.GetDamage(DamageClass.Summon) += (p.GetModPlayer<AtlasProgressionPlayer>().MaxWard / 10) * 0.02f,
                R2BaseName = "+10 Max Spirit Ward", R2BaseIcons = wardIcons, R2BaseStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 10,
                R2LeftName = "+10 Max Spirit Ward", R2LeftIcons = wardIcons, R2LeftStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 10,
                R2RightName = "+10 Max Spirit Ward", R2RightIcons = wardIcons, R2RightStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 10,
                MajorKeyName = "Symbiotic Link", MajorKeyDesc = "Ward cannot regenerate natively, but 100% of Minion Damage is leeched as Ward.", MajorKeyIcon = ItemID.SpectreMask, MajorKeyStats = p => p.GetModPlayer<AtlasProgressionPlayer>().KS_SymbioticLink = true
            });

            // --- DIR 5: FERAL BEAST (SOUTHWEST) ---
            int t5 = BuildClassBranch(rootId, ref id, 5, new BranchBlueprint { 
                R1BaseName = "+1% Summon Crit Chance", R1BaseIcons = sumDmgIcons, R1BaseStats = p => p.GetCritChance(DamageClass.Summon) += 1f,
                R1LeftName = "+1% Minion Attack Speed", R1LeftIcons = minSpdIcons, R1LeftStats = p => p.GetAttackSpeed(DamageClass.Summon) += 0.01f,
                R1RightName = "+2% Movement Speed", R1RightIcons = moveIcons, R1RightStats = p => p.moveSpeed += 0.02f,
                MinorKeyName = "Rabies", MinorKeyDesc = "+15% Summon Crit Chance, -10 Defense.", MinorKeyIcon = ItemID.FeralClaws, MinorKeyStats = p => { p.GetCritChance(DamageClass.Summon) += 15f; p.statDefense -= (int)p.statDefense; },
                R2BaseName = "+1% Summon Crit Chance", R2BaseIcons = sumDmgIcons, R2BaseStats = p => p.GetCritChance(DamageClass.Summon) += 1f,
                R2LeftName = "+1% Minion Attack Speed", R2LeftIcons = minSpdIcons, R2LeftStats = p => p.GetAttackSpeed(DamageClass.Summon) += 0.01f,
                R2RightName = "+2% Movement Speed", R2RightIcons = moveIcons, R2RightStats = p => p.moveSpeed += 0.02f,
                MajorKeyName = "Beastmaster", MajorKeyDesc = "+30% Summon Crit Chance.", MajorKeyIcon = ItemID.SpookyHelmet, MajorKeyStats = p => p.GetCritChance(DamageClass.Summon) += 30f
            });

            // Outer Fork 5: The Stampede
            BuildClassBranch(t5, ref id, 5, new BranchBlueprint {
                R1BaseName = "+2% Movement Speed", R1BaseIcons = moveIcons, R1BaseStats = p => p.moveSpeed += 0.02f,
                R1LeftName = "+2% Movement Speed", R1LeftIcons = moveIcons, R1LeftStats = p => p.moveSpeed += 0.02f,
                R1RightName = "+2% Movement Speed", R1RightIcons = moveIcons, R1RightStats = p => p.moveSpeed += 0.02f,
                MinorKeyName = "Agility", MinorKeyDesc = "+10% Movement Speed.", MinorKeyIcon = ItemID.LightningBoots, MinorKeyStats = p => p.moveSpeed += 0.10f,
                R2BaseName = "+2% Movement Speed", R2BaseIcons = moveIcons, R2BaseStats = p => p.moveSpeed += 0.02f,
                R2LeftName = "+2% Movement Speed", R2LeftIcons = moveIcons, R2LeftStats = p => p.moveSpeed += 0.02f,
                R2RightName = "+2% Movement Speed", R2RightIcons = moveIcons, R2RightStats = p => p.moveSpeed += 0.02f,
                MajorKeyName = "Momentum", MajorKeyDesc = "Converts all Movement Speed bonuses into Minion Attack Speed.", MajorKeyIcon = ItemID.FrostsparkBoots, MajorKeyStats = p => { float bonus = p.moveSpeed - 1f; if(bonus > 0) { p.GetAttackSpeed(DamageClass.Summon) += bonus; p.moveSpeed = 1f; } }
            });

            // --- DIR 6: COMMANDER (WEST) ---
            int t6 = BuildClassBranch(rootId, ref id, 6, new BranchBlueprint { 
                R1BaseName = "+2 Defense", R1BaseIcons = defIcons, R1BaseStats = p => p.statDefense += 2,
                R1LeftName = "+2% Summon Damage", R1LeftIcons = sumDmgIcons, R1LeftStats = p => p.GetDamage(DamageClass.Summon) += 0.02f,
                R1RightName = "+10 Max Life", R1RightIcons = lifeIcons, R1RightStats = p => p.statLifeMax2 += 10,
                MinorKeyName = "Shield Wall", MinorKeyDesc = "+15 Defense.", MinorKeyIcon = ItemID.ObsidianShield, MinorKeyStats = p => p.statDefense += 15,
                R2BaseName = "+2 Defense", R2BaseIcons = defIcons, R2BaseStats = p => p.statDefense += 2,
                R2LeftName = "+2% Summon Damage", R2LeftIcons = sumDmgIcons, R2LeftStats = p => p.GetDamage(DamageClass.Summon) += 0.02f,
                R2RightName = "+10 Max Life", R2RightIcons = lifeIcons, R2RightStats = p => p.statLifeMax2 += 10,
                MajorKeyName = "Phalanx", MajorKeyDesc = "+20 Defense, -15% Movement Speed.", MajorKeyIcon = ItemID.PaladinsShield, MajorKeyStats = p => { p.statDefense += 20; p.moveSpeed -= 0.15f; }
            });

            // Outer Fork 6: The Tactician
            BuildClassBranch(t6, ref id, 6, new BranchBlueprint {
                R1BaseName = "+2 Defense", R1BaseIcons = defIcons, R1BaseStats = p => p.statDefense += 2,
                R1LeftName = "+2 Defense", R1LeftIcons = defIcons, R1LeftStats = p => p.statDefense += 2,
                R1RightName = "+2 Defense", R1RightIcons = defIcons, R1RightStats = p => p.statDefense += 2,
                MinorKeyName = "Iron Will", MinorKeyDesc = "+10 Defense.", MinorKeyIcon = ItemID.Shackle, MinorKeyStats = p => p.statDefense += 10,
                R2BaseName = "+2 Defense", R2BaseIcons = defIcons, R2BaseStats = p => p.statDefense += 2,
                R2LeftName = "+2 Defense", R2LeftIcons = defIcons, R2LeftStats = p => p.statDefense += 2,
                R2RightName = "+2 Defense", R2RightIcons = defIcons, R2RightStats = p => p.statDefense += 2,
                MajorKeyName = "Vanguard", MajorKeyDesc = "Converts all Defense into Summon Damage.", MajorKeyIcon = ItemID.WarriorEmblem, MajorKeyStats = p => { if(p.statDefense > 0) { p.GetDamage(DamageClass.Summon) += ((int)p.statDefense * 0.01f); p.statDefense -= (int)p.statDefense; } }
            });

            // --- DIR 7: ANIMIST (NORTHWEST) ---
            int t7 = BuildClassBranch(rootId, ref id, 7, new BranchBlueprint { 
                R1BaseName = "+1 Life Regeneration", R1BaseIcons = regenIcons, R1BaseStats = p => p.lifeRegen += 1,
                R1LeftName = "+10 Max Spirit Ward", R1LeftIcons = wardIcons, R1LeftStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 10,
                R1RightName = "+2% Summon Damage", R1RightIcons = sumDmgIcons, R1RightStats = p => p.GetDamage(DamageClass.Summon) += 0.02f,
                MinorKeyName = "Healing Totem", MinorKeyDesc = "+5 Life Regeneration.", MinorKeyIcon = ItemID.BandofRegeneration, MinorKeyStats = p => p.lifeRegen += 5,
                R2BaseName = "+1 Life Regeneration", R2BaseIcons = regenIcons, R2BaseStats = p => p.lifeRegen += 1,
                R2LeftName = "+10 Max Spirit Ward", R2LeftIcons = wardIcons, R2LeftStats = p => p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 10,
                R2RightName = "+2% Summon Damage", R2RightIcons = sumDmgIcons, R2RightStats = p => p.GetDamage(DamageClass.Summon) += 0.02f,
                MajorKeyName = "Nature's Gift", MajorKeyDesc = "+50 Max Life, +50 Max Spirit Ward.", MajorKeyIcon = ItemID.NaturesGift, MajorKeyStats = p => { p.statLifeMax2 += 50; p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += 50; }
            });

            // Outer Fork 7: The Shaman
            BuildClassBranch(t7, ref id, 7, new BranchBlueprint {
                R1BaseName = "+1 Life Regeneration", R1BaseIcons = regenIcons, R1BaseStats = p => p.lifeRegen += 1,
                R1LeftName = "+1 Life Regeneration", R1LeftIcons = regenIcons, R1LeftStats = p => p.lifeRegen += 1,
                R1RightName = "+1 Life Regeneration", R1RightIcons = regenIcons, R1RightStats = p => p.lifeRegen += 1,
                MinorKeyName = "Vitality", MinorKeyDesc = "+10 Life Regeneration.", MinorKeyIcon = ItemID.CharmofMyths, MinorKeyStats = p => p.lifeRegen += 10,
                R2BaseName = "+1 Life Regeneration", R2BaseIcons = regenIcons, R2BaseStats = p => p.lifeRegen += 1,
                R2LeftName = "+1 Life Regeneration", R2LeftIcons = regenIcons, R2LeftStats = p => p.lifeRegen += 1,
                R2RightName = "+1 Life Regeneration", R2RightIcons = regenIcons, R2RightStats = p => p.lifeRegen += 1,
                MajorKeyName = "Life Weaver", MajorKeyDesc = "Disables Life Regeneration, converting it all into massive Spirit Ward capacity.", MajorKeyIcon = ItemID.LifeFruit, MajorKeyStats = p => { if(p.lifeRegen > 0) { p.GetModPlayer<AtlasProgressionPlayer>().MaxWard += (p.lifeRegen * 10); p.lifeRegen = 0; } }
            });
        }
    }
}