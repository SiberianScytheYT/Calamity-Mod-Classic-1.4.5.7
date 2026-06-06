using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.NPCs.Providence;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Boss
{
    public class HolyFire2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Holy Fire");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }
            if (Math.Abs(Projectile.velocity.X) < 7f)
            {
                Projectile.velocity.X *= 1.05f;
            }
            int num103 = Player.FindClosest(Projectile.Center, 1, 1);
			if (Projectile.ai[1] == 0f)
			{
				Projectile.damage = Projectile.GetProjectileDamage(ModContent.NPCType<Providence>());
			}
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] < 180f && Projectile.ai[1] > 60f)
            {
                float scaleFactor2 = Projectile.velocity.Length();
                Vector2 vector11 = Main.player[num103].Center - Projectile.Center;
                vector11.Normalize();
                vector11 *= scaleFactor2;
                Projectile.velocity = (Projectile.velocity * 24f + vector11) / 25f;
                Projectile.velocity.Normalize();
                Projectile.velocity *= scaleFactor2;
            }
        }

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.dayTime ? new Color(250, 150, 0, Projectile.alpha) : new Color(100, 200, 250, Projectile.alpha);
		}

		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Main.dayTime ? TextureAssets.Projectile[Projectile.type].Value : ModContent.Request<Texture2D>("CalRD/Projectiles/Boss/HolyFire2Night").Value;
			int num214 = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y6 = num214 * Projectile.frame;
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture2D13.Width, num214)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture2D13.Width / 2f, num214 / 2f), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
            Projectile.position.X = Projectile.position.X + (Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (Projectile.height / 2);
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);
			int dustType = Main.dayTime ? (int)CalamityDusts.ProfanedFire : (int)CalamityDusts.Nightwither;
			for (int num621 = 0; num621 < 5; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 2f);
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 10; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 2f);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			int buffType = Main.dayTime ? ModContent.BuffType<HolyFlames>() : ModContent.BuffType<Nightwither>();
			target.AddBuff(buffType, 120);
		}

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)	
        {
			target.Calamity().lastProjectileHit = Projectile;
		}
    }
}
