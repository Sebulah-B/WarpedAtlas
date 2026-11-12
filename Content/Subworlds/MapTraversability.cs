using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using WarpedAtlas.Content.Items;
using WarpedAtlas.Content.Players;
using System.Linq;

namespace WarpedAtlas.Content.Subworlds
{
    public enum MapCell : byte {
        Void = 0, Shell = 1, Hollow = 2, Rope = 3, Platform = 4
    }

    public static class MapTraversability
    {
        public const int CorridorWidth = 6;
        public const int CorridorHeight = 6;

        public static HashSet<int> DroppedLootTracker = new HashSet<int>();
        

        public static void MapCorridor(Point start, Point end, MapCell[,] blueprint, bool isShell) {
            int width = isShell ? CorridorWidth + 4 : CorridorWidth;
            int height = isShell ? CorridorHeight + 4 : CorridorHeight;
            MapCell cellType = isShell ? MapCell.Shell : MapCell.Hollow;

            int currentX = start.X;
            int currentY = start.Y;

            int stepX = start.X < end.X ? 1 : -1;
            while (currentX != end.X) {
                FillCentered(currentX, currentY, width, height, cellType, blueprint);
                currentX += stepX;
            }

            int stepY = start.Y < end.Y ? 1 : -1;
            while (currentY != end.Y) {
                FillCentered(currentX, currentY, width, height, cellType, blueprint);
                if (!isShell) {
                    if (currentX >= 0 && currentX < blueprint.GetLength(0) && currentY >= 0 && currentY < blueprint.GetLength(1)) {
                        blueprint[currentX, currentY] = MapCell.Rope;
                    }
                }
                currentY += stepY;
            }
            FillCentered(end.X, end.Y, width, height, cellType, blueprint);
        }

        public static void FillCentered(int x, int y, int width, int height, MapCell cellType, MapCell[,] blueprint) {
            int halfWidth = width / 2;
            int halfHeight = height / 2;
            for (int cx = x - halfWidth; cx <= x + halfWidth; cx++) {
                for (int cy = y - halfHeight; cy <= y + halfHeight; cy++) {
                    if (cx < 0 || cx >= blueprint.GetLength(0) || cy < 0 || cy >= blueprint.GetLength(1)) continue;
                    if (cellType == MapCell.Hollow && (blueprint[cx, cy] == MapCell.Rope || blueprint[cx, cy] == MapCell.Platform)) continue;
                    blueprint[cx, cy] = cellType;
                }
            }
        }

        public static void FillBounds(Rectangle bounds, MapCell cellType, MapCell[,] blueprint) {
            for (int x = bounds.X; x < bounds.Right; x++) {
                for (int y = bounds.Y; y < bounds.Bottom; y++) {
                    if (x < 0 || x >= blueprint.GetLength(0) || y < 0 || y >= blueprint.GetLength(1)) continue;
                    blueprint[x, y] = cellType;
                }
            }
        }

        public static void FinalizeTraversability(MapGridGraph graph, MapCell[,] blueprint) {
            int width = blueprint.GetLength(0);
            int height = blueprint.GetLength(1);

            // 1. Gravity Cascade for existing vertical corridor ropes
            for (int y = 0; y < height - 1; y++) {
                for (int x = 0; x < width; x++) {
                    if (blueprint[x, y] == MapCell.Rope) {
                        if (blueprint[x, y + 1] == MapCell.Hollow) {
                            blueprint[x, y + 1] = MapCell.Rope;
                        }
                    }
                }
            }

            // 2. Doorway Rope Drops for elevated corridor entrances
            foreach (var node in graph.Nodes) {
                int leftInterior = node.TileBounds.X;
                int rightInterior = node.TileBounds.Right - 1;
                for (int y = node.TileBounds.Y; y < node.TileBounds.Bottom - 1; y++) {
                    
                    if (leftInterior > 0 && 
                        (blueprint[leftInterior - 1, y] == MapCell.Hollow || blueprint[leftInterior - 1, y] == MapCell.Rope) && 
                        (blueprint[leftInterior - 1, y + 1] == MapCell.Shell || blueprint[leftInterior - 1, y + 1] == MapCell.Platform) &&
                        blueprint[leftInterior, y + 1] == MapCell.Hollow) 
                    {
                        DropRope(leftInterior, y, blueprint);
                    }

                    if (rightInterior < width - 1 && 
                        (blueprint[rightInterior + 1, y] == MapCell.Hollow || blueprint[rightInterior + 1, y] == MapCell.Rope) && 
                        (blueprint[rightInterior + 1, y + 1] == MapCell.Shell || blueprint[rightInterior + 1, y + 1] == MapCell.Platform) &&
                        blueprint[rightInterior, y + 1] == MapCell.Hollow) 
                    {
                        DropRope(rightInterior, y, blueprint);
                    }
                }
            }
            
            // 3. APPLY ATLAS LEVERS POST-PROCESSING
            ApplyAtlasLevers(graph, blueprint);
        }
        
        public static void ApplyAtlasLevers(MapGridGraph graph, MapCell[,] blueprint) {
            var prog = Main.LocalPlayer.GetModPlayer<AtlasProgressionPlayer>();
            var profile = BaseDungeonSubworld.ActiveProfile;
            
            int width = blueprint.GetLength(0);
            int height = blueprint.GetLength(1);

            bool shouldFlood = (profile != null && profile.Submerged) || prog.Lever_TlalocsTorrent;
            bool isVoid = prog.Lever_CelestialVoid;

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    
                    // Tlaloc's Torrent: Flood the bottom of hollow spaces
                    if (shouldFlood && blueprint[x, y] == MapCell.Hollow) {
                        if (y < height - 1 && (blueprint[x, y + 1] == MapCell.Shell || blueprint[x, y + 1] == MapCell.Platform)) {
                            Tile tile = Main.tile[x, y]; // Safe extraction to avoid CS1612
                            tile.LiquidAmount = 255;
                            tile.LiquidType = LiquidID.Water;
                        }
                    }

                    // Celestial Void: Erase structural walls and convert floors to platforms
                    if (isVoid) {
                        Tile tile = Main.tile[x, y]; 
                        tile.WallType = 0; 

                        if (blueprint[x, y] == MapCell.Shell) {
                            blueprint[x, y] = MapCell.Platform; 
                        }
                    }
                }
            }
        }

        private static void DropRope(int x, int startY, MapCell[,] blueprint) {
            int height = blueprint.GetLength(1);
            for (int y = startY; y < height; y++) {
                if (blueprint[x, y] == MapCell.Shell || blueprint[x, y] == MapCell.Platform) {
                    break; 
                }
                if (blueprint[x, y] == MapCell.Hollow) {
                    blueprint[x, y] = MapCell.Rope;
                }
            }
        }

        private static void RollChestAffixes(Item item) {
            if (AtlasGlobalItem.IsValidForAffixes(item)) {
                var globalItem = item.GetGlobalItem<AtlasGlobalItem>();
                
                if (BaseDungeonSubworld.MapRNG.NextBool()) {
                    globalItem.CurrentTier = ItemTier.Rare;
                    var prePool = AtlasGlobalItem.GetPool(item, "Prefix");
                    var sufPool = AtlasGlobalItem.GetPool(item, "Suffix");
                    
                    int preCount = BaseDungeonSubworld.MapRNG.Next(1, 4);
                    int sufCount = BaseDungeonSubworld.MapRNG.Next(1, 4);
                    
                    for (int i = 0; i < preCount; i++) {
                        var available = prePool.Where(p => !globalItem.Prefixes.Any(existing => existing.Name == p.Name)).ToList();
                        if (available.Count > 0) globalItem.Prefixes.Add(available[BaseDungeonSubworld.MapRNG.Next(available.Count)].CloneAndRoll());
                    }
                    for (int i = 0; i < sufCount; i++) {
                        var available = sufPool.Where(s => !globalItem.Suffixes.Any(existing => existing.Name == s.Name)).ToList();
                        if (available.Count > 0) globalItem.Suffixes.Add(available[BaseDungeonSubworld.MapRNG.Next(available.Count)].CloneAndRoll());
                    }
                } else {
                    globalItem.CurrentTier = ItemTier.Magic;
                    if (BaseDungeonSubworld.MapRNG.NextBool()) {
                        var pool = AtlasGlobalItem.GetPool(item, "Prefix");
                        globalItem.Prefixes.Add(pool[BaseDungeonSubworld.MapRNG.Next(pool.Count)].CloneAndRoll());
                    } else {
                        var pool = AtlasGlobalItem.GetPool(item, "Suffix");
                        globalItem.Suffixes.Add(pool[BaseDungeonSubworld.MapRNG.Next(pool.Count)].CloneAndRoll());
                    }
                }
            }
        }

        public static void BuildProceduralInterior(Rectangle bounds, RoomType type, ushort floorTile, ushort wallTile) {
            if (type == RoomType.Standard) {
                MapPrefab.StampPrefab(bounds, MapPrefab.BalconyChamber, floorTile, wallTile);
            } 
            else if (type == RoomType.Puzzle) {
                MapPrefab.StampPrefab(bounds, MapPrefab.PillaredVault, floorTile, wallTile);
            }
            if (type == RoomType.Vault) {
                int targetX = bounds.X + (bounds.Width / 4);
                int floorY = bounds.Bottom - 1;
                for (int px = targetX - 2; px <= targetX + 2; px++) {
                    for (int py = floorY - 1; py <= floorY; py++) {
                        Tile pTile = Main.tile[px, py]; 
                        pTile.HasTile = true;
                        pTile.TileType = floorTile;
                        pTile.IsHalfBlock = false;
                        pTile.Slope = SlopeType.Solid;
                    }
                }
                for (int px = targetX - 2; px <= targetX + 2; px++) {
                    WorldGen.SquareTileFrame(px, floorY - 1, true);
                }
                for (int cx = targetX; cx <= targetX + 1; cx++) {
                    for (int cy = floorY - 3; cy <= floorY - 2; cy++) {
                        Tile airTile = Main.tile[cx, cy];
                        airTile.HasTile = false;
                    }
                }
                
                int chestIndex = WorldGen.PlaceChest(targetX, floorY - 3, style: 1);
                
                if (chestIndex == -1) {
                    chestIndex = Chest.CreateChest(targetX, floorY - 3);
                    if (chestIndex != -1) {
                        for (int cx = 0; cx < 2; cx++) {
                            for (int cy = 0; cy < 2; cy++) {
                                Tile tile = Main.tile[targetX + cx, floorY - 3 + cy];
                                tile.HasTile = true;
                                tile.TileType = TileID.Containers;
                                tile.TileFrameX = (short)(1 * 36 + cx * 18);
                                tile.TileFrameY = (short)(cy * 18);
                            }
                        }
                    }
                }
                
                if (chestIndex != -1) {
                    int slotIndex = 0;
                    
                    string playerClass = Main.LocalPlayer.GetModPlayer<AtlasProgressionPlayer>().ChosenClass;
                    int weaponId = GetVaultWeapon(BaseDungeonSubworld.ActiveProfile, playerClass);
                    if (weaponId != 0) {
                        Item vaultWeapon = new Item();
                        vaultWeapon.SetDefaults(weaponId);
                        RollChestAffixes(vaultWeapon);
                        Main.chest[chestIndex].item[slotIndex++] = vaultWeapon;
                    }
                    int accessoryId = GetVaultAccessory(BaseDungeonSubworld.ActiveProfile);
                    if (accessoryId != 0) {
                        Item vaultAccessory = new Item();
                        vaultAccessory.SetDefaults(accessoryId);
                        RollChestAffixes(vaultAccessory);
                        Main.chest[chestIndex].item[slotIndex++] = vaultAccessory;
                    }
                    int armorId = GetVaultArmor(BaseDungeonSubworld.ActiveProfile, playerClass);
                    if (armorId != 0) {
                        Item vaultArmor = new Item();
                        vaultArmor.SetDefaults(armorId);
                        RollChestAffixes(vaultArmor);
                        Main.chest[chestIndex].item[slotIndex++] = vaultArmor;
                    }
                }
            }
        }

        private static int GetVaultWeapon(MapProfile activeProfile, string playerClass) {
            if (activeProfile == null) return 0;
            string profileName = activeProfile.GetType().Name;
            
            int[] pool = profileName switch {
                "DungeonProfile" => playerClass switch {
                    "Warrior" => new int[] { ItemID.FieryGreatsword, ItemID.Flamarang, ItemID.DarkLance, ItemID.Cascade },
                    "Ranger" => new int[] { ItemID.Boomstick, ItemID.Minishark, ItemID.HellwingBow, ItemID.MoltenFury },
                    "Sorcerer" => new int[] { ItemID.WaterBolt, ItemID.DemonScythe, ItemID.Flamelash, ItemID.FlowerofFire },
                    "Summoner" => new int[] { ItemID.SlimeStaff, ItemID.ImpStaff, ItemID.BlandWhip, ItemID.BabyBirdStaff },
                    _ => new int[] { ItemID.GoldBroadsword }
                },
                "LihzahrdProfile" => playerClass switch {
                    "Warrior" => new int[] { ItemID.BouncingShield, ItemID.Yelets, ItemID.DeathSickle },
                    "Ranger" => new int[] { ItemID.OnyxBlaster, ItemID.Uzi, ItemID.ChlorophyteShotbow },
                    "Sorcerer" => new int[] { ItemID.PoisonStaff, ItemID.CrystalSerpent, ItemID.MeteorStaff },
                    "Summoner" => new int[] { ItemID.CoolWhip, ItemID.OpticStaff, ItemID.QueenSpiderStaff },
                    _ => new int[] { ItemID.BouncingShield }
                },
                "SandstoneProfile" => playerClass switch {
                    "Warrior" => new int[] { ItemID.Excalibur, ItemID.BouncingShield, ItemID.ChlorophyteClaymore },
                    "Ranger" => new int[] { ItemID.Megashark, ItemID.ClockworkAssaultRifle, ItemID.DaedalusStormbow },
                    "Sorcerer" => new int[] { ItemID.CrystalSerpent, ItemID.RainbowRod, ItemID.MagicalHarp },
                    "Summoner" => new int[] { ItemID.SpiderStaff, ItemID.SwordWhip },
                    _ => new int[] { ItemID.Excalibur }
                },
                "HallowedProfile" => playerClass switch {
                    "Warrior" => new int[] { ItemID.TrueExcalibur, ItemID.VampireKnives, ItemID.PaladinsHammer },
                    "Ranger" => new int[] { ItemID.SnowmanCannon, ItemID.SniperRifle, ItemID.VenusMagnum },
                    "Sorcerer" => new int[] { ItemID.VenomStaff, ItemID.ShadowbeamStaff, ItemID.InfernoFork },
                    "Summoner" => new int[] { ItemID.MaceWhip, ItemID.SanguineStaff, ItemID.DeadlySphereStaff },
                    _ => new int[] { ItemID.TrueExcalibur }
                },
                "CryoFrostProfile" => playerClass switch {
                    "Warrior" => new int[] { ItemID.Frostbrand, ItemID.NorthPole, ItemID.Seedler },
                    "Ranger" => new int[] { ItemID.ChainGun, ItemID.IceBow, ItemID.Toxikarp },
                    "Sorcerer" => new int[] { ItemID.BlizzardStaff, ItemID.FrostStaff, ItemID.ToxicFlask },
                    "Summoner" => new int[] { ItemID.StaffoftheFrostHydra, ItemID.PygmyStaff, ItemID.RavenStaff },
                    _ => new int[] { ItemID.Frostbrand }
                },
                "HellforgeProfile" => playerClass switch {
                    "Warrior" => new int[] { ItemID.TerraBlade, ItemID.GolemFist, ItemID.PossessedHatchet },
                    "Ranger" => new int[] { ItemID.Tsunami, ItemID.Xenopopper, ItemID.ElectrosphereLauncher },
                    "Sorcerer" => new int[] { ItemID.RazorbladeTyphoon, ItemID.MagnetSphere, ItemID.SpiritFlame },
                    "Summoner" => new int[] { ItemID.XenoStaff, ItemID.DeadlySphereStaff, ItemID.EmpressBlade },
                    _ => new int[] { ItemID.TerraBlade }
                },
                "GraniteProfile" => playerClass switch {
                    "Warrior" => new int[] { ItemID.InfluxWaver, ItemID.DD2SquireBetsySword, ItemID.PiercingStarlight },
                    "Ranger" => new int[] { ItemID.ElectrosphereLauncher, ItemID.FairyQueenRangedItem, ItemID.RocketLauncher },
                    "Sorcerer" => new int[] { ItemID.LaserMachinegun, ItemID.FairyQueenMagicItem, ItemID.BatScepter },
                    "Summoner" => new int[] { ItemID.ScytheWhip, ItemID.StardustDragonStaff, ItemID.TempestStaff },
                    _ => new int[] { ItemID.InfluxWaver }
                },
                "FungalProfile" => playerClass switch {
                    "Warrior" => new int[] { ItemID.SolarEruption, ItemID.DayBreak, ItemID.StarWrath, ItemID.Terrarian },
                    "Ranger" => new int[] { ItemID.Phantasm, ItemID.SDMG, ItemID.VortexBeater, ItemID.Celeb2 },
                    "Sorcerer" => new int[] { ItemID.LastPrism, ItemID.LunarFlareBook, ItemID.NebulaArcanum, ItemID.NebulaBlaze },
                    "Summoner" => new int[] { ItemID.RainbowWhip, ItemID.EmpressBlade, ItemID.StardustCellStaff },
                    _ => new int[] { ItemID.SolarEruption }
                },
                _ => new int[] { ItemID.CopperBroadsword }
            };
            return RerollDuplicates(pool);
        }

        private static int GetVaultAccessory(MapProfile activeProfile) {
            if (activeProfile == null) return 0;
            
            int[] pool = activeProfile.GetType().Name switch {
                "DungeonProfile" => new int[] { ItemID.HermesBoots, ItemID.CloudinaBottle, ItemID.FeralClaws, ItemID.Aglet, ItemID.CobaltShield },
                "LihzahrdProfile" => new int[] { ItemID.LightningBoots, ItemID.WarriorEmblem, ItemID.RangerEmblem, ItemID.SorcererEmblem, ItemID.SummonerEmblem },
                "SandstoneProfile" => new int[] { ItemID.TitanGlove, ItemID.ObsidianShield, ItemID.MoonCharm, ItemID.MagicQuiver },
                "HallowedProfile" => new int[] { ItemID.AvengerEmblem, ItemID.MechanicalGlove, ItemID.PutridScent, ItemID.FleshKnuckles },
                "CryoFrostProfile" => new int[] { ItemID.FrostsparkBoots, ItemID.IceSkates, ItemID.FrozenTurtleShell, ItemID.BlizzardinaBottle },
                "HellforgeProfile" => new int[] { ItemID.LavaWaders, ItemID.LavaCharm, ItemID.MagmaStone, ItemID.FireGauntlet },
                "GraniteProfile" => new int[] { ItemID.MasterNinjaGear, ItemID.ReconScope, ItemID.NecromanticScroll, ItemID.PapyrusScarab },
                "FungalProfile" => new int[] { ItemID.TerrasparkBoots, ItemID.CelestialShell, ItemID.DestroyerEmblem, ItemID.CelestialStone },
                _ => new int[] { ItemID.HermesBoots }
            };
            return RerollDuplicates(pool);
        }

        private static int GetVaultArmor(MapProfile activeProfile, string playerClass) {
            if (activeProfile == null) return 0;
            string profileName = activeProfile.GetType().Name;
            
            int[] pool = profileName switch {
                "DungeonProfile" => playerClass switch {
                    "Warrior" => new int[] { ItemID.MoltenHelmet, ItemID.MoltenBreastplate, ItemID.MoltenGreaves },
                    "Ranger" => new int[] { ItemID.NecroHelmet, ItemID.NecroBreastplate, ItemID.NecroGreaves },
                    "Sorcerer" => new int[] { ItemID.MeteorHelmet, ItemID.MeteorSuit, ItemID.MeteorLeggings },
                    "Summoner" => new int[] { ItemID.BeeHeadgear, ItemID.BeeBreastplate, ItemID.BeeGreaves },
                    _ => new int[] { ItemID.MoltenHelmet }
                },
                "LihzahrdProfile" => playerClass switch {
                    "Warrior" => new int[] { ItemID.AdamantiteHelmet, ItemID.AdamantiteBreastplate, ItemID.AdamantiteLeggings },
                    "Ranger" => new int[] { ItemID.AdamantiteMask, ItemID.AdamantiteBreastplate, ItemID.AdamantiteLeggings },
                    "Sorcerer" => new int[] { ItemID.AdamantiteHeadgear, ItemID.AdamantiteBreastplate, ItemID.AdamantiteLeggings },
                    "Summoner" => new int[] { ItemID.SpiderMask, ItemID.SpiderBreastplate, ItemID.SpiderGreaves },
                    _ => new int[] { ItemID.AdamantiteHelmet }
                },
                "SandstoneProfile" => playerClass switch {
                    "Warrior" => new int[] { ItemID.HallowedMask, ItemID.HallowedPlateMail, ItemID.HallowedGreaves },
                    "Ranger" => new int[] { ItemID.HallowedHelmet, ItemID.HallowedPlateMail, ItemID.HallowedGreaves },
                    "Sorcerer" => new int[] { ItemID.HallowedHeadgear, ItemID.HallowedPlateMail, ItemID.HallowedGreaves },
                    "Summoner" => new int[] { ItemID.HallowedHood, ItemID.HallowedPlateMail, ItemID.HallowedGreaves },
                    _ => new int[] { ItemID.HallowedMask }
                },
                "HallowedProfile" => playerClass switch {
                    "Warrior" => new int[] { ItemID.TurtleHelmet, ItemID.TurtleScaleMail, ItemID.TurtleLeggings },
                    "Ranger" => new int[] { ItemID.ShroomiteHeadgear, ItemID.ShroomiteBreastplate, ItemID.ShroomiteLeggings },
                    "Sorcerer" => new int[] { ItemID.SpectreHood, ItemID.SpectreRobe, ItemID.SpectrePants },
                    "Summoner" => new int[] { ItemID.SpookyHelmet, ItemID.SpookyBreastplate, ItemID.SpookyLeggings },
                    _ => new int[] { ItemID.TurtleHelmet }
                },
                _ => new int[] { ItemID.GoldHelmet }
            };
            return RerollDuplicates(pool);
        }

        private static int RerollDuplicates(int[] pool) {
            List<int> validItems = pool.Where(id => !DroppedLootTracker.Contains(id)).ToList();
            if (validItems.Count == 0) {
                return pool[BaseDungeonSubworld.MapRNG.Next(pool.Length)];
            }
            int chosen = validItems[BaseDungeonSubworld.MapRNG.Next(validItems.Count)];
            DroppedLootTracker.Add(chosen);
            return chosen;
        }
    }
}