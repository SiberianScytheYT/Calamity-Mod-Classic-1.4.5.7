using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Boss
{
    public class BrimstoneHellblast2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimstone Hellblast");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1500;
			Projectile.Opacity = 0f;
            CooldownSlot = 1;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
                Projectile.frame = 0;

			if (Projectile.timeLeft < 60)
				Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, 1f);
			else
				Projectile.Opacity = MathHelper.Clamp(1f - ((Projectile.timeLeft - 1440) / 60f), 0f, 1f);

            Lighting.AddLight(Projectile.Center, 0.9f, 0f, 0f);

            if (Projectile.velocity.X < 0f)
            {
                Projectile.spriteDirection = -1;
                Projectile.rotation = (float)Math.Atan2(-Projectile.velocity.Y, -Projectile.velocity.X);
            }
            else
            {
                Projectile.spriteDirection = 1;
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int drawStart = frameHeight * Projectile.frame;
			lightColor.R = (byte)(255 * Projectile.Opacity);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, drawStart, texture.Width, frameHeight)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), Projectile.scale, spriteEffects, 0f);
            return false;
        }

		public override bool CanHitPlayer(Player target) => Projectile.Opacity == 1f;

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if (Projectile.Opacity != 1f)
				return;

			target.AddBuff(ModContent.BuffType<AbyssalFlames>(), 180);
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120, true);
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)	
        {
			target.Calamity().lastProjectileHit = Projectile;
		}
    }
}
