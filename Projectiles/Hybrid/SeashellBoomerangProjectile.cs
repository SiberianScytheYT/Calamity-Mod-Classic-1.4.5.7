using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Hybrid
{
    public class SeashellBoomerangProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/SeashellBoomerang";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Boomerang");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 240;
            AIType = ProjectileID.WoodenBoomerang;
            Projectile.DamageType = DamageClass.Melee;
        }
    }
}
