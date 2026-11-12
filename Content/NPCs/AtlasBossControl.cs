using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;
using WarpedAtlas.Content.Subworlds;
using WarpedAtlas.Content.UI;

namespace WarpedAtlas.Content.NPCs
{
    public class AtlasBossControl : GlobalNPC
    {
        public override void SetDefaults(NPC npc) {
            if (npc.ModNPC is ApexBossBase) {
                npc.BossBar = ModContent.GetInstance<AtlasBossBar>();
                npc.lavaImmune = true;
                npc.trapImmune = true;
                npc.knockBackResist = 0f;
            }
        }

        public override bool PreAI(NPC npc) {
            if (npc.ModNPC is ApexBossBase boss && SubworldSystem.IsActive<BossDomainSubworld>()) {
                // Keep all Apex Bosses strictly clamped inside the 800x800 Arena Box
                int centerX = 400 * 16;
                int centerY = 400 * 16;
                float maxOffsetX = 56 * 16f;
                float maxOffsetY = 36 * 16f;

                if (npc.Center.X < centerX - maxOffsetX) {
                    npc.position.X = centerX - maxOffsetX - npc.width * 0.5f;
                    if (npc.velocity.X < 0) npc.velocity.X *= -0.5f;
                }
                if (npc.Center.X > centerX + maxOffsetX) {
                    npc.position.X = centerX + maxOffsetX - npc.width * 0.5f;
                    if (npc.velocity.X > 0) npc.velocity.X *= -0.5f;
                }
                if (npc.Center.Y < centerY - maxOffsetY) {
                    npc.position.Y = centerY - maxOffsetY - npc.height * 0.5f;
                    if (npc.velocity.Y < 0) npc.velocity.Y *= -0.5f;
                }
                if (npc.Center.Y > centerY + maxOffsetY) {
                    npc.position.Y = centerY + maxOffsetY - npc.height * 0.5f;
                    if (npc.velocity.Y > 0) npc.velocity.Y = 0f;
                }
            }
            return base.PreAI(npc);
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers) {
            if (npc.ModNPC is ApexBossBase boss) {
                // Enrage mitigation below 50% HP: +15% Damage Reduction
                if (npc.life < npc.lifeMax * 0.5f) {
                    modifiers.FinalDamage *= 0.85f;
                }
            }
        }
    }
}