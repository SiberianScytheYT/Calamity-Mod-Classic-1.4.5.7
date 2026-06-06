using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class Ancient : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ancient");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.penetrate = 6;
            Projectile.extraUpdates = 6;
            Projectile.timeLeft = 151;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.6f, 0.5f, 0f);
            if (Projectile.timeLeft % 30 == 0)
            {
                int numProj = 2;
                int randomSpread = Main.rand.Next(3, 19);
                float rotation = MathHelper.ToRadians(randomSpread);
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < numProj + 1; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<Ancient2>(), (int)(Projectile.damage * 0.5), Projectile.knockBack * 0.5f, Projectile.owner, 0f, 0f);
                    }
                }
            }
            if (Projectile.timeLeft > 151)
            {
                Projectile.timeLeft = 151;
            }
            if (Projectile.ai[0] > 4f)
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
                        dust.scale *= 2f;
                        dust.velocity.X *= 6f;
                        dust.velocity.Y *= 6f;
                    }
                    else
                    {
                        dust.scale *= 1.5f;
                    }
                    dust.velocity.X *= 3f;
                    dust.velocity.Y *= 3f;
                    dust.scale *= num296;
                }
                for (int num298 = 0; num298 < 2; num298++)
                {
                    int num299 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, num297, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                    Dust dust = Main.dust[num299];
                    if (Main.rand.NextBool(3))
                    {
						dust.noGravity = true;
                        dust.scale *= 4f;
                        dust.velocity.X *= 4f;
                        dust.velocity.Y *= 4f;
                    }
                    else
                    {
                        dust.scale *= 2.5f;
                    }
                    dust.velocity.X *= 2f;
                    dust.velocity.Y *= 2f;
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
