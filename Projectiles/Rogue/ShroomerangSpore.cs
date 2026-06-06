using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class ShroomerangSpore : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Ranged/FungiOrb";

		private bool initialized = false;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spore");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 3600;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
			if (!initialized)
			{
				initialized = true;
				Projectile.localAI[0] = Main.rand.Next(-60, 61); //used for mycoroot stealth strike for minor randomization
			}

            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0.35f / 255f, (255 - Projectile.alpha) * 0.5f / 255f);
			Projectile.SporeSacAI();
        }

        public override void OnKill(int timeLeft)
        {
			CalamityGlobalProjectile.ExpandHitboxBy(Projectile, 56);
            int num226 = 36;
            for (int num227 = 0; num227 < num226; num227++)
            {
                Vector2 vector6 = Vector2.Normalize(Projectile.velocity) * new Vector2((float)Projectile.width / 2f, (float)Projectile.height) * 0.75f;
                vector6 = vector6.RotatedBy((double)((float)(num227 - (num226 / 2 - 1)) * 6.28318548f / (float)num226), default) + Projectile.Center;
                Vector2 vector7 = vector6 - Projectile.Center;
                int num228 = Dust.NewDust(vector6 + vector7, 0, 0, 56, vector7.X, vector7.Y, 100, default, 0.5f);
                Main.dust[num228].noGravity = true;
                Main.dust[num228].noLight = true;
                Main.dust[num228].velocity = vector7;
            }
        }
    }
}
