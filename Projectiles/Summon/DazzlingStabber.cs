using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.Summon;
using CalRD.CalPlayer;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Summon
{
	public class DazzlingStabber : ModProjectile
    {
        public float NPCTargetTimer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public float IdleOffsetAngle
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public const float DistanceToCheck = 1500f;
        public const float TeleportSlice = 40f;
        public static readonly float TeleportSliceAngleMax = MathHelper.ToRadians(23f);
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dazzling Stabber");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 58;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3());
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.Calamity().spawnedPlayerMinionDamageValue = player.MinionDamage();
                Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue = Projectile.damage;
                Projectile.localAI[0] = 1f;
            }
            if (player.MinionDamage() != Projectile.Calamity().spawnedPlayerMinionDamageValue)
            {
                int trueDamage = (int)(Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue /
                    Projectile.Calamity().spawnedPlayerMinionDamageValue *
                    player.MinionDamage());
                Projectile.damage = trueDamage;
            }
            bool isProperProjectile = Projectile.type == ModContent.ProjectileType<DazzlingStabber>();
            player.AddBuff(ModContent.BuffType<DazzlingStabberBuff>(), 3600);
            if (isProperProjectile)
            {
                if (player.dead)
                {
                    modPlayer.providenceStabber = false;
                }
                if (modPlayer.providenceStabber)
                {
                    Projectile.timeLeft = 2;
                }
            }

            NPC potentialTarget = Projectile.Center.MinionHoming(DistanceToCheck, player);

            if (Projectile.frameCounter++ > 6)
            {
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
                Projectile.frameCounter = 0;
            }

            if (potentialTarget is null)
            {
                Projectile.rotation = Projectile.rotation.AngleTowards(IdleOffsetAngle, 0.05f);
                Vector2 destination = player.Center + new Vector2(0f, -120f).RotatedBy(IdleOffsetAngle);
                Projectile.velocity = (destination - Projectile.Center) / 15f;
            }
            else
            {
                NPCTargetTimer++;
                // Alternate between normal charge and slower charge/knife summon
                if (NPCTargetTimer % 160f < 100f)
                {
                    ChargeAttack(potentialTarget);
                }
                // Teleport onto the target and just the rotation for a slice.
                else if (NPCTargetTimer % 160f == 160f - TeleportSlice)
                {
                    TeleportOntoTarget(potentialTarget);
                }
                // Slice the target and don't move.
                else if (NPCTargetTimer % 160f > 160f - TeleportSlice)
                {
                    Projectile.rotation -= TeleportSliceAngleMax * 2f / TeleportSlice;
                }
            }
            Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
        }

        public void ChargeAttack(NPC target)
        {
            if (NPCTargetTimer % 160f < 30f)
            {
                Projectile.velocity *= 0.99f;
                Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(target.Center) + MathHelper.PiOver2, 0.25f);
            }
            else if (NPCTargetTimer % 160f == 30f)
            {
                Projectile.velocity = Projectile.DirectionTo(target.Center) * 20f;
            }
            else if (NPCTargetTimer % 160f < 60f)
            {
                Projectile.velocity *= 0.99f;
                Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(target.Center) + MathHelper.PiOver2, 0.25f);
            }
            else if (NPCTargetTimer % 160f == 60f)
            {
                Projectile.velocity = Projectile.DirectionTo(target.Center) * 16f;
                for (int i = 0; i < 3; i++)
                {
                    float angle = MathHelper.Lerp(-0.3f, 0.3f, i / 3f);
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(angle), ModContent.ProjectileType<DazzlingStabberKnife>(), (int)(Projectile.damage * 0.25), 1f, Projectile.owner);
                }
            }
            else Projectile.velocity *= 0.99f;
        }

        public void TeleportOntoTarget(NPC target)
        {
            // Spawn a spiral of holy flame dust.
            float angleStart = Main.rand.NextFloat(MathHelper.TwoPi);
            for (int i = 0; i < 30; i++)
            {
                float angle = MathHelper.TwoPi / 30f * i + angleStart;
                Dust dust = Dust.NewDustPerfect(Projectile.Center + angle.ToRotationVector2() * 10f, (int)CalamityDusts.ProfanedFire);
                dust.velocity = angle.ToRotationVector2() * 5f * MathHelper.Lerp(1f, 0f, i % 6f / 6f);
            }

            Vector2 teleportOffset = Utils.RandomVector2(Main.rand, -12f, 12f);

            Projectile.Center = target.Center + teleportOffset;
            Projectile.rotation = Projectile.AngleTo(target.Center) + MathHelper.PiOver2 + TeleportSliceAngleMax;
            Projectile.velocity = Vector2.Zero;
            Projectile.netUpdate = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*{
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }*/
    }
}
