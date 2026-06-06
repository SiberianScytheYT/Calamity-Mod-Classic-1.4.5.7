using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Boss
{
    public class DarkEnergyBall : ModProjectile
    {
        private double timeElapsed = 0.0;
        private double circleSize = 1.0;
        private double circleGrowth = 0.02;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dark Energy");
            Main.projFrames[Projectile.type] = 6;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            CooldownSlot = 1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(timeElapsed);
            writer.Write(circleSize);
            writer.Write(circleGrowth);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            timeElapsed = reader.ReadDouble();
            circleSize = reader.ReadDouble();
            circleGrowth = reader.ReadDouble();
        }

        public override void AI()
        {
            timeElapsed += 0.02;
            Projectile.velocity.X = (float)(Math.Sin(timeElapsed * (double)(0.5f * Projectile.ai[0])) * circleSize);
            Projectile.velocity.Y = (float)(Math.Cos(timeElapsed * (double)(0.5f * Projectile.ai[0])) * circleSize);
            circleSize += circleGrowth;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 5)
            {
                Projectile.frame = 0;
            }
        }

		public override bool PreDraw(ref Color lightColor)
		{
			CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);

			Rectangle frame = new Rectangle(0, Projectile.frame * TextureAssets.Projectile[Projectile.type].Value.Height, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]);
			Color color = Color.Lerp(Color.White, Color.Fuchsia, 0.5f);

			Main.EntitySpriteDraw(ModContent.Request<Texture2D>("CalRD/Projectiles/Boss/DarkEnergyBallGlow").Value, Projectile.Center - Main.screenPosition, frame, color, Projectile.rotation, Projectile.Size / 2, 1f, SpriteEffects.None, 0f);

			color = Color.Lerp(Color.White, Color.Cyan, 0.5f);

			Main.EntitySpriteDraw(ModContent.Request<Texture2D>("CalRD/Projectiles/Boss/DarkEnergyBallGlow2").Value, Projectile.Center - Main.screenPosition, frame, color, Projectile.rotation, Projectile.Size / 2, 1f, SpriteEffects.None, 0f);

			return false;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(BuffID.VortexDebuff, 60);
		}

		public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 90, 0f, 0f);
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)	
        {
			target.Calamity().lastProjectileHit = Projectile;
		}
    }
}
