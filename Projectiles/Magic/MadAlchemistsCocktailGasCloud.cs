using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class MadAlchemistsCocktailGasCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mad Alchemist's Cocktail Gas Cloud");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void AI()
        {
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 60f)
            {
                Projectile.ai[0] += 10f;
            }
            if (Projectile.ai[0] > 255f)
            {
                Projectile.Kill();
                Projectile.ai[0] = 255f;
            }
            Projectile.alpha = (int)(100.0 + (double)Projectile.ai[0] * 0.7);
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            Projectile.rotation += (float)Projectile.direction * 0.003f;
            Projectile.velocity *= 0.96f;

            Rectangle cloudHitbox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
				Projectile otherProj = Main.projectile[i];
				// Short circuits to make the loop as fast as possible
				if (!otherProj.active || otherProj.owner != Projectile.owner || !otherProj.minion || i == Projectile.whoAmI)
					continue;

                if (otherProj.type == Projectile.type)
                {
                    Rectangle otherProjHitbox = new Rectangle((int)otherProj.position.X, (int)otherProj.position.Y, otherProj.width, otherProj.height);
                    if (cloudHitbox.Intersects(otherProjHitbox))
                    {
                        Vector2 projDistance = otherProj.Center - Projectile.Center;
                        if (projDistance.X == 0f && projDistance.Y == 0f)
                        {
                            if (i < Projectile.whoAmI)
                            {
                                projDistance.X = -1f;
                                projDistance.Y = 1f;
                            }
                            else
                            {
                                projDistance.X = 1f;
                                projDistance.Y = -1f;
                            }
                        }
                        projDistance.Normalize();
                        projDistance *= 0.005f;
                        Projectile.velocity -= projDistance;
                        otherProj.velocity += projDistance;
                    }
                }
            }
        }
    }
}
