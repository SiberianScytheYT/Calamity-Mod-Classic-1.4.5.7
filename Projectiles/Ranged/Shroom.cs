using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Ranged
{
	public class Shroom : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroom");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.alpha = 80;
            Projectile.penetrate = 2;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.2f, 0.6f);
            Projectile.rotation += 1f;
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 400f, 25f, 20f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 5;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }
    }
}
