using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Ranged
{
	public class MagnomalyAura : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

		private int radius = 100;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magnomaly Aura");
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
			Projectile.width = 200;
			Projectile.height = 200;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 300;
        }

		public override void AI()
		{
			Projectile parent = Main.projectile[0];
			bool active = false;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile p = Main.projectile[i];
				if (p.identity == Projectile.ai[0] && p.active && p.type == ModContent.ProjectileType<MagnomalyRocket>())
				{
					parent = p;
					active = true;
				}
			}

			if (active)
			{
				Projectile.Center = parent.Center;
			}
			else
			{
				Projectile.Kill();
			}

			if (!parent.active)
			{
				Projectile.Kill();
			}
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float dist1 = Vector2.Distance(Projectile.Center, targetHitbox.TopLeft());
            float dist2 = Vector2.Distance(Projectile.Center, targetHitbox.TopRight());
            float dist3 = Vector2.Distance(Projectile.Center, targetHitbox.BottomLeft());
            float dist4 = Vector2.Distance(Projectile.Center, targetHitbox.BottomRight());

            float minDist = dist1;
            if (dist2 < minDist)
                minDist = dist2;
            if (dist3 < minDist)
                minDist = dist3;
            if (dist4 < minDist)
                minDist = dist4;

            return minDist <= radius;
        }
    }
}
