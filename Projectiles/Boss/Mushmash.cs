using Terraria.ModLoader;
namespace CalRD.Projectiles.Boss
{
    public class Mushmash : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mushmash");
        }

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 10;
        }
    }
}
