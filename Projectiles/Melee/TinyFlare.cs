using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee
{
	public class TinyFlare : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flare");
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            for (int num468 = 0; num468 < 3; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 174, 0f, 0f, 100, default, 0.75f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 0f;
            }

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, true, 400f, 10f, 20f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
