using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
	public class SupernovaBoom : ModProjectile
	{
		public int frameX = 0;
		public int frameY = 0;
		private const int horizontalFrames = 5;
		private const int verticalFrames = 4;
		private const int frameLength = 4;
		private const float radius = 204.5f;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Explosion");
		}

		public override void SetDefaults()
		{
			Projectile.width = 408;
			Projectile.height = 410;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.Calamity().rogue = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = frameLength * horizontalFrames * verticalFrames / 5;
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter % frameLength == frameLength - 1)
			{
				frameY++;
				if (frameY >= verticalFrames)
				{
					frameX++;
					frameY = 0;
				}
				if (frameX >= horizontalFrames)
				{
					Projectile.Kill();
				}
			}

			Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.5f / 255f, Main.DiscoG * 0.5f / 255f, Main.DiscoB * 0.5f / 255f);

			/*float dustSpeed = (float)Main.rand.Next(12, 36);
			Vector2 dustVel = CalamityUtils.RandomVelocity(40f, dustSpeed, dustSpeed, 1f);
			int dustType = Utils.SelectRandom(Main.rand, new int[]
			{
				107,
				234,
				269
			});
			int rainbow = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, default, 2f);
			Dust dust = Main.dust[rainbow];
			dust.noGravity = true;
			dust.position = projectile.Center;
			dust.position.X += (float)Main.rand.Next(-10, 11);
			dust.position.Y += (float)Main.rand.Next(-10, 11);
			dust.velocity = dustVel;*/
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

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int length = texture.Width / horizontalFrames;
			int height = texture.Height / verticalFrames;
			Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			Rectangle frame = new Rectangle(frameX * length, frameY * height, length, height);
			Vector2 origin = new Vector2(length / 2f, height / 2f);
			Main.spriteBatch.Draw(texture, drawPos, frame, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.ExoDebuffs();
		}

		//public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
		/*{
			target.ExoDebuffs();
		}*/
	}
}
