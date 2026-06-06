using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Boss
{
    public class DoGFire : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Death Fire");
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 480;
            CooldownSlot = 1;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            }

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

            if (Projectile.alpha > 0)
                Projectile.alpha -= 25;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            int dust = Dust.NewDust(new Vector2(Projectile.position.X + Projectile.velocity.X, Projectile.position.Y + Projectile.velocity.Y), Projectile.width, Projectile.height, 173, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.2f);
            Main.dust[dust].noGravity = true;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.ai[1] == 1f)
            {
                int num103 = (int)Player.FindClosest(Projectile.Center, 1, 1);
                Vector2 vector11 = Main.player[num103].Center - Projectile.Center;
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] >= 60f)
                {
                    if (Projectile.ai[0] < 300f)
                    {
                        float scaleFactor2 = Projectile.velocity.Length();
                        vector11.Normalize();
                        vector11 *= scaleFactor2;
                        Projectile.velocity = (Projectile.velocity * 24f + vector11) / 25f;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= scaleFactor2;
                    }
                    else if (Projectile.velocity.Length() < 24f)
                    {
                        Projectile.tileCollide = true;
                        Projectile.velocity *= 1.02f;
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
            target.AddBuff(BuffID.Frostburn, 300, true);
            target.AddBuff(BuffID.Darkness, 300, true);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 64;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int num621 = 0; num621 < 5; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 10; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
            }
            Projectile.Damage();
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)	
        {
			target.Calamity().lastProjectileHit = Projectile;
		}
    }
}
