using Terraria;
using Terraria.ModLoader;
using CalRD.Buffs.DamageOverTime;

namespace CalRD.Projectiles.Rogue
{
    public class ToxicantTwisterDust : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dust");
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 85, 0f, 0f, 100, default, 1f);
            Main.dust[idx].noGravity = true;
            Main.dust[idx].velocity *= 0f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 60);
        }
    }
}
