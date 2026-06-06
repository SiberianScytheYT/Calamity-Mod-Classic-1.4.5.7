using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
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
    public class HolyBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Holy Blast");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 180;
            Projectile.height = 180;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
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
            if (Projectile.wet || Projectile.lavaWet)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[1] == 0f)
            {
				int dustType = Main.dayTime ? (int)CalamityDusts.ProfanedFire : (int)CalamityDusts.Nightwither;
				for (int num621 = 0; num621 < 10; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                Projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            }
            if (Math.Abs(Projectile.velocity.X) > 0.2)
            {
                Projectile.spriteDirection = -Projectile.direction;
            }
            if (Projectile.velocity.X < 0f)
            {
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            }
            else
            {
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Main.dayTime ? new Color(250, 150, 0, Projectile.alpha) : new Color(100, 200, 250, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Main.dayTime ? TextureAssets.Projectile[Projectile.type].Value : ModContent.Request<Texture2D>("CalRD/Projectiles/Boss/HolyBlastNight").Value;
            int num214 = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y6 = num214 * Projectile.frame;
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture2D13.Width, num214)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture2D13.Width / 2f, num214 / 2f), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 shootFromVector = new Vector2(Projectile.Center.X, Projectile.Center.Y);
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle;
                for (int i = 0; i < 4; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), shootFromVector.X, shootFromVector.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<HolyFire2>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), shootFromVector.X, shootFromVector.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<HolyFire2>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
                }
            }
			SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/ProvidenceHolyBlastImpact"), Projectile.Center);
            int dustType = Main.dayTime ? (int)CalamityDusts.ProfanedFire : (int)CalamityDusts.Nightwither;
            for (int num193 = 0; num193 < 6; num193++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 50, default, 1.5f);
            }
            for (int num194 = 0; num194 < 60; num194++)
            {
                int num195 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 0, default, 2.5f);
                Main.dust[num195].noGravity = true;
                Main.dust[num195].velocity *= 3f;
                num195 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 50, default, 1.5f);
                Main.dust[num195].velocity *= 2f;
                Main.dust[num195].noGravity = true;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			int buffType = Main.dayTime ? ModContent.BuffType<HolyFlames>() : ModContent.BuffType<Nightwither>();
			target.AddBuff(buffType, 240);
		}

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)	
        {
			target.Calamity().lastProjectileHit = Projectile;
		}
    }
}
