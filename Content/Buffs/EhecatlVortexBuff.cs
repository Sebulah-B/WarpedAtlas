using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Buffs
{
   public class EhecatlVortexBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Swiftness;
        public override void SetStaticDefaults() {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex) {
            player.moveSpeed += 0.30f;
            player.endurance += 0.12f;
            player.noKnockback = true;
            player.noFallDmg = true;
            if (player.buffTime[buffIndex] == 1) {
                player.AddBuff(ModContent.BuffType<FrigidGlimmerCooldownDebuff>(), 25 * 60);
            }
        }
    }
}