using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Projectiles.Ranged
{
    public class Aquashard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aquashard");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 1;
        }

        public override void AI()
        {
            Projectile.velocity.X *= 0.9995f;
            Projectile.velocity.Y += 0.01f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int projAmt = Main.rand.Next(2, 4);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < projAmt; i++)
                {
					Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<AquashardSplit>(), (int)(Projectile.damage * 0.5), 0f, Projectile.owner, 0f, 0f);
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 154, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
