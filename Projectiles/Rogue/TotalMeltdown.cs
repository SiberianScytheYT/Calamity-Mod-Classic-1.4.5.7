using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
	public class TotalMeltdown : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Explosion");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 13;
        }

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 122;
            Projectile.friendly = true;
            Projectile.Calamity().rogue = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Main.projFrames[Projectile.type] * 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void AI()
        {
            if (Projectile.timeLeft % 5f == 4f)
                Projectile.frame++;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
