using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class HolyLaser : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Laser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                int num249 = Main.rand.Next(2);
                if (num249 == 0)
                {
                    num249 = 244;
                }
                else
                {
                    num249 = 246;
                }
				Vector2 vector33 = Projectile.position;
				vector33 -= Projectile.velocity * 0.25f;
				int num448 = Dust.NewDust(vector33, 1, 1, num249, 0f, 0f, 0, default, 0.25f);
				Main.dust[num448].position = vector33;
				Main.dust[num448].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[num448].velocity *= 0.1f;
			}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<FuckYou>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
                Main.projectile[proj].Calamity().forceMagic = true;
            }
        }
    }
}
