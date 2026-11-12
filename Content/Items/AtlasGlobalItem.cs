using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using WarpedAtlas.Content.Players;

namespace WarpedAtlas.Content.Items
{
    public enum ItemTier { Normal, Magic, Rare, Unique }

    public class Affix
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string StatClass { get; set; } 
        
        public float MinDamage { get; set; } public float MaxDamage { get; set; } public float CurrentDamage { get; set; }
        public float MinSpeed { get; set; } public float MaxSpeed { get; set; } public float CurrentSpeed { get; set; }
        public int MinCrit { get; set; } public int MaxCrit { get; set; } public int CurrentCrit { get; set; }
        public int MinDefense { get; set; } public int MaxDefense { get; set; } public int CurrentDefense { get; set; }
        public float MinMoveSpeed { get; set; } public float MaxMoveSpeed { get; set; } public float CurrentMoveSpeed { get; set; }
        
        public int MinMana { get; set; } public int MaxMana { get; set; } public int CurrentMana { get; set; }
        public int MinManaRegen { get; set; } public int MaxManaRegen { get; set; } public int CurrentManaRegen { get; set; }
        public float MinJump { get; set; } public float MaxJump { get; set; } public float CurrentJump { get; set; }
        public int MinLife { get; set; } public int MaxLife { get; set; } public int CurrentLife { get; set; }
        public int MinLifeRegen { get; set; } public int MaxLifeRegen { get; set; } public int CurrentLifeRegen { get; set; }
        
        // --- NEW ARPG VECTORS ---
        public int MinEnergyShield { get; set; } public int MaxEnergyShield { get; set; } public int CurrentEnergyShield { get; set; }
        public int MinWard { get; set; } public int MaxWard { get; set; } public int CurrentWard { get; set; }
        public float MinEvasion { get; set; } public float MaxEvasion { get; set; } public float CurrentEvasion { get; set; }
        public float MinEndurance { get; set; } public float MaxEndurance { get; set; } public float CurrentEndurance { get; set; }
        public int MinArmorPen { get; set; } public int MaxArmorPen { get; set; } public int CurrentArmorPen { get; set; }
        public int MinMinions { get; set; } public int MaxMinions { get; set; } public int CurrentMinions { get; set; }

        public Affix(string name, string type, float minDmg=0, float maxDmg=0, float minSpd=0, float maxSpd=0, int minCrit=0, int maxCrit=0, int minDef=0, int maxDef=0, float minMove=0, float maxMove=0, string statClass="Generic", int minMana=0, int maxMana=0, int minManaRegen=0, int maxManaRegen=0, float minJump=0, float maxJump=0, int minLife=0, int maxLife=0, int minLifeRegen=0, int maxLifeRegen=0, int minEs=0, int maxEs=0, int minWard=0, int maxWard=0, float minEva=0, float maxEva=0, float minEnd=0, float maxEnd=0, int minPen=0, int maxPen=0, int minMinions=0, int maxMinions=0)
        {
            Name = name; Type = type; StatClass = statClass;
            MinDamage = minDmg; MaxDamage = maxDmg;
            MinSpeed = minSpd; MaxSpeed = maxSpd;
            MinCrit = minCrit; MaxCrit = maxCrit;
            MinDefense = minDef; MaxDefense = maxDef;
            MinMoveSpeed = minMove; MaxMoveSpeed = maxMove;
            MinMana = minMana; MaxMana = maxMana;
            MinManaRegen = minManaRegen; MaxManaRegen = maxManaRegen;
            MinJump = minJump; MaxJump = maxJump;
            MinLife = minLife; MaxLife = maxLife;
            MinLifeRegen = minLifeRegen; MaxLifeRegen = maxLifeRegen;
            MinEnergyShield = minEs; MaxEnergyShield = maxEs;
            MinWard = minWard; MaxWard = maxWard;
            MinEvasion = minEva; MaxEvasion = maxEva;
            MinEndurance = minEnd; MaxEndurance = maxEnd;
            MinArmorPen = minPen; MaxArmorPen = maxPen;
            MinMinions = minMinions; MaxMinions = maxMinions;
        }

        public Affix Clone() => (Affix)this.MemberwiseClone();
        
        public Affix CloneAndRoll() {
            var clone = this.Clone();
            clone.RollValues();
            return clone;
        }

        public void RollValues() {
            CurrentDamage = Main.rand.NextFloat(MinDamage, MaxDamage);
            CurrentSpeed = Main.rand.NextFloat(MinSpeed, MaxSpeed);
            CurrentCrit = Main.rand.Next(MinCrit, MaxCrit + 1);
            CurrentDefense = Main.rand.Next(MinDefense, MaxDefense + 1);
            CurrentMoveSpeed = Main.rand.NextFloat(MinMoveSpeed, MaxMoveSpeed);
            CurrentMana = Main.rand.Next(MinMana, MaxMana + 1);
            CurrentManaRegen = Main.rand.Next(MinManaRegen, MaxManaRegen + 1);
            CurrentJump = Main.rand.NextFloat(MinJump, MaxJump);
            CurrentLife = Main.rand.Next(MinLife, MaxLife + 1);
            CurrentLifeRegen = Main.rand.Next(MinLifeRegen, MaxLifeRegen + 1);
            CurrentEnergyShield = Main.rand.Next(MinEnergyShield, MaxEnergyShield + 1);
            CurrentWard = Main.rand.Next(MinWard, MaxWard + 1);
            CurrentEvasion = Main.rand.NextFloat(MinEvasion, MaxEvasion);
            CurrentEndurance = Main.rand.NextFloat(MinEndurance, MaxEndurance);
            CurrentArmorPen = Main.rand.Next(MinArmorPen, MaxArmorPen + 1);
            CurrentMinions = Main.rand.Next(MinMinions, MaxMinions + 1);
        }
    }

    public class AtlasGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        
        public ItemTier CurrentTier = ItemTier.Normal;
        public List<Affix> Prefixes = new List<Affix>();
        public List<Affix> Suffixes = new List<Affix>();

        // =========================================================================
        // WEAPON AFFIXES (Damage, Speed, Armor Pen, Crit, Life/Mana Sustain)
        // =========================================================================
        public static List<Affix> WeaponPrefixes = new List<Affix>() {
            new Affix("Heavy", "Prefix", minDmg: 0.10f, maxDmg: 0.20f, minSpd: -0.15f, maxSpd: -0.05f),
            new Affix("Savage", "Prefix", minDmg: 0.15f, maxDmg: 0.25f),
            new Affix("Lethal", "Prefix", minCrit: 10, maxCrit: 15),
            // Warrior
            new Affix("Cleaving", "Prefix", minDmg: 0.10f, maxDmg: 0.20f, minPen: 2, maxPen: 5, statClass: "Melee"),
            new Affix("Vampiric", "Prefix", minDmg: 0.05f, maxDmg: 0.15f, minLifeRegen: 2, maxLifeRegen: 6, statClass: "Melee"),
            // Ranger
            new Affix("Piercing", "Prefix", minDmg: 0.10f, maxDmg: 0.20f, minPen: 4, maxPen: 8, statClass: "Ranged"),
            new Affix("Ballistic", "Prefix", minSpd: 0.10f, maxSpd: 0.20f, minCrit: 4, maxCrit: 8, statClass: "Ranged"),
            // Sorcerer
            new Affix("Eldritch", "Prefix", minDmg: 0.15f, maxDmg: 0.25f, minMana: 20, maxMana: 40, statClass: "Magic"),
            new Affix("Channeling", "Prefix", minSpd: 0.10f, maxSpd: 0.20f, minManaRegen: 2, maxManaRegen: 5, statClass: "Magic"),
            // Summoner
            new Affix("Commanding", "Prefix", minDmg: 0.15f, maxDmg: 0.30f, statClass: "Summon"),
            new Affix("Whipmaster's", "Prefix", minDmg: 0.10f, maxDmg: 0.20f, minSpd: 0.10f, maxSpd: 0.15f, statClass: "Summon")
        };

        public static List<Affix> WeaponSuffixes = new List<Affix>() {
            new Affix("of Precision", "Suffix", minCrit: 8, maxCrit: 15),
            new Affix("of Destruction", "Suffix", minDmg: 0.15f, maxDmg: 0.25f),
            // Warrior
            new Affix("of the Juggernaut", "Suffix", minDmg: 0.10f, maxDmg: 0.20f, minLife: 10, maxLife: 20, statClass: "Melee"),
            new Affix("of Slaughter", "Suffix", minCrit: 5, maxCrit: 12, minPen: 3, maxPen: 6, statClass: "Melee"),
            // Ranger
            new Affix("of the Sniper", "Suffix", minCrit: 10, maxCrit: 20, minPen: 2, maxPen: 5, statClass: "Ranged"),
            new Affix("of the Wind", "Suffix", minSpd: 0.10f, maxSpd: 0.20f, minMove: 0.05f, maxMove: 0.10f, statClass: "Ranged"),
            // Sorcerer
            new Affix("of the Archmage", "Suffix", minDmg: 0.10f, maxDmg: 0.20f, minMana: 15, maxMana: 35, statClass: "Magic"),
            new Affix("of the Oracle", "Suffix", minCrit: 5, maxCrit: 10, minManaRegen: 3, maxManaRegen: 6, statClass: "Magic"),
            // Summoner
            new Affix("of the Swarm", "Suffix", minDmg: 0.10f, maxDmg: 0.20f, minWard: 10, maxWard: 20, statClass: "Summon"),
            new Affix("of the Sovereign", "Suffix", minDmg: 0.20f, maxDmg: 0.35f, statClass: "Summon")
        };

        // =========================================================================
        // ARMOR AFFIXES (Defense, Resource Pools, Damage Reduction, Evasion)
        // =========================================================================
        public static List<Affix> ArmorPrefixes = new List<Affix>() {
            new Affix("Plated", "Prefix", minDef: 2, maxDef: 5),
            new Affix("Fortified", "Prefix", minDef: 5, maxDef: 10),
            // Warrior
            new Affix("Colossal", "Prefix", minLife: 15, maxLife: 30, minEnd: 0.02f, maxEnd: 0.05f, statClass: "Melee"),
            new Affix("Impenetrable", "Prefix", minDef: 6, maxDef: 12, minEnd: 0.01f, maxEnd: 0.03f, statClass: "Melee"),
            // Ranger
            new Affix("Acrobat's", "Prefix", minEva: 0.02f, maxEva: 0.05f, minMove: 0.05f, maxMove: 0.10f, statClass: "Ranged"),
            new Affix("Elusive", "Prefix", minEva: 0.04f, maxEva: 0.08f, statClass: "Ranged"),
            // Sorcerer
            new Affix("Arcane", "Prefix", minEs: 15, maxEs: 30, minMana: 10, maxMana: 25, statClass: "Magic"),
            new Affix("Resonant", "Prefix", minEs: 25, maxEs: 50, statClass: "Magic"),
            // Summoner
            new Affix("Spiritual", "Prefix", minWard: 15, maxWard: 30, minDef: 2, maxDef: 5, statClass: "Summon"),
            new Affix("Animist's", "Prefix", minWard: 25, maxWard: 50, statClass: "Summon")
        };

        public static List<Affix> ArmorSuffixes = new List<Affix>() {
            new Affix("of Resilience", "Suffix", minDef: 2, maxDef: 6),
            // Warrior
            new Affix("of the Titan", "Suffix", minLife: 20, maxLife: 40, statClass: "Melee"),
            new Affix("of the Mountain", "Suffix", minDef: 4, maxDef: 8, minLifeRegen: 1, maxLifeRegen: 3, statClass: "Melee"),
            // Ranger
            new Affix("of the Phantom", "Suffix", minEva: 0.03f, maxEva: 0.06f, minMove: 0.04f, maxMove: 0.08f, statClass: "Ranged"),
            new Affix("of the Cheetah", "Suffix", minMove: 0.10f, maxMove: 0.18f, statClass: "Ranged"),
            // Sorcerer
            new Affix("of the Covenant", "Suffix", minEs: 20, maxEs: 40, minManaRegen: 2, maxManaRegen: 4, statClass: "Magic"),
            new Affix("of the Barrier", "Suffix", minEs: 30, maxEs: 60, statClass: "Magic"),
            // Summoner
            new Affix("of the Soul", "Suffix", minWard: 20, maxWard: 40, minLifeRegen: 1, maxLifeRegen: 3, statClass: "Summon"),
            new Affix("of the Hive", "Suffix", minWard: 30, maxWard: 60, statClass: "Summon")
        };

        // =========================================================================
        // ACCESSORY AFFIXES (Movement, Jump, Minion Capacity, Specific Enhancers)
        // =========================================================================
        public static List<Affix> AccessoryPrefixes = new List<Affix>() {
            new Affix("Brisk", "Prefix", minMove: 0.05f, maxMove: 0.10f),
            new Affix("Soaring", "Prefix", minJump: 0.15f, maxJump: 0.30f),
            new Affix("Vital", "Prefix", minLife: 10, maxLife: 25),
            // Warrior
            new Affix("Brawler's", "Prefix", minDmg: 0.05f, maxDmg: 0.10f, minEnd: 0.01f, maxEnd: 0.03f, statClass: "Melee"),
            // Ranger
            new Affix("Marksman's", "Prefix", minDmg: 0.05f, maxDmg: 0.10f, minPen: 3, maxPen: 6, statClass: "Ranged"),
            // Sorcerer
            new Affix("Sorcerer's", "Prefix", minDmg: 0.05f, maxDmg: 0.10f, minMana: 15, maxMana: 30, statClass: "Magic"),
            // Summoner
            new Affix("Summoner's", "Prefix", minDmg: 0.08f, maxDmg: 0.15f, minWard: 10, maxWard: 20, statClass: "Summon")
        };

        public static List<Affix> AccessorySuffixes = new List<Affix>() {
            new Affix("of Agility", "Suffix", minMove: 0.05f, maxMove: 0.10f, minJump: 0.10f, maxJump: 0.20f),
            new Affix("of Clarity", "Suffix", minMana: 10, maxMana: 20, minManaRegen: 2, maxManaRegen: 4),
            // Warrior
            new Affix("of the Gladiator", "Suffix", minEnd: 0.02f, maxEnd: 0.04f, minLife: 15, maxLife: 30, statClass: "Melee"),
            // Ranger
            new Affix("of the Scout", "Suffix", minEva: 0.03f, maxEva: 0.06f, minPen: 2, maxPen: 5, statClass: "Ranged"),
            // Sorcerer
            new Affix("of the Invoker", "Suffix", minEs: 15, maxEs: 35, minManaRegen: 2, maxManaRegen: 5, statClass: "Magic"),
            // Summoner
            new Affix("of the Ritual", "Suffix", minWard: 15, maxWard: 35, minMinions: 1, maxMinions: 1, statClass: "Summon") // Extremely powerful Suffix
        };

        public static bool IsValidForAffixes(Item item) {
            return item != null && !item.IsAir && item.maxStack == 1 && (item.damage > 0 || item.accessory || item.defense > 0) && !(item.ModItem is Uniques.AtlasUniqueItem);
        }

        public static List<Affix> GetPool(Item item, string type) {
            List<Affix> rawPool;
            
            if (item.accessory) rawPool = type == "Prefix" ? AccessoryPrefixes : AccessorySuffixes;
            else if (item.defense > 0) rawPool = type == "Prefix" ? ArmorPrefixes : ArmorSuffixes;
            else rawPool = type == "Prefix" ? WeaponPrefixes : WeaponSuffixes;

            string playerClass = Main.LocalPlayer.GetModPlayer<AtlasProgressionPlayer>().ChosenClass;
            string allowedStatClass = playerClass switch {
                "Warrior" => "Melee",
                "Ranger" => "Ranged",
                "Sorcerer" => "Magic",
                "Summoner" => "Summon",
                _ => "Generic"
            };

            return rawPool.Where(aff => aff.StatClass == "Generic" || aff.StatClass == allowedStatClass).ToList();
        }

        public override bool AllowPrefix(Item item, int pre) {
            if (IsValidForAffixes(item)) return false; 
            return true; 
        }

        public override int ChoosePrefix(Item item, Terraria.Utilities.UnifiedRandom rand) {
            if (IsValidForAffixes(item)) return 0; 
            return base.ChoosePrefix(item, rand);
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
            if (item.damage > 0 && !item.accessory) {
                foreach (var aff in Prefixes.Concat(Suffixes)) damage += aff.CurrentDamage;
            }
        }

        public override float UseSpeedMultiplier(Item item, Player player) {
            if (item.damage > 0 && !item.accessory) {
                float mult = 1f;
                foreach (var aff in Prefixes.Concat(Suffixes)) mult += aff.CurrentSpeed;
                return mult;
            }
            return 1f;
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit) {
            if (item.damage > 0 && !item.accessory) {
                foreach (var aff in Prefixes.Concat(Suffixes)) crit += aff.CurrentCrit;
            }
        }

        public override void HoldItem(Item item, Player player) {
            if (item.damage > 0 && !item.accessory) {
                foreach (var aff in Prefixes.Concat(Suffixes)) {
                    player.statManaMax2 += aff.CurrentMana;
                    player.manaRegen += aff.CurrentManaRegen;
                    player.jumpSpeedBoost += aff.CurrentJump;
                    player.statLifeMax2 += aff.CurrentLife;
                    player.lifeRegen += aff.CurrentLifeRegen;
                    player.GetArmorPenetration(DamageClass.Generic) += aff.CurrentArmorPen;
                    
                    var prog = player.GetModPlayer<AtlasProgressionPlayer>();
                    prog.MaxEnergyShield += aff.CurrentEnergyShield;
                    prog.MaxWard += aff.CurrentWard;
                    prog.EvasionChance += aff.CurrentEvasion;
                    player.endurance += aff.CurrentEndurance;
                    player.maxMinions += aff.CurrentMinions;
                }
            }
        }

        public override void UpdateEquip(Item item, Player player) {
            if (item.accessory || item.defense > 0) {
                var prog = player.GetModPlayer<AtlasProgressionPlayer>();
                foreach (var aff in Prefixes.Concat(Suffixes)) {
                    player.statDefense += aff.CurrentDefense;
                    player.moveSpeed += aff.CurrentMoveSpeed;
                    
                    player.statManaMax2 += aff.CurrentMana;
                    player.manaRegen += aff.CurrentManaRegen;
                    player.jumpSpeedBoost += aff.CurrentJump;
                    player.statLifeMax2 += aff.CurrentLife;
                    player.lifeRegen += aff.CurrentLifeRegen;
                    player.GetArmorPenetration(DamageClass.Generic) += aff.CurrentArmorPen;
                    
                    prog.MaxEnergyShield += aff.CurrentEnergyShield;
                    prog.MaxWard += aff.CurrentWard;
                    prog.EvasionChance += aff.CurrentEvasion;
                    player.endurance += aff.CurrentEndurance;
                    player.maxMinions += aff.CurrentMinions;
                    
                    if (item.accessory) {
                        DamageClass dmgClass = aff.StatClass switch {
                            "Melee" => DamageClass.Melee,
                            "Ranged" => DamageClass.Ranged,
                            "Magic" => DamageClass.Magic,
                            "Summon" => DamageClass.Summon,
                            _ => DamageClass.Generic
                        };
                        
                        player.GetDamage(dmgClass) += aff.CurrentDamage;
                        player.GetCritChance(dmgClass) += aff.CurrentCrit;
                        player.GetAttackSpeed(dmgClass) += aff.CurrentSpeed;
                    }
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (CurrentTier == ItemTier.Unique) return;

            if (Prefixes.Count > 0 || Suffixes.Count > 0) {
                string originalName = item.Name;
                string newName = originalName;
                if (Prefixes.Count > 0) newName = $"{Prefixes[0].Name} {newName}";
                if (Suffixes.Count > 0) newName = $"{newName} {Suffixes[0].Name}";

                Color tierColor = CurrentTier == ItemTier.Rare ? new Color(255, 255, 102) : new Color(136, 136, 255);
                
                var nameLine = tooltips.FirstOrDefault(l => l.Name == "ItemName" && l.Mod == "Terraria");
                if (nameLine != null) { nameLine.Text = newName; nameLine.OverrideColor = tierColor; }

                var defenseLine = tooltips.FirstOrDefault(l => l.Name == "Defense" && l.Mod == "Terraria");
                if (defenseLine != null && item.defense > 0) {
                    int bonusDef = 0;
                    foreach (var aff in Prefixes.Concat(Suffixes)) bonusDef += aff.CurrentDefense;
                    if (bonusDef != 0) {
                        int totalDef = item.defense + bonusDef;
                        defenseLine.Text = $"{totalDef} defense";
                    }
                }

                foreach (var aff in Prefixes.Concat(Suffixes)) {
                    string text = $"[{aff.Type}] {aff.Name}: ";
                    if (aff.CurrentDamage != 0) text += $"{(aff.CurrentDamage * 100):+0;-0}% {aff.StatClass} Damage. ";
                    if (aff.CurrentSpeed != 0) text += $"{(aff.CurrentSpeed * 100):+0;-0}% Speed. ";
                    if (aff.CurrentCrit != 0) text += $"{aff.CurrentCrit:+0;-0}% {aff.StatClass} Crit. ";
                    if (aff.CurrentDefense != 0) text += $"{aff.CurrentDefense:+0;-0} Defense. ";
                    if (aff.CurrentMoveSpeed != 0) text += $"{(aff.CurrentMoveSpeed * 100):+0;-0}% Move Speed. ";
                    
                    if (aff.CurrentMana != 0) text += $"{aff.CurrentMana:+0;-0} Max Mana. ";
                    if (aff.CurrentManaRegen != 0) text += $"{aff.CurrentManaRegen:+0;-0} Mana Regen. ";
                    if (aff.CurrentJump != 0) text += $"{(aff.CurrentJump * 100):+0;-0}% Jump Height. ";
                    if (aff.CurrentLife != 0) text += $"{aff.CurrentLife:+0;-0} Max Life. ";
                    if (aff.CurrentLifeRegen != 0) text += $"{aff.CurrentLifeRegen:+0;-0} Life Regen. ";
                    
                    if (aff.CurrentEnergyShield != 0) text += $"{aff.CurrentEnergyShield:+0;-0} Max Energy Shield. ";
                    if (aff.CurrentWard != 0) text += $"{aff.CurrentWard:+0;-0} Max Spirit Ward. ";
                    if (aff.CurrentEvasion != 0) text += $"{(aff.CurrentEvasion * 100):+0;-0}% Evasion Chance. ";
                    if (aff.CurrentEndurance != 0) text += $"{(aff.CurrentEndurance * 100):+0;-0}% Damage Reduction. ";
                    if (aff.CurrentArmorPen != 0) text += $"{aff.CurrentArmorPen:+0;-0} Armor Penetration. ";
                    if (aff.CurrentMinions != 0) text += $"{aff.CurrentMinions:+0;-0} Max Minions. ";
                    
                    tooltips.Add(new TooltipLine(Mod, $"Atlas{aff.Name}", text) { OverrideColor = new Color(170, 170, 255) });
                }
            }

            if (IsValidForAffixes(item) && item.damage > 0 && !item.accessory) {
                Player player = Main.LocalPlayer;
                float classSpeed = player.GetAttackSpeed(item.DamageType);
                float customSpeed = UseSpeedMultiplier(item, player); 
                float totalSpeed = classSpeed * customSpeed;
                                
                int finalUseTime = (int)System.Math.Max(1, item.useTime / totalSpeed);
                float aps = 60f / finalUseTime;
                var speedLine = tooltips.Find(l => l.Name == "Speed" && l.Mod == "Terraria");
                if (speedLine != null) {
                    speedLine.Text += $"  [{aps:0.00} Attacks/Sec | {finalUseTime} Frames]";
                    speedLine.OverrideColor = new Color(210, 210, 210);
                }
            }
        }

        // --- SAVING & LOADING ---
        public override void SaveData(Item item, TagCompound tag) {
            tag["CurrentTier"] = (int)CurrentTier;
            // Filter out any nulls before saving
            tag["Prefixes"] = Prefixes.Where(p => p != null).Select(p => SerializeAffix(p)).ToList();
            tag["Suffixes"] = Suffixes.Where(s => s != null).Select(s => SerializeAffix(s)).ToList();
        }

        public override void LoadData(Item item, TagCompound tag) {
            if (tag.ContainsKey("CurrentTier")) CurrentTier = (ItemTier)tag.GetInt("CurrentTier");
            // Automatically strip out any legacy/broken affixes that fail to deserialize
            if (tag.ContainsKey("Prefixes")) Prefixes = tag.GetList<TagCompound>("Prefixes").Select(DeserializeAffix).Where(a => a != null).ToList();
            if (tag.ContainsKey("Suffixes")) Suffixes = tag.GetList<TagCompound>("Suffixes").Select(DeserializeAffix).Where(a => a != null).ToList();
        }

        private TagCompound SerializeAffix(Affix aff) => new TagCompound {
            ["Name"] = aff.Name, ["Damage"] = aff.CurrentDamage, ["Speed"] = aff.CurrentSpeed, 
            ["Crit"] = aff.CurrentCrit, ["Defense"] = aff.CurrentDefense, ["MoveSpeed"] = aff.CurrentMoveSpeed,
            ["StatClass"] = aff.StatClass,
            ["Mana"] = aff.CurrentMana, ["ManaRegen"] = aff.CurrentManaRegen,
            ["Jump"] = aff.CurrentJump, ["Life"] = aff.CurrentLife, ["LifeRegen"] = aff.CurrentLifeRegen,
            ["EnergyShield"] = aff.CurrentEnergyShield, ["Ward"] = aff.CurrentWard, ["Evasion"] = aff.CurrentEvasion,
            ["Endurance"] = aff.CurrentEndurance, ["ArmorPen"] = aff.CurrentArmorPen, ["Minions"] = aff.CurrentMinions
        };

        private Affix DeserializeAffix(TagCompound tag) {
            string name = tag.GetString("Name");
            var template = WeaponPrefixes.Concat(WeaponSuffixes).Concat(ArmorPrefixes).Concat(ArmorSuffixes).Concat(AccessoryPrefixes).Concat(AccessorySuffixes).FirstOrDefault(a => a.Name == name);
            if (template == null) return null;
            
            var aff = (Affix)template.Clone();
            aff.CurrentDamage = tag.GetFloat("Damage");
            aff.CurrentSpeed = tag.GetFloat("Speed");
            aff.CurrentCrit = tag.GetInt("Crit");
            aff.CurrentDefense = tag.GetInt("Defense");
            aff.CurrentMoveSpeed = tag.GetFloat("MoveSpeed");
            aff.StatClass = tag.ContainsKey("StatClass") ? tag.GetString("StatClass") : "Generic";
            
            aff.CurrentMana = tag.GetInt("Mana");
            aff.CurrentManaRegen = tag.GetInt("ManaRegen");
            aff.CurrentJump = tag.GetFloat("Jump");
            aff.CurrentLife = tag.GetInt("Life");
            aff.CurrentLifeRegen = tag.GetInt("LifeRegen");
            
            aff.CurrentEnergyShield = tag.GetInt("EnergyShield");
            aff.CurrentWard = tag.GetInt("Ward");
            aff.CurrentEvasion = tag.GetFloat("Evasion");
            aff.CurrentEndurance = tag.GetFloat("Endurance");
            aff.CurrentArmorPen = tag.GetInt("ArmorPen");
            aff.CurrentMinions = tag.GetInt("Minions");
            
            return aff;
        }

        public override void OnSpawn(Item item, IEntitySource source) {
            if (IsValidForAffixes(item) && CurrentTier != ItemTier.Unique) {
                if (source is EntitySource_Loot || source is EntitySource_Parent || source is EntitySource_ItemOpen) {
                    int roll = Main.rand.Next(100);
                    
                    if (roll < 10) {
                        CurrentTier = ItemTier.Rare;
                        var prePool = GetPool(item, "Prefix");
                        var sufPool = GetPool(item, "Suffix");
                        
                        int preCount = Main.rand.Next(1, 4);
                        int sufCount = Main.rand.Next(1, 4);
                        
                        for (int i = 0; i < preCount; i++) {
                            var available = prePool.Where(p => !Prefixes.Any(existing => existing.Name == p.Name)).ToList();
                            if (available.Count > 0) Prefixes.Add(available[Main.rand.Next(available.Count)].CloneAndRoll());
                        }
                        for (int i = 0; i < sufCount; i++) {
                            var available = sufPool.Where(s => !Suffixes.Any(existing => existing.Name == s.Name)).ToList();
                            if (available.Count > 0) Suffixes.Add(available[Main.rand.Next(available.Count)].CloneAndRoll());
                        }
                    } 
                    else if (roll < 50) {
                        CurrentTier = ItemTier.Magic;
                        if (Main.rand.NextBool()) {
                            var pool = GetPool(item, "Prefix");
                            Prefixes.Add(pool[Main.rand.Next(pool.Count)].CloneAndRoll());
                        } else {
                            var pool = GetPool(item, "Suffix");
                            Suffixes.Add(pool[Main.rand.Next(pool.Count)].CloneAndRoll());
                        }
                    }
                }
            }
        }
    }
}