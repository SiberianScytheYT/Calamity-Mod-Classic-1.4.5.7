using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Melee
{
    public class Dark : ModProjectile
    {
		private const int speedTimerMax = 60;
        private int speedTimer = speedTimerMax;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dark");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.rotation += 0.5f;
            speedTimer--;
			if (speedTimer <= 0)
			{
				speedTimer = speedTimerMax;
				Projectile.velocity *= -1f;
			}
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }
    }
}
