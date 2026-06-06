using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class T1000Laser : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/LaserProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Laser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0f, 0.7f, 0.1f);
            float num55 = 100f;
            float num56 = 3f;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.localAI[0] += num56;
                if (Projectile.localAI[0] > num55)
                {
                    Projectile.localAI[0] = num55;
                }
            }
            else
            {
                Projectile.localAI[0] -= num56;
                if (Projectile.localAI[0] <= 0f)
                {
                    Projectile.Kill();
                }
            }
        }

        public override Color? GetAlpha(Color lightColor) => new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);

        public override bool PreDraw(ref Color lightColor) => Projectile.DrawBeam(100f, 3f, lightColor);

        public override void OnKill(int timeLeft)
        {
            int dustAmt = Main.rand.Next(3, 7);
            for (int d = 0; d < dustAmt; d++)
            {
                int rainbow = Dust.NewDust(Projectile.Center - Projectile.velocity / 2f, 0, 0, 66, 0f, 0f, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 2.1f);
                Main.dust[rainbow].velocity *= 2f;
                Main.dust[rainbow].noGravity = true;
            }
        }
    }
}
