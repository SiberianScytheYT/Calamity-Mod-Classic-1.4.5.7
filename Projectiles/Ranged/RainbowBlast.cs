using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
	public class RainbowBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blast");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            //Rotation
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);

            Lighting.AddLight(Projectile.Center, new Vector3(Main.DiscoR, Main.DiscoG, Main.DiscoB) * (1.5f / 255));

            Projectile.localAI[0] ++;
            if (Projectile.localAI[0] > 5f)
            {
                Vector2 dspeed = -Projectile.velocity * 0.5f;
                int num469 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 66, dspeed.X, dspeed.Y, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.1f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity = dspeed;
            }

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 200f, 18f, 20f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
            return color;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
