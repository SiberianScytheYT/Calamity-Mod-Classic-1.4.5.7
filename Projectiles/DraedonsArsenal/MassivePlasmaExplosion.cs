using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRD.Projectiles.DraedonsArsenal
{
	public class MassivePlasmaExplosion : ModProjectile
	{
		public float Time
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		private float lightAmt = 1f;

		public int frameX = 0;
		public int frameY = 0;
		private const int horizontalFrames = 4;
		private const int verticalFrames = 5;
		private const int frameLength = 5;
		private const float radius = 191.5f;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Plasma Explosion");
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 383;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.Calamity().rogue = true;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = frameLength * horizontalFrames * verticalFrames / 2;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.SourceDamage.Flat += target.defense / 4;
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

			Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 4f * lightAmt);
			if (Projectile.localAI[0] == 0f)
			{
				SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Item/FlareSound"), Projectile.Center);
				Projectile.localAI[0] = 1f;
			}
			lightAmt = (float)Math.Sin(Time / 37 * MathHelper.Pi) * 2f;
			if (lightAmt > 1f)
				lightAmt = 1f;
			Time++;
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
