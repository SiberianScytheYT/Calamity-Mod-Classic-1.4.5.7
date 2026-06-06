using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Summon
{
    public class TundraFlameBlossomsOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blossom Orb");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.minionSlots = 0f;
            Projectile.minion = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3());
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);
            // Arc around after a bit of time.
            if (Projectile.timeLeft > 110f && Projectile.timeLeft < 150f)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2 / 40f);
            }
            if (Projectile.timeLeft < 60f)
            {
                Projectile.alpha = (int)MathHelper.Lerp(0f, 255f, 1f - Projectile.timeLeft / 60f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.Frostburn, 180);

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Grass, Projectile.position);
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 179, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
