using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRD.Projectiles.DraedonsArsenal
{
	public class PlasmaGrenadeSmallExplosion : ModProjectile
	{
		public float Time
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public int frameX = 0;
		public int frameY = 0;
		private const int horizontalFrames = 2;
		private const int verticalFrames = 7;
		private const int frameLength = 5;
		private const float radius = 139.5f;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Plasma Explosion");
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 279;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.Calamity().rogue = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
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
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.SourceDamage.Flat += target.defense / 4;
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
			Main.EntitySpriteDraw(texture, drawPos, frame, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}
