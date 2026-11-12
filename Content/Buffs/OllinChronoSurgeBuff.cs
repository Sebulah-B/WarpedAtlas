using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Buffs
{
    public class OllinChronoSurgeBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Archery;
        public override void SetStaticDefaults() {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex) {
            player.GetAttackSpeed(DamageClass.Ranged) += 0.30f;
            player.moveSpeed += 0.15f;
        }
    }
}