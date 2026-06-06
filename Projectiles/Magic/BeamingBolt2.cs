using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class BeamingBolt2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Beaming Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.alpha = 0;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			if (Projectile.ai[0] == 1f)
				tex = ModContent.Request<Texture2D>("CalRD/Projectiles/Magic/BeamingThorn").Value;

			Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;
			int randomDust = Utils.SelectRandom(Main.rand, new int[]
			{
				164,
				58,
				204
			});
			Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, randomDust, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 400f, 20f, 20f);
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 3; k++)
            {
				int randomDust = Utils.SelectRandom(Main.rand, new int[]
				{
					164,
					58,
					204
				});
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, randomDust, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
