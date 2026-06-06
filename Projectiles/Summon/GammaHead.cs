using CalRD.Buffs.Summon;
using CalRD.Dusts;
using CalRD.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Summon
{
	public class GammaHead : ModProjectile
    {
        public Vector2 DeltaPosition;
        public int BulletShootCounter;
        public float AngularMultiplier1;
        public float AngularMultiplier2;
        public const int MaximumLaserCount = 6; // Otherwise intense lag ensues.
        public const float AttackStartWait = 30f;
        public const float SuperchargeTimeMax = 540f;
        public const float DistanceToCheck = 1000f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gamma Head");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 36;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
			Projectile.coldDamage = true;
        }
        public Vector2 DrawStartPosition
        {
            get
            {
                if (Projectile.owner < 0 || Projectile.owner >= Main.player.Length)
                    return Vector2.Zero;
                return Main.player[Projectile.owner].Top + Vector2.UnitY * 8f;
            }
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            BulletShootCounter = reader.ReadInt32();
            AngularMultiplier1 = reader.ReadSingle();
            AngularMultiplier2 = reader.ReadSingle();
            DeltaPosition = reader.ReadVector2();
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(BulletShootCounter);
            writer.Write(AngularMultiplier1);
            writer.Write(AngularMultiplier2);
            writer.WriteVector2(DeltaPosition);
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();

            if (Projectile.localAI[0] == 0f)
            {
                int totalHeads = CalamityUtils.CountProjectiles(Projectile.type);
                DeltaPosition = new Vector2(Main.rand.NextFloat(-42f - 6f * totalHeads, 42f + 6f * totalHeads), -Main.rand.NextFloat(8f, 64f + 7f * totalHeads));

                Projectile.Calamity().spawnedPlayerMinionDamageValue = player.MinionDamage();
                Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue = Projectile.damage;

                AngularMultiplier1 = Main.rand.NextFloat(3f);
                AngularMultiplier2 = Main.rand.NextFloat(3f);
                Projectile.netUpdate = true;
                Projectile.localAI[0] = 1f;
			}

            bool isProperProjectile = Projectile.type == ModContent.ProjectileType<GammaHead>();
            player.AddBuff(ModContent.BuffType<GammaHeadBuff>(), 3600);
            if (isProperProjectile)
            {
                if (player.dead)
                {
                    modPlayer.gammaHead = false;
                }
                if (modPlayer.gammaHead)
                {
                    Projectile.timeLeft = 2;
                }
            }

            if (modPlayer.GammaCanisters.Count > 0)
            {
                for (int i = 0; i < modPlayer.GammaCanisters.Count; i++)
                {
                    if (Projectile.Hitbox.Intersects(Main.projectile[modPlayer.GammaCanisters[i]].Hitbox) &&
                        Main.projectile[modPlayer.GammaCanisters[i]].active)
                    {
                        int laserCount = 0;
                        for (int j = 0; j < Main.projectile.Length; j++)
                        {
                            if (laserCount < MaximumLaserCount)
                            {
                                if (Main.projectile[j].type == Projectile.type && Main.projectile[j].owner == Projectile.owner &&
                                    Main.projectile[j].ai[0] == 0f && Main.projectile[j].active && Main.projectile[j].ai[1] <= 0)
                                {
                                    Main.projectile[j].ai[0] = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.UnitY, ModContent.ProjectileType<GammaDeathray>(),
                                                                                (int)(Projectile.damage * 1.2f), Projectile.knockBack, Projectile.owner, Main.projectile[j].whoAmI);

                                    Main.projectile[j].ai[1] = SuperchargeTimeMax;
                                    Main.projectile[j].netUpdate = true;
                                    laserCount++;
                                }
                            }
                            else
                            {
                                Main.projectile[j].ai[1] = SuperchargeTimeMax;
                                Main.projectile[j].netUpdate = true;
                            }
                        }

                        // Kill removes the respective canister from the list. No need to do this manually.
                        for (int j = 0; j < modPlayer.GammaCanisters.Count; j++)
                        {
                            Main.projectile[modPlayer.GammaCanisters[i]].Kill();
                        }
                        break;
                    }
                }
            }

            // Spawn gamma canisters
            else if (player.ownedProjectileCounts[Projectile.type] >= 7)
            {
                int ownedGammaHeads = player.ownedProjectileCounts[Projectile.type];
                int canisterSpawnChance = 520 - (int)(200 * (1 - 1f / (ownedGammaHeads + 6f)));
                if (Main.rand.NextBool(canisterSpawnChance * ownedGammaHeads))
                {
                    Vector2 spawnPosition = player.Center + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) * Main.rand.NextFloat(-1f, 1f);
                    Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), spawnPosition, Vector2.Zero, ModContent.ProjectileType<GammaCanister>(), 0, 0f, Projectile.owner);
                }
            }

            // Omega-complex dust.
            Projectile.localAI[1]++;
            if (Projectile.localAI[1] < 60f)
            {
                for (int i = 0; i < 5; i++)
                {
                    float angle = Projectile.localAI[1] / 60f * MathHelper.TwoPi;
                    float x = (float)Math.Sin(angle * AngularMultiplier1) * (float)Math.Cos(angle);
                    float y = (float)Math.Cos(angle * (float)Math.Cos(MathHelper.PiOver2 * AngularMultiplier2 + angle * 2f)) * (float)Math.Sin(angle);
                    Vector2 velocity = new Vector2(x * 4f, y * 4f) + player.velocity;
                    velocity = velocity.RotatedBy(MathHelper.TwoPi / 5f * i);

                    Dust dust = Dust.NewDustPerfect(Projectile.Center + angle.ToRotationVector2() * 8f, (int)CalamityDusts.SulfurousSeaAcid);
                    dust.velocity = velocity;
                    dust.scale = (float)Math.Cos(angle) + 1.3f;
                    dust.noGravity = true;
                }
            }
            // Explosion
            if (Projectile.localAI[1] == 60f)
            {
                for (int i = 0; i < 60; i++)
                {
                    float angle = MathHelper.TwoPi / 60f * i;
                    angle += Projectile.AngleFrom(player.Center) / 60f;
                    angle += (float)Math.Sin(angle) * MathHelper.PiOver2;
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + angle.ToRotationVector2() * 8f, (int)CalamityDusts.SulfurousSeaAcid);
                    dust.velocity = angle.ToRotationVector2() * new Vector2(4f, 3f * (float)Math.Cos(angle)) * 3f;
                    dust.scale = 1.3f;
                    dust.noGravity = true;
                }
            }
            if (player.MinionDamage() != Projectile.Calamity().spawnedPlayerMinionDamageValue)
            {
                int trueDamage = (int)(Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue /
                    Projectile.Calamity().spawnedPlayerMinionDamageValue *
                    player.MinionDamage());
                Projectile.damage = trueDamage;
            }

            Vector2 returnPosition = DrawStartPosition + (player.direction > 0).ToDirectionInt() * 6f * Vector2.UnitX + DeltaPosition;

            NPC potentialTarget = Projectile.Center.MinionHoming(DistanceToCheck, player);

            // Kill laser when done
            if (Projectile.ai[1] == GammaDeathray.TotalFadeoutTime && Projectile.ai[0] != 0f)
            {
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].ai[1] >= 0f && Main.projectile[i].type == Projectile.type && Main.projectile[i].owner == Projectile.owner)
                    {
                        Main.projectile[i].spriteDirection = 1;
                        Main.projectile[i].ai[1] = 0f;
                        Main.projectile[i].netUpdate = true;
                        Main.projectile[(int)Main.projectile[i].ai[0]].timeLeft = GammaDeathray.TotalFadeoutTime;
                    }
                }
            }
            // Bullet/deathray
            if (Projectile.ai[1] > 0)
            {
                Projectile.direction = Projectile.spriteDirection = 1;
                Projectile.rotation = Projectile.AngleTo(Main.MouseWorld);
                Projectile.ai[1]--;
            }
            else if (potentialTarget != null)
            {
                if (Projectile.Distance(player.Center) < DistanceToCheck &&
                    Collision.CanHit(Projectile.Center, 1, 1, player.Center, 1, 1) &&
                    Main.myPlayer == Projectile.owner)
                {
                    BulletShootCounter++;
                    if (BulletShootCounter % 20f == 14f)
                    {
                        Projectile bullet = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), Projectile.Center,
                            Projectile.DirectionTo(potentialTarget.Center) * 2.5f,
                            ModContent.ProjectileType<GammaBullet>(), Projectile.damage, 2f, Projectile.owner);
                        bullet.ai[0] = potentialTarget.whoAmI;
                    }

                    if (BulletShootCounter % 20f >= 17f)
                        Projectile.frame = Main.projFrames[Projectile.type] - 1;
                    else if (BulletShootCounter % 20f >= 11f)
                        Projectile.frame = Main.projFrames[Projectile.type] - 2;
                    else if (BulletShootCounter % 20f >= 6f)
                        Projectile.frame = Main.projFrames[Projectile.type] - 3;

                    Projectile.direction = Projectile.spriteDirection = (potentialTarget.Center.X - Projectile.Center.X > 0).ToDirectionInt();
                    returnPosition.X -= 48f * (player.Center.X - Projectile.Center.X > 0).ToDirectionInt();
                }
                else
                {
                    Projectile.direction = Projectile.spriteDirection = (player.Center.X - Projectile.Center.X > 0).ToDirectionInt();
                    Projectile.frame = 0;
                }
                Projectile.rotation = Projectile.rotation.AngleTowards(0f, 0.05f);
            }
            else
            {
                Projectile.direction = Projectile.spriteDirection = (player.Center.X - Projectile.Center.X > 0).ToDirectionInt();
                Projectile.rotation = Projectile.rotation.AngleTowards(0f, 0.05f);
                Projectile.frame = 0;
            }

            if (Projectile.Distance(returnPosition) > 28f)
            {
                Projectile.velocity += new Vector2(Math.Sign(returnPosition.X - Projectile.Center.X), Math.Sign(returnPosition.Y - Projectile.Center.Y)) * new Vector2(0.015f, 0.025f) * Main.rand.NextFloat(0.98f, 1.02f);
                if (Math.Abs(Projectile.velocity.X) > 2.5f)
                {
                    Projectile.velocity.X = 2.1f * Math.Sign(Projectile.velocity.X);
                }
                if (Math.Abs(Projectile.velocity.Y) > 1.6f)
                {
                    Projectile.velocity.Y = 1.4f * Math.Sign(Projectile.velocity.Y);
                }
                Projectile.velocity += (returnPosition - Projectile.Center) / 10f;
            }
            else Projectile.velocity *= 0.98f;
            Projectile.Center = new Vector2(Projectile.Center.X, MathHelper.Clamp(Projectile.Center.Y, 1f, returnPosition.Y - 8f));
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, (int)CalamityDusts.SulfurousSeaAcid, Vector2.One.RotatedByRandom(MathHelper.TwoPi) * 4f).noGravity = true;
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            Texture2D chain = ModContent.Request<Texture2D>("CalRD/Projectiles/Summon/GammaHeadChain").Value;
            Vector2 start = Projectile.Center + (Projectile.spriteDirection == -1).ToInt() * 10f * Vector2.UnitX;
            Vector2 end = DrawStartPosition - Projectile.DirectionTo(DrawStartPosition) * 11f;
            Vector2 bodyTop = DrawStartPosition + new Vector2(player.direction == 1 ? 6f : -6f, 0f);

            for (int i = 0; i <= (int)Projectile.Distance(end) / 10 + 1; i++)
            {
                float ratio = i / (Projectile.Distance(end) / 10 + 1);
                Vector2 positionAtPoint = start + (bodyTop - start) * ratio;
                if (Projectile.Distance(positionAtPoint) > 9)
                {
                    float angleAtPoint = (start - positionAtPoint).ToRotation();
                    if (i < (int)Projectile.Distance(end) / 10 + 1)
                    {
                        float nextRatio = (i + 1f) / (Projectile.Distance(end) / 10 + 1);
                        Vector2 nextPositionAtPoint = start + (bodyTop - start) * nextRatio;

                        angleAtPoint = (nextPositionAtPoint - positionAtPoint).ToRotation();
                    }
                    angleAtPoint += MathHelper.PiOver2;
                    Main.spriteBatch.Draw(chain,
                        positionAtPoint - Main.screenPosition,
                        null,
                        Color.Lerp(Color.White, Color.Transparent, 0.5f),
                        angleAtPoint,
                        chain.Size() / 2f,
                        1f,
                        SpriteEffects.None,
                        0f);
                }
            }
        }
        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */ => false;
    }
}
