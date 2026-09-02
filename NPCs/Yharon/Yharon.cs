using CalRD.Buffs.DamageOverTime;
using CalRD.Events;
using CalRD.Items.Accessories.Wings;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Pets;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.Potions;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.TownNPCs;
using CalRD.Projectiles.Boss;
using CalRD.Tiles.Ores;
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
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.NPCs.Yharon
{
    [AutoloadBossHead]
    public class Yharon : ModNPC
    {
        private Rectangle safeBox = default;
		private Vector2 flareDustBulletHellSpawn = default;

		private bool enraged = false;
        private bool protectionBoost = false;
        private bool moveCloser = false;
        private bool phaseOneLoot = true;
        private bool dropLoot = false;
        private bool useTornado = true;
        private int healCounter = 0;
        private int secondPhasePhase = 1;
        private int teleportLocation = 0;
        private bool startSecondAI = false;
        private bool spawnArena = false;
        private int invincibilityCounter = 0;

        public static float Phase1_DR = 0.24f;
        public static float Phase2_DR = 0.26f;
		public static float ChargeTelegraph_DR = 0.4f;
        public static float EnragedDR = 0.9f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Jungle Dragon, Yharon");
            Main.npcFrameCount[NPC.type] = 7;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Scale = 0.3f,
				PortraitScale = 0.4f,
				PortraitPositionYOverride = -16f,
				SpriteDirection = 1
			};
			value.Position.X += 26f;
			value.Position.Y -= 14f;
			NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
		}
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
	        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
	        {
		        BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
		        new FlavorTextBestiaryInfoElement("The tyrant's loyal lifelong friend has set his sight on you.")
	        });
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 50f;
			NPC.GetNPCDamage();
			NPC.width = 200;
            NPC.height = 200;
            NPC.defense = 150;
            NPC.LifeMaxNERB(2275000, 2525000, 3700000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.value = Item.buyPrice(1, 50, 0, 0);
            NPC.boss = true;

            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;

			NPC.DR_NERD(Phase1_DR, null, null, null, true);
			CalamityGlobalNPC global = NPC.Calamity();
            global.flatDRReductions.Add(BuffID.CursedInferno, 0.05f);

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/YHARONREBIRTH");
            if (CalamityWorld.buffedEclipse || BossRushEvent.BossRushActive)
                Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/YHARON");
            
            NPC.HitSound = SoundID.NPCHit56;
            NPC.DeathSound = SoundID.NPCDeath60;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            BitsByte bb = new BitsByte();
            bb[0] = enraged;
            bb[1] = protectionBoost;
            bb[2] = moveCloser;
            bb[3] = phaseOneLoot;
            bb[4] = dropLoot;
            bb[5] = useTornado;
            bb[6] = startSecondAI;
            bb[7] = NPC.dontTakeDamage;
            writer.Write(bb);
            writer.Write(healCounter);
            writer.Write(secondPhasePhase);
            writer.Write(teleportLocation);
            writer.Write(invincibilityCounter);
			writer.WriteVector2(flareDustBulletHellSpawn);
            writer.Write(safeBox.X);
            writer.Write(safeBox.Y);
            writer.Write(safeBox.Width);
            writer.Write(safeBox.Height);
			writer.Write(NPC.localAI[0]);
			writer.Write(NPC.localAI[1]);
			writer.Write(NPC.localAI[2]);
		}

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            BitsByte bb = reader.ReadByte();
            enraged = bb[0];
            protectionBoost = bb[1];
            moveCloser = bb[2];
            phaseOneLoot = bb[3];
            dropLoot = bb[4];
            useTornado = bb[5];
            startSecondAI = bb[6];
            NPC.dontTakeDamage = bb[7];
            healCounter = reader.ReadInt32();
            secondPhasePhase = reader.ReadInt32();
            teleportLocation = reader.ReadInt32();
            invincibilityCounter = reader.ReadInt32();
			flareDustBulletHellSpawn = reader.ReadVector2();
			safeBox.X = reader.ReadInt32();
            safeBox.Y = reader.ReadInt32();
            safeBox.Width = reader.ReadInt32();
            safeBox.Height = reader.ReadInt32();
			NPC.localAI[0] = reader.ReadSingle();
			NPC.localAI[1] = reader.ReadSingle();
			NPC.localAI[2] = reader.ReadSingle();
		}

        public override void AI()
        {
            // Disable loot drop
            dropLoot = NPC.life <= NPC.lifeMax * 0.1;

            // Stop rain
            CalRD.StopRain();

			// Variables
			bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
			float pie = (float)Math.PI;

			// Start phase 2 or not
			if (startSecondAI)
            {
                // Despawn and drop phase 1 loot
                if (!CalamityWorld.buffedEclipse && !BossRushEvent.BossRushActive)
                {
                    NPC.DeathSound = null;
                    NPC.dontTakeDamage = true;

                    NPC.velocity.Y -= 0.4f;

                    if (NPC.alpha < 255)
                    {
                        NPC.alpha += 5;
                        if (NPC.alpha > 255)
                            NPC.alpha = 255;
                    }

                    if (NPC.timeLeft > 55)
                        NPC.timeLeft = 55;

                    if (NPC.timeLeft < 5)
                    {
                        startSecondAI = false;

                        NPC.boss = false;
                        NPC.life = 0;

                        if (dropLoot)
                            NPC.NPCLoot();

                        NPC.active = false;
                        NPC.netUpdate = true;
                    }

                    return;
                }

				if (phaseOneLoot && !BossRushEvent.BossRushActive)
				{
					NPC.boss = false;

					if (dropLoot)
						NPC.NPCLoot();

					NPC.boss = true;

					NPC.Calamity().AITimer = 0;
				}

                // Don't drop phase 1 loot
                phaseOneLoot = false;

                // Start second AI
                Yharon_AI2(expertMode, revenge, death, pie);

                return;
            }

			// Timed DR adjustment
			if (CalamityWorld.buffedEclipse)
			{
				if (NPC.Calamity().AITimer < 3600)
					NPC.Calamity().AITimer = 3600;
			}

			// Phase bools
            bool phase2Check = death || NPC.life <= NPC.lifeMax * (revenge ? 0.8 : (expertMode ? 0.7 : 0.5));
            bool phase3Check = NPC.life <= NPC.lifeMax * (death ? 0.6 : (revenge ? 0.5 : (expertMode ? 0.4 : 0.25)));
            bool phase4Check = NPC.life <= NPC.lifeMax * 0.1;
			bool phase1Change = NPC.ai[0] > -1f;
            bool phase2Change = NPC.ai[0] > 5f;
            bool phase3Change = NPC.ai[0] > 12f;
            bool isCharging = NPC.ai[3] < 20f;

            // Flare limit
            int maxFlareCount = 3;

            // Timer, velocity and acceleration for idle phase before phase switch
            int phaseSwitchTimer = expertMode ? 36 : 40;
            float acceleration = expertMode ? 0.75f : 0.7f;
            float velocity = expertMode ? 12f : 11f;

			// Damage immunity
			if (phase3Change)
				NPC.dontTakeDamage = phase4Check;
			else if (phase2Change)
				NPC.dontTakeDamage = phase3Check;
			else if (phase1Change)
				NPC.dontTakeDamage = phase2Check;

			if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
            {
				acceleration = 0.95f;
				velocity = 15f;
				phaseSwitchTimer = 25;
            }
            else if (phase3Change)
            {
				acceleration = expertMode ? 0.85f : 0.8f;
				velocity = expertMode ? 14f : 13f;
				phaseSwitchTimer = expertMode ? 25 : 28;
            }
            else if (phase2Change && isCharging)
            {
				acceleration = expertMode ? 0.8f : 0.75f;
				velocity = expertMode ? 13f : 12f;
				phaseSwitchTimer = expertMode ? 32 : 36;
            }
            else if (isCharging && !phase2Change && !phase3Change)
            {
				phaseSwitchTimer = 25;
            }

			// Timers and velocity for charging
            int chargeTime = expertMode ? 40 : 45;
            float chargeSpeed = expertMode ? 28f : 26f;
			float fastChargeVelocityMultiplier = 1.5f;
			int fastChargeTelegraphTimer = 120;

            if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
            {
                chargeTime = 30;
                chargeSpeed = 40f;
            }
            else if (phase3Change)
            {
                chargeTime = 35;
                chargeSpeed = 30f;
            }
            else if (isCharging && phase2Change)
            {
                chargeTime = expertMode ? 38 : 43;

                if (expertMode)
                    chargeSpeed = 28.5f;
            }

			if (revenge)
			{
				int chargeTimeDecrease = death ? 4 : 2;
				float velocityMult = death ? 1.1f : 1.05f;
				phaseSwitchTimer -= chargeTimeDecrease;
				acceleration *= velocityMult;
				velocity *= velocityMult;
				chargeTime -= chargeTimeDecrease;
				chargeSpeed *= velocityMult;
			}

            int flareBombPhaseTimer = death ? 40 : 60;
            int flareBombSpawnDivisor = flareBombPhaseTimer / 20;
            float flareBombPhaseAcceleration = death ? 0.92f : 0.8f;
            float flareBombPhaseVelocity = death ? 14f : 12f;

            int fireTornadoPhaseTimer = 90;

            int newPhaseTimer = 180;

            int flareDustPhaseTimer = death ? 250 : 300;
			int flareDustPhaseTimer2 = death ? 120 : 150;

			float spinTime = flareDustPhaseTimer / 2;

			int flareDustSpawnDivisor = flareDustPhaseTimer / 10;
			int flareDustSpawnDivisor2 = flareDustPhaseTimer2 / 30;
			int flareDustSpawnDivisor3 = flareDustPhaseTimer / 25;

			float spinPhaseVelocity = 25f;
            float spinPhaseRotation = MathHelper.TwoPi * 3 / spinTime;

			float increasedIdleTimeAfterBulletHell = -120f;

			float teleportPhaseTimer = 30f;

			int spawnPhaseTimer = 75;

			Vector2 vectorCenter = NPC.Center;

			// Target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
            }

			Player player = Main.player[NPC.target];

			// Despawn
			if (player.dead || !player.active)
            {
				NPC.TargetClosest(true);
				player = Main.player[NPC.target];
				if (player.dead || !player.active)
				{
					NPC.velocity.Y -= 0.4f;

					if (NPC.timeLeft > 60)
						NPC.timeLeft = 60;

					if (NPC.ai[0] > 12f)
						NPC.ai[0] = 13f;
					else if (NPC.ai[0] > 5f)
						NPC.ai[0] = 6f;
					else
						NPC.ai[0] = 0f;

					NPC.ai[2] = 0f;
				}
            }
            else if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

			int xPos = 60 * NPC.direction;
			Vector2 vector = Vector2.Normalize(player.Center - vectorCenter) * (NPC.width + 20) / 2f + vectorCenter;
			Vector2 fromMouth = new Vector2((int)vector.X + xPos, (int)vector.Y - 15);

			// Create the arena, but not as a multiplayer client.
			// In single player, the arena gets created and never gets synced because it's single player.
			// In multiplayer, only the server/host creates the arena, and everyone else receives it on the next frame via SendExtraAI.
			// Everyone however sets spawnArena to true to confirm that the fight has started.
			if (!spawnArena)
            {
                spawnArena = true;
                enraged = false;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    safeBox.X = (int)(player.Center.X - (revenge ? 3000f : 3500f));
                    safeBox.Y = (int)(player.Center.Y - (revenge ? 9000f : 10500f));
                    safeBox.Width = revenge ? 6000 : 7000;
                    safeBox.Height = revenge ? 18000 : 21000;
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), player.Center.X + (revenge ? 3000f : 3500f), player.Center.Y + 100f, 0f, 0f, ModContent.ProjectileType<SkyFlareRevenge>(), 0, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), player.Center.X - (revenge ? 3000f : 3500f), player.Center.Y + 100f, 0f, 0f, ModContent.ProjectileType<SkyFlareRevenge>(), 0, 0f, Main.myPlayer, 0f, 0f);
                }

                // Force Yharon to send a sync packet so that the arena gets sent immediately
                NPC.netUpdate = true;
            }
            // Enrage code doesn't run on frame 1 so that Yharon won't be enraged for 1 frame in multiplayer
            else
            {
                enraged = !player.Hitbox.Intersects(safeBox);
                if (enraged)
                {
					phaseSwitchTimer = 15;
                    protectionBoost = true;
                    NPC.damage = NPC.defDamage * 5;
                    chargeSpeed += 25f;
                }
                else
                {
                    NPC.damage = NPC.defDamage;
                    protectionBoost = false;
                }
            }

			// Set DR based on protection boost (aka enrage)
			bool chargeTelegraph = (NPC.ai[0] == 0f || NPC.ai[0] == 6f || NPC.ai[0] == 13f) && NPC.localAI[1] > 0f;
			bool bulletHell = NPC.ai[0] == 8f || NPC.ai[0] == 15f;
			NPC.Calamity().DR = protectionBoost ? EnragedDR : ((chargeTelegraph || bulletHell) ? ChargeTelegraph_DR : Phase1_DR);

			if (bulletHell)
				NPC.damage = 0;

            // Trigger spawn effects
            if (NPC.localAI[0] == 0f)
            {
                NPC.localAI[0] = 1f;
                NPC.alpha = 255;
                NPC.rotation = 0f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[0] = -1f;
                    NPC.netUpdate = true;
                }
            }

            // Rotation
            float npcRotation = (float)Math.Atan2(player.Center.Y - vectorCenter.Y, player.Center.X - vectorCenter.X);
            if (NPC.spriteDirection == 1)
                npcRotation += pie;
            if (npcRotation < 0f)
                npcRotation += MathHelper.TwoPi;
            if (npcRotation > MathHelper.TwoPi)
                npcRotation -= MathHelper.TwoPi;
            if (NPC.ai[0] == -1f || NPC.ai[0] == 3f || NPC.ai[0] == 4f || NPC.ai[0] == 9f || NPC.ai[0] == 10f || NPC.ai[0] == 16f)
                npcRotation = 0f;

            float npcRotationSpeed = 0.04f;
            if (NPC.ai[0] == 1f || NPC.ai[0] == 5f || NPC.ai[0] == 7f || NPC.ai[0] == 8f || NPC.ai[0] == 11f || NPC.ai[0] == 12f ||
				NPC.ai[0] == 14f || NPC.ai[0] == 15f || NPC.ai[0] == 18f || NPC.ai[0] == 19f)
                npcRotationSpeed = 0f;
            if (NPC.ai[0] == 3f || NPC.ai[0] == 4f || NPC.ai[0] == 9f || NPC.ai[0] == 16f)
                npcRotationSpeed = 0.01f;

			if (npcRotationSpeed != 0f)
				NPC.rotation = NPC.rotation.AngleTowards(npcRotation, npcRotationSpeed);

			// Alpha effects
			if (NPC.ai[0] != -1f && ((NPC.ai[0] != 6f && NPC.ai[0] != 13f) || NPC.ai[2] <= phaseSwitchTimer))
            {
                bool colliding = Collision.SolidCollision(NPC.position, NPC.width, NPC.height);

                if (colliding)
                    NPC.alpha += 15;
                else
                    NPC.alpha -= 15;

                if (NPC.alpha < 0)
                    NPC.alpha = 0;

                if (NPC.alpha > 150)
                    NPC.alpha = 150;
            }

            // Spawn effects
            if (NPC.ai[0] == -1f)
            {
                NPC.velocity *= 0.98f;

                int num1467 = Math.Sign(player.Center.X - vectorCenter.X);
                if (num1467 != 0)
                {
                    NPC.direction = num1467;
                    NPC.spriteDirection = -NPC.direction;
                }

                if (NPC.ai[2] > 20f)
                {
                    NPC.velocity.Y = -2f;
                    NPC.alpha -= 5;

                    bool colliding = Collision.SolidCollision(NPC.position, NPC.width, NPC.height);

                    if (colliding)
                        NPC.alpha += 15;

                    if (NPC.alpha < 0)
                        NPC.alpha = 0;

                    if (NPC.alpha > 150)
                        NPC.alpha = 150;
                }

                if (NPC.ai[2] == fireTornadoPhaseTimer - 30)
                {
                    int num1468 = 72;
                    for (int num1469 = 0; num1469 < num1468; num1469++)
                    {
                        Vector2 vector169 = Vector2.Normalize(NPC.velocity) * new Vector2(NPC.width / 2f, NPC.height) * 0.75f * 0.5f;
                        vector169 = vector169.RotatedBy((num1469 - (num1468 / 2 - 1)) * MathHelper.TwoPi / num1468) + NPC.Center;
                        Vector2 value16 = vector169 - NPC.Center;
                        int num1470 = Dust.NewDust(vector169 + value16, 0, 0, 244, value16.X * 2f, value16.Y * 2f, 100, default, 1.4f);
                        Main.dust[num1470].noGravity = true;
                        Main.dust[num1470].noLight = true;
                        Main.dust[num1470].velocity = Vector2.Normalize(value16) * 3f;
                    }

                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);
                }

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= spawnPhaseTimer)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.netUpdate = true;
                }
            }

            #region Phase1
			// Phase switch
            else if (NPC.ai[0] == 0f && !player.dead)
            {
                if (NPC.ai[1] == 0f)
                    NPC.ai[1] = 500 * Math.Sign((vectorCenter - player.Center).X);

                Vector2 value17 = player.Center + new Vector2(NPC.ai[1], -200f) - vectorCenter;
                Vector2 vector170 = Vector2.Normalize(value17 - NPC.velocity) * velocity;
				NPC.SimpleFlyMovement(vector170, acceleration);

                int num1471 = Math.Sign(player.Center.X - vectorCenter.X);
                if (num1471 != 0)
                {
                    if (NPC.ai[2] == 0f && num1471 != NPC.direction)
                        NPC.rotation += pie;

                    NPC.direction = num1471;

                    if (NPC.spriteDirection != -NPC.direction)
                        NPC.rotation += pie;

                    NPC.spriteDirection = -NPC.direction;
                }

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= phaseSwitchTimer)
                {
                    int aiState = 0;
                    switch ((int)NPC.ai[3])
                    {
                        case 0:
                        case 1:
                        case 2:
                            aiState = 1;
                            break;
                        case 3:
                            aiState = 5;
                            break;
                        case 4:
                            NPC.ai[3] = 1f;
                            aiState = 2;
                            break;
                        case 5:
                            NPC.ai[3] = 0f;
                            aiState = 3;
                            break;
                    }

                    if (phase2Check)
                        aiState = 4;

                    if (aiState == 1)
                    {
                        NPC.ai[0] = 1f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;

                        NPC.velocity = Vector2.Normalize(player.Center - vectorCenter) * chargeSpeed;
                        NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);

                        if (num1471 != 0)
                        {
                            NPC.direction = num1471;

                            if (NPC.spriteDirection == 1)
                                NPC.rotation += pie;

                            NPC.spriteDirection = -NPC.direction;
                        }
                    }
                    else if (aiState == 2)
                    {
                        NPC.ai[0] = 2f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                    }
                    else if (aiState == 3)
                    {
                        NPC.ai[0] = 3f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                    }
                    else if (aiState == 4)
                    {
                        NPC.ai[0] = 4f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                    }
                    else if (aiState == 5)
                    {
						NPC.localAI[1] += 1f;
						if (NPC.localAI[1] == fastChargeTelegraphTimer - 60)
							SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);

						if (NPC.localAI[1] > fastChargeTelegraphTimer)
						{
							NPC.ai[0] = 5f;
							NPC.ai[1] = 0f;
							NPC.ai[2] = 0f;
							NPC.localAI[1] = 0f;

							NPC.velocity = Vector2.Normalize(player.Center - vectorCenter) * chargeSpeed * fastChargeVelocityMultiplier;
							NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);

							if (num1471 != 0)
							{
								NPC.direction = num1471;

								if (NPC.spriteDirection == 1)
									NPC.rotation += pie;

								NPC.spriteDirection = -NPC.direction;
							}
						}
                    }

                    NPC.netUpdate = true;
                }
            }

			// Charge
            else if (NPC.ai[0] == 1f)
            {
				ChargeDust(7, pie);

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= chargeTime)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] += 2f;
                    NPC.netUpdate = true;
                }
            }

			// Fireball breath
            else if (NPC.ai[0] == 2f)
            {
                if (NPC.ai[1] == 0f)
                    NPC.ai[1] = 500 * Math.Sign((vectorCenter - player.Center).X);

                Vector2 value19 = player.Center + new Vector2(NPC.ai[1], -400f) - vectorCenter;
                Vector2 vector172 = Vector2.Normalize(value19 - NPC.velocity) * flareBombPhaseVelocity;
				NPC.SimpleFlyMovement(vector172, flareBombPhaseAcceleration);

                if (NPC.ai[2] == 0f)
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);

                if (NPC.ai[2] % flareBombSpawnDivisor == 0f)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
						SpawnDetonatingFlares(fromMouth, player, maxFlareCount, new int[] { ModContent.NPCType<DetonatingFlare>() });
						int type = ModContent.ProjectileType<FlareBomb>();
						int damage = NPC.GetProjectileDamage(type);
						Projectile.NewProjectile(NPC.GetSource_FromThis(), fromMouth, Vector2.Zero, type, damage, 0f, Main.myPlayer, NPC.target, 1f);
                    }
                }

                int num1476 = Math.Sign(player.Center.X - vectorCenter.X);
                if (num1476 != 0)
                {
                    NPC.direction = num1476;

                    if (NPC.spriteDirection != -NPC.direction)
                        NPC.rotation += pie;

                    NPC.spriteDirection = -NPC.direction;
                }

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= flareBombPhaseTimer)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.netUpdate = true;
                }
            }

			// Fire tornadoes
            else if (NPC.ai[0] == 3f)
            {
                NPC.velocity *= 0.98f;
                NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0f, 0.02f);

                if (NPC.ai[2] == fireTornadoPhaseTimer - 30)
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoarShort"), NPC.position);

                if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[2] == fireTornadoPhaseTimer - 30)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), vectorCenter.X, vectorCenter.Y, NPC.direction * 4, 8f, ModContent.ProjectileType<Flare>(), 0, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), vectorCenter.X, vectorCenter.Y, -(float)NPC.direction * 4, 8f, ModContent.ProjectileType<Flare>(), 0, 0f, Main.myPlayer, 0f, 0f);
                }

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= fireTornadoPhaseTimer)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.netUpdate = true;
                }
            }

			// Enter new phase
            else if (NPC.ai[0] == 4f)
            {
                NPC.velocity *= 0.98f;
                NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0f, 0.02f);

                if (NPC.ai[2] == newPhaseTimer - 60)
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= newPhaseTimer)
                {
                    NPC.ai[0] = 6f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
					NPC.localAI[1] = 0f;
					NPC.netUpdate = true;
                }
            }

			// Fast charge
            else if (NPC.ai[0] == 5f)
            {
				ChargeDust(14, pie);

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= chargeTime)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] += 2f;
                    NPC.netUpdate = true;
                }
            }
            #endregion

            #region Phase2
			// Phase switch
            else if (NPC.ai[0] == 6f && !player.dead)
            {
                if (NPC.ai[1] == 0f)
                    NPC.ai[1] = 500 * Math.Sign((vectorCenter - player.Center).X);

                Vector2 value20 = player.Center + new Vector2(NPC.ai[1], -200f) - vectorCenter;
                Vector2 vector175 = Vector2.Normalize(value20 - NPC.velocity) * velocity;
				NPC.SimpleFlyMovement(vector175, acceleration);

                int num1477 = Math.Sign(player.Center.X - vectorCenter.X);
                if (num1477 != 0)
                {
                    if (NPC.ai[2] == 0f && num1477 != NPC.direction)
                        NPC.rotation += pie;

                    NPC.direction = num1477;

                    if (NPC.spriteDirection != -NPC.direction)
                        NPC.rotation += pie;

                    NPC.spriteDirection = -NPC.direction;
                }

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= phaseSwitchTimer)
                {
                    int aiState = 0;
                    switch ((int)NPC.ai[3])
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            aiState = 1;
                            break;
                        case 4:
                            aiState = 5;
                            break;
                        case 5:
                            aiState = 6;
                            break;
                        case 6:
                            aiState = 2;
                            break;
                        case 7:
                            NPC.ai[3] = 0f;
                            aiState = 3;
                            break;
                    }

                    if (phase3Check)
                        aiState = 4;

                    if (aiState == 1)
                    {
                        NPC.ai[0] = 7f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;

                        NPC.velocity = Vector2.Normalize(player.Center - vectorCenter) * chargeSpeed;
                        NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);

                        if (num1477 != 0)
                        {
                            NPC.direction = num1477;

                            if (NPC.spriteDirection == 1)
                                NPC.rotation += pie;

                            NPC.spriteDirection = -NPC.direction;
                        }
                    }
                    else if (aiState == 2)
                    {
						Vector2 npcCenter = NPC.Center;

						if (NPC.alpha < 255)
						{
							NPC.alpha += 17;
							if (NPC.alpha > 255)
								NPC.alpha = 255;
						}

						if (NPC.ai[2] == phaseSwitchTimer + 15f)
							SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoarShort"), NPC.position);

						if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[2] == phaseSwitchTimer + 15f)
						{
							Vector2 center = player.Center + new Vector2(0f, -540f);
							npcCenter = NPC.Center = center;
						}

						if (NPC.ai[2] < phaseSwitchTimer + teleportPhaseTimer)
							return;

						NPC.velocity = Vector2.Normalize(player.Center - vectorCenter) * spinPhaseVelocity;
                        NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);

                        if (num1477 != 0)
                        {
                            NPC.direction = num1477;

                            if (NPC.spriteDirection == 1)
                                NPC.rotation += pie;

                            NPC.spriteDirection = -NPC.direction;
                        }

                        NPC.ai[0] = 8f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
						NPC.ai[3] = 1f;
					}
                    else if (aiState == 3)
                    {
                        NPC.ai[0] = 9f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                    }
                    else if (aiState == 4)
                    {
                        NPC.ai[0] = 10f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                    }
                    else if (aiState == 5)
                    {
						NPC.localAI[1] += 1f;
						if (NPC.localAI[1] == fastChargeTelegraphTimer - 60)
							SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);

						if (NPC.localAI[1] > fastChargeTelegraphTimer)
						{
							NPC.ai[0] = 11f;
							NPC.ai[1] = 0f;
							NPC.ai[2] = 0f;
							NPC.localAI[1] = 0f;

							NPC.velocity = Vector2.Normalize(player.Center - vectorCenter) * chargeSpeed * fastChargeVelocityMultiplier;
							NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);

							if (num1477 != 0)
							{
								NPC.direction = num1477;

								if (NPC.spriteDirection == 1)
									NPC.rotation += pie;

								NPC.spriteDirection = -NPC.direction;
							}
						}
                    }
                    else if (aiState == 6)
                    {
                        NPC.velocity = Vector2.Normalize(player.Center - vectorCenter) * spinPhaseVelocity;
                        NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);

                        if (num1477 != 0)
                        {
                            NPC.direction = num1477;

                            if (NPC.spriteDirection == 1)
                                NPC.rotation += pie;

                            NPC.spriteDirection = -NPC.direction;
                        }

                        NPC.ai[0] = 12f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                    }

                    NPC.netUpdate = true;
                }
            }

			// Charge
            else if (NPC.ai[0] == 7f)
            {
				ChargeDust(7, pie);

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= chargeTime)
                {
                    NPC.ai[0] = 6f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] += 2f;
                    NPC.netUpdate = true;
                }
            }

			// Flare Dust bullet hell
            else if (NPC.ai[0] == 8f)
            {
				if (NPC.ai[2] == 0f)
				{
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);
					flareDustBulletHellSpawn = vectorCenter + NPC.velocity.RotatedBy(MathHelper.PiOver2 * -NPC.direction) * spinTime / (MathHelper.TwoPi * 3f);
				}

				NPC.ai[2] += 1f;

				if (NPC.ai[2] % flareDustSpawnDivisor == 0f)
                {
					if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
						SpawnDetonatingFlares(flareDustBulletHellSpawn, player, maxFlareCount, new int[] { ModContent.NPCType<DetonatingFlare2>() });
						int ringReduction = (int)MathHelper.Lerp(0f, 20f, NPC.ai[2] / flareDustPhaseTimer);
						int totalProjectiles = 38 - ringReduction; // 36 for first ring, 18 for last ring
						DoFlareDustBulletHell(0, flareDustSpawnDivisor, NPC.GetProjectileDamage(ModContent.ProjectileType<FlareDust>()), totalProjectiles, 0f, 0f, false);
					}
                }

                NPC.velocity = NPC.velocity.RotatedBy(-(double)spinPhaseRotation * (float)NPC.direction);
                NPC.rotation -= spinPhaseRotation * NPC.direction;

                if (NPC.ai[2] >= flareDustPhaseTimer)
                {
					NPC.ai[0] = 6f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = increasedIdleTimeAfterBulletHell;
                    NPC.netUpdate = true;
                }
            }

			// Infernado
            else if (NPC.ai[0] == 9f)
            {
                NPC.velocity *= 0.98f;
                NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0f, 0.02f);

                if (NPC.ai[2] == fireTornadoPhaseTimer - 30)
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoarShort"), NPC.position);

                if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[2] == fireTornadoPhaseTimer - 30)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), vectorCenter.X, vectorCenter.Y, 0f, 0f, ModContent.ProjectileType<BigFlare>(), 0, 0f, Main.myPlayer, 1f, NPC.target + 1);

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= fireTornadoPhaseTimer)
                {
                    NPC.ai[0] = 6f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.netUpdate = true;
                }
            }

			// Enter new phase
            else if (NPC.ai[0] == 10f)
            {
                NPC.velocity *= 0.98f;
                NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0f, 0.02f);

                if (NPC.ai[2] == newPhaseTimer - 60)
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= newPhaseTimer)
                {
                    NPC.ai[0] = 13f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
					NPC.localAI[1] = 0f;
					NPC.netUpdate = true;
                }
            }

			// Fast charge
            else if (NPC.ai[0] == 11f)
            {
				ChargeDust(14, pie);

				NPC.ai[2] += 1f;
                if (NPC.ai[2] >= chargeTime)
                {
                    NPC.ai[0] = 6f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] += 2f;
                    NPC.netUpdate = true;
                }
            }

			// Flare Dust that speeds up and whips around in a wave
            else if (NPC.ai[0] == 12f)
            {
				if (NPC.ai[2] == 0f)
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);

				NPC.ai[2] += 1f;

				if (NPC.ai[2] % flareDustSpawnDivisor2 == 0f)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
						Vector2 projectileVelocity = player.Center - fromMouth;
						projectileVelocity.Normalize();
						projectileVelocity *= 0.1f;
						int type = ModContent.ProjectileType<FlareDust2>();
						int damage = NPC.GetProjectileDamage(type);
						Projectile.NewProjectile(NPC.GetSource_FromThis(), fromMouth, projectileVelocity, type, damage, 0f, Main.myPlayer, 0f, 0f);
                    }
                }

                NPC.velocity = NPC.velocity.RotatedBy(-(double)spinPhaseRotation * (float)NPC.direction);
                NPC.rotation -= spinPhaseRotation * NPC.direction;

                if (NPC.ai[2] >= flareDustPhaseTimer2)
                {
                    NPC.ai[0] = 6f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] += 2f;
                    NPC.netUpdate = true;
                }
            }
            #endregion

            #region Phase3
			// Phase switch
            else if (NPC.ai[0] == 13f && !player.dead)
            {
                if (NPC.ai[1] == 0f)
                    NPC.ai[1] = 500 * Math.Sign((vectorCenter - player.Center).X);

                Vector2 value20 = player.Center + new Vector2(NPC.ai[1], -200f) - vectorCenter;
                Vector2 vector175 = Vector2.Normalize(value20 - NPC.velocity) * velocity;
				NPC.SimpleFlyMovement(vector175, acceleration);

                int num1477 = Math.Sign(player.Center.X - vectorCenter.X);
                if (num1477 != 0)
                {
                    if (NPC.ai[2] == 0f && num1477 != NPC.direction)
                        NPC.rotation += pie;

                    NPC.direction = num1477;

                    if (NPC.spriteDirection != -NPC.direction)
                        NPC.rotation += pie;

                    NPC.spriteDirection = -NPC.direction;
                }

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= phaseSwitchTimer)
                {
                    int aiState = 0;
                    switch ((int)NPC.ai[3])
                    {
                        case 0:
                        case 1:
                            aiState = 1;
                            break;
                        case 2:
                        case 3:
                        case 4:
                            aiState = 5;
                            break;
                        case 5:
                            aiState = 3;
                            break;
                        case 6:
							aiState = 6;
                            break;
                        case 7:
                            NPC.ai[3] = 1f;
                            aiState = 7;
                            break;
						case 8:
							aiState = 2;
							break;
                    }

                    if (phase4Check)
                        aiState = 4;

                    if (aiState == 1)
                    {
                        NPC.ai[0] = 14f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;

                        NPC.velocity = Vector2.Normalize(player.Center - vectorCenter) * chargeSpeed;
                        NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);

                        if (num1477 != 0)
                        {
                            NPC.direction = num1477;

                            if (NPC.spriteDirection == 1)
                                NPC.rotation += pie;

                            NPC.spriteDirection = -NPC.direction;
                        }
                    }
                    else if (aiState == 2)
                    {
						Vector2 npcCenter = NPC.Center;

						if (NPC.alpha < 255)
						{
							NPC.alpha += 17;
							if (NPC.alpha > 255)
								NPC.alpha = 255;
						}

						if (NPC.ai[2] == phaseSwitchTimer + 15f)
							SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoarShort"), NPC.position);

						if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[2] == phaseSwitchTimer + 15f)
						{
							Vector2 center = player.Center + new Vector2(0f, -540f);
							npcCenter = NPC.Center = center;
						}

						if (NPC.ai[2] < phaseSwitchTimer + teleportPhaseTimer)
							return;

						NPC.velocity = Vector2.Normalize(player.Center - vectorCenter) * spinPhaseVelocity;
                        NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);

                        if (num1477 != 0)
                        {
                            NPC.direction = num1477;

                            if (NPC.spriteDirection == 1)
                                NPC.rotation += pie;

                            NPC.spriteDirection = -NPC.direction;
                        }

                        NPC.ai[0] = 15f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
						NPC.ai[3] = 0f;
					}
                    else if (aiState == 3)
                    {
                        NPC.ai[0] = 16f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                    }
                    else if (aiState == 4)
                    {
                        NPC.ai[0] = 17f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                    }
                    else if (aiState == 5)
                    {
						NPC.localAI[1] += 1f;
						if (NPC.localAI[1] == fastChargeTelegraphTimer - 60)
							SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);

						if (NPC.localAI[1] > fastChargeTelegraphTimer)
						{
							NPC.ai[0] = 18f;
							NPC.ai[1] = 0f;
							NPC.ai[2] = 0f;
							NPC.localAI[1] = 0f;

							NPC.velocity = Vector2.Normalize(player.Center - vectorCenter) * chargeSpeed * fastChargeVelocityMultiplier;
							NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);

							if (num1477 != 0)
							{
								NPC.direction = num1477;

								if (NPC.spriteDirection == 1)
									NPC.rotation += pie;

								NPC.spriteDirection = -NPC.direction;
							}
						}
                    }
                    else if (aiState == 6)
                    {
                        NPC.velocity = Vector2.Normalize(player.Center - vectorCenter) * spinPhaseVelocity;
                        NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);

                        if (num1477 != 0)
                        {
                            NPC.direction = num1477;

                            if (NPC.spriteDirection == 1)
                                NPC.rotation += pie;

                            NPC.spriteDirection = -NPC.direction;
                        }

                        NPC.ai[0] = 19f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                    }
					else if (aiState == 7)
					{
						NPC.ai[0] = 20f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
					}

					NPC.netUpdate = true;
                }
            }

			// Charge
            else if (NPC.ai[0] == 14f)
            {
				ChargeDust(7, pie);

				NPC.ai[2] += 1f;
                if (NPC.ai[2] >= chargeTime)
                {
                    NPC.ai[0] = 13f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] += 2f;
                    NPC.netUpdate = true;
                }
            }

			// Flare Dust bullet hell
			else if (NPC.ai[0] == 15f)
            {
				if (NPC.ai[2] == 0f)
				{
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);
					flareDustBulletHellSpawn = vectorCenter + NPC.velocity.RotatedBy(MathHelper.PiOver2 * -NPC.direction) * spinTime / (MathHelper.TwoPi * 3f);
				}

				NPC.ai[2] += 1f;

				if (NPC.ai[2] % flareDustSpawnDivisor == 0f)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
						SpawnDetonatingFlares(flareDustBulletHellSpawn, player, maxFlareCount, new int[] { ModContent.NPCType<DetonatingFlare>(), ModContent.NPCType<DetonatingFlare2>() });
				}

				if (NPC.ai[2] % flareDustSpawnDivisor3 == 0f)
				{
					// Rotate spiral by 7.2 * (300 / 12) = +90 degrees and then back -90 degrees

					if (Main.netMode != NetmodeID.MultiplayerClient)
						DoFlareDustBulletHell(1, flareDustPhaseTimer, NPC.GetProjectileDamage(ModContent.ProjectileType<FlareDust>()), 8, 12f, 3.6f, false);
				}

				NPC.velocity = NPC.velocity.RotatedBy(-(double)spinPhaseRotation * (float)NPC.direction);
				NPC.rotation -= spinPhaseRotation * NPC.direction;

				if (NPC.ai[2] >= flareDustPhaseTimer)
				{
					NPC.ai[0] = 13f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.localAI[2] = 0f;
					NPC.netUpdate = true;
				}
            }

			// Infernado
            else if (NPC.ai[0] == 16f)
            {
                NPC.velocity *= 0.98f;
                NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0f, 0.02f);

                if (NPC.ai[2] == fireTornadoPhaseTimer - 30)
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoarShort"), NPC.position);

                if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[2] == fireTornadoPhaseTimer - 30)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), vectorCenter.X, vectorCenter.Y, 0f, 0f, ModContent.ProjectileType<BigFlare>(), 0, 0f, Main.myPlayer, 1f, NPC.target + 1);

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= fireTornadoPhaseTimer)
                {
                    NPC.ai[0] = 13f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] += 3f;
                    NPC.netUpdate = true;
                }
            }

			// Enter new phase
            else if (NPC.ai[0] == 17f)
            {
				NPC.velocity *= 0.98f;
				NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0f, 0.02f);

				if (NPC.ai[2] == newPhaseTimer - 60)
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);

				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= newPhaseTimer)
				{
					startSecondAI = true;
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					NPC.localAI[1] = 0f;
					NPC.netUpdate = true;
				}
            }

			// Fast charge
            else if (NPC.ai[0] == 18f)
            {
				ChargeDust(14, pie);

				NPC.ai[2] += 1f;
                if (NPC.ai[2] >= chargeTime)
                {
                    NPC.ai[0] = 13f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] += 2f;
                    NPC.netUpdate = true;
                }
            }

			// Fireball ring
            else if (NPC.ai[0] == 19f)
            {
				if (NPC.ai[2] == 0f)
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);

				NPC.ai[2] += 1f;

				if (NPC.ai[2] % flareDustSpawnDivisor2 == 0f)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 projectileVelocity = player.Center - fromMouth;
						projectileVelocity.Normalize();
						projectileVelocity *= 0.1f;
						int type = ModContent.ProjectileType<FlareDust2>();
						int damage = NPC.GetProjectileDamage(type);
						Projectile.NewProjectile(NPC.GetSource_FromThis(), fromMouth, projectileVelocity, type, damage, 0f, Main.myPlayer, 0f, 0f);
					}
				}

				NPC.velocity = NPC.velocity.RotatedBy(-(double)spinPhaseRotation * (float)NPC.direction);
				NPC.rotation -= spinPhaseRotation * NPC.direction;

				if (NPC.ai[2] >= flareDustPhaseTimer2 - 50)
				{
					NPC.ai[0] = 13f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] += 1f;
					NPC.netUpdate = true;
				}
            }

			// Fireball breath
			else if (NPC.ai[0] == 20f)
			{
				if (NPC.ai[1] == 0f)
					NPC.ai[1] = 400 * Math.Sign((vectorCenter - player.Center).X);

				Vector2 value19 = player.Center + new Vector2(NPC.ai[1], -400f) - vectorCenter;
				Vector2 vector172 = Vector2.Normalize(value19 - NPC.velocity) * flareBombPhaseVelocity;
				NPC.SimpleFlyMovement(vector172, flareBombPhaseAcceleration);

				if (NPC.ai[2] == 0f)
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);

				if (NPC.ai[2] % flareBombSpawnDivisor == 0f)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						SpawnDetonatingFlares(fromMouth, player, maxFlareCount, new int[] { ModContent.NPCType<DetonatingFlare>(), ModContent.NPCType<DetonatingFlare2>() });
						int type = ModContent.ProjectileType<FlareBomb>();
						int damage = NPC.GetProjectileDamage(type);
						Projectile.NewProjectile(NPC.GetSource_FromThis(), fromMouth, Vector2.Zero, type, damage, 0f, Main.myPlayer, NPC.target, 1f);
					}
				}

				int num1476 = Math.Sign(player.Center.X - vectorCenter.X);
				if (num1476 != 0)
				{
					NPC.direction = num1476;

					if (NPC.spriteDirection != -NPC.direction)
						NPC.rotation += pie;

					NPC.spriteDirection = -NPC.direction;
				}

				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= flareBombPhaseTimer - 15)
				{
					NPC.ai[0] = 13f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.netUpdate = true;
				}
			}
			#endregion
		}

        #region AI2
        public void Yharon_AI2(bool expertMode, bool revenge, bool death, float pie)
        {
            bool phase2 = death || NPC.life <= NPC.lifeMax * (revenge ? 0.8 : (expertMode ? 0.7 : 0.5));
            bool phase3 = NPC.life <= NPC.lifeMax * (death ? 0.65 : (revenge ? 0.5 : (expertMode ? 0.4 : 0.25)));
            bool phase4 = NPC.life <= NPC.lifeMax * (death ? 0.3 : 0.2) && revenge;

            if (NPC.ai[0] != 8f)
            {
                NPC.alpha -= 25;
                if (NPC.alpha < 0)
                    NPC.alpha = 0;
            }

            if (!moveCloser)
            {
                Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/DragonGod");

                moveCloser = true;

                string key = "The air is getting warmer around you.";
                Color messageColor = Color.Orange;

                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }

			if (invincibilityCounter < 900)
			{
				phase2 = phase3 = phase4 = false;

				invincibilityCounter += 1;

				int heal = 5; //900 / 5 = 180
				healCounter += 1;
				if (healCounter >= heal)
				{
					healCounter = 0;

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int healAmt = NPC.lifeMax / 200;
						if (healAmt > NPC.lifeMax - NPC.life)
							healAmt = NPC.lifeMax - NPC.life;

						if (healAmt > 0)
						{
							NPC.life += healAmt;
							NPC.HealEffect(healAmt, true);
							NPC.netUpdate = true;
						}
					}
				}
			}
			else
			{
				// Damage immunity
				switch (secondPhasePhase)
				{
					case 1:
						NPC.dontTakeDamage = phase2;
						break;
					case 2:
						NPC.dontTakeDamage = phase3;
						break;
					case 3:
						NPC.dontTakeDamage = phase4;
						break;
					case 4:
						NPC.dontTakeDamage = false;
						break;
				}

				if (!NPC.dontTakeDamage)
					NPC.dontTakeDamage = NPC.ai[0] == 9f;
			}

			// Acquire target and determine enrage state
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(true);
				NPC.netUpdate = true;
			}

			Player targetData = Main.player[NPC.target];

			// Despawn
			bool targetDead = false;
			if (targetData.dead || !targetData.active)
			{
				NPC.TargetClosest(true);
				targetData = Main.player[NPC.target];
				if (targetData.dead || !targetData.active)
				{
					targetDead = true;

					NPC.velocity.Y -= 0.4f;

					if (NPC.timeLeft > 60)
						NPC.timeLeft = 60;

					NPC.ai[0] = 1f;
					NPC.ai[1] = 0f;
				}
			}
			else if (NPC.timeLeft < 1800)
				NPC.timeLeft = 1800;

			enraged = !targetData.Hitbox.Intersects(safeBox);
			if (enraged)
			{
				protectionBoost = true;
				NPC.damage = NPC.defDamage * 5;
			}
			else
			{
				protectionBoost = false;
				NPC.damage = NPC.defDamage;
			}

			// Set DR based on protection boost (aka enrage)
			bool chargeTelegraph = NPC.ai[0] < 2f && NPC.localAI[1] > 0f;
			bool bulletHell = NPC.ai[0] == 5f;
			NPC.Calamity().DR = protectionBoost ? EnragedDR : ((chargeTelegraph || bulletHell) ? ChargeTelegraph_DR : Phase2_DR);

			if (bulletHell)
				NPC.damage = 0;

			float phaseSwitchTimer = expertMode ? 30f : 32f;
			float acceleration = expertMode ? 0.92f : 0.9f;
            float velocity = expertMode ? 14.5f : 14f;
			float chargeTime = expertMode ? 32f : 35f;
            float chargeSpeed = expertMode ? 32f : 30f;
			float fastChargeVelocityMultiplier = 1.5f;
			int fastChargeTelegraphTimer = secondPhasePhase == 4 ? 60 : 120;

			float fireballBreathTimer = 60f;
            float fireballBreathPhaseTimer = fireballBreathTimer + 120f;
			float fireballBreathPhaseVelocity = 22f;

            float splittingFireballBreathTimer = 40f;
            float splittingFireballBreathPhaseVelocity = 22f;
            int splittingFireballBreathDivisor = 10;
			int splittingFireballs = 10;
            int splittingFireballBreathTimer2 = splittingFireballs * splittingFireballBreathDivisor;
            float splittingFireballBreathYVelocityTimer = 40f;
            float splittingFireballBreathPhaseTimer = splittingFireballBreathTimer + splittingFireballBreathTimer2 + splittingFireballBreathYVelocityTimer;

            int spinPhaseTimer = secondPhasePhase == 4 ? (death ? 160 : 180) : (death ? 200 : 240);
			float spinTime = spinPhaseTimer / 2;
			float spinRotation = MathHelper.TwoPi * 3 / spinTime;
			float spinPhaseVelocity = 25f;
			int flareDustSpawnDivisor = spinPhaseTimer / 10;
			int flareDustSpawnDivisor2 = spinPhaseTimer / 20 + (secondPhasePhase == 4 ? spinPhaseTimer / 60 : 0);
			float increasedIdleTimeAfterBulletHell = -120f;

			float flareSpawnDecelerationTimer = death ? 75f : 90f;
			float flareSpawnPhaseTimer = death ? 150f : 180f;

			float teleportPhaseTimer = 45f;

			if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
            {
                acceleration = 1.2f;
                velocity = 18f;
				chargeTime = 25f;
                chargeSpeed = 45f;
            }
			else if (revenge)
			{
				float chargeTimeDecrease = death ? 4f : 2f;
				float velocityMult = death ? 1.1f : 1.05f;
				acceleration *= velocityMult;
				velocity *= velocityMult;
				chargeTime -= chargeTimeDecrease;
				chargeSpeed *= velocityMult;
			}

            if (NPC.ai[0] == 0f)
            {
				NPC.ai[1] += 1f;
                if (NPC.ai[1] >= 10f)
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 1f;
                    NPC.ai[2] = 0f;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 1f)
            {
                if (NPC.ai[2] == 0f)
                    NPC.ai[2] = (NPC.Center.X < targetData.Center.X) ? 1 : -1;

                Vector2 destination = targetData.Center + new Vector2(-NPC.ai[2] * 450f, -200f);
                Vector2 desiredVelocity = NPC.DirectionTo(destination) * velocity;

				if (!targetDead)
					NPC.SimpleFlyMovement(desiredVelocity, acceleration);

                int num27 = (NPC.Center.X < targetData.Center.X) ? 1 : -1;
                NPC.direction = NPC.spriteDirection = num27;

				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= phaseSwitchTimer)
                {
                    int num28 = 1;
                    if (phase4)
                    {
                        switch ((int)NPC.ai[3])
                        {
                            case 0:
                                num28 = 8; //teleport
                                break;
                            case 1:
                            case 2:
                                num28 = 7; //fast charge
                                break;
                            case 3:
                                num28 = 5; //fire circle + tornado (only once) + fireballs
                                break;
                        }
                    }
                    else if (phase3)
                    {
                        switch ((int)NPC.ai[3])
                        {
                            case 0:
                                num28 = 6; //tornado
                                break;
                            case 1:
                                num28 = 7; //fast charge
                                break;
                            case 2:
                                num28 = 8; //teleport
                                break;
                            case 3:
                                num28 = 7; //fast charge
                                break;
                            case 4:
                                num28 = 5; //fire circle
                                break;
                            case 5:
                                num28 = 4; //fireballs
                                break;
                            case 6:
                                num28 = 7; //fast charge
                                break;
                            case 7:
                                num28 = 8; //teleport
                                break;
                            case 8:
                                num28 = 7; //fast charge
                                break;
                            case 9:
                                num28 = 3; //fireballs
                                break;
                            case 10:
                                num28 = 6; //tornado
                                break;
                            case 11:
                                num28 = 7; //fast charge
                                break;
                            case 12:
                                num28 = 8; //teleport
                                break;
                            case 13:
                                num28 = 7; //fast charge
                                break;
                            case 14:
                                num28 = 5; //fire circle
                                break;
                            case 15:
                                num28 = 4; //fireballs
                                break;
                        }
                    }
                    else if (phase2)
                    {
                        switch ((int)NPC.ai[3])
                        {
                            case 0:
                                num28 = 6; //tornado
                                break;
                            case 1:
                                num28 = 7; //fast charge
                                break;
                            case 2:
                                num28 = 2; //charge
                                break;
                            case 3:
                                num28 = 5; //fire circle
                                break;
                            case 4:
                                num28 = 4; //fireballs
                                break;
                            case 5:
                                num28 = 7; //fast charge
                                break;
                            case 6:
                                num28 = 2; //charge
                                break;
                            case 7:
                                num28 = 3; //fireballs
                                break;
                            case 8:
                                num28 = 7; //fast charge
                                break;
                            case 9:
                                num28 = 2; //charge
                                break;
                            case 10:
                                num28 = 5; //fire circle
                                break;
                        }
                    }
                    else
                    {
                        switch ((int)NPC.ai[3])
                        {
                            case 0:
                                num28 = 6; //tornado
                                break;
                            case 1:
                            case 2:
                                num28 = 2; //charge
                                break;
                            case 3:
                                num28 = 3; //fireballs
                                break;
                            case 4:
                            case 5:
                                num28 = 7; //fast charge
                                break;
                            case 6:
                                num28 = 4; //fireballs
                                break;
                            case 7:
                            case 8:
                                num28 = 2; //charge
                                break;
                            case 9:
                                num28 = 5; //fire circle
                                break;
                        }
                    }

					if (num28 == 5 && NPC.ai[1] < phaseSwitchTimer + teleportPhaseTimer)
					{
						float newRotation = NPC.DirectionTo(targetData.Center).ToRotation();
						float amount = 0.04f;

						if (NPC.spriteDirection == -1)
							newRotation += pie;

						if (amount != 0f)
							NPC.rotation = NPC.rotation.AngleTowards(newRotation, amount);

						Vector2 npcCenter = NPC.Center;

						if (NPC.alpha < 255)
						{
							NPC.alpha += 17;
							if (NPC.alpha > 255)
								NPC.alpha = 255;
						}

						float timeBeforeTeleport = teleportPhaseTimer - 15f;
						if (NPC.ai[1] == phaseSwitchTimer + timeBeforeTeleport)
							SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoarShort"), NPC.position);

						if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[1] == phaseSwitchTimer + timeBeforeTeleport)
						{
							Vector2 center = targetData.Center + new Vector2(0f, -540f);
							npcCenter = NPC.Center = center;
						}

						return;
					}

					if (num28 == 7 && NPC.localAI[1] <= fastChargeTelegraphTimer)
					{
						float newRotation = NPC.DirectionTo(targetData.Center).ToRotation();
						float amount = 0.04f;

						if (NPC.spriteDirection == -1)
							newRotation += pie;

						if (amount != 0f)
							NPC.rotation = NPC.rotation.AngleTowards(newRotation, amount);

						NPC.localAI[1] += 1f;
						if (NPC.localAI[1] == fastChargeTelegraphTimer - (secondPhasePhase == 4 ? 30 : 60))
							SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);

						return;
					}

                    NPC.ai[0] = num28;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] += 1f;
					NPC.localAI[1] = 0f;

					switch (secondPhasePhase)
                    {
                        case 1:
                            if (phase2)
                            {
                                secondPhasePhase = 2;
                                NPC.ai[0] = 9f;
                                NPC.ai[1] = 0f;
                                NPC.ai[2] = 0f;
                                NPC.ai[3] = 0f;
                            }
                            break;
                        case 2:
                            if (phase3)
                            {
                                secondPhasePhase = 3;
                                NPC.ai[0] = 9f;
                                NPC.ai[1] = 0f;
                                NPC.ai[2] = 0f;
                                NPC.ai[3] = 0f;
                            }
                            break;
                        case 3:
                            if (phase4)
                            {
                                secondPhasePhase = 4;
                                NPC.ai[0] = 9f;
                                NPC.ai[1] = 0f;
                                NPC.ai[2] = 0f;
                                NPC.ai[3] = 0f;
                            }
                            break;
                    }

                    NPC.netUpdate = true;

                    float aiLimit = 10f;
                    if (phase4)
                        aiLimit = 4f;
                    else if (phase3)
                        aiLimit = 16f;
                    else if (phase2)
                        aiLimit = 11f;

                    if (NPC.ai[3] >= aiLimit)
                        NPC.ai[3] = 0f;

                    switch (num28)
                    {
                        case 2: //charge
                        {
                            Vector2 vector = NPC.DirectionTo(targetData.Center);
                            NPC.spriteDirection = (vector.X > 0f) ? 1 : -1;
                            NPC.rotation = vector.ToRotation();

                            if (NPC.spriteDirection == -1)
                                NPC.rotation += pie;

                            NPC.velocity = vector * chargeSpeed;

                            break;
                        }
                        case 3: //fireballs
                        {
                            Vector2 vector2 = new Vector2((targetData.Center.X > NPC.Center.X) ? 1 : -1, 0f);
                            NPC.spriteDirection = (vector2.X > 0f) ? 1 : -1;
                            NPC.velocity = vector2 * -2f;

                            break;
                        }
                        case 5: //spin move
                        {
                            Vector2 vector3 = NPC.DirectionTo(targetData.Center);
                            NPC.spriteDirection = (vector3.X > 0f) ? 1 : -1;
                            NPC.rotation = vector3.ToRotation();

                            if (NPC.spriteDirection == -1)
                                NPC.rotation += pie;

                            NPC.velocity = vector3 * spinPhaseVelocity;

                            break;
                        }
                        case 7: //fast charge
                        {
                            Vector2 vector = NPC.DirectionTo(targetData.Center);
                            NPC.spriteDirection = (vector.X > 0f) ? 1 : -1;
                            NPC.rotation = vector.ToRotation();

                            if (NPC.spriteDirection == -1)
                                NPC.rotation += pie;

                            NPC.velocity = vector * chargeSpeed * fastChargeVelocityMultiplier;

                            break;
                        }
                    }
                }
            }

			// Charge
            else if (NPC.ai[0] == 2f)
            {
                if (NPC.ai[1] == 1f)
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoarShort"), NPC.position);

				ChargeDust(7, pie);

				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= chargeTime)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                }
            }

			// Fireball spit
            else if (NPC.ai[0] == 3f)
            {
                int num29 = (NPC.Center.X < targetData.Center.X) ? 1 : -1;
                NPC.ai[2] = num29;

				NPC.ai[1] += 1f;
				if (NPC.ai[1] < fireballBreathTimer)
                {
                    Vector2 vector4 = targetData.Center + new Vector2(num29 * -750f, -300f);
                    Vector2 value = NPC.DirectionTo(vector4) * 16f;

                    if (NPC.Distance(vector4) < 16f)
                        NPC.Center = vector4;
                    else
                        NPC.position += value;

                    if (Vector2.Distance(vector4, NPC.Center) < 32f)
                        NPC.ai[1] = fireballBreathTimer - 1f;
                }

                if (NPC.ai[1] == fireballBreathTimer)
                {
                    int direction = (targetData.Center.X > NPC.Center.X) ? 1 : -1;
                    NPC.velocity = new Vector2(direction, 0f) * fireballBreathPhaseVelocity;
                    NPC.direction = NPC.spriteDirection = direction;
                }

                if (NPC.ai[1] >= fireballBreathTimer)
                {
                    if (NPC.ai[1] % 10 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        float xOffset = 30f;
                        Vector2 position = NPC.Center + new Vector2((110f + xOffset) * NPC.direction, -20f).RotatedBy(NPC.rotation);
						Vector2 projectileVelocity = targetData.Center - position;
						projectileVelocity.Normalize();
						projectileVelocity *= 0.1f;
						int type = ModContent.ProjectileType<FlareDust2>();
						int damage = NPC.GetProjectileDamage(type);
						Projectile.NewProjectile(Entity.GetSource_FromThis(), position, projectileVelocity, type, damage, 0f, Main.myPlayer, 1f, 0f);
                    }

                    if (Math.Abs(targetData.Center.X - NPC.Center.X) > 700f && Math.Abs(NPC.velocity.X) < chargeSpeed)
                        NPC.velocity.X += Math.Sign(NPC.velocity.X) * 0.5f;
                }

                if (NPC.ai[1] >= fireballBreathPhaseTimer)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                }
            }

			// Splitting fireball breath
            else if (NPC.ai[0] == 4f)
            {
                int num31 = (NPC.Center.X < targetData.Center.X) ? 1 : -1;
                NPC.ai[2] = num31;

                if (NPC.ai[1] < splittingFireballBreathTimer)
                {
                    Vector2 vector5 = targetData.Center + new Vector2(num31 * -750f, -300f);
                    Vector2 value2 = NPC.DirectionTo(vector5) * splittingFireballBreathPhaseVelocity;

                    NPC.velocity = Vector2.Lerp(NPC.velocity, value2, 0.0333333351f);

                    int direction = (NPC.Center.X < targetData.Center.X) ? 1 : -1;
                    NPC.direction = NPC.spriteDirection = direction;

                    if (Vector2.Distance(vector5, NPC.Center) < 32f)
                        NPC.ai[1] = splittingFireballBreathTimer - 1f;
                }
                else if (NPC.ai[1] == splittingFireballBreathTimer)
                {
                    Vector2 vector6 = NPC.DirectionTo(targetData.Center);
                    vector6.Y *= 0.15f;
                    vector6 = vector6.SafeNormalize(Vector2.UnitX * NPC.direction);

                    NPC.spriteDirection = (vector6.X > 0f) ? 1 : -1;
                    NPC.rotation = vector6.ToRotation();

                    if (NPC.spriteDirection == -1)
                        NPC.rotation += pie;

                    NPC.velocity = vector6 * splittingFireballBreathPhaseVelocity;
                }
                else
                {
                    NPC.position.X += NPC.DirectionTo(targetData.Center).X * 7f;
                    NPC.position.Y += NPC.DirectionTo(targetData.Center + new Vector2(0f, -400f)).Y * 6f;

                    float xOffset = 30f;
                    Vector2 position = NPC.Center + new Vector2((110f + xOffset) * NPC.direction, -20f).RotatedBy(NPC.rotation);
                    int num34 = (int)(NPC.ai[1] - splittingFireballBreathTimer + 1f);

					int type = ModContent.ProjectileType<YharonFireball>();
					int damage = NPC.GetProjectileDamage(type);
					if (num34 <= splittingFireballBreathTimer2 && num34 % splittingFireballBreathDivisor == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), position, NPC.velocity, type, damage, 0f, Main.myPlayer, 0f, 0f);
                }

                if (NPC.ai[1] > splittingFireballBreathPhaseTimer - splittingFireballBreathYVelocityTimer)
                    NPC.velocity.Y -= 0.1f;

                NPC.ai[1] += 1f;
                if (NPC.ai[1] >= splittingFireballBreathPhaseTimer)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                }
            }

			// Fireball spin
            else if (NPC.ai[0] == 5f)
            {
				NPC.velocity = NPC.velocity.RotatedBy(-(double)spinRotation * (float)NPC.direction);
                NPC.rotation -= spinRotation * NPC.direction;

				if (NPC.ai[1] == 1f)
				{
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);
					flareDustBulletHellSpawn = NPC.Center + NPC.velocity.RotatedBy(MathHelper.PiOver2 * -NPC.direction) * spinTime / (MathHelper.TwoPi * 3f);
				}

				NPC.ai[1] += 1f;
				if (Main.netMode != NetmodeID.MultiplayerClient)
                {
					if (secondPhasePhase >= 3)
					{
						// Rotate spiral by 9 * (240 / 12) = +90 degrees and then back -90 degrees

						// For phase 4: Rotate spiral by 18 * (240 / 16) = +135 degrees and then back -135 degrees

						if (NPC.ai[1] % flareDustSpawnDivisor2 == 0f)
						{
							int totalProjectiles = secondPhasePhase == 4 ? 12 : 10;
							float projectileVelocity = secondPhasePhase == 4 ? 16f : 12f;
							float radialOffset = secondPhasePhase == 4 ? 2.8f : 3.2f;
							DoFlareDustBulletHell(1, spinPhaseTimer, NPC.GetProjectileDamage(ModContent.ProjectileType<FlareDust>()), totalProjectiles, projectileVelocity, radialOffset, true);
						}
					}
					else
					{
						if (NPC.ai[1] % flareDustSpawnDivisor == 0f)
						{
							int ringReduction = (int)MathHelper.Lerp(0f, 18f, NPC.ai[1] / spinPhaseTimer);
							int totalProjectiles = (secondPhasePhase == 2 ? 42 : 38) - ringReduction; // 36 for first ring, 18 for last ring
							DoFlareDustBulletHell(0, spinPhaseTimer, NPC.GetProjectileDamage(ModContent.ProjectileType<FlareDust>()), totalProjectiles, 0f, 0f, true);
						}
					}

					if (NPC.ai[1] == 210f && secondPhasePhase == 4 && useTornado)
                    {
                        useTornado = false;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<BigFlare2>(), 0, 0f, Main.myPlayer, 1f, NPC.target + 1);
                    }
                }

                if (NPC.ai[1] >= spinPhaseTimer)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = increasedIdleTimeAfterBulletHell;
                    NPC.ai[2] = 0f;
					NPC.localAI[2] = 0f;
					NPC.velocity /= 2f;
                }
            }

			// Flare spawn and fire ring
            else if (NPC.ai[0] == 6f)
            {
                if (NPC.ai[1] == 0f)
                {
                    Vector2 destination2 = targetData.Center + new Vector2(0f, -200f);
                    Vector2 desiredVelocity2 = NPC.DirectionTo(destination2) * velocity * 1.5f;
                    NPC.SimpleFlyMovement(desiredVelocity2, acceleration * 1.5f);

                    int num35 = (NPC.Center.X < targetData.Center.X) ? 1 : -1;
                    NPC.direction = NPC.spriteDirection = num35;

                    NPC.ai[2] += 1f;
                    if (NPC.Distance(targetData.Center) < 600f || NPC.ai[2] >= 180f)
                    {
                        NPC.ai[1] = 1f;
                        NPC.netUpdate = true;
                    }
                }
                else
                {
                    if (NPC.ai[1] < flareSpawnDecelerationTimer)
                        NPC.velocity *= 0.95f;
                    else
                        NPC.velocity *= 0.98f;

                    if (NPC.ai[1] == flareSpawnDecelerationTimer)
                    {
                        if (NPC.velocity.Y > 0f)
                            NPC.velocity.Y /= 3f;

                        NPC.velocity.Y -= 3f;
                    }

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
						if (NPC.ai[1] == 20f || NPC.ai[1] == 80f || NPC.ai[1] == 140f)
						{
							SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoarShort"), NPC.position);

							if (expertMode)
								DoFireRing(300, NPC.GetProjectileDamage(ModContent.ProjectileType<FlareBomb>()), NPC.target, 1f);

							Vector2 vector7 = NPC.Center + (MathHelper.TwoPi * Main.rand.NextFloat()).ToRotationVector2() * new Vector2(2f, 1f) * 100f * (0.6f + Main.rand.NextFloat() * 0.4f);

							if (Vector2.Distance(vector7, targetData.Center) > 150f)
								SpawnDetonatingFlares(vector7, targetData, 6, new int[] { ModContent.NPCType<DetonatingFlare>(), ModContent.NPCType<DetonatingFlare2>() });
						}
                    }

                    NPC.ai[1] += 1f;
                }

                if (NPC.ai[1] >= flareSpawnPhaseTimer)
                {
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoarShort"), NPC.position);

					if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<BigFlare2>(), 0, 0f, Main.myPlayer, 1f, NPC.target + 1);

                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                }
            }

			// Fast charge
            else if (NPC.ai[0] == 7f)
            {
                if (NPC.ai[1] == 1f)
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoarShort"), NPC.position);

				ChargeDust(14, pie);

				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= chargeTime)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                }
            }

			// Teleport
            else if (NPC.ai[0] == 8f)
            {
                Vector2 npcCenter = NPC.Center;

                if (NPC.alpha < 255)
                {
                    NPC.alpha += 17;
                    if (NPC.alpha > 255)
                        NPC.alpha = 255;
                }

                NPC.velocity *= 0.98f;
                NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0f, 0.02f);

                if (NPC.ai[2] == 15f)
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoarShort"), NPC.position);

                if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[2] == 15f)
                {
                    if (NPC.ai[1] == 0f)
                        NPC.ai[1] = 450 * Math.Sign((npcCenter - targetData.Center).X);

                    teleportLocation = Main.rand.NextBool(2) ? (revenge ? 500 : 600) : (revenge ? -500 : -600);
                    Vector2 center = targetData.Center + new Vector2(-NPC.ai[1], teleportLocation);
                    npcCenter = NPC.Center = center;
                }

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= teleportPhaseTimer)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
					NPC.localAI[1] = fastChargeTelegraphTimer + 1f;
					NPC.netUpdate = true;
                }
            }

			// Enter new phase
            else if (NPC.ai[0] == 9f)
            {
                NPC.velocity *= 0.95f;

                Vector2 vector = NPC.DirectionTo(targetData.Center);
                NPC.spriteDirection = (vector.X > 0f) ? 1 : -1;
                NPC.rotation = vector.ToRotation();

                if (NPC.spriteDirection == -1)
                    NPC.rotation += pie;

                if (NPC.ai[2] == 120f)
                {
                    if (secondPhasePhase == 4)
                    {
                        for (int x = 0; x < 1000; x++)
                        {
                            Projectile projectile = Main.projectile[x];
                            if (projectile.active)
                            {
                                if (projectile.type == ModContent.ProjectileType<Infernado2>())
                                {
                                    if (projectile.timeLeft >= 300)
                                        projectile.active = false;
                                    else if (projectile.timeLeft > 5)
                                        projectile.timeLeft = (int)(5f * projectile.ai[1]);
                                }
                                else if (projectile.type == ModContent.ProjectileType<BigFlare2>())
                                    projectile.active = false;
                            }
                        }
                    }

                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/YharonRoar"), NPC.position);
                }

                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= 180f)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                    NPC.netUpdate = true;
                }
            }

            float num42 = NPC.DirectionTo(targetData.Center).ToRotation();
            float num43 = 0.04f;

            switch ((int)NPC.ai[0])
            {
                case 2:
                case 5:
                case 7:
                case 8:
                case 9:
                    num43 = 0f;
                    break;
                case 3:
                    num43 = 0.01f;
                    num42 = 0f;

                    if (NPC.spriteDirection == -1)
                        num42 -= pie;

                    if (NPC.ai[1] >= fireballBreathTimer)
                    {
                        num42 += NPC.spriteDirection * pie / 12f;
                        num43 = 0.05f;
                    }

                    break;
                case 4:
                    num43 = 0.01f;
                    num42 = pie;

                    if (NPC.spriteDirection == 1)
                        num42 += pie;

                    break;
                case 6:
                    num43 = 0.02f;
                    num42 = 0f;

                    if (NPC.spriteDirection == -1)
                        num42 -= pie;

                    break;
            }

            if (NPC.spriteDirection == -1)
                num42 += pie;

            if (num43 != 0f)
                NPC.rotation = NPC.rotation.AngleTowards(num42, num43);
        }
		#endregion

		#region Charge Dust
		private void ChargeDust(int dustAmt, float pie)
		{
			for (int num1474 = 0; num1474 < dustAmt; num1474++)
			{
				Vector2 vector171 = Vector2.Normalize(NPC.velocity) * new Vector2((NPC.width + 50) / 2f, NPC.height) * 0.75f;
				vector171 = vector171.RotatedBy((num1474 - (dustAmt / 2 - 1)) * (double)pie / (float)dustAmt) + NPC.Center;
				Vector2 value18 = ((float)(Main.rand.NextDouble() * pie) - MathHelper.PiOver2).ToRotationVector2() * Main.rand.Next(3, 8);
				int num1475 = Dust.NewDust(vector171 + value18, 0, 0, 244, value18.X * 2f, value18.Y * 2f, 100, default, 1.4f);
				Main.dust[num1475].noGravity = true;
				Main.dust[num1475].noLight = true;
				Main.dust[num1475].velocity /= 4f;
				Main.dust[num1475].velocity -= NPC.velocity;
			}
		}
		#endregion

		#region Spawn Detonating Flares
		private void SpawnDetonatingFlares(Vector2 origin, Player target, int maxFlareCount, int[] flareTypes)
		{
			if (flareTypes.Length > 1)
			{
				if (NPC.CountNPCS(flareTypes[0]) + NPC.CountNPCS(flareTypes[1]) < maxFlareCount)
					SpawnFlares(flareTypes[0], flareTypes[1]);
			}
			else
			{
				if (NPC.CountNPCS(flareTypes[0]) < maxFlareCount)
					SpawnFlares(flareTypes[0]);
			}

			void SpawnFlares(int type1 = 0, int type2 = 0)
			{
				int type = type2 == 0 ? type1 : Main.rand.NextBool(2) ? type1 : type2;
				int npc = NPC.NewNPC(NPC.GetSource_FromThis(), (int)origin.X, (int)origin.Y, type, 0, 0f, 0f, 0f, 0f, 255);
				Main.npc[npc].velocity = target.Center - origin;
				Main.npc[npc].velocity.Normalize();
				Main.npc[npc].velocity *= BossRushEvent.BossRushActive ? 15f : 10f;
				Main.npc[npc].netUpdate = true;
			}
		}
		#endregion

		#region Flare Dust Bullet Hell
		private void DoFlareDustBulletHell(int attackType, int timer, int projectileDamage, int totalProjectiles, float projectileVelocity, float radialOffset, bool phase2)
		{
			SoundEngine.PlaySound(SoundID.Item20, flareDustBulletHellSpawn);
			float aiVariableUsed = phase2 ? NPC.ai[1] : NPC.ai[2];
			switch (attackType)
			{
				case 0:
					float offsetAngle = 360 / totalProjectiles;
					int totalSpaces = totalProjectiles / 5;
					int spaceStart = Main.rand.Next(totalProjectiles - totalSpaces);
					float ai0 = aiVariableUsed % (timer * 2) == 0f ? 1f : 0f;

					int spacesMade = 0;
					for (int i = 0; i < totalProjectiles; i++)
					{
						if (i >= spaceStart && spacesMade < totalSpaces)
							spacesMade++;
						else
							Projectile.NewProjectile(NPC.GetSource_FromThis(), flareDustBulletHellSpawn, Vector2.Zero, ModContent.ProjectileType<FlareDust>(), projectileDamage, 0f, Main.myPlayer, ai0, i * offsetAngle);
					}
					break;

				case 1:
					double radians = MathHelper.TwoPi / totalProjectiles;
					Vector2 spinningPoint = Vector2.Normalize(new Vector2(-NPC.localAI[2], -projectileVelocity));

					for (int i = 0; i < totalProjectiles; i++)
					{
						Vector2 vector2 = spinningPoint.RotatedBy(radians * i) * projectileVelocity;
						Projectile.NewProjectile(NPC.GetSource_FromThis(), flareDustBulletHellSpawn, vector2, ModContent.ProjectileType<FlareDust>(), projectileDamage, 0f, Main.myPlayer, 2f, 0f);
					}

					float newRadialOffset = (int)aiVariableUsed / (timer / 4) % 2f == 0f ? radialOffset : -radialOffset;
					NPC.localAI[2] += newRadialOffset;
					break;

				default:
					break;
			}
		}
		#endregion

		#region Fire Ring
		public void DoFireRing(int timeLeft, int damage, float ai0, float ai1)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				float velocity = ai1 == 0f ? 10f : 5f;
				int totalProjectiles = 50;
				float radians = MathHelper.TwoPi / totalProjectiles;
				for (int i = 0; i < totalProjectiles; i++)
				{
					Vector2 vector255 = new Vector2(0f, -velocity).RotatedBy(radians * i);
					int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vector255, ModContent.ProjectileType<FlareBomb>(), damage, 0f, Main.myPlayer, ai0, ai1);
					Main.projectile[proj].timeLeft = timeLeft;
				}
			}
		}
		#endregion

		#region Drawing
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			bool idlePhases = (!startSecondAI && (NPC.ai[0] == 0f || NPC.ai[0] == 6f || NPC.ai[0] == 13f)) || (startSecondAI && (NPC.ai[0] == 5f || NPC.ai[0] < 2f));

			bool chargingOrSpawnPhases = (!startSecondAI && (NPC.ai[0] == 1f || NPC.ai[0] == 5f || NPC.ai[0] == 7f || NPC.ai[0] == 11f || NPC.ai[0] == 14f || NPC.ai[0] == 18f)) ||
				(startSecondAI && (NPC.ai[0] == 6f || NPC.ai[0] == 2f || NPC.ai[0] == 7f));

			bool projectileOrCirclePhases = (!startSecondAI && (NPC.ai[0] == 2f || NPC.ai[0] == 8f || NPC.ai[0] == 12f || NPC.ai[0] == 15f || NPC.ai[0] == 19f || NPC.ai[0] == 20f)) ||
				(startSecondAI && (NPC.ai[0] == 4f || NPC.ai[0] == 3f));

			bool tornadoPhase = !startSecondAI && (NPC.ai[0] == 3f || NPC.ai[0] == 9f || NPC.ai[0] == -1f || NPC.ai[0] == 16f);

			bool newPhasePhase = (!startSecondAI && (NPC.ai[0] == 4f || NPC.ai[0] == 10f || NPC.ai[0] == 17f)) || (startSecondAI && NPC.ai[0] == 9f);

			bool pauseAfterTeleportPhase = startSecondAI && NPC.ai[0] == 8f;

			bool ai2 = startSecondAI && !phaseOneLoot;

			SpriteEffects spriteEffects = ai2 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = ai2 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Texture2D texture = TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[NPC.type] / 2);
			Color color = drawColor;
			Color invincibleColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
			Color color36 = Color.White;

			float amount9 = 0f;
			bool invincible = ai2 && invincibilityCounter < 900;
			bool flag8 = NPC.ai[0] > 5f;
			bool flag9 = NPC.ai[0] > 12f;
			bool flag10 = startSecondAI;
			int num150 = 120;
			int num151 = 60;

			if (flag10)
				color = CalamityGlobalNPC.buffColor(color, 0.9f, 0.7f, 0.3f, 1f);
			else if (flag9)
				color = CalamityGlobalNPC.buffColor(color, 0.8f, 0.7f, 0.4f, 1f);
			else if (flag8)
				color = CalamityGlobalNPC.buffColor(color, 0.7f, 0.7f, 0.5f, 1f);
			else if (NPC.ai[0] == 4f && NPC.ai[2] > num150)
			{
				float num152 = NPC.ai[2] - num150;
				num152 /= num151;
				color = CalamityGlobalNPC.buffColor(color, 1f - 0.3f * num152, 1f - 0.3f * num152, 1f - 0.5f * num152, 1f);
			}

			int num153 = 10;
			int num154 = 2;
			if (NPC.ai[0] == -1f)
				num153 = 0;
			if (idlePhases)
				num153 = 7;

			if (invincible)
				color36 = invincibleColor;
			else if (chargingOrSpawnPhases)
			{
				color36 = Color.Red;
				amount9 = 0.5f;
			}
			else
				color = drawColor;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += num154)
				{
					Color color38 = color;
					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
					vector41 -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			int num156 = 0;
			float num157 = 0f;
			float scaleFactor9 = 0f;

			if (NPC.ai[0] == -1f)
				num156 = 0;

			if (tornadoPhase)
			{
				int num158 = 60;
				int num159 = 30;
				if (NPC.ai[2] > num158)
				{
					num156 = 6;
					num157 = 1f - (float)Math.Cos((NPC.ai[2] - num158) / num159 * MathHelper.TwoPi);
					num157 /= 3f;
					scaleFactor9 = 40f;
				}
			}

			if (newPhasePhase && NPC.ai[2] > num150)
			{
				num156 = 6;
				num157 = 1f - (float)Math.Cos((NPC.ai[2] - num150) / num151 * MathHelper.TwoPi);
				num157 /= 3f;
				scaleFactor9 = 60f;
			}

			if (pauseAfterTeleportPhase)
			{
				num156 = 6;
				num157 = 1f - (float)Math.Cos(NPC.ai[2] / 30f * MathHelper.TwoPi);
				num157 /= 3f;
				scaleFactor9 = 20f;
			}

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num160 = 0; num160 < num156; num160++)
				{
					Color color39 = drawColor;
					color39 = Color.Lerp(color39, color36, amount9);
					color39 = NPC.GetAlpha(color39);
					color39 *= 1f - num157;
					Vector2 vector42 = NPC.Center + (num160 / (float)num156 * MathHelper.TwoPi + NPC.rotation).ToRotationVector2() * scaleFactor9 * num157 - screenPos;
					vector42 -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector42 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture, vector42, NPC.frame, color39, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - screenPos;
			vector43 -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture, vector43, NPC.frame, (invincible ? invincibleColor : NPC.GetAlpha(drawColor)), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			if (flag8 || NPC.ai[0] == 4f || startSecondAI)
			{
				texture = ModContent.Request<Texture2D>("CalRD/NPCs/Yharon/YharonGlowOrange").Value;
				Color color40 = Color.Lerp(Color.White, (invincible ? invincibleColor : Color.Orange), 0.5f);
				color36 = invincible ? invincibleColor : Color.Orange;

				Texture2D texture2 = ModContent.Request<Texture2D>("CalRD/NPCs/Yharon/YharonGlowGreen").Value;
				Color color43 = Color.Lerp(Color.White, (invincible ? invincibleColor : Color.Chartreuse), 0.5f);
				Color color44 = invincible ? invincibleColor : Color.Chartreuse;

				Texture2D texture3 = ModContent.Request<Texture2D>("CalRD/NPCs/Yharon/YharonGlowPurple").Value;
				Color color45 = Color.Lerp(Color.White, (invincible ? invincibleColor : Color.BlueViolet), 0.5f);
				Color color46 = invincible ? invincibleColor : Color.BlueViolet;

				amount9 = 1f;
				num157 = 0.5f;
				scaleFactor9 = 10f;
				num154 = 1;

				if (newPhasePhase)
				{
					float num161 = NPC.ai[2] - num150;
					num161 /= num151;
					color36 *= num161;
					color40 *= num161;

					if (flag9 || NPC.ai[0] == 10f || startSecondAI)
					{
						color43 *= num161;
						color44 *= num161;
					}

					if (flag10 || NPC.ai[0] == 17f)
					{
						color45 *= num161;
						color46 *= num161;
					}
				}

				if (pauseAfterTeleportPhase)
				{
					float num162 = NPC.ai[2];
					num162 /= 30f;

					if (num162 > 0.5f)
						num162 = 1f - num162;

					num162 *= 2f;
					num162 = 1f - num162;
					color36 *= num162;
					color40 *= num162;
					color43 *= num162;
					color44 *= num162;
					color45 *= num162;
					color46 *= num162;
				}

				if (CalamityConfig.Instance.Afterimages)
				{
					for (int num163 = 1; num163 < num153; num163 += num154)
					{
						Color color41 = color40;
						color41 = Color.Lerp(color41, color36, amount9);
						color41 *= (num153 - num163) / 15f;
						Vector2 vector44 = NPC.oldPos[num163] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
						vector44 -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
						vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
						spriteBatch.Draw(texture, vector44, NPC.frame, color41, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

						if (flag9 || NPC.ai[0] == 10f || startSecondAI)
						{
							Color color47 = color43;
							color47 = Color.Lerp(color47, color44, amount9);
							color47 *= (num153 - num163) / 15f;
							spriteBatch.Draw(texture2, vector44, NPC.frame, color47, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
						}

						if (flag10 || NPC.ai[0] == 17f)
						{
							Color color48 = color45;
							color48 = Color.Lerp(color48, color46, amount9);
							color48 *= (num153 - num163) / 15f;
							spriteBatch.Draw(texture3, vector44, NPC.frame, color48, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
						}
					}

					for (int num164 = 1; num164 < num156; num164++)
					{
						Color color42 = color40;
						color42 = Color.Lerp(color42, color36, amount9);
						color42 = NPC.GetAlpha(color42);
						color42 *= 1f - num157;
						Vector2 vector45 = NPC.Center + (num164 / (float)num156 * MathHelper.TwoPi + NPC.rotation).ToRotationVector2() * scaleFactor9 * num157 - screenPos;
						vector45 -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
						vector45 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
						spriteBatch.Draw(texture, vector45, NPC.frame, color42, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

						if (flag9 || NPC.ai[0] == 10f || startSecondAI)
						{
							Color color49 = color43;
							color49 = Color.Lerp(color49, color44, amount9);
							color49 = NPC.GetAlpha(color49);
							color49 *= 1f - num157;
							spriteBatch.Draw(texture2, vector45, NPC.frame, color49, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
						}

						if (flag10 || NPC.ai[0] == 17f)
						{
							Color color50 = color45;
							color50 = Color.Lerp(color50, color46, amount9);
							color50 = NPC.GetAlpha(color50);
							color50 *= 1f - num157;
							spriteBatch.Draw(texture3, vector45, NPC.frame, color50, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
						}
					}
				}

				spriteBatch.Draw(texture, vector43, NPC.frame, color40, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

				if (flag9 || NPC.ai[0] == 10f || startSecondAI)
					spriteBatch.Draw(texture2, vector43, NPC.frame, color43, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

				if (flag10 || NPC.ai[0] == 17f)
					spriteBatch.Draw(texture3, vector43, NPC.frame, color45, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
			}

            return false;
        }
        #endregion

        #region Loot
        public override bool SpecialOnKill()
        {
            return !dropLoot;
        }

        public override void OnKill()
        {
            // If Yharon runs away in phase 1 and the Eclipse isn't buffed yet, notify players of the buffed Solar Eclipse
            if (!startSecondAI && !CalamityWorld.buffedEclipse)
            {
                CalamityWorld.buffedEclipse = true;
                CalamityNetcode.SyncWorld();

                string key = "The dark sun awaits.";
                Color messageColor = Color.Orange;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }

			CalamityGlobalTownNPC.SetNewShopVariable(new int[] { ModContent.NPCType<THIEF>() }, CalamityWorld.buffedEclipse);

            // Bags occur in either phase 1 or 2, as they don't contain phase 2 only drops
            DropHelper.DropBags(ModContent.ItemType<YharonBag>(), NPC);

            // Phase 1 drops: Contained in the bag, so they only drop directly on Normal
            if (!Main.expertMode)
            {
                // Weapons
                float w = DropHelper.DirectWeaponDropRateFloat;
                DropHelper.DropEntireWeightedSet(NPC.GetSource_FromThis(), NPC,
                    DropHelper.WeightStack<DragonRage>(w),
                    DropHelper.WeightStack<TheBurningSky>(w),
                    DropHelper.WeightStack<DragonsBreath>(w),
                    DropHelper.WeightStack<ChickenCannon>(w),
                    DropHelper.WeightStack<PhoenixFlameBarrage>(w),
                    DropHelper.WeightStack<AngryChickenStaff>(w), // Yharon Kindle Staff
                    DropHelper.WeightStack<ProfanedTrident>(w), // Infernal Spear
                    DropHelper.WeightStack<FinalDawn>(w)
                );

                // Vanity
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<YharonMask>(), 7);
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<ForgottenDragonEgg>(), 10);
            }

            // These drops only occur in Phase 2 (where you actually kill Yharon)
            if (startSecondAI && !phaseOneLoot)
            {
                // Materials
                int soulFragMin = Main.expertMode ? 22 : 15;
                int soulFragMax = Main.expertMode ? 28 : 22;
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<HellcasterFragment>(), true, soulFragMin, soulFragMax);

                // Equipment
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<DrewsWings>(), Main.expertMode);

                // Weapons
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<VoidVortex>(), Main.expertMode, DropHelper.RareVariantDropRateInt);
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<YharimsCrystal>(), Main.expertMode, 100); //not affected by defiled and not a leggie

                // Vanity
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<YharonTrophy>(), 10);

                // Other
                //DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<BossRush>());
                DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeYharon>(), true, !CalamityWorld.downedYharon);
                DropHelper.DropResidentEvilAmmo(NPC.GetSource_FromThis(), NPC, CalamityWorld.downedYharon, 6, 3, 2);

                // If Yharon has not been killed yet, notify players of Auric Ore
                if (!CalamityWorld.downedYharon)
                {
                    WorldGenerationMethods.SpawnOre(ModContent.TileType<AuricOre>(), 2E-05, .6f, .8f);

                    string key = "A godly aura has blessed the world's caverns.";
                    Color messageColor = Color.Gold;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                }

                // Mark Yharon as dead
                CalamityWorld.downedYharon = true;
                CalamityNetcode.SyncWorld();
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ModContent.ItemType<OmegaHealingPotion>();
        }
        #endregion

        #region Strike NPC
        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
			// Safeguard if damage would kill phase 1 before phase 2.
			modifiers.ModifyHitInfo += Safeguard;

            // Safeguard to prevent damage which would allow skipping phase 2.
            if (!startSecondAI && dropLoot)
	            modifiers.SetMaxDamage(0);
        }

        public void Safeguard(ref NPC.HitInfo hit)
        {
	        if (phaseOneLoot && (hit.Damage >= NPC.life || (hit.Crit && hit.Damage * 2 >= NPC.life)))
	        {
		        float lifeAboveTenPercent = NPC.life - NPC.lifeMax * 0.1f;
		        hit.Damage = (int)MathHelper.Clamp((float)hit.Damage, 0f, lifeAboveTenPercent);
	        }
        }
        #endregion

        #region HP Bar Cooldown Slot and Stats
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2f;
            return null;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }
        #endregion

        #region Find Frame
        public override void FindFrame(int frameHeight)
        {
			bool idlePhases = (!startSecondAI && (NPC.ai[0] == 0f || NPC.ai[0] == 6f || NPC.ai[0] == 13f)) || (startSecondAI && (NPC.ai[0] == 5f || NPC.ai[0] < 2f));

			bool chargingOrSpawnPhases = (!startSecondAI && (NPC.ai[0] == 1f || NPC.ai[0] == 5f || NPC.ai[0] == 7f || NPC.ai[0] == 11f || NPC.ai[0] == 14f || NPC.ai[0] == 18f)) ||
				(startSecondAI && (NPC.ai[0] == 6f || NPC.ai[0] == 2f || NPC.ai[0] == 7f));

			bool projectileOrCirclePhases = (!startSecondAI && (NPC.ai[0] == 2f || NPC.ai[0] == 8f || NPC.ai[0] == 12f || NPC.ai[0] == 15f || NPC.ai[0] == 19f || NPC.ai[0] == 20f)) ||
				(startSecondAI && (NPC.ai[0] == 4f || NPC.ai[0] == 3f || NPC.ai[0] == 8f));

			bool tornadoPhase = !startSecondAI && (NPC.ai[0] == 3f || NPC.ai[0] == 9f || NPC.ai[0] == -1f || NPC.ai[0] == 16f);

			bool newPhasePhase = (!startSecondAI && (NPC.ai[0] == 4f || NPC.ai[0] == 10f || NPC.ai[0] == 17f)) || (startSecondAI && NPC.ai[0] == 9f);

			bool chargeTelegraph = (!startSecondAI && (NPC.ai[0] == 0f || NPC.ai[0] == 6f || NPC.ai[0] == 13f) && NPC.localAI[1] > 0f) ||
				(startSecondAI && NPC.ai[0] < 2f && NPC.localAI[1] > 0f);

			if (chargeTelegraph)
			{
				bool phase4 = startSecondAI && NPC.life <= NPC.lifeMax * 0.15 && (CalamityWorld.revenge || BossRushEvent.BossRushActive);

				int num86 = phase4 ? 60 : 120;
				if (NPC.localAI[1] < num86 - (phase4 ? 30 : 60) || NPC.localAI[1] > num86 - (phase4 ? 10 : 20))
				{
					NPC.frameCounter += phase4 ? 2D : 1D;
					if (NPC.frameCounter > 5D)
					{
						NPC.frameCounter = 0D;
						NPC.frame.Y += frameHeight;
					}
					if (NPC.frame.Y >= frameHeight * 5)
					{
						NPC.frame.Y = 0;
					}
				}
				else
				{
					NPC.frame.Y = frameHeight * 5;
					if (NPC.localAI[1] > num86 - (phase4 ? 25 : 50) && NPC.localAI[1] < num86 - (phase4 ? 12 : 25))
					{
						NPC.frame.Y = frameHeight * 6;
					}
				}
				return;
			}

			if (idlePhases)
            {
                int num84 = 5;
                if (!startSecondAI && (NPC.ai[0] == 6f || NPC.ai[0] == 13f))
                {
                    num84 = 4;
                }
                NPC.frameCounter += 1D;
                if (NPC.frameCounter > num84)
                {
                    NPC.frameCounter = 0D;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y >= frameHeight * 5)
                {
                    NPC.frame.Y = 0;
                }
            }

            if (chargingOrSpawnPhases)
                NPC.frame.Y = frameHeight * 5;

            if (projectileOrCirclePhases)
                NPC.frame.Y = frameHeight * 5;

            if (tornadoPhase)
            {
                int num85 = 90;
                if (NPC.ai[2] < num85 - 30 || NPC.ai[2] > num85 - 10)
                {
                    NPC.frameCounter += 1D;
                    if (NPC.frameCounter > 5D)
                    {
                        NPC.frameCounter = 0D;
                        NPC.frame.Y += frameHeight;
                    }
                    if (NPC.frame.Y >= frameHeight * 5)
                    {
                        NPC.frame.Y = 0;
                    }
                }
                else
                {
                    NPC.frame.Y = frameHeight * 5;
                    if (NPC.ai[2] > num85 - 20 && NPC.ai[2] < num85 - 15)
                    {
                        NPC.frame.Y = frameHeight * 6;
                    }
                }
            }

            if (newPhasePhase)
            {
                int num86 = 180;
                if (NPC.ai[2] < num86 - 60 || NPC.ai[2] > num86 - 20)
                {
                    NPC.frameCounter += 1D;
                    if (NPC.frameCounter > 5D)
                    {
                        NPC.frameCounter = 0D;
                        NPC.frame.Y += frameHeight;
                    }
                    if (NPC.frame.Y >= frameHeight * 5)
                    {
                        NPC.frame.Y = 0;
                    }
                }
                else
                {
                    NPC.frame.Y = frameHeight * 5;
                    if (NPC.ai[2] > num86 - 50 && NPC.ai[2] < num86 - 25)
                    {
                        NPC.frame.Y = frameHeight * 6;
                    }
                }
            }
        }
        #endregion

        #region Hit Effect
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                DoFireRing(300, (Main.expertMode || BossRushEvent.BossRushActive) ? 125 : 150, -1f, 0f);
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 300;
                NPC.height = 280;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 244, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 244, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 244, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }
        #endregion
    }
}
