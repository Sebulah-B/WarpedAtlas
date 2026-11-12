using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WarpedAtlas.Content.Players;
using WarpedAtlas.Content.Projectiles;

namespace WarpedAtlas.Content.Systems
{
    public class AscendancyGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool IsTemporalEcho = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source) {
            if (IsTemporalEcho || projectile.owner != Main.myPlayer) return;

            Player player = Main.player[projectile.owner];
            if (player == null || !player.active) return;
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();

            // RANGER OF TIME (Ollin): Fire an additional projectile in a volley on ranged weapon use
            if (prog.Asc_TimeRanger && source is EntitySource_ItemUse && projectile.friendly &&
                projectile.DamageType == DamageClass.Ranged && !projectile.minion && !projectile.sentry) {
                Vector2 echoVel = projectile.velocity.RotatedBy(Main.rand.NextBool() ? 0.08f : -0.08f) * 1.05f;
                int pIdx = Projectile.NewProjectile(
                    player.GetSource_Misc("OllinEcho"),
                    projectile.Center,
                    echoVel,
                    projectile.type,
                    (int)(projectile.damage * 0.75f),
                    projectile.knockBack,
                    projectile.owner
                );
                if (pIdx >= 0 && pIdx < Main.maxProjectiles) {
                    Main.projectile[pIdx].GetGlobalProjectile<AscendancyGlobalProjectile>().IsTemporalEcho = true;
                }
                for (int i = 0; i < 8; i++) {
                    Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.GoldFlame, echoVel.X * 0.3f, echoVel.Y * 0.3f, 100, default, 1.3f);
                    d.noGravity = true;
                }
            }

            // RANGER EXTRACTION TOOLKIT BRANCH: Extra Trap Volley when throwing Shrapnel Traps
            if (prog.Toolkit_ExtraTraps && source is EntitySource_ItemUse &&
                projectile.type == ModContent.ProjectileType<ShrapnelTrapProjectile>()) {
                for (int i = -1; i <= 1; i += 2) {
                    int pIdx = Projectile.NewProjectile(
                        player.GetSource_Misc("TrapVolley"),
                        projectile.Center,
                        projectile.velocity.RotatedBy(i * 0.18f),
                        projectile.type,
                        (int)(projectile.damage * prog.ToolkitDamageMult),
                        projectile.knockBack,
                        projectile.owner
                    );
                    if (pIdx >= 0 && pIdx < Main.maxProjectiles) {
                        Main.projectile[pIdx].GetGlobalProjectile<AscendancyGlobalProjectile>().IsTemporalEcho = true;
                    }
                }
            }
        }

        public override void OnKill(Projectile projectile, int timeLeft) {
            if (projectile.owner != Main.myPlayer) return;
            Player player = Main.player[projectile.owner];
            if (player == null || !player.active) return;
            var prog = player.GetModPlayer<AtlasProgressionPlayer>();

            // FIRE RANGER (Xiuhtecuhtli) OR TRAPPER KEYSTONE: Shrapnel Trap secondary blast + 4 cluster bombs
            if ((prog.Asc_FireTrapper || prog.Toolkit_TrapCluster) && projectile.type == ModContent.ProjectileType<ShrapnelTrapProjectile>()) {
                SoundEngine.PlaySound(SoundID.Item14, projectile.Center);
                for (int d = 0; d < 35; d++) {
                    Vector2 v = Main.rand.NextVector2Circular(9f, 9f);
                    Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(16, 16), 32, 32, DustID.SolarFlare, v.X, v.Y, 100, default, 2.0f);
                    dust.noGravity = true;
                }

                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    projectile.Center,
                    Vector2.Zero,
                    ProjectileID.SolarWhipSwordExplosion,
                    projectile.damage,
                    6f,
                    projectile.owner,
                    0f,
                    1.2f
                );

                for (int i = 0; i < 4; i++) {
                    Vector2 clusterVel = new Vector2((i - 1.5f) * 3.8f, Main.rand.NextFloat(-7.5f, -4.5f));
                    int p = Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        projectile.Center,
                        clusterVel,
                        ProjectileID.MolotovFire,
                        (int)(projectile.damage * 0.65f),
                        4f,
                        projectile.owner
                    );
                    if (p >= 0 && p < Main.maxProjectiles) {
                        Main.projectile[p].DamageType = DamageClass.Ranged;
                        Main.projectile[p].friendly = true;
                        Main.projectile[p].hostile = false;
                    }
                }
            }
        }
    }
}