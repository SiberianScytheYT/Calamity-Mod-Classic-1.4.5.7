using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class HadopelagicEcho2 : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Magic/EidolicWailSoundwave";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Echo");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.scale = 0.85f;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 30;
			Projectile.extraUpdates = 1;
			Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.width = (int)(36f * Projectile.scale);
			Projectile.height = (int)(36f * Projectile.scale);
            if (Projectile.alpha > 100)
            {
                Projectile.alpha -= 25;
            }
			if (Projectile.alpha < 100)
				Projectile.alpha = 100;
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);

			if (Projectile.ai[0] == 1f)
                CalamityGlobalProjectile.HomeInOnNPC(Projectile, true, 500f, 8f, 20f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 600);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * ((float)b2 / 255f));
                return new Color((int)b2, (int)b2, (int)b2, (int)a2);
            }
            return new Color(255, 255, 255, 100);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
