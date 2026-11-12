using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Items.Uniques
{
    // --- TIER 1: THE LEVIATHAN ---
    public class TheLeviathan : AtlasUniqueItem
    {
        public override int BossTier => 1;
        public override string Archetype => "Warrior";
        public override string Texture => "Terraria/Images/Item_" + ItemID.BreakerBlade;
        public override string EffectText => "+50 Max Life and +10% Damage Reduction while held.\nHits unleash a blood nova dealing your Max Life to nearby enemies.";
        public override string FlavorText => "Let the earth drink your vitality, and return it as wrath.";

        public override void SafeSetDefaults() {
            Item.width = 60; Item.height = 60;
            Item.damage = 85;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 45; Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 10f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.scale = 2.2f;
            Item.crit = 20;
        }

        public override void HoldItem(Player player) {
            player.statLifeMax2 += 50;
            player.endurance += 0.10f;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.NextBool(2)) {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Blood, 0f, 0f, 100, default, 1.5f);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            SoundEngine.PlaySound(SoundID.Item14, target.Center);
            for (int i = 0; i < 40; i++) {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                Dust.NewDust(target.position, target.width, target.height, DustID.Blood, velocity.X, velocity.Y, 0, default, 2.5f);
            }

            int radius = 150;
            int novaDamage = player.statLifeMax2;
            for (int i = 0; i < Main.maxNPCs; i++) {
                NPC n = Main.npc[i];
                if (n.active && !n.friendly && n.lifeMax > 5 && n.Distance(target.Center) < radius && n.whoAmI != target.whoAmI) {
                    n.SimpleStrikeNPC(novaDamage, n.Center.X < target.Center.X ? -1 : 1);
                }
            }
        }
    }

    // --- TIER 2: FAULTLINE ---
    public class Faultline : AtlasUniqueItem
    {
        public override int BossTier => 2;
        public override string Archetype => "Warrior";
        public override string Texture => "Terraria/Images/Item_" + ItemID.PaladinsHammer;
        public override string EffectText => "Critical strikes trigger seismic shockwaves dealing 400% of your Defense.";
        public override string FlavorText => "The mountain does not yield. It shatters.";

        public override void SafeSetDefaults() {
            Item.width = 40; Item.height = 40;
            Item.damage = 60;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 35; Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 0f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.crit = 15;
            Item.scale = 1.5f;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.NextBool(2)) {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Stone, 0f, 0f, 100, default, 1.5f);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            if (hit.Crit) {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, target.Center);
                for (int i = 0; i < 20; i++) {
                    Dust.NewDust(target.position, target.width, target.height, DustID.Stone, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 0, default, 2f);
                }

                int shockwaveRadius = 120;
                int waveDamage = player.statDefense * 4;
                for (int i = 0; i < Main.maxNPCs; i++) {
                    NPC n = Main.npc[i];
                    if (n.active && !n.friendly && n.lifeMax > 5 && n.Distance(target.Center) < shockwaveRadius && n.whoAmI != target.whoAmI) {
                        n.SimpleStrikeNPC(waveDamage, n.Center.X < target.Center.X ? -1 : 1);
                    }
                }
            }
        }
    }

    // --- TIER 3: SUNSCORCHED KHOPESH ---
    public class SunscorchedKhopesh : AtlasUniqueItem
    {
        public override int BossTier => 3;
        public override string Archetype => "Warrior";
        public override string Texture => "Terraria/Images/Item_" + ItemID.Cutlass;
        public override string EffectText => "Halves your Defense, granting +1% Melee Damage per point of Defense lost.";
        public override string FlavorText => "Leave your armor in the dunes. Strike with the heat of the sun.";

        public override void SafeSetDefaults() {
            Item.width = 44; Item.height = 48;
            Item.damage = 75;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 18; Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
            Item.useTurn = false;
        }

        public override void HoldItem(Player player) {
            if (player.statDefense > 0) {
                int lostDefense = player.statDefense / 2;
                player.statDefense -= lostDefense;
                player.GetDamage(DamageClass.Melee) += (lostDefense * 0.01f);
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.NextBool(2)) {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Sand, 0f, 0f, 50, default, 1.4f);
            }
        }
    }

    // --- TIER 4: PRISMATIC EDGE ---
    public class PrismaticEdge : AtlasUniqueItem
    {
        public override int BossTier => 4;
        public override string Archetype => "Warrior";
        public override string Texture => "Terraria/Images/Item_" + ItemID.Keybrand;
        public override string EffectText => "Converts bonus Melee Attack Speed into Life Regeneration.";
        public override string FlavorText => "Refracts the wielder's vitality into blinding light.";

        public override void SafeSetDefaults() {
            Item.width = 50; Item.height = 50;
            Item.damage = 110;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 20; Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.FairyQueenMagicItemShot;
            Item.shootSpeed = 12f;
        }

        public override void HoldItem(Player player) {
            float speedBonus = player.GetAttackSpeed(DamageClass.Melee) - 1f;
            if (speedBonus > 0) {
                player.lifeRegen += (int)(speedBonus * 20);
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.NextBool(3)) {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.RainbowMk2, 0f, 0f, 0, Main.DiscoColor, 1.5f);
                Main.dust[dust].noGravity = true;
            }
        }
    }

    // --- TIER 5: AVALANCHE ---
    public class Avalanche : AtlasUniqueItem
    {
        public override int BossTier => 5;
        public override string Archetype => "Warrior";
        public override string Texture => "Terraria/Images/Item_" + ItemID.IceBlade;
        public override string EffectText => "Grants +1% Melee Damage for every point of Defense.";
        public override string FlavorText => "An unstoppable force born of immovable stone.";

        public override void SafeSetDefaults() {
            Item.width = 54; Item.height = 54;
            Item.damage = 130;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 40; Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 12f;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.scale = 1.8f;
        }

        public override void HoldItem(Player player) {
            if (player.statDefense > 0) {
                player.GetDamage(DamageClass.Melee) += (player.statDefense * 0.01f);
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.NextBool(2)) {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Ice, 0f, 0f, 100, Color.LightCyan, 1.5f);
            }
        }
    }

    // --- TIER 6: CRUCIBLE'S WRATH ---
    public class CruciblesWrath : AtlasUniqueItem
    {
        public override int BossTier => 6;
        public override string Archetype => "Warrior";
        public override string Texture => "Terraria/Images/Item_" + ItemID.FieryGreatsword;
        public override string EffectText => "Ignites the wielder to grant +20% Damage Reduction and +40% Melee Damage.";
        public override string FlavorText => "To hold it is to burn. To swing it is to incinerate.";

        public override void SafeSetDefaults() {
            Item.width = 48; Item.height = 48;
            Item.damage = 110;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 25; Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
            Item.scale = 1.5f;
        }

        public override void HoldItem(Player player) {
            player.AddBuff(BuffID.OnFire, 2);
            player.endurance += 0.20f;
            player.GetDamage(DamageClass.Melee) += 0.40f;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.NextBool(2)) {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Lava, 0f, 0f, 100, default, 2f);
            }
        }
    }

    // --- TIER 7: KINETIC CRUSHER ---
    public class KineticCrusher : AtlasUniqueItem
    {
        public override int BossTier => 7;
        public override string Archetype => "Warrior";
        public override string Texture => "Terraria/Images/Item_" + ItemID.PaladinsHammer;
        public override string EffectText => "Converts horizontal momentum directly into bonus Defense.";
        public override string FlavorText => "Velocity is the ultimate armor.";

        public override void SafeSetDefaults() {
            Item.width = 40; Item.height = 40;
            Item.damage = 150;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 16; Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 9f;
            Item.shoot = ProjectileID.PaladinsHammerFriendly;
            Item.shootSpeed = 15f;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            int velocityDef = (int)(Math.Abs(player.velocity.X) * 4f);
            player.statDefense += velocityDef;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.NextBool(2)) {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Granite, 0f, 0f, 100, default, 1.5f);
            }
        }
    }

    // --- TIER 8: MYCELIAL GLAIVE ---
    public class MycelialGlaive : AtlasUniqueItem
    {
        public override int BossTier => 8;
        public override string Archetype => "Warrior";
        public override string Texture => "Terraria/Images/Item_" + ItemID.MushroomSpear;
        public override string EffectText => "Halves your Max Life, but grants +40 Life Regeneration.";
        public override string FlavorText => "It feeds on you, but keeps you alive to spread its spores.";

        public override void SafeSetDefaults() {
            Item.width = 50; Item.height = 50;
            Item.damage = 180;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 22; Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 8f;
            Item.shoot = ProjectileID.MushroomSpear;
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            player.statLifeMax2 /= 2;
            player.lifeRegen += 40;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.NextBool(2)) {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.CorruptionThorns, 0f, 0f, 0, Color.Black, 1.5f);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}