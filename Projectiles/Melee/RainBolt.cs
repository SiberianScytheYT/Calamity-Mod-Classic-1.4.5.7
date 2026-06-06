using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Melee
{
	public class RainBolt : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 150;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.95f;

            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
                Projectile.localAI[0] += 1f;
            }

            int num469 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 66, 0f, 0f, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 2f);
            Main.dust[num469].noGravity = true;
            Main.dust[num469].velocity *= 0f;

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 400f, 14f, 20f);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item60, Projectile.Center);
            for (int k = 0; k < 5; k++)
            {
                int rain = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 66, 0f, 0f, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB));
                Main.dust[rain].noGravity = true;
                Main.dust[rain].velocity *= 4f;
            }
        }
    }
}
