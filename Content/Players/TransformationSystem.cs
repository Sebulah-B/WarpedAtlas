using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Players
{
    public enum TransformationForm
    {
        None,
        BluePaladin
    }

    public class TransformationPlayer : ModPlayer
    {
        public TransformationForm ActiveTransform = TransformationForm.None;

        public override void ResetEffects() {
            ActiveTransform = TransformationForm.None;
        }

        // ==========================================
        // HIDE VANILLA PLAYER SPRITE & WEAPON
        // ==========================================
        public override void HideDrawLayers(PlayerDrawSet drawInfo) {
            if (ActiveTransform == TransformationForm.BluePaladin) {
                foreach (var layer in PlayerDrawLayerLoader.Layers) {
                    
                    // FIX 1: We MUST exempt HeldItem from being hidden. 
                    // If we hide it, our Paladin layer gets destroyed because it is anchored to it.
                    if (layer != PlayerDrawLayers.HeldItem && 
                        layer.Name != "PaladinTransformationLayer") 
                    {
                        layer.Hide();
                    }
                }
            }
        }

        // FIX 2: Visually delete the weapon during the draw phase so it doesn't render over the Paladin
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
            if (ActiveTransform == TransformationForm.BluePaladin) {
                // Replaces the drawn weapon with thin air, without breaking the underlying attack logic
                drawInfo.heldItem = new Item(); 
            }
        }

        // ==========================================
        // OVERWRITE HELD WEAPON MECHANICS
        // ==========================================
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (ActiveTransform == TransformationForm.BluePaladin) {
                return false; // Cancels standard bows, guns, and magic staves
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }

        public override bool? CanHitNPCWithItem(Item item, NPC target) {
            if (ActiveTransform == TransformationForm.BluePaladin) {
                return false; // Cancels the base weapon damage from hitting the enemy twice
            }
            return base.CanHitNPCWithItem(item, target);
        }

        public override void PostUpdate() {
            if (ActiveTransform == TransformationForm.BluePaladin) {
                // Triggers exactly once per attack cycle (on the very first frame of the swing/cast)
                if (Player.itemAnimation > 0 && Player.itemAnimation == Player.itemAnimationMax - 1) {
                    UnleashCrashingWave();
                }
            }
        }

        private void UnleashCrashingWave() {
            if (Main.myPlayer != Player.whoAmI) return;

            SoundEngine.PlaySound(SoundID.Item101, Player.Center);
            int damage = 50 + (Player.statDefense * 2); 

            // Calculate the angle from the player to the cursor
            Vector2 aimDir = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.UnitX);

            // FIX 3: Deerclops spikes cannot fly (they snap to the ground horizontally). 
            // Switched to FrostBlastFriendly so a 5-spread cone shoots accurately at the cursor!
            for (int i = -2; i <= 2; i++) {
                Vector2 velocity = aimDir.RotatedBy(MathHelper.ToRadians(12 * i)) * 14f;
                
                int proj = Projectile.NewProjectile(
                    Player.GetSource_FromThis(), 
                    Player.Center, 
                    velocity, 
                    ProjectileID.FrostBlastFriendly, 
                    damage, 
                    8f, 
                    Player.whoAmI
                );

                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].DamageType = DamageClass.Melee;
            }
        }
    }

    // ==========================================
    // CUSTOM DRAW LAYER (BLUE/WHITE TINT OVERLAY)
    // ==========================================
    public class PaladinTransformationLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HeldItem);
        
        protected override void Draw(ref PlayerDrawSet drawInfo) {
            if (drawInfo.drawPlayer.GetModPlayer<TransformationPlayer>().ActiveTransform != TransformationForm.BluePaladin) 
                return;

            Main.instance.LoadNPC(NPCID.Paladin);
            Texture2D texture = TextureAssets.Npc[NPCID.Paladin].Value;

            int totalFrames = Main.npcFrameCount[NPCID.Paladin]; 
            int frameHeight = texture.Height / totalFrames;
            int frameIndex = 0;

            if (drawInfo.drawPlayer.itemAnimation > 0) {
                float progress = 1f - ((float)drawInfo.drawPlayer.itemAnimation / drawInfo.drawPlayer.itemAnimationMax);
                frameIndex = 8 + (int)(progress * 6);
                if (frameIndex > 14) frameIndex = 14;
            } 
            else if (drawInfo.drawPlayer.velocity.Y != 0) {
                frameIndex = 8; 
            } 
            else if (Math.Abs(drawInfo.drawPlayer.velocity.X) > 0.1f) {
                frameIndex = (int)(Main.GlobalTimeWrappedHourly * 15f) % 8;
            } 
            else {
                frameIndex = 0; 
            }

            Rectangle sourceRect = new Rectangle(0, frameIndex * frameHeight, texture.Width, frameHeight);

            Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawInfo.drawPlayer.width / 2f, drawInfo.drawPlayer.height);
            drawPos.Y -= frameHeight / 2f; 
            drawPos.Y += 8f; 

            // Applying a pure, highly visible Ice-Blue/White tint
            Color iceTint = new Color(180, 230, 255, 255);

            DrawData drawData = new DrawData(
                texture,
                new Vector2((int)drawPos.X, (int)drawPos.Y),
                sourceRect,
                iceTint, 
                drawInfo.drawPlayer.fullRotation,
                new Vector2(texture.Width / 2f, frameHeight / 2f),
                1f,
                drawInfo.playerEffect, 
                0
            );

            drawInfo.DrawDataCache.Add(drawData);
        }
    }
}