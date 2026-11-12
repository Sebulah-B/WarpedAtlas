using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Buffs;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Projectiles;

namespace WarpedAtlas.Content.Items.Weapons
{
    public class SeismicCry : ModItem
    {
        // Uses the vanilla Ancient Horn sprite
        public override string Texture => "Terraria/Images/Item_" + ItemID.AncientHorn;

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 85;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 14f; // Massive knockback to push hordes away
            
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.HoldUp; // Raises the horn in the air
            Item.noMelee = true; // The horn itself doesn't hit
            
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Orange;

            Item.shoot = ModContent.ProjectileType<SeismicShockwaveProjectile>();
            Item.shootSpeed = 0f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "WarcryDesc", "Unleashes a devastating seismic shout that damages and knocks back nearby foes.\nGrants 'Seismic Might' for 10 seconds, scaling Defense, Endurance, and Melee Damage per enemy hit.\n8 second cooldown.") {
                OverrideColor = new Color(255, 190, 110)
            });
        }

        // Block usage if the player has the Warcry Cooldown debuff
        public override bool CanUseItem(Player player) {
            return !player.HasBuff(ModContent.BuffType<WarcryCooldownDebuff>());
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            // 1. Layer the Roar and Explosion sounds for a heavy guttural shout
            SoundEngine.PlaySound(SoundID.Roar with { Pitch = -0.3f, Volume = 1.2f }, player.Center);
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, player.Center);

            // 2. Scan the 350-pixel blast zone to count nearby hostile threats
            int enemiesHit = 0;
            float scanRadius = 350f;
            for (int i = 0; i < Main.maxNPCs; i++) {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage) {
                    if (Vector2.Distance(player.Center, npc.Center) <= scanRadius) {
                        enemiesHit++;
                    }
                }
            }

            // Cap the power stacks at 10 so the player doesn't become immortal in massive swarms
            int finalStacks = Math.Clamp(enemiesHit, 1, 10);
            
            // 3. Store the stacks and apply the Buffs
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            prog.WarcryPowerStacks = finalStacks;

            player.AddBuff(ModContent.BuffType<SeismicMightBuff>(), 600);     // 10 Seconds of power
            player.AddBuff(ModContent.BuffType<WarcryCooldownDebuff>(), 480); // 8 Seconds cooldown

            // 4. Spawn the visual/damage shockwave centered on the player
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI);

            return false;
        }
    }
}