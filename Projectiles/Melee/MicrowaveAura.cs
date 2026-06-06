using CalRD.Buffs.DamageOverTime;
using CalRD.Projectiles.Melee.Yoyos;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee
{
	public class MicrowaveAura : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

		private int radius = 100;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Microwave Radiation");
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
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
				if (p.identity == Projectile.ai[0] && p.active && p.type == ModContent.ProjectileType<MicrowaveYoyo>())
				{
					parent = p;
					active = true;
				}
			}

			if (active)
			{
				Projectile.Center = parent.Center;
				Projectile.timeLeft = 2;
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 180);
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
