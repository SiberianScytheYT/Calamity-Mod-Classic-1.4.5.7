using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Magic
{
	public class WaterLeechProj : ModProjectile
	{
        public override string Texture => "CalRD/NPCs/AcidRain/WaterLeech";

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Leech");
            Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 540;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override void AI()
		{
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }

			if (Projectile.ai[0] == 0f)
			{
				Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.Pi;
			}
			//Sticky Behaviour
			Projectile.StickyProjAI(5);
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			Projectile.ModifyHitNPCSticky(3, false);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
		}

		//public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
		/*
		{
			target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
		}
		*/

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			Projectile.Kill();
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int height = texture.Height / Main.projFrames[Projectile.type];
			int drawStart = height * Projectile.frame;
			Vector2 origin = Projectile.Size / 2;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, drawStart, texture.Width, height)), Color.White, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0f);
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int height = texture.Height / Main.projFrames[Projectile.type];
			int drawStart = height * Projectile.frame;
			Vector2 origin = Projectile.Size / 2;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/NPCs/AcidRain/WaterLeechGlow").Value, Projectile.Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, drawStart, texture.Width, height)), Color.White, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0f);
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath1.WithVolumeScale(0.5f), Projectile.position);
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.Center, 1, 1, (int)CalamityDusts.SulfurousSeaAcid, 0, 0, 0, default, 1.5f);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
