using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class TerraBolt : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] >= 29f && Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[1] = 0f;
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<TerraOrb2>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner, 0f, 0f);
            }

			for (int num447 = 0; num447 < 2; num447++)
			{
				Vector2 vector33 = Projectile.position;
				vector33 -= Projectile.velocity * ((float)num447 * 0.25f);
				int num448 = Dust.NewDust(vector33, 1, 1, 107, 0f, 0f, 0, default, 1.25f);
				Main.dust[num448].position = vector33;
				Main.dust[num448].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[num448].velocity *= 0.1f;
			}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
        }
    }
}
