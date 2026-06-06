using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class NapalmArrowProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Ammo/NapalmArrow";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 32;
            Projectile.position.X -= (float)(Projectile.width / 2);
            Projectile.position.Y -= (float)(Projectile.height / 2);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int j = 0; j < 5; j++)
            {
                int fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                Main.dust[fire].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[fire].scale = 0.5f;
                    Main.dust[fire].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int k = 0; k < 10; k++)
            {
                int fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                Main.dust[fire].noGravity = true;
                Main.dust[fire].velocity *= 5f;
                fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                Main.dust[fire].velocity *= 2f;
            }
			CalamityUtils.ExplosionGores(Projectile.GetSource_FromThis(), Projectile.Center, 3);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 velocity = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    while (velocity.X == 0f && velocity.Y == 0f)
                    {
                        velocity = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    }
                    velocity.Normalize();
                    velocity *= (float)Main.rand.Next(70, 101) * 0.1f;
                    int flames = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<TotalityFire>(), (int)(Projectile.damage * 0.3), 0f, Projectile.owner, 0f, 0f);
					Main.projectile[flames].Calamity().forceRanged = true;
					Main.projectile[flames].penetrate = 3;
					Main.projectile[flames].usesLocalNPCImmunity = false;
					Main.projectile[flames].usesIDStaticNPCImmunity = true;
					Main.projectile[flames].idStaticNPCHitCooldown = 10;
                }
            }
        }
    }
}
