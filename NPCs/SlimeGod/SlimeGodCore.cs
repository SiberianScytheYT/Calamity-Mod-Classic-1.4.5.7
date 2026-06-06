using CalRD.Buffs.StatDebuffs;
using CalRD.CalPlayer;
using CalRD.Events;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.Potions;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.TownNPCs;
using CalRD.Projectiles.Boss;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.SlimeGod
{
	[AutoloadBossHead]
    public class SlimeGodCore : ModNPC
    {
		private bool slimesSpawned = false;
		private int buffedSlime = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Slime God");
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 10f;
            NPC.width = 44;
            NPC.height = 44;
            NPC.defense = 6;
            NPC.LifeMaxNERB(2000, 2500, 2500000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPCID.Sets.TrailCacheLength[NPC.type] = 8;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.buffImmune[ModContent.BuffType<GlacialState>()] = true;
            NPC.buffImmune[ModContent.BuffType<TemporalSadness>()] = true;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 8, 0, 0);
            NPC.alpha = 55;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Mod CalamityModMusic = ModLoader.HasMod("CalamityModMusic") ? ModLoader.GetMod("CalamityModMusic") : null;
            if (CalamityModMusic != null)
                Music = MusicLoader.GetMusicSlot("CalamityModMusic/Sounds/Music/SlimeGod");
            else
                Music = MusicID.Boss1;
        }

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(NPC.dontTakeDamage);
			writer.Write(NPC.localAI[0]);
			writer.Write(NPC.localAI[1]);
			writer.Write(NPC.localAI[2]);
			writer.Write(NPC.localAI[3]);
			writer.Write(buffedSlime);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			NPC.dontTakeDamage = reader.ReadBoolean();
			NPC.localAI[0] = reader.ReadSingle();
			NPC.localAI[1] = reader.ReadSingle();
			NPC.localAI[2] = reader.ReadSingle();
			NPC.localAI[3] = reader.ReadSingle();
			buffedSlime = reader.ReadInt32();
		}

		public override void AI()
        {
            CalamityGlobalNPC.slimeGod = NPC.whoAmI;

            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
            bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;

			// Percent life remaining
			float lifeRatio = NPC.life / (float)NPC.lifeMax;

			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];

			Vector2 vectorCenter = NPC.Center;
			if (Main.netMode != NetmodeID.MultiplayerClient && !slimesSpawned)
			{
				slimesSpawned = true;
				NPC.NewNPC(NPC.GetSource_FromThis(), (int)vectorCenter.X, (int)vectorCenter.Y, ModContent.NPCType<SlimeGod>());
				NPC.NewNPC(NPC.GetSource_FromThis(), (int)vectorCenter.X, (int)vectorCenter.Y, ModContent.NPCType<SlimeGodRun>());
			}

			// Emit dust
			int randomDust = Main.rand.NextBool(2) ? 173 : 260;
            int num658 = Dust.NewDust(NPC.position, NPC.width, NPC.height, randomDust, NPC.velocity.X, NPC.velocity.Y, 255, new Color(0, 80, 255, 80), NPC.scale * 1.5f);
            Main.dust[num658].noGravity = true;
            Main.dust[num658].velocity *= 0.5f;

			NPC.dontTakeDamage = false;

			// Set damage
			NPC.damage = NPC.defDamage;

			// Enrage based on large slimes
			bool phase2 = lifeRatio < 0.4f;
			bool hyperMode = true;
			bool purpleSlimeAlive = false;
			bool redSlimeAlive = false;

			if (CalamityGlobalNPC.slimeGodPurple != -1)
			{
				if (Main.npc[CalamityGlobalNPC.slimeGodPurple].active)
				{
					if (buffedSlime == 1)
						Main.npc[CalamityGlobalNPC.slimeGodPurple].localAI[1] = 1f;
					else
						Main.npc[CalamityGlobalNPC.slimeGodPurple].localAI[1] = 0f;

					NPC.Calamity().newAI[0] = Main.npc[CalamityGlobalNPC.slimeGodPurple].Center.X;
					NPC.Calamity().newAI[1] = Main.npc[CalamityGlobalNPC.slimeGodPurple].Center.Y;

					purpleSlimeAlive = true;
					phase2 = lifeRatio < 0.2f;
					hyperMode = false;
				}
			}

			if (CalamityGlobalNPC.slimeGodRed != -1)
			{
				if (Main.npc[CalamityGlobalNPC.slimeGodRed].active)
				{
					if (buffedSlime == 2)
						Main.npc[CalamityGlobalNPC.slimeGodRed].localAI[1] = 1f;
					else
						Main.npc[CalamityGlobalNPC.slimeGodRed].localAI[1] = 0f;

					NPC.localAI[2] = Main.npc[CalamityGlobalNPC.slimeGodRed].Center.X;
					NPC.localAI[3] = Main.npc[CalamityGlobalNPC.slimeGodRed].Center.Y;

					redSlimeAlive = true;
					phase2 = lifeRatio < 0.2f;
					hyperMode = false;
				}
			}

			// Despawn
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
					if (NPC.velocity.Y < -3f)
						NPC.velocity.Y = -3f;
					NPC.velocity.Y += 0.2f;
					if (NPC.velocity.Y > 16f)
						NPC.velocity.Y = 16f;

					if (NPC.position.Y > Main.worldSurface * 16.0)
                    {
                        for (int x = 0; x < Main.maxNPCs; x++)
                        {
                            if (Main.npc[x].type == ModContent.NPCType<SlimeGod>() || Main.npc[x].type == ModContent.NPCType<SlimeGodSplit>() ||
                                Main.npc[x].type == ModContent.NPCType<SlimeGodRun>() || Main.npc[x].type == ModContent.NPCType<SlimeGodRunSplit>())
                            {
                                Main.npc[x].active = false;
                                Main.npc[x].netUpdate = true;
                            }
                        }
                        NPC.active = false;
                        NPC.netUpdate = true;
                    }

					if (NPC.ai[0] != 0f || NPC.ai[1] != 0f)
					{
						NPC.ai[0] = 0f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						NPC.ai[3] = 0f;
						NPC.localAI[0] = 0f;
						NPC.localAI[1] = 0f;
						NPC.netUpdate = true;
					}
					return;
                }
            }
            else if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

			float ai1 = hyperMode ? 270f : 360f;

			// Hide inside large slime
			if (!hyperMode && NPC.ai[1] < ai1)
			{
				if (NPC.Calamity().newAI[2] == 0f && NPC.life > 0)
				{
					NPC.Calamity().newAI[2] = NPC.lifeMax;
				}
				if (NPC.life > 0)
				{
					int num660 = (int)(NPC.lifeMax * 0.05);
					if ((NPC.life + num660) < NPC.Calamity().newAI[2])
					{
						NPC.Calamity().newAI[2] = NPC.life;
						NPC.Calamity().newAI[3] = 1f;
						SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SlimeGodPossession"), NPC.position);
					}
				}

				if (NPC.Calamity().newAI[3] == 1f)
				{
					NPC.dontTakeDamage = true;

					NPC.rotation = NPC.velocity.X * 0.1f;

					if (buffedSlime == 0)
					{
						if (purpleSlimeAlive && redSlimeAlive)
							buffedSlime = Main.rand.Next(2) + 1;
						else if (purpleSlimeAlive)
							buffedSlime = 1;
						else if (redSlimeAlive)
							buffedSlime = 2;
					}

					Vector2 purpleSlimeVector = new Vector2(NPC.Calamity().newAI[0], NPC.Calamity().newAI[1]);
					Vector2 redSlimeVector = new Vector2(NPC.localAI[2], NPC.localAI[3]);
					Vector2 goToVector = buffedSlime == 1 ? purpleSlimeVector : redSlimeVector;

					Vector2 goToPosition = goToVector - vectorCenter;
					NPC.velocity = Vector2.Normalize(goToPosition) * 24f;

					bool slimeDead = false;
					if (goToVector == purpleSlimeVector)
						slimeDead = CalamityGlobalNPC.slimeGodPurple < 0 || !Main.npc[CalamityGlobalNPC.slimeGodPurple].active;
					else
						slimeDead = CalamityGlobalNPC.slimeGodRed < 0 || !Main.npc[CalamityGlobalNPC.slimeGodRed].active;

					NPC.ai[2] += 1f;
					if (NPC.ai[2] >= 600f || slimeDead)
					{
						NPC.ai[2] = 0f;
						NPC.Calamity().newAI[3] = 0f;
						SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SlimeGodExit"), NPC.position);
						for (int i = 0; i < 20; i++)
						{
							int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 4, 0f, 0f, 100, default, 2f);
							Main.dust[dust].velocity *= 3f;
							if (Main.rand.NextBool(2))
							{
								Main.dust[dust].scale = 0.5f;
								Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
							}
						}
						for (int j = 0; j < 30; j++)
						{
							int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 4, 0f, 0f, 100, default, 3f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 5f;
							dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 4, 0f, 0f, 100, default, 2f);
							Main.dust[dust].velocity *= 2f;
						}
					}

					return;
				}

				buffedSlime = 0;
			}

			// Spin and shoot orbs
            if (phase2)
            {
				NPC.ai[1] += 1f;
				if (revenge)
				{
					if (NPC.ai[1] >= ai1)
					{
						if (NPC.localAI[1] == 0f)
						{
							// Slow down, rotation
							NPC.rotation = NPC.velocity.X * 0.1f;

							// Set teleport location, turn invisible, spin direction
							NPC.alpha += 20;
							if (NPC.alpha >= 255)
							{
								NPC.alpha = 255;
								NPC.velocity.Normalize();

								int teleportX = player.velocity.X < 0f ? -20 : 20;
								int teleportY = player.velocity.Y < 0f ? -10 : 10;
								int playerPosX = (int)player.Center.X / 16 + teleportX;
								int playerPosY = (int)player.Center.Y / 16 - teleportY;

								NPC.ai[2] = playerPosX;
								NPC.ai[3] = playerPosY;
								NPC.localAI[1] = 1f;
								NPC.netUpdate = true;
							}
						}
						else if (NPC.localAI[1] == 1f)
						{
							// Rotation
							NPC.rotation = NPC.velocity.X * 0.1f;

							// Teleport to location
							if (NPC.alpha == 255)
							{
								Vector2 position = new Vector2(NPC.ai[2] * 16f - (NPC.width / 2), NPC.ai[3] * 16f - (NPC.height / 2));
								NPC.position = position;
							}

							// Turn visible
							NPC.alpha -= 20;
							if (NPC.alpha < 55)
							{
								NPC.alpha = 55;
								NPC.localAI[0] = vectorCenter.X - player.Center.X < 0 ? 1f : -1f;
								NPC.localAI[1] = 2f;
							}
							NPC.netUpdate = true;
						}
						else
						{
							// No damage while spinning
							NPC.damage = 0;

							// Rotation
							NPC.rotation += NPC.direction * 0.3f;

							// Velocity boost
							if (NPC.localAI[1] == 2f)
							{
								NPC.localAI[1] = 3f;
								NPC.velocity *= 12f;
							}

							// Spin velocity
							float velocity = MathHelper.TwoPi / (180f - (NPC.ai[1] - ai1));
							NPC.velocity = NPC.velocity.RotatedBy(-(double)velocity * NPC.localAI[0]);

							// Reset and charge at target
							if (NPC.ai[1] >= ai1 + 100f)
							{
								NPC.ai[1] = 0f;
								NPC.ai[2] = 0f;
								NPC.ai[3] = 0f;
								NPC.localAI[0] = 0f;
								NPC.localAI[1] = 0f;
								float chargeVelocity = death ? 12f : 9f;
								NPC.velocity = Vector2.Normalize(player.Center - vectorCenter) * chargeVelocity;
								return;
							}

							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								if (NPC.ai[1] % 15f == 0f && Vector2.Distance(player.Center, vectorCenter) > 160f)
								{
									if (expertMode && Main.rand.NextBool(2))
									{
										float num179 = revenge ? 2f : 3f;
										if (BossRushEvent.BossRushActive)
											num179 = 12f;
										Vector2 value9 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
										float num180 = player.position.X + player.width * 0.5f - value9.X;
										float num181 = Math.Abs(num180) * 0.1f;
										float num182 = player.position.Y + player.height * 0.5f - value9.Y - num181;
										float num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
										NPC.netUpdate = true;
										num183 = num179 / num183;
										num180 *= num183;
										num182 *= num183;
										int type = Main.rand.NextBool(2) ? ModContent.ProjectileType<AbyssMine>() : ModContent.ProjectileType<AbyssMine2>();
										int damage = NPC.GetProjectileDamage(type);
										value9.X += num180;
										value9.Y += num182;
										num180 = player.position.X + player.width * 0.5f - value9.X;
										num182 = player.position.Y + player.height * 0.5f - value9.Y;
										num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
										num183 = num179 / num183;
										num180 *= num183;
										num182 *= num183;
										Projectile.NewProjectile(NPC.GetSource_FromThis(), value9.X, value9.Y, num180, num182, type, damage, 0f, Main.myPlayer, 0f, 0f);
									}
									else
									{
										float num179 = revenge ? 6f : 5f;
										if (BossRushEvent.BossRushActive)
											num179 = 12f;
										Vector2 value9 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
										float num180 = player.position.X + player.width * 0.5f - value9.X;
										float num181 = Math.Abs(num180) * 0.1f;
										float num182 = player.position.Y + player.height * 0.5f - value9.Y - num181;
										float num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
										NPC.netUpdate = true;
										num183 = num179 / num183;
										num180 *= num183;
										num182 *= num183;
										int type = Main.rand.NextBool(2) ? ModContent.ProjectileType<AbyssBallVolley>() : ModContent.ProjectileType<AbyssBallVolley2>();
										int damage = NPC.GetProjectileDamage(type);
										value9.X += num180;
										value9.Y += num182;
										num180 = player.position.X + player.width * 0.5f - value9.X;
										num182 = player.position.Y + player.height * 0.5f - value9.Y;
										num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
										num183 = num179 / num183;
										num180 *= num183;
										num182 *= num183;
										Projectile.NewProjectile(NPC.GetSource_FromThis(), value9.X, value9.Y, num180, num182, type, damage, 0f, Main.myPlayer, 0f, 0f);
									}
								}
							}
						}
						return;
					}
				}
				else
				{
					if (Main.netMode != NetmodeID.MultiplayerClient && Vector2.Distance(player.Center, vectorCenter) > 160f)
					{
						if (NPC.ai[1] % 40f == 0f)
						{
							if (expertMode && Main.rand.NextBool(2))
							{
								float num179 = revenge ? 2f : 3f;
								if (BossRushEvent.BossRushActive)
									num179 = 12f;
								Vector2 value9 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
								float num180 = player.position.X + player.width * 0.5f - value9.X;
								float num181 = Math.Abs(num180) * 0.1f;
								float num182 = player.position.Y + player.height * 0.5f - value9.Y - num181;
								float num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
								NPC.netUpdate = true;
								num183 = num179 / num183;
								num180 *= num183;
								num182 *= num183;
								int type = Main.rand.NextBool(2) ? ModContent.ProjectileType<AbyssMine>() : ModContent.ProjectileType<AbyssMine2>();
								int damage = NPC.GetProjectileDamage(type);
								value9.X += num180;
								value9.Y += num182;
								num180 = player.position.X + player.width * 0.5f - value9.X;
								num182 = player.position.Y + player.height * 0.5f - value9.Y;
								num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
								num183 = num179 / num183;
								num180 *= num183;
								num182 *= num183;
								Projectile.NewProjectile(NPC.GetSource_FromThis(), value9.X, value9.Y, num180, num182, type, damage, 0f, Main.myPlayer, 0f, 0f);
							}
							else
							{
								float num179 = revenge ? 6f : 5f;
								if (BossRushEvent.BossRushActive)
									num179 = 12f;
								Vector2 value9 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
								float num180 = player.position.X + player.width * 0.5f - value9.X;
								float num181 = Math.Abs(num180) * 0.1f;
								float num182 = player.position.Y + player.height * 0.5f - value9.Y - num181;
								float num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
								NPC.netUpdate = true;
								num183 = num179 / num183;
								num180 *= num183;
								num182 *= num183;
								int type = Main.rand.NextBool(2) ? ModContent.ProjectileType<AbyssBallVolley>() : ModContent.ProjectileType<AbyssBallVolley2>();
								int damage = NPC.GetProjectileDamage(type);
								value9.X += num180;
								value9.Y += num182;
								for (int num186 = 0; num186 < 2; num186++)
								{
									num180 = player.position.X + player.width * 0.5f - value9.X;
									num182 = player.position.Y + player.height * 0.5f - value9.Y;
									num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
									num183 = num179 / num183;
									num180 += Main.rand.Next(-30, 31);
									num182 += Main.rand.Next(-30, 31);
									num180 *= num183;
									num182 *= num183;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), value9.X, value9.Y, num180, num182, type, damage, 0f, Main.myPlayer, 0f, 0f);
								}
							}
						}
					}
				}
            }

            float num1372 = death ? 14f : revenge ? 11f : expertMode ? 8.5f : 6f;
            if (phase2)
            {
                num1372 = revenge ? 18f : expertMode ? 16f : 14f;
            }
            if (BossRushEvent.BossRushActive || player.gravDir == -1f)
            {
                num1372 = 22f;
            }
            if (NPC.Calamity().enraged > 0 || player.gravDir == -1f || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
            {
                num1372 += 8f;
            }
			if (hyperMode)
			{
				num1372 *= 1.25f;
			}

            Vector2 vector167 = new Vector2(vectorCenter.X + (NPC.direction * 20), vectorCenter.Y + 6f);
            float num1373 = player.position.X + player.width * 0.5f - vector167.X;
            float num1374 = player.Center.Y - vector167.Y;
            float num1375 = (float)Math.Sqrt(num1373 * num1373 + num1374 * num1374);
            float num1376 = num1372 / num1375;
            num1373 *= num1376;
            num1374 *= num1376;

            NPC.ai[0] -= 1f;
            if (num1375 < 200f || NPC.ai[0] > 0f)
            {
                if (num1375 < 200f)
                {
                    NPC.ai[0] = 20f;
                }
                if (NPC.velocity.X < 0f)
                {
                    NPC.direction = -1;
                }
                else
                {
                    NPC.direction = 1;
                }
				NPC.rotation += NPC.direction * 0.3f;
				return;
            }

            NPC.velocity.X = (NPC.velocity.X * 50f + num1373) / 51f;
            NPC.velocity.Y = (NPC.velocity.Y * 50f + num1374) / 51f;
            if (num1375 < 350f)
            {
                NPC.velocity.X = (NPC.velocity.X * 10f + num1373) / 11f;
                NPC.velocity.Y = (NPC.velocity.Y * 10f + num1374) / 11f;
            }
            if (num1375 < 300f)
            {
                NPC.velocity.X = (NPC.velocity.X * 7f + num1373) / 8f;
                NPC.velocity.Y = (NPC.velocity.Y * 7f + num1374) / 8f;
            }
			NPC.rotation = NPC.velocity.X * 0.1f;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Color color24 = NPC.GetAlpha(drawColor);
            Color color25 = Lighting.GetColor((int)((double)NPC.position.X + (double)NPC.width * 0.5) / 16, (int)(((double)NPC.position.Y + (double)NPC.height * 0.5) / 16.0));
            Texture2D texture2D3 = TextureAssets.Npc[NPC.type].Value;
            int num156 = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
            int y3 = num156 * (int)NPC.frameCounter;
            Rectangle rectangle = new Rectangle(0, y3, texture2D3.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            int num157 = 8;
            int num158 = 2;
            int num159 = 1;
            float num160 = 0f;
            int num161 = num159;
            spriteBatch.Draw(texture2D3, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, color24, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, spriteEffects, 0);
            while (((num158 > 0 && num161 < num157) || (num158 < 0 && num161 > num157)) && CalamityConfig.Instance.Afterimages)
            {
                Color color26 = NPC.GetAlpha(color25);
                {
                    goto IL_6899;
                }
                IL_6881:
                num161 += num158;
                continue;
                IL_6899:
                float num164 = (float)(num157 - num161);
                if (num158 < 0)
                {
                    num164 = (float)(num159 - num161);
                }
                color26 *= num164 / ((float)NPCID.Sets.TrailCacheLength[NPC.type] * 1.5f);
                Vector2 value4 = NPC.oldPos[num161];
                float num165 = NPC.rotation;
                Main.spriteBatch.Draw(texture2D3, value4 + NPC.Size / 2f - Main.screenPosition + new Vector2(0, NPC.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, num165 + NPC.rotation * num160 * (float)(num161 - 1) * -(float)spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin2, NPC.scale, spriteEffects, 0f);
                goto IL_6881;
			}
			return false;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override void OnKill()
        {
            bool otherSlimeGodsAlive =
                NPC.AnyNPCs(ModContent.NPCType<SlimeGod>()) ||
                NPC.AnyNPCs(ModContent.NPCType<SlimeGodSplit>()) ||
                NPC.AnyNPCs(ModContent.NPCType<SlimeGodRun>()) ||
                NPC.AnyNPCs(ModContent.NPCType<SlimeGodRunSplit>());
            if (!otherSlimeGodsAlive)
                DropSlimeGodLoot(NPC);
        }

        // This loot code is shared with every other Slime God component.
        public static void DropSlimeGodLoot(NPC npc)
        {
            CalRD mod = ModContent.GetInstance<CalRD>();
            DropHelper.DropBags(ModContent.ItemType<SlimeGodBag>(), npc);

            DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<SlimeGodTrophy>(), 10);
            DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeSlimeGod>(), true, !CalamityWorld.downedSlimeGod);
            DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, CalamityWorld.downedSlimeGod, 3, 1, 0);

			CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Dryad, ModContent.NPCType<THIEF>() }, CalamityWorld.downedSlimeGod);

			// Purified Jam is once per player, but drops for all players.
			CalamityPlayer mp = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].Calamity();
            if (!mp.revJamDrop)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<PurifiedJam>(), true, CalamityWorld.revenge && !CalamityWorld.downedSlimeGod, 6, 8);
                mp.revJamDrop = true;
            }

            // Gel always drops directly, even on Expert
            DropHelper.DropItemSpray(npc.GetSource_FromThis(), npc, ItemID.Gel, 180, 250);

            // All other drops are contained in the bag, so they only drop directly on Normal
            if (!Main.expertMode)
            {
                // Materials
                DropHelper.DropItemSpray(npc.GetSource_FromThis(), npc, ModContent.ItemType<PurifiedGel>(), 30, 45);

				// Weapons
				float w = DropHelper.DirectWeaponDropRateFloat;
				DropHelper.DropEntireWeightedSet(npc.GetSource_FromThis(), npc,
					DropHelper.WeightStack<OverloadedBlaster>(w),
					DropHelper.WeightStack<AbyssalTome>(w),
					DropHelper.WeightStack<EldritchTome>(w),
					DropHelper.WeightStack<CorroslimeStaff>(w),
					DropHelper.WeightStack<CrimslimeStaff>(w)
				);

				// Vanity
				DropHelper.DropItemFromSetChance(npc.GetSource_FromThis(), npc, 0.142857f, ModContent.ItemType<SlimeGodMask>(), ModContent.ItemType<SlimeGodMask2>());

                // Other
            }

            // Mark the Slime God as dead
            CalamityWorld.downedSlimeGod = true;
            CalamityNetcode.SyncWorld();
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 40;
                NPC.height = 40;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 4, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 4, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 4, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.VortexDebuff, 120, true);
        }
    }
}
