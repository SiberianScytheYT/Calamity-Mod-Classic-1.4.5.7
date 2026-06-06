using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Boss
{
    public class DeusMine : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Mine");
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.hostile = true;
            Projectile.alpha = 100;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 900;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0f)
            {
                Projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item33, Projectile.position);
            }

			if (Projectile.timeLeft < 85)
				Projectile.damage = 0;

			if (Projectile.timeLeft < 815)
				return;

			float velocity = 0.1f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i].active)
				{
					if (i != Projectile.whoAmI && Main.projectile[i].type == Projectile.type)
					{
						if (Vector2.Distance(Projectile.Center, Main.projectile[i].Center) < 48f)
						{
							if (Projectile.position.X < Main.projectile[i].position.X)
								Projectile.velocity.X -= velocity;
							else
								Projectile.velocity.X += velocity;

							if (Projectile.position.Y < Main.projectile[i].position.Y)
								Projectile.velocity.Y -= velocity;
							else
								Projectile.velocity.Y += velocity;
						}
						else
							Projectile.velocity = Vector2.Zero;
					}
				}
			}
		}

        public override bool CanHitPlayer(Player target)
		{
            if (Projectile.timeLeft > 815 || Projectile.timeLeft < 85)
            {
                return false;
            }
            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft > 815)
            {
                Projectile.localAI[1] += 1f;
                byte b2 = (byte)(((int)Projectile.localAI[1]) * 3);
                byte a2 = (byte)(Projectile.alpha * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(Projectile.alpha * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return new Color(255, 255, 255, Projectile.alpha);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 96;
            Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);
            for (int num621 = 0; num621 < 30; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 1.2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 60; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), 0f, 0f, 100, default, 1.7f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 1.5f;
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), 0f, 0f, 100, default, 1f);
            }
            Projectile.Damage();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
        }
    }
}
