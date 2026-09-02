using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Events;
using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Ravager
{
    [AutoloadBossHead]
    public class RavagerBody : ModNPC
    {
		private float velocityY = -16f;

		public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ravager");
            Main.npcFrameCount[NPC.type] = 7;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
	            Scale = 0.5f,
	            PortraitPositionYOverride = -40f,
	            PortraitScale = 0.6f,
            };
            value.Position.Y -= 50f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
		
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
	        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
	        {
		        BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
		        new FlavorTextBestiaryInfoElement("The innumerable corpses of the undead, forced to rise once more in a bid for victory... A great mistake that cost its creators their lives.")
	        });
        }

        public override void SetDefaults()
        {
            NPC.lavaImmune = true;
			NPC.noGravity = true;
            NPC.npcSlots = 20f;
            NPC.aiStyle = -1;
			NPC.GetNPCDamage();
			NPC.width = 332;
            NPC.height = 214;
            NPC.defense = 55;
            NPC.value = Item.buyPrice(0, 25, 0, 0);
			NPC.DR_NERD(0.4f);
            NPC.LifeMaxNERB(42700, 53500, 4600000);
            if (CalamityWorld.downedProvidence && !BossRushEvent.BossRushActive)
            {
                NPC.damage *= 2;
                NPC.defense *= 2;
                NPC.lifeMax *= 7;
                NPC.value *= 1.5f;
            }
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            AIType = -1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;
			NPC.buffImmune[BuffID.Frostburn] = false;
			NPC.buffImmune[BuffID.Daybreak] = false;
			NPC.buffImmune[BuffID.BetsysCurse] = false;
			NPC.buffImmune[BuffID.StardustMinionBleed] = false;
			NPC.buffImmune[BuffID.DryadsWardDebuff] = false;
			NPC.buffImmune[BuffID.Oiled] = false;
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<HolyFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WarCleave>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.boss = true;
            NPC.alpha = 255;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath14;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/Ravager");
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.dontTakeDamage);
			writer.Write(velocityY);
		}

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.dontTakeDamage = reader.ReadBoolean();
			velocityY = reader.ReadSingle();
		}

        public override void FindFrame(int frameHeight)
        {
	        if (NPC.IsABestiaryIconDummy)
		        NPC.Opacity = 1f;
	        
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            bool provy = CalamityWorld.downedProvidence && !BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;

			// Percent life remaining
			float lifeRatio = NPC.life / (float)NPC.lifeMax;

            // Large fire light
            Lighting.AddLight((int)(NPC.Center.X - 110f) / 16, (int)(NPC.Center.Y - 30f) / 16, 0f, 0.5f, 2f);
            Lighting.AddLight((int)(NPC.Center.X + 110f) / 16, (int)(NPC.Center.Y - 30f) / 16, 0f, 0.5f, 2f);

            // Small fire light
            Lighting.AddLight((int)(NPC.Center.X - 40f) / 16, (int)(NPC.Center.Y - 60f) / 16, 0f, 0.25f, 1f);
            Lighting.AddLight((int)(NPC.Center.X + 40f) / 16, (int)(NPC.Center.Y - 60f) / 16, 0f, 0.25f, 1f);

            CalamityGlobalNPC.scavenger = NPC.whoAmI;

            if (NPC.localAI[0] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.localAI[0] = 1f;
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X - 70, (int)NPC.Center.Y + 88, ModContent.NPCType<RavagerLegLeft>());
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X + 70, (int)NPC.Center.Y + 88, ModContent.NPCType<RavagerLegRight>());
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X - 120, (int)NPC.Center.Y + 50, ModContent.NPCType<RavagerClawLeft>());
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X + 120, (int)NPC.Center.Y + 50, ModContent.NPCType<RavagerClawRight>());
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X + 1, (int)NPC.Center.Y - 20, ModContent.NPCType<RavagerHead>());
            }

            if (NPC.target >= 0 && Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(true);
                if (Main.player[NPC.target].dead)
                    NPC.noTileCollide = true;
            }

			Player player = Main.player[NPC.target];

            if (NPC.alpha > 0)
            {
                NPC.alpha -= 10;
                if (NPC.alpha < 0)
                    NPC.alpha = 0;

                NPC.ai[1] = 0f;
            }

            bool leftLegActive = false;
            bool rightLegActive = false;
            bool headActive = false;
            bool rightClawActive = false;
            bool leftClawActive = false;

            for (int num619 = 0; num619 < Main.maxNPCs; num619++)
            {
                if (Main.npc[num619].active && Main.npc[num619].type == ModContent.NPCType<RavagerHead>())
                    headActive = true;
                if (Main.npc[num619].active && Main.npc[num619].type == ModContent.NPCType<RavagerClawRight>())
                    rightClawActive = true;
                if (Main.npc[num619].active && Main.npc[num619].type == ModContent.NPCType<RavagerClawLeft>())
                    leftClawActive = true;
                if (Main.npc[num619].active && Main.npc[num619].type == ModContent.NPCType<RavagerLegRight>())
                    rightLegActive = true;
                if (Main.npc[num619].active && Main.npc[num619].type == ModContent.NPCType<RavagerLegLeft>())
                    leftLegActive = true;
            }

			bool immunePhase = headActive || rightClawActive || leftClawActive || rightLegActive || leftLegActive;
			bool finalPhase = !leftClawActive && !rightClawActive && !headActive && !leftLegActive && !rightLegActive && expertMode;

			bool enrage = false;
            if (player.position.Y + (player.height / 2) > NPC.position.Y + (NPC.height / 2) + 10f)
                enrage = true;

            if (immunePhase)
                NPC.dontTakeDamage = true;
            else
            {
                NPC.dontTakeDamage = false;
                if (Main.netMode != NetmodeID.Server)
                {
                    if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active && revenge)
                        Main.player[Main.myPlayer].AddBuff(ModContent.BuffType<WeakPetrification>(), 2);
                }
            }

            if (!headActive)
            {
                int rightDust = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - 30f), 8, 8, DustID.Blood, 0f, 0f, 100, default, 2.5f);
                Main.dust[rightDust].alpha += Main.rand.Next(100);
                Main.dust[rightDust].velocity *= 0.2f;

                Dust rightDustExpr = Main.dust[rightDust];
                rightDustExpr.velocity.Y -= 3f + Main.rand.Next(10) * 0.1f;
                Main.dust[rightDust].fadeIn = 0.5f + Main.rand.Next(10) * 0.1f;

                if (Main.rand.NextBool(10))
                {
                    rightDust = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - 30f), 8, 8, DustID.Torch, 0f, 0f, 0, default, 1.5f);
                    if (Main.rand.Next(20) != 0)
                    {
                        Main.dust[rightDust].noGravity = true;
                        Main.dust[rightDust].scale *= 1f + Main.rand.Next(10) * 0.1f;
                        Dust rightDustExpr2 = Main.dust[rightDust];
                        rightDustExpr2.velocity.Y -= 4f;
                    }
                }

				if (!finalPhase)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.localAI[1] += enrage ? 6f : 1f;
						if (NPC.localAI[1] >= 600f)
						{
							NPC.localAI[1] = 0f;
							if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
							{
								float velocity = BossRushEvent.BossRushActive ? 10f : 7f;
								int totalProjectiles = 8;
								float radians = MathHelper.TwoPi / totalProjectiles;
								int type = ProjectileID.EyeBeam;
								int damage = NPC.GetProjectileDamage(type);
								for (int i = 0; i < totalProjectiles; i++)
								{
									Vector2 vector255 = new Vector2(0f, -velocity).RotatedBy(radians * i);
									Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vector255, type, damage + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
								}
							}
						}
					}
				}
            }

            if (!rightClawActive)
            {
                int rightDust = Dust.NewDust(new Vector2(NPC.Center.X + 80f, NPC.Center.Y + 45f), 8, 8, DustID.Blood, 0f, 0f, 100, default, 3f);
                Main.dust[rightDust].alpha += Main.rand.Next(100);
                Main.dust[rightDust].velocity *= 0.2f;

                Dust rightDustExpr = Main.dust[rightDust];
                rightDustExpr.velocity.X += 3f + Main.rand.Next(10) * 0.1f;
                Main.dust[rightDust].fadeIn = 0.5f + Main.rand.Next(10) * 0.1f;

                if (Main.rand.NextBool(10))
                {
                    rightDust = Dust.NewDust(new Vector2(NPC.Center.X + 80f, NPC.Center.Y + 45f), 8, 8, DustID.Torch, 0f, 0f, 0, default, 2f);
                    if (Main.rand.Next(20) != 0)
                    {
                        Main.dust[rightDust].noGravity = true;
                        Main.dust[rightDust].scale *= 1f + Main.rand.Next(10) * 0.1f;
                        Dust rightDustExpr2 = Main.dust[rightDust];
                        rightDustExpr2.velocity.X += 4f;
                    }
                }

				if (!finalPhase)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.localAI[2] += enrage ? 2f : 1f;
						if (NPC.localAI[2] >= 480f)
						{
							SoundEngine.PlaySound(SoundID.Item20, NPC.position);
							NPC.localAI[2] = 0f;
							Vector2 shootFromVector = new Vector2(NPC.Center.X + 80f, NPC.Center.Y + 45f);
							int type = ProjectileID.Fireball;
							int damage = NPC.GetProjectileDamage(type);
							float velocity = BossRushEvent.BossRushActive ? 18f : 12f;
							Projectile.NewProjectile(Entity.GetSource_FromThis(), shootFromVector.X, shootFromVector.Y, velocity, 0f, type, damage + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
						}
					}
				}
            }

            if (!leftClawActive)
            {
                int leftDust = Dust.NewDust(new Vector2(NPC.Center.X - 80f, NPC.Center.Y + 45f), 8, 8, DustID.Blood, 0f, 0f, 100, default, 3f);
                Dust leftDustExpr = Main.dust[leftDust];
                leftDustExpr.alpha += Main.rand.Next(100);
                leftDustExpr.velocity *= 0.2f;
                leftDustExpr.velocity.X -= 3f + Main.rand.Next(10) * 0.1f;
                leftDustExpr.fadeIn = 0.5f + Main.rand.Next(10) * 0.1f;

                if (Main.rand.NextBool(10))
                {
                    leftDust = Dust.NewDust(new Vector2(NPC.Center.X - 80f, NPC.Center.Y + 45f), 8, 8, DustID.Torch, 0f, 0f, 0, default, 2f);
                    if (Main.rand.Next(20) != 0)
                    {
                        Dust leftDustExpr2 = Main.dust[leftDust];
                        leftDustExpr2.noGravity = true;
                        leftDustExpr2.scale *= 1f + Main.rand.Next(10) * 0.1f;
                        leftDustExpr2.velocity.X -= 4f;
                    }
                }

				if (!finalPhase)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.localAI[3] += enrage ? 2f : 1f;
						if (NPC.localAI[3] >= 480f)
						{
							SoundEngine.PlaySound(SoundID.Item20, NPC.position);
							NPC.localAI[3] = 0f;
							Vector2 shootFromVector = new Vector2(NPC.Center.X - 80f, NPC.Center.Y + 45f);
							int type = ProjectileID.Fireball;
							int damage = NPC.GetProjectileDamage(type);
							float velocity = BossRushEvent.BossRushActive ? -18f : -12f;
							Projectile.NewProjectile(Entity.GetSource_FromThis(), shootFromVector.X, shootFromVector.Y, velocity, 0f, type, damage + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
						}
					}
				}
            }

            if (!rightLegActive)
            {
                int rightDust = Dust.NewDust(new Vector2(NPC.Center.X + 60f, NPC.Center.Y + 60f), 8, 8, DustID.Blood, 0f, 0f, 100, default, 2f);
                Dust rightDustExpr = Main.dust[rightDust];
                rightDustExpr.alpha += Main.rand.Next(100);
                rightDustExpr.velocity *= 0.2f;
                rightDustExpr.velocity.Y += 0.5f + Main.rand.Next(10) * 0.1f;
                rightDustExpr.fadeIn = 0.5f + Main.rand.Next(10) * 0.1f;

                if (Main.rand.NextBool(10))
                {
                    rightDust = Dust.NewDust(new Vector2(NPC.Center.X + 60f, NPC.Center.Y + 60f), 8, 8, DustID.Torch, 0f, 0f, 0, default, 1.5f);
                    if (Main.rand.Next(20) != 0)
                    {
                        Dust rightDustExpr2 = Main.dust[rightDust];
                        rightDustExpr2.noGravity = true;
                        rightDustExpr2.scale *= 1f + Main.rand.Next(10) * 0.1f;
                        rightDustExpr2.velocity.Y += 1f;
                    }
                }

				if (!finalPhase)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.ai[2] += 1f;
						if (NPC.ai[2] >= 300f)
						{
							NPC.ai[2] = 0f;
							Vector2 shootFromVector = new Vector2(NPC.Center.X + 60f, NPC.Center.Y + 60f);
							int type = ProjectileID.GreekFire1;
							int damage = NPC.GetProjectileDamage(type);
							int fire = Projectile.NewProjectile(Entity.GetSource_FromThis(), shootFromVector.X, shootFromVector.Y, 0f, 2f, type + Main.rand.Next(3), damage + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
							Main.projectile[fire].timeLeft = 180;
						}
					}
				}
            }

            if (!leftLegActive)
            {
                int leftDust = Dust.NewDust(new Vector2(NPC.Center.X - 60f, NPC.Center.Y + 60f), 8, 8, DustID.Blood, 0f, 0f, 100, default, 2f);
                Main.dust[leftDust].alpha += Main.rand.Next(100);
                Main.dust[leftDust].velocity *= 0.2f;

                Dust leftDustExpr = Main.dust[leftDust];
                leftDustExpr.velocity.Y += 0.5f + Main.rand.Next(10) * 0.1f;
                Main.dust[leftDust].fadeIn = 0.5f + Main.rand.Next(10) * 0.1f;

                if (Main.rand.NextBool(10))
                {
                    leftDust = Dust.NewDust(new Vector2(NPC.Center.X - 60f, NPC.Center.Y + 60f), 8, 8, DustID.Torch, 0f, 0f, 0, default, 1.5f);
                    if (Main.rand.Next(20) != 0)
                    {
                        Main.dust[leftDust].noGravity = true;
                        Main.dust[leftDust].scale *= 1f + Main.rand.Next(10) * 0.1f;
                        Dust leftDustExpr2 = Main.dust[leftDust];
                        leftDustExpr2.velocity.Y += 1f;
                    }
                }

				if (!finalPhase)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.ai[3] += 1f;
						if (NPC.ai[3] >= 300f)
						{
							NPC.ai[3] = 0f;
							Vector2 shootFromVector = new Vector2(NPC.Center.X - 60f, NPC.Center.Y + 60f);
							int type = ProjectileID.GreekFire1;
							int damage = NPC.GetProjectileDamage(type);
							int fire = Projectile.NewProjectile(Entity.GetSource_FromThis(), shootFromVector.X, shootFromVector.Y, 0f, 2f, type + Main.rand.Next(3), damage + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
							Main.projectile[fire].timeLeft = 180;
						}
					}
				}
            }

            if (NPC.ai[0] == 0f)
            {
				NPC.noTileCollide = false;

                if (NPC.velocity.Y == 0f)
                {
                    NPC.velocity.X *= 0.8f;

                    NPC.ai[1] += 1f;
                    if (NPC.ai[1] > 0f)
                    {
						if (revenge)
						{
							if (NPC.Calamity().newAI[0] % 3f == 0f)
								NPC.ai[1] += 1f;
							else if (NPC.Calamity().newAI[0] % 2f == 0f)
								NPC.ai[1] += 1f;
						}

						if ((!rightClawActive && !leftClawActive) || NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
                            NPC.ai[1] += 1f;
                        if (!headActive || NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
                            NPC.ai[1] += 1f;
                        if ((!rightLegActive && !leftLegActive) || NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
                            NPC.ai[1] += 1f;
                    }

                    if (NPC.ai[1] >= 300f)
                        NPC.ai[1] = -20f;
                    else if (NPC.ai[1] == -1f)
                    {
                        NPC.TargetClosest(true);

						bool shouldFall = player.position.Y >= NPC.Bottom.Y;
						float velocityXBoost = death ? 6f * (1f - lifeRatio) : 4f * (1f - lifeRatio);
						float velocityX = ((enrage || NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive)) ? 8f : 4f) + velocityXBoost;
						velocityY = -16f;

						float distanceBelowTarget = NPC.position.Y - (player.position.Y + 80f);

						if (revenge)
						{
							if (distanceBelowTarget > 0f)
								NPC.Calamity().newAI[1] += 1f + distanceBelowTarget * 0.001f;

							if (NPC.Calamity().newAI[1] > 2f)
								NPC.Calamity().newAI[1] = 2f;

							if (NPC.Calamity().newAI[1] > 1f)
								velocityY *= NPC.Calamity().newAI[1];
						}

						if (expertMode && !finalPhase)
						{
							NPC.noTileCollide = true;
							if (shouldFall)
								velocityY = 1f;

							if (NPC.Calamity().newAI[0] % 3f == 0f)
							{
								velocityX *= 2f;
								if (!shouldFall)
									velocityY *= 0.5f;
							}
							else if (NPC.Calamity().newAI[0] % 2f == 0f)
							{
								velocityX *= 1.5f;
								if (!shouldFall)
									velocityY *= 0.75f;
							}
						}

						if (finalPhase)
						{
							NPC.noTileCollide = true;
							NPC.Calamity().newAI[2] = player.direction;
						}

						NPC.velocity.X = velocityX * NPC.direction;
                        NPC.velocity.Y = velocityY;

                        NPC.ai[0] = finalPhase ? 2f : 1f;
                        NPC.ai[1] = 0f;
					}
                }

				CustomGravity();
			}
            else if (NPC.ai[0] >= 1f)
            {
                if (NPC.velocity.Y == 0f && (NPC.ai[1] == 31f || NPC.ai[0] == 1f))
                {
					SoundEngine.PlaySound(SoundID.Item14.WithVolumeScale(1.25f).WithPitchOffset(-0.25f), NPC.position);

					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;

					if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
						if (expertMode)
						{
							for (int i = 0; i < Main.maxNPCs; i++)
							{
                                if (Main.npc[i].type == ModContent.NPCType<RockPillar>() && Main.npc[i].ai[0] == 0f)
                                {
                                    Main.npc[i].ai[1] = -1f;
                                    Main.npc[i].direction = NPC.direction;
                                    Main.npc[i].netUpdate = true;
                                }
							}

							int spawnDistance = 360;

							if (!NPC.AnyNPCs(ModContent.NPCType<RockPillar>()))
							{
								NPC.NewNPC(NPC.GetSource_FromThis(), (int)(player.Center.X - spawnDistance * 1.25f), (int)player.Center.Y - 100, ModContent.NPCType<RockPillar>());
								NPC.NewNPC(NPC.GetSource_FromThis(), (int)(player.Center.X + spawnDistance * 1.25f), (int)player.Center.Y - 100, ModContent.NPCType<RockPillar>());
							}
							else if (!NPC.AnyNPCs(ModContent.NPCType<FlamePillar>()))
							{
								float distanceMultiplier = finalPhase ? 2.5f : 2f;
								NPC.NewNPC(NPC.GetSource_FromThis(), (int)player.Center.X - (int)(spawnDistance * distanceMultiplier), (int)player.Center.Y - 100, ModContent.NPCType<FlamePillar>());
								NPC.NewNPC(NPC.GetSource_FromThis(), (int)player.Center.X + (int)(spawnDistance * distanceMultiplier), (int)player.Center.Y - 100, ModContent.NPCType<FlamePillar>());
							}
						}
                    }

					if (revenge)
						NPC.Calamity().newAI[0] += 1f;

					NPC.Calamity().newAI[1] = 0f;
					NPC.Calamity().newAI[3] = 0f;

					for (int stompDustArea = (int)NPC.position.X - 30; stompDustArea < (int)NPC.position.X + NPC.width + 60; stompDustArea += 30)
                    {
                        for (int stompDustAmount = 0; stompDustAmount < 6; stompDustAmount++)
                        {
                            int stompDust = Dust.NewDust(new Vector2(NPC.position.X - 30f, NPC.position.Y + NPC.height), NPC.width + 30, 4, 31, 0f, 0f, 100, default, 1.5f);
                            Main.dust[stompDust].velocity *= 0.2f;
                        }

                        int stompGore = Gore.NewGore(Entity.GetSource_FromThis(), new Vector2(stompDustArea - 30, NPC.position.Y + NPC.height - 12f), default, Main.rand.Next(61, 64), 1f);
                        Main.gore[stompGore].velocity *= 0.4f;
                    }
                }
                else
                {
					NPC.TargetClosest(true);

					// Fall through
					if (!player.dead && expertMode)
					{
						if ((player.position.Y > NPC.Bottom.Y && NPC.velocity.Y > 0f) || (player.position.Y < NPC.Bottom.Y && NPC.velocity.Y < 0f))
							NPC.noTileCollide = true;
						else if ((NPC.velocity.Y > 0f && NPC.Bottom.Y > Main.player[NPC.target].Top.Y) || (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].Center, 1, 1) && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height)))
							NPC.noTileCollide = false;
					}

					Vector2 targetVector = player.position;
					float aimY = targetVector.Y - 640f;
					float distanceFromTargetPos = Math.Abs(NPC.Top.Y - aimY);
					bool inRange = NPC.Top.Y <= aimY + 160f && NPC.Top.Y >= aimY - 16f;

					if (NPC.ai[0] == 2f && NPC.ai[1] == 0f)
					{
						NPC.Calamity().newAI[3] += 1f;

						if (inRange)
							NPC.velocity.Y = 0f;
						else if (NPC.Top.Y > aimY)
							NPC.velocity.Y -= 0.2f + distanceFromTargetPos * 0.001f;
						else
							NPC.velocity.Y += 0.2f + distanceFromTargetPos * 0.001f;

						if (NPC.velocity.Y < velocityY)
							NPC.velocity.Y = velocityY;
						if (NPC.velocity.Y > -velocityY)
							NPC.velocity.Y = -velocityY;
					}

					float maxOffset = death ? 320f * (1f - lifeRatio) : 240f * (1f - lifeRatio);
					float offset = NPC.ai[0] == 2f ? maxOffset * NPC.Calamity().newAI[2] : 0f;

					// Set offset to 0 if the target stops moving
					if (Math.Abs(player.velocity.X) < 0.5f)
						NPC.Calamity().newAI[2] = 0f;
					else
						NPC.Calamity().newAI[2] = player.direction;

					if ((NPC.position.X < targetVector.X + offset && NPC.position.X + NPC.width > targetVector.X + player.width + offset && (inRange || NPC.ai[0] != 2f)) || NPC.ai[1] > 0f || NPC.Calamity().newAI[3] >= 180f)
                    {
						NPC.damage = NPC.defDamage;

						if (NPC.ai[0] == 2f)
						{
							float stopBeforeFallTime = 30f;
							if (expertMode)
								stopBeforeFallTime -= death ? 15f * (1f - lifeRatio) : 10f * (1f - lifeRatio);

							if (NPC.ai[1] < stopBeforeFallTime)
							{
								NPC.ai[1] += 1f;
								NPC.velocity = Vector2.Zero;
							}
							else
							{
								float fallSpeedBoost = death ? 1.8f * (1f - lifeRatio) : 1.2f * (1f - lifeRatio);
								float fallSpeed = 1.2f + fallSpeedBoost;

								if (NPC.Calamity().newAI[1] > 1f)
									fallSpeed *= NPC.Calamity().newAI[1];

								NPC.velocity.Y += fallSpeed;

								NPC.ai[1] = 31f;
							}
						}
						else
						{
							NPC.velocity.X *= 0.9f;

							if (NPC.Bottom.Y < player.position.Y)
							{
								float fallSpeedBoost = death ? 0.9f * (1f - lifeRatio) : 0.6f * (1f - lifeRatio);
								float fallSpeed = 0.6f + fallSpeedBoost;

								if (NPC.Calamity().newAI[1] > 1f)
									fallSpeed *= NPC.Calamity().newAI[1];

								NPC.velocity.Y += fallSpeed;
							}
						}
                    }
                    else
                    {
						float velocityMult = 1.8f;
						float velocityXChange = 0.2f + Math.Abs(NPC.Center.X - player.Center.X) * 0.001f;

						float velocityXBoost = death ? 6f * (1f - lifeRatio) : 4f * (1f - lifeRatio);
						float velocityX = 8f + velocityXBoost + Math.Abs(NPC.Center.X - player.Center.X) * 0.001f;

						if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
							velocityX += 3f;
						if (!rightClawActive)
							velocityX += 1f;
						if (!leftClawActive)
							velocityX += 1f;
						if (!headActive)
							velocityX += 1f;
						if (!rightLegActive)
							velocityX += 1f;
						if (!leftLegActive)
							velocityX += 1f;

						if (NPC.ai[0] == 2f)
						{
							NPC.damage = 0;
							velocityXChange *= velocityMult;
							velocityX *= velocityMult;
						}

						if (NPC.direction < 0)
                            NPC.velocity.X -= velocityXChange;
                        else if (NPC.direction > 0)
                            NPC.velocity.X += velocityXChange;

                        if (NPC.velocity.X < -velocityX)
                            NPC.velocity.X = -velocityX;
                        if (NPC.velocity.X > velocityX)
                            NPC.velocity.X = velocityX;
                    }

					CustomGravity();
				}
            }

			void CustomGravity()
			{
				float gravity = NPC.ai[0] == 2f ? 0f : 0.3f;
				float maxFallSpeed = NPC.ai[0] == 2f ? 24f : 10f;
				if (NPC.wet)
				{
					if (NPC.honeyWet)
					{
						gravity *= 0.33f;
						maxFallSpeed *= 0.4f;
					}
					else
					{
						gravity *= 0.66f;
						maxFallSpeed *= 0.7f;
					}
				}

				if (NPC.Calamity().newAI[1] > 1f)
					maxFallSpeed *= NPC.Calamity().newAI[1];

				NPC.velocity.Y += gravity;
				if (NPC.velocity.Y > maxFallSpeed)
					NPC.velocity.Y = maxFallSpeed;
			}

			player = Main.player[NPC.target];
			if (NPC.target <= 0 || NPC.target == 255 || player.dead || !player.active)
			{
				NPC.TargetClosest(true);
				player = Main.player[NPC.target];
			}

            int distanceFromTarget = 5600;
            if (Vector2.Distance(NPC.Center, player.Center) > distanceFromTarget)
            {
                NPC.TargetClosest(true);
				player = Main.player[NPC.target];

				if (Vector2.Distance(NPC.Center, player.Center) > distanceFromTarget)
                {
                    NPC.active = false;
                    NPC.netUpdate = true;
                }
            }
        }
        
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
	        Vector2 center = new Vector2(NPC.Center.X, NPC.Center.Y);
	        if (NPC.IsABestiaryIconDummy)
	        {
		        spriteBatch.Draw(TextureAssets.Npc[ModContent.NPCType<RavagerClawLeft>()].Value, new Vector2(center.X - screenPos.X - NPC.scale * 180, center.Y - screenPos.Y + 50),
			        new Rectangle?(new Rectangle(0, 0, TextureAssets.Npc[ModContent.NPCType<RavagerClawLeft>()].Value.Width, TextureAssets.Npc[ModContent.NPCType<RavagerClawLeft>()].Value.Height)),
			        Color.White, 0f, default, NPC.scale, SpriteEffects.None, 0f);
		        spriteBatch.Draw(TextureAssets.Npc[ModContent.NPCType<RavagerClawRight>()].Value, new Vector2(center.X - screenPos.X + NPC.scale * 110, center.Y - screenPos.Y + 50),
			        new Rectangle?(new Rectangle(0, 0, TextureAssets.Npc[ModContent.NPCType<RavagerClawRight>()].Value.Width, TextureAssets.Npc[ModContent.NPCType<RavagerClawRight>()].Value.Height)),
			        Color.White, 0f, default, NPC.scale, SpriteEffects.None, 0f);
	        }
	        return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Vector2 center = new Vector2(NPC.Center.X, NPC.Center.Y);
            Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
            Vector2 vector = center - screenPos;
            vector -= new Vector2(ModContent.Request<Texture2D>("CalRD/NPCs/Ravager/RavagerBodyGlow").Value.Width, ModContent.Request<Texture2D>("CalRD/NPCs/Ravager/RavagerBodyGlow").Value.Height / Main.npcFrameCount[NPC.type]) * 1f / 2f;
            vector += vector11 * 1f + new Vector2(0f, 0f + 4f + NPC.gfxOffY);
            Color color = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(Color.Blue);
            spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/NPCs/Ravager/RavagerBodyGlow").Value, vector,
                NPC.frame, color, NPC.rotation, vector11, 1f, spriteEffects, 0f);
            
            float legOffset = 20f;
            float headOffset = 75f;
            Color color2 = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16f));
            if (NPC.IsABestiaryIconDummy)
            {
	            color2 = Color.White;
	            legOffset = 60f;
	            headOffset = 0f;
            }
            spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/NPCs/Ravager/RavagerLegRight").Value, new Vector2(center.X - screenPos.X + NPC.scale * 28f, center.Y - screenPos.Y + legOffset), //72
                new Rectangle?(new Rectangle(0, 0, ModContent.Request<Texture2D>("CalRD/NPCs/Ravager/RavagerLegRight").Value.Width, ModContent.Request<Texture2D>("CalRD/NPCs/Ravager/RavagerLegRight").Value.Height)),
                color2, 0f, default, NPC.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/NPCs/Ravager/RavagerLegLeft").Value, new Vector2(center.X - screenPos.X - NPC.scale * 112f, center.Y - screenPos.Y + legOffset), //72
                new Rectangle?(new Rectangle(0, 0, ModContent.Request<Texture2D>("CalRD/NPCs/Ravager/RavagerLegLeft").Value.Width, ModContent.Request<Texture2D>("CalRD/NPCs/Ravager/RavagerLegLeft").Value.Height)),
                color2, 0f, default, NPC.scale, SpriteEffects.None, 0f);
            if (NPC.AnyNPCs(ModContent.NPCType<RavagerHead>()) || NPC.IsABestiaryIconDummy)
            {
                spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/NPCs/Ravager/RavagerHead").Value, new Vector2(center.X - screenPos.X - NPC.scale * 70f, center.Y - screenPos.Y - headOffset),
                    new Rectangle?(new Rectangle(0, 0, ModContent.Request<Texture2D>("CalRD/NPCs/Ravager/RavagerHead").Value.Width, ModContent.Request<Texture2D>("CalRD/NPCs/Ravager/RavagerHead").Value.Height)),
                    color2, 0f, default, NPC.scale, SpriteEffects.None, 0f);
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 2f);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
	            if (Main.netMode != NetmodeID.Server)
	            {
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerBody").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerBody2").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerBody3").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerBody4").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerBody5").Type, 1f);
                }
                for (int k = 0; k < 50; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f, 0, default, 2f);
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
			target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 300, true);
		}

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "Ravager";
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void OnKill()
        {
            DropHelper.DropBags(ModContent.ItemType<RavagerBag>(), NPC);

            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<RavagerTrophy>(), 10);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeRavager>(), true, !CalamityWorld.downedScavenger);
            DropHelper.DropResidentEvilAmmo(NPC.GetSource_FromThis(), NPC, CalamityWorld.downedScavenger, 4, 2, 1);

			CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.WitchDoctor }, CalamityWorld.downedScavenger);

			// All other drops are contained in the bag, so they only drop directly on Normal
			if (!Main.expertMode)
            {
                // Materials
				DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<FleshyGeodeT1>(), !CalamityWorld.downedProvidence);
				DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<FleshyGeodeT2>(), CalamityWorld.downedProvidence);

                // Weapons
                float w = DropHelper.DirectWeaponDropRateFloat;
                DropHelper.DropEntireWeightedSet(NPC.GetSource_FromThis(), NPC,
                    DropHelper.WeightStack<UltimusCleaver>(w),
                    DropHelper.WeightStack<RealmRavager>(w),
                    DropHelper.WeightStack<Hematemesis>(w),
                    DropHelper.WeightStack<SpikecragStaff>(w),
                    DropHelper.WeightStack<CraniumSmasher>(w)
                );

                // Equipment
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<BloodPact>(), 3);
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<FleshTotem>(), 3);

                // Vanity
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<RavagerMask>(), 7);
            }

            // Mark Ravager as dead
            CalamityWorld.downedScavenger = true;
            CalamityNetcode.SyncWorld();
        }
    }
}
