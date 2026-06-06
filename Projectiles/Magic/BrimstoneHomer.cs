using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
	public class BrimstoneHomer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Homer");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Projectile.rotation += 0.7f * Projectile.direction;

			int brimstone = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 1f);
			Main.dust[brimstone].noGravity = true;

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 500f, 8f, 20f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
        }
    }
}
