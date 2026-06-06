using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class MiniRocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rocket");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 95;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

            //Animation
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 7)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }

			if (Projectile.velocity.Length() >= 8f)
			{
				for (int num246 = 0; num246 < 2; num246++)
				{
					float num247 = 0f;
					float num248 = 0f;
					if (num246 == 1)
					{
						num247 = Projectile.velocity.X * 0.5f;
						num248 = Projectile.velocity.Y * 0.5f;
					}
					int num249 = Dust.NewDust(new Vector2(Projectile.position.X + 3f + num247, Projectile.position.Y + 3f + num248) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, 6, 0f, 0f, 100, default, 1f);
					Main.dust[num249].scale *= 2f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num249].velocity *= 0.2f;
					Main.dust[num249].noGravity = true;
					num249 = Dust.NewDust(new Vector2(Projectile.position.X + 3f + num247, Projectile.position.Y + 3f + num248) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, 31, 0f, 0f, 100, default, 0.5f);
					Main.dust[num249].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[num249].velocity *= 0.05f;
				}
			}
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 400f, 20f, 20f);
        }

        public override void OnKill(int timeLeft)
        {
			CalamityGlobalProjectile.ExpandHitboxBy(Projectile, 32);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int num621 = 0; num621 < 20; num621++)
            {
                int num622 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 31, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 30; num623++)
            {
                int num624 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
            }
			CalamityUtils.ExplosionGores(Projectile.GetSource_FromThis(), Projectile.Center, 3);
        }
    }
}
