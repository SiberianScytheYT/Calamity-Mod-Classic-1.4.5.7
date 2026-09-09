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
using CalRD.Projectiles.Boss;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.Leviathan
{
    [AutoloadBossHead]
    public class Leviathan : ModNPC
    {
        int counter = 0;
        bool initialised = false;
		int soundDelay = 0;
        public static Asset<Texture2D> AttackTexture = null;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Leviathan");
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            if (!Main.dedServ)
                AttackTexture = ModContent.Request<Texture2D>("CalRD/NPCs/Leviathan/LeviathanAttack", AssetRequestMode.AsyncLoad);
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
	        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
	        {
		        BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
		        new FlavorTextBestiaryInfoElement("Theorized to be the very last of her kind, this reptile only awakens if her slumber is disturbed.")
	        });
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 20f;
			NPC.GetNPCDamage();
			NPC.width = 650;
            NPC.height = 300;
            NPC.defense = 40;
			NPC.DR_NERD(0.35f);
            NPC.LifeMaxNERB(55200, 72560, 6000000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;
			NPC.Opacity = 0f;
			NPC.value = Item.buyPrice(0, 15, 0, 0);
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
			NPC.buffImmune[BuffID.BoneJavelin] = false;
			NPC.buffImmune[BuffID.SoulDrain] = false;
			NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<HolyFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Plague>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WarCleave>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.buffImmune[ModContent.BuffType<SulphuricPoisoning>()] = false;
            NPC.HitSound = SoundID.NPCHit56;
            NPC.DeathSound = SoundID.NPCDeath60;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.netAlways = true;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/LeviathanAndSiren");
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.dontTakeDamage);
            writer.Write(soundDelay);
            writer.Write(NPC.Calamity().newAI[3]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.dontTakeDamage = reader.ReadBoolean();
            soundDelay = reader.ReadInt32();
            NPC.Calamity().newAI[3] = reader.ReadSingle();
        }

        public override void AI()
        {
			CalamityGlobalNPC calamityGlobalNPC = NPC.Calamity();

            CalamityGlobalNPC.leviathan = NPC.whoAmI;

			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
            bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
            Vector2 vector = NPC.Center;

			// Is in spawning animation
			float spawnAnimationTime = 180f;
			bool spawnAnimation = calamityGlobalNPC.newAI[3] < spawnAnimationTime;

			// Don't do damage during spawn animation
			NPC.damage = NPC.defDamage;
			if (spawnAnimation)
				NPC.damage = 0;

			// Percent life remaining
			float lifeRatio = NPC.life / (float)NPC.lifeMax;

			// Phases
			bool phase2 = lifeRatio < 0.7f && expertMode;
			bool phase3 = lifeRatio < 0.4f;
			bool phase4 = lifeRatio < 0.2f;

			NPC.spriteDirection = (NPC.direction > 0) ? 1 : -1;

            bool sirenAlive = false;
            if (CalamityGlobalNPC.siren != -1)
                sirenAlive = Main.npc[CalamityGlobalNPC.siren].active;

			if (CalamityGlobalNPC.siren != -1)
			{
				if (Main.npc[CalamityGlobalNPC.siren].active)
				{
					if (Main.npc[CalamityGlobalNPC.siren].damage == 0)
					{
						sirenAlive = false;
					}
				}
			}

			SoundStyle soundChoiceRage = SoundID.Zombie92;
			SoundStyle soundChoice = Utils.SelectRandom(Main.rand, new SoundStyle[]
			{
				SoundID.Zombie38,
				SoundID.Zombie39,
				SoundID.Zombie40
			});

			if (soundDelay > 0)
				soundDelay--;

            if (Main.rand.NextBool(600) && !spawnAnimation)
                SoundEngine.PlaySound((sirenAlive && !death) ? soundChoice : soundChoiceRage, NPC.Center);

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest(true);

            Player player = Main.player[NPC.target];

            bool notOcean = player.position.Y < 800f || player.position.Y > Main.worldSurface * 16.0 || (player.position.X > 6400f && player.position.X < (Main.maxTilesX * 16 - 6400));

			float enrageScale = 0f;
			if (notOcean)
				enrageScale += 2f;

			if (BossRushEvent.BossRushActive)
				enrageScale = 0f;

			NPC.dontTakeDamage = spawnAnimation;

            if (!player.active || player.dead || Vector2.Distance(player.Center, vector) > 5600f)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead || Vector2.Distance(player.Center, vector) > 5600f)
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
                            if (Main.npc[x].type == ModContent.NPCType<Siren>())
                            {
                                Main.npc[x].active = false;
                                Main.npc[x].netUpdate = true;
                            }
                        }
                        NPC.active = false;
                        NPC.netUpdate = true;
                    }

					if (NPC.ai[0] != 0f)
					{
						NPC.ai[0] = 0f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						NPC.netUpdate = true;
					}

					return;
                }
            }
            else
            {
				// Slowly drift up when spawning
				if (spawnAnimation)
				{
					float minSpawnVelocity = 0.4f;
					float maxSpawnVelocity = 4f;
					float velocityY = maxSpawnVelocity - MathHelper.Lerp(minSpawnVelocity, maxSpawnVelocity, calamityGlobalNPC.newAI[3] / spawnAnimationTime);
					NPC.velocity = new Vector2(0f, -velocityY);

					if (calamityGlobalNPC.newAI[3] == 10f)
						SoundEngine.PlaySound(soundChoiceRage, NPC.Center);

					NPC.Opacity = MathHelper.Clamp(calamityGlobalNPC.newAI[3] / spawnAnimationTime, 0f, 1f);

					calamityGlobalNPC.newAI[3] += 1f;

					return;
				}

				if (NPC.ai[0] == 0f)
                {
                    NPC.TargetClosest(true);

                    float num412 = (sirenAlive && !phase4) ? 3.5f : 7f;
                    float num413 = (sirenAlive && !phase4) ? 0.1f : 0.2f;
					num412 += 4f * enrageScale;
					num413 += 0.1f * enrageScale;

					if (expertMode && (!sirenAlive || phase4))
					{
						num412 += death ? 6f * (1f - lifeRatio) : 3.5f * (1f - lifeRatio);
						num413 += death ? 0.15f * (1f - lifeRatio) : 0.1f * (1f - lifeRatio);
					}

                    if (BossRushEvent.BossRushActive)
                    {
                        num412 *= 1.5f;
                        num413 *= 1.5f;
                    }

                    int num414 = 1;
                    if (NPC.Center.X < player.position.X + player.width)
                        num414 = -1;

                    Vector2 vector40 = NPC.Center;
                    float num415 = player.Center.X + (num414 * ((sirenAlive && !phase4) ? 1000f : 800f)) - vector40.X;
                    float num416 = player.Center.Y - vector40.Y;
                    float num417 = (float)Math.Sqrt(num415 * num415 + num416 * num416);
                    num417 = num412 / num417;
                    num415 *= num417;
                    num416 *= num417;

                    if (NPC.velocity.X < num415)
                    {
                        NPC.velocity.X += num413;
                        if (NPC.velocity.X < 0f && num415 > 0f)
                        {
                            NPC.velocity.X += num413;
                        }
                    }
                    else if (NPC.velocity.X > num415)
                    {
                        NPC.velocity.X -= num413;
                        if (NPC.velocity.X > 0f && num415 < 0f)
                        {
                            NPC.velocity.X -= num413;
                        }
                    }
                    if (NPC.velocity.Y < num416)
                    {
                        NPC.velocity.Y += num413;
                        if (NPC.velocity.Y < 0f && num416 > 0f)
                        {
                            NPC.velocity.Y += num413;
                        }
                    }
                    else if (NPC.velocity.Y > num416)
                    {
                        NPC.velocity.Y -= num413;
                        if (NPC.velocity.Y > 0f && num416 < 0f)
                        {
                            NPC.velocity.Y -= num413;
                        }
                    }

                    NPC.ai[1] += 1f;
					float phaseTimer = 240f;
					if (!sirenAlive || phase4)
						phaseTimer -= 120f * (1f - lifeRatio);

					if (NPC.ai[1] >= phaseTimer)
                    {
                        NPC.ai[0] = 1f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                        NPC.target = 255;
                        NPC.netUpdate = true;
                    }
                    else
                    {
                        if (!player.dead)
                        {
                            NPC.ai[2] += 1f;
                            if (!sirenAlive || phase4)
                                NPC.ai[2] += 2f;
                        }

                        if (NPC.ai[2] >= 75f)
                        {
                            NPC.ai[2] = 0f;
                            vector40 = new Vector2(NPC.Center.X, NPC.Center.Y + 20f);
                            num415 = player.Center.X - vector40.X;
                            num416 = player.Center.Y - vector40.Y;

                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                float speed = (sirenAlive && !phase4 && !death) ? 13.5f : 16f;
                                int type = ModContent.ProjectileType<LeviathanBomb>();
								int damage = NPC.GetProjectileDamage(type);

								if (expertMode)
                                    speed = (sirenAlive && !phase4 && !death) ? 14f : 17f;

								speed += 8f * enrageScale;

                                if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
                                    speed = 22f;

								if (!sirenAlive || phase4)
									speed += 3f * (1f - lifeRatio);

								if (BossRushEvent.BossRushActive)
                                    speed *= 1.5f;

                                num417 = (float)Math.Sqrt(num415 * num415 + num416 * num416);
                                num417 = speed / num417;
                                num415 *= num417;
                                num416 *= num417;
                                vector40.X += num415 * 4f;
                                vector40.Y += num416 * 4f;
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), vector40.X, vector40.Y, num415, num416, type, damage, 0f, Main.myPlayer, 0f, 0f);
								if (soundDelay <= 0)
								{
									soundDelay = 120;
									SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/LeviathanRoarMeteor"), NPC.Center);
								}
                            }
                        }
                    }
                }
                else if (NPC.ai[0] == 1f)
                {
                    NPC.TargetClosest(true);

                    Vector2 vector119 = new Vector2(NPC.Center.X, NPC.position.Y + NPC.height * 0.8f);
                    Vector2 vector120 = NPC.Center;
                    float num1058 = player.Center.X - vector120.X;
                    float num1059 = player.Center.Y - vector120.Y;
                    float num1060 = (float)Math.Sqrt(num1058 * num1058 + num1059 * num1059);

                    NPC.ai[1] += 1f;
					int num638 = 0;
					for (int num639 = 0; num639 < Main.maxPlayers; num639++)
					{
						if (Main.player[num639].active && !Main.player[num639].dead && (vector - Main.player[num639].Center).Length() < 1000f)
							num638++;
					}
					NPC.ai[1] += num638 / 2;

                    bool flag103 = false;
					float num640 = 60f;
					if (!sirenAlive || phase4)
						num640 -= 40f * (1f - lifeRatio);

					if (NPC.ai[1] > num640)
                    {
                        NPC.ai[1] = 0f;
                        NPC.ai[2] += 1f;
                        flag103 = true;
                    }

					int spawnLimit = (sirenAlive && !phase4) ? 1 : 3;
					bool spawnParasea = NPC.CountNPCS(ModContent.NPCType<Parasea>()) < spawnLimit;
					bool spawnAberration = (!sirenAlive || phase4) && !NPC.AnyNPCs(ModContent.NPCType<AquaticAberration>());

					if (flag103 && (spawnParasea || spawnAberration))
                    {
                        SoundEngine.PlaySound(soundChoice, NPC.Center);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
							int type = spawnAberration ? ModContent.NPCType<AquaticAberration>() : ModContent.NPCType<Parasea>();
							NPC.NewNPC(NPC.GetSource_FromThis(), (int)vector119.X, (int)vector119.Y, type);
						}
                    }

                    if (num1060 > ((sirenAlive && !phase4) ? 1000f : 800f))
                    {
                        float num1063 = (sirenAlive && !phase4) ? 7f : 8f;
                        float num1064 = (sirenAlive && !phase4) ? 0.05f : 0.065f;
						num1063 += 4f * enrageScale;
						num1064 += 0.04f * enrageScale;

						if (expertMode && (!sirenAlive || phase4))
						{
							num1063 += death ? 7f * (1f - lifeRatio) : 4f * (1f - lifeRatio);
							num1064 += death ? 0.05f * (1f - lifeRatio) : 0.03f * (1f - lifeRatio);
						}

						if (BossRushEvent.BossRushActive)
                        {
                            num1063 *= 1.5f;
                            num1064 *= 1.5f;
                        }

                        vector120 = vector119;
                        num1058 = player.Center.X - vector120.X;
                        num1059 = player.Center.Y - vector120.Y;
                        num1060 = (float)Math.Sqrt(num1058 * num1058 + num1059 * num1059);
                        num1060 = num1063 / num1060;

                        if (NPC.velocity.X < num1058)
                        {
                            NPC.velocity.X += num1064;
                            if (NPC.velocity.X < 0f && num1058 > 0f)
                                NPC.velocity.X += num1064;
                        }
                        else if (NPC.velocity.X > num1058)
                        {
                            NPC.velocity.X -= num1064;
                            if (NPC.velocity.X > 0f && num1058 < 0f)
                                NPC.velocity.X -= num1064;
                        }
                        if (NPC.velocity.Y < num1059)
                        {
                            NPC.velocity.Y += num1064;
                            if (NPC.velocity.Y < 0f && num1059 > 0f)
                                NPC.velocity.Y += num1064;
                        }
                        else if (NPC.velocity.Y > num1059)
                        {
                            NPC.velocity.Y -= num1064;
                            if (NPC.velocity.Y > 0f && num1059 < 0f)
                                NPC.velocity.Y -= num1064;
                        }
                    }
                    else
                        NPC.velocity *= 0.9f;

                    NPC.spriteDirection = NPC.direction;

                    if (NPC.ai[2] > ((sirenAlive && !phase4) ? 2f : 3f))
                    {
                        NPC.ai[0] = (((phase2 || phase3) && !sirenAlive) || phase4) ? 2f : 0f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.ai[0] == 2f)
                {
                    Vector2 distFromPlayer = player.Center - NPC.Center;
					float chargeAmt = death ? 2f : 1f;
                    if (NPC.ai[1] >= chargeAmt * 2f || distFromPlayer.Length() > 2400f)
                    {
                        NPC.ai[0] = 0f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                        NPC.netUpdate = true;
                        return;
                    }

					float chargeDistance = (sirenAlive && !phase4) ? 1100f : 900f;
					chargeDistance -= 150f * enrageScale;
					if (!sirenAlive || phase4)
						chargeDistance -= 250f * (1f - lifeRatio);

					if (NPC.ai[1] % 2f == 0f)
                    {
                        int num24 = 7;
                        for (int j = 0; j < num24; j++)
                        {
                            Vector2 arg_E1C_0 = (Vector2.Normalize(NPC.velocity) * new Vector2((NPC.width + 50) / 2f, NPC.height) * 0.75f).RotatedBy((j - (num24 / 2 - 1)) * MathHelper.Pi / num24) + vector;
                            Vector2 vector4 = ((float)(Main.rand.NextDouble() * MathHelper.Pi) - MathHelper.PiOver2).ToRotationVector2() * Main.rand.Next(3, 8);
                            int num25 = Dust.NewDust(arg_E1C_0 + vector4, 0, 0, 172, vector4.X * 2f, vector4.Y * 2f, 100, default, 1.4f);
                            Main.dust[num25].noGravity = true;
                            Main.dust[num25].noLight = true;
                            Main.dust[num25].velocity /= 4f;
                            Main.dust[num25].velocity -= NPC.velocity;
                        }

                        NPC.TargetClosest(true);

                        if (Math.Abs(NPC.position.Y + (NPC.height / 2) - (player.position.Y + (player.height / 2))) < 20f)
                        {
                            NPC.ai[1] += 1f;
                            NPC.ai[2] = 0f;

                            float num1044 = revenge ? 20f : 18f;
							num1044 += 8f * enrageScale;

							if (revenge && (!sirenAlive || phase4))
								num1044 += death ? 9f * (1f - lifeRatio) : 6f * (1f - lifeRatio);

                            if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
                                num1044 += 4f;

                            if (BossRushEvent.BossRushActive)
                                num1044 *= 1.25f;

                            Vector2 vector117 = NPC.Center;
                            float num1045 = player.Center.X - vector117.X;
                            float num1046 = player.Center.Y - vector117.Y;
                            float num1047 = (float)Math.Sqrt(num1045 * num1045 + num1046 * num1046);
                            num1047 = num1044 / num1047;
                            NPC.velocity.X = num1045 * num1047;
                            NPC.velocity.Y = num1046 * num1047;
                            NPC.spriteDirection = NPC.direction;
							SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/LeviathanRoarCharge"), NPC.Center);
                            return;
                        }

                        float num1048 = revenge ? 7.5f : 6.5f;
                        float num1049 = revenge ? 0.12f : 0.11f;
						num1048 += 4f * enrageScale;
						num1049 += 0.06f * enrageScale;

						if (revenge && (!sirenAlive || phase4))
						{
							num1048 += death ? 9f * (1f - lifeRatio) : 6f * (1f - lifeRatio);
							num1049 += death ? 0.15f * (1f - lifeRatio) : 0.1f * (1f - lifeRatio);
						}

                        if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
                        {
                            num1048 += 3f;
                            num1049 += 0.2f;
                        }

                        if (BossRushEvent.BossRushActive)
                        {
                            num1048 *= 1.25f;
                            num1049 *= 1.25f;
                        }

                        if (NPC.Center.Y < player.Center.Y)
                            NPC.velocity.Y += num1049;
                        else
                            NPC.velocity.Y -= num1049;

                        if (NPC.velocity.Y < -num1048)
                            NPC.velocity.Y = -num1048;
                        if (NPC.velocity.Y > num1048)
                            NPC.velocity.Y = num1048;

                        if (Math.Abs(NPC.Center.X - player.Center.X) > chargeDistance + 200f)
                            NPC.velocity.X += num1049 * NPC.direction;
                        else if (Math.Abs(NPC.Center.X - player.Center.X) < chargeDistance)
                            NPC.velocity.X -= num1049 * NPC.direction;
                        else
                            NPC.velocity.X *= 0.8f;

                        if (NPC.velocity.X < -num1048)
                            NPC.velocity.X = -num1048;
                        if (NPC.velocity.X > num1048)
                            NPC.velocity.X = num1048;

                        NPC.spriteDirection = NPC.direction;
                    }
                    else
                    {
                        if (NPC.velocity.X < 0f)
                            NPC.direction = -1;
                        else
                            NPC.direction = 1;

                        NPC.spriteDirection = NPC.direction;

                        int num1051 = 1;
                        if (NPC.Center.X < player.Center.X)
                            num1051 = -1;
                        if (NPC.direction == num1051 && Math.Abs(NPC.Center.X - player.Center.X) > chargeDistance)
                            NPC.ai[2] = 1f;

                        if (NPC.ai[2] != 1f)
                            return;

                        NPC.TargetClosest(true);

                        NPC.spriteDirection = NPC.direction;

                        NPC.velocity *= 0.9f;
                        float num1052 = revenge ? 0.11f : 0.1f;
						num1052 += 0.06f * enrageScale;

						if (revenge && (!sirenAlive || phase4))
						{
							NPC.velocity *= death ? MathHelper.Lerp(0.75f, 1f, lifeRatio) : MathHelper.Lerp(0.81f, 1f, lifeRatio);
							num1052 += death ? 0.15f * (1f - lifeRatio) : 0.1f * (1f - lifeRatio);
						}

                        if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < num1052)
                        {
                            NPC.ai[2] = 0f;
                            NPC.ai[1] += 1f;
                        }
                    }
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Blood, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }

                if (Main.netMode != NetmodeID.Server)
                {
	                float randomSpread = Main.rand.Next(-200, 200) / 100;
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("LeviGore").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("LeviGore2").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("LeviGore3").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("LeviGore4").Type, 1f);
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        // The Leviathan runs the same loot code as Anahita, but only if she dies last.
        public override void OnKill()
        {
            if (LastAnLStanding())
            {
	            // Mark Siren & Levi as dead
	            CalamityWorld.downedLeviathan = true;
	            CalamityNetcode.SyncWorld();
            }
        }
        
        public static bool LastAnLStanding()
        {
	        int count = NPC.CountNPCS(ModContent.NPCType<Siren>()) + NPC.CountNPCS(ModContent.NPCType<Leviathan>());
	        return count <= 1;
        }

        // This loot code is shared with Anahita.
        public static void DropSirenLeviLoot(NPCLoot npcLoot)
        {
	        var lastStanding = npcLoot.DefineConditionalDropSet(() => LastAnLStanding());
	        lastStanding.Add(ItemDropRule.BossBag(ModContent.ItemType<LeviathanBag>()));

	        npcLoot.AddConditionalPerPlayer(() => !CalamityWorld.downedLeviathan && LastAnLStanding(), ModContent.ItemType<KnowledgeOcean>(), 1);
	        npcLoot.AddConditionalPerPlayer(() => !CalamityWorld.downedLeviathan && LastAnLStanding(), ModContent.ItemType<KnowledgeLeviathanandSiren>(), 1);
	        lastStanding.AddResidentEvilAmmo(CalamityWorld.downedLeviathan, 4, 2, 1);

            // All other drops are contained in the bag, so they only drop directly on Normal
            var normalOnly = npcLoot.DefineNormalOnlyDropSet();
            lastStanding.Add(normalOnly);
            {
                // Weapons
                int[] weapons = new int[]
                {
                    ModContent.ItemType<Greentide>(),
                    ModContent.ItemType<Leviatitan>(),
                    ModContent.ItemType<SirensSong>(),
                    ModContent.ItemType<Atlantis>(),
                    ModContent.ItemType<GastricBelcherStaff>(),
                    ModContent.ItemType<BrackishFlask>(),
                    ModContent.ItemType<LeviathanTeeth>()
                };
                normalOnly.Add(DropHelper.CalamityStyle(DropHelper.DirectWeaponDropRateFraction, weapons));

                // Equipment
                normalOnly.Add(ModContent.ItemType<LureofEnthrallment>(), 4);

                // Vanity
                normalOnly.Add(ModContent.ItemType<LeviathanMask>(), 7);
                normalOnly.Add(ModContent.ItemType<AnahitaMask>(), 7);

                // Fishing
                normalOnly.Add(ItemID.HotlineFishingHook, 10);
                normalOnly.Add(ItemID.BottomlessBucket, 10);
                normalOnly.Add(ItemID.SuperAbsorbantSponge, 10);
                normalOnly.Add(ItemID.FishingPotion, 5, 5, 8);
                normalOnly.Add(ItemID.SonarPotion, 5, 5, 8);
                normalOnly.Add(ItemID.CratePotion, 5, 5, 8);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
	        DropSirenLeviLoot(npcLoot);
	        
	        //Trophy dropped regardless of Anahita, precedent of Twins
	        npcLoot.Add(ModContent.ItemType<LeviathanTrophy>(), 10);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Wet, 240, true);
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = AttackTexture.Value;
			if (NPC.ai[0] == 1f || NPC.Calamity().newAI[3] < 180f)
            {
				texture = TextureAssets.Npc[NPC.type].Value;
            }
			SpriteEffects spriteEffects = SpriteEffects.FlipHorizontally;
			float xOffset = -50f;
			if (NPC.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.None;
				xOffset *= -1f;
			}
			Rectangle rectangle = new Rectangle(NPC.frame.X, NPC.frame.Y, texture.Width / 2, texture.Height / 3);
			Vector2 origin = rectangle.Size() / 2f;
			spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(xOffset, NPC.gfxOffY), rectangle, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
			int width = 1011;
			int height = 486;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Scale = 0.2f,
				PortraitScale = 0.3f,
			};
			NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;

			if (NPC.IsABestiaryIconDummy)
				NPC.Opacity = 1f;

            if (!initialised)
            {
                counter = 3;
                NPC.frameCounter = 8D;
                initialised = true;
            }

			NPC.frameCounter += 1D;
			if (NPC.frameCounter >= 8D)
			{
				NPC.frameCounter = 0D;
				counter++;
				NPC.frame.X = counter >= 3 ? width + 3 : 0;

				if (counter == 3)
					NPC.frame.Y = 0;
				else
					NPC.frame.Y += height;
			}

            if (counter == 6)
            {
                counter = 1;
                NPC.frame.Y = 0;
                NPC.frame.X = 0;
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }
    }
}
