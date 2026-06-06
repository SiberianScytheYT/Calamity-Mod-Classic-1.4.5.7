using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
	public class MagnomalyExplosion : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults() //don't remove this projectile
        {
            //DisplayName.SetDefault("Explosion");
        }

        public override void SetDefaults()
        {
            Projectile.width = 192;
            Projectile.height = 192;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
			Projectile.alpha = 255;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.ExoDebuffs();
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
			target.ExoDebuffs();
        }
        */
    }
}
