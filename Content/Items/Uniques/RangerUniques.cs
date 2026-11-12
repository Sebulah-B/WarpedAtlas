using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;

namespace WarpedAtlas.Content.Items.Uniques
{
    // --- TIER 1: WINDRIPPER'S ECHO ---
    public class WindripperEcho : AtlasUniqueItem
    {
        public override int BossTier => 1;
        public override string Archetype => "Ranger";
        public override string Texture => "Terraria/Images/Item_" + ItemID.Tsunami;
        public override string EffectText => "+10% Evasion Chance. Fires a 3-arrow spread.\nBonus Movement Speed is added to Ranged Damage.";
        public override string FlavorText => "The gale asks no permission. Velocity is lethality.";

        public override void SafeSetDefaults() {
            Item.width = 32; Item.height = 64;
            Item.damage = 40;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 18; Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 15f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            prog.EvasionChance += 0.10f;

            float speedBonus = player.moveSpeed - 1f;
            if (speedBonus > 0) {
                player.GetDamage(DamageClass.Ranged) += speedBonus;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            for (int i = -1; i <= 1; i++) {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(10 * i));
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }

    // --- TIER 2: RUBBLE MAKER ---
    public class RubbleMaker : AtlasUniqueItem
    {
        public override int BossTier => 2;
        public override string Archetype => "Ranger";
        public override string Texture => "Terraria/Images/Item_" + ItemID.OnyxBlaster;
        public override string EffectText => "Grants +25 Defense and +40% Ranged Damage while standing completely still.";
        public override string FlavorText => "Demolition requires a sturdy foundation.";

        public override void SafeSetDefaults() {
            Item.width = 44; Item.height = 20;
            Item.damage = 45;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 25; Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 12f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            if (player.velocity.X == 0 && player.velocity.Y == 0) {
                player.statDefense += 25;
                player.GetDamage(DamageClass.Ranged) += 0.40f;

                if (Main.rand.NextBool(10)) {
                    Dust.NewDust(player.position, player.width, player.height, DustID.Torch, 0f, -2f, 0, default, 1.5f);
                }
            }
        }
    }

    // --- TIER 3: DUNE SHREDDER ---
    public class DuneShredder : AtlasUniqueItem
    {
        public override int BossTier => 3;
        public override string Archetype => "Ranger";
        public override string Texture => "Terraria/Images/Item_" + ItemID.Megashark;
        public override string EffectText => "Grants +20% Flat Evasion Chance while actively firing.";
        public override string FlavorText => "A sandstorm compressed into a firing chamber.";

        public override void SafeSetDefaults() {
            Item.width = 60; Item.height = 24;
            Item.damage = 38;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 6; Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 16f;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            if (player.itemAnimation > 0) {
                player.GetModPlayer<AtlasProgressionPlayer>().EvasionChance += 0.20f;
            }
        }
    }

    // --- TIER 4: HEAVEN'S MANDATE ---
    public class HeavensMandate : AtlasUniqueItem
    {
        public override int BossTier => 4;
        public override string Archetype => "Ranger";
        public override string Texture => "Terraria/Images/Item_" + ItemID.HallowedRepeater;
        public override string EffectText => "Converts all Evasion Chance into Ranged Critical Strike Chance. Disables dodging.";
        public override string FlavorText => "Do not hide from judgment. Deliver it.";

        public override void SafeSetDefaults() {
            Item.width = 50; Item.height = 20;
            Item.damage = 55;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 16; Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.HolyArrow;
            Item.shootSpeed = 14f;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.EvasionChance > 0) {
                player.GetCritChance(DamageClass.Ranged) += (prog.EvasionChance * 100f);
                prog.EvasionChance = 0f;
            }
        }
    }

    // --- TIER 5: HAILSTORM ---
    public class Hailstorm : AtlasUniqueItem
    {
        public override int BossTier => 5;
        public override string Archetype => "Ranger";
        public override string Texture => "Terraria/Images/Item_" + ItemID.IceBow;
        public override string EffectText => "Bonus Movement Speed is added to Ranged Attack Speed.";
        public override string FlavorText => "Kinetic energy chilled to the breaking point.";

        public override void SafeSetDefaults() {
            Item.width = 32; Item.height = 46;
            Item.damage = 65;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 22; Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.FrostburnArrow;
            Item.shootSpeed = 18f;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            float speedBonus = player.moveSpeed - 1f;
            if (speedBonus > 0) {
                player.GetAttackSpeed(DamageClass.Ranged) += speedBonus;
            }
        }
    }

    // --- TIER 6: BRIMSTONE SALVO ---
    public class BrimstoneSalvo : AtlasUniqueItem
    {
        public override int BossTier => 6;
        public override string Archetype => "Ranger";
        public override string Texture => "Terraria/Images/Item_" + ItemID.HellwingBow;
        public override string EffectText => "Trades all Defense for an equal amount of flat Armor Penetration.";
        public override string FlavorText => "Hellfire seeks out the living.";

        public override void SafeSetDefaults() {
            Item.width = 30; Item.height = 44;
            Item.damage = 75;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 14; Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.HellfireArrow;
            Item.shootSpeed = 16f;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            int currentDef = player.statDefense;
            if (currentDef > 0) {
                player.GetArmorPenetration(DamageClass.Generic) += currentDef;
                player.statDefense -= currentDef;
            }
        }
    }

    // --- TIER 7: RAILGUN CORE ---
    public class RailgunCore : AtlasUniqueItem
    {
        public override int BossTier => 7;
        public override string Archetype => "Ranger";
        public override string Texture => "Terraria/Images/Item_" + ItemID.SniperRifle;
        public override string EffectText => "Horizontal velocity scales Ranged Damage.";
        public override string FlavorText => "Calibrated for high-speed evasion.";

        public override void SafeSetDefaults() {
            Item.width = 54; Item.height = 18;
            Item.damage = 220;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 32; Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.BulletHighVelocity;
            Item.shootSpeed = 24f;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            float velocityDmg = Math.Abs(player.velocity.X) * 0.05f;
            player.GetDamage(DamageClass.Ranged) += velocityDmg;
        }
    }

    // --- TIER 8: TOXIC PROLIFERATOR ---
    public class ToxicProliferator : AtlasUniqueItem
    {
        public override int BossTier => 8;
        public override string Archetype => "Ranger";
        public override string Texture => "Terraria/Images/Item_" + ItemID.Xenopopper;
        public override string EffectText => "Converts all Evasion Chance directly into Ranged Damage. Disables dodging.";
        public override string FlavorText => "There is nowhere left to run.";

        public override void SafeSetDefaults() {
            Item.width = 54; Item.height = 24;
            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 24; Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.ChlorophyteBullet;
            Item.shootSpeed = 14f;
            Item.value = Item.sellPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            if (prog.EvasionChance > 0) {
                player.GetDamage(DamageClass.Ranged) += prog.EvasionChance;
                prog.EvasionChance = 0f;
            }
        }
    }
}