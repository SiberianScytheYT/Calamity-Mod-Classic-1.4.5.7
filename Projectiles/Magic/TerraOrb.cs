using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
	public class TerraOrb : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bolt");
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
        }

        public override void AI()
        {
			CalamityGlobalProjectile.MagnetSphereHitscan(Projectile, 300f, 6f, 24f, 5, ModContent.ProjectileType<TerraBolt>());
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
        }
    }
}
