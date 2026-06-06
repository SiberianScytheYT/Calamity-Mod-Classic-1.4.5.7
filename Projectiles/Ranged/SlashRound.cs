using CalRD.Buffs.DamageOverTime;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class SlashRound : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Ranged/AMRShot";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Round");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.light = 0.5f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 7;
            Projectile.scale = 1.18f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.BulletHighVelocity;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Shred>(), 180);
        }
    }
}
