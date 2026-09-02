using CalRD.Dusts;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.SupremeCalamitas
{
    public class SCalWormHead : ModNPC
    {
        private const int minLength = 51;
        private const int maxLength = 52;
        private float passedVar = 0f;
        private bool TailSpawned = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sepulcher");
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
	            Scale = 0.8f,
	            PortraitScale = 0.8f,
	            CustomTexturePath = "CalRD/ExtraTextures/Bestiary/Sepulcher_Bestiary",
	            PortraitPositionXOverride = 40f,
	            PortraitPositionYOverride = 40
            };
            value.Position.X += 50f;
            value.Position.Y += 35f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
	        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
	        {
		        new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
		        new FlavorTextBestiaryInfoElement("A mechanical abomination summoned by the Brimstone Witch. Its armor proves unbreakable.")
	        });
        }

        public override void SetDefaults()
        {
            NPC.damage = 0; //150
            NPC.npcSlots = 5f;
            NPC.width = 62; //324
            NPC.height = 78; //216
            NPC.defense = 0;
            CalamityGlobalNPC global = NPC.Calamity();
            global.DR = 0.999999f;
            global.unbreakableDR = true;
            NPC.lifeMax = CalamityWorld.revenge ? 1200000 : 1000000;
            if (CalamityWorld.death)
            {
                NPC.lifeMax = 2000000;
            }
            NPC.aiStyle = -1; //new
            AIType = -1; //new
            NPC.knockBackResist = 0f;
            NPC.scale = 1.2f;
            if (Main.expertMode)
            {
                NPC.scale = 1.35f;
            }
            NPC.alpha = 255;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.chaseable = false;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.netAlways = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
			writer.Write(NPC.alpha);
		}

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
			NPC.alpha = reader.ReadInt32();
		}

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void AI()
        {
            CalamityGlobalNPC.SCalWorm = NPC.whoAmI;

			// Target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				NPC.TargetClosest(true);

			Player player = Main.player[NPC.target];

			if (NPC.ai[3] > 0f)
			{
				NPC.realLife = (int)NPC.ai[3];
			}

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!TailSpawned && NPC.ai[0] == 0f)
                {
                    int Previous = NPC.whoAmI;
                    for (int num36 = 0; num36 < maxLength; num36++)
                    {
                        int lol;
                        if (num36 >= 0 && num36 < minLength && num36 % 2 == 0)
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<SCalWormBodyWeak>(), NPC.whoAmI);
                            Main.npc[lol].localAI[0] += passedVar;
                            passedVar += 36f;
                        }
                        else if (num36 >= 0 && num36 < minLength)
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<SCalWormBody>(), NPC.whoAmI);
                            if (NPC.localAI[0] % 2 == 0)
                            {
                                Main.npc[lol].localAI[3] = 1f;
                                NPC.localAI[0] = 1f;
                            }
                            else
                            {
                                NPC.localAI[0] = 2f;
                            }
                        }
                        else
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<SCalWormTail>(), NPC.whoAmI);
                        }
                        Main.npc[lol].realLife = NPC.whoAmI;
                        Main.npc[lol].ai[2] = NPC.whoAmI;
                        Main.npc[lol].ai[1] = Previous;
                        Main.npc[Previous].ai[0] = lol;
                        Previous = lol;
                    }
                    TailSpawned = true;
                }
                if (!NPC.active && Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
            }

            if (Main.player[NPC.target].dead || !NPC.AnyNPCs(ModContent.NPCType<SCalWormHeart>()) || CalamityGlobalNPC.SCal < 0 || !Main.npc[CalamityGlobalNPC.SCal].active)
            {
                NPC.TargetClosest(false);
                NPC.alpha += 5;
                if (NPC.alpha >= 255)
                {
                    NPC.alpha = 255;
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						if (Main.npc[i].type == NPC.type || Main.npc[i].type == ModContent.NPCType<SCalWormBody>() || Main.npc[i].type == ModContent.NPCType<SCalWormBodyWeak>() || Main.npc[i].type == ModContent.NPCType<SCalWormTail>())
						{
							Main.npc[i].active = false;
						}
					}
				}
            }
            else
            {
                NPC.alpha -= 42;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
            }

			Vector2 vector18 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
			float num191 = Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2);
			float num192 = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2);
			float num188 = 10f;
			float num189 = 0.1f;

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
			NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + MathHelper.PiOver2;
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

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/SupremeCalamitas/SCalWormHeadGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Red, 0.5f);

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
			if (CalamityLists.projectileDestroyExceptionList.TrueForAll(x => projectile.type != x))
			{
				if (projectile.penetrate == -1 && !projectile.minion)
				{
					projectile.penetrate = 1;
				}
				else if (projectile.penetrate >= 1)
				{
					projectile.penetrate = 1;
				}
			}
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
                NPC.width = 50;
                NPC.height = 50;
                NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
                for (int num621 = 0; num621 < 5; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 10; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }
    }
}
