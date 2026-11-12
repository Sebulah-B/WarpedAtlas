using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Projectiles;

namespace WarpedAtlas.Content.Items.Weapons
{
    public class WolfsHowl : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MoonCharm;

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 45;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            
            Item.useTime = 15;
            Item.useAnimation = 15;
            // HoldUp makes the werewolf raise their arm to the sky to "howl"
            Item.useStyle = ItemUseStyleID.HoldUp; 
            
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = true;
            
            Item.shoot = ModContent.ProjectileType<WolfsHowlMist>();
            Item.shootSpeed = 16f; // Fall speed
        }

        public override void HoldItem(Player player) {
            // Keeps the UI buff icon active
            player.AddBuff(BuffID.Werewolf, 2);
            
            // FORCES the physical visual transformation every frame the weapon is held
            player.wereWolf = true; 
            
            // Optional: grant passive stats specifically tied to holding this weapon
            player.moveSpeed += 0.15f; 
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            // 1. Play a subtle wolf howl sound effect
            SoundEngine.PlaySound(SoundID.Item4 with { Pitch = 0.2f, Volume = 0.8f }, player.Center);
            
            // 2. Define where the cursor is
            Vector2 target = Main.MouseWorld;
            
            // 3. Force the spawn position to be 600 pixels straight up in the sky above the cursor,
            // with a slight random horizontal variance so it looks like a scattered storm.
            Vector2 spawnPos = new Vector2(target.X + Main.rand.NextFloat(-60f, 60f), target.Y - 600f);
            
            // 4. Force the velocity to travel straight down
            Vector2 fallVelocity = new Vector2(0f, Item.shootSpeed);
            
            Projectile.NewProjectile(source, spawnPos, fallVelocity, type, damage, knockback, player.whoAmI);
            
            return false; // Prevent the default shooting behavior
        }
    }
}