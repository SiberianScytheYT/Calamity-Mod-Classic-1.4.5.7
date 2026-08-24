using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class ExoFire : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public bool ProducedAcceleration = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fire");
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            float speedX = 1f;
            float speedY = 1f;
            if (!ProducedAcceleration)
            {
                speedX = Main.rand.NextBool(2) ? 1.03f : 0.97f;
                Projectile.velocity *= Utils.RandomVector2(Main.rand, 0.97f, 1.03f);
                ProducedAcceleration = true;
            }
            Projectile.velocity.X *= speedX;
            Projectile.velocity.X *= speedY;
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
                int num297 = Main.rand.NextBool(2) ? 107 : 234;
                if (Main.rand.NextBool(4))
                {
                    num297 = 269;
                }
                if (Main.rand.NextBool(2))
                {
                    for (int num298 = 0; num298 < 2; num298++)
                    {
                        int num299 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, num297, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 0.6f);
                        Dust dust = Main.dust[num299];
                        if (Main.rand.NextBool(3))
                        {
                            dust.scale *= 1.5f;
                            dust.velocity.X *= 1.2f;
                            dust.velocity.Y *= 1.2f;
                        }
                        else
                        {
                            dust.scale *= 0.75f;
                        }
                        dust.noGravity = true;
                        dust.velocity.X *= 0.8f;
                        dust.velocity.Y *= 0.8f;
                        dust.scale *= num296;
                        dust.velocity += Projectile.velocity;
                    }
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            Projectile.rotation += 0.3f * (float)Projectile.direction;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.ExoDebuffs();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			target.ExoDebuffs();
        }
    }
}
