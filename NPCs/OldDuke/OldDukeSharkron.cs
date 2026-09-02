using CalRD.Buffs.StatDebuffs;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Dusts;
using CalRD.Projectiles.Boss;
using CalRD.Events;

namespace CalRD.NPCs.OldDuke
{
	public class OldDukeSharkron : ModNPC
	{
		bool spawnedProjectiles = false;
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Sulphurous Sharkron");
			NPCID.Sets.TrailingMode[NPC.type] = 1;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}
		
		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			AIType = -1;
			NPC.width = 44;
			NPC.height = 44;
			NPC.GetNPCDamage();
			NPC.defense = 100;
			NPC.lifeMax = 8000;
			if (BossRushEvent.BossRushActive)
			{
				NPC.lifeMax = 100000;
			}
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.alpha = 255;
			NPC.noGravity = true;
			NPC.dontTakeDamage = true;
			NPC.noTileCollide = true;
			for (int k = 0; k < NPC.buffImmune.Length; k++)
			{
				NPC.buffImmune[k] = true;
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(NPC.dontTakeDamage);
			writer.Write(NPC.noGravity);
			writer.Write(spawnedProjectiles);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			NPC.dontTakeDamage = reader.ReadBoolean();
			NPC.noGravity = reader.ReadBoolean();
			spawnedProjectiles = reader.ReadBoolean();
		}

		public override void AI()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest(false);
				NPC.netUpdate = true;
			}

			if (NPC.velocity.X < 0f)
			{
				NPC.spriteDirection = -1;
				NPC.rotation = (float)Math.Atan2(-NPC.velocity.Y, -NPC.velocity.X);
			}
			else
			{
				NPC.spriteDirection = 1;
				NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
			}

			NPC.alpha -= 6;
			if (NPC.alpha < 0)
				NPC.alpha = 0;

			bool normalAI = NPC.ai[3] == 0f;
			bool upwardAI = NPC.ai[3] < 0f;
			bool downwardAI = NPC.ai[3] > 0f;

			float aiGateValue = normalAI ? 210f : 175f;
			float maxVelocity = 18f;

			if (NPC.ai[0] == 0f)
			{
				if (NPC.ai[1] == 0f)
				{
					if (normalAI)
						NPC.velocity = Vector2.Normalize(Main.npc[(int)NPC.ai[2]].Center - NPC.Center) * (maxVelocity - 6f);
					else
						NPC.velocity = new Vector2(NPC.ai[2], NPC.ai[3]);

					SoundEngine.PlaySound(SoundID.NPCDeath19, NPC.position);
				}

				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= 90f)
				{
					if (!Collision.SolidCollision(NPC.position, NPC.width, NPC.height) && NPC.ai[1] >= aiGateValue)
					{
						NPC.ai[0] = 1f;
					}

					if (!normalAI)
					{
						if (NPC.velocity.Length() < maxVelocity)
							NPC.velocity *= 1.01f;
					}

					float scaleFactor2 = NPC.velocity.Length();
					Vector2 vector17 = Main.player[NPC.target].Center - NPC.Center;
					vector17.Normalize();
					vector17 *= scaleFactor2;
					NPC.velocity = (NPC.velocity * 24f + vector17) / 25f;
					NPC.velocity.Normalize();
					NPC.velocity *= scaleFactor2;
				}
			}
			else if (NPC.ai[0] == 1f)
			{
				if (upwardAI)
					maxVelocity = 12f;

				if (NPC.velocity.Length() > maxVelocity)
					NPC.velocity *= 0.99f;

				NPC.dontTakeDamage = false;

				NPC.ai[1] += 1f;
				if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height) || NPC.ai[1] >= aiGateValue + 120f)
				{
					if (NPC.DeathSound != null)
					{
						SoundEngine.PlaySound(NPC.DeathSound, NPC.position);
					}

					NPC.life = 0;
					NPC.HitEffect(0, 10.0);
					NPC.checkDead();
					NPC.active = false;
					return;
				}

				if (NPC.ai[1] >= aiGateValue + 60f)
				{
					NPC.noGravity = false;
					NPC.velocity.Y += 0.3f;
				}
			}
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.FlipHorizontally;
			if (NPC.spriteDirection == -1)
				spriteEffects = SpriteEffects.None;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
			Color color36 = Color.Lime;
			float amount9 = 0.5f;
			int num153 = 10;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += 2)
				{
					Color color38 = drawColor;
					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					vector41 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			cooldownSlot = 1;
			return NPC.alpha == 0;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			target.AddBuff(BuffID.Venom, 180, true);
			target.AddBuff(BuffID.Rabies, 180, true);
			target.AddBuff(BuffID.Poisoned, 180, true);
			target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
		}

        public override bool CheckDead()
		{
			SoundEngine.PlaySound(SoundID.NPCDeath12, NPC.position);

			NPC.position.X = NPC.position.X + (NPC.width / 2);
            NPC.position.Y = NPC.position.Y + (NPC.height / 2);
            NPC.width = NPC.height = 96;
            NPC.position.X = NPC.position.X - (NPC.width / 2);
            NPC.position.Y = NPC.position.Y - (NPC.height / 2);

			for (int num621 = 0; num621 < 15; num621++)
			{
				int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, 0f, 0f, 100, default, 2f);
				Main.dust[num622].velocity.Y *= 6f;
				Main.dust[num622].velocity.X *= 3f;
				if (Main.rand.NextBool(2))
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
				}
			}

			for (int num623 = 0; num623 < 30; num623++)
			{
				int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Blood, 0f, 0f, 100, default, 3f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity.Y *= 10f;
				num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Blood, 0f, 0f, 100, default, 2f);
				Main.dust[num624].velocity.X *= 2f;
			}

			if (Main.netMode != NetmodeID.MultiplayerClient && !spawnedProjectiles)
			{
				spawnedProjectiles = true;
				int spawnX = NPC.width / 2;
				int damage = Main.expertMode ? 55 : 70;
				for (int i = 0; i < 2; i++)
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X + Main.rand.Next(-spawnX, spawnX), NPC.Center.Y,
						Main.rand.Next(-3, 4), Main.rand.Next(-12, -6), ModContent.ProjectileType<OldDukeGore>(), damage, 0f, Main.myPlayer, 0f, 0f);
			}

			return true;
        }

        public override void HitEffect(NPC.HitInfo hit)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
			}
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
				}
			}
		}
	}
}