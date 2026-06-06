using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class StellarKnifeProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/StellarKnife";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stellar Knife");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
			//synthesized timeLeft
			Projectile.localAI[1]++;
			if (Projectile.localAI[1] > 600f)
				Projectile.Kill();

            if (Projectile.ai[0] == 1f)
            {
                Projectile.ai[0] = 0f;
                Projectile.damage = (int)(Projectile.damage * (Projectile.ai[1] == 1f ? 0.9f : 0.75f));
                Projectile.ai[1] = 0f;
            }
            Projectile.ai[1] += 0.75f;
            if (Projectile.ai[1] <= 60f)
            {
                Projectile.rotation -= 1f;
                Projectile.velocity.X *= 0.985f;
                Projectile.velocity.Y *= 0.985f;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
				Projectile.localAI[0]++;

				Vector2 center = Projectile.Center;
				float maxDistance = 600f;
				bool homeIn = false;

				if (Projectile.localAI[0] >= 20f && !homeIn) //shorten knife lifespan if it hasn't found a target
					if (Projectile.timeLeft >= 60)
						Projectile.timeLeft = 60;

				for (int i = 0; i < Main.maxNPCs; i++)
				{
					if (Main.npc[i].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
					{
						float extraDistance = (float)(Main.npc[i].width / 2) + (float)(Main.npc[i].height / 2);

						if (Vector2.Distance(Main.npc[i].Center, Projectile.Center) < (maxDistance + extraDistance))
						{
							center = Main.npc[i].Center;
							homeIn = true;
							break;
						}
					}
				}

				if (homeIn)
				{
					Projectile.timeLeft = 600; //when homing in, the projectile cannot run out of timeLeft, but synthesized timeLeft still runs

					Vector2 homeInVector = Projectile.DirectionTo(center);
					if (homeInVector.HasNaNs())
						homeInVector = Vector2.UnitY;

					Projectile.velocity = (Projectile.velocity * 10f + homeInVector * 30f) / (10f + 1f);
				}
                else
                {
                    Projectile.velocity.X *= 0.92f;
                    Projectile.velocity.Y *= 0.92f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<AstralBlue>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
