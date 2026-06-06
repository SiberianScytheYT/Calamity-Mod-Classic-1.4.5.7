using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class ProBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            //Rotation
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi) + MathHelper.ToRadians(90) *Projectile.direction;

            Lighting.AddLight(Projectile.Center, new Vector3(158, 240, 240) * (1.5f / 255));
            for (int num134 = 0; num134 < 6; num134++)
            {
                Vector2 dspeed = -Projectile.velocity * 0.8f;
                float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)num134;
                float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)num134;
                int num135 = Dust.NewDust(new Vector2(x, y), 1, 1, 160, 0f, 0f, 0, default, 1.25f);
                Main.dust[num135].alpha = Projectile.alpha;
                Main.dust[num135].position.X = x;
                Main.dust[num135].position.Y = y;
                Main.dust[num135].velocity = dspeed;
                Main.dust[num135].noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.ai[0] += 0.1f;
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 160, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f); //206 160 226
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 7;
        }
    }
}
