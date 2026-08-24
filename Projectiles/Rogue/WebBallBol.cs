using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class WebBallBol : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/WebBall";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Web Ball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.Calamity().rogue = true;
			Projectile.aiStyle = 14;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(12))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 30, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (Projectile.Calamity().stealthStrike == true)
			{
				target.AddBuff(BuffID.Webbed, 180);
			}
			else
			{
				target.AddBuff(BuffID.Webbed, 60);
			}
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if (Projectile.Calamity().stealthStrike == true)
			{
				target.AddBuff(BuffID.Webbed, 180);
			}
			else
			{
				target.AddBuff(BuffID.Webbed, 60);
			}
        }
    }
}

