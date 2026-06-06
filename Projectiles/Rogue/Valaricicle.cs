using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class Valaricicle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Valaricicle");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.Calamity().rogue = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 1;
			Projectile.coldDamage = true;
        }

        public override void AI()
        {
            Projectile.velocity.X *= 0.9995f;
            Projectile.velocity.Y = Projectile.velocity.Y + 0.01f;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 120);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*{
            target.AddBuff(BuffID.Frostburn, 120);
        }*/
    }
}
