using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
	public class MadAlchemistsCocktailShrapnel : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shrapnel");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Vector2 value7 = new Vector2(6f, 12f);
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] == 48f)
            {
                Projectile.localAI[0] = 0f;
            }
            else
            {
                for (int num41 = 0; num41 < 2; num41++)
                {
                    Vector2 value8 = Vector2.UnitX * -15f;
                    value8 = -Vector2.UnitY.RotatedBy((double)(Projectile.localAI[0] * 0.1308997f + (float)num41 * 3.14159274f), default) * value7 * 0.75f;
                    int num42 = Dust.NewDust(Projectile.Center, 0, 0, 173, 0f, 0f, 160, default, 0.75f);
                    Main.dust[num42].noGravity = true;
                    Main.dust[num42].position = Projectile.Center + value8;
                    Main.dust[num42].velocity = Projectile.velocity;
                }
            }

            int num458 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 1f);
            Main.dust[num458].noGravity = true;

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 500f, 12f, 20f);
        }
    }
}
