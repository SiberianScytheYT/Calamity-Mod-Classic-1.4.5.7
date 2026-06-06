using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Enemy
{
    public class CragmawVibeCheckChain : ModProjectile
    {
		public bool ReelingPlayer = false;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("You Think You're Safe");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
			Projectile.alpha = 255;
            Projectile.timeLeft = 420;
			Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.localAI[0]);
			writer.Write(ReelingPlayer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.localAI[0] = reader.ReadSingle();
			ReelingPlayer = reader.ReadBoolean();
		}
		public override void AI()
        {
			Vector2 offsetDrawVector = new Vector2(0f, 30f);
			Projectile.alpha -= 15;
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			int toTarget = (int)Projectile.ai[1];
			if (!Main.npc[(int)Projectile.ai[0]].active)
			{
				Projectile.Kill();
				return;
			}
			Projectile.rotation = (Main.npc[(int)Projectile.ai[0]].Top - Projectile.Center + offsetDrawVector).ToRotation() + MathHelper.PiOver2;
			if (Projectile.localAI[0] == 0f)
			{
				Projectile.velocity = (Projectile.velocity * 10f + Projectile.DirectionTo(Main.player[toTarget].Center) * 9f) / 11f;
				if (Projectile.timeLeft <= 180f)
				{
					Projectile.localAI[0] = 1f;
					Projectile.netUpdate = true;
				}
				if (Projectile.Distance(Main.player[toTarget].Center) < 16f)
				{
					Projectile.localAI[0] = 1f;
					ReelingPlayer = true;
					Projectile.netUpdate = true;
				}
			}
			else
			{
				if (ReelingPlayer)
				{
					Main.player[toTarget].Center = Projectile.Center;
				}
				if (Main.player[toTarget].dead)
				{
					ReelingPlayer = false;
				}
				if (Projectile.Distance(Main.npc[(int)Projectile.ai[0]].Center) > 6f)
				{
					Projectile.velocity = (Projectile.velocity * 16f + Projectile.DirectionTo(Main.npc[(int)Projectile.ai[0]].Center) * 19f) / 17f;
				}
			}
		}

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D endTexture = ModContent.Request<Texture2D>(Texture).Value;
			Texture2D chainTexture = ModContent.Request<Texture2D>("CalRD/Projectiles/Enemy/CragmawVibeCheckMid").Value;
			Vector2 distanceVectorToStart = Main.npc[(int)Projectile.ai[0]].Top + Vector2.UnitY * 30f - Projectile.Center;
			float distanceToStart = distanceVectorToStart.Length();
			Vector2 directionToStart = Vector2.Normalize(distanceVectorToStart);
			Rectangle frameRectangle = endTexture.Frame(1, 1, 0, 0);
			frameRectangle.Height /= 4;
			frameRectangle.Y += Projectile.frame * frameRectangle.Height;

			Main.EntitySpriteDraw(endTexture, Projectile.Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(frameRectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, frameRectangle.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);

			distanceToStart -= (frameRectangle.Height / 2 + chainTexture.Height) * Projectile.scale;
			Vector2 chainDrawPosition = Projectile.Center;
			chainDrawPosition += directionToStart * Projectile.scale * frameRectangle.Height / 2f;
			if (distanceToStart > 0f)
			{
				float distanceMoved = 0f;
				Rectangle chainTextureFrameRectangle = new Rectangle(0, 0, chainTexture.Width, chainTexture.Height);
				while (distanceMoved + 1f < distanceToStart)
				{
					if (distanceToStart - distanceMoved < chainTextureFrameRectangle.Height)
					{
						chainTextureFrameRectangle.Height = (int)(distanceToStart - distanceMoved);
					}
					Point chainPositionTileCoords = chainDrawPosition.ToTileCoordinates();
					Color colorAtChainPosition = Lighting.GetColor(chainPositionTileCoords.X, chainPositionTileCoords.Y);
					colorAtChainPosition = Color.Lerp(colorAtChainPosition, Color.White, 0.3f);
					Main.EntitySpriteDraw(chainTexture, chainDrawPosition - Main.screenPosition, new Rectangle?(chainTextureFrameRectangle), Projectile.GetAlpha(colorAtChainPosition), Projectile.rotation, chainTextureFrameRectangle.Bottom(), Projectile.scale, SpriteEffects.None, 0f);
					distanceMoved += chainTextureFrameRectangle.Height * Projectile.scale;
					chainDrawPosition += directionToStart * chainTextureFrameRectangle.Height * Projectile.scale;
				}
			}
			return false;
        }
    }
}
