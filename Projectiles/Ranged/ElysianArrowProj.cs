using CalRD.Buffs.DamageOverTime;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Ranged
{
    public class ElysianArrowProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Ammo/ElysianArrow";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Arrow");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 32;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.damage /= 2;
            Projectile.Damage();
            for (int num621 = 0; num621 < 2; num621++)
            {
                int num622 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 6; num623++)
            {
                int num624 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
            }
			CalamityUtils.ExplosionGores(Projectile.GetSource_FromThis(), Projectile.Center, 3);
            float x = Projectile.position.X + (float)Main.rand.Next(-400, 400);
            float y = Projectile.position.Y - (float)Main.rand.Next(500, 800);
            Vector2 vector = new Vector2(x, y);
            float num15 = Projectile.position.X + (float)(Projectile.width / 2) - vector.X;
            float num16 = Projectile.position.Y + (float)(Projectile.height / 2) - vector.Y;
            num15 += (float)Main.rand.Next(-100, 101);
            int num17 = 25;
            float num18 = (float)Math.Sqrt((double)(num15 * num15 + num16 * num16));
            num18 = (float)num17 / num18;
            num15 *= num18;
            num16 *= num18;
            if (Projectile.owner == Main.myPlayer)
            {
                int num19 = Projectile.NewProjectile(Entity.GetSource_FromThis(), x, y, num15, num16, ModContent.ProjectileType<SkyFlareFriendly>(), Projectile.damage, 5f, Projectile.owner, 0f, 0f);
                Main.projectile[num19].ai[1] = Projectile.position.Y;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 360);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
