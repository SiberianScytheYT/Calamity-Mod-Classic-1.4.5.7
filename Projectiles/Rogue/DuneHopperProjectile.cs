using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class DuneHopperProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/DuneHopper";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scourge");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.penetrate = 6;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 20f)
            {
                if (Projectile.velocity.Y < 24f)
                {
                    Projectile.velocity.Y += 0.4f;
                }
            }
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.ToRadians(45f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], Projectile.Calamity().stealthStrike ? 4 : 2);
            return false;
        }
    }
}
