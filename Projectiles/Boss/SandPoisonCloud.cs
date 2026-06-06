using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Boss
{
    public class SandPoisonCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Toxic Cloud");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1800;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.5f, 0.3f, 0f);

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 9)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }

            Projectile.velocity *= 0.995f;

            if (Projectile.timeLeft < 180)
            {
				Projectile.damage = 0;
                if (Projectile.alpha < 255)
                {
                    Projectile.alpha += 5;
                    if (Projectile.alpha > 255)
                    {
                        Projectile.alpha = 255;
                        Projectile.Kill();
                    }
                }
            }
            else if (Projectile.alpha > 30)
            {
                Projectile.alpha -= 30;
                if (Projectile.alpha < 30)
                {
                    Projectile.alpha = 30;
                }
            }
        }

        public override bool CanHitPlayer(Player target)
		{
            if (Projectile.timeLeft < 180)
            {
                return false;
            }
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Venom, 300);
        }
    }
}
