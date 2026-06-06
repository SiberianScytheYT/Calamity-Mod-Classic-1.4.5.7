using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Ranged
{
	public class ImpactRound : ModProjectile
	{
        public override string Texture => "CalRD/Projectiles/Ranged/AMRShot";

		private bool initialized = false;
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Impact Round");
		}

		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.light = 0.5f;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 7;
			Projectile.scale = 1.18f;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.BulletHighVelocity;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			if (!initialized && Projectile.CountsAsClass(DamageClass.Ranged)) //Ranged check prevents quiver splits triggering the sound
			{
				initialized = true;
				SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Item/LargeWeaponFire"), Projectile.Center);
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			double damageMult = 1D;
			if (modifiers.ToHitInfo(target.damage, true, modifiers.Knockback.Base, false, 0f).Crit)
				damageMult += 0.25;
			if (target.Inorganic())
				damageMult += 0.1;
			Projectile.damage = (int)(Projectile.damage * damageMult);
		}

		public override bool PreDraw(ref Color lightColor) => Projectile.timeLeft < 600;
	}
}
