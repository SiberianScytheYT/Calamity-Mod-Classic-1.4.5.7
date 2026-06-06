using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class AstralRound : ModProjectile
    {
		private float speed = 0f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Round");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.25f;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override bool PreAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Projectile.spriteDirection = Projectile.direction;
            if (Main.rand.NextBool(2))
            {
				int randomDust = Utils.SelectRandom(Main.rand, new int[]
				{
					ModContent.DustType<AstralOrange>(),
					ModContent.DustType<AstralBlue>()
				});
                int astral = Dust.NewDust(Projectile.position, 1, 1, randomDust, 0f, 0f, 0, default, 0.5f);
                Main.dust[astral].alpha = Projectile.alpha;
                Main.dust[astral].velocity *= 0f;
                Main.dust[astral].noGravity = true;
            }

			if (speed == 0f)
				speed = Projectile.velocity.Length();
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 200f, speed, 12f);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
        }
    }
}
