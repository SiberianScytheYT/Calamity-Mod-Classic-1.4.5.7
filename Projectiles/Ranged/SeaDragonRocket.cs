using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class SeaDragonRocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("RPG");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 95;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0f)
            {
                for (int i = 0; i < 5; i++)
                {
                    int fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                    Main.dust[fire].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[fire].scale = 0.5f;
                        Main.dust[fire].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                Projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item109, Projectile.position);
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                Projectile.ai[1] = 0f;
                Projectile.alpha = 255;
                Projectile.position.X = Projectile.Center.X;
                Projectile.position.Y = Projectile.Center.Y;
                Projectile.width = 200;
                Projectile.height = 200;
                Projectile.position.X -= (float)(Projectile.width / 2);
                Projectile.position.Y -= (float)(Projectile.height / 2);
                Projectile.knockBack = 10f;
            }
            else
            {
                if (Math.Abs(Projectile.velocity.X) >= 8f || Math.Abs(Projectile.velocity.Y) >= 8f)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        float num247 = 0f;
                        float num248 = 0f;
                        if (j == 1)
                        {
                            num247 = Projectile.velocity.X * 0.5f;
                            num248 = Projectile.velocity.Y * 0.5f;
                        }
                        int explosion = Dust.NewDust(new Vector2(Projectile.position.X + 3f + num247, Projectile.position.Y + 3f + num248) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, DustID.Torch, 0f, 0f, 100, default, 1f);
                        Main.dust[explosion].scale *= 2f + (float)Main.rand.Next(10) * 0.1f;
                        Main.dust[explosion].velocity *= 0.2f;
                        Main.dust[explosion].noGravity = true;
                        explosion = Dust.NewDust(new Vector2(Projectile.position.X + 3f + num247, Projectile.position.Y + 3f + num248) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, 33, 0f, 0f, 100, default, 0.5f);
                        Main.dust[explosion].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[explosion].velocity *= 0.05f;
                    }
                }
            }
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 600f, 24f, 20f);
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 192;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item110, Projectile.position);
            for (int i = 0; i < 15; i++)
            {
                int smoke = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33, 0f, 0f, 100, default, 2f);
                Main.dust[smoke].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[smoke].scale = 0.5f;
                    Main.dust[smoke].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 25; j++)
            {
                int fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                Main.dust[fire].noGravity = true;
                Main.dust[fire].velocity *= 5f;
                fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                Main.dust[fire].velocity *= 2f;
            }
            int projAmt = Main.rand.Next(2, 4);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int k = 0; k < projAmt; k++)
                {
                    Vector2 velocity = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    while (velocity.X == 0f && velocity.Y == 0f)
                    {
                        velocity = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    }
                    velocity.Normalize();
                    velocity *= (float)Main.rand.Next(70, 101) * 0.1f;
                    int flames = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<TotalityFire>(), (int)(Projectile.damage * 0.33), 0f, Projectile.owner, 0f, 0f);
					Main.projectile[flames].Calamity().forceRanged = true;
					Main.projectile[flames].penetrate = 3;
                }
            }
        }
    }
}
