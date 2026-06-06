using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class PhantasmalRuinGhost : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Phantasmal Ghost");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 240;
            Projectile.extraUpdates = 1;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
			if (Projectile.timeLeft >= 207)
			{
				Projectile.alpha += 6;
			}
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.785f;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 175, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default(Color), 0.85f);
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 600f, 18f, 20f);
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 110;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
			Projectile.damage /= 2;
            Projectile.Damage();
        }

        public override bool PreDraw(ref Color lightColor)
        {
			if (Projectile.timeLeft > 239)
				return false;

            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
