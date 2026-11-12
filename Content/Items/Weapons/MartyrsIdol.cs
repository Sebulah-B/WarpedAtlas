using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Projectiles;

namespace WarpedAtlas.Content.Items.Weapons
{
    public class MartyrsIdol : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.ShadowJoustingLance; // Placeholder

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 250; // Massive base burst damage
            Item.DamageType = DamageClass.Summon;
            Item.mana = 20;
            
            Item.useTime = 25; 
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 8f;
            
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = true;
            
            Item.shoot = ModContent.ProjectileType<MartyrPayload>();
            Item.shootSpeed = 12f;
        }

        public override bool CanUseItem(Player player) {
            // Weapon can only be swung if the player has at least one active minion to sacrifice
            for (int i = 0; i < Main.maxProjectiles; i++) {
                if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].minion) {
                    return true;
                }
            }
            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile sacrificedMinion = null;
            float closestDist = 800f; // Cursor snap radius

            // 1. Prioritize the minion closest to the player's cursor
            for (int i = 0; i < Main.maxProjectiles; i++) {
                Projectile p = Main.projectile[i];
                if (p.active && p.owner == player.whoAmI && p.minion) {
                    float dist = Vector2.Distance(p.Center, Main.MouseWorld);
                    if (dist < closestDist) {
                        closestDist = dist;
                        sacrificedMinion = p;
                    }
                }
            }

            // Fallback: If no minion is near the cursor, sacrifice the one closest to the player
            if (sacrificedMinion == null) {
                float playerDist = float.MaxValue;
                for (int i = 0; i < Main.maxProjectiles; i++) {
                    Projectile p = Main.projectile[i];
                    if (p.active && p.owner == player.whoAmI && p.minion) {
                        float dist = Vector2.Distance(p.Center, player.Center);
                        if (dist < playerDist) {
                            playerDist = dist;
                            sacrificedMinion = p;
                        }
                    }
                }
            }

            if (sacrificedMinion != null) {
                // 2. Audio/Visual Sacrifice Feedback
                SoundEngine.PlaySound(SoundID.NPCDeath11 with { Pitch = 0.5f, Volume = 1.2f }, sacrificedMinion.Center);
                
                for (int i = 0; i < 20; i++) {
                    Vector2 dustVel = Main.rand.NextVector2CircularEdge(5f, 5f);
                    Dust.NewDustPerfect(sacrificedMinion.Center, DustID.Blood, dustVel, 0, default, 2f);
                }

                // 3. Vaporize the minion and spawn the payload in its exact place
                Vector2 spawnPos = sacrificedMinion.Center;
                sacrificedMinion.Kill();

                // Direct the payload initially toward the cursor
                Vector2 initialVelocity = (Main.MouseWorld - spawnPos).SafeNormalize(Vector2.UnitY) * Item.shootSpeed;
                Projectile.NewProjectile(source, spawnPos, initialVelocity, type, damage, knockback, player.whoAmI);
            }

            return false; // Prevent default shooting
        }
    }
}