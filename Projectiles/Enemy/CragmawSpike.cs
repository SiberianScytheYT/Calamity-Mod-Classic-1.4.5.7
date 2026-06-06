using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Enemy
{
    public class CragmawSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spike");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 28;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }
        public override void AI()
        {
            if (Projectile.velocity.Y < 6f)
                Projectile.velocity.Y += 0.075f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 120);
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
