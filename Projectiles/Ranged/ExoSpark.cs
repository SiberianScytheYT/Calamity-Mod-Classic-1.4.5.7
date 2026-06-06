using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Ranged
{
    public class ExoSpark : ModProjectile
    {
        public static readonly int[] FrameToDustIDTable = new int[]
        {
            107,
            234,
            269,
        };
        public const float HomingInertia = 10f;
        public const float MaxTargetDistance = 750f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Exo Spark");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 4;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.frame = Main.rand.Next(3);
                Projectile.localAI[0] = 1f;
                Projectile.netUpdate = true;
            }
            NPC potentialTarget = Projectile.Center.ClosestNPCAt(MaxTargetDistance);
            if (potentialTarget != null)
            {
                Projectile.velocity = (Projectile.velocity * (HomingInertia - 1) + Projectile.DirectionTo(potentialTarget.Center) * 16f) / HomingInertia;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (!Main.dedServ)
            {
                GenerateCircularDust();
            }
        }

        public void GenerateCircularDust()
        {
            for (int i = 0; i < 12; i++)
            {
                float angle = i / 12f * MathHelper.TwoPi;
                Vector2 spawnPosition = Projectile.Center + angle.ToRotationVector2().RotatedBy(Projectile.rotation) * new Vector2(10f, 6f);
                Dust dust = Dust.NewDustPerfect(spawnPosition, FrameToDustIDTable[Projectile.frame]);
                dust.velocity = Vector2.Zero;
                dust.scale = 0.5f;
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 60);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 60);
            target.AddBuff(ModContent.BuffType<Plague>(), 60);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 60);
            target.AddBuff(BuffID.CursedInferno, 120);
            target.AddBuff(BuffID.Frostburn, 120);
            target.AddBuff(BuffID.OnFire, 120);
            target.AddBuff(BuffID.Ichor, 120);
        }
    }
}
