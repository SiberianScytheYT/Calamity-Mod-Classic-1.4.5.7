using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Summon
{
    public class GammaBullet : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Enemy/NuclearBulletMedium";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gamma Bullet");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 0f;
            Projectile.alpha = 255;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.25f);
            Projectile.ai[1]++;
            if (Projectile.ai[1] <= 20f)
            {
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, Projectile.ai[1] / 20f);
            }
            if (Projectile.ai[1] % 10f == 9f)
            {
                for (int i = 0; i < 24; i++)
                {
                    float angle = MathHelper.TwoPi / 24f * i;
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + angle.ToRotationVector2().RotatedBy(Projectile.rotation) * new Vector2(7f, 4f), (int)CalamityDusts.SulfurousSeaAcid);
                    dust.scale = 0.9f;
                    dust.alpha = Projectile.alpha;
                    dust.noGravity = true;
                }
            }
            Projectile.velocity *= 1.03f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 240);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 2; i++)
            {
                int idx = Dust.NewDust(Projectile.position, 8, 8, (int)CalamityDusts.SulfurousSeaAcid, 0, 0, 0, default, 0.75f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
                idx = Dust.NewDust(Projectile.position, 8, 8, (int)CalamityDusts.SulfurousSeaAcid, 0, 0, 0, default, 0.75f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
            }
        }
    }
}
