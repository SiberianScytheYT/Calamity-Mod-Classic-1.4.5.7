using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Polterghast
{
	[AutoloadBossHead]
	public class PolterPhantom : ModNPC
    {
        private int despawnTimer = 600;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Polterghast");
            Main.npcFrameCount[NPC.type] = 4;
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
			NPC.width = 90;
            NPC.height = 120;
			NPC.LifeMaxNERB(130000, 150000, 900000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.Opacity = 0f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;
            NPC.buffImmune[BuffID.Daybreak] = false;
			NPC.buffImmune[BuffID.StardustMinionBleed] = false;
			NPC.buffImmune[BuffID.BetsysCurse] = false;
			NPC.buffImmune[BuffID.Oiled] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.canGhostHeal = false;
			NPC.dontTakeDamage = true;
            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCDeath39;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(despawnTimer);
			CalamityGlobalNPC cgn = NPC.Calamity();
			writer.Write(cgn.newAI[0]);
			writer.Write(cgn.newAI[1]);
			writer.Write(cgn.newAI[2]);
			writer.Write(cgn.newAI[3]);
		}

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            despawnTimer = reader.ReadInt32();
			CalamityGlobalNPC cgn = NPC.Calamity();
			cgn.newAI[0] = reader.ReadSingle();
			cgn.newAI[1] = reader.ReadSingle();
			cgn.newAI[2] = reader.ReadSingle();
			cgn.newAI[3] = reader.ReadSingle();
		}

        public override void AI()
        {
            CalamityGlobalNPC.ghostBossClone = NPC.whoAmI;

			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;

            if (CalamityGlobalNPC.ghostBoss < 0 || !Main.npc[CalamityGlobalNPC.ghostBoss].active)
            {
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }

            Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2)) / 16f), (int)((NPC.position.Y + (NPC.height / 2)) / 16f), 0.5f, 0.25f, 0.75f);

			Player player = Main.player[Main.npc[CalamityGlobalNPC.ghostBoss].target];

			// Percent life remaining, Polter
			float lifeRatio = Main.npc[CalamityGlobalNPC.ghostBoss].life / Main.npc[CalamityGlobalNPC.ghostBoss].lifeMax;

			Vector2 vector = NPC.Center;

			// Scale multiplier based on nearby active tiles
			float tileEnrageMult = Main.npc[CalamityGlobalNPC.ghostBoss].ai[3];
			bool chargePhase = Main.npc[CalamityGlobalNPC.ghostBoss].Calamity().newAI[0] >= 420f || NPC.Calamity().newAI[3] == 1f;
			float chargeVelocity = 24f;
			float chargeAcceleration = 0.6f;
			float chargeDistance = 480f;

			bool speedBoost1 = false;
            bool despawnBoost = false;
            bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;

            if (NPC.timeLeft < 1500)
                NPC.timeLeft = 1500;

            float velocity = 3f;
            float acceleration = 0.03f;
            if (!player.ZoneDungeon && !BossRushEvent.BossRushActive && player.position.Y < Main.worldSurface * 16.0)
            {
                despawnTimer--;
				if (despawnTimer <= 0)
				{
					despawnBoost = true;
					NPC.ai[1] = 0f;
					NPC.Calamity().newAI[0] = 0f;
					NPC.Calamity().newAI[1] = 0f;
					NPC.Calamity().newAI[2] = 0f;
					NPC.Calamity().newAI[3] = 0f;
				}

                speedBoost1 = true;
				velocity += 8f;
				acceleration = 0.15f;
            }
            else
                despawnTimer++;

            if (Main.npc[CalamityGlobalNPC.ghostBoss].ai[2] < 300f)
            {
				velocity = 21f;
				acceleration = 0.13f;
            }

			if (expertMode)
			{
				chargeVelocity += revenge ? 4f : 2f;
				velocity += revenge ? 5f : 3.5f;
				acceleration += revenge ? 0.035f : 0.025f;
			}

			// Look at target
			float num740 = player.Center.X - vector.X;
			float num741 = player.Center.Y - vector.Y;
			NPC.rotation = (float)Math.Atan2(num741, num740) + MathHelper.PiOver2;

			NPC.damage = NPC.defDamage;
			if (speedBoost1)
				NPC.damage *= 2;

			if (!chargePhase)
			{
				NPC.Opacity += 0.02f;
				if (NPC.Opacity > 0.8f)
					NPC.Opacity = 0.8f;

				float movementLimitX = 0f;
				float movementLimitY = 0f;
				int numHooks = 4;
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<PolterghastHook>())
					{
						movementLimitX += Main.npc[i].Center.X;
						movementLimitY += Main.npc[i].Center.Y;
					}
				}
				movementLimitX /= numHooks;
				movementLimitY /= numHooks;

				Vector2 vector91 = new Vector2(movementLimitX, movementLimitY);
				float num736 = player.Center.X - vector91.X;
				float num737 = player.Center.Y - vector91.Y;

				if (despawnBoost)
				{
					num737 *= -1f;
					num736 *= -1f;
					velocity += 8f;
				}

				float num738 = (float)Math.Sqrt(num736 * num736 + num737 * num737);
				float maxDistanceFromHooks = expertMode ? 650f : 500f;
				if (speedBoost1)
					maxDistanceFromHooks += 500f;
				if (death)
					maxDistanceFromHooks += maxDistanceFromHooks * 0.1f * (1f - lifeRatio);

				// Increase speed based on nearby active tiles
				velocity *= tileEnrageMult;
				acceleration *= tileEnrageMult;

				if (death)
				{
					velocity += velocity * 0.15f * (1f - lifeRatio);
					acceleration += acceleration * 0.15f * (1f - lifeRatio);
				}

				if (num738 >= maxDistanceFromHooks)
				{
					num738 = maxDistanceFromHooks / num738;
					num736 *= num738;
					num737 *= num738;
				}

				movementLimitX += num736;
				movementLimitY += num737;
				vector91 = vector;
				num736 = movementLimitX - vector91.X;
				num737 = movementLimitY - vector91.Y;
				num738 = (float)Math.Sqrt(num736 * num736 + num737 * num737);

				if (num738 < velocity)
				{
					num736 = NPC.velocity.X;
					num737 = NPC.velocity.Y;
				}
				else
				{
					num738 = velocity / num738;
					num736 *= num738;
					num737 *= num738;
				}

				if (NPC.velocity.X < num736)
				{
					NPC.velocity.X += acceleration;
					if (NPC.velocity.X < 0f && num736 > 0f)
						NPC.velocity.X += acceleration * 2f;
				}
				else if (NPC.velocity.X > num736)
				{
					NPC.velocity.X -= acceleration;
					if (NPC.velocity.X > 0f && num736 < 0f)
						NPC.velocity.X -= acceleration * 2f;
				}
				if (NPC.velocity.Y < num737)
				{
					NPC.velocity.Y += acceleration;
					if (NPC.velocity.Y < 0f && num737 > 0f)
						NPC.velocity.Y += acceleration * 2f;
				}
				else if (NPC.velocity.Y > num737)
				{
					NPC.velocity.Y -= acceleration;
					if (NPC.velocity.Y > 0f && num737 < 0f)
						NPC.velocity.Y -= acceleration * 2f;
				}
			}
			else
			{
				// Charge
				if (NPC.Calamity().newAI[3] == 1f)
				{
					NPC.Opacity += 0.06f;
					if (NPC.Opacity > 0.8f)
						NPC.Opacity = 0.8f;

					if (NPC.Calamity().newAI[1] == 0f)
					{
						NPC.velocity = Vector2.Normalize(player.Center - vector) * chargeVelocity;
						NPC.Calamity().newAI[1] = 1f;
					}
					else
					{
						NPC.Calamity().newAI[2] += 1f;

						// Slow down for a few frames
						float totalChargeTime = chargeDistance * 4f / chargeVelocity;
						float slowDownTime = chargeVelocity;
						if (NPC.Calamity().newAI[2] >= totalChargeTime - slowDownTime)
							NPC.velocity *= 0.9f;

						// Reset and either go back to normal or charge again
						if (NPC.Calamity().newAI[2] >= totalChargeTime)
						{
							NPC.Calamity().newAI[1] = 0f;
							NPC.Calamity().newAI[2] = 0f;
							NPC.Calamity().newAI[3] = 0f;
							NPC.ai[1] += 1f;

							if (NPC.ai[1] >= 3f)
							{
								// Reset and return to normal movement
								NPC.Calamity().newAI[0] = 0f;
								NPC.ai[1] = 0f;
							}
						}
					}
				}
				else
				{
					// Random location choice
					if (NPC.ai[0] == 0f)
					{
						NPC.velocity = Vector2.Zero;
						NPC.ai[0] = Main.rand.Next(2) + 1;
					}

					// Pick a charging location
					// Set charge locations X
					if (Main.npc[CalamityGlobalNPC.ghostBoss].Center.X >= player.Center.X)
						NPC.Calamity().newAI[1] = NPC.ai[0] == 1f ? player.Center.X - chargeDistance : Main.npc[CalamityGlobalNPC.ghostBoss].Calamity().newAI[1];
					else
						NPC.Calamity().newAI[1] = NPC.ai[0] == 1f ? player.Center.X + chargeDistance : Main.npc[CalamityGlobalNPC.ghostBoss].Calamity().newAI[1];

					// Set charge locations Y
					if (Main.npc[CalamityGlobalNPC.ghostBoss].Center.Y >= player.Center.Y)
						NPC.Calamity().newAI[2] = NPC.ai[0] == 2f ? player.Center.Y - chargeDistance : Main.npc[CalamityGlobalNPC.ghostBoss].Calamity().newAI[2];
					else
						NPC.Calamity().newAI[2] = NPC.ai[0] == 2f ? player.Center.Y + chargeDistance : Main.npc[CalamityGlobalNPC.ghostBoss].Calamity().newAI[2];

					// Do not deal damage during movement to avoid cheap bullshit hits
					NPC.damage = 0;

					// Charge location
					Vector2 chargeVector = new Vector2(NPC.Calamity().newAI[1], NPC.Calamity().newAI[2]);
					Vector2 chargeLocationVelocity = Vector2.Normalize(chargeVector - vector) * chargeVelocity;

					// Line up a charge
					float chargeDistanceGateValue = 32f;

					if (Vector2.Distance(vector, chargeVector) <= chargeDistanceGateValue * 3f)
					{
						NPC.Opacity += 0.06f;
						if (NPC.Opacity > 0.8f)
							NPC.Opacity = 0.8f;
					}
					else
					{
						NPC.Opacity -= 0.06f;
						if (NPC.Opacity < 0f)
							NPC.Opacity = 0f;
					}

					if (Vector2.Distance(vector, chargeVector) <= chargeDistanceGateValue)
						NPC.velocity *= 0.8f;
					else
					{
						if (Vector2.Distance(vector, chargeVector) > 1200f)
							NPC.velocity = chargeLocationVelocity;
						else
							NPC.SimpleFlyMovement(chargeLocationVelocity, chargeAcceleration);
					}
				}

				NPC.netUpdate = true;

				if (NPC.netSpam > 10)
					NPC.netSpam = 10;

				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI, 0f, 0f, 0f, 0, 0, 0);
			}
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return new Color(200, 150, 255) * NPC.Opacity;
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
			Color color36 = Color.White;
			Color lightRed = new Color(255, 100, 100, 255) * NPC.Opacity;
			float amount9 = 0.5f;
			int num153 = 7;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += 2)
				{
					Color color38 = drawColor;

					if (Main.npc[CalamityGlobalNPC.ghostBoss].Calamity().newAI[0] > 300f)
						color38 = Color.Lerp(color38, lightRed, MathHelper.Clamp((Main.npc[CalamityGlobalNPC.ghostBoss].Calamity().newAI[0] - 300f) / 120f, 0f, 1f));

					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					vector41 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Color color = NPC.GetAlpha(drawColor);

			if (Main.npc[CalamityGlobalNPC.ghostBoss].Calamity().newAI[0] > 300f)
				color = Color.Lerp(color, lightRed, MathHelper.Clamp((Main.npc[CalamityGlobalNPC.ghostBoss].Calamity().newAI[0] - 300f) / 120f, 0f, 1f));

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			Texture2D texture2D16 = ModContent.Request<Texture2D>("CalRD/NPCs/Polterghast/PolterPhantomGlow").Value;
			Color color42 = Color.Lerp(Color.White, Color.Red, 0.5f);

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num163 = 1; num163 < num153; num163++)
				{
					Vector2 vector44 = NPC.oldPos[num163] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					vector44 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					Color color43 = color42;
					color43 = Color.Lerp(color43, color36, amount9);
					color43 = NPC.GetAlpha(color43);
					color43 *= (num153 - num163) / 15f;
					spriteBatch.Draw(texture2D16, vector44, NPC.frame, color43, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			spriteBatch.Draw(texture2D16, vector43, NPC.frame, color42, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1.0;
            if (NPC.frameCounter > 6.0)
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y = NPC.frame.Y + frameHeight;
            }
            if (NPC.frame.Y > frameHeight * 3)
            {
                NPC.frame.Y = 0;
            }
        }

		public override bool CheckActive()
		{
			return false;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
			target.AddBuff(BuffID.MoonLeech, 360, true);
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

        public override void HitEffect(NPC.HitInfo hit)
        {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, 180, hit.HitDirection, -1f, 0, default, 1f);
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 90;
                NPC.height = 90;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 10; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Phantoplasm, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 60; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 180, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 180, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }
    }
}
