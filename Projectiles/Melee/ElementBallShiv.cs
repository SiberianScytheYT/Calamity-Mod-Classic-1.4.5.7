using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee
{
    public class ElementBallShiv : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shiv");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
            Projectile.aiStyle = 27;
        }

        public override void AI()
        {
            int rainbow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, (float)(Projectile.direction * 2), 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.3f);
            Main.dust[rainbow].noGravity = true;
            Main.dust[rainbow].velocity = Vector2.Zero;

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 600f, 36f, 20f);
        }

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.timeLeft > 115)
				return false;

			return true;
		}

		public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 4; k++)
            {
                int rainbow = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 66, (float)(Projectile.direction * 2), 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                Main.dust[rainbow].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			OnHitEffects(target.Center);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			OnHitEffects(target.Center);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
        }

		private void OnHitEffects(Vector2 targetPos)
		{
			for (int x = 0; x < 4; x++)
			{
				if (Projectile.owner == Main.myPlayer)
				{
					CalamityUtils.ProjectileBarrage(Projectile.GetSource_FromThis(), Projectile.Center, targetPos, x > 2, 800f, 800f, 0f, 800f, 1f, ModContent.ProjectileType<SHIV>(), Projectile.damage, Projectile.knockBack, Projectile.owner, false, 50f);
				}
			}
        }
    }
}
