using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class LuminousShard : ModProjectile
    {
		bool gravity = false;

    	public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Stardust Shard");
		}

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.Calamity().rogue = true;
		}

		public override void AI()
        {
			if (Projectile.ai[0] == 0f && Projectile.velocity.X == 0f && Projectile.velocity.Y == -2f)
				gravity = true;

			Projectile.ai[0] = 1f;
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

			if (gravity)
				Projectile.velocity.Y *= 1.05f;

		}

		public override void OnKill(int timeLeft)
        {
			for (int i = 0; i <= 2; i++)
        	{
				int num195 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 176, Projectile.oldVelocity.X / 4, Projectile.oldVelocity.Y / 4, 0, default, 0.75f);
				Main.dust[num195].noGravity = true;
				Main.dust[num195].velocity *= 3f;
				num195 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 177, Projectile.oldVelocity.X / 4, Projectile.oldVelocity.Y / 4, 0, default, 0.75f);
				Main.dust[num195].noGravity = true;
				Main.dust[num195].velocity *= 3f;
			}
		}
    }
}
