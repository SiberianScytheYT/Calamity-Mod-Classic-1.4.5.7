using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Typeless
{
	public class XerocBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int num468 = 0; num468 < 1; num468++)
                {
                    int num469 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 62, 0f, 0f, 100, default, 1f);
                    Main.dust[num469].noGravity = true;
                    Main.dust[num469].velocity *= 0f;
                }
            }
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 400f, 8f, 20f);
        }
    }
}
