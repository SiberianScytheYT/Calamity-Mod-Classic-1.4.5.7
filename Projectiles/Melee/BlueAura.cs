using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Melee
{
    public class BlueAura : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blue Aura");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            for (int num468 = 0; num468 < 3; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 15, 0f, 0f, 100, default, 1.3f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 0f;
            }
        }
    }
}
