using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Typeless
{
    public class VanquisherArrowSplit : ModProjectile
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
            Projectile.arrow = true;
            Projectile.timeLeft = 90;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 600f, 20f, 20f);
        }

        public override void PostDraw(Color lightColor)
        {
			if (Projectile.timeLeft < 90)
			{
				Vector2 origin = new Vector2(0f, 0f);
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
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return new Color(0, 0, 0, 0);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
		}
    }
}
