using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Boss
{
    public class JewelProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ruby Bolt");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.penetrate = 2;
			Projectile.hostile = true;
        }

        public override void AI()
        {
            Projectile.rotation += 0.3f * (float)Projectile.direction;
			for (int index = 0; index < 2; ++index)
			{
				int ruby = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 90, Projectile.velocity.X, Projectile.velocity.Y, 90, new Color(), 1.2f);
				Dust dust = Main.dust[ruby];
				dust.noGravity = true;
				dust.velocity *= 0.3f;
			}
        }

        public override Color? GetAlpha(Color drawColor) => new Color(255, 50, 50, 0);

        public override void OnKill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			for (int index1 = 0; index1 < 15; ++index1)
			{
				int ruby = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 90, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 50, new Color(), 1.2f);
				Dust dust = Main.dust[ruby];
				dust.noGravity = true;
				dust.scale *= 1.25f;
				dust.velocity *= 0.5f;
			}
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
