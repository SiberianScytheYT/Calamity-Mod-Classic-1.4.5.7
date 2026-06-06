using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class TerraArrowSplit : ModProjectile
    {
        public override string Texture => "CalRD/Items/Ammo/TerraArrow";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 120;
            Projectile.aiStyle = 1;
        }

        public override void AI()
        {
            Projectile.alpha -= 5;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 300f, 20f, 20f);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item60, Projectile.position);
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int num621 = 0; num621 < 2; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 107, 0f, 0f, 100, default, 2f);
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
        }
    }
}
