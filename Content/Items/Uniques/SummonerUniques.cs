using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;

namespace WarpedAtlas.Content.Items.Uniques
{
    // ==========================================
    // CUSTOM BUFFS & PROJECTILES
    //
    // Known Bug: No icon for custom minions, to fix later.
    // ==========================================
    public class FungalEaterBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.StardustMinion;

        public override void SetStaticDefaults() {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<FungalEaterMinion>()] > 0) {
                player.buffTime[buffIndex] = 18000;
            } else {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams) {
            drawParams.DrawColor = Color.LimeGreen;
            return true;
        }
    }

    public class FungalEaterMinion : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.TinyEater;

        public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.StardustCellMinion);
            AIType = ProjectileID.StardustCellMinion;
            Projectile.width = 16;
            Projectile.height = 16;
            
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override bool PreAI() {
            Player player = Main.player[Projectile.owner];
            player.stardustMinion = false;
            
            if (player.dead || !player.active) {
                player.ClearBuff(ModContent.BuffType<FungalEaterBuff>());
            }
            
            // FIX: Removed the explicit active = false.
            // When the player right-clicks the buff, this check fails, causing the 
            // timeLeft to tick from 2 to 0, which safely despawns the minion.
            if (player.HasBuff(ModContent.BuffType<FungalEaterBuff>())) {
                Projectile.timeLeft = 2;
            }
            
            return true;
        }

        public override void PostAI() {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6) {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 2) {
                Projectile.frame = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            lightColor = Color.LimeGreen;
            return true;
        }
    }

    // ==========================================
    // SUMMONER UNIQUE WEAPONS
    // ==========================================

    // --- TIER 1: SOVEREIGN'S CHAIN ---
    public class SovereignChain : AtlasUniqueItem
    {
        public override int BossTier => 1;
        public override string Archetype => "Summoner";
        public override string Texture => "Terraria/Images/Item_" + ItemID.CoolWhip;
        public override string EffectText => "+1 Max Minion. Current Spirit Ward scales Summon Damage.\nWhip strikes leech Spirit Ward up to 200% of maximum capacity.";
        public override string FlavorText => "Submit to the collective will. My shield is their strength.";
        
        public override void SafeSetDefaults() {
            Item.width = 30; Item.height = 30;
            Item.damage = 45;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.useTime = 25; Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.shoot = ProjectileID.CoolWhip;
            Item.shootSpeed = 8f;
            Item.color = Color.DarkRed;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
        }
        
        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            player.maxMinions += 1;
            if (prog.CurrentWard > 0) {
                player.GetDamage(DamageClass.Summon) += (prog.CurrentWard * 0.002f);
            }
        }
        
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.MaxWard > 0) {
                prog.CurrentWard += hit.Damage * 0.15f;
                float overcapLimit = prog.MaxWard * 2f;
                if (prog.CurrentWard > overcapLimit) {
                    prog.CurrentWard = overcapLimit;
                }
            }
        }
    }

    // --- TIER 2: MONOLITH CORE ---
    public class MonolithCore : AtlasUniqueItem
    {
        public override int BossTier => 2;
        public override string Archetype => "Summoner";
        public override string Texture => "Terraria/Images/Item_" + ItemID.QueenSpiderStaff;
        public override string EffectText => "Grants +1% Summon Damage for every 5 Max Spirit Ward.";
        public override string FlavorText => "Carved from the bones of the old world.";
        
        public override void SafeSetDefaults() {
            Item.width = 46; Item.height = 46;
            Item.damage = 35;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.useTime = 30; Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.shoot = ProjectileID.VenomSpider;
            Item.sentry = true;
            Item.color = Color.Gray; 
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
        }
        
        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.MaxWard > 0) {
                float wardScaling = (prog.MaxWard / 5) * 0.01f;
                player.GetDamage(DamageClass.Summon) += wardScaling;
            }
        }
    }

    // --- TIER 3: PHARAOH'S CURSE ---
    public class PharaohsCurse : AtlasUniqueItem
    {
        public override int BossTier => 3;
        public override string Archetype => "Summoner";
        public override string Texture => "Terraria/Images/Item_" + ItemID.StormTigerStaff;
        public override string EffectText => "Grants +2 Max Minions, but reduces Movement Speed by 30%.";
        public override string FlavorText => "The sands remember every empire they've swallowed.";
        
        public override void SafeSetDefaults() {
            Item.width = 46; Item.height = 46;
            Item.damage = 65;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.useTime = 30; Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.shoot = ProjectileID.StormTigerGem;
            Item.buffType = BuffID.StormTiger;
            Item.color = Color.Gold; 
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Red;
        }
        
        public override void HoldItem(Player player) {
            player.maxMinions += 2;
            player.moveSpeed -= 0.30f;
        }
    }

    // --- TIER 4: SERAPHIM CHIME ---
    public class SeraphimChime : AtlasUniqueItem
    {
        public override int BossTier => 4;
        public override string Archetype => "Summoner";
        public override string Texture => "Terraria/Images/Item_" + ItemID.RainbowWhip;
        public override string EffectText => "Grants +1 Max Minion for every 50 Max Spirit Ward.";
        public override string FlavorText => "Every toll brings them closer to the light.";
        
        public override void SafeSetDefaults() {
            Item.width = 40; Item.height = 40;
            Item.damage = 80;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.useTime = 28; Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.shoot = ProjectileID.RainbowWhip;
            Item.shootSpeed = 10f;
            Item.color = Color.Magenta; 
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
        }
        
        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.MaxWard >= 50) {
                player.maxMinions += (prog.MaxWard / 50);
            }
        }
    }

    // --- TIER 5: CRYO HEART ---
    public class CryoHeart : AtlasUniqueItem
    {
        public override int BossTier => 5;
        public override string Archetype => "Summoner";
        public override string Texture => "Terraria/Images/Item_" + ItemID.StaffoftheFrostHydra;
        public override string EffectText => "Sets Max Minions to 0. Grants +3 Max Sentries and +60% Summon Damage.";
        public override string FlavorText => "Shatter the swarm. Fortify the solitary guard.";
        
        public override void SafeSetDefaults() {
            Item.width = 46; Item.height = 46;
            Item.damage = 110;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 20;
            Item.useTime = 30; Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.shoot = ProjectileID.FrostHydra;
            Item.sentry = true;
            Item.color = Color.LightCyan; 
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Red;
        }
        
        public override void HoldItem(Player player) {
            player.maxMinions = 0;
            player.maxTurrets += 3;
            player.GetDamage(DamageClass.Summon) += 0.60f;
        }
    }

    // --- TIER 6: ASHEN LASH ---
    public class AshenLash : AtlasUniqueItem
    {
        public override int BossTier => 6;
        public override string Archetype => "Summoner";
        public override string Texture => "Terraria/Images/Item_" + ItemID.FireWhip;
        public override string EffectText => "Max Spirit Ward scales Summon Damage (+0.5% per point of Ward).";
        public override string FlavorText => "Even embers leave scars.";
        
        public override void SafeSetDefaults() {
            Item.width = 40; Item.height = 40;
            Item.damage = 125;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.useTime = 30; Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.shoot = ProjectileID.FireWhip;
            Item.shootSpeed = 12f;
            Item.color = Color.OrangeRed; 
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Red;
        }
        
        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.MaxWard > 0) {
                player.GetDamage(DamageClass.Summon) += (prog.MaxWard * 0.005f);
            }
        }
    }

    // --- TIER 7: MATRIX OVERLORD ---
    public class MatrixOverlord : AtlasUniqueItem
    {
        public override int BossTier => 7;
        public override string Archetype => "Summoner";
        public override string Texture => "Terraria/Images/Item_" + ItemID.DeadlySphereStaff;
        public override string EffectText => "+15% Summon Attack Speed (+40% while Spirit Ward is at maximum capacity).";
        public override string FlavorText => "The network demands obedience.";
        
        public override void SafeSetDefaults() {
            Item.width = 42; Item.height = 42;
            Item.damage = 80;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.useTime = 30; Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.shoot = ProjectileID.DeadlySphere;
            Item.buffType = BuffID.DeadlySphere; 
            Item.color = Color.DeepSkyBlue; 
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Red;
        }
        
        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.CurrentWard >= prog.MaxWard && prog.MaxWard > 0) {
                player.GetAttackSpeed(DamageClass.Summon) += 0.40f;
            } else {
                player.GetAttackSpeed(DamageClass.Summon) += 0.15f;
            }
        }
    }

    // --- TIER 8: FUNGAL SYMBIOTE ---
    public class FungalSymbiote : AtlasUniqueItem
    {
        public override int BossTier => 8;
        public override string Archetype => "Summoner";
        public override string Texture => "Terraria/Images/Item_" + ItemID.StardustCellStaff;
        public override string EffectText => "Rapidly drains Spirit Ward, granting +1.5% Summon Damage per missing Spirit Ward.";
        public override string FlavorText => "It breeds in the fractures of your soul.";
        
        public override void SafeSetDefaults() {
            Item.width = 46; Item.height = 46;
            Item.damage = 160;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.useTime = 30; Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.color = Color.LimeGreen; 
            
            Item.shoot = ModContent.ProjectileType<FungalEaterMinion>();
            Item.buffType = ModContent.BuffType<FungalEaterBuff>();
            Item.shootSpeed = 10f; 
            
            Item.value = Item.sellPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Red;
        }
        
        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.MaxWard > 0) {
                prog.CurrentWard -= 2f;
                if (prog.CurrentWard < 0) prog.CurrentWard = 0;
                
                float missingWard = prog.MaxWard - prog.CurrentWard;
                player.GetDamage(DamageClass.Summon) += (missingWard * 0.015f);
            }
        }
    }
}