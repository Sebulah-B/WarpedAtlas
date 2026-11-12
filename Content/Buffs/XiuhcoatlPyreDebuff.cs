using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarpedAtlas.Content.Buffs
{
    public class XiuhcoatlPyreDebuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.OnFire3;
        public override void SetStaticDefaults() {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex) {
            player.GetDamage(DamageClass.Magic) += 0.25f;
            if (Main.rand.NextBool(2)) {
                Dust d = Dust.NewDustDirect(player.position, player.width, player.height, DustID.SolarFlare, 0f, -2f, 100, default, 1.4f);
                d.noGravity = true;
            }
        }
    }
}