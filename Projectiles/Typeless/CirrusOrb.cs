using Terraria.ModLoader;
namespace CalRD.Projectiles.Typeless
{
	public class CirrusOrb : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Orb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
			Projectile.npcProj = true;
		}

        public override void AI()
        {
			CalamityGlobalProjectile.MagnetSphereHitscan(Projectile, 150f, 6f, 6f, 2, ModContent.ProjectileType<CirrusBolt>());
        }
    }
}
