using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Projectiles.Boss;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.NPCs.DevourerofGods
{
    [AutoloadBossHead]
    public class DevourerofGodsHead : ModNPC
    {
		private enum LaserWallType
		{
			DiagonalRight = 0,
			DiagonalLeft = 1,
			DiagonalHorizontal = 2,
			DiagonalCross = 3
		}

		private const int shotSpacingMax = 750;
		private int shotSpacing = shotSpacingMax;
		private const int spacingVar = 250;
		private const int totalShots = 6;
		private int laserWallType = 0;
		private const float laserWallSpacingOffset = 16f;

		private bool tail = false;
        private const int minLength = 80;
        private const int maxLength = 81;
        private bool halfLife = false;
        private bool halfLife2 = false;
        private int spawnDoGCountdown = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Devourer of Gods");
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 5f;
            NPC.width = 104;
            NPC.height = 104;
            NPC.defense = 50;
			NPC.LifeMaxNERB(675000, 750000);
			double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.takenDamageMultiplier = 1.25f;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.value = Item.buyPrice(0, 75, 0, 0);
            NPC.alpha = 255;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
			NPC.DeathSound = SoundID.NPCDeath14;
            NPC.netAlways = true;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/ScourgeofTheUniverse");
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(halfLife);
            writer.Write(halfLife2);
            writer.Write(spawnDoGCountdown);
			writer.Write(shotSpacing);
			writer.Write(laserWallType);
            for (int i = 0; i < 3; i++)
                writer.Write(NPC.Calamity().newAI[i]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            halfLife = reader.ReadBoolean();
            halfLife2 = reader.ReadBoolean();
            spawnDoGCountdown = reader.ReadInt32();
			shotSpacing = reader.ReadInt32();
			laserWallType = reader.ReadInt32();
            for (int i = 0; i < 3; i++)
                NPC.Calamity().newAI[i] = reader.ReadSingle();
        }

        public override void AI()
        {
			CalamityGlobalNPC calamityGlobalNPC = NPC.Calamity();

            // whoAmI variable
            CalamityGlobalNPC.DoGHead = NPC.whoAmI;

            // Stop rain
            CalRD.StopRain();

            // Variables
            Vector2 vector = NPC.Center;
            bool flies = NPC.ai[2] == 0f;
			bool expertMode = Main.expertMode;
			bool revenge = CalamityWorld.revenge;
			bool death = CalamityWorld.death;

			// Percent life remaining
			float lifeRatio = NPC.life / (float)NPC.lifeMax;

			bool phase2 = lifeRatio < 0.75f;
			bool phase3 = lifeRatio < 0.2f;

            // Light
            Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2)) / 16f), (int)((NPC.position.Y + (NPC.height / 2)) / 16f), 0.2f, 0.05f, 0.2f);

            // Worm variable
            if (NPC.ai[3] > 0f)
                NPC.realLife = (int)NPC.ai[3];

			// Target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				NPC.TargetClosest(true);

			Player player = Main.player[NPC.target];

			bool increaseSpeed = Vector2.Distance(player.Center, vector) > CalamityGlobalNPC.CatchUpDistance200Tiles;
			bool increaseSpeedMore = Vector2.Distance(player.Center, vector) > CalamityGlobalNPC.CatchUpDistance350Tiles;

			// Spawn Guardians
			if (phase3)
            {
                if (!halfLife)
                {
                    if (revenge)
                        spawnDoGCountdown = 10;

                    string key = "Don't get cocky, kid!";
                    Color messageColor = Color.Cyan;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);

                    halfLife = true;
                }
                if (spawnDoGCountdown > 0)
                {
                    spawnDoGCountdown--;
                    if (spawnDoGCountdown == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
						SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/DevourerAttack"), player.position);

						for (int i = 0; i < 2; i++)
                            NPC.SpawnOnPlayer(NPC.FindClosestPlayer(), ModContent.NPCType<DevourerofGodsHead2>());
                    }
                }
            }
            else if (phase2)
            {
                if (!halfLife2)
                {
                    if (revenge)
                        spawnDoGCountdown = 10;

                    halfLife2 = true;
                }
                if (spawnDoGCountdown > 0)
                {
                    spawnDoGCountdown--;

					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/DevourerAttack"), player.position);

					if (spawnDoGCountdown == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                        NPC.SpawnOnPlayer(NPC.FindClosestPlayer(), ModContent.NPCType<DevourerofGodsHead2>());
                }
            }

            // Spawn dust
            if (NPC.alpha != 0)
            {
                for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                {
                    int num935 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 182, 0f, 0f, 100, default, 2f);
                    Main.dust[num935].noGravity = true;
                    Main.dust[num935].noLight = true;
                }
            }

            // Alpha
            NPC.alpha -= 12;
            if (NPC.alpha < 0)
                NPC.alpha = 0;

            // Spawn segments
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!tail && NPC.ai[0] == 0f)
                {
                    int Previous = NPC.whoAmI;
                    for (int segmentSpawn = 0; segmentSpawn < maxLength; segmentSpawn++)
                    {
                        int segment;
                        if (segmentSpawn >= 0 && segmentSpawn < minLength)
                            segment = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<DevourerofGodsBody>(), NPC.whoAmI);
                        else
                            segment = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<DevourerofGodsTail>(), NPC.whoAmI);

                        Main.npc[segment].realLife = NPC.whoAmI;
                        Main.npc[segment].ai[2] = NPC.whoAmI;
                        Main.npc[segment].ai[1] = Previous;
                        Main.npc[Previous].ai[0] = segment;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, segment, 0f, 0f, 0f, 0);
                        Previous = segment;
                    }
                    tail = true;
                }

				if (phase2)
				{
					float speed = 12f;
					float spawnOffset = 1500f;
					float divisor = death ? 360f : 480f;

					if (calamityGlobalNPC.newAI[1] % divisor == 0f)
					{
						SoundEngine.PlaySound(SoundID.Item12, player.position);

						// Side walls
						int type = ModContent.ProjectileType<DoGDeath>();
						int damage = NPC.GetProjectileDamage(type);
						Vector2 start = default;
						Vector2 velocity = default;
						Vector2 aim = expertMode ? player.Center + player.velocity * 20f : Vector2.Zero;
						Vector2 aimClone = aim;

						switch (laserWallType)
						{
							case (int)LaserWallType.DiagonalRight:

								for (int x = 0; x < totalShots + 1; x++)
								{
									start = new Vector2(player.position.X + spawnOffset, player.position.Y + shotSpacing);
									aim.Y += laserWallSpacingOffset * (x - 3);
									velocity = Vector2.Normalize(aim - start) * speed;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer, 0f, 0f);

									shotSpacing -= spacingVar;
								}

								laserWallType = (int)LaserWallType.DiagonalLeft;
								break;

							case (int)LaserWallType.DiagonalLeft:

								for (int x = 0; x < totalShots + 1; x++)
								{
									start = new Vector2(player.position.X - spawnOffset, player.position.Y + shotSpacing);
									aim.Y += laserWallSpacingOffset * (x - 3);
									velocity = Vector2.Normalize(aim - start) * speed;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer, 0f, 0f);

									shotSpacing -= spacingVar;
								}

								laserWallType = expertMode ? (int)LaserWallType.DiagonalHorizontal : (int)LaserWallType.DiagonalRight;
								break;

							case (int)LaserWallType.DiagonalHorizontal:

								for (int x = 0; x < totalShots + 1; x++)
								{
									start = new Vector2(player.position.X + spawnOffset, player.position.Y + shotSpacing);
									aim.Y += laserWallSpacingOffset * (x - 3);
									velocity = Vector2.Normalize(aim - start) * speed;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer, 0f, 0f);

									start = new Vector2(player.position.X - spawnOffset, player.position.Y + shotSpacing);
									velocity = Vector2.Normalize(aim - start) * speed;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer, 0f, 0f);

									shotSpacing -= spacingVar;
								}

								laserWallType = revenge ? (int)LaserWallType.DiagonalCross : (int)LaserWallType.DiagonalRight;
								break;

							case (int)LaserWallType.DiagonalCross:

								for (int x = 0; x < totalShots + 1; x++)
								{
									start = new Vector2(player.position.X + spawnOffset, player.position.Y + shotSpacing);
									aim.Y += laserWallSpacingOffset * (x - 3);
									velocity = Vector2.Normalize(aim - start) * speed;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer, 0f, 0f);

									start = new Vector2(player.position.X - spawnOffset, player.position.Y + shotSpacing);
									velocity = Vector2.Normalize(aim - start) * speed;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer, 0f, 0f);

									start = new Vector2(player.position.X + shotSpacing, player.position.Y + spawnOffset);
									aimClone.X += laserWallSpacingOffset * (x - 3);
									velocity = Vector2.Normalize(aimClone - start) * speed;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer, 0f, 0f);

									start = new Vector2(player.position.X + shotSpacing, player.position.Y - spawnOffset);
									velocity = Vector2.Normalize(aimClone - start) * speed;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer, 0f, 0f);

									shotSpacing -= spacingVar;
								}

								laserWallType = (int)LaserWallType.DiagonalRight;
								break;
						}
						shotSpacing = shotSpacingMax;
					}

					calamityGlobalNPC.newAI[1] += 1f;
				}
				else
					calamityGlobalNPC.newAI[1] = 0f;
			}

            // Despawn
            if (player.dead)
            {
				NPC.TargetClosest(false);
				flies = true;

                NPC.velocity.Y -= 3f;
                if ((double)NPC.position.Y < Main.topWorld + 16f)
                    NPC.velocity.Y -= 3f;

                if ((double)NPC.position.Y < Main.topWorld + 16f)
                {
                    for (int a = 0; a < 200; a++)
                    {
                        if (Main.npc[a].type == ModContent.NPCType<DevourerofGodsHead>() || Main.npc[a].type == ModContent.NPCType<DevourerofGodsBody>() || Main.npc[a].type == ModContent.NPCType<DevourerofGodsTail>())
                            Main.npc[a].active = false;
                    }
                }
            }

            // Movement
            int num180 = (int)(NPC.position.X / 16f) - 1;
            int num181 = (int)((NPC.position.X + NPC.width) / 16f) + 2;
            int num182 = (int)(NPC.position.Y / 16f) - 1;
            int num183 = (int)((NPC.position.Y + NPC.height) / 16f) + 2;

            if (num180 < 0)
                num180 = 0;
            if (num181 > Main.maxTilesX)
                num181 = Main.maxTilesX;
            if (num182 < 0)
                num182 = 0;
            if (num183 > Main.maxTilesY)
                num183 = Main.maxTilesY;

            if (NPC.velocity.X < 0f)
                NPC.spriteDirection = -1;
            else if (NPC.velocity.X > 0f)
                NPC.spriteDirection = 1;

            // Flight
            if (NPC.ai[2] == 0f)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active && Vector2.Distance(Main.player[Main.myPlayer].Center, vector) < CalamityGlobalNPC.CatchUpDistance350Tiles)
                        Main.player[Main.myPlayer].AddBuff(ModContent.BuffType<Warped>(), 2);
                }

                // Flying movement
                NPC.localAI[1] = 0f;

				float phaseChangeRate = 1f + (expertMode ? 9f * (1f - lifeRatio) : 0f);
				calamityGlobalNPC.newAI[2] += phaseChangeRate;

				float speed = death ? 16.5f : 15f;
				float turnSpeed = death ? 0.33f : 0.3f;
				float homingSpeed = death ? 22.5f : 18f;
				float homingTurnSpeed = death ? 0.405f : 0.33f;

				if (expertMode)
				{
					speed += 3f * (1f - lifeRatio);
					turnSpeed += 0.06f * (1f - lifeRatio);
					homingSpeed += 9f * (1f - lifeRatio);
					homingTurnSpeed += 0.15f * (1f - lifeRatio);
				}

				// Go to ground phase sooner
				if (increaseSpeedMore)
					calamityGlobalNPC.newAI[2] += 10f;
				else if (increaseSpeed)
					calamityGlobalNPC.newAI[2] += 2f;

				float num188 = speed;
                float num189 = turnSpeed;
                Vector2 vector18 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                float num191 = player.position.X + (player.width / 2);
                float num192 = player.position.Y + (player.height / 2);
                int num42 = -1;
                int num43 = (int)(player.Center.X / 16f);
                int num44 = (int)(player.Center.Y / 16f);

                for (int num45 = num43 - 2; num45 <= num43 + 2; num45++)
                {
                    for (int num46 = num44; num46 <= num44 + 15; num46++)
                    {
                        if (WorldGen.SolidTile2(num45, num46))
                        {
                            num42 = num46;
                            break;
                        }
                    }
                    if (num42 > 0)
                        break;
                }

                if (num42 > 0)
                {
                    num42 *= 16;
                    float num47 = num42 - 800;
                    if (player.position.Y > num47)
                    {
                        num192 = num47;
                        if (Math.Abs(NPC.Center.X - player.Center.X) < 500f)
                        {
                            if (NPC.velocity.X > 0f)
                                num191 = player.Center.X + 600f;
                            else
                                num191 = player.Center.X - 600f;
                        }
                    }
                }
                else
                {
                    num188 = homingSpeed;
                    num189 = homingTurnSpeed;
                }

				if (expertMode)
				{
					num188 += Vector2.Distance(player.Center, NPC.Center) * 0.005f * (1f - lifeRatio);
					num189 += Vector2.Distance(player.Center, NPC.Center) * 0.0001f * (1f - lifeRatio);
				}

                float num48 = num188 * 1.3f;
                float num49 = num188 * 0.7f;
                float num50 = NPC.velocity.Length();
                if (num50 > 0f)
                {
                    if (num50 > num48)
                    {
                        NPC.velocity.Normalize();
                        NPC.velocity *= num48;
                    }
                    else if (num50 < num49)
                    {
                        NPC.velocity.Normalize();
                        NPC.velocity *= num49;
                    }
                }

                num191 = (int)(num191 / 16f) * 16;
                num192 = (int)(num192 / 16f) * 16;
                vector18.X = (int)(vector18.X / 16f) * 16;
                vector18.Y = (int)(vector18.Y / 16f) * 16;
                num191 -= vector18.X;
                num192 -= vector18.Y;
                float num193 = (float)Math.Sqrt(num191 * num191 + num192 * num192);
                float num196 = Math.Abs(num191);
                float num197 = Math.Abs(num192);
                float num198 = num188 / num193;
                num191 *= num198;
                num192 *= num198;

                if ((NPC.velocity.X > 0f && num191 > 0f) || (NPC.velocity.X < 0f && num191 < 0f) || (NPC.velocity.Y > 0f && num192 > 0f) || (NPC.velocity.Y < 0f && num192 < 0f))
                {
                    if (NPC.velocity.X < num191)
                        NPC.velocity.X += num189;
                    else
                    {
                        if (NPC.velocity.X > num191)
                            NPC.velocity.X -= num189;
                    }

                    if (NPC.velocity.Y < num192)
                        NPC.velocity.Y += num189;
                    else
                    {
                        if (NPC.velocity.Y > num192)
                            NPC.velocity.Y -= num189;
                    }

                    if (Math.Abs(num192) < num188 * 0.2 && ((NPC.velocity.X > 0f && num191 < 0f) || (NPC.velocity.X < 0f && num191 > 0f)))
                    {
                        if (NPC.velocity.Y > 0f)
                            NPC.velocity.Y += num189 * 2f;
                        else
                            NPC.velocity.Y -= num189 * 2f;
                    }

                    if (Math.Abs(num191) < num188 * 0.2 && ((NPC.velocity.Y > 0f && num192 < 0f) || (NPC.velocity.Y < 0f && num192 > 0f)))
                    {
                        if (NPC.velocity.X > 0f)
                            NPC.velocity.X += num189 * 2f;
                        else
                            NPC.velocity.X -= num189 * 2f;
                    }
                }
                else
                {
                    if (num196 > num197)
                    {
                        if (NPC.velocity.X < num191)
                            NPC.velocity.X += num189 * 1.1f;
                        else if (NPC.velocity.X > num191)
                            NPC.velocity.X -= num189 * 1.1f;

                        if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < num188 * 0.5)
                        {
                            if (NPC.velocity.Y > 0f)
                                NPC.velocity.Y += num189;
                            else
                                NPC.velocity.Y -= num189;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y < num192)
                            NPC.velocity.Y += num189 * 1.1f;
                        else if (NPC.velocity.Y > num192)
                            NPC.velocity.Y -= num189 * 1.1f;

                        if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < num188 * 0.5)
                        {
                            if (NPC.velocity.X > 0f)
                                NPC.velocity.X += num189;
                            else
                                NPC.velocity.X -= num189;
                        }
                    }
                }

                NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + MathHelper.PiOver2;

                if (calamityGlobalNPC.newAI[2] > 900f)
                {
                    NPC.ai[2] = 1f;
					calamityGlobalNPC.newAI[2] = 0f;
                    NPC.netUpdate = true;
                }
            }

            // Ground
            else
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active && Vector2.Distance(Main.player[Main.myPlayer].Center, vector) < CalamityGlobalNPC.CatchUpDistance350Tiles)
                        Main.player[Main.myPlayer].AddBuff(ModContent.BuffType<ExtremeGrav>(), 2);
                }

				calamityGlobalNPC.newAI[2] += 1f;

                float fallSpeed = death ? 17.75f : 16f;
                float speed = death ? 0.22f : 0.18f;
                float turnSpeed = death ? 0.18f : 0.12f;

				if (expertMode)
				{
					fallSpeed += 3.5f * (1f - lifeRatio);
					speed += 0.08f * (1f - lifeRatio);
					turnSpeed += 0.12f * (1f - lifeRatio);
					speed += Vector2.Distance(player.Center, NPC.Center) * 0.00005f * (1f - lifeRatio);
					turnSpeed += Vector2.Distance(player.Center, NPC.Center) * 0.00005f * (1f - lifeRatio);
				}

                // Enrage
                if (increaseSpeedMore)
                {
					fallSpeed *= 3f;
                    speed *= 4f;
                    turnSpeed *= 6f;
                }
                else if (increaseSpeed)
                {
					fallSpeed *= 1.5f;
					speed *= 2f;
                    turnSpeed *= 3f;
                }

                if (!flies)
                {
                    for (int num952 = num180; num952 < num181; num952++)
                    {
                        for (int num953 = num182; num953 < num183; num953++)
                        {
                            if (Main.tile[num952, num953] != null && ((Main.tile[num952, num953].HasUnactuatedTile && (Main.tileSolid[Main.tile[num952, num953].TileType] || (Main.tileSolidTop[Main.tile[num952, num953].TileType] && Main.tile[num952, num953].TileFrameY == 0))) || Main.tile[num952, num953].LiquidAmount > 64))
                            {
                                Vector2 vector105;
                                vector105.X = num952 * 16;
                                vector105.Y = num953 * 16;
                                if (NPC.position.X + NPC.width > vector105.X && NPC.position.X < vector105.X + 16f && NPC.position.Y + NPC.height > vector105.Y && NPC.position.Y < vector105.Y + 16f)
                                {
                                    flies = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!flies)
                {
                    NPC.localAI[1] = 1f;
                    Rectangle rectangle12 = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);

					int num954 = death ? 1100 : 1175;
					if (expertMode)
						num954 -= (int)(150f * (1f - lifeRatio));
					if (num954 < 1025)
						num954 = 1025;

					bool flag95 = true;
                    if (NPC.position.Y > player.position.Y)
                    {
                        for (int num955 = 0; num955 < 255; num955++)
                        {
                            if (Main.player[num955].active)
                            {
                                Rectangle rectangle13 = new Rectangle((int)Main.player[num955].position.X - 1000, (int)Main.player[num955].position.Y - 1000, 2000, num954);
                                if (rectangle12.Intersects(rectangle13))
                                {
                                    flag95 = false;
                                    break;
                                }
                            }
                        }
                        if (flag95)
                            flies = true;
                    }
                }
                else
                    NPC.localAI[1] = 0f;

                Vector2 vector3 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                float num20 = player.position.X + (player.width / 2);
                float num21 = player.position.Y + (player.height / 2);
                num20 = (int)(num20 / 16f) * 16;
                num21 = (int)(num21 / 16f) * 16;
                vector3.X = (int)(vector3.X / 16f) * 16;
                vector3.Y = (int)(vector3.Y / 16f) * 16;
                num20 -= vector3.X;
                num21 -= vector3.Y;
                float num22 = (float)Math.Sqrt(num20 * num20 + num21 * num21);

                if (!flies)
                {
                    NPC.TargetClosest(true);

                    NPC.velocity.Y += turnSpeed;
                    if (NPC.velocity.Y > fallSpeed)
                        NPC.velocity.Y = fallSpeed;

                    if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < fallSpeed * 1.8)
                    {
                        if (NPC.velocity.X < 0f)
                            NPC.velocity.X -= speed * 1.1f;
                        else
                            NPC.velocity.X += speed * 1.1f;
                    }
                    else if (NPC.velocity.Y == fallSpeed)
                    {
                        if (NPC.velocity.X < num20)
                            NPC.velocity.X += speed;
                        else if (NPC.velocity.X > num20)
                            NPC.velocity.X -= speed;
                    }
                    else if (NPC.velocity.Y > 4f)
                    {
                        if (NPC.velocity.X < 0f)
                            NPC.velocity.X += speed * 0.9f;
                        else
                            NPC.velocity.X -= speed * 0.9f;
                    }
                }
                else
                {

					double maximumSpeed1 = death ? 0.44 : 0.4;
					double maximumSpeed2 = death ? 1.08 : 1D;

					if (increaseSpeedMore)
					{
						maximumSpeed1 *= 4;
						maximumSpeed2 *= 4;
					}
					if (increaseSpeed)
					{
						maximumSpeed1 *= 2;
						maximumSpeed2 *= 2;
					}

					if (expertMode)
					{
						maximumSpeed1 += 0.08f * (1f - lifeRatio);
						maximumSpeed2 += 0.16f * (1f - lifeRatio);
					}

                    num22 = (float)Math.Sqrt(num20 * num20 + num21 * num21);
                    float num25 = Math.Abs(num20);
                    float num26 = Math.Abs(num21);
                    float num27 = fallSpeed / num22;
                    num20 *= num27;
                    num21 *= num27;

                    if (((NPC.velocity.X > 0f && num20 > 0f) || (NPC.velocity.X < 0f && num20 < 0f)) && ((NPC.velocity.Y > 0f && num21 > 0f) || (NPC.velocity.Y < 0f && num21 < 0f)))
                    {
                        if (NPC.velocity.X < num20)
                            NPC.velocity.X += turnSpeed * 1.3f;
                        else if (NPC.velocity.X > num20)
                            NPC.velocity.X -= turnSpeed * 1.3f;

                        if (NPC.velocity.Y < num21)
                            NPC.velocity.Y += turnSpeed * 1.3f;
                        else if (NPC.velocity.Y > num21)
                            NPC.velocity.Y -= turnSpeed * 1.3f;
                    }

                    if ((NPC.velocity.X > 0f && num20 > 0f) || (NPC.velocity.X < 0f && num20 < 0f) || (NPC.velocity.Y > 0f && num21 > 0f) || (NPC.velocity.Y < 0f && num21 < 0f))
                    {
                        if (NPC.velocity.X < num20)
                            NPC.velocity.X += speed;
                        else if (NPC.velocity.X > num20)
                            NPC.velocity.X -= speed;
                        if (NPC.velocity.Y < num21)
                            NPC.velocity.Y += speed;
                        else if (NPC.velocity.Y > num21)
                            NPC.velocity.Y -= speed;

                        if (Math.Abs(num21) < fallSpeed * maximumSpeed1 && ((NPC.velocity.X > 0f && num20 < 0f) || (NPC.velocity.X < 0f && num20 > 0f)))
                        {
                            if (NPC.velocity.Y > 0f)
                                NPC.velocity.Y += speed * 2f;
                            else
                                NPC.velocity.Y -= speed * 2f;
                        }
                        if (Math.Abs(num20) < fallSpeed * maximumSpeed1 && ((NPC.velocity.Y > 0f && num21 < 0f) || (NPC.velocity.Y < 0f && num21 > 0f)))
                        {
                            if (NPC.velocity.X > 0f)
                                NPC.velocity.X += speed * 2f;
                            else
                                NPC.velocity.X -= speed * 2f;
                        }
                    }
                    else if (num25 > num26)
                    {
                        if (NPC.velocity.X < num20)
                            NPC.velocity.X += speed * 1.1f;
                        else if (NPC.velocity.X > num20)
                            NPC.velocity.X -= speed * 1.1f;

                        if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < fallSpeed * maximumSpeed2)
                        {
                            if (NPC.velocity.Y > 0f)
                                NPC.velocity.Y += speed;
                            else
                                NPC.velocity.Y -= speed;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y < num21)
                            NPC.velocity.Y += speed * 1.1f;
                        else if (NPC.velocity.Y > num21)
                            NPC.velocity.Y -= speed * 1.1f;

                        if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < fallSpeed * maximumSpeed2)
                        {
                            if (NPC.velocity.X > 0f)
                                NPC.velocity.X += speed;
                            else
                                NPC.velocity.X -= speed;
                        }
                    }
                }

                NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + MathHelper.PiOver2;

                if (flies)
                {
                    if (NPC.localAI[0] != 1f)
                        NPC.netUpdate = true;

                    NPC.localAI[0] = 1f;
                }
                else
                {
                    if (NPC.localAI[0] != 0f)
                        NPC.netUpdate = true;

                    NPC.localAI[0] = 0f;
                }

                if (((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f) || (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f) || (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f) || (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f)) && !NPC.justHit)
                    NPC.netUpdate = true;

                if (calamityGlobalNPC.newAI[2] > 900f)
                {
                    NPC.ai[2] = 0f;
					calamityGlobalNPC.newAI[2] = 0f;
                    NPC.netUpdate = true;
                }
            }
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / 2);

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2(texture2D15.Width, texture2D15.Height) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/DevourerofGods/DevourerofGodsHeadGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Fuchsia, 0.5f);

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/DevourerofGods/DevourerofGodsHeadGlow2").Value;
			color37 = Color.Lerp(Color.White, Color.Cyan, 0.5f);

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.None;
        }

        // DoG phase 1 does not drop loot, but starts the sentinel phase of the fight.
        public override void OnKill()
        {
            // Skip the sentinel phase entirely if DoG has already been killed
            CalamityWorld.DoGSecondStageCountdown = (CalamityWorld.downedDoG || CalamityWorld.downedSecondSentinels) ? 600 : 21600;

            if (Main.netMode == NetmodeID.Server)
            {
                var netMessage = Mod.GetPacket();
                netMessage.Write((byte)CalRDMessageType.DoGCountdownSync);
                netMessage.Write(CalamityWorld.DoGSecondStageCountdown);
                netMessage.Send();
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            modifiers.ModifyHitInfo += ButcherBlock;
        }

        public void ButcherBlock(ref NPC.HitInfo hit)
        {
            if (hit.Damage >= NPC.lifeMax * 0.5f)
            {
                string key = "You think...you can butcher...ME!?";
                Color messageColor = Color.Cyan;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
                hit.Damage = 0;
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 8;
                SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/NPCHit/OtherworldlyHit"), NPC.Center);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DoGHead").Type, 1f);
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 50;
                NPC.height = 50;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 15; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 30; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 2f);
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
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300, true);
            target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 420, true);
            target.AddBuff(BuffID.Frostburn, 300, true);
            if (CalamityWorld.death && !target.Calamity().lol)
            {
                target.KillMe(PlayerDeathReason.ByCustomReason(target.name + "'s essence was consumed by the devourer."), 1000.0, 0, false);
            }

			if (target.Calamity().dogTextCooldown <= 0)
			{
				string text = Utils.SelectRandom(Main.rand, new string[]
				{
					"A fatal mistake!",
					"Good luck recovering from that!",
					"Delicious...",
					"Did that hurt?",
					"Nothing personal, kid."
				});
				Color messageColor = Color.Cyan;
				Rectangle location = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
				CombatText.NewText(location, messageColor, Language.GetTextValue(text), true);
                target.Calamity().dogTextCooldown = 60;
			}
        }
    }
}
