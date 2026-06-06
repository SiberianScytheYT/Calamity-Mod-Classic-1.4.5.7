using Terraria.ModLoader;
namespace CalRD.Projectiles.Melee
{
    public class RoxShockwave : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 450;
            Projectile.height = 450;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 40;
        }
    }
}
