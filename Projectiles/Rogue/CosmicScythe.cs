using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class CosmicScythe : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Boss/SignusScythe";

        private int originalDamage;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cosmic Scythe");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 400;
            Projectile.alpha = 100;
            Projectile.penetrate = 5;
            Projectile.Calamity().rogue = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation += 0.5f * (float)Projectile.direction;
            int shadow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 1f);
            Main.dust[shadow].noGravity = true;
            Main.dust[shadow].velocity *= 0f;
            Projectile.velocity *= 0.95f;
            if (Projectile.timeLeft == 400)
            {
                originalDamage = Projectile.damage;
                Projectile.damage = 0;
            }
            if (Projectile.timeLeft <= 375)
            {
                if (Projectile.timeLeft > 350)
                    Projectile.velocity *= 1.06f;
                Projectile.damage = (int)(originalDamage * 1.25);
				CalamityGlobalProjectile.HomeInOnNPC(Projectile, true, 1500f, 20f, 20f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			int buffType = Projectile.ai[0] == 1f ? BuffID.ShadowFlame : ModContent.BuffType<GodSlayerInferno>();
			target.AddBuff(buffType, 60, false);
            Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
			CalamityGlobalProjectile.ExpandHitboxBy(Projectile, 50);
            for (int d = 0; d < 4; d++)
            {
                int shadow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0f, 0f, 100, default, 2f);
                Main.dust[shadow].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[shadow].scale = 0.5f;
                    Main.dust[shadow].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int d = 0; d < 12; d++)
            {
                int shadow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0f, 0f, 100, default, 3f);
                Main.dust[shadow].noGravity = true;
                Main.dust[shadow].velocity *= 5f;
                shadow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0f, 0f, 100, default, 2f);
                Main.dust[shadow].velocity *= 2f;
            }
        }
    }
}
