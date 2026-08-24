using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class BlueExoArrow : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/LaserProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            float num55 = 40f;
            float num56 = 1.5f;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.localAI[0] += num56;
                if (Projectile.localAI[0] > num55)
                {
                    Projectile.localAI[0] = num55;
                }
            }
            else
            {
                Projectile.localAI[0] -= num56;
                if (Projectile.localAI[0] <= 0f)
                {
                    Projectile.Kill();
                }
            }
        }

        public override Color? GetAlpha(Color lightColor) => new Color(0, 0, 250, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor) => Projectile.DrawBeam(40f, 1.5f, lightColor);

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			target.ExoDebuffs();
			OnHitEffects(target.Center);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.ExoDebuffs();
			OnHitEffects(target.Center);
        }

		private void OnHitEffects(Vector2 targetPos)
		{
            for (int x = 0; x < 3; x++)
            {
				if (Projectile.owner == Main.myPlayer)
				{
					CalamityUtils.ProjectileBarrage(Projectile.GetSource_FromThis(), Projectile.Center, targetPos, Main.rand.NextBool(2), 500f, 500f, 0f, 500f, 10f, ModContent.ProjectileType<BlueExoArrow2>(), (int)(Projectile.damage * 0.7), Projectile.knockBack * 0.7f, Projectile.owner);
				}
            }
        }
    }
}
