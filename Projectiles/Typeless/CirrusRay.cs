using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Typeless
{
    public class CirrusRay : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ray");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 5;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 0;
			Projectile.npcProj = true;
		}

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.2f, 0.01f, 0.1f);
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] % 6f == 0f && Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CirrusOrb>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }

            if (Projectile.ai[0] > 16f && Projectile.ai[0] % 2f == 0f)
            {
				Vector2 source = Projectile.position;
				int pink = Dust.NewDust(source, 1, 1, 234, 0f, 0f, 0, default, 1.25f);
				Main.dust[pink].noGravity = true;
				Main.dust[pink].noLight = true;
				Main.dust[pink].position = source;
				Main.dust[pink].scale = Main.rand.Next(70, 110) * 0.013f;
				Main.dust[pink].velocity *= 0.1f;
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
    }
}
