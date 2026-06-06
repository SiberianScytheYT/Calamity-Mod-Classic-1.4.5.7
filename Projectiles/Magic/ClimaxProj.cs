using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class ClimaxProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Climax");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.light = 0.5f;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.96f;

            if (Projectile.velocity.X > 0f)
            {
                Projectile.rotation += (Math.Abs(Projectile.velocity.Y) + Math.Abs(Projectile.velocity.X)) * 0.001f;
            }
            else
            {
                Projectile.rotation -= (Math.Abs(Projectile.velocity.Y) + Math.Abs(Projectile.velocity.X)) * 0.001f;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 4)
                {
                    Projectile.frame = 0;
                }
            }

			if (Projectile.timeLeft % 2 == Projectile.ai[0])
				CalamityGlobalProjectile.MagnetSphereHitscan(Projectile, 400f, 8f, 4f, 2, ModContent.ProjectileType<ClimaxBeam>(), 1D, true);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 30)
            {
                float num7 = Projectile.timeLeft / 30f;
                Projectile.alpha = (int)(255f - 255f * num7);
            }
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = TextureAssets.Projectile[Projectile.type].Value;
            int num214 = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y6 = num214 * Projectile.frame;
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture2D13.Width, num214)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture2D13.Width / 2f, num214 / 2f), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */ => false;
    }
}
