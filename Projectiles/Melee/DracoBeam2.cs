using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee
{
    public class DracoBeam2 : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Melee/DracoBeam";

        private int start = 60;
        private int speedTimer = 120;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Beam");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = 27;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            start--;
            if (start <= 0)
            {
                speedTimer--;
                if (speedTimer > 60)
                {
                    Projectile.velocity.X = 0f;
                    Projectile.velocity.Y = 10f;
                }
                else if (speedTimer <= 60)
                {
                    Projectile.velocity.X = 0f;
                    Projectile.velocity.Y = -10f;
                }
                if (speedTimer <= 0)
                {
                    speedTimer = 120;
                }
            }
            Lighting.AddLight(Projectile.Center, 0.5f, 0.25f, 0f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
			if (Projectile.timeLeft > 235)
				return false;

			CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 244, 0f, 0f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 600);
        }
    }
}
