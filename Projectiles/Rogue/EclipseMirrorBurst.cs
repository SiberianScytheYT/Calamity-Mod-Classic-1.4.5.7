using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class EclipseMirrorBurst : ModProjectile
    {
        private int frameCounter = 0;
        private int frameX = 0;
        private int frameY = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eclipse Mirror Flash");
        }

        public override void SetDefaults()
        {
            Projectile.width = 750;
            Projectile.height = 750;
            Projectile.friendly = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 150;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            frameCounter++;
            if (frameCounter > 3)
            {
                frameCounter = 0;
                frameY++;
                if (frameY > 1)
                {
                    frameX++;
                    frameY = 0;
                }
            }
            if (frameX > 0 && frameY > 0)
            {
                Projectile.Kill();
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f - 50,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height - 750 * 0.5f - 50
                ),
                new Rectangle(frameX * 752, frameY * 752, 750, 750),
                Color.White,
                Projectile.rotation,
                new Vector2(325, 325),
                Projectile.scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
