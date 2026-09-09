using CalRD.Dusts;
using CalRD.Events;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Ranged;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.StormWeaver
{
    [AutoloadBossHead]
    public class StormWeaverHeadNaked : ModNPC
    {
        private const float BoltAngleSpread = 280;
        private const float speed = 13f;
        private const float turnSpeed = 0.35f;
        private bool tail = false;
        private int invinceTime = 180;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Storm Weaver");
			NPCID.Sets.TrailingMode[NPC.type] = 1;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 5f;
            NPC.width = 74;
            NPC.height = 74;
			bool notDoGFight = CalamityWorld.DoGSecondStageCountdown <= 0 || !CalamityWorld.downedSentinel2;
			NPC.LifeMaxNERB(notDoGFight ? 900000 : 150000, notDoGFight ? 900000 : 150000, 3500000);
			Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/ScourgeofTheUniverse");
            if (notDoGFight)
            {
                NPC.value = Item.buyPrice(0, 35, 0, 0);
                Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/Weaver");
            }
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.alpha = 255;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit13;
            NPC.DeathSound = SoundID.NPCDeath13;
            NPC.netAlways = true;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }

			if (CalamityWorld.death || BossRushEvent.BossRushActive)
				NPC.scale = 1.2f;
			else if (CalamityWorld.revenge)
				NPC.scale = 1.15f;
			else if (Main.expertMode)
				NPC.scale = 1.1f;
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(invinceTime);
            writer.Write(NPC.dontTakeDamage);
			writer.Write(NPC.localAI[1]);
			writer.Write(NPC.localAI[2]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            invinceTime = reader.ReadInt32();
            NPC.dontTakeDamage = reader.ReadBoolean();
			NPC.localAI[1] = reader.ReadSingle();
			NPC.localAI[2] = reader.ReadSingle();
        }

        public override void AI()
        {
			CalamityGlobalNPC calamityGlobalNPC = NPC.Calamity();

			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;

            if (invinceTime > 0)
            {
                invinceTime--;
                NPC.damage = 0;
                NPC.dontTakeDamage = true;
            }
            else
            {
                NPC.damage = NPC.defDamage;
                NPC.dontTakeDamage = false;
            }

            if (!Main.raining && !BossRushEvent.BossRushActive && CalamityWorld.DoGSecondStageCountdown <= 0)
            {
				CalamityUtils.StartRain();
            }

            double lifeRatio = NPC.life / (double)NPC.lifeMax;
            int lifePercentage = (int)(100.0 * lifeRatio);

			int BoltProjectiles = 2;
            if (lifePercentage < 33 || death)
            {
                BoltProjectiles = 4;
            }
            else if (lifePercentage < 66)
            {
                BoltProjectiles = 3;
            }

            Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2)) / 16f), (int)((NPC.position.Y + (NPC.height / 2)) / 16f), 0.2f, 0.05f, 0.2f);

            if (NPC.ai[3] > 0f)
            {
                NPC.realLife = (int)NPC.ai[3];
            }

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(true);
            }

            if (NPC.alpha != 0)
            {
                for (int num934 = 0; num934 < 2; num934++)
                {
                    int num935 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 182, 0f, 0f, 100, default, 2f);
                    Main.dust[num935].noGravity = true;
                    Main.dust[num935].noLight = true;
                }
            }

            NPC.alpha -= 12;
            if (NPC.alpha < 0)
            {
                NPC.alpha = 0;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!tail && NPC.ai[0] == 0f)
                {
                    int Previous = NPC.whoAmI;
					int totalLength = death ? 60 : revenge ? 50 : expertMode ? 40 : 30;
					for (int num36 = 0; num36 < totalLength; num36++)
                    {
                        int lol;
                        if (num36 >= 0 && num36 < totalLength - 1)
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<StormWeaverBodyNaked>(), NPC.whoAmI);
                        }
                        else
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<StormWeaverTailNaked>(), NPC.whoAmI);
                        }
                        Main.npc[lol].realLife = NPC.whoAmI;
                        Main.npc[lol].ai[2] = NPC.whoAmI;
                        Main.npc[lol].ai[1] = Previous;
                        Main.npc[Previous].ai[0] = lol;
                        NPC.netUpdate = true;
                        Previous = lol;
                    }
                    tail = true;
                }

				if (expertMode)
				{
					NPC.localAI[0] += 1f;
					if (NPC.localAI[0] >= 450f)
					{
						NPC.localAI[0] = 0f;
						NPC.TargetClosest(true);
						NPC.netUpdate = true;

						int random = Main.rand.Next(250, 501);
						float xPos = Main.rand.NextBool(2) ? Main.player[NPC.target].position.X + random : Main.player[NPC.target].position.X - random;
						random = Main.rand.Next(250, 501);
						float yPos = Main.rand.NextBool(2) ? Main.player[NPC.target].position.Y + random : Main.player[NPC.target].position.Y - random;
						Vector2 spawnPos = new Vector2(xPos, yPos);

						int type = ProjectileID.CultistBossLightningOrb;
						int damage = NPC.GetProjectileDamage(type);
						Projectile.NewProjectile(Entity.GetSource_FromThis(), spawnPos, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);
					}
				}
            }

            if (Main.player[NPC.target].dead && NPC.life > 0)
            {
				NPC.localAI[1] = 0f;
				calamityGlobalNPC.newAI[0] = 0f;
				NPC.TargetClosest(false);
                NPC.velocity.Y = NPC.velocity.Y - 10f;
                if ((double)NPC.position.Y < Main.topWorld + 16f)
                {
                    NPC.velocity.Y = NPC.velocity.Y - 10f;
                }
                if ((double)NPC.position.Y < Main.topWorld + 16f)
                {
                    CalamityWorld.DoGSecondStageCountdown = 0;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var netMessage = Mod.GetPacket();
                        netMessage.Write((byte)CalRDMessageType.DoGCountdownSync);
                        netMessage.Write(CalamityWorld.DoGSecondStageCountdown);
                        netMessage.Send();
                    }
                    for (int num957 = 0; num957 < 200; num957++)
                    {
                        if (Main.npc[num957].active && (Main.npc[num957].type == ModContent.NPCType<StormWeaverBodyNaked>() 
                            || Main.npc[num957].type == ModContent.NPCType<StormWeaverHeadNaked>()
                            || Main.npc[num957].type == ModContent.NPCType<StormWeaverTailNaked>()))
                        {
                            Main.npc[num957].active = false;
                        }
                    }
                }
            }

            if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > 10000f && NPC.life > 0)
            {
                CalamityWorld.DoGSecondStageCountdown = 0;
                if (Main.netMode == NetmodeID.Server)
                {
                    var netMessage = Mod.GetPacket();
                    netMessage.Write((byte)CalRDMessageType.DoGCountdownSync);
                    netMessage.Write(CalamityWorld.DoGSecondStageCountdown);
                    netMessage.Send();
                }
                for (int num957 = 0; num957 < 200; num957++)
                {
                    if (Main.npc[num957].type == ModContent.NPCType<StormWeaverBodyNaked>()
                       || Main.npc[num957].type == ModContent.NPCType<StormWeaverHeadNaked>()
                       || Main.npc[num957].type == ModContent.NPCType<StormWeaverTailNaked>())
                    {
                        Main.npc[num957].active = false;
                    }
                }
            }

            if (NPC.velocity.X < 0f)
            {
                NPC.spriteDirection = -1;
            }
            else if (NPC.velocity.X > 0f)
            {
                NPC.spriteDirection = 1;
            }

            if (Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(false);
            }

            Vector2 vector18 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
            float num191 = Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2);
            float num192 = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2);
            float num188 = revenge ? 13f : 12f;
            float num189 = revenge ? 0.31f : 0.28f;

			calamityGlobalNPC.newAI[0] += 1f;
			if (calamityGlobalNPC.newAI[0] >= 400f)
			{
				if (NPC.localAI[1] == 0f)
					NPC.localAI[1] = 1f;

				if (calamityGlobalNPC.newAI[0] >= 500f)
				{
					NPC.localAI[1] = 0f;
					calamityGlobalNPC.newAI[0] = 0f;
				}

				if (revenge)
				{
					if (NPC.localAI[1] == 2f)
					{
						num188 += Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) * 0.01f * (1f - (float)lifeRatio);
						num189 += Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) * 0.0001f * (1f - (float)lifeRatio);
						num188 *= 2f;
						num189 *= 0.85f;

						float stopChargeDistance = 800f * NPC.localAI[2];
						if (stopChargeDistance < 0)
						{
							if (NPC.Center.X < Main.player[NPC.target].Center.X + stopChargeDistance)
							{
								NPC.localAI[1] = 0f;
								calamityGlobalNPC.newAI[0] = 0f;
							}
						}
						else
						{
							if (NPC.Center.X > Main.player[NPC.target].Center.X + stopChargeDistance)
							{
								NPC.localAI[1] = 0f;
								calamityGlobalNPC.newAI[0] = 0f;
							}
						}
					}

					int dustAmt = 5;
					for (int num1474 = 0; num1474 < dustAmt; num1474++)
					{
						Vector2 vector171 = Vector2.Normalize(NPC.velocity) * new Vector2((NPC.width + 50) / 2f, NPC.height) * 0.75f;
						vector171 = vector171.RotatedBy((num1474 - (dustAmt / 2 - 1)) * (double)MathHelper.Pi / (float)dustAmt) + NPC.Center;
						Vector2 value18 = ((float)(Main.rand.NextDouble() * MathHelper.Pi) - MathHelper.PiOver2).ToRotationVector2() * Main.rand.Next(3, 8);
						int num1475 = Dust.NewDust(vector171 + value18, 0, 0, 206, value18.X, value18.Y, 100, default, 3f);
						Main.dust[num1475].noGravity = true;
						Main.dust[num1475].noLight = true;
						Main.dust[num1475].velocity /= 4f;
						Main.dust[num1475].velocity -= NPC.velocity;
					}
				}
			}
			else if (revenge)
				calamityGlobalNPC.newAI[0] += death ? 2f : 2f * (float)(1D - lifeRatio);

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

			if (NPC.localAI[1] == 1f)
			{
				NPC.localAI[1] = 2f;
				SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/LightningStrike"), Main.player[NPC.target].Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int speed2 = revenge ? 8 : 7;
					if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
					{
						speed2 += 1;
					}
					float spawnX2 = NPC.Center.X > Main.player[NPC.target].Center.X ? 1000f : -1000f;
					float spawnY2 = -1000f + Main.player[NPC.target].Center.Y;
					Vector2 baseSpawn = new Vector2(spawnX2 + Main.player[NPC.target].Center.X, spawnY2);
					Vector2 baseVelocity = Main.player[NPC.target].Center - baseSpawn;
					baseVelocity.Normalize();
					baseVelocity *= speed2;
					for (int i = 0; i < BoltProjectiles; i++)
					{
						Vector2 source = baseSpawn;
						source.X += i * 30 - (BoltProjectiles * 15);
						Vector2 velocity = baseVelocity.RotatedBy(MathHelper.ToRadians(-BoltAngleSpread / 2 + (BoltAngleSpread * i / BoltProjectiles)));
						velocity.X = velocity.X + 3 * Main.rand.NextFloat() - 1.5f;
						Vector2 vector94 = Main.player[NPC.target].Center - source;
						float ai = Main.rand.Next(100);
						int type = ProjectileID.CultistBossLightningOrbArc;
						int damage = NPC.GetProjectileDamage(type);
						Projectile.NewProjectile(Entity.GetSource_FromThis(), source, velocity, type, damage, 0f, Main.myPlayer, vector94.ToRotation(), ai);
					}
				}

				if (revenge)
					NPC.velocity = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * (num188 + Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) * 0.01f * (1f - (float)lifeRatio)) * 2f;

				float chargeDirection = 0;
				if (NPC.velocity.X < 0f)
					chargeDirection = -1f;
				else if (NPC.velocity.X > 0f)
					chargeDirection = 1f;

				NPC.localAI[2] = chargeDirection;
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
                if (Math.Abs(num192) < num188 * 0.2 && ((NPC.velocity.X > 0f && num191 < 0f) || (NPC.velocity.X < 0f && num191 > 0f)))
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
                if (Math.Abs(num191) < num188 * 0.2 && ((NPC.velocity.Y > 0f && num192 < 0f) || (NPC.velocity.Y < 0f && num192 > 0f)))
                {
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X + num189 * 2f; //changed from 2
                    }
                    else
                    {
                        NPC.velocity.X = NPC.velocity.X - num189 * 2f; //changed from 2
                    }
                }
            }
            else
            {
                if (num196 > num197)
                {
                    if (NPC.velocity.X < num191)
                    {
                        NPC.velocity.X = NPC.velocity.X + num189 * 1.1f; //changed from 1.1
                    }
                    else if (NPC.velocity.X > num191)
                    {
                        NPC.velocity.X = NPC.velocity.X - num189 * 1.1f; //changed from 1.1
                    }
                    if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < num188 * 0.5)
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
                    if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < num188 * 0.5)
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
            NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / 2);
			Color color36 = Color.White;
			float amount9 = 0.5f;
			int num153 = 5;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += 2)
				{
					Color color38 = drawColor;

					if (NPC.Calamity().newAI[0] > 280f && (CalamityWorld.revenge || BossRushEvent.BossRushActive))
						color38 = Color.Lerp(color38, Color.Cyan, MathHelper.Clamp((NPC.Calamity().newAI[0] - 280f) / 120f, 0f, 1f));

					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					vector41 -= new Vector2(texture2D15.Width, texture2D15.Height) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2(texture2D15.Width, texture2D15.Height) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			Color color = NPC.GetAlpha(drawColor);

			if (NPC.Calamity().newAI[0] > 280f && (CalamityWorld.revenge || BossRushEvent.BossRushActive))
				color = Color.Lerp(color, Color.Cyan, MathHelper.Clamp((NPC.Calamity().newAI[0] - 280f) / 120f, 0f, 1f));

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

            return false;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			int buffDuration = NPC.Calamity().newAI[0] >= 400f ? 300 : 90;
			target.AddBuff(BuffID.Electrified, buffDuration, true);
		}

		public override bool CheckActive()
        {
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
	            if (Main.netMode != NetmodeID.Server)
	            {
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SWNudeHead1").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SWNudeHead2").Type, 1f);
                }
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 30;
                NPC.height = 30;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 20; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 40; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override bool CheckDead()
        {
            for (int num569 = 0; num569 < 200; num569++)
            {
                if (Main.npc[num569].active && (Main.npc[num569].type == ModContent.NPCType<StormWeaverBodyNaked>() || Main.npc[num569].type == ModContent.NPCType<StormWeaverTailNaked>()))
                {
                    Main.npc[num569].life = 0;
                }
            }
            return true;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override bool SpecialOnKill()
        {
            int closestSegmentID = DropHelper.FindClosestWormSegment(NPC.GetSource_FromThis(), NPC,
                ModContent.NPCType<StormWeaverHeadNaked>(),
                ModContent.NPCType<StormWeaverBodyNaked>(),
                ModContent.NPCType<StormWeaverTailNaked>());
            NPC.position = Main.npc[closestSegmentID].position;
            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Only drop items if fought alone
            var alone = new LeadingConditionRule(DropHelper.If(() => CalamityWorld.DoGSecondStageCountdown <= 0));
            {
                // Materials
                alone.AddPerPlayer(ModContent.ItemType<ArmoredShell>(), 1, 5, 8);

                // Weapons
                alone.AddIf(() => Main.expertMode, ModContent.ItemType<TheStorm>(), 3);
                alone.AddIf(() => !Main.expertMode, ModContent.ItemType<TheStorm>(), 4);
                alone.AddIf(() => Main.expertMode, ModContent.ItemType<StormDragoon>(), 3);
                alone.AddIf(() => !Main.expertMode, ModContent.ItemType<StormDragoon>(), 4);
                alone.Add(ModContent.ItemType<Thunderstorm>(), DropHelper.RareVariantDropRateInt);

                // Vanity
                alone.Add(ModContent.ItemType<WeaverTrophy>(), 10);
                alone.Add(ModContent.ItemType<StormWeaverMask>(), 7);
                var godSlayerVanity = ItemDropRule.Common(ModContent.ItemType<AncientGodSlayerHelm>(), 20);
                godSlayerVanity.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientGodSlayerChestplate>()));
                godSlayerVanity.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientGodSlayerLeggings>()));
                alone.Add(godSlayerVanity);

                // Other
                bool lastSentinelKilled = CalamityWorld.downedSentinel1 && !CalamityWorld.downedSentinel2 && CalamityWorld.downedSentinel3;
                alone.AddConditionalPerPlayer(() => lastSentinelKilled, ModContent.ItemType<KnowledgeSentinels>(), 1);
                alone.AddResidentEvilAmmo(info => !CalamityWorld.downedSentinel2, 5, 2, 1);
            }
        }

        public override void OnKill()
        {
	        // If DoG's fight is active, set the timer for Signus' phase
	        if (CalamityWorld.DoGSecondStageCountdown > 7260)
	        {
		        CalamityWorld.DoGSecondStageCountdown = 7260;
		        if (Main.netMode == NetmodeID.Server)
		        {
			        var netMessage = Mod.GetPacket();
			        netMessage.Write((byte)CalRDMessageType.DoGCountdownSync);
			        netMessage.Write(CalamityWorld.DoGSecondStageCountdown);
			        netMessage.Send();
		        }
	        }

	        // Mark Storm Weaver as dead
	        if (CalamityWorld.DoGSecondStageCountdown <= 0)
	        {
		        CalamityWorld.downedSentinel2 = true;
		        CalamityNetcode.SyncWorld();
	        }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
        }
    }
}
