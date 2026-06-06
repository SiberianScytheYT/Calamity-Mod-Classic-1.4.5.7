using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee
{
    public class TerrorBeam : ModProjectile
    {
        private bool hasHitEnemy = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Beam");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 7;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.light = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3;
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
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<TerrorBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.owner == Main.myPlayer && !hasHitEnemy)
            {
                hasHitEnemy = true;
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<TerrorBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
        }

        public override void AI()
        {
            if (Projectile.localAI[1] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item60, Projectile.position);
                Projectile.localAI[1] += 1f;
            }
            Projectile.alpha -= 40;
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.785f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 0, 0, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
			if (Projectile.timeLeft > 595)
				return false;

			CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item60, Projectile.position);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 400;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int num193 = 0; num193 < 6; num193++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 60, 0f, 0f, 100, default, 1.5f);
            }
            for (int num194 = 0; num194 < 60; num194++)
            {
                int num195 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 60, 0f, 0f, 0, default, 2.5f);
                Main.dust[num195].noGravity = true;
                Main.dust[num195].velocity *= 3f;
                num195 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 60, 0f, 0f, 100, default, 1.5f);
                Main.dust[num195].velocity *= 2f;
                Main.dust[num195].noGravity = true;
            }
            Projectile.Damage();
        }
    }
}
