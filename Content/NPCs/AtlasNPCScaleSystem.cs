using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using SubworldLibrary;
using WarpedAtlas.Content.Subworlds;
using WarpedAtlas.Content.Items.Currencies;
using WarpedAtlas.Content.Players;

namespace WarpedAtlas.Content.NPCs
{
    public class AtlasNPCScaleSystem : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossLifeScale)
        {
            if (!SubworldSystem.IsActive<BaseDungeonSubworld>())
                return;

            NPC vanillaDefaults = ContentSamples.NpcsByNetId[npc.type];

            if (vanillaDefaults.lifeMax <= 5 || !npc.chaseable || npc.dontTakeDamage || npc.immortal)
                return;

            // Inside AtlasNPCScaleSystem.ApplyDifficultyAndPlayerScaling
            if (!npc.friendly && npc.damage > 0)
            {
                int tier = BaseDungeonSubworld.CurrentMapTier;
                int pressure = BaseDungeonSubworld.CurrentPressureLevel;
                
                // Each Pressure Level applies a compounding 15% increase to Health and Damage
                float pressureMult = 1f + (pressure * 0.15f); 
                
                // --- STANDARDIZED HEALTH FLOOR & SCALING ---
                int baseFloorHp = 100 + ((tier - 1) * 50);
                
                float roleMult = 1.0f;
                if (npc.type == NPCID.GreenSlime || npc.type == NPCID.BlueSlime || npc.type == NPCID.Hellbat || npc.type == NPCID.Pixie) {
                    roleMult = 0.6f;
                }
                else if (npc.type == NPCID.ArmoredSkeleton || npc.type == NPCID.SandElemental) {
                    roleMult = 1.2f;
                }
                else if (npc.type == NPCID.DarkCaster || npc.type == NPCID.FlyingSnake || npc.type == NPCID.IlluminantBat) {
                    roleMult = 0.8f;
                }

                npc.lifeMax = (int)(((vanillaDefaults.lifeMax * 0.2f + baseFloorHp * 0.8f) * roleMult) * pressureMult);
                npc.life = npc.lifeMax;

                int baseDmg = vanillaDefaults.damage > 0 ? vanillaDefaults.damage : 20;
                npc.damage = (int)((baseDmg * (1f + (tier - 1) * 0.15f)) * pressureMult);
                npc.defense = vanillaDefaults.defense + (tier * 2) + (pressure * 2);
            }
        }

        public override void OnKill(NPC npc) {
            if (!SubworldSystem.IsActive<BaseDungeonSubworld>())
                return;

            if (npc.friendly || npc.lifeMax <= 5)
                return;

            Player player = Main.LocalPlayer;
            if (player.active) {
                int tier = BaseDungeonSubworld.CurrentMapTier;
                long baseExp = npc.boss ? 500 * tier : 50 * tier; 
                
                var prog = player.GetModPlayer<AtlasProgressionPlayer>();
                prog.AddExp(baseExp);
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            if (!SubworldSystem.IsActive<BaseDungeonSubworld>())
                return;

            // STRIP VANILLA DROPS: Wipes out Gel, Lenses, Bones, Banners, etc., for all standard monsters.
            // Hearts, Mana Stars, and Money completely bypass this database and will still drop dynamically based on player needs.
            if (npc.type < NPCID.Count) {
                npcLoot.RemoveWhere(rule => true);
            }

            if (npc.friendly || npc.lifeMax <= 5 || npc.damage <= 0)
                return;

            int tier = BaseDungeonSubworld.CurrentMapTier;
            
            // Custom ARPG currency and potion injection
            npcLoot.Add(ItemDropRule.Common(72, 1, 5 * tier, 15 * tier)); 
            
            int healthPotionId = tier switch {
                1 => ItemID.LesserHealingPotion,
                2 => ItemID.HealingPotion,
                3 => ItemID.GreaterHealingPotion,
                _ => ItemID.SuperHealingPotion
            };
            npcLoot.Add(ItemDropRule.Common(healthPotionId, 15, 1, 2));
            
            int manaPotionId = tier switch {
                1 => ItemID.LesserManaPotion,
                2 => ItemID.ManaPotion,
                3 => ItemID.GreaterManaPotion,
                _ => ItemID.SuperManaPotion
            };
            npcLoot.Add(ItemDropRule.Common(manaPotionId, 15, 1, 2));
            
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OrbOfAlteration>(), 40));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OrbOfScouring>(), 80)); 
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OrbOfAnnulment>(), 150));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DivineOrb>(), 300));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ExaltedOrb>(), 500)); 
        }
    }
}