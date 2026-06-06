using CalRD.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class IronFranciscaProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/IronFrancisca";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Iron Francisca");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 1f)
            {
                Projectile.penetrate = -1;
            }

            float rotateratio = 0.019f;
            float rotation = (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * rotateratio;
            Projectile.rotation += rotation * Projectile.direction;

            Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            if (Projectile.ai[0] == 0)
            {
                Projectile.damage *= 2;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 10)
            {
                Projectile.damage /= 2;
            }

            return;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.rand.Next(4) == 0)
            {
                Item.NewItem(Entity.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<IronFrancisca>());
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            Projectile.Kill();
            return false;
        }
    }
}
