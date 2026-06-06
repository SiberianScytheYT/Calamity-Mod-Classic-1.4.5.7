using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.DraedonsArsenal
{
    public class PulseTurretShot : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public const int SpiralPrecision = 36;
        public const int SpiralRings = 6;
        public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Pulse Bolt");
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
		}

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.Calamity().hasInorganicEnemyHitBoost = true;
            Projectile.Calamity().inorganicEnemyHitBoost = 0.002f;
			Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.ai[0] += MathHelper.ToRadians(6f);
            if (!Main.dedServ)
            {
                for (int i = 0; i < 3; i++)
                {
                    float angle = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                    float pulse = (float)Math.Sin(Projectile.ai[0] + MathHelper.TwoPi / 3f * i);
                    Vector2 offset = angle.ToRotationVector2().RotatedBy(MathHelper.TwoPi / 3f * i) * pulse * 7f;
                    Dust.NewDustPerfect(Projectile.Center + offset, 234, Vector2.Zero).noGravity = true;
                    Dust.NewDustPerfect(Projectile.Center - offset, 234, Vector2.Zero).noGravity = true;
                }
            }
            if (Projectile.ai[1] == 1f)
            {
                NPC potentialTarget = Projectile.Center.MinionHoming(850f, Main.player[Projectile.owner], false);
                if (potentialTarget != null)
                {
                    float speed = Projectile.velocity.Length();
                    float inertia = MathHelper.Lerp(18f, 6f, 1f - Projectile.timeLeft / 300f);
                    Projectile.velocity = (Projectile.velocity * inertia + Projectile.DirectionTo(potentialTarget.Center) * speed) / (inertia + 1f);
                    Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX) * speed;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Main.dedServ)
            {
                // Spirals
                for (int i = 0; i < SpiralPrecision; i++)
                {
                    for (int direction = -1; direction <= 1; direction += 2)
                    {
                        for (int j = 0; j < SpiralRings; j++)
                        {
                            Dust dust = Dust.NewDustPerfect(Projectile.Center +
                                Vector2.UnitY.RotatedBy(j / (float)SpiralRings * MathHelper.TwoPi * direction).RotatedBy(i / (float)SpiralPrecision * MathHelper.TwoPi / SpiralRings * direction) *
                                36f * i / SpiralPrecision, 173);
                            dust.velocity = Projectile.DirectionTo(dust.position) * 2.4f;
                            dust.scale = 1.6f;
                            dust.noGravity = true;

                            dust = Dust.NewDustPerfect(Projectile.Center +
                                Vector2.UnitY.RotatedBy(j / (float)SpiralRings * MathHelper.TwoPi * direction).RotatedBy(i / (float)SpiralPrecision * MathHelper.TwoPi / SpiralRings * direction) *
                                36f * i / SpiralPrecision, 173);
                            dust.velocity = Projectile.DirectionFrom(dust.position) * 4f;
                            dust.scale = 1.6f;
                            dust.noGravity = true;
                        }
                    }
                }
            }
        }
    }
}
