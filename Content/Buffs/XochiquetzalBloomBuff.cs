using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Buffs
{
    public class XochiquetzalBloomBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.DryadsWard;
        public override void SetStaticDefaults() {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex) {
            player.GetDamage(DamageClass.Summon) += 0.35f;
            player.GetAttackSpeed(DamageClass.Summon) += 0.25f;
            if (Main.rand.NextBool(4)) {
                Dust d = Dust.NewDustDirect(player.position, player.width, player.height, DustID.JungleSpore, 0f, -1f, 100, default, 1.2f);
                d.noGravity = true;
            }
        }
    }
}