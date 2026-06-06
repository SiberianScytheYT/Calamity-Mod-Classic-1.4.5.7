using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Typeless
{
	public class NebulaStar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Star");
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 3600;
		}

		public override void AI()
		{
			Projectile.SporeSacAI();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.owner == Main.myPlayer && Projectile.ai[1] == 0f)
			{
				Vector2 velocity = CalamityUtils.RandomVelocity(100f, 1f, 1f, 0.3f);
				Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<NebulaDust>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			}
		}

		public override void PostDraw(Color lightColor)
		{
			Rectangle frame = new Rectangle(0, 0, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/Projectiles/Typeless/NebulaStarGlow").Value, Projectile.Center - Main.screenPosition, frame, Color.White * ((255 - Projectile.alpha) / 255f), Projectile.rotation, Projectile.Size / 2, 1f, SpriteEffects.None, 0f);
		}
	}
}
