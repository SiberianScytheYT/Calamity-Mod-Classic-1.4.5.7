using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class SoulPiercerBolt : ModProjectile
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
            Projectile.extraUpdates = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void AI()
        {
            for (int num447 = 0; num447 < 4; num447++)
            {
                Vector2 vector33 = Projectile.position;
                vector33 -= Projectile.velocity * ((float)num447 * 0.25f);
                int num448 = Dust.NewDust(vector33, 1, 1, 173, 0f, 0f, 0, default, 0.5f);
                Main.dust[num448].position = vector33;
                Main.dust[num448].scale = (float)Main.rand.Next(70, 110) * 0.007f;
                Main.dust[num448].velocity *= 0.2f;
            }
        }
    }
}
