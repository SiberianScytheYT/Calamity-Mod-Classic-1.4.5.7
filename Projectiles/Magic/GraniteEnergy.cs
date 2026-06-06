using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class GraniteEnergy : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Granite Energy");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 90;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.ToRadians(90);
			if (Main.rand.NextBool(2))
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 229, 0f, 0f, 100, default, 0.6f);

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 500f, 22f, 20f);
        }
    }
}
