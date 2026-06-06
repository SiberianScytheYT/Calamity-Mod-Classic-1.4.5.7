using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Events;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.Placeables.FurnitureCosmilite;
using CalRD.Items.Potions;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
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
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.NPCs.DevourerofGods
{
    [AutoloadBossHead]
    public class DevourerofGodsHeadS : ModNPC
    {
		private enum LaserWallPhase
		{
			SetUp = 0,
			FireLaserWalls = 1,
			End = 2
		}

		private enum LaserWallType
		{
			Normal = 0,
			Offset = 1,
			MultiLayered = 2,
			DiagonalHorizontal = 3,
			DiagonalVertical = 4
		}

		private bool tail = false;
        private const int minLength = 100;
        private const int maxLength = 101;
        private bool halfLife = false;

		private const int shotSpacingMax = 1050;
		private int[] shotSpacing = new int[4] { shotSpacingMax, shotSpacingMax, shotSpacingMax, shotSpacingMax };
        private const int spacingVar = 105;
		private const int diagonalSpacingVar = 350;
        private const int totalShots = 20;
		private const int totalDiagonalShots = 6;
		private const float laserWallSpacingOffset = 16f;
		private int laserWallType = 0;
		public int laserWallPhase = 0;

		private const int idleCounterMax = 360;
        private int idleCounter = idleCounterMax;
		private int postTeleportTimer = 0;
		private int teleportTimer = -1;

		private const float alphaGateValue = 669f;

		public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Devourer of Gods");
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 5f;
            NPC.width = 186;
            NPC.height = 186;
            NPC.defense = 50;
            NPC.LifeMaxNERB(1150000, 1350000, 9200000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.takenDamageMultiplier = 1.25f;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.value = Item.buyPrice(1, 0, 0, 0);
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
            Mod CalamityModMusic = ModLoader.HasMod("CalamityModMusic") ? ModLoader.GetMod("CalamityModMusic") : null;
            if (CalamityModMusic != null)
                Music = MusicLoader.GetMusicSlot("CalamityModMusic/Sounds/Music/DevourerofGodsPhase2");
            else
                Music = MusicID.LunarBoss;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(NPC.dontTakeDamage);
            writer.Write(halfLife);
            writer.Write(shotSpacing[0]);
            writer.Write(shotSpacing[1]);
            writer.Write(shotSpacing[2]);
            writer.Write(shotSpacing[3]);
            writer.Write(idleCounter);
            writer.Write(laserWallPhase);
			writer.Write(postTeleportTimer);
			writer.Write(laserWallType);
			writer.Write(teleportTimer);
            writer.Write(NPC.alpha);
		}

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			NPC.dontTakeDamage = reader.ReadBoolean();
            halfLife = reader.ReadBoolean();
            shotSpacing[0] = reader.ReadInt32();
            shotSpacing[1] = reader.ReadInt32();
            shotSpacing[2] = reader.ReadInt32();
            shotSpacing[3] = reader.ReadInt32();
            idleCounter = reader.ReadInt32();
            laserWallPhase = reader.ReadInt32();
			postTeleportTimer = reader.ReadInt32();
			laserWallType = reader.ReadInt32();
			teleportTimer = reader.ReadInt32();
            NPC.alpha = reader.ReadInt32();
		}

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation;
        }

        public override void AI()
        {
            CalamityGlobalNPC calamityGlobalNPC = NPC.Calamity();

            // whoAmI variable
            CalamityGlobalNPC.DoGHead = NPC.whoAmI;

            // Percent life remaining
            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            // Variables
            Vector2 vector = NPC.Center;
            bool flies = NPC.ai[2] == 0f;
            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
            bool phase2 = lifeRatio < 0.75f;
            bool phase3 = lifeRatio < 0.3f;
            bool breathFireMore = lifeRatio < 0.15f;

			// Light
			Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2)) / 16f), (int)((NPC.position.Y + (NPC.height / 2)) / 16f), 0.2f, 0.05f, 0.2f);

			// Worm shit again
			if (NPC.ai[3] > 0f)
				NPC.realLife = (int)NPC.ai[3];

			// Target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				NPC.TargetClosest(true);

			Player player = Main.player[NPC.target];

			float distanceFromTarget = Vector2.Distance(player.Center, vector);
			bool increaseSpeed = distanceFromTarget > CalamityGlobalNPC.CatchUpDistance200Tiles;
			bool increaseSpeedMore = distanceFromTarget > CalamityGlobalNPC.CatchUpDistance350Tiles;

			// Immunity after teleport
			NPC.dontTakeDamage = postTeleportTimer > 0;

			// Teleport
			if (teleportTimer >= 0)
			{
				teleportTimer--;
				if (teleportTimer == 0)
					Teleport(player, death, revenge, expertMode);
			}

			// Laser walls
			if (phase2 && !phase3 && postTeleportTimer <= 0)
            {
				if (laserWallPhase == (int)LaserWallPhase.SetUp)
				{
					// Increment next laser wall phase timer
					calamityGlobalNPC.newAI[3] += 1f;

					// Set alpha value prior to firing laser walls
					if (calamityGlobalNPC.newAI[3] > alphaGateValue)
					{
						// Disable teleports
						if (teleportTimer >= 0)
						{
							GetRiftLocation(false);
							teleportTimer = -1;
						}

						NPC.alpha = (int)MathHelper.Clamp((calamityGlobalNPC.newAI[3] - alphaGateValue) * 5f, 0f, 255f);
					}

					// Fire laser walls every 12 seconds after a laser wall phase ends
					if (calamityGlobalNPC.newAI[3] >= 720f)
					{
						NPC.alpha = 255;

						// Reset laser wall timer to 0
						calamityGlobalNPC.newAI[1] = 0f;

						calamityGlobalNPC.newAI[3] = 0f;
						laserWallPhase = (int)LaserWallPhase.FireLaserWalls;
					}
				}
				else if (laserWallPhase == (int)LaserWallPhase.FireLaserWalls)
				{
					// Remain in laser wall firing phase for 6 seconds
					idleCounter--;
					if (idleCounter <= 0)
					{
						SpawnTeleportLocation(player);
						laserWallPhase = (int)LaserWallPhase.End;
						idleCounter = idleCounterMax;
					}
				}
				else if (laserWallPhase == (int)LaserWallPhase.End)
				{
					// End laser wall phase after 4.25 seconds
					NPC.alpha -= 1;
					if (NPC.alpha <= 0)
					{
						NPC.alpha = 0;
						laserWallPhase = (int)LaserWallPhase.SetUp;
					}
				}
            }
            else
            {
				// Set alpha after teleport
				if (postTeleportTimer > 0)
				{
					postTeleportTimer -= 1;
					if (postTeleportTimer < 0)
						postTeleportTimer = 0;

					NPC.alpha = postTeleportTimer;
				}
				else
				{
					NPC.alpha -= 6;
					if (NPC.alpha < 0)
						NPC.alpha = 0;
				}

				// Reset laser wall phase
                if (laserWallPhase > (int)LaserWallPhase.SetUp)
                    laserWallPhase = (int)LaserWallPhase.SetUp;

				// Enter final phase
				if (!halfLife && phase3)
				{
					SpawnTeleportLocation(player);

					// Anger message
					string key = "A GOD DOES NOT FEAR DEATH!";
					Color messageColor = Color.Cyan;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);

                    // Summon Thots
                    if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/DevourerAttack"), player.position);

						for (int i = 0; i < 3; i++)
							NPC.SpawnOnPlayer(NPC.FindClosestPlayer(), ModContent.NPCType<DevourerofGodsHead2>());
					}

					halfLife = true;
				}
			}

			// Spawn segments and fire projectiles
			if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Segments
                if (!tail && NPC.ai[0] == 0f)
                {
                    int Previous = NPC.whoAmI;
                    for (int segmentSpawn = 0; segmentSpawn < maxLength; segmentSpawn++)
                    {
                        int segment;
                        if (segmentSpawn >= 0 && segmentSpawn < minLength)
                            segment = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<DevourerofGodsBodyS>(), NPC.whoAmI);
                        else
                            segment = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<DevourerofGodsTailS>(), NPC.whoAmI);

                        Main.npc[segment].realLife = NPC.whoAmI;
                        Main.npc[segment].ai[2] = NPC.whoAmI;
                        Main.npc[segment].ai[1] = Previous;
                        Main.npc[Previous].ai[0] = segment;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, segment, 0f, 0f, 0f, 0);
                        Previous = segment;
                    }
                    tail = true;
                }

                // Fireballs
                if (NPC.alpha <= 0 && distanceFromTarget > 500f)
                {
                    calamityGlobalNPC.newAI[0] += 1f;
					if (calamityGlobalNPC.newAI[0] >= 150f && calamityGlobalNPC.newAI[0] % (breathFireMore ? 60f : 120f) == 0f)
					{
						Vector2 vector44 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						float num427 = player.position.X + (player.width / 2) - vector44.X;
						float num428 = player.position.Y + (player.height / 2) - vector44.Y;
						float num430 = 16f;
						float num429 = (float)Math.Sqrt(num427 * num427 + num428 * num428);
						num429 = num430 / num429;
						num427 *= num429;
						num428 *= num429;
						num428 += NPC.velocity.Y * 0.5f;
						num427 += NPC.velocity.X * 0.5f;
						vector44.X -= num427 * 1f;
						vector44.Y -= num428 * 1f;

						int type = ModContent.ProjectileType<DoGFire>();
						int damage = NPC.GetProjectileDamage(type);
						Projectile.NewProjectile(NPC.GetSource_FromThis(), vector44.X, vector44.Y, num427, num428, type, damage, 0f, Main.myPlayer);
					}
                }
                else if (distanceFromTarget < 250f)
                    calamityGlobalNPC.newAI[0] = 0f;

                // Laser walls
                if (!phase3 && (laserWallPhase == (int)LaserWallPhase.FireLaserWalls || calamityGlobalNPC.enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive)))
                {
                    float speed = 12f;
                    float spawnOffset = 1500f;
                    float divisor = 120f;

					if (calamityGlobalNPC.newAI[1] % divisor == 0f)
					{
						SoundEngine.PlaySound(SoundID.Item12, player.position);

						// Side walls
						float targetPosY = player.position.Y;
						int type = ModContent.ProjectileType<DoGDeath>();
						int damage = NPC.GetProjectileDamage(type);
						int halfTotalDiagonalShots = totalDiagonalShots / 2;
						Vector2 start = default;
						Vector2 velocity = default;
						Vector2 aim = expertMode ? player.Center + player.velocity * 20f : Vector2.Zero;

						switch (laserWallType)
						{
							case (int)LaserWallType.Normal:

								for (int x = 0; x < totalShots; x++)
								{
									Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + spawnOffset, targetPosY + shotSpacing[0], -speed, 0f, type, damage, 0f, Main.myPlayer);
									Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X - spawnOffset, targetPosY + shotSpacing[0], speed, 0f, type, damage, 0f, Main.myPlayer);
									shotSpacing[0] -= spacingVar;
								}

								laserWallType = (int)LaserWallType.Offset;
								break;

							case (int)LaserWallType.Offset:

								targetPosY += 50f;
								for (int x = 0; x < totalShots; x++)
								{
									Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + spawnOffset, targetPosY + shotSpacing[0], -speed, 0f, type, damage, 0f, Main.myPlayer);
									Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X - spawnOffset, targetPosY + shotSpacing[0], speed, 0f, type, damage, 0f, Main.myPlayer);
									shotSpacing[0] -= spacingVar;
								}

								laserWallType = revenge ? (int)LaserWallType.MultiLayered : expertMode ? (int)LaserWallType.DiagonalHorizontal : (int)LaserWallType.Normal;
								break;

							case (int)LaserWallType.MultiLayered:

								for (int x = 0; x < totalShots; x++)
								{
									Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + spawnOffset, targetPosY + shotSpacing[0], -speed, 0f, type, damage, 0f, Main.myPlayer);
									Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X - spawnOffset, targetPosY + shotSpacing[0], speed, 0f, type, damage, 0f, Main.myPlayer);
									shotSpacing[0] -= spacingVar;
								}

								int totalBonusLasers = totalShots / 2;
								for (int x = 0; x < totalBonusLasers; x++)
								{
									Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + spawnOffset, targetPosY + shotSpacing[3], -speed, 0f, type, damage, 0f, Main.myPlayer);
									Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X - spawnOffset, targetPosY + shotSpacing[3], speed, 0f, type, damage, 0f, Main.myPlayer);
									shotSpacing[3] -= Main.rand.NextBool(2) ? 180 : 200;
								}

								laserWallType = (int)LaserWallType.DiagonalHorizontal;
								break;

							case (int)LaserWallType.DiagonalHorizontal:

								for (int x = 0; x < totalDiagonalShots + 1; x++)
								{
									start = new Vector2(player.position.X + spawnOffset, targetPosY + shotSpacing[0]);
									aim.Y += laserWallSpacingOffset * (x - halfTotalDiagonalShots);
									velocity = Vector2.Normalize(aim - start) * speed;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer);

									start = new Vector2(player.position.X - spawnOffset, targetPosY + shotSpacing[0]);
									velocity = Vector2.Normalize(aim - start) * speed;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer);

									shotSpacing[0] -= diagonalSpacingVar;
								}

								laserWallType = revenge ? (int)LaserWallType.DiagonalVertical : (int)LaserWallType.Normal;
								break;

							case (int)LaserWallType.DiagonalVertical:

								for (int x = 0; x < totalDiagonalShots + 1; x++)
								{
									start = new Vector2(player.position.X + shotSpacing[0], targetPosY + spawnOffset);
									aim.X += laserWallSpacingOffset * (x - halfTotalDiagonalShots);
									velocity = Vector2.Normalize(aim - start) * speed;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer);

									start = new Vector2(player.position.X + shotSpacing[0], targetPosY - spawnOffset);
									velocity = Vector2.Normalize(aim - start) * speed;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer);

									shotSpacing[0] -= diagonalSpacingVar;
								}

								laserWallType = (int)LaserWallType.Normal;
								break;
						}

						// Lower wall
						for (int x = 0; x < totalShots; x++)
						{
							Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + shotSpacing[1], player.position.Y + spawnOffset, 0f, -speed, type, damage, 0f, Main.myPlayer);
							shotSpacing[1] -= spacingVar;
						}

						// Upper wall
						for (int x = 0; x < totalShots; x++)
						{
							Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + shotSpacing[2], player.position.Y - spawnOffset, 0f, speed, type, damage, 0f, Main.myPlayer);
							shotSpacing[2] -= spacingVar;
						}

						for (int i = 0; i < shotSpacing.Length; i++)
							shotSpacing[i] = shotSpacingMax;
					}

					calamityGlobalNPC.newAI[1] += 1f;
				}
            }

            // Despawn
            if (!NPC.AnyNPCs(ModContent.NPCType<DevourerofGodsTailS>()))
                NPC.active = false;

            float fallSpeed = death ? 17.75f : 16f;

			if (expertMode)
				fallSpeed += 3.5f * (1f - lifeRatio);

			if (player.dead)
            {
				NPC.TargetClosest(false);
				flies = true;
                NPC.velocity.Y -= 3f;
                if ((double)NPC.position.Y < Main.topWorld + 16f)
                {
                    NPC.velocity.Y -= 3f;
                    fallSpeed = 32f;
                }
                if ((double)NPC.position.Y < Main.topWorld + 16f)
                {
                    for (int a = 0; a < Main.maxNPCs; a++)
                    {
                        if (Main.npc[a].type == ModContent.NPCType<DevourerofGodsHeadS>() || Main.npc[a].type == ModContent.NPCType<DevourerofGodsBodyS>() || Main.npc[a].type == ModContent.NPCType<DevourerofGodsTailS>())
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

			int phaseLimit = death ? 600 : 900;

			// Flight
			if (NPC.ai[2] == 0f)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active && Vector2.Distance(Main.player[Main.myPlayer].Center, vector) < CalamityGlobalNPC.CatchUpDistance350Tiles)
                        Main.player[Main.myPlayer].AddBuff(ModContent.BuffType<Warped>(), 2);
                }

				// Charge in a direction for a second until the timer is back at 0
				if (postTeleportTimer > 0)
				{
					NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + MathHelper.PiOver2;
					return;
				}

				calamityGlobalNPC.newAI[2] += 1f;

                NPC.localAI[1] = 0f;

				float speed = death ? 16.5f : 15f;
                float turnSpeed = death ? 0.33f : 0.3f;
                float homingSpeed = death ? 30f : 24f;
                float homingTurnSpeed = death ? 0.405f : 0.33f;

				if (expertMode)
				{
					phaseLimit /= 1 + (int)(5f * (1f - lifeRatio));

					if (phaseLimit < 180)
						phaseLimit = 180;

					speed += 3f * (1f - lifeRatio);
					turnSpeed += 0.06f * (1f - lifeRatio);
					homingSpeed += 12f * (1f - lifeRatio);
					homingTurnSpeed += 0.15f * (1f - lifeRatio);
				}

				// Go to ground phase sooner
				if (increaseSpeedMore)
				{
					if (laserWallPhase == (int)LaserWallPhase.SetUp && calamityGlobalNPC.newAI[3] <= alphaGateValue)
						SpawnTeleportLocation(player);
					else
						calamityGlobalNPC.newAI[2] += 10f;
				}
				else
					calamityGlobalNPC.newAI[2] += 2f;

				float num188 = speed;
                float num189 = turnSpeed;
                Vector2 vector18 = NPC.Center;
                float num191 = player.Center.X;
                float num192 = player.Center.Y;
                int num42 = -1;
                int num43 = (int)(player.Center.X / 16f);
                int num44 = (int)(player.Center.Y / 16f);

                // Charge at target for 1.5 seconds
                bool flyAtTarget = (!phase2 || phase3) && calamityGlobalNPC.newAI[2] > phaseLimit - 90 && revenge;

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

                if (!flyAtTarget)
                {
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

                if (calamityGlobalNPC.newAI[2] > phaseLimit)
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

				// Charge in a direction for a second until the timer is back at 0
				if (postTeleportTimer > 0)
				{
					NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + MathHelper.PiOver2;
					return;
				}

				calamityGlobalNPC.newAI[2] += 1f;

                float turnSpeed = death ? 0.24f : 0.18f;

				if (expertMode)
				{
					turnSpeed += 0.1f * (1f - lifeRatio);
					turnSpeed += Vector2.Distance(player.Center, NPC.Center) * 0.00005f * (1f - lifeRatio);
				}

				// Enrage
				if (increaseSpeedMore)
				{
					if (laserWallPhase == (int)LaserWallPhase.SetUp && calamityGlobalNPC.newAI[3] <= alphaGateValue)
						SpawnTeleportLocation(player);
					else
					{
						fallSpeed *= 3f;
						turnSpeed *= 6f;
					}
				}
				else if (increaseSpeed)
				{
					fallSpeed *= 1.5f;
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

                    int num954 = death ? 1125 : 1200;
                    if (lifeRatio < 0.8f && lifeRatio > 0.2f && !death)
                        num954 = 1400;

					if (expertMode)
						num954 -= (int)(150f * (1f - lifeRatio));

					if (num954 < 1050)
						num954 = 1050;

                    bool flag95 = true;
                    if (NPC.position.Y > player.position.Y)
                    {
                        for (int num955 = 0; num955 < Main.maxPlayers; num955++)
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

                float num189 = turnSpeed;
                Vector2 vector18 = NPC.Center;
                float num191 = player.Center.X;
                float num192 = player.Center.Y;
                num191 = (int)(num191 / 16f) * 16;
                num192 = (int)(num192 / 16f) * 16;
                vector18.X = (int)(vector18.X / 16f) * 16;
                vector18.Y = (int)(vector18.Y / 16f) * 16;
                num191 -= vector18.X;
                num192 -= vector18.Y;
                float num193 = (float)Math.Sqrt(num191 * num191 + num192 * num192);

                if (!flies)
                {
                    NPC.TargetClosest(true);

                    NPC.velocity.Y += turnSpeed;
                    if (NPC.velocity.Y > fallSpeed)
                        NPC.velocity.Y = fallSpeed;

                    if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < fallSpeed * 2.2)
                    {
                        if (NPC.velocity.X < 0f)
                            NPC.velocity.X -= num189 * 1.1f;
                        else
                            NPC.velocity.X += num189 * 1.1f;
                    }
                    else if (NPC.velocity.Y == fallSpeed)
                    {
                        if (NPC.velocity.X < num191)
                            NPC.velocity.X += num189;
                        else if (NPC.velocity.X > num191)
                            NPC.velocity.X -= num189;
                    }
                    else if (NPC.velocity.Y > 4f)
                    {
                        if (NPC.velocity.X < 0f)
                            NPC.velocity.X += num189 * 0.9f;
                        else
                            NPC.velocity.X -= num189 * 0.9f;
                    }
                }
                else
                {
                    double maximumSpeed1 = death ? 0.46 : 0.4;
                    double maximumSpeed2 = death ? 1.125 : 1D;

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
						maximumSpeed1 += 0.1f * (1f - lifeRatio);
						maximumSpeed2 += 0.2f * (1f - lifeRatio);
					}

                    num193 = (float)Math.Sqrt(num191 * num191 + num192 * num192);
                    float num25 = Math.Abs(num191);
                    float num26 = Math.Abs(num192);
                    float num27 = fallSpeed / num193;
                    num191 *= num27;
                    num192 *= num27;

                    if (((NPC.velocity.X > 0f && num191 > 0f) || (NPC.velocity.X < 0f && num191 < 0f)) && ((NPC.velocity.Y > 0f && num192 > 0f) || (NPC.velocity.Y < 0f && num192 < 0f)))
                    {
                        if (NPC.velocity.X < num191)
                            NPC.velocity.X += turnSpeed * 1.5f;
                        else if (NPC.velocity.X > num191)
                            NPC.velocity.X -= turnSpeed * 1.5f;

                        if (NPC.velocity.Y < num192)
                            NPC.velocity.Y += turnSpeed * 1.5f;
                        else if (NPC.velocity.Y > num192)
                            NPC.velocity.Y -= turnSpeed * 1.5f;
                    }

                    if ((NPC.velocity.X > 0f && num191 > 0f) || (NPC.velocity.X < 0f && num191 < 0f) || (NPC.velocity.Y > 0f && num192 > 0f) || (NPC.velocity.Y < 0f && num192 < 0f))
                    {
                        if (NPC.velocity.X < num191)
                            NPC.velocity.X += turnSpeed;
                        else if (NPC.velocity.X > num191)
                            NPC.velocity.X -= turnSpeed;

                        if (NPC.velocity.Y < num192)
                            NPC.velocity.Y += turnSpeed;
                        else if (NPC.velocity.Y > num192)
                            NPC.velocity.Y -= turnSpeed;

                        if (Math.Abs(num192) < fallSpeed * maximumSpeed1 && ((NPC.velocity.X > 0f && num191 < 0f) || (NPC.velocity.X < 0f && num191 > 0f)))
                        {
                            if (NPC.velocity.Y > 0f)
                                NPC.velocity.Y += turnSpeed * 2f;
                            else
                                NPC.velocity.Y -= turnSpeed * 2f;
                        }

                        if (Math.Abs(num191) < fallSpeed * maximumSpeed1 && ((NPC.velocity.Y > 0f && num192 < 0f) || (NPC.velocity.Y < 0f && num192 > 0f)))
                        {
                            if (NPC.velocity.X > 0f)
                                NPC.velocity.X += turnSpeed * 2f;
                            else
                                NPC.velocity.X -= turnSpeed * 2f;
                        }
                    }
                    else if (num25 > num26)
                    {
                        if (NPC.velocity.X < num191)
                            NPC.velocity.X += turnSpeed * 1.1f;
                        else if (NPC.velocity.X > num191)
                            NPC.velocity.X -= turnSpeed * 1.1f;

                        if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < fallSpeed * maximumSpeed2)
                        {
                            if (NPC.velocity.Y > 0f)
                                NPC.velocity.Y += turnSpeed;
                            else
                                NPC.velocity.Y -= turnSpeed;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y < num192)
                            NPC.velocity.Y += turnSpeed * 1.1f;
                        else if (NPC.velocity.Y > num192)
                            NPC.velocity.Y -= turnSpeed * 1.1f;

                        if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < fallSpeed * maximumSpeed2)
                        {
                            if (NPC.velocity.X > 0f)
                                NPC.velocity.X += turnSpeed;
                            else
                                NPC.velocity.X -= turnSpeed;
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

                if (calamityGlobalNPC.newAI[2] > phaseLimit)
                {
                    NPC.ai[2] = 0f;
					calamityGlobalNPC.newAI[2] = 0f;
                    NPC.netUpdate = true;
                }
            }
        }

		private void SpawnTeleportLocation(Player player)
		{
			if (teleportTimer > -1 || player.dead || !player.active)
				return;

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int randomRange = 48;
				float distance = 640f;
				Vector2 targetVector = player.Center + player.velocity.SafeNormalize(Vector2.UnitX) * distance + new Vector2(Main.rand.Next(-randomRange, randomRange + 1), Main.rand.Next(-randomRange, randomRange + 1));
				SoundEngine.PlaySound(SoundID.Item109, player.Center);
				Projectile.NewProjectile(NPC.GetSource_FromThis(), targetVector, Vector2.Zero, ModContent.ProjectileType<DoGTeleportRift>(), 0, 0f, Main.myPlayer, NPC.whoAmI);
			}

			teleportTimer = 180;
		}

		private void Teleport(Player player, bool death, bool revenge, bool expertMode)
		{
			Vector2 newPosition = GetRiftLocation(true);

			if (player.dead || !player.active || newPosition == default)
				return;

			NPC.position = newPosition;
			float chargeVelocity = BossRushEvent.BossRushActive ? 30f : death ? 24f : revenge ? 22f : expertMode ? 20f : 18f;
			float maxChargeDistance = 1200f;
			postTeleportTimer = (int)Math.Round(maxChargeDistance / chargeVelocity);
			NPC.alpha = postTeleportTimer;
			NPC.velocity = Vector2.Normalize(player.Center - NPC.Center) * chargeVelocity;
			NPC.netUpdate = true;

			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active && (Main.npc[i].type == ModContent.NPCType<DevourerofGodsBodyS>() || Main.npc[i].type == ModContent.NPCType<DevourerofGodsTailS>()))
				{
					Main.npc[i].position = newPosition;
                    if (Main.npc[i].type == ModContent.NPCType<DevourerofGodsTailS>())
                    {
                        ((DevourerofGodsTailS)Main.npc[i].ModNPC).setInvulTime(720);
                    }
					Main.npc[i].netUpdate = true;
				}
			}

			SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/DevourerAttack"), player.Center);
		}

		private Vector2 GetRiftLocation(bool spawnDust)
		{
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i].type == ModContent.ProjectileType<DoGTeleportRift>())
				{
					if (!spawnDust)
						Main.projectile[i].ai[0] = -1f;

					Main.projectile[i].Kill();
					return Main.projectile[i].Center;
				}
			}
			return default;
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

			if (!NPC.dontTakeDamage)
			{
				texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/DevourerofGods/DevourerofGodsHeadSGlow").Value;
				Color color37 = Color.Lerp(Color.White, Color.Fuchsia, 0.5f);

				spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

				texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/DevourerofGods/DevourerofGodsHeadSGlow2").Value;
				color37 = Color.Lerp(Color.White, Color.Cyan, 0.5f);

				spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
			}

			return false;
		}

		public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ModContent.ItemType<CosmiliteBrick>();
        }

        public override bool SpecialOnKill()
        {
            int closestSegmentID = DropHelper.FindClosestWormSegment(NPC.GetSource_FromThis(), NPC,
                ModContent.NPCType<DevourerofGodsHeadS>(),
                ModContent.NPCType<DevourerofGodsBodyS>(),
                ModContent.NPCType<DevourerofGodsTailS>());
            NPC.position = Main.npc[closestSegmentID].position;
            return false;
        }

        public override void OnKill()
        {
            // Stop the countdown -- if you kill DoG in less than 60 frames, this will stop another one from spawning.
            CalamityWorld.DoGSecondStageCountdown = 0;

            DropHelper.DropBags(ModContent.ItemType<DevourerofGodsBag>(), NPC);

            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<SupremeHealingPotion>(), 5, 15);
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<DevourerofGodsTrophy>(), 10);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeDevourerofGods>(), true, !CalamityWorld.downedDoG);
            DropHelper.DropResidentEvilAmmo(NPC.GetSource_FromThis(), NPC, CalamityWorld.downedDoG, 6, 3, 2);

			CalamityGlobalTownNPC.SetNewShopVariable(new int[] { ModContent.NPCType<THIEF>() }, CalamityWorld.downedDoG);

			// All other drops are contained in the bag, so they only drop directly on Normal
			if (!Main.expertMode)
            {
                // Materials
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CosmiliteBar>(), 25, 35);
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CosmiliteBrick>(), 150, 250);

                // Weapons
                float w = DropHelper.DirectWeaponDropRateFloat;
                DropHelper.DropEntireWeightedSet(NPC.GetSource_FromThis(), NPC,
                    DropHelper.WeightStack<Excelsus>(w),
                    DropHelper.WeightStack<TheObliterator>(w),
                    DropHelper.WeightStack<Deathwind>(w),
                    DropHelper.WeightStack<DeathhailStaff>(w),
                    DropHelper.WeightStack<StaffoftheMechworm>(w),
                    Main.rand.NextBool() ? DropHelper.WeightStack<EradicatorMelee>(w) : DropHelper.WeightStack<Eradicator>(w)
                );

                // Vanity
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<DevourerofGodsMask>(), 7);
            }

            // If DoG has not been killed yet, notify players that the holiday moons are buffed
            if (!CalamityWorld.downedDoG)
            {
                string key = "The frigid moon shimmers brightly.";
                Color messageColor = Color.Cyan;
                string key2 = "The harvest moon glows eerily.";
                Color messageColor2 = Color.Orange;

                CalamityUtils.DisplayLocalizedText(key, messageColor);
                CalamityUtils.DisplayLocalizedText(key2, messageColor2);
            }

            // Mark DoG as dead
            CalamityWorld.downedDoG = true;
			CalamityNetcode.SyncWorld();
		}

		// Can only hit the target if within certain distance
		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;

            Rectangle targetHitbox = target.Hitbox;

            float dist1 = Vector2.Distance(NPC.Center, targetHitbox.TopLeft());
            float dist2 = Vector2.Distance(NPC.Center, targetHitbox.TopRight());
            float dist3 = Vector2.Distance(NPC.Center, targetHitbox.BottomLeft());
            float dist4 = Vector2.Distance(NPC.Center, targetHitbox.BottomRight());

            float minDist = dist1;
            if (dist2 < minDist)
                minDist = dist2;
            if (dist3 < minDist)
                minDist = dist3;
            if (dist4 < minDist)
                minDist = dist4;

            return minDist <= 80f && (NPC.alpha <= 0 || postTeleportTimer > 0);
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
	        if (modifiers.FinalDamage.Base >= NPC.lifeMax * 0.5f)
            {
                string key = "You think...you can butcher...ME!?";
                Color messageColor = Color.Cyan;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
                modifiers.SetMaxDamage(0);
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2f;
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
	            {
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DoGS").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DoGS2").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DoGS5").Type, 1f);
                }
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
            if ((CalamityWorld.death || BossRushEvent.BossRushActive) && (NPC.alpha <= 0 || postTeleportTimer > 0) && !target.Calamity().lol)
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
				Rectangle location = new Microsoft.Xna.Framework.Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
				CombatText.NewText(location, messageColor, Language.GetTextValue(text), true);
				target.Calamity().dogTextCooldown = 60;
			}
        }
    }
}
