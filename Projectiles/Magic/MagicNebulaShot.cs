using CalRD.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Magic
{
	public class MagicNebulaShot : BaseLaserbeamProjectile
	{
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

		private Color startingColor = new Color(119, 210, 255);
		private Color secondColor = new Color(247, 119, 255);
		public override float MaxScale => 0.7f;
		public override float MaxLaserLength => 1599.999999f;
		public override float Lifetime => 20f;
		public override Color LaserOverlayColor => CalamityUtils.ColorSwap(startingColor, secondColor, 0.9f);
		public override Color LightCastColor => LaserOverlayColor;
		public override Texture2D LaserBeginTexture => ModContent.Request<Texture2D>("CalRD/ExtraTextures/Lasers/UltimaRayStart").Value;
		public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>("CalRD/ExtraTextures/Lasers/UltimaRayMid").Value;
		public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>("CalRD/ExtraTextures/Lasers/UltimaRayEnd").Value;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Nebula Shot");
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 22;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.localNPCHitCooldown = 2;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override bool PreAI()
		{
			// Initialization. Using the AI hook would override the base laser's code, and we don't want that.
			if (Projectile.localAI[0] == 0f)
			{
				if (Main.rand.NextBool())
				{
					secondColor = new Color(119, 210, 255);
					startingColor = new Color(247, 119, 255);
				}
			}
			return true;
		}

		public override bool ShouldUpdatePosition() => false;

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire, 600);
			target.AddBuff(BuffID.Frostburn, 600);
		}
	}
}
