using CalRD.Buffs.Summon;
using CalRD.Dusts;
using CalRD.Items.Weapons.Ranged;
using CalRD.CalPlayer;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Summon
{
    public class TacticalPlagueEngineSummon : ModProjectile
    {
        public static Item FalseGun = null;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tactical Plague Jet");
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 32;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.Calamity().spawnedPlayerMinionDamageValue = player.MinionDamage();
                Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue = Projectile.damage;
                for (int i = 0; i < 45; i++)
                {
                    float angle = MathHelper.TwoPi / 45f * i;
                    Vector2 velocity = angle.ToRotationVector2() * 4f;
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + velocity * 2f, (int)CalamityDusts.Plague, velocity);
                    dust.noGravity = true;
                }
                FalseGun = ItemLoader.GetItem(ModContent.ItemType<P90>()).Item;
                Projectile.localAI[0] = 1f;
            }
            if (Projectile.frameCounter++ > 6f)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
            if (player.MinionDamage() != Projectile.Calamity().spawnedPlayerMinionDamageValue)
            {
                int trueDamage = (int)((float)Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue /
                    Projectile.Calamity().spawnedPlayerMinionDamageValue *
                    player.MinionDamage());
                Projectile.damage = trueDamage;
            }
            bool isCorrectProjectile = Projectile.type == ModContent.ProjectileType<TacticalPlagueEngineSummon>();
            player.AddBuff(ModContent.BuffType<TacticalPlagueEngineBuff>(), 3600);
            if (isCorrectProjectile)
            {
                if (player.dead)
                {
                    modPlayer.plagueEngine = false;
                }
                if (modPlayer.plagueEngine)
                {
                    Projectile.timeLeft = 2;
                }
            }
            NPC potentialTarget = Projectile.Center.MinionHoming(1560f, player);

            if (potentialTarget is null || !player.HasAmmo(FalseGun))
            {
                float acceleration = 0.1f;
                Vector2 distanceVector = player.Center - Projectile.Center;
                if (distanceVector.Length() < 200f)
                {
                    acceleration = 0.07f;
                }
                if (distanceVector.Length() < 140f)
                {
                    acceleration = 0.035f;
                }
                if (distanceVector.Length() > 100f)
                {
                    if (Math.Abs(player.Center.X - Projectile.Center.X) > 20f)
                    {
                        Projectile.velocity.X += acceleration * Math.Sign(player.Center.X - Projectile.Center.X);
                    }
                    if (Math.Abs(player.Center.Y - Projectile.Center.Y) > 10f)
                    {
                        Projectile.velocity.Y += acceleration * Math.Sign(player.Center.Y - Projectile.Center.Y);
                    }
                }
                else if (Projectile.velocity.Length() > 4f)
                {
                    Projectile.velocity *= 0.95f;
                }
                if (Math.Abs(Projectile.velocity.Y) < 2f)
                {
                    Projectile.velocity.Y += 0.1f * Math.Sign(player.Center.Y - Projectile.Center.Y);
                }
                if (Projectile.velocity.Length() > 9f)
                {
                    Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 9f;
                }

                if (Projectile.velocity.X > 0.25f)
                {
                    Projectile.spriteDirection = 1;
                }
                else if (Projectile.velocity.X < -0.25f)
                {
                    Projectile.spriteDirection = -1;
                }
                Projectile.rotation = Projectile.rotation.AngleTowards(0f, 0.2f);

                if (distanceVector.Length() > 2700f)
                {
                    Projectile.Center = player.Center;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.spriteDirection = 1;
                Vector2 idealVelocity = Projectile.DirectionTo(potentialTarget.Center - Vector2.UnitY * 195f) * 17f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, idealVelocity, 0.035f);
                Projectile.rotation = Projectile.rotation.AngleTowards(Projectile.AngleTo(potentialTarget.Center), 0.1f);

                if (Projectile.ai[0]++ % 75f == 24f)
                {
                    int damage = Projectile.damage;
                    if (Main.rand.NextBool(20))
                    {
                        int idx = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(potentialTarget.Center) * 18f, ModContent.ProjectileType<MK2RocketHoming>(),
                            (int)(damage * 1.5), 5f, Projectile.owner);
                        Main.projectile[idx].Calamity().forceMinion = true;
                    }
                    else
                    {
                        int shoot = 0;
                        float shootSpeed = 0f;
                        bool canShoot = true;
                        float knockBack = Projectile.knockBack * 0.5f;
                        player.PickAmmo(FalseGun, out shoot, out shootSpeed, out damage, out knockBack, out _);
                        int idx = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(potentialTarget.Center) * shootSpeed, shoot,
                            damage, Projectile.knockBack, Projectile.owner);
                        // There's airway for a small bug in here, but the potential alterative (that has indeed been appearing), where
                        // the projectile simply cannot exist, is far worse than this. If you have another solution, let me know.
                        if (idx >= 0 && idx < Main.projectile.Length)
                        {
                            Main.projectile[idx].Calamity().forceMinion = true;
                        }
                    }
                }
				Projectile.MinionAntiClump(0.25f);
            }
        }

        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */ => false;
    }
}
