using CalRD.Buffs.StatBuffs;
using CalRD.Buffs.StatDebuffs;
using CalRD.Events;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Projectiles.Boss;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.Crabulon
{
	[AutoloadBossHead]
    public class CrabulonIdle : ModNPC
    {
        private int shotSpacing = 1000;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crabulon");
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
	            Scale = 0.32f,
	            PortraitScale = 0.55f,
	            PortraitPositionYOverride = 54f
            };
            value.Position.Y += 80f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
	        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
	        {
		        BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom,
		        new FlavorTextBestiaryInfoElement("Once an ocean crab that migrated towards the underground caves that resembled its home the most... It met an unfortunate fate, now a walking puppet of the shrooms.")
	        });
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 14f;
			NPC.GetNPCDamage();
			NPC.width = 280;
            NPC.height = 160;
            NPC.defense = 8;
            NPC.LifeMaxNERB(3000, 4000, 11000000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.buffImmune[ModContent.BuffType<GlacialState>()] = true;
            NPC.buffImmune[ModContent.BuffType<TemporalSadness>()] = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/Crabulon");
            NPC.boss = true;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 4, 0, 0);
            NPC.HitSound = SoundID.NPCHit45;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(NPC.localAI[0]);
			writer.Write(shotSpacing);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			NPC.localAI[0] = reader.ReadSingle();
			shotSpacing = reader.ReadInt32();
        }

        public override void AI()
        {
			NPC.gfxOffY = -16;

			Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2)) / 16f), (int)((NPC.position.Y + (NPC.height / 2)) / 16f), 0f, 0.3f, 0.7f);
			
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;

			NPC.spriteDirection = NPC.direction;

			// Percent life remaining
			float lifeRatio = NPC.life / (float)NPC.lifeMax;

			// Phases
			bool phase2 = lifeRatio < 0.66f && expertMode;
			bool phase3 = lifeRatio < 0.33f && expertMode;

			// Get a target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				NPC.TargetClosest(true);

			Player player = Main.player[NPC.target];
			if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.noTileCollide = true;

					if (NPC.velocity.Y < -3f)
						NPC.velocity.Y = -3f;
					NPC.velocity.Y += 0.1f;
					if (NPC.velocity.Y > 12f)
						NPC.velocity.Y = 12f;

					if (NPC.timeLeft > 60)
                        NPC.timeLeft = 60;

					if (NPC.ai[0] != 1f)
					{
						NPC.ai[0] = 1f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						NPC.ai[3] = 0f;
						shotSpacing = 1000;
						NPC.netUpdate = true;
					}
					return;
                }
            }
            else if (NPC.timeLeft < 1800)
				NPC.timeLeft = 1800;

			float enrageScale = 0f;
			if ((NPC.position.Y / 16f) < Main.worldSurface)
				enrageScale += 1f;
			if (!player.ZoneGlowshroom)
				enrageScale += 1f;

			if (BossRushEvent.BossRushActive)
				enrageScale = 0f;

			if (NPC.ai[0] != 0f && NPC.ai[0] < 3f)
            {
                Vector2 vector34 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                float num349 = player.position.X + (player.width / 2) - vector34.X;
                float num350 = player.position.Y + (player.height / 2) - vector34.Y;
                float num351 = (float)Math.Sqrt(num349 * num349 + num350 * num350);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int num352 = 1;
                    NPC.localAI[3] += 2f;
                    if (BossRushEvent.BossRushActive)
                    {
                        NPC.localAI[3] += 2f;
                        num352 += 3;
                    }
                    if (phase2)
                    {
                        NPC.localAI[3] += 1f;
                        num352 += 2;
                    }
                    if (phase3)
                    {
                        NPC.localAI[3] += 2f;
                        num352 += 3;
                    }
                    if (NPC.ai[3] == 0f)
                    {
                        if (NPC.localAI[3] > 600f)
                        {
                            NPC.ai[3] = 1f;
                            NPC.localAI[3] = 0f;
                        }
                    }
                    else if (NPC.localAI[3] > 45f)
                    {
                        NPC.localAI[3] = 0f;
                        NPC.ai[3] += 1f;
                        if (NPC.ai[3] >= num352)
                        {
                            NPC.ai[3] = 0f;
                        }
                        float num353 = 10f;
                        int type = ModContent.ProjectileType<MushBomb>();
                        SoundEngine.PlaySound(SoundID.Item42, NPC.position);
                        if (BossRushEvent.BossRushActive)
                        {
                            num353 += 3f;
                        }
                        if (phase2)
                        {
                            num353 += 1f;
                        }
                        if (phase3)
                        {
                            num353 += 1f;
                        }
                        vector34 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        num349 = player.position.X + player.width * 0.5f - vector34.X;
                        num350 = player.position.Y + player.height * 0.5f - vector34.Y;
                        num351 = (float)Math.Sqrt(num349 * num349 + num350 * num350);
                        num351 = num353 / num351;
                        num349 *= num351;
                        num350 *= num351;
                        vector34.X += num349;
                        vector34.Y += num350;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), vector34.X, vector34.Y, num349, num350 - 5f, type, NPC.GetProjectileDamage(type), 0f, Main.myPlayer, 0f, 0f);
                    }
                }
            }
            if (NPC.ai[0] == 0f)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    if (!player.dead && player.active && (player.Center - NPC.Center).Length() < 800f)
                    {
                        player.AddBuff(ModContent.BuffType<Mushy>(), 2);
                    }
                }
                int sporeDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 56, NPC.velocity.X, NPC.velocity.Y, 255, new Color(0, 80, 255, 80), 1.2f);
                Main.dust[sporeDust].noGravity = true;
                Main.dust[sporeDust].velocity *= 0.5f;
                NPC.ai[1] += 1f;
                if (NPC.justHit || NPC.ai[1] >= 420f)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 1f)
            {
                NPC.velocity *= 0.98f;
                NPC.ai[1] += 1f;
                if (NPC.ai[1] >= (death ? 5f : revenge ? 30f : 60f))
                {
					NPC.TargetClosest(true);
					NPC.noGravity = true;
                    NPC.noTileCollide = true;
                    NPC.ai[0] = 2f;
                    NPC.ai[1] = 0f;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 2f)
            {
                float num823 = 1f;
                if (phase2)
                    num823 = 1.25f;
                if (phase3)
                    num823 = 1.75f;
				if (death)
					num823 += 2f * (1f - lifeRatio);
                if (BossRushEvent.BossRushActive)
                    num823 = 12f;
                if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
                    num823 = 16f;
				num823 += 2f * enrageScale;

				bool flag51 = false;
				if (Math.Abs(NPC.Center.X - player.Center.X) < 50f)
                    flag51 = true;

                if (flag51)
                {
                    NPC.velocity.X *= 0.9f;
                    if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                    {
                        NPC.velocity.X = 0f;
                    }
                }
                else
                {
                    float playerLocation = NPC.Center.X - player.Center.X;
                    NPC.direction = playerLocation < 0 ? 1 : -1;

                    if (NPC.direction > 0)
                        NPC.velocity.X = (NPC.velocity.X * 20f + num823) / 21f;
                    if (NPC.direction < 0)
                        NPC.velocity.X = (NPC.velocity.X * 20f - num823) / 21f;
                }
                int num854 = 80;
                int num855 = 20;
                Vector2 position2 = new Vector2(NPC.Center.X - (num854 / 2), NPC.position.Y + NPC.height - num855);
                bool flag52 = false;
                if (NPC.position.X < player.position.X && NPC.position.X + NPC.width > player.position.X + player.width && NPC.position.Y + NPC.height < player.position.Y + player.height - 16f)
                {
                    flag52 = true;
                }
                if (flag52)
                {
                    NPC.velocity.Y += 0.5f;
                }
                else if (Collision.SolidCollision(position2, num854, num855))
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y = 0f;
                    }
                    if (NPC.velocity.Y > -0.2)
                    {
                        NPC.velocity.Y -= 0.025f;
                    }
                    else
                    {
                        NPC.velocity.Y -= 0.2f;
                    }
                    if (NPC.velocity.Y < -4f)
                    {
                        NPC.velocity.Y = -4f;
                    }
                }
                else
                {
                    if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y = 0f;
                    }
                    if (NPC.velocity.Y < 0.1)
                    {
                        NPC.velocity.Y += 0.025f;
                    }
                    else
                    {
                        NPC.velocity.Y += 0.5f;
                    }
                }

                if (NPC.ai[1] % 25f == 24f)
                    NPC.netUpdate = true;

                NPC.ai[1] += 1f;
                if (NPC.ai[1] >= (360f - (death ? 120f * (1f - lifeRatio) : 0f)))
                {
					NPC.TargetClosest(true);
					NPC.noGravity = false;
                    NPC.noTileCollide = false;
                    NPC.ai[0] = 3f;
                    NPC.ai[1] = 0f;
                    NPC.netUpdate = true;
                }
                if (NPC.velocity.Y > 10f)
                {
                    NPC.velocity.Y = 10f;
                }
            }
            else if (NPC.ai[0] == 3f)
            {
                NPC.noTileCollide = false;
                if (NPC.velocity.Y == 0f)
                {
                    NPC.velocity.X *= 0.8f;
                    NPC.ai[1] += 1f;
                    if (NPC.ai[1] % 15f == 14f)
                        NPC.netUpdate = true;

                    if (NPC.ai[1] > 0f)
                    {
						if (revenge)
						{
							switch ((int)NPC.ai[3])
							{
								case 0:
									break;
								case 1:
									NPC.ai[1] += 1f;
									break;
								case 2:
									NPC.ai[1] += 3f;
									break;
								case 3:
									NPC.ai[1] += 6f;
									break;
								default:
									break;
							}
						}
                        if (phase2)
                        {
                            NPC.ai[1] += !revenge ? 4f : 1f;
                        }
                        if (phase3)
                        {
                            NPC.ai[1] += !revenge ? 4f : 1f;
                        }
                    }

                    if (NPC.ai[1] >= 300f)
                    {
                        NPC.ai[1] = -20f;
                    }
                    else if (NPC.ai[1] == -1f)
                    {
						int velocityX = BossRushEvent.BossRushActive ? 12 : 4;
						float velocityY = BossRushEvent.BossRushActive ? -16f : -12f;

						float distanceBelowTarget = NPC.position.Y - (player.position.Y + 80f);
						float speedMult = 1f;

						if (revenge)
						{
							switch ((int)NPC.ai[3])
							{
								case 0: // Normal
									break;
								case 1: // High
									velocityY -= 4f;
									break;
								case 2: // Low
									velocityY += 4f;
									break;
								case 3: // Long and low
									velocityX += 4;
									velocityY += 4f;
									break;
								default:
									break;
							}

							if (distanceBelowTarget > 0f)
								speedMult += distanceBelowTarget * 0.001f;

							if (speedMult > 2f)
								speedMult = 2f;

							velocityY *= speedMult;
						}

						if (expertMode)
						{
							if (player.position.Y < NPC.Bottom.Y)
								NPC.velocity.Y = velocityY;
							else
								NPC.velocity.Y = 1f;

							NPC.noTileCollide = true;
						}
						else
							NPC.velocity.Y = velocityY;

						NPC.velocity.X = velocityX * NPC.direction;

                        NPC.ai[0] = 4f;
                        NPC.ai[1] = 0f;
                        NPC.netUpdate = true;
                    }
                }
            }
            else
            {
                if (NPC.velocity.Y == 0f)
                {
                    SoundEngine.PlaySound(SoundID.Item14, NPC.position);

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y + 20, 0f, 0f, ModContent.ProjectileType<Mushmash>(), 20, 0f, Main.myPlayer, 0f, 0f);

					int type = ModContent.ProjectileType<MushBombFall>();
					int damage = NPC.GetProjectileDamage(type);

					if (NPC.ai[2] % 2f == 0f && phase2 && revenge)
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							float velocityX = NPC.ai[2] == 0f ? -4f : 4f;
							for (int x = 0; x < 20; x++)
							{
								Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X + shotSpacing, NPC.Center.Y - 1000f, velocityX, 0f, type, damage, 0f, Main.myPlayer, 0f, 0f);
								shotSpacing -= 100;
							}
							shotSpacing = 1000;
						}
					}

					NPC.TargetClosest(true);
					NPC.ai[2] += 1f;
					if (NPC.ai[2] >= (phase2 ? 4f : 3f))
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient && revenge && !phase2)
                        {
                            for (int x = 0; x < 20; x++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X + shotSpacing, NPC.Center.Y - 1000f, 0f, 0f, type, damage, 0f, Main.myPlayer, 0f, 0f);
                                shotSpacing -= 100;
                            }
                            shotSpacing = 1000;
                        }

                        NPC.ai[0] = 1f;
                        NPC.ai[2] = 0f;
						if (revenge)
							NPC.ai[3] = 0f;
                        NPC.netUpdate = true;
                    }
                    else
                    {
                        NPC.ai[0] = 3f;
						if (revenge)
							NPC.ai[3] += 1f;
                        NPC.netUpdate = true;
                    }

                    for (int num622 = (int)NPC.position.X - 20; num622 < (int)NPC.position.X + NPC.width + 40; num622 += 20)
                    {
                        for (int num623 = 0; num623 < 4; num623++)
                        {
                            int num624 = Dust.NewDust(new Vector2(NPC.position.X - 20f, NPC.position.Y + NPC.height), NPC.width + 20, 4, 56, 0f, 0f, 100, default, 1.5f);
                            Main.dust[num624].velocity *= 0.2f;
                        }
                    }
                }
                else
                {
					if (!player.dead && expertMode)
					{
						if ((player.position.Y > NPC.Bottom.Y && NPC.velocity.Y > 0f) || (player.position.Y < NPC.Bottom.Y && NPC.velocity.Y < 0f))
							NPC.noTileCollide = true;
						else if ((NPC.velocity.Y > 0f && NPC.Bottom.Y > Main.player[NPC.target].Top.Y) || (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].Center, 1, 1) && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height)))
							NPC.noTileCollide = false;
					}

					if (NPC.position.X < player.position.X && NPC.position.X + NPC.width > player.position.X + player.width)
                    {
                        NPC.velocity.X *= 0.9f;
                        NPC.velocity.Y += BossRushEvent.BossRushActive ? 0.3f : death ? 0.18f : 0.15f;
                    }
                    else
                    {
						float velocityX = 0.11f +
							(expertMode ? 0.02f : 0f) +
							(revenge ? 0.02f : 0f) +
							(death ? 0.02f : 0f);
						velocityX += 0.05f * enrageScale;

                        if (NPC.direction < 0)
                            NPC.velocity.X -= velocityX;
                        else if (NPC.direction > 0)
                            NPC.velocity.X += velocityX;

                        float num626 = BossRushEvent.BossRushActive ? 5f : 2.5f;
						num626 += enrageScale;
                        if (revenge)
                        {
                            num626 += 1f;
                        }
                        if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
                        {
                            num626 += 3f;
                        }
                        if (phase2)
                        {
                            num626 += 1f;
                        }
                        if (phase3)
                        {
                            num626 += 1f;
                        }
                        if (NPC.velocity.X < -num626)
                        {
                            NPC.velocity.X = -num626;
                        }
                        if (NPC.velocity.X > num626)
                        {
                            NPC.velocity.X = num626;
                        }
                    }
                }
            }

            if (NPC.localAI[0] == 0f && NPC.life > 0)
            {
                NPC.localAI[0] = NPC.lifeMax;
            }
            if (NPC.life > 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int num660 = (int)(NPC.lifeMax * 0.05);
                    if ((NPC.life + num660) < NPC.localAI[0])
                    {
                        NPC.localAI[0] = NPC.life;
                        int num661 = death ? 3 : expertMode ? Main.rand.Next(2, 4) : 2;
                        for (int num662 = 0; num662 < num661; num662++)
                        {
                            int x = (int)(NPC.position.X + Main.rand.Next(NPC.width - 32));
                            int y = (int)(NPC.position.Y + Main.rand.Next(NPC.height - 32));
                            int num663 = ModContent.NPCType<CrabShroom>();
                            int num664 = NPC.NewNPC(NPC.GetSource_FromThis(), x, y, num663);
                            Main.npc[num664].SetDefaults(num663, default);
                            Main.npc[num664].velocity.X = Main.rand.Next(-50, 51) * 0.1f;
                            Main.npc[num664].velocity.Y = Main.rand.Next(-50, -31) * 0.1f;
                            if (Main.netMode == NetmodeID.Server && num664 < 200)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                            }
                        }
                    }
                }
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.ai[0] > 1f;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D glow = ModContent.Request<Texture2D>("CalRD/NPCs/Crabulon/CrabulonIdleGlow").Value;
			Texture2D texture = ModContent.Request<Texture2D>("CalRD/NPCs/Crabulon/CrabulonIdleAlt").Value;
			Texture2D textureGlow = ModContent.Request<Texture2D>("CalRD/NPCs/Crabulon/CrabulonIdleAltGlow").Value;
			Texture2D textureAttack = ModContent.Request<Texture2D>("CalRD/NPCs/Crabulon/CrabulonAttack").Value;
			Texture2D textureAttackGlow = ModContent.Request<Texture2D>("CalRD/NPCs/Crabulon/CrabulonAttackGlow").Value;

			Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
			Vector2 vector43 = NPC.Center - screenPos;
			vector43 -= new Vector2(TextureAssets.Npc[NPC.type].Value.Width, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			Color color37 = Color.Lerp(Color.White, Color.Cyan, 0.5f);

			if (NPC.ai[0] > 2f)
			{
				vector11 = new Vector2(textureAttack.Width / 2, textureAttack.Height / 2);
				vector43 = NPC.Center - screenPos;
				vector43 -= new Vector2(textureAttack.Width, textureAttack.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
				vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);

				spriteBatch.Draw(textureAttack, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

				spriteBatch.Draw(textureAttackGlow, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
			}
			else if (NPC.ai[0] == 2f)
			{
				vector11 = new Vector2(texture.Width / 2, texture.Height / 2);
				vector43 = NPC.Center - screenPos;
				vector43 -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
				vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);

				spriteBatch.Draw(texture, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

				spriteBatch.Draw(textureGlow, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
			}
			else
			{
				spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

				spriteBatch.Draw(glow, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
			}

			return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CrabulonBag>()));

            npcLoot.Add(ModContent.ItemType<CrabulonTrophy>(), 10);
            npcLoot.AddConditionalPerPlayer(() => !CalamityWorld.downedCrabulon, ModContent.ItemType<KnowledgeCrabulon>(), 1);
            npcLoot.AddResidentEvilAmmo(CalamityWorld.downedCrabulon, 2, 0, 0);

			// All other drops are contained in the bag, so they only drop directly on Normal
			var normalOnly = npcLoot.DefineNormalOnlyDropSet();
            {
                // Materials
                normalOnly.Add(ItemID.GlowingMushroom, 1, 20, 30);
                normalOnly.Add(ItemID.MushroomGrassSeeds, 1, 3, 6);

                // Weapons
                int[] weapons = new int[]
                {
                    ModContent.ItemType<MycelialClaws>(),
                    ModContent.ItemType<Fungicide>(),
                    ModContent.ItemType<HyphaeRod>(),
                    ModContent.ItemType<Mycoroot>(),
                    ModContent.ItemType<Shroomerang>()
                };
                normalOnly.Add(DropHelper.CalamityStyle(DropHelper.DirectWeaponDropRateFraction, weapons));

                // Vanity
                normalOnly.Add(ModContent.ItemType<CrabulonMask>(), 7);
            }
        }

        public override void OnKill()
        {
	        CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Dryad }, CalamityWorld.downedCrabulon);
	        
	        // Mark Crabulon as dead
	        CalamityWorld.downedCrabulon = true;
	        CalamityNetcode.SyncWorld();
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 56, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 200;
                NPC.height = 100;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 56, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 56, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 56, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }

                if (Main.netMode != NetmodeID.Server)
                {
	                float randomSpread = Main.rand.Next(-200, 200) / 100;
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("Crabulon").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("Crabulon2").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("Crabulon3").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("Crabulon4").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("Crabulon5").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("Crabulon6").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("Crabulon7").Type, 1f);
                }
            }
        }
    }
}
