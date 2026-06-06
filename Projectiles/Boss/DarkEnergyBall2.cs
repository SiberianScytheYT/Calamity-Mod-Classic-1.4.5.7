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
    public class DarkEnergyBall2 : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Boss/DarkEnergyBall";

        private bool start = true;
		private float startingPosX = 0f;
		private float startingPosY = 0f;
		private double distance = 0D;

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
            writer.Write(start);
			writer.Write(startingPosX);
			writer.Write(startingPosY);
			writer.Write(distance);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            start = reader.ReadBoolean();
			startingPosX = reader.ReadSingle();
			startingPosY = reader.ReadSingle();
			distance = reader.ReadDouble();
		}

        public override void AI()
        {
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

			if (start)
			{
				startingPosX = Projectile.Center.X;
				startingPosY = Projectile.Center.Y;
				start = false;
			}

			double deg = Projectile.ai[0];
			double rad = deg * (Math.PI / 180);
			distance += Projectile.ai[1] == 1f ? 2D : 1D;
			Projectile.position.X = startingPosX - (int)(Math.Cos(rad) * distance) - Projectile.width / 2;
			Projectile.position.Y = startingPosY - (int)(Math.Sin(rad) * distance) - Projectile.height / 2;
			Projectile.ai[0] += 0.5f;
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
    }
}
