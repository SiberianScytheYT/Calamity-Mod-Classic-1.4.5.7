using CalRD.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Magic
{
    public class EternityHex : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public int TargetNPCIndex
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public float LemniscateAngle
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public float Time
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        public int BookProjectileIndex
        {
            get => (int)Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }
        public const int Lifetime = 310;
        public const float BossLifeMaxDamageMult = 1f / 350f;
        public const float NormalEnemyLifeMaxDamageMult = 1f / 100f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eternity");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (TargetNPCIndex >= Main.npc.Length || TargetNPCIndex < 0)
            {
                DeathDust();
                Projectile.Kill();
                return;
            }

            NPC target = Main.npc[TargetNPCIndex];

            // Delete the hex (and everything else by extension) if any necessary components are incorrect/would cause errors.
            if (BookProjectileIndex >= Main.projectile.Length || BookProjectileIndex < 0 || Time < 0)
            {
                DeathDust();
                Projectile.Kill();
                return;
            }

            if (!target.active)
            {
                NPC potentialTarget = Main.MouseWorld.ClosestNPCAt(4400f, true, true);
                if (potentialTarget != null)
                {
                    // If something happens to the original NPC, such as death, attempt to locate a new target and attack to them.
                    ChooseNewTarget(potentialTarget);
                    target = potentialTarget;
                }
                // If there is no NPC to attack to, die.
                else
                {
                    DeathDust();
                    Projectile.Kill();
                    return;
                }
            }

            Projectile book = Main.projectile[BookProjectileIndex];

            if (!book.active)
            {
                DeathDust();
                Projectile.Kill();
                return;
            }

            Time++;

            // Generate a field of dust with a color that fades to black with time in the shape of a Lemniscate of Bernoulli.
            for (int i = 0; i < 3; i++)
            {
                LemniscateAngle += MathHelper.TwoPi / 200f;
                DrawLemniscate(target);
            }
            if (Time < Lifetime * Projectile.MaxUpdates)
            {
                float effectRate = MathHelper.Lerp(0.4f, 1f, Time / (Lifetime * Projectile.MaxUpdates - 40));
                float random = Main.rand.NextFloat();

                // Spawn a bunch of swirling dust and do damage.
                if (random <= effectRate)
                    SpawnSwirlingDust(target);

                if (random <= effectRate / 30f)
                {
                    if (!target.immortal && !target.dontTakeDamage && !target.townNPC)
                    {
                        int damage = 2;
                        damage += (int)Math.Sqrt(target.width * target.height) * 10; // Damage done to Leviathan based on this formula = floor(sqrt(850 * 450) * 10) = 6184 damage.
                        damage += (int)(target.lifeMax * (target.boss ? BossLifeMaxDamageMult : NormalEnemyLifeMaxDamageMult));
                        damage += target.damage * 5;
                        damage = (int)(damage * Main.rand.NextFloat(0.9f, 1.1f));
                        damage = (int)MathHelper.Clamp(damage, 1f, Eternity.BaseDamage * player.MagicDamage() * 3);
                        target.StrikeNPC(target.CalculateHitInfo(damage, 0));
                        RegisterDPS(damage);
                    }
                }
                // This is where most of the damage comes from. Be careful when messing with this.
                if ((int)Time % 30 == 0 && CalamityUtils.CountProjectiles(ModContent.ProjectileType<EternityHoming>()) < Eternity.MaxHomers)
                {
                    int homerCount = 6;
                    int damage = (int)(Eternity.BaseDamage * player.MagicDamage() * 0.8f);

                    for (int i = 0; i < homerCount; i++)
                    {
                        Vector2 velocity = Vector2.UnitY.RotatedBy(MathHelper.TwoPi / homerCount * i).RotatedByRandom(0.3f) * 10f;
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center + velocity * 4f, velocity, ModContent.ProjectileType<EternityHoming>(), damage, 0f, Projectile.owner, TargetNPCIndex);
                    }
                }
            }
            else
            {
                Projectile.Kill();
            }
        }
        public void DeathDust()
        {
            for (int i = 0; i < 44; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, Eternity.DustID, newColor: new Color(245, 112, 218));
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 6f);
                dust.noGravity = true;
            }
        }
        public void DrawLemniscate(NPC target)
        {
            // This value causes the lemniscate to smoothen out and look better.
            float scale = 2f / (3f - (float)Math.Cos(2 * LemniscateAngle));

            float outwardMultiplier = MathHelper.Lerp(4f, 220f, Utils.GetLerpValue(0f, 120f, Time, true));
            Vector2 lemniscateOffset = scale * new Vector2((float)Math.Cos(LemniscateAngle), (float)Math.Sin(2f * LemniscateAngle) / 2f);

            Projectile.Center = target.Center + lemniscateOffset * outwardMultiplier;

            // This part isn't actually throughout the projectile's lifetime because of extra updates.
            float completionValue = Utils.GetLerpValue(0f, Lifetime, Time, true);
            Color dustColor = Color.Lerp(new Color(245, 112, 218), new Color(28, 13, 118), completionValue);

            Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, Eternity.DustID, newColor: dustColor);
            dust.velocity = Vector2.Zero;
            dust.scale = MathHelper.Lerp(0.8f, 1.5f, completionValue);
            dust.noGravity = true;
        }
        public void ChooseNewTarget(NPC newTarget)
        {
            TargetNPCIndex = newTarget.whoAmI;

            // Adjust the target index for the other components of the projectile.
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if (!proj.active)
                    continue;
                if (proj.whoAmI != Projectile.whoAmI)
                    continue;
                if (proj.type != ModContent.ProjectileType<EternityCrystal>() && proj.type != ModContent.ProjectileType<EternityCircle>())
                    continue;

                proj.ai[0] = TargetNPCIndex;
                DeathDust();
            }
        }
        public void SpawnSwirlingDust(NPC target)
        {
            for (int i = 0; i < 12; i++)
            {
                float randomAngle = Main.rand.NextFloat() * MathHelper.TwoPi;
                float outwardnessFactor = Main.rand.NextFloat();
                Vector2 spawnPosition = target.Center + randomAngle.ToRotationVector2() * MathHelper.Lerp(70f, EternityCircle.TargetOffsetRadius - 60f, outwardnessFactor);
                Vector2 velocity = (randomAngle - 3f * MathHelper.Pi / 8f).ToRotationVector2() * (10f + 9f * Main.rand.NextFloat() + 4f * outwardnessFactor);
                Dust swirlingDust = Dust.NewDustPerfect(spawnPosition, Eternity.DustID, new Vector2?(velocity), 0, Main.rand.NextBool(3) ? Eternity.BlueColor : Eternity.PinkColor, 1.4f);
                swirlingDust.scale = 0.8f;
                swirlingDust.fadeIn = 0.95f + outwardnessFactor * 0.3f;
                swirlingDust.noGravity = true;
            }
        }
        // So that the player can gauge the DPS of this weapon effectively (StrikeNPC alone will not register the DPS to the player. I have to do this myself).
        public void RegisterDPS(int damage)
        {
            Main.player[Projectile.owner].addDPS(damage);
        }
    }
}