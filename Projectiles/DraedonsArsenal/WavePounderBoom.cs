using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.DraedonsArsenal
{
	public struct ScreenShakeSpot
	{
		public float ScreenShakePower;
		public Vector2 Position;
		public ScreenShakeSpot(float screenShakePower, Vector2 position)
		{
			ScreenShakePower = screenShakePower;
			Position = position;
		}
	}

	public class WavePounderBoom : ModProjectile
	{
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

		public ScreenShakeSpot CurrentSpot;

		public float Radius
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public float MaxRadius
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public float InterpolationStep
		{
			get => Projectile.localAI[1];
			set => Projectile.localAI[1] = value;
		}

		public const int Lifetime = 60;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Explosion");
			ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = true;
			Projectile.Calamity().rogue = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.timeLeft = Lifetime;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.localAI[0]);
			writer.Write(Projectile.localAI[1]);
			writer.Write(CurrentSpot.ScreenShakePower);
			writer.WriteVector2(CurrentSpot.Position);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.localAI[0] = reader.ReadSingle();
			Projectile.localAI[1] = reader.ReadSingle();
			CurrentSpot = new ScreenShakeSpot(reader.ReadInt32(), reader.ReadVector2());
		}

		public override void AI()
		{
			if (Projectile.Calamity().stealthStrike)
			{
				if (Projectile.localAI[0] == 0f)
				{
					CurrentSpot = new ScreenShakeSpot(0, Projectile.Center);
					Projectile.localAI[0] = 1f;
				}
				CurrentSpot.ScreenShakePower = (float)Math.Sin(MathHelper.Pi * Projectile.timeLeft / Lifetime) * 16f;
				CurrentSpot.Position = Projectile.Center;
				CalamityWorld.ScreenShakeSpots[Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI)] = CurrentSpot;
			}

			Lighting.AddLight(Projectile.Center, 0.2f, 0.1f, 0f);
			Radius = MathHelper.Lerp(Radius, MaxRadius, 0.25f);
			Projectile.scale = MathHelper.Lerp(1.2f, 5f, Utils.GetLerpValue(Lifetime, 0f, Projectile.timeLeft, true));
			CalamityGlobalProjectile.ExpandHitboxBy(Projectile, (int)(Radius * Projectile.scale), (int)(Radius * Projectile.scale));
		}

		public override void OnKill(int timeLeft)
		{
			if (Projectile.Calamity().stealthStrike)
				CalamityWorld.ScreenShakeSpots.Remove(Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI)); // Remove the explosion associated with this projectile's UUID.
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			float pulseCompletionRatio = Utils.GetLerpValue(Lifetime, 0f, Projectile.timeLeft, true);
			Vector2 scale = new Vector2(1.5f, 1f);
			DrawData drawData = new DrawData(ModContent.Request<Texture2D>("Terraria/Images/Misc/Perlin").Value,
				Projectile.Center - Main.screenPosition + Projectile.Size * scale * 0.5f,
				new Rectangle(0, 0, Projectile.width, Projectile.height),
				new Color(new Vector4(1f - (float)Math.Sqrt(pulseCompletionRatio))) * 0.7f * Projectile.Opacity,
				Projectile.rotation,
				Projectile.Size,
				scale,
				SpriteEffects.None, 0);

			Color pulseColor = Color.Lerp(Color.Yellow * 1.6f, Color.White, MathHelper.Clamp(pulseCompletionRatio * 2.2f, 0f, 1f));
			GameShaders.Misc["ForceField"].UseColor(pulseColor);
			GameShaders.Misc["ForceField"].Apply(drawData);
			drawData.Draw(Main.spriteBatch);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
	}
}
