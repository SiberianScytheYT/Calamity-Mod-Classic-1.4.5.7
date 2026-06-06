using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Ranged
{
    public class BloodClotFriendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Clot");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int num468 = 0; num468 < 2; num468++)
                {
                    Vector2 dspeed = -Projectile.velocity * 0.7f;
                    int num469 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5, dspeed.X, dspeed.Y, 100, default, 2f);
                    Main.dust[num469].noGravity = true;
                }
            }
        }
    }
}
