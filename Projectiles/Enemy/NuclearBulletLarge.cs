using Microsoft.Xna.Framework;
using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Enemy
{
    public class NuclearBulletLarge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nuclear Bullet");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.25f);
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 300);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 2; i++)
            {
                int idx = Dust.NewDust(Projectile.position, 8, 8, (int)CalamityDusts.SulfurousSeaAcid, 0, 0, 0, default, 0.75f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
                idx = Dust.NewDust(Projectile.position, 8, 8, (int)CalamityDusts.SulfurousSeaAcid, 0, 0, 0, default, 0.75f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
            }
        }
    }
}
