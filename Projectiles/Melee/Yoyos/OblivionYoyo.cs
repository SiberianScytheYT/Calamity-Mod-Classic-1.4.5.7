using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Melee.Yoyos
{
	public class OblivionYoyo : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Oblivion");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 400f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 16.5f;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 99;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 1.1f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
			Projectile.MaxUpdates = 2;
		}

        public override void AI()
        {
			CalamityGlobalProjectile.MagnetSphereHitscan(Projectile, 300f, 6f, 48f, 5, ModContent.ProjectileType<BrimstoneLaserFriendly>(), 0.5);
			if ((Projectile.position - Main.player[Projectile.owner].position).Length() > 3200f) //200 blocks
				Projectile.Kill();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
