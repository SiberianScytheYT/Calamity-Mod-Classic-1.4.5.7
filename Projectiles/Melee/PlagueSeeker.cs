using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Melee
{
	public class PlagueSeeker : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Seeker");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int num468 = 0; num468 < 3; num468++)
                {
                    int num469 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 107, 0f, 0f, 100, default, 0.75f);
                    Main.dust[num469].noGravity = true;
                    Main.dust[num469].velocity *= 0f;
                }
            }

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 400f, 18f, 20f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 180);
        }
    }
}
