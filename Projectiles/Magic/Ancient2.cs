using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class Ancient2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ancient");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 4;
            Projectile.extraUpdates = 12;
            Projectile.timeLeft = 30;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.3f, 0.25f, 0f);
            if (Projectile.timeLeft > 30)
            {
                Projectile.timeLeft = 30;
            }
            if (Projectile.ai[0] > 7f)
            {
                float num296 = 1f;
                if (Projectile.ai[0] == 8f)
                {
                    num296 = 0.25f;
                }
                else if (Projectile.ai[0] == 9f)
                {
                    num296 = 0.5f;
                }
                else if (Projectile.ai[0] == 10f)
                {
                    num296 = 0.75f;
                }
                Projectile.ai[0] += 1f;
                int num297 = 32;
                for (int num298 = 0; num298 < 2; num298++)
                {
                    int num299 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, num297, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                    Dust dust = Main.dust[num299];
                    if (Main.rand.NextBool(2))
                    {
                        dust.noGravity = true;
                        dust.scale *= 1.5f;
                        dust.velocity.X *= 3f;
                        dust.velocity.Y *= 3f;
                    }
                    dust.velocity.X *= 2f;
                    dust.velocity.Y *= 2f;
                    dust.scale *= num296;
                }
                for (int num298 = 0; num298 < 2; num298++)
                {
                    int num299 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, num297, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                    Dust dust = Main.dust[num299];
                    if (Main.rand.NextBool(3))
                    {
                        dust.noGravity = true;
                        dust.scale *= 3f;
                        dust.velocity.X *= 2f;
                        dust.velocity.Y *= 2f;
                    }
                    else
                    {
                        dust.scale *= 2f;
                    }
                    dust.velocity.X *= 1.2f;
                    dust.velocity.Y *= 1.2f;
                    dust.scale *= num296;
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            Projectile.rotation += 0.3f * (float)Projectile.direction;
        }
    }
}
