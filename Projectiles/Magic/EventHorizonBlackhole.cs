using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Magic
{
	public class EventHorizonBlackhole : ModProjectile
    {
		public int killCounter = 21;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blackhole");
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public override void AI()
        {
			if (Projectile.frame == 8)
				return;
            // Update animation
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
			if (Projectile.timeLeft > 15)
			{
				if (Projectile.frame >= 4)
					Projectile.frame = 0;
			}
			else
			{
				if (Projectile.frame < 4)
					Projectile.frame = 4;
				if (Projectile.frame >= 8)
					Projectile.frame = 4;
			}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 300);
        }
    }
}
