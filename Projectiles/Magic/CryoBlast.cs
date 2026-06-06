using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class CryoBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blast");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 35;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
			Projectile.coldDamage = true;
        }

        public override void AI()
        {
            if (Projectile.scale <= 3.6f)
            {
                Projectile.scale *= 1.01f;
				CalamityGlobalProjectile.ExpandHitboxBy(Projectile, (int)(35f * Projectile.scale));
            }

			if (Projectile.timeLeft < 53)
				Projectile.alpha += 5;

            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
				int ice = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, 0f, 0f, 100, default, Projectile.scale * 0.5f);
				Main.dust[ice].noGravity = true;
				Main.dust[ice].velocity *= 0f;
				int snow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 185, 0f, 0f, 100, default, Projectile.scale * 0.5f);
				Main.dust[snow].noGravity = true;
				Main.dust[snow].velocity *= 0f;
            }
        }

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.timeLeft > 599)
				return false;

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			int height = texture.Height / Main.projFrames[Projectile.type];
			int frameHeight = height * Projectile.frame;
			Rectangle rectangle = new Rectangle(0, frameHeight, texture.Width, height);
			Vector2 origin = new Vector2(texture.Width / 2f, height / 2f);
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Main.EntitySpriteDraw(texture, drawPos, new Microsoft.Xna.Framework.Rectangle?(rectangle), lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0f);
			return false;
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 360);
            target.AddBuff(BuffID.Frostburn, 360);
        }
    }
}
