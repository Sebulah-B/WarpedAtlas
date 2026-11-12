using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;

namespace WarpedAtlas.Content.Buffs
{
    // --- THE COOLDOWN DEBUFF ---
    public class WarcryCooldownDebuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Silenced;

        public override void SetStaticDefaults() {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; // Prevents cheesing cooldowns
        }
    }

    // --- THE SCALING WARCRY BUFF ---
    public class SeismicMightBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Wrath;

        public override void SetStaticDefaults() {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();
            
            // Ensure a baseline of at least 1 stack even if shouting at thin air
            int stacks = prog.WarcryPowerStacks > 0 ? prog.WarcryPowerStacks : 1;

            // Grant +5 Defense, +2% Damage Reduction, and +8% Melee Damage per enemy hit!
            player.statDefense += (stacks * 5);
            player.endurance += (stacks * 0.02f);
            player.GetDamage(DamageClass.Melee) += (stacks * 0.08f);

            // Emit subtle earth/fire particles from the player while buffed
            if (Main.rand.NextBool(4)) {
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.SolarFlare, 0f, -2f, 100, default, 1.2f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
            var prog = Main.LocalPlayer.GetModPlayer<AtlasProgressionPlayer>();
            int stacks = prog.WarcryPowerStacks > 0 ? prog.WarcryPowerStacks : 1;
            
            buffName = $"Seismic Might ({stacks}x Power)";
            tip = $"Empowering your strikes from the fury of {stacks} nearby foes.\n" +
                  $"+{stacks * 5} Defense\n" +
                  $"+{stacks * 2}% Damage Reduction\n" +
                  $"+{stacks * 8}% Melee Damage";
            rare = ItemRarityID.Orange;
        }
    }
}