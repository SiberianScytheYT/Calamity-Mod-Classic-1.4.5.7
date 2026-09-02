using CalRD.Dusts;
using CalRD.Items.Weapons.Summon;
using CalRD.Projectiles.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using CalRD.BiomeManagers;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Buffs.StatDebuffs;
using Terraria.GameContent.Bestiary;

namespace CalRD.NPCs.AcidRain
{
    public class NuclearTerror : ModNPC
    {
        public int AttackIndex = 0;
        public int DelayTime = 0;
        public bool Dying = false;
        public bool Walking = false;
        public float JumpTimer = 0f;
        public Vector2 ShootPosition;
        public static readonly int[] PhaseArray = new int[]
        {
            2, 0, 1, 1, 2, 1, 0, 2, 1, 1, 0, 1, 2, 1, 0, 2, 1
        };
        public const int AttackCycleTime = 520;
        public const int SpecialAttackTime = 240;
        public const float TeleportTime = 60f;
        public const float TeleportFadeinTime = 10f;
        public const float TeleportCooldown = 60f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nuclear Terror");
            Main.npcFrameCount[NPC.type] = 14;
            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.4f,
                Direction = 1
            };
            value.Position.X += 10f;
            value.Position.Y += 50f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("A terrifyingly powerful creature that only makes itself known once the worst has come.")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 176;
            NPC.height = 138;
            NPC.aiStyle = AIType = -1;

            NPC.lifeMax = 360420;
            NPC.defense = 50;

            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 20, 0, 0);
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
			NPC.DR_NERD(0.4f);
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit56;
            NPC.DeathSound = SoundID.NPCDeath60;
            SpawnModBiomes = new int[] { ModContent.GetInstance<Sulphur>().Type, ModContent.GetInstance<AcidRainBiome>().Type };
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.dontTakeDamage);
            writer.Write(Dying);
            writer.Write(Walking);
            writer.Write(AttackIndex);
            writer.Write(DelayTime);
            writer.Write(JumpTimer);
            writer.WriteVector2(ShootPosition);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.dontTakeDamage = reader.ReadBoolean();
            Dying = reader.ReadBoolean();
            Walking = reader.ReadBoolean();
            AttackIndex = reader.ReadInt32();
            DelayTime = reader.ReadInt32();
            JumpTimer = reader.ReadSingle();
            ShootPosition = reader.ReadVector2();
        }
        public override void AI()
        {
            Lighting.AddLight(NPC.Center, (Dying ? Color.Lime.ToVector3() : Color.White.ToVector3()) * 2f);
            if (Dying)
                return;
            bool phase2 = NPC.life / (float)NPC.lifeMax < 0.5f;
            if (DelayTime > 0)
            {
                DelayTime--;
                NPC.velocity.X *= 0.9f;
                if (NPC.velocity.Y < 18f)
                {
                    NPC.velocity.Y += 0.35f;
                }
                return;
            }
            if (NPC.target < 0 || NPC.target >= 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(false);
                NPC.netUpdate = true;
            }
            if (NPC.ai[1] > -TeleportCooldown)
            {
                TeleportEffects();
            }
            Player player = Main.player[NPC.target];
            NPC.defDamage = 170;
            NPC.damage = Dying ? 0 : NPC.defDamage;
            TeleportCheck(player);
            NPC.ai[0]++;
            Walking = false;
            // For teleporting if constantly spam-colliding
            if (NPC.collideX)
            {
                if (NPC.localAI[0] > 0)
                {
                    NPC.localAI[1]++;
                }
                NPC.localAI[0] = 20f;
            }
            if (NPC.localAI[0] > 0)
            {
                NPC.localAI[0]--;
            }
            if (NPC.ai[0] % AttackCycleTime < 240f)
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.velocity.X *= 0.8f;
                    if (JumpTimer++ >= 70f || !Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                    {
                        NPC.velocity.Y -= MathHelper.Clamp(Math.Abs(player.Center.Y - NPC.Center.Y) / 12.5f, 8f, 18f);
                        NPC.velocity.X = NPC.DirectionTo(player.Center).X * 18f;
                        JumpTimer = 0f;
                        NPC.netUpdate = true;
                    }
                    else
                    {
                        bool wasWalking = Walking;
                        if (wasWalking != Math.Abs(NPC.velocity.X) > 4f)
                        {
                            Walking = Math.Abs(NPC.velocity.X) > 4f;
                            NPC.netUpdate = true;
                        }
                        if (NPC.collideX)
                        {
                            JumpTimer = 50; // Force a jump the next frame to overcome the obstacle
                            NPC.netUpdate = true;
                        }
                        else if (Math.Abs(player.Center.X - NPC.Center.X) > 125f)
                        {
                            NPC.velocity.X += Math.Sign(NPC.DirectionTo(player.Center).X) * 3f;
                            NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -28f, 28f);
                        }
                        else
                        {
                            NPC.velocity.X *= 0.99f;
                        }
                    }
                }
                NPC.spriteDirection = (NPC.velocity.X < 0).ToDirectionInt();
            }
            else
            {
                if (NPC.ai[0] % AttackCycleTime == 255f)
                {
                    ShootPosition = player.Center;
                    NPC.netUpdate = true;
                    NPC.spriteDirection = (ShootPosition.X - NPC.Center.X < 0).ToDirectionInt();
                }
                switch (PhaseArray[AttackIndex])
                {
                    // Tightly packed, diverging bullets
                    case 0:
                        NPC.velocity.X *= 0.9f;
                        if (((NPC.ai[0] % AttackCycleTime >= AttackCycleTime - SpecialAttackTime + 20f && NPC.ai[0] % AttackCycleTime <= AttackCycleTime - SpecialAttackTime + 32f) ||
                            (NPC.ai[0] % AttackCycleTime >= AttackCycleTime - SpecialAttackTime + 50f && NPC.ai[0] % AttackCycleTime <= AttackCycleTime - SpecialAttackTime + 62f) ||
                            (NPC.ai[0] % AttackCycleTime >= AttackCycleTime - SpecialAttackTime + 80f && NPC.ai[0] % AttackCycleTime <= AttackCycleTime - SpecialAttackTime + 92f) ||
                            (NPC.ai[0] % AttackCycleTime >= AttackCycleTime - SpecialAttackTime + 110f && NPC.ai[0] % AttackCycleTime <= AttackCycleTime - SpecialAttackTime + 122f) ||
                            (NPC.ai[0] % AttackCycleTime >= AttackCycleTime - SpecialAttackTime + 140f && NPC.ai[0] % AttackCycleTime <= AttackCycleTime - SpecialAttackTime + 152f)) && NPC.ai[0] % 3f == 0f)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                float angle = (NPC.ai[0] % AttackCycleTime - (AttackCycleTime - SpecialAttackTime + 20f)) % 12f / 12f * MathHelper.ToRadians(15f) - MathHelper.ToRadians(7.5f);
                                int idx = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(ShootPosition).RotatedBy(angle) * 14f, ModContent.ProjectileType<NuclearBulletLarge>(), 48, 4f);
                                Main.projectile[idx].localAI[0] = angle;
                            }
                            SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
                        }
                        if (NPC.ai[0] % AttackCycleTime >= (AttackCycleTime - SpecialAttackTime + 35f) && NPC.ai[0] % 10f == 9f)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(player.Center) * 12f, ModContent.ProjectileType<NuclearBulletLarge>(), 48, 3f);
                        }
                        break;
                    // Cone of bullets
                    case 1:
                        NPC.velocity.X *= 0.9f;
                        if (NPC.ai[0] % AttackCycleTime >= AttackCycleTime - SpecialAttackTime + 35f && NPC.ai[0] % 4f == 3f)
                        {
                            float angle = MathHelper.Lerp(MathHelper.ToRadians(35f), MathHelper.ToRadians(5f), (NPC.ai[0] % AttackCycleTime - (AttackCycleTime - SpecialAttackTime + 35f)) / (SpecialAttackTime + 35f));
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(player.Center).RotatedBy(angle) * 16f, ModContent.ProjectileType<NuclearBulletLarge>(), 48, 4.5f);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(player.Center).RotatedBy(-angle) * 16f, ModContent.ProjectileType<NuclearBulletLarge>(), 48, 4.5f);
                        }
                        break;
                    // Shotgun bursts of bullets
                    case 2:
                        NPC.velocity.X *= 0.9f;
                        if (NPC.ai[0] % AttackCycleTime >= AttackCycleTime - SpecialAttackTime + 35f && NPC.ai[0] % 20f == 19f)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                float angle = MathHelper.Lerp(-0.5f, 0.5f, i / 3f);
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(ShootPosition).RotatedBy(angle) * 13f, ModContent.ProjectileType<NuclearBulletMedium>(), 48, 4f);
                            }
                        }
                        break;
                }
                if (NPC.ai[0] % AttackCycleTime == AttackCycleTime - 1f)
                {
                    DelayTime = phase2 ? 45 : 75;
                    AttackIndex++;
                    AttackIndex %= PhaseArray.Length;
                }
            }
        }
        public void TeleportCheck(Player player)
        {
            if (NPC.ai[1] <= -TeleportCooldown)
            {
                if (NPC.Distance(player.Center) > 2700f || 
                    (!Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) && NPC.Distance(player.Center) > 900f) ||
                    StuckOnPlatform(player) ||
                    NPC.wet ||
                    NPC.localAI[1] > 5f)
                {
                    Point playerPositionTileCoords = player.position.ToTileCoordinates();
                    Point npcPositionTileCoords = NPC.position.ToTileCoordinates();
                    int tries = 0;
                    int maxTeleportDistance = 20;
                    bool cannotTeleport = false;
                    while (!cannotTeleport && tries < 250)
                    {
                        tries++;
                        int x = Main.rand.Next(playerPositionTileCoords.X - maxTeleportDistance, playerPositionTileCoords.X + maxTeleportDistance);
                        int yStart = Main.rand.Next(playerPositionTileCoords.Y - maxTeleportDistance, playerPositionTileCoords.Y + maxTeleportDistance);
                        if (StuckOnPlatform(player))
                        {
                            yStart = Main.rand.Next(playerPositionTileCoords.Y, playerPositionTileCoords.Y + 4 * maxTeleportDistance);
                        }
                        for (int y = yStart; y < playerPositionTileCoords.Y + maxTeleportDistance; y++)
                        {
                            if ((y < playerPositionTileCoords.Y - 12 || y > playerPositionTileCoords.Y + 12 || x < playerPositionTileCoords.X - 12 || x > playerPositionTileCoords.X + 12)
                                && (y < npcPositionTileCoords.Y - 8 || y > npcPositionTileCoords.Y + 8 || x < npcPositionTileCoords.X - 7 || x > npcPositionTileCoords.X + 7)
                                && CalamityUtils.ParanoidTileRetrieval(x, y).HasUnactuatedTile)
                            {
                                bool canTeleport = true;
                                if ((CalamityUtils.ParanoidTileRetrieval(x, y - 1).LiquidType == LiquidID.Lava))
                                {
                                    canTeleport = false;
                                }
                                if (canTeleport &&
                                    Main.tileSolid[CalamityUtils.ParanoidTileRetrieval(x, y).TileType] && 
                                    !Collision.SolidTiles(x - 12, x + 12, y - 7, y - 7))
                                {
                                    for (int dy = y - 5; dy <= y; dy++)
                                    {
                                        if (CalamityUtils.ParanoidTileRetrieval(x, dy).LiquidAmount > 0)
                                        {
                                            continue;
                                        }
                                    }
                                    NPC.ai[1] = TeleportTime;
                                    NPC.ai[2] = x;
                                    NPC.ai[3] = y - 3;
                                    cannotTeleport = true;
                                    NPC.localAI[1] = 0f;
                                    break;
                                }
                            }
                        }
                    }
                    NPC.netUpdate = true;
                }
            }
        }
        public void TeleportEffects()
        {
            if (NPC.ai[1] > TeleportTime)
                NPC.ai[1] = TeleportTime;
            NPC.ai[1]--;
            if (NPC.ai[1] >= 0f)
            {
                if (NPC.ai[1] == 0f && NPC.ai[2] != 0f && NPC.ai[3] != 0f)
                {
                    NPC.position.X = NPC.ai[2] * 16f - NPC.width / 2 + 8f;
                    NPC.position.Y = NPC.ai[3] * 16f - NPC.height;
                    NPC.netUpdate = true;
                    NPC.velocity = Vector2.Zero;
                }
                else
                {
                    NPC.alpha = (int)MathHelper.Lerp(0f, 255f, 1f - NPC.ai[1] / TeleportTime);
                    int totalDust = (int)(30 * NPC.alpha / 255f);
                    for (int i = 0; i < totalDust; i++)
                    {
                        Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid);
                        dust.noGravity = true;
                        dust.velocity = NPC.DirectionFrom(dust.position) * 2f;
                        dust.scale = 1.6f;
                    }
                    NPC.velocity.X *= 0.95f;
                    if (NPC.velocity.Y < 18f)
                    {
                        NPC.velocity.Y += 0.35f;
                    }
                }
                return;
            }
            else if (NPC.ai[1] >= -TeleportFadeinTime)
            {
                NPC.alpha = (int)MathHelper.Lerp(255f, 0f, NPC.ai[1] / -TeleportFadeinTime);
                if (NPC.ai[1] == -TeleportFadeinTime)
                {
                    for (int i = 0; i < 48; i++)
                    {
                        Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid);
                        dust.noGravity = true;
                        dust.velocity = NPC.DirectionFrom(dust.position) * Main.rand.NextFloat(2f, 3.6f);
                        dust.scale = 1.8f;
                    }
                }
            }
        }
        public bool StuckOnPlatform(Player player)
        {
            for (int i = 0; i < 18; i++)
            {
                Point bottom = (NPC.Bottom + Vector2.UnitY * i).ToTileCoordinates();
                if (TileID.Sets.Platforms[CalamityUtils.ParanoidTileRetrieval(bottom.X, bottom.Y).TileType] && player.Top.Y > NPC.Bottom.Y + 48)
                    return true;
            }
            return false;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * 0.85f);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            int framesNeeded = Dying ? 7 : 6;
            if (Walking)
            {
                framesNeeded = 8 - (int)Math.Ceiling(Math.Abs(NPC.velocity.X) / 5f); // Walk faster the faster we're moving
            }
            if (NPC.frameCounter >= framesNeeded)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
            if (Dying)
            {
                if (NPC.frame.Y < frameHeight * 8)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            int type = Main.rand.NextBool(4) ? ModContent.ProjectileType<SulphuricAcidMist>() : ModContent.ProjectileType<NuclearBulletLarge>();
                            float angle = MathHelper.TwoPi / 16f * i;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, angle.ToRotationVector2() * Main.rand.NextFloat(4f, 11f), type, 48, 3f);
                        }
                    }
                    for (int i = 0; i < 60; i++)
                    {
                        Dust dust = Dust.NewDustDirect(NPC.Center, 45, 45, (int)CalamityDusts.SulfurousSeaAcid);
                        dust.velocity = Utils.NextVector2Unit(Main.rand) * Main.rand.NextFloat(4f, 15f);
                        dust.noGravity = true;
                        dust.scale = Main.rand.NextFloat(2f, 3f);
                    }
                    NPC.frame.Y = frameHeight * 8;
                }
                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                {
                    NPC.StrikeInstantKill();
                }
            }
            else if (NPC.frame.Y >= (Walking ? 8 : 4) * frameHeight)
            {
                NPC.frame.Y = Walking ? 4 * frameHeight : 0;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            CalamityGlobalNPC.DrawGlowmask(NPC, spriteBatch, null, true);
            if (NPC.velocity.Length() > 0f)
            {
                Color endColor = Color.DarkOliveGreen;
                endColor.A = Color.Transparent.A;
                CalamityGlobalNPC.DrawAfterimage(NPC, spriteBatch, drawColor, endColor, directioning: true, invertedDirection: true);
            }
            return false;
        }
        public override bool CheckDead()
        {
            if (!Dying)
            {
                NPC.active = true;
                NPC.life = 1;
                NPC.dontTakeDamage = true;
                Dying = true;
                NPC.velocity = Vector2.Zero;
                NPC.netUpdate = true;
                return false;
            }
            return Dying;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 300);
        }
        public override void OnKill()
        {
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<GammaHeart>(), 3);
        }
    }
}
