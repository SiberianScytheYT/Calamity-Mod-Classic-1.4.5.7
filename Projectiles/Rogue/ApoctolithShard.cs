using CalRD.Buffs.DamageOverTime;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
	public class ApoctolithShard : ModProjectile
	{
        public override string Texture => "CalRD/Projectiles/Rogue/AbyssalMirrorProjectile";

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Apoctolith Shard");
			Main.projFrames[Projectile.type] = 3;
		}
		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.width = 13;
			Projectile.height = 13;
			Projectile.Calamity().rogue = true;
		}
		public override void AI()
		{
			//Rotation and gravity
			Projectile.rotation += 0.6f * Projectile.direction;
			Projectile.velocity.Y += 0.27f;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}

		}
		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			//Dust effect
			int splash = 0;
			while (splash < 4)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67, -Projectile.velocity.X * 0.15f, -Projectile.velocity.Y * 0.10f, 150, default, 0.9f);
				splash += 1;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<CrushDepth>(), 120);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<CrushDepth>(), 120);
		}
	}
}
