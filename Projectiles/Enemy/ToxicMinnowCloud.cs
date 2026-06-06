using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Enemy
{
    public class ToxicMinnowCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cloud");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 7;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
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
            if (Main.rand.NextBool(2))
            {
                Projectile.velocity *= 0.95f;
            }
            else if (Main.rand.NextBool(2))
            {
                Projectile.velocity *= 0.9f;
            }
            else if (Main.rand.NextBool(2))
            {
                Projectile.velocity *= 0.85f;
            }
            else
            {
                Projectile.velocity *= 0.8f;
            }
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 560f)
            {
                if (Projectile.alpha < 255)
                {
                    Projectile.alpha += 5;
                    if (Projectile.alpha > 255)
                    {
                        Projectile.alpha = 255;
                    }
                }
                else if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.Kill();
                }
            }
            else if (Projectile.alpha > 80)
            {
                Projectile.alpha -= 30;
                if (Projectile.alpha < 80)
                {
                    Projectile.alpha = 80;
                }
            }
        }

		public override bool CanHitPlayer(Player target)
		{
			if (Projectile.timeLeft < 40)
			{
				return false;
			}
			return true;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Poisoned, 600);
        }

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.timeLeft < 40)
			{
				return false;
			}
			return null;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 600);
        }
    }
}
