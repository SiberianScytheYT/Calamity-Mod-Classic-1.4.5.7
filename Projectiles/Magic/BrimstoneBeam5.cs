using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class BrimstoneBeam5 : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Beam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 30;
        }

        public override void AI()
        {
            if (Projectile.velocity.X != Projectile.velocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -Projectile.velocity.X;
            }
            if (Projectile.velocity.Y != Projectile.velocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -Projectile.velocity.Y;
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                for (int num447 = 0; num447 < 1; num447++)
                {
                    Vector2 vector33 = Projectile.position;
                    vector33 -= Projectile.velocity * ((float)num447 * 0.25f);
                    Projectile.alpha = 255;
                    int num249 = 235;
                    int num448 = Dust.NewDust(vector33, 1, 1, num249, 0f, 0f, 0, default, 1.5f);
                    Main.dust[num448].position = vector33;
                    Main.dust[num448].velocity *= 0.1f;
                    Main.dust[num448].noGravity = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
        }
    }
}
