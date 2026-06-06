using CalRD.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class CobaltKunaiProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/CobaltKunai";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Kunai");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.aiStyle = 2;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.ThrowingKnife;
            Projectile.Calamity().rogue = true;
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
                Projectile.ai[0] += 0.1f;
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

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.rand.NextBool(2))
            {
                Item.NewItem(Entity.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<CobaltKunai>());
            }
        }
    }
}
