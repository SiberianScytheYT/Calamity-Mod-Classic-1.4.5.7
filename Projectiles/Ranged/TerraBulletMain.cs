using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Ranged
{
    public class TerraBulletMain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bullet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
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
            Projectile.alpha = 255;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0.25f / 255f, (255 - Projectile.alpha) * 0f / 255f);
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] >= 6f)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 17;
                }
                for (int num136 = 0; num136 < 2; num136++)
                {
                    float x2 = Projectile.position.X - Projectile.velocity.X / 10f * (float)num136;
                    float y2 = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)num136;
                    int num137 = Dust.NewDust(new Vector2(x2, y2), 1, 1, 74, 0f, 0f, 0, default, 0.8f);
                    Main.dust[num137].alpha = Projectile.alpha;
                    Main.dust[num137].position.X = x2;
                    Main.dust[num137].position.Y = y2;
                    Main.dust[num137].velocity *= 0f;
                    Main.dust[num137].noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int b = 0; b < 2; b++)
                {
					Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<TerraBulletSplit>(), (int)(Projectile.damage * 0.3), 0f, Projectile.owner, 0f, 0f);
                }
            }
            SoundEngine.PlaySound(SoundID.Item118, Projectile.position);
        }
    }
}
