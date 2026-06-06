using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class SoulPiercerBeam : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Piercer");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 240;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                for (int num447 = 0; num447 < 4; num447++)
                {
                    Vector2 vector33 = Projectile.position;
                    vector33 -= Projectile.velocity * ((float)num447 * 0.25f);
                    Projectile.alpha = 255;
                    int num448 = Dust.NewDust(vector33, 1, 1, 173, 0f, 0f, 0, default, 0.5f);
                    Main.dust[num448].position = vector33;
                    Main.dust[num448].scale = (float)Main.rand.Next(70, 110) * 0.007f;
                    Main.dust[num448].velocity *= 0.2f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int x = 0; x < 3; x++)
            {
                if (Projectile.owner == Main.myPlayer)
                {
					CalamityUtils.ProjectileBarrage(Projectile.GetSource_FromThis(), Projectile.Center, target.Center, true, -500f, 500f, 0f, 500f, 10f, ModContent.ProjectileType<SoulPiercerBolt>(), (int)(Projectile.damage * 0.5), 0f, Projectile.owner, false, 0f);
                }
            }
        }
    }
}
