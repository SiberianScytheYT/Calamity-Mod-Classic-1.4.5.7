using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
namespace CalRD.Projectiles.Summon
{
	public class SquirrelSquireAcorn : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Acorn");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 1;
        }

        public override void AI()
        {
            Projectile.velocity.X *= 0.9995f;
            Projectile.velocity.Y = Projectile.velocity.Y + 0.01f;
            Projectile.rotation += MathHelper.ToRadians(180) * Projectile.direction;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 7, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
