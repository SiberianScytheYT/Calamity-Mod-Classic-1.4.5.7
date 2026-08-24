using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Hybrid
{
    public class CorpusAvertorClone : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/CorpusAvertor";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Corpus Avertor");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 1f)
            {
                Projectile.Calamity().rogue = true;
            }

            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.04f;

            Projectile.velocity.X *= 1.005f;
            Projectile.velocity.Y *= 1.005f;

            switch ((int)Projectile.ai[0])
            {
                case 20:
                    Projectile.scale = 0.7f;
                    break;
                case 40:
                    Projectile.scale = 0.8f;
                    break;
                case 60:
                    Projectile.scale = 0.9f;
                    break;
                default:
                    break;
            }
            Projectile.width = Projectile.height = (int)(24f * Projectile.scale);

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, true, 300f, 16f, 20f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                return new Color((int)(150f * ((float)Projectile.timeLeft / 85f)), 0, 0, Projectile.timeLeft / 5 * 3);
            }
            return new Color(150, 0, 0, 50);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type == NPCID.TargetDummy)
                return;

            float heal = (float)Projectile.damage * 0.05f;
            if ((int)heal == 0)
                return;
            if (Main.player[Main.myPlayer].lifeSteal <= 0f)
                return;

			CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ProjectileID.VampireHeal, 1200f, 1.5f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            float heal = (float)info.Damage * 0.05f;
            if ((int)heal == 0)
                return;
            if (Main.player[Main.myPlayer].lifeSteal <= 0f)
                return;

			CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ProjectileID.VampireHeal, 1200f, 1.5f);
        }
    }
}
