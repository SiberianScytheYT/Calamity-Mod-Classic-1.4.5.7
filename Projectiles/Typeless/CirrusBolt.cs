using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Typeless
{
    public class CirrusBolt : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 45;
            Projectile.DamageType = DamageClass.Magic;
			Projectile.npcProj = true;
		}

		public override void AI()
		{
			if (Projectile.timeLeft % 2f == 0f)
			{
				Vector2 vector33 = Projectile.position;
				vector33 -= Projectile.velocity * 0.25f;
				int num448 = Dust.NewDust(vector33, 1, 1, 234, 0f, 0f, 0, default, 1.25f);
				Main.dust[num448].position = vector33;
				Main.dust[num448].noGravity = true;
				Main.dust[num448].noLight = true;
				Main.dust[num448].scale = Main.rand.Next(70, 110) * 0.013f;
				Main.dust[num448].velocity *= 0.1f;
			}
		}
    }
}
