using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Boss
{
    public class FlareDust2 : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Boss/FlareBomb";

		private bool start = true;
		private Vector2 velocity = Vector2.Zero;

		public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flare Bomb");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 1.5f;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 840;
            CooldownSlot = 1;
        }

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(start);
			writer.WriteVector2(velocity);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			start = reader.ReadBoolean();
			velocity = reader.ReadVector2();
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

            if (Projectile.ai[0] == 1f)
            {
				if (Projectile.timeLeft < 630)
				{
					if (Projectile.velocity.Length() < 6f)
					{
						Projectile.velocity *= 1.025f;
					}
					else
					{
						if (start)
						{
							velocity = Projectile.velocity;
							start = false;
						}

						Projectile.ai[1] += 0.1f;

						float amplitude = 2f;

						float wavyVelocity = (float)Math.Cos(Projectile.ai[1]);

						Projectile.velocity = velocity + new Vector2(wavyVelocity, wavyVelocity) * amplitude;
					}
				}
            }
            else
            {
				if (Projectile.timeLeft < 420)
				{
					if (Projectile.velocity.Length() < 12f)
					{
						Projectile.velocity *= 1.05f;
					}
					else
					{
						if (start)
						{
							velocity = Projectile.velocity;
							start = false;
						}

						Projectile.ai[1] += 0.1f;

						float amplitude = 2f;

						float wavyVelocity = (float)Math.Sin(Projectile.ai[1]);

						Projectile.velocity = velocity + new Vector2(wavyVelocity, wavyVelocity) * amplitude;
					}
				}
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 0.25f, 0f);
        }

        public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = TextureAssets.Projectile[Projectile.type].Value;
            int num214 = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y6 = num214 * Projectile.frame;
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture2D13.Width, num214)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2((float)texture2D13.Width / 2f, (float)num214 / 2f), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
			CalamityGlobalProjectile.ExpandHitboxBy(Projectile, 48);
            for (int d = 0; d < 2; d++)
            {
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 1f);
                Main.dust[idx].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[idx].scale = 0.5f;
                    Main.dust[idx].fadeIn = 1f + Main.rand.NextFloat(0.1f, 1f);
                }
            }
            for (int d = 0; d < 4; d++)
            {
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 2f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 5f;
                idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 1f);
                Main.dust[idx].velocity *= 2f;
            }
			CalamityUtils.ExplosionGores(Projectile.GetSource_FromThis(), Projectile.Center, 3);
            Projectile.Damage();
        }

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<LethalLavaBurn>(), 180);
		}

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)	
        {
			target.Calamity().lastProjectileHit = Projectile;
		}
    }
}
