using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Typeless
{
	public class ReaverSpore : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Spore");
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Projectile.SporeSacAI();
		}

		public override void OnKill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 velocity = CalamityUtils.RandomVelocity(100f, 1f, 1f, 0.3f);
				int gas = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ProjectileID.SporeGas + Main.rand.Next(3), Projectile.damage, 0f, Projectile.owner);
				Main.projectile[gas].usesLocalNPCImmunity = true;
				Main.projectile[gas].localNPCHitCooldown = 30;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture;
			switch (Projectile.ai[0])
			{
				case 1f: texture = ModContent.Request<Texture2D>("CalRD/Projectiles/Typeless/ReaverSpore2").Value;
					break;
				default: texture = TextureAssets.Projectile[Projectile.type].Value;
					break;
			}
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Poisoned, 60 * Main.rand.Next(5, 11));
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(BuffID.Poisoned, 60 * Main.rand.Next(5, 11));
		}
	}
}
