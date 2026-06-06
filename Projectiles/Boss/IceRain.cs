using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Boss
{
    public class IceRain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ice Rain");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
        }

        public override void AI()
        {
            Lighting.AddLight((int)((Projectile.position.X + (Projectile.width / 2)) / 16f), (int)((Projectile.position.Y + (Projectile.height / 2)) / 16f), 0f, 0.25f, 0.25f);

			if (Projectile.ai[0] != 2f)
				Projectile.aiStyle = 1;

			if (Projectile.ai[0] == 0f)
			{
				Projectile.velocity.Y += 0.2f;
			}
			else if (Projectile.ai[0] == 2f)
			{
				Projectile.velocity.Y += 0.1f;

				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;

				if (Projectile.velocity.Y > 6f)
					Projectile.velocity.Y = 6f;
			}

			if (Projectile.localAI[0] == 0f)
            {
                Projectile.scale += 0.01f;
                Projectile.alpha -= 50;
                if (Projectile.alpha <= 0)
                {
                    Projectile.localAI[0] = 1f;
                    Projectile.alpha = 0;
                }
            }
            else
            {
                Projectile.scale -= 0.01f;
                Projectile.alpha += 50;
                if (Projectile.alpha >= 255)
                {
                    Projectile.localAI[0] = 0f;
                    Projectile.alpha = 255;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Main.dayTime ? new Color(50, 50, 255, Projectile.alpha) : new Color(255, 255, 255, Projectile.alpha);
        }

        public override void OnKill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.Item27.WithVolumeScale(0.25f), Projectile.Center);
            for (int num373 = 0; num373 < 3; num373++)
            {
                int num374 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 76, 0f, 0f, 0, default, 1f);
                Main.dust[num374].noGravity = true;
                Main.dust[num374].noLight = true;
                Main.dust[num374].scale = 0.7f;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Frostburn, 60, true);
            target.AddBuff(BuffID.Chilled, 30, true);
        }
    }
}
