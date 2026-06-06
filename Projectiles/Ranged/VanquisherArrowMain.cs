using CalRD.Buffs.DamageOverTime;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class VanquisherArrowMain : ModProjectile
    {
        public override string Texture => "CalRD/Items/Ammo/VanquisherArrow";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.arrow = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 90;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            if (Projectile.timeLeft % 60 == 0)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0.25f, ModContent.ProjectileType<VanquisherArrowSplit>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Color color = Color.White;
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                color = new Color(b2, b2, b2, a2);
            }
            Rectangle frame = new Rectangle(0, 0, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/Items/Ammo/VanquisherArrowGlow").Value, Projectile.Center - Main.screenPosition, frame, color, Projectile.rotation, Projectile.Size / 2, 1f, SpriteEffects.None, 0f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return new Color(255, 255, 255, 100);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
        }
    }
}
