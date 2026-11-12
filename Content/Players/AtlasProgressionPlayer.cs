using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WarpedAtlas.Content.Systems;
using WarpedAtlas.Content.Items;
using WarpedAtlas.Content.Buffs;
using WarpedAtlas.Content.UI;
using WarpedAtlas.Content.Items.Ascendancies;
using WarpedAtlas.Content.Projectiles;

namespace WarpedAtlas.Content.Players
{
    public class AtlasProgressionPlayer : ModPlayer
    {
        public HashSet<string> UnlockedProfiles = new HashSet<string>() { "DungeonProfile" };
        public HashSet<string> CompletedProfiles = new HashSet<string>();

        public string ChosenClass = "";
        public string ChosenAscendancy = "";

        public bool HasOnboarded = false;
        public int Level = 1;
        public long CurrentExp = 0;
        public long ExpToNextLevel => Level * 1000;

        public HashSet<int> AllocatedNodeIds = new HashSet<int>();
        public int TotalPassivePoints = 3;

        public Item[] StashItems = new Item[100];
        public Item[] AscendancyLoadout = new Item[4];

        // --- SUB-ARCHETYPE BONUS TRACKERS ---
        public float BonusYoyoDamage = 0f;
        public float BonusBowDamage = 0f;
        public float BonusGunDamage = 0f;
        public float BonusTomeDamage = 0f;
        public float BonusWhipDamage = 0f;

        // --- EXTRACTION TOOLKIT PASSIVE BRANCH TRACKERS (DIR 6) ---
        public float ToolkitDamageMult = 1f;
        public bool Toolkit_WarcryCDR = false;
        public bool Toolkit_ExtraTraps = false;
        public bool Toolkit_TrapCluster = false;
        public bool Toolkit_ConduitOvercharge = false;
        public bool Toolkit_IdolWardLeech = false;

        // --- ARPG DEFENSIVE RESOURCES ---
        public int MaxEnergyShield = 0;
        public float CurrentEnergyShield = 0;
        public int EnergyShieldRechargeDelay = 0;
        public float EvasionChance = 0f;
        public int MaxWard = 0;
        public float CurrentWard = 0;
        public int WarcryPowerStacks = 0;

        // --- ASCENDANCY NORTH ULTIMATE FLAGS ---
        public bool Asc_FireTrapper = false;
        public bool Asc_TimeRanger = false;
        public bool Asc_FlameSorcerer = false;
        public bool Asc_LightningSorcerer = false;
        public bool Asc_PlantSummoner = false;
        public bool Asc_LightningSummoner = false;

        // --- ASCENDANCY EAST OFFENSE FLAGS ---
        public bool Asc_WindOffense = false;
        public bool Asc_FireRangerOffense = false;
        public bool Asc_TimeRangerOffense = false;
        public bool Asc_FlameSorcererOffense = false;
        public bool Asc_LightningSorcererOffense = false;
        public bool Asc_PlantSummonerOffense = false;
        public bool Asc_LightningSummonerOffense = false;

        // --- INTERNAL COOLDOWNS & COUNTERS ---
        public bool IsPaladinSlamming = false;
        public int PaladinSlamCooldown = 0;
        public int PaladinAnimFrame = 0;
        public int PaladinAnimCounter = 0;
        public int TornadoPulseTimer = 0;
        public int LightningOrbCooldown = 0;
        public int TlalocArcCooldown = 0;
        public int WindWhirlwindCooldown = 0;
        public int FireRangerHitCounter = 0;
        public int EastProcCooldown = 0;

        // --- WARRIOR KEYSTONE FLAGS ---
        public bool KS_CentrifugalForce = false;
        public bool KS_Juggernaut = false;
        public bool KS_Flurry = false;
        public bool KS_Berserker = false;
        public bool KS_HeavyBlade = false;
        public bool KS_TitansGrip = false;
        public bool KS_BloodPact = false;
        public bool KS_AegisWall = false;
        public bool KS_Gladiator = false;
        public bool KS_WarlordsReach = false;
        public bool KS_RetaliationMatrix = false;
        public bool KS_Inertia = false;

        // --- RANGER KEYSTONE FLAGS ---
        public bool KS_FletchersPride = false;
        public bool KS_EndlessQuiver = false;
        public bool KS_Deadeye = false;
        public bool KS_TriggerHappy = false;
        public bool KS_Demolitionist = false;
        public bool KS_SiegeEngineer = false;
        public bool KS_Acrobat = false;
        public bool KS_SnipersVeil = false;
        public bool KS_Entrenched = false;

        // --- SORCERER KEYSTONE FLAGS ---
        public bool KS_Librarian = false;
        public bool KS_GlassCannon = false;
        public bool KS_ArcaneBattery = false;
        public bool KS_Archmage = false;
        public bool KS_Channeler = false;
        public bool KS_TimeWarp = false;
        public bool KS_ManaShield = false;
        public bool KS_Elementalist = false;
        public bool KS_MindOverMatter = false;
        public bool KS_Overload = false;

        // --- SUMMONER KEYSTONE FLAGS ---
        public bool KS_TheHive = false;
        public bool KS_FeralStampede = false;
        public bool KS_Whipmaster = false;
        public bool KS_Puppeteer = false;
        public bool KS_LoneSovereign = false;
        public bool KS_SentryCommander = false;
        public bool KS_Frontline = false;
        public bool KS_SoulLink = false;
        public bool KS_TheSingularity = false;
        public bool KS_SymbioticLink = false;

        public bool Lever_TlalocsTorrent = false;
        public bool Lever_ObsidianVeins = false;
        public bool Lever_CelestialVoid = false;

        // Campaign Progression Tracker
        public int HighestUnlockedAct = 1;
        public int HighestUnlockedZone = 1;
        public int ActiveRunAct = 1;
        public int ActiveRunZone = 1;

        public List<string> UnlockedWaypoints = new List<string>();

        public override void Initialize() {
            EnsureArraysInitialized();
        }

        private void EnsureArraysInitialized() {
            if (AscendancyLoadout == null || AscendancyLoadout.Length < 4) {
                AscendancyLoadout = new Item[4];
            }
            for (int i = 0; i < 4; i++) {
                if (AscendancyLoadout[i] == null) {
                    AscendancyLoadout[i] = new Item();
                    AscendancyLoadout[i].TurnToAir();
                }
            }

            if (StashItems == null || StashItems.Length < 100) {
                StashItems = new Item[100];
            }
            for (int i = 0; i < 100; i++) {
                if (StashItems[i] == null) {
                    StashItems[i] = new Item();
                    StashItems[i].TurnToAir();
                }
            }

            if (HighestUnlockedAct < 1) HighestUnlockedAct = 1;
            if (HighestUnlockedZone < 1) HighestUnlockedZone = 1;
        }

        public void CompleteZone(int completedAct, int completedZone) {
            EnsureArraysInitialized();

            bool advanced = false;
            if (completedAct == HighestUnlockedAct && completedZone == HighestUnlockedZone) {
                if (completedZone < 4) {
                    HighestUnlockedZone = completedZone + 1;
                    Main.NewText($"  [Tonalpohualli] Act {HighestUnlockedAct} Zone {HighestUnlockedZone} Unlocked!", 255, 215, 0);
                } else {
                    if (HighestUnlockedAct < 8) {
                        HighestUnlockedAct++;
                        HighestUnlockedZone = 1;
                        Main.NewText($"  [Tonalpohualli] A Rift opens! Act {HighestUnlockedAct} has been charted!", 255, 215, 0);
                    } else {
                        HighestUnlockedZone = 5;
                        Main.NewText("  [Tonalpohualli] All Eight Suns have been conquered!", 255, 215, 0);
                    }
                }
                advanced = true;
            } else if (completedAct > HighestUnlockedAct || (completedAct == HighestUnlockedAct && completedZone > HighestUnlockedZone)) {
                if (completedZone < 4) {
                    HighestUnlockedAct = completedAct;
                    HighestUnlockedZone = completedZone + 1;
                } else {
                    HighestUnlockedAct = Math.Min(8, completedAct + 1);
                    HighestUnlockedZone = 1;
                }
                advanced = true;
            }

            if (advanced && !Main.dedServ) {
                var uiSys = ModContent.GetInstance<AtlasUISystem>();
                uiSys?.MapUIState?.BuildActNodes();
            }
        }

        public bool HasFrigidGlimmerInUltimateSlot() {
            EnsureArraysInitialized();
            Item ultItem = AscendancyLoadout[0];
            return ultItem != null && !ultItem.IsAir && ultItem.type == ModContent.ItemType<FrigidGlimmer>();
        }

        public bool HasEhecatlGaleInUltimateSlot() {
            EnsureArraysInitialized();
            Item ultItem = AscendancyLoadout[0];
            return ultItem != null && !ultItem.IsAir && ultItem.type == ModContent.ItemType<EhecatlGaleJewel>();
        }

        public override void OnEnterWorld() {
            EnsureArraysInitialized();
            Player.statLife = Player.statLifeMax2;
            Player.statMana = Player.statManaMax2;
            CurrentEnergyShield = MaxEnergyShield;
            CurrentWard = MaxWard;

            var uiSystem = ModContent.GetInstance<AtlasUISystem>();
            if (uiSystem != null && !Main.dedServ) {
                uiSystem.MapUIState?.BuildActNodes();
                if (!HasOnboarded) {
                    for (int i = 0; i < Player.inventory.Length; i++) Player.inventory[i].TurnToAir();
                    uiSystem.ShowIntroMenu();
                }
            }
        }

        public void AddExp(long amount) {
            CurrentExp += amount;
            bool leveledUp = false;

            while (CurrentExp >= ExpToNextLevel) {
                CurrentExp -= ExpToNextLevel;
                Level++;
                TotalPassivePoints += 2;
                leveledUp = true;
            }

            if (leveledUp && Player.whoAmI == Main.myPlayer) {
                SoundEngine.PlaySound(SoundID.Roar, Player.Center);
                Main.NewText($"[c/00ffcc:LEVEL UP!] You reached Level {Level}! (+2 Passive Points)", 0, 255, 200);
            }
        }

        public override void ResetEffects() {
            BonusYoyoDamage = BonusBowDamage = BonusGunDamage = BonusTomeDamage = BonusWhipDamage = 0f;
            ToolkitDamageMult = 1f;
            Toolkit_WarcryCDR = Toolkit_ExtraTraps = Toolkit_TrapCluster = Toolkit_ConduitOvercharge = Toolkit_IdolWardLeech = false;

            MaxEnergyShield = 0;
            EvasionChance = 0f;
            MaxWard = 0;

            Asc_FireTrapper = Asc_TimeRanger = Asc_FlameSorcerer = Asc_LightningSorcerer = Asc_PlantSummoner = Asc_LightningSummoner = false;
            Asc_WindOffense = Asc_FireRangerOffense = Asc_TimeRangerOffense = Asc_FlameSorcererOffense = Asc_LightningSorcererOffense = Asc_PlantSummonerOffense = Asc_LightningSummonerOffense = false;

            if (PaladinSlamCooldown > 0) PaladinSlamCooldown--;
            if (LightningOrbCooldown > 0) LightningOrbCooldown--;
            if (TlalocArcCooldown > 0) TlalocArcCooldown--;
            if (WindWhirlwindCooldown > 0) WindWhirlwindCooldown--;
            if (EastProcCooldown > 0) EastProcCooldown--;

            KS_CentrifugalForce = KS_Juggernaut = KS_Flurry = KS_Berserker = false;
            KS_HeavyBlade = KS_TitansGrip = KS_BloodPact = KS_AegisWall = KS_Gladiator = KS_WarlordsReach = false;
            KS_RetaliationMatrix = KS_Inertia = false;

            KS_FletchersPride = KS_EndlessQuiver = KS_Deadeye = KS_TriggerHappy = false;
            KS_Demolitionist = KS_SiegeEngineer = KS_Acrobat = KS_SnipersVeil = false;
            KS_Entrenched = false;

            KS_Librarian = KS_GlassCannon = KS_ArcaneBattery = KS_Archmage = KS_Channeler = false;
            KS_TimeWarp = KS_ManaShield = KS_Elementalist = false;
            KS_MindOverMatter = KS_Overload = false;

            KS_TheHive = KS_FeralStampede = KS_Whipmaster = KS_Puppeteer = false;
            KS_LoneSovereign = KS_SentryCommander = KS_Frontline = KS_SoulLink = false;
            KS_TheSingularity = KS_SymbioticLink = false;
        }

        public override void PostUpdateEquips() {
            EnsureArraysInitialized();

            for (int i = 0; i < 4; i++) {
                if (AscendancyLoadout[i]?.ModItem is AscendancyJewelItem jewel) {
                    jewel.ApplyAscendancyPassive(Player, i);
                }
            }

            int rootId = ChosenClass switch {
                "Warrior" => 1000,
                "Ranger" => 2000,
                "Sorcerer" => 3000,
                "Summoner" => 4000,
                _ => 0
            };

            if (rootId != 0 && !AllocatedNodeIds.Contains(rootId)) {
                AllocatedNodeIds.Add(rootId);
                AllocatedNodeIds.Remove(0);
            }

            foreach (int id in AllocatedNodeIds) {
                if (PassiveTreeRegistry.Nodes.TryGetValue(id, out var node)) {
                    node.ApplyHealthAndDefense?.Invoke(Player);
                    node.ApplyOffensiveStats?.Invoke(Player);
                }
            }

            switch (ChosenClass) {
                case "Warrior":  Player.statDefense += 4;  Player.endurance += 0.02f;  break;
                case "Ranger":   Player.GetDamage(DamageClass.Ranged) += 0.05f;  EvasionChance += 0.05f;   break;
                case "Sorcerer": Player.statManaMax2 += 20;  MaxEnergyShield += 20;    break;
                case "Summoner": Player.maxMinions += 1;  MaxWard += 20;        break;
            }

            // --- WARRIOR KEYSTONES ---
            if (KS_Berserker) { Player.GetDamage(DamageClass.Melee) += 0.40f; Player.endurance -= 0.20f; }
            if (KS_Flurry) { Player.GetAttackSpeed(DamageClass.Melee) += 0.30f; Player.GetDamage(DamageClass.Melee) -= 0.15f; }
            if (KS_Juggernaut) { Player.statLifeMax2 += 100; Player.moveSpeed -= 0.25f; }
            if (KS_CentrifugalForce) { BonusYoyoDamage += 0.20f; }
            if (KS_HeavyBlade) { Player.GetDamage(DamageClass.Melee) += 0.50f; Player.moveSpeed -= 0.20f; }
            if (KS_TitansGrip) { Player.GetAttackSpeed(DamageClass.Melee) -= 0.15f; Player.noKnockback = true; }
            if (KS_BloodPact) { Player.statLifeMax2 = (int)(Player.statLifeMax2 * 1.50f); Player.lifeRegen += 5; }
            if (KS_AegisWall) { Player.moveSpeed -= 0.30f; Player.statDefense += 20; }
            if (KS_Gladiator) { Player.GetCritChance(DamageClass.Melee) += 20f; Player.endurance -= 0.10f; }
            if (KS_WarlordsReach) { Player.GetDamage(DamageClass.Melee) += 0.30f; Player.statDefense -= 10; }
            if (KS_RetaliationMatrix) { Player.GetDamage(DamageClass.Melee) += (Player.statDefense * 0.01f); }
            if (KS_Inertia) { Player.GetDamage(DamageClass.Melee) += (Math.Abs(Player.velocity.X) * 0.05f); }

            // --- RANGER KEYSTONES ---
            if (KS_FletchersPride) { BonusBowDamage += 0.30f; }
            if (KS_EndlessQuiver) { Player.moveSpeed -= 0.15f; }
            if (KS_Deadeye) { Player.GetCritChance(DamageClass.Ranged) += 15f; }
            if (KS_TriggerHappy) { Player.GetAttackSpeed(DamageClass.Ranged) += 0.40f; Player.GetDamage(DamageClass.Ranged) -= 0.20f; }
            if (KS_Demolitionist) { Player.GetDamage(DamageClass.Ranged) += 0.40f; Player.endurance -= 0.15f; }
            if (KS_SiegeEngineer) { Player.noKnockback = true; }
            if (KS_Acrobat) { Player.moveSpeed += 0.25f; Player.statDefense -= 10; EvasionChance += 0.15f; }
            if (KS_SnipersVeil) { Player.GetCritChance(DamageClass.Ranged) += 25f; Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.80f); }
            if (KS_Entrenched && Player.velocity == Vector2.Zero) { Player.GetArmorPenetration(DamageClass.Generic) += 20f; Player.GetDamage(DamageClass.Ranged) += 0.50f; }

            // --- SORCERER KEYSTONES ---
            if (KS_GlassCannon) { Player.GetDamage(DamageClass.Magic) += 0.40f; Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.50f); }
            if (KS_ArcaneBattery) { Player.statManaMax2 += 100; Player.GetDamage(DamageClass.Magic) += 0.20f; Player.statDefense -= 10; }
            if (KS_Archmage) { Player.GetDamage(DamageClass.Magic) += (Player.statManaMax2 / 10) * 0.01f; }
            if (KS_Channeler) { Player.GetAttackSpeed(DamageClass.Magic) += 0.30f; }
            if (KS_TimeWarp) { Player.GetAttackSpeed(DamageClass.Magic) += 0.30f; }
            if (KS_ManaShield) { MaxEnergyShield += Player.statManaMax2; }

            // --- SUMMONER KEYSTONES ---
            if (KS_TheHive) { Player.maxMinions += 3; Player.GetDamage(DamageClass.Summon) -= 0.30f; }
            if (KS_FeralStampede) { Player.moveSpeed -= 0.20f; Player.GetCritChance(DamageClass.Summon) += 20f; }
            if (KS_Whipmaster) { BonusWhipDamage += 0.30f; Player.GetDamage(DamageClass.Summon) -= 0.20f; }
            if (KS_Puppeteer) { Player.GetDamage(DamageClass.Summon) += 0.20f; }
            if (KS_LoneSovereign) { Player.maxMinions = 1; Player.GetDamage(DamageClass.Summon) += 1.00f; }
            if (KS_SentryCommander) { Player.maxTurrets += 3; Player.maxMinions = 0; }
            if (KS_Frontline) { Player.statDefense += Player.maxMinions * 3; Player.moveSpeed -= 0.20f; }
            if (KS_SoulLink) { Player.endurance += 0.15f; Player.GetDamage(DamageClass.Summon) -= 0.10f; }
            if (KS_TheSingularity) { Player.GetDamage(DamageClass.Summon) += (Player.maxMinions - 1) * 0.50f; Player.maxMinions = 1; }

            // ==========================================
            // ASCENDANCY TRANSFORMATIONS & PYRE MECHANICS
            // ==========================================
            if (Player.HasBuff(ModContent.BuffType<FrigidGlimmerBuff>())) {
                UpdateIcePaladinTransformation();
            } else {
                IsPaladinSlamming = false;
            }

            if (Player.HasBuff(ModContent.BuffType<EhecatlVortexBuff>())) {
                UpdateEhecatlTornado();
            }

            if (Asc_FlameSorcerer) {
                UpdateXiuhcoatlPyre();
            }

            // WIND WARRIOR EAST OFFENSE: Launch Tome of Infinite Wisdom Tornado + Weather Pain Gale on Melee Swing
            if (Asc_WindOffense && Player.itemAnimation > 0 && WindWhirlwindCooldown <= 0 && Player.whoAmI == Main.myPlayer) {
                Item held = Player.HeldItem;
                if (held != null && !held.IsAir && held.DamageType == DamageClass.Melee) {
                    WindWhirlwindCooldown = 32;
                    SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, Player.Center);
                    Vector2 shootDir = Vector2.Normalize(Main.MouseWorld - Player.Center);
                    if (shootDir.HasNaNs()) shootDir = new Vector2(Player.direction, 0f);
                    int tornadoDmg = (int)Player.GetDamage(DamageClass.Melee).ApplyTo(Math.Max(42, held.damage));

                    // 1. Spawn the full animated Tome of Infinite Wisdom + Sand Elemental Cyclone
                    Projectile.NewProjectile(
                        Player.GetSource_ItemUse(held),
                        Player.Center + new Vector2(Player.direction * 28f, -10f),
                        shootDir * 6.5f,
                        ModContent.ProjectileType<EhecatlTempestCycloneProj>(),
                        tornadoDmg,
                        4f,
                        Player.whoAmI
                    );

                    // 2. Spawn the initial Tome of Infinite Wisdom wind bolt
                    int bolt = Projectile.NewProjectile(
                        Player.GetSource_ItemUse(held),
                        Player.Center,
                        shootDir * 11f,
                        ProjectileID.BookStaffShot,
                        tornadoDmg,
                        4f,
                        Player.whoAmI
                    );
                    if (bolt >= 0 && bolt < Main.maxProjectiles) {
                        Main.projectile[bolt].DamageType = DamageClass.Melee;
                    }
                }
            }

            // ==========================================
            // CAPPING AND REGENERATION 
            // ==========================================
            if (CurrentEnergyShield > MaxEnergyShield) CurrentEnergyShield = MaxEnergyShield;
            if (CurrentWard > MaxWard) CurrentWard = MaxWard;

            if (EnergyShieldRechargeDelay > 0) {
                EnergyShieldRechargeDelay--;
            } else if (MaxEnergyShield > 0 && CurrentEnergyShield < MaxEnergyShield) {
                CurrentEnergyShield += MaxEnergyShield * 0.012f;
                if (CurrentEnergyShield > MaxEnergyShield) CurrentEnergyShield = MaxEnergyShield;
            }

            if (EvasionChance > 0.75f) EvasionChance = 0.75f;
            if (Player.moveSpeed < 0.15f) Player.moveSpeed = 0.15f;
        }

        private void UpdateEhecatlTornado() {
            bool groundNearby = Collision.SolidCollision(Player.BottomLeft + new Vector2(0, 4f), Player.width, 36);
            if (Player.controlUp || Player.controlJump) {
                Player.velocity.Y = MathHelper.Lerp(Player.velocity.Y, -4.2f, 0.15f);
            } else if (!Player.controlDown && groundNearby && Player.velocity.Y >= 0f) {
                float hoverVel = (float)Math.Sin(Main.GameUpdateCount * 0.12f) * 0.6f - 0.35f;
                Player.velocity.Y = hoverVel;
            } else if (!Player.controlDown && Player.velocity.Y > 2.5f) {
                Player.velocity.Y = 2.5f;
            }

            for (int i = 0; i < 3; i++) {
                float angle = Main.GameUpdateCount * 0.25f + i * MathHelper.TwoPi / 3f;
                Vector2 offset = new Vector2((float)Math.Cos(angle) * 32f, Main.rand.NextFloat(-34f, 28f));
                Dust d = Dust.NewDustPerfect(Player.Center + offset, DustID.Sandnado, new Vector2(-offset.Y * 0.12f, -2f), 100, new Color(130, 255, 200), 1.45f);
                d.noGravity = true;
            }

            TornadoPulseTimer++;
            // Launch homing Weather Pain miniature hurricanes every 45 ticks while in Tornado Form
            if (TornadoPulseTimer % 45 == 0 && Player.whoAmI == Main.myPlayer) {
                SoundEngine.PlaySound(SoundID.Item43, Player.Center);
                int galeDmg = (int)Player.GetDamage(DamageClass.Melee).ApplyTo(50 + Player.statDefense);
                int p = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(Player.direction * 6f, -2f), ProjectileID.WeatherPainShot, galeDmg, 3f, Player.whoAmI);
                if (p >= 0 && p < Main.maxProjectiles) {
                    Main.projectile[p].DamageType = DamageClass.Melee;
                }
            }

            if (TornadoPulseTimer >= 15) {
                int dmg = (int)Player.GetDamage(DamageClass.Melee).ApplyTo(45 + Player.statDefense);
                for (int i = 0; i < Main.maxNPCs; i++) {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage) {
                        float dist = Vector2.Distance(npc.Center, Player.Center);
                        if (dist < 155f && Collision.CanHitLine(Player.position, Player.width, Player.height, npc.position, npc.width, npc.height)) {
                            if (!npc.boss && npc.knockBackResist > 0f) {
                                npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Normalize(Player.Center - npc.Center) * 4.5f, 0.25f);
                            }
                            if (Player.whoAmI == Main.myPlayer && TornadoPulseTimer % 15 == 0) {
                                Player.ApplyDamageToNPC(npc, dmg, 1.5f, npc.Center.X < Player.Center.X ? -1 : 1, crit: Main.rand.Next(100) < Player.GetCritChance(DamageClass.Melee));
                            }
                        }
                    }
                }
            }
        }

        private void UpdateXiuhcoatlPyre() {
            int manaSickIdx = Player.FindBuffIndex(BuffID.ManaSickness);
            if (manaSickIdx != -1) {
                int duration = Math.Max(300, Player.buffTime[manaSickIdx]);
                Player.DelBuff(manaSickIdx);
                Player.AddBuff(ModContent.BuffType<XiuhcoatlPyreDebuff>(), duration);
            }

            if (Player.HasBuff(ModContent.BuffType<XiuhcoatlPyreDebuff>())) {
                EnergyShieldRechargeDelay = Math.Max(EnergyShieldRechargeDelay, 30);
                float drainPerFrame = 0.20f;
                if (CurrentEnergyShield > 0f) {
                    CurrentEnergyShield = Math.Max(0f, CurrentEnergyShield - drainPerFrame);
                } else if (Main.GameUpdateCount % 15 == 0 && Player.statLife > 5) {
                    Player.statLife -= 3;
                    CombatText.NewText(Player.getRect(), Color.OrangeRed, 3, dramatic: false, dot: true);
                }
            }
        }

        private void UpdateIcePaladinTransformation() {
            PaladinAnimCounter++;
            int totalFrames = Main.npcFrameCount[NPCID.Paladin];
            if (Player.itemAnimation > 0 || IsPaladinSlamming) {
                if (PaladinAnimCounter >= 4) {
                    PaladinAnimCounter = 0;
                    PaladinAnimFrame = (PaladinAnimFrame + 1) % totalFrames;
                }
            } else if (Math.Abs(Player.velocity.X) > 0.5f) {
                if (PaladinAnimCounter >= 6) {
                    PaladinAnimCounter = 0;
                    PaladinAnimFrame = (PaladinAnimFrame + 1) % totalFrames;
                }
            } else {
                PaladinAnimFrame = 0;
            }

            bool onGround = Player.velocity.Y == 0f;
            if (!onGround && Player.controlDown && !IsPaladinSlamming && PaladinSlamCooldown <= 0) {
                IsPaladinSlamming = true;
                Player.velocity.Y = 22f;
                Player.velocity.X *= 0.3f;
                Player.maxFallSpeed = 28f;
                SoundEngine.PlaySound(SoundID.Item1, Player.Center);
            }

            if (IsPaladinSlamming) {
                Player.velocity.Y = Math.Max(Player.velocity.Y, 22f);
                Player.maxFallSpeed = 28f;
                Player.noFallDmg = true;

                for (int i = 0; i < 3; i++) {
                    Dust d = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.IceTorch, 0f, -4f, 100, Color.Cyan, 2.2f);
                    d.noGravity = true;
                }

                if (onGround) {
                    IsPaladinSlamming = false;
                    PaladinSlamCooldown = 35;
                    TriggerPaladinIcicleCrash(isHeavySlam: true);
                }
            }

            if (onGround && Player.itemAnimation > 0 && PaladinSlamCooldown <= 0) {
                PaladinSlamCooldown = 32;
                TriggerPaladinIcicleCrash(isHeavySlam: false);
            }
        }

        private void TriggerPaladinIcicleCrash(bool isHeavySlam) {
            if (Player.whoAmI != Main.myPlayer) return;

            SoundEngine.PlaySound(SoundID.Item14, Player.Bottom);
            SoundEngine.PlaySound(SoundID.DeerclopsIceAttack, Player.Bottom);

            for (int i = 0; i < (isHeavySlam ? 50 : 25); i++) {
                Vector2 vel = new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-6f, 1f));
                Dust d = Dust.NewDustDirect(Player.Bottom - new Vector2(24, 8), 48, 16, DustID.IceRod, vel.X, vel.Y, 80, Color.DeepSkyBlue, 2.0f);
                d.noGravity = true;
            }

            int baseDmg = (int)(Player.GetDamage(DamageClass.Melee).ApplyTo(isHeavySlam ? 95 : 60)) + (Player.statDefense * 2);
            int spikeCount = isHeavySlam ? 8 : 5;
            int[] dirs = isHeavySlam ? new int[] { -1, 1 } : new int[] { Player.direction };

            foreach (int dir in dirs) {
                for (int step = 1; step <= spikeCount; step++) {
                    Vector2 spawnPos = Player.Bottom + new Vector2(dir * step * 28f, -4f);

                    Point tileCoord = spawnPos.ToTileCoordinates();
                    for (int yOff = -3; yOff <= 5; yOff++) {
                        if (WorldGen.InWorld(tileCoord.X, tileCoord.Y + yOff)) {
                            Tile t = Main.tile[tileCoord.X, tileCoord.Y + yOff];
                            if (t.HasTile && (Main.tileSolid[t.TileType] || Main.tileSolidTop[t.TileType])) {
                                spawnPos.Y = (tileCoord.Y + yOff) * 16f;
                                break;
                            }
                        }
                    }

                    float scale = 0.75f + (step * 0.12f) + (isHeavySlam ? 0.25f : 0f);
                    int pIdx = Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        spawnPos,
                        new Vector2(0f, -0.01f),
                        ProjectileID.DeerclopsIceSpike,
                        baseDmg,
                        6f,
                        Player.whoAmI,
                        ai0: -step * 3f,
                        ai1: scale
                    );

                    if (pIdx >= 0 && pIdx < Main.maxProjectiles) {
                        Main.projectile[pIdx].friendly = true;
                        Main.projectile[pIdx].hostile = false;
                        Main.projectile[pIdx].DamageType = DamageClass.Melee;
                        Main.projectile[pIdx].penetrate = -1;
                        Main.projectile[pIdx].usesLocalNPCImmunity = true;
                        Main.projectile[pIdx].localNPCHitCooldown = 20;
                    }
                }
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) {
            if (drawInfo.shadow != 0f) return;

            if (Player.HasBuff(ModContent.BuffType<FrigidGlimmerBuff>())) {
                drawInfo.hideEntirePlayer = true;

                Main.instance.LoadNPC(NPCID.Paladin);
                Texture2D paladinTex = TextureAssets.Npc[NPCID.Paladin].Value;
                int totalFrames = Main.npcFrameCount[NPCID.Paladin];
                int frameHeight = paladinTex.Height / totalFrames;
                Rectangle sourceRect = new Rectangle(0, (PaladinAnimFrame % totalFrames) * frameHeight, paladinTex.Width, frameHeight - 2);

                Vector2 drawPos = Player.Bottom - Main.screenPosition + new Vector2(0f, Player.gfxOffY - (frameHeight * 0.5f) + 6f);
                SpriteEffects effects = Player.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 origin = new Vector2(sourceRect.Width * 0.5f, sourceRect.Height * 0.5f);

                Main.spriteBatch.Draw(paladinTex, drawPos + new Vector2(2, 0), sourceRect, new Color(50, 200, 255, 120), Player.fullRotation, origin, 1.18f, effects, 0f);
                Main.spriteBatch.Draw(paladinTex, drawPos + new Vector2(-2, 0), sourceRect, new Color(50, 200, 255, 120), Player.fullRotation, origin, 1.18f, effects, 0f);
                Main.spriteBatch.Draw(paladinTex, drawPos, sourceRect, new Color(150, 230, 255), Player.fullRotation, origin, 1.12f, effects, 0f);
            }

            // PROPERLY SLICED SAND ELEMENTAL & APPRENTICE STORM TORNADO AROUND THE PLAYER
            if (Player.HasBuff(ModContent.BuffType<EhecatlVortexBuff>())) {
                Main.instance.LoadProjectile(ProjectileID.SandnadoFriendly);
                Main.instance.LoadProjectile(ProjectileID.DD2ApprenticeStorm);

                Texture2D sandTex = TextureAssets.Projectile[ProjectileID.SandnadoFriendly].Value;
                Texture2D stormTex = TextureAssets.Projectile[ProjectileID.DD2ApprenticeStorm].Value;

                int animFrame = (int)(Main.GameUpdateCount / 3) % 6;
                Rectangle sandFrame = sandTex.Frame(1, 6, 0, animFrame);
                Rectangle stormFrame = stormTex.Frame(1, 6, 0, animFrame);

                Vector2 drawPos = Player.Center - Main.screenPosition + new Vector2(0f, Player.gfxOffY);
                float sway = (float)Math.Sin(Main.GameUpdateCount * 0.15f) * 0.08f;

                // Outer Sand Elemental Cyclone (Emerald-Tinted)
                Main.spriteBatch.Draw(sandTex, drawPos, sandFrame, new Color(100, 255, 185, 160), sway, sandFrame.Size() * 0.5f, new Vector2(1.05f, 0.85f), SpriteEffects.None, 0f);
                // Inner Tome of Infinite Wisdom Whirlwind Core
                Main.spriteBatch.Draw(stormTex, drawPos, stormFrame, new Color(210, 255, 245, 200), -sway, stormFrame.Size() * 0.5f, new Vector2(0.90f, 0.78f), SpriteEffects.FlipHorizontally, 0f);
            }
        }

        public override bool FreeDodge(Player.HurtInfo info) {
            if (EvasionChance > 0f && Main.rand.NextFloat() < EvasionChance) {
                Player.SetImmuneTimeForAllTypes(45);
                for (int i = 0; i < 10; i++) Dust.NewDust(Player.position, Player.width, Player.height, DustID.Smoke);
                return true;
            }

            int totalShield = (int)(CurrentEnergyShield + CurrentWard);
            if (totalShield >= info.Damage) {
                AbsorbDamage(info.Damage);
                Player.SetImmuneTimeForAllTypes(30);
                EnergyShieldRechargeDelay = 180;
                SoundEngine.PlaySound(SoundID.Item43, Player.Center);
                return true;
            }

            return base.FreeDodge(info);
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers) {
            modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) => {
                if (KS_MindOverMatter && Player.statMana > 0) {
                    int manaDamage = (int)(info.Damage * 0.5f);
                    if (Player.statMana >= manaDamage) {
                        Player.statMana -= manaDamage;
                        info.Damage -= manaDamage;
                    } else {
                        info.Damage -= Player.statMana;
                        Player.statMana = 0;
                    }
                }

                int totalShield = (int)(CurrentEnergyShield + CurrentWard);
                if (totalShield > 0 && totalShield < info.Damage) {
                    info.Damage -= totalShield;
                    AbsorbDamage(totalShield);
                    EnergyShieldRechargeDelay = 180;
                    SoundEngine.PlaySound(SoundID.Item43, Player.Center);
                }
            };
        }

        private void AbsorbDamage(int damage) {
            if (CurrentWard >= damage) {
                CurrentWard -= damage;
            } else {
                damage -= (int)CurrentWard;
                CurrentWard = 0;
                CurrentEnergyShield -= damage;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
            bool isMinionOrSentry = proj.minion || ProjectileID.Sets.MinionShot[proj.type] || proj.sentry || ProjectileID.Sets.SentryShot[proj.type] || proj.DamageType == DamageClass.Summon;

            if (isMinionOrSentry) {
                if (MaxWard > 0 && CurrentWard < MaxWard) {
                    CurrentWard = Math.Min(MaxWard, CurrentWard + hit.Damage * 0.10f);
                }
                if (KS_SymbioticLink) {
                    CurrentWard = Math.Min(MaxWard, CurrentWard + hit.Damage * 0.25f);
                }

                // SUMMONER #2 NORTH (Tlaloc): Lightning Arc between Player and Critting Minion
                bool triggeredCrit = hit.Crit || (Asc_LightningSummoner && Main.rand.Next(100) < Player.GetCritChance(DamageClass.Summon));
                if (Asc_LightningSummoner && triggeredCrit && TlalocArcCooldown <= 0 && Player.whoAmI == Main.myPlayer) {
                    TlalocArcCooldown = 20;
                    SoundEngine.PlaySound(SoundID.Item93, proj.Center);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<TlalocArcVisualProj>(), 0, 0f, Player.whoAmI, proj.Center.X, proj.Center.Y);

                    int arcDmg = (int)Player.GetDamage(DamageClass.Summon).ApplyTo(55 + hit.Damage * 0.5f);
                    float collisionPoint = 0f;
                    for (int i = 0; i < Main.maxNPCs; i++) {
                        NPC npc = Main.npc[i];
                        if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage) {
                            if (Collision.CheckAABBvLineCollision(npc.position, npc.Size, Player.Center, proj.Center, 28f, ref collisionPoint)) {
                                Player.ApplyDamageToNPC(npc, arcDmg, 2f, 0, crit: true);
                            }
                        }
                    }
                }

                // SUMMONER #1 EAST (Xochiquetzal Briars): Erupt 3 Visible Animated Blood-Thorn Briar Spikes beneath enemy
                if (Asc_PlantSummonerOffense && EastProcCooldown <= 0 && proj.type != ModContent.ProjectileType<XochiquetzalBriarSpikeProj>() && Player.whoAmI == Main.myPlayer) {
                    EastProcCooldown = 24;
                    int briarDmg = (int)Player.GetDamage(DamageClass.Summon).ApplyTo(Math.Max(35, hit.Damage * 0.65f));
                    for (int b = -1; b <= 1; b++) {
                        Vector2 spawnPos = target.Bottom + new Vector2(b * 24f, 12f);
                        Projectile.NewProjectile(
                            Player.GetSource_FromThis(),
                            spawnPos,
                            new Vector2(0f, -2f),
                            ModContent.ProjectileType<XochiquetzalBriarSpikeProj>(),
                            briarDmg,
                            3f,
                            Player.whoAmI,
                            ai0: b * 0.18f
                        );
                    }
                }

                // SUMMONER #2 EAST (Tlaloc Storm Call): Call down vertical sky-to-ground thunderbolts
                if (Asc_LightningSummonerOffense && EastProcCooldown <= 0 && Player.whoAmI == Main.myPlayer) {
                    EastProcCooldown = 26;
                    Vector2 skyPos = target.Center + new Vector2(Main.rand.NextFloat(-40f, 40f), -260f);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), skyPos, Vector2.Zero, ModContent.ProjectileType<TlalocArcVisualProj>(), 0, 0f, Player.whoAmI, target.Center.X, target.Center.Y);
                    SoundEngine.PlaySound(SoundID.Item94, target.Center);
                    Player.ApplyDamageToNPC(target, (int)(hit.Damage * 0.55f), 1f, 0, crit: false);
                }
            }

            // RANGER #1 EAST (Xiuhtecuhtli Eruption): Every 3rd Ranged hit erupts a Volcanic Geyser + Solar Blast
            if (Asc_FireRangerOffense && proj.DamageType == DamageClass.Ranged && proj.type != ProjectileID.GeyserTrap && proj.type != ProjectileID.SolarWhipSwordExplosion && Player.whoAmI == Main.myPlayer) {
                target.AddBuff(BuffID.OnFire3, 180);
                FireRangerHitCounter++;
                if (FireRangerHitCounter >= 3) {
                    FireRangerHitCounter = 0;
                    SoundEngine.PlaySound(SoundID.Item14, target.Bottom);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileID.SolarWhipSwordExplosion, (int)(hit.Damage * 0.85f), 5f, Player.whoAmI, 0f, 0.95f);
                    int g = Projectile.NewProjectile(Player.GetSource_FromThis(), target.Bottom + new Vector2(0, 8f), new Vector2(0f, -8f), ProjectileID.GeyserTrap, (int)(hit.Damage * 0.9f), 4f, Player.whoAmI);
                    if (g >= 0 && g < Main.maxProjectiles) {
                        Main.projectile[g].friendly = true;
                        Main.projectile[g].hostile = false;
                        Main.projectile[g].DamageType = DamageClass.Ranged;
                    }
                }
            }

            // RANGER #2 EAST (Ollin Velocity): Ranged Crit grants Chrono-Surge + Temporal Piercer
            if (Asc_TimeRangerOffense && proj.DamageType == DamageClass.Ranged && hit.Crit && EastProcCooldown <= 0 && Player.whoAmI == Main.myPlayer) {
                EastProcCooldown = 25;
                Player.AddBuff(ModContent.BuffType<OllinChronoSurgeBuff>(), 180);
                Vector2 behindPlayer = Player.Center + new Vector2(-Player.direction * 40f, -20f);
                Vector2 aim = Vector2.Normalize(target.Center - behindPlayer) * 16f;
                int p = Projectile.NewProjectile(Player.GetSource_FromThis(), behindPlayer, aim, ProjectileID.HallowStar, (int)(hit.Damage * 0.8f), 3f, Player.whoAmI);
                if (p >= 0 && p < Main.maxProjectiles) {
                    Main.projectile[p].DamageType = DamageClass.Ranged;
                }
            }

            // SUMMONER #1 NORTH (Xochiquetzal): Sprout Life Fruit on kill or Boss Hit
            if (Asc_PlantSummoner && Player.whoAmI == Main.myPlayer) {
                bool killedTarget = target.life <= 0;
                if ((killedTarget && Main.rand.NextFloat() < 0.40f) || (target.boss && Main.rand.NextFloat() < 0.16f)) {
                    Vector2 popVel = new Vector2(Main.rand.NextFloat(-3.5f, 3.5f), -5.5f);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), target.Center, popVel, ModContent.ProjectileType<XochiquetzalFruitProj>(), 0, 0f, Player.whoAmI);
                }
            }

            // SORCERER #1 NORTH & EAST (Xiuhcoatl): Self-pyre + Solar Combustion on Burning Targets
            if (proj.DamageType == DamageClass.Magic) {
                if (Asc_FlameSorcererOffense && (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3) || target.HasBuff(BuffID.Daybreak)) && EastProcCooldown <= 0 && Player.whoAmI == Main.myPlayer) {
                    EastProcCooldown = 24;
                    Projectile.NewProjectile(Player.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileID.SolarWhipSwordExplosion, (int)(hit.Damage * 0.75f), 4f, Player.whoAmI, 0f, 0.9f);
                }
                if (Asc_FlameSorcerer) {
                    target.AddBuff(BuffID.Daybreak, 180);
                    target.AddBuff(BuffID.OnFire3, 240);
                    if (hit.Crit) {
                        Player.AddBuff(ModContent.BuffType<XiuhcoatlPyreDebuff>(), 240);
                    }
                }
            }

            // SORCERER #2 NORTH & EAST (Tezcatlipoca): Living Lightning on Crit + Chain Lightning on Hit
            if (proj.DamageType == DamageClass.Magic && Player.whoAmI == Main.myPlayer) {
                if (Asc_LightningSorcerer && hit.Crit && proj.type != ModContent.ProjectileType<LivingLightningOrbProj>() && LightningOrbCooldown <= 0) {
                    LightningOrbCooldown = 18;
                    SoundEngine.PlaySound(SoundID.Item94, target.Center);
                    Vector2 spawnVel = Main.rand.NextVector2CircularEdge(6f, 6f);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, spawnVel, ModContent.ProjectileType<LivingLightningOrbProj>(), (int)(hit.Damage * 0.65f), 2f, Player.whoAmI);
                }

                if (Asc_LightningSorcererOffense && EastProcCooldown <= 0) {
                    EastProcCooldown = 22;
                    int chains = 0;
                    for (int i = 0; i < Main.maxNPCs && chains < 3; i++) {
                        NPC other = Main.npc[i];
                        if (other.active && !other.friendly && other.whoAmI != target.whoAmI && other.lifeMax > 5 && !other.dontTakeDamage) {
                            if (Vector2.Distance(other.Center, target.Center) < 280f) {
                                chains++;
                                Projectile.NewProjectile(Player.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<TlalocArcVisualProj>(), 0, 0f, Player.whoAmI, other.Center.X, other.Center.Y);
                                Player.ApplyDamageToNPC(other, (int)(hit.Damage * 0.45f), 1f, 0, crit: false);
                            }
                        }
                    }
                }
            }

            if (KS_Overload && proj.DamageType == DamageClass.Magic && CurrentEnergyShield == MaxEnergyShield && MaxEnergyShield > 0) {
                target.SimpleStrikeNPC((int)MaxEnergyShield, 0);
                CurrentEnergyShield = 0;
                EnergyShieldRechargeDelay = 180;
                SoundEngine.PlaySound(SoundID.Item14, Player.Center);
            }
        }

        public static bool IsSpellTome(Item item) {
            int[] tomes = { ItemID.WaterBolt, ItemID.BookofSkulls, ItemID.DemonScythe, ItemID.CrystalStorm, ItemID.CursedFlames, ItemID.GoldenShower, ItemID.MagnetSphere, ItemID.LunarFlareBook };
            return tomes.Contains(item.type);
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
            if (ItemID.Sets.Yoyo[item.type]) damage += BonusYoyoDamage;
            if (item.useAmmo == AmmoID.Arrow) damage += BonusBowDamage;
            if (item.useAmmo == AmmoID.Bullet) damage += BonusGunDamage;
            if (ProjectileID.Sets.IsAWhip[item.shoot]) damage += BonusWhipDamage;
            if (IsSpellTome(item)) damage += BonusTomeDamage;

            if (KS_CentrifugalForce && !ItemID.Sets.Yoyo[item.type] && item.DamageType == DamageClass.Melee) damage *= 0f;
            if (KS_FletchersPride && item.useAmmo == AmmoID.Bullet) damage *= 0f;
            if (KS_Deadeye && item.useAmmo == AmmoID.Arrow) damage *= 0f;
            if (KS_Librarian && !IsSpellTome(item) && item.DamageType == DamageClass.Magic) damage *= 0f;
            if (KS_Channeler && IsSpellTome(item)) damage *= 0f;
            if (KS_Puppeteer && ProjectileID.Sets.IsAWhip[item.shoot]) damage *= 0f;
        }

        public override void SaveData(TagCompound tag) {
            EnsureArraysInitialized();
            tag["ChosenClass"] = ChosenClass ?? "";
            tag["ChosenAscendancy"] = ChosenAscendancy ?? "";
            tag["HasOnboarded"] = HasOnboarded;
            tag["Level"] = Level;
            tag["CurrentExp"] = CurrentExp;
            tag["TotalPassivePoints"] = TotalPassivePoints;
            tag["AllocatedNodeIds"] = AllocatedNodeIds.ToList();
            tag["UnlockedProfiles"] = UnlockedProfiles.ToList();
            tag["CompletedProfiles"] = CompletedProfiles.ToList();

            tag["AscendancyLoadout"] = AscendancyLoadout.Select(item => item != null ? ItemIO.Save(item) : ItemIO.Save(new Item())).ToList();
            tag["StashItems"] = StashItems.Select(item => item != null ? ItemIO.Save(item) : ItemIO.Save(new Item())).ToList();

            tag["HighestUnlockedAct"] = HighestUnlockedAct;
            tag["HighestUnlockedZone"] = HighestUnlockedZone;
        }

        public override void LoadData(TagCompound tag) {
            EnsureArraysInitialized();
            ChosenClass = tag.GetString("ChosenClass");
            if (tag.ContainsKey("ChosenAscendancy")) ChosenAscendancy = tag.GetString("ChosenAscendancy");
            HasOnboarded = tag.GetBool("HasOnboarded");
            if (tag.ContainsKey("Level")) Level = tag.GetInt("Level");
            if (tag.ContainsKey("CurrentExp")) CurrentExp = tag.GetLong("CurrentExp");
            if (tag.ContainsKey("TotalPassivePoints")) TotalPassivePoints = tag.GetInt("TotalPassivePoints");

            if (tag.ContainsKey("AllocatedNodeIds")) {
                HashSet<int> loadedIds = new HashSet<int>(tag.GetList<int>("AllocatedNodeIds"));
                AllocatedNodeIds = new HashSet<int>();
                foreach (int id in loadedIds) {
                    if (id >= 100 && id < 300) AllocatedNodeIds.Add(id + 900);
                    else if (id >= 300 && id < 500) AllocatedNodeIds.Add(id + 1700);
                    else if (id >= 500 && id < 700) AllocatedNodeIds.Add(id + 2500);
                    else if (id >= 700 && id < 900) AllocatedNodeIds.Add(id + 3300);
                    else AllocatedNodeIds.Add(id);
                }
            }

            if (tag.ContainsKey("UnlockedProfiles")) UnlockedProfiles = new HashSet<string>(tag.GetList<string>("UnlockedProfiles"));
            if (tag.ContainsKey("CompletedProfiles")) CompletedProfiles = new HashSet<string>(tag.GetList<string>("CompletedProfiles"));

            if (tag.ContainsKey("AscendancyLoadout")) {
                var savedList = tag.GetList<TagCompound>("AscendancyLoadout");
                for (int i = 0; i < Math.Min(4, savedList.Count); i++) {
                    AscendancyLoadout[i] = ItemIO.Load(savedList[i]) ?? new Item();
                }
            }

            if (tag.ContainsKey("HighestUnlockedAct")) HighestUnlockedAct = Math.Max(1, tag.GetInt("HighestUnlockedAct"));
            if (tag.ContainsKey("HighestUnlockedZone")) HighestUnlockedZone = Math.Max(1, tag.GetInt("HighestUnlockedZone"));

            if (tag.ContainsKey("StashItems")) {
                var savedStash = tag.GetList<TagCompound>("StashItems");
                for (int i = 0; i < Math.Min(100, savedStash.Count); i++) {
                    StashItems[i] = ItemIO.Load(savedStash[i]) ?? new Item();
                }
            }
        }
    }
}