using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class TitaniumClone : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/TitaniumShuriken";

        private static float RotationIncrement = 0.22f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shuriken");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.alpha = 150;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
            Projectile.Calamity().rogue = true;
			Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
			Projectile.rotation += RotationIncrement;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 30f)
            {
				CalamityGlobalProjectile.HomeInOnNPC(Projectile, true, 400f, 15f, 20f);
			}
		}

        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */ => Projectile.ai[0] >= 30f;

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
