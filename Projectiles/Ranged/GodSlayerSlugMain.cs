using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Ranged
{
	public class GodSlayerSlugMain : ModProjectile
    {
        private const int Lifetime = 600;
        private const int NoDrawFrames = 2;
        
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("God Slayer Slug");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 1;
            AIType = 242;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 5;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = Lifetime;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < Lifetime - NoDrawFrames)
                Projectile.alpha = 100;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 140);

        public override bool PreDraw(ref Color lightColor)
        {
            if(Projectile.timeLeft < Lifetime - NoDrawFrames)
                CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 0, drawCentered: false);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            return true;
        }
    }
}
