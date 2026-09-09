using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Events;
using CalRD.Items.Materials;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.Perforator
{
	[AutoloadBossHead]
    public class PerforatorHeadLarge : ModNPC
    {
        private bool flies = false;
        private bool TailSpawned = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Perforator");
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.75f,
                PortraitScale = 0.75f,
                CustomTexturePath = "CalRD/ExtraTextures/Bestiary/PerforatorLarge_Bestiary",
                PortraitPositionXOverride = 40,
                PortraitPositionYOverride = 40
            };
            value.Position.X += 70;
            value.Position.Y += 40;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                new FlavorTextBestiaryInfoElement("The middest of the perforators, its purpose is to tear.")
            });
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 5f;
            NPC.width = 70;
            NPC.height = 84;
            NPC.defense = 4;
			NPC.LifeMaxNERB(2500, 2700, 800000);
			double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = 6;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.alpha = 255;
            NPC.buffImmune[ModContent.BuffType<TimeSlow>()] = false;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;

			if (CalamityWorld.death || BossRushEvent.BossRushActive)
				NPC.scale = 1.25f;
			else if (CalamityWorld.revenge)
				NPC.scale = 1.15f;
			else if (Main.expertMode)
				NPC.scale = 1.1f;
		}

        public override void AI()
        {
            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;

			float enrageScale = 0f;
			if ((NPC.position.Y / 16f) < Main.worldSurface)
				enrageScale += 1f;
			if (!Main.player[NPC.target].ZoneCrimson)
				enrageScale += 1f;

			if (BossRushEvent.BossRushActive)
				enrageScale = 0f;

			// Percent life remaining
			float lifeRatio = NPC.life / (float)NPC.lifeMax;

			if (revenge || lifeRatio < (expertMode ? 0.75f : 0.5f))
				NPC.Calamity().newAI[0] += 1f;

			float burrowTimeGateValue = death ? 210f : 270f;
			bool burrow = NPC.Calamity().newAI[0] >= burrowTimeGateValue;
			bool resetTime = NPC.Calamity().newAI[0] >= burrowTimeGateValue + 600f;
			bool lungeUpward = burrow && NPC.Calamity().newAI[1] == 1f;
			bool quickFall = NPC.Calamity().newAI[1] == 2f;

			float speed = 8f;
			float turnSpeed = 0.06f;

			if (expertMode)
			{
				float velocityScale = (death ? 12f : 10f) * enrageScale;
				speed += velocityScale * (1f - lifeRatio);
				float accelerationScale = (death ? 0.12f : 0.1f) * enrageScale;
				turnSpeed += accelerationScale * (1f - lifeRatio);
			}

			if (lungeUpward)
			{
				speed *= 1.25f;
				turnSpeed *= 1.25f;
			}

			if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
			{
				speed *= 1.25f;
				turnSpeed *= 1.25f;
			}

			if (BossRushEvent.BossRushActive)
			{
				speed *= 1.25f;
				turnSpeed *= 1.25f;
			}

			if (NPC.ai[3] > 0f)
            {
                NPC.realLife = (int)NPC.ai[3];
            }

			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(true);
			}

			Player player = Main.player[NPC.target];

            NPC.alpha -= 42;
            if (NPC.alpha < 0)
            {
                NPC.alpha = 0;
            }

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (!TailSpawned)
				{
					int Previous = NPC.whoAmI;
					int maxLength = death ? 27 : revenge ? 24 : expertMode ? 21 : 15;
					for (int num36 = 0; num36 < maxLength; num36++)
					{
						int lol;
						if (num36 >= 0 && num36 < maxLength - 1)
						{
							lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<PerforatorBodyLarge>(), NPC.whoAmI);
						}
						else
						{
							lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<PerforatorTailLarge>(), NPC.whoAmI);
						}
						if (num36 % 2 == 0)
						{
							Main.npc[lol].localAI[3] = 1f;
						}
						Main.npc[lol].realLife = NPC.whoAmI;
						Main.npc[lol].ai[2] = NPC.whoAmI;
						Main.npc[lol].ai[1] = Previous;
						Main.npc[Previous].ai[0] = lol;
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, lol, 0f, 0f, 0f, 0);
						Previous = lol;
					}
					TailSpawned = true;
				}
			}

            int num180 = (int)(NPC.position.X / 16f) - 1;
            int num181 = (int)((NPC.position.X + NPC.width) / 16f) + 2;
            int num182 = (int)(NPC.position.Y / 16f) - 1;
            int num183 = (int)((NPC.position.Y + NPC.height) / 16f) + 2;
            if (num180 < 0)
            {
                num180 = 0;
            }
            if (num181 > Main.maxTilesX)
            {
                num181 = Main.maxTilesX;
            }
            if (num182 < 0)
            {
                num182 = 0;
            }
            if (num183 > Main.maxTilesY)
            {
                num183 = Main.maxTilesY;
            }
            bool flag94 = flies || lungeUpward;
            if (!flag94)
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
                                flag94 = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (!flag94)
            {
                NPC.localAI[1] = 1f;
                Rectangle rectangle12 = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
                int num954 = death ? 160 : revenge ? 200 : expertMode ? 240 : 300;
                bool flag95 = true;
                if (NPC.position.Y > player.position.Y)
                {
                    for (int num955 = 0; num955 < Main.maxPlayers; num955++)
                    {
                        if (Main.player[num955].active)
                        {
                            Rectangle rectangle13 = new Rectangle((int)Main.player[num955].position.X - num954, (int)Main.player[num955].position.Y - num954, num954 * 2, num954 * 2);
                            if (rectangle12.Intersects(rectangle13))
                            {
                                flag95 = false;
                                break;
                            }
                        }
                    }
                    if (flag95)
                    {
                        flag94 = true;
                    }
                }
            }
            else
            {
                NPC.localAI[1] = 0f;
            }
            if (player.dead || CalamityGlobalNPC.perfHive < 0 || !Main.npc[CalamityGlobalNPC.perfHive].active)
            {
				NPC.TargetClosest(false);
				flag94 = false;
                NPC.velocity.Y += 1f;
                if (NPC.position.Y > Main.worldSurface * 16.0)
                {
                    NPC.velocity.Y += 1f;
                }
                if (NPC.position.Y > Main.rockLayer * 16.0)
                {
                    for (int num957 = 0; num957 < Main.maxNPCs; num957++)
                    {
                        if (Main.npc[num957].aiStyle == NPC.aiStyle)
                        {
                            Main.npc[num957].active = false;
                        }
                    }
                }
            }
            float num188 = speed;
            float num189 = turnSpeed;
			float burrowTarget = player.Center.Y + 1000f;
			float lungeTarget = player.Center.Y - 600f;
			Vector2 vector18 = NPC.Center;
            float num191 = player.Center.X;
            float num192 = lungeUpward ? lungeTarget : burrow ? burrowTarget : player.Center.Y;
			num191 = (int)(num191 / 16f) * 16;
            num192 = (int)(num192 / 16f) * 16;
            vector18.X = (int)(vector18.X / 16f) * 16;
            vector18.Y = (int)(vector18.Y / 16f) * 16;
            num191 -= vector18.X;
            num192 -= vector18.Y;
            float num193 = (float)Math.Sqrt(num191 * num191 + num192 * num192);

			// Lunge up towards target
			if (burrow && NPC.Center.Y >= burrowTarget - 16f)
				NPC.Calamity().newAI[1] = 1f;

			// Quickly fall back down once above target
			if (lungeUpward && NPC.Center.Y <= player.Center.Y - 420f)
				NPC.Calamity().newAI[1] = 2f;

			// Quickly fall and reset variables once at target's Y position
			if (quickFall)
			{
				NPC.velocity.Y += 1f;
				if (NPC.Center.Y >= player.Center.Y)
				{
					NPC.Calamity().newAI[0] = 0f;
					NPC.Calamity().newAI[1] = 0f;
				}
			}

			// Reset variables if the burrow and lunge attack is taking too long
			if (resetTime)
			{
				NPC.Calamity().newAI[0] = 0f;
				NPC.Calamity().newAI[1] = 0f;
			}

			if (!flag94)
            {
                NPC.TargetClosest(true);
                NPC.velocity.Y = NPC.velocity.Y + (turnSpeed * 0.5f);
                if (NPC.velocity.Y > num188)
                {
                    NPC.velocity.Y = num188;
                }
                if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < num188 * 0.4)
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X - num189 * 1.1f;
                    }
                    else
                    {
                        NPC.velocity.X = NPC.velocity.X + num189 * 1.1f;
                    }
                }
                else if (NPC.velocity.Y == num188)
                {
                    if (NPC.velocity.X < num191)
                    {
                        NPC.velocity.X = NPC.velocity.X + num189;
                    }
                    else if (NPC.velocity.X > num191)
                    {
                        NPC.velocity.X = NPC.velocity.X - num189;
                    }
                }
                else if (NPC.velocity.Y > 4f)
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X + num189 * 0.9f;
                    }
                    else
                    {
                        NPC.velocity.X = NPC.velocity.X - num189 * 0.9f;
                    }
                }
            }
            else
            {
                if (!flies && NPC.behindTiles && NPC.soundDelay == 0)
                {
                    float num195 = num193 / 40f;
                    if (num195 < 10f)
                    {
                        num195 = 10f;
                    }
                    if (num195 > 20f)
                    {
                        num195 = 20f;
                    }
                    NPC.soundDelay = (int)num195;
                    SoundEngine.PlaySound(SoundID.WormDig, NPC.position);
                }
                num193 = (float)Math.Sqrt((double)(num191 * num191 + num192 * num192));
                float num196 = Math.Abs(num191);
                float num197 = Math.Abs(num192);
                float num198 = num188 / num193;
                num191 *= num198;
                num192 *= num198;
                bool flag21 = false;
                if (!flag21)
                {
                    if ((NPC.velocity.X > 0f && num191 > 0f) || (NPC.velocity.X < 0f && num191 < 0f) || (NPC.velocity.Y > 0f && num192 > 0f) || (NPC.velocity.Y < 0f && num192 < 0f))
                    {
                        if (NPC.velocity.X < num191)
                        {
                            NPC.velocity.X = NPC.velocity.X + num189;
                        }
                        else
                        {
                            if (NPC.velocity.X > num191)
                            {
                                NPC.velocity.X = NPC.velocity.X - num189;
                            }
                        }
                        if (NPC.velocity.Y < num192)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + num189;
                        }
                        else
                        {
                            if (NPC.velocity.Y > num192)
                            {
                                NPC.velocity.Y = NPC.velocity.Y - num189;
                            }
                        }
                        if ((double)Math.Abs(num192) < (double)num188 * 0.2 && ((NPC.velocity.X > 0f && num191 < 0f) || (NPC.velocity.X < 0f && num191 > 0f)))
                        {
                            if (NPC.velocity.Y > 0f)
                            {
                                NPC.velocity.Y = NPC.velocity.Y + num189 * 2f;
                            }
                            else
                            {
                                NPC.velocity.Y = NPC.velocity.Y - num189 * 2f;
                            }
                        }
                        if ((double)Math.Abs(num191) < (double)num188 * 0.2 && ((NPC.velocity.Y > 0f && num192 < 0f) || (NPC.velocity.Y < 0f && num192 > 0f)))
                        {
                            if (NPC.velocity.X > 0f)
                            {
                                NPC.velocity.X = NPC.velocity.X + num189 * 2f;
                            }
                            else
                            {
                                NPC.velocity.X = NPC.velocity.X - num189 * 2f;
                            }
                        }
                    }
                    else
                    {
                        if (num196 > num197)
                        {
                            if (NPC.velocity.X < num191)
                            {
                                NPC.velocity.X = NPC.velocity.X + num189 * 1.1f;
                            }
                            else if (NPC.velocity.X > num191)
                            {
                                NPC.velocity.X = NPC.velocity.X - num189 * 1.1f;
                            }
                            if ((double)(Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.5)
                            {
                                if (NPC.velocity.Y > 0f)
                                {
                                    NPC.velocity.Y = NPC.velocity.Y + num189;
                                }
                                else
                                {
                                    NPC.velocity.Y = NPC.velocity.Y - num189;
                                }
                            }
                        }
                        else
                        {
                            if (NPC.velocity.Y < num192)
                            {
                                NPC.velocity.Y = NPC.velocity.Y + num189 * 1.1f;
                            }
                            else if (NPC.velocity.Y > num192)
                            {
                                NPC.velocity.Y = NPC.velocity.Y - num189 * 1.1f;
                            }
                            if ((double)(Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.5)
                            {
                                if (NPC.velocity.X > 0f)
                                {
                                    NPC.velocity.X = NPC.velocity.X + num189;
                                }
                                else
                                {
                                    NPC.velocity.X = NPC.velocity.X - num189;
                                }
                            }
                        }
                    }
                }
                NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 1.57f;
                if (flag94)
                {
                    if (NPC.localAI[0] != 1f)
                    {
                        NPC.netUpdate = true;
                    }
                    NPC.localAI[0] = 1f;
                }
                else
                {
                    if (NPC.localAI[0] != 0f)
                    {
                        NPC.netUpdate = true;
                    }
                    NPC.localAI[0] = 0f;
                }
                if (((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f) || (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f) || (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f) || (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f)) && !NPC.justHit)
                {
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
			Vector2 vector11 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / 2));

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height)) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/Perforator/PerforatorHeadLargeGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Yellow, 0.5f);

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override bool CheckActive()
        {
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                    }
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("LargePerf").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("LargePerf2").Type, 1f);
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Large Perforator";
            potionType = ItemID.HealingPotion;
        }

        public override bool SpecialOnKill()
        {
            int closestSegmentID = DropHelper.FindClosestWormSegment(NPC.GetSource_FromThis(), NPC,
                ModContent.NPCType<PerforatorHeadLarge>(),
                ModContent.NPCType<PerforatorBodyLarge>(),
                ModContent.NPCType<PerforatorTailLarge>());
            NPC.position = Main.npc[closestSegmentID].position;
            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<BloodSample>(), 1, 4, 8);
            npcLoot.Add(ItemID.CrimtaneBar, 1, 3, 5);
            npcLoot.Add(ItemID.Vertebrae, 1, 2, 4);
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 300, true);
            target.AddBuff(BuffID.Bleeding, 300, true);
        }
    }
}
