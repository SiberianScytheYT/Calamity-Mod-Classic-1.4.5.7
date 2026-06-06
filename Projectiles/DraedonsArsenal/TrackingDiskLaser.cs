using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Projectiles.DraedonsArsenal
{
	public class TrackingDiskLaser : ModProjectile
	{
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

		public float Time
		{
			get => Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Laser");
		}

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = true;
			Projectile.Calamity().rogue = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 100;
			Projectile.timeLeft = 600;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.2f, 0.1f, 0f);

			Time++;
			if (Time >= 10f)
			{
				for (int i = 0; i < 2; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 182, 0f, 0f, 160, default, 2f);
					dust.position = Projectile.Center;
					dust.velocity = Projectile.velocity;
					dust.scale = Projectile.scale;
					dust.noGravity = true;
				}
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			Projectile.damage -= target.defense / 4;
		}

		public override void OnKill(int timeLeft)
		{
			CalamityGlobalProjectile.ExpandHitboxBy(Projectile, 60);
			Projectile.maxPenetrate = -1;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.Damage();
		}
	}
}
