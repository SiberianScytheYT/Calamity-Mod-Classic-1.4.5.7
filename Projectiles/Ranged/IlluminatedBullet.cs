using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class IlluminatedBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Illuminated Bullet");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;

            // Invisible for the first few frames
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Lighting.AddLight(Projectile.Center, 1f, 0.7f, 0f);

            // Projectile becomes visible after a few frames
            if (Projectile.timeLeft == 298)
                Projectile.alpha = 0;

            // Once projectile is visible, spawn trailing sparkles
            if (Projectile.timeLeft <= 298 && Main.rand.NextBool(5))
            {
                int idx = Dust.NewDust(Projectile.Center, 1, 1, 228, 0f, 0f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].noLight = true;
                Main.dust[idx].position = Projectile.Center;
                Main.dust[idx].velocity = Vector2.Zero;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 360);
        }

        // On impact, make impact dust and play a sound.
        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}
