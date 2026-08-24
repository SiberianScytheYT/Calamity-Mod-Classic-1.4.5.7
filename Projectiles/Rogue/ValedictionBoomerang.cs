using CalRD.Buffs.DamageOverTime;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class ValedictionBoomerang : ModProjectile
	{
        public override string Texture => "CalRD/Items/Weapons/Rogue/Valediction";

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Valediction");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 70;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 3;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
			Projectile.Calamity().rogue = true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 8;
				SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
			}
			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[1] += 1f;
				if (Projectile.ai[1] >= 60f)
				{
					Projectile.ai[0] = 1f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}
				else
				{
					CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 400f, 20f, 20f);
				}
			}
			else
			{
				float returnSpeed = 30f;
				float acceleration = 5f;
				Vector2 projVector = player.Center - Projectile.Center;
				float playerDist = projVector.Length();
				if (playerDist > 3000f)
				{
					Projectile.Kill();
				}
				playerDist = returnSpeed / playerDist;
				projVector.X *= playerDist;
				projVector.Y *= playerDist;
				if (Projectile.velocity.X < projVector.X)
				{
					Projectile.velocity.X += acceleration;
					if (Projectile.velocity.X < 0f && projVector.X > 0f)
					{
						Projectile.velocity.X += acceleration;
					}
				}
				else if (Projectile.velocity.X > projVector.X)
				{
					Projectile.velocity.X -= acceleration;
					if (Projectile.velocity.X > 0f && projVector.X < 0f)
					{
						Projectile.velocity.X -= acceleration;
					}
				}
				if (Projectile.velocity.Y < projVector.Y)
				{
					Projectile.velocity.Y += acceleration;
					if (Projectile.velocity.Y < 0f && projVector.Y > 0f)
					{
						Projectile.velocity.Y += acceleration;
					}
				}
				else if (Projectile.velocity.Y > projVector.Y)
				{
					Projectile.velocity.Y -= acceleration;
					if (Projectile.velocity.Y > 0f && projVector.Y < 0f)
					{
						Projectile.velocity.Y -= acceleration;
					}
				}
				if (Main.myPlayer == Projectile.owner)
				{
					Rectangle projHitbox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
					Rectangle playerHitbox = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
					if (projHitbox.Intersects(playerHitbox))
					{
						Projectile.Kill();
					}
				}
			}
			Projectile.rotation += 0.5f;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<CrushDepth>(), 600);
			OnHitEffects();
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<CrushDepth>(), 600);
			OnHitEffects();
		}

		private void OnHitEffects()
		{
			int typhoonAmt = 3;
			if (Projectile.owner == Main.myPlayer && Projectile.Calamity().stealthStrike)
			{
				SoundEngine.PlaySound(SoundID.Item84, Projectile.position);
				for (int typhoonCount = 0; typhoonCount < typhoonAmt; typhoonCount++)
				{
					Vector2 velocity = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
					while (velocity.X == 0f && velocity.Y == 0f)
					{
						velocity = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
					}
					velocity.Normalize();
					velocity *= (float)Main.rand.Next(70, 101) * 0.1f;
					Projectile typhoon = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<NuclearFuryProjectile>(), Projectile.damage / 2, 0f, Projectile.owner);
					typhoon.Calamity().forceRogue = true;
					typhoon.usesLocalNPCImmunity = true;
					typhoon.localNPCHitCooldown = 10;
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
			Projectile.position = Projectile.Center;
			Projectile.width = Projectile.height = 100;
			Projectile.Center = Projectile.position;
			for (int d = 0; d < 5; d++)
			{
				int water = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33, 0f, 0f, 100, default, 2f);
				Main.dust[water].velocity *= 3f;
				if (Main.rand.NextBool(2))
				{
					Main.dust[water].scale = 0.5f;
					Main.dust[water].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
			}
			for (int d = 0; d < 8; d++)
			{
				int water = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33, 0f, 0f, 100, default, 3f);
				Main.dust[water].noGravity = true;
				Main.dust[water].velocity *= 5f;
				water = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33, 0f, 0f, 100, default, 2f);
				Main.dust[water].velocity *= 2f;
			}
		}
	}
}
