using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class IceCluster : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ice");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
			Projectile.coldDamage = true;
        }

        public override void AI()
        {
            Projectile.rotation += 0.5f;
            if (Projectile.localAI[1] == 0f)
            {
                Projectile.localAI[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item120, Projectile.position);
            }
            Projectile.ai[0] += 1f;
            if (Projectile.ai[1] == 1f)
            {
                if (Projectile.ai[0] >= 130f)
                {
                    Projectile.alpha += 10;
                }
                else
                {
                    Projectile.alpha -= 10;
                }
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                }
                if (Projectile.ai[0] >= 150f)
                {
                    return;
                }
                if (Projectile.ai[0] % 30f == 0f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 vector80 = Projectile.rotation.ToRotationVector2();
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vector80.X, vector80.Y, ModContent.ProjectileType<IceCluster>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
                Lighting.AddLight(Projectile.Center, 0.3f, 0.75f, 0.9f);
            }
            else
            {
                if (Projectile.ai[0] >= 40f)
                {
                    Projectile.alpha += 3;
                }
                else
                {
                    Projectile.alpha -= 40;
                }
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                }
                if (Projectile.ai[0] >= 45f)
                {
                    return;
                }
                Vector2 value47 = new Vector2(0f, -720f).RotatedBy((double)Projectile.velocity.ToRotation(), default);
                float scaleFactor8 = Projectile.ai[0] % 45f / 45f;
                Vector2 spinningpoint = value47 * scaleFactor8;
                for (int num844 = 0; num844 < 6; num844++)
                {
                    Vector2 vector81 = Projectile.Center + spinningpoint.RotatedBy((double)((float)num844 * 6.28318548f / 6f), default);
                    Lighting.AddLight(vector81, 0.3f, 0.75f, 0.9f);
                    for (int num845 = 0; num845 < 2; num845++)
                    {
                        int num846 = Dust.NewDust(vector81 + Utils.RandomVector2(Main.rand, -8f, 8f) / 2f, 8, 8, 197, 0f, 0f, 100, Color.Transparent, 1f);
                        Main.dust[num846].noGravity = true;
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 5;
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
			if (target.type == NPCID.TargetDummy)
				return;
            Vector2 vector80 = Projectile.rotation.ToRotationVector2();
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vector80.X, vector80.Y, ModContent.ProjectileType<IceCluster>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
        }
    }
}
