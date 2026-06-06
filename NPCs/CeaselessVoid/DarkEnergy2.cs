using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Events;
using CalRD.Projectiles.Rogue;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.CeaselessVoid
{
    public class DarkEnergy2 : ModNPC
    {
        public int invinceTime = 120;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dark Energy");
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
		}

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
			NPC.GetNPCDamage();
			NPC.dontTakeDamage = true;
            NPC.width = 80;
            NPC.height = 80;
            NPC.defense = 50;
            NPC.lifeMax = 6000;
            if (CalamityWorld.DoGSecondStageCountdown <= 0 || !CalamityWorld.downedSentinel1)
            {
                NPC.lifeMax = 24000;
            }
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 44000;
            }
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0.1f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            AIType = -1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.HitSound = SoundID.NPCHit53;
            NPC.DeathSound = SoundID.NPCDeath44;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(invinceTime);
            writer.Write(NPC.dontTakeDamage);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            invinceTime = reader.ReadInt32();
            NPC.dontTakeDamage = reader.ReadBoolean();
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

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Texture2D texture2D16 = ModContent.Request<Texture2D>("CalRD/NPCs/CeaselessVoid/DarkEnergyGlow2").Value;
			Vector2 vector11 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			Color color36 = Color.White;
			float amount9 = 0.5f;
			int num153 = 5;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += 2)
				{
					Color color38 = drawColor;
					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (float)(num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - Main.screenPosition;
					vector41 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/CeaselessVoid/DarkEnergyGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Cyan, 0.5f);
			Color color42 = Color.Lerp(Color.White, Color.Fuchsia, 0.5f);

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num163 = 1; num163 < num153; num163++)
				{
					Color color41 = color37;
					color41 = Color.Lerp(color41, color36, amount9);
					color41 *= (float)(num153 - num163) / 15f;
					Vector2 vector44 = NPC.oldPos[num163] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - Main.screenPosition;
					vector44 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
					vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector44, NPC.frame, color41, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

					Color color43 = color42;
					color43 = Color.Lerp(color43, color36, amount9);
					color43 *= (float)(num153 - num163) / 15f;
					spriteBatch.Draw(texture2D16, vector44, NPC.frame, color43, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			spriteBatch.Draw(texture2D16, vector43, NPC.frame, color42, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override void AI()
        {
            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
            if (invinceTime > 0)
            {
				NPC.damage = 0;
                invinceTime--;
            }
            else
            {
                NPC.damage = NPC.defDamage;
                NPC.dontTakeDamage = false;
            }

			if (CalamityGlobalNPC.voidBoss < 0 || !Main.npc[CalamityGlobalNPC.voidBoss].active)
			{
				NPC.active = false;
				NPC.netUpdate = true;
				return;
			}

			double mult = 0.5 +
                (CalamityWorld.revenge ? 0.2 : 0.0) +
                (CalamityWorld.death ? 0.2 : 0.0);

            if (NPC.life < NPC.lifeMax * mult || BossRushEvent.BossRushActive)
                NPC.knockBackResist = 0f;

			float tileEnrageMult = Main.npc[CalamityGlobalNPC.voidBoss].ai[1];

			float num1247 = 0.1f;
			float maxDistance = 24f * MathHelper.Lerp(0.3333333334f, 1.6f, MathHelper.Clamp((tileEnrageMult - 1f) * 1.6666666667f, 0f, 1f));
			for (int num1248 = 0; num1248 < Main.maxNPCs; num1248++)
			{
				if (Main.npc[num1248].active)
				{
					if (num1248 != NPC.whoAmI && Main.npc[num1248].type == NPC.type)
					{
						if (Vector2.Distance(NPC.Center, Main.npc[num1248].Center) < maxDistance)
						{
							if (NPC.position.X < Main.npc[num1248].position.X)
								NPC.velocity.X = NPC.velocity.X - num1247;
							else
								NPC.velocity.X = NPC.velocity.X + num1247;

							if (NPC.position.Y < Main.npc[num1248].position.Y)
								NPC.velocity.Y = NPC.velocity.Y - num1247;
							else
								NPC.velocity.Y = NPC.velocity.Y + num1247;
						}
					}
				}
			}

			if (NPC.ai[1] == 0f)
            {
                NPC.scale -= 0.01f;
                NPC.alpha += 15;
                if (NPC.alpha >= 125)
                {
                    NPC.alpha = 130;
                    NPC.ai[1] = 1f;
                }
            }
            else if (NPC.ai[1] == 1f)
            {
                NPC.scale += 0.01f;
                NPC.alpha -= 15;
                if (NPC.alpha <= 0)
                {
                    NPC.alpha = 0;
                    NPC.ai[1] = 0f;
                }
            }

            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0f, -10f);

                    if (NPC.timeLeft > 150)
                        NPC.timeLeft = 150;

                    return;
                }
            }
            else if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

            Vector2 vector145 = new Vector2(NPC.Center.X, NPC.Center.Y);
            float num1258 = Main.player[NPC.target].Center.X - vector145.X;
            float num1259 = Main.player[NPC.target].Center.Y - vector145.Y;
            float num1260 = (float)Math.Sqrt((double)(num1258 * num1258 + num1259 * num1259));

            float num1261 = (expertMode ? 15f : 12f) * tileEnrageMult;
            if (CalamityWorld.revenge || BossRushEvent.BossRushActive)
                num1261 += 3f;
            if (CalamityWorld.death || BossRushEvent.BossRushActive)
                num1261 += 3f;

            num1260 = num1261 / num1260;
            num1258 *= num1260;
            num1259 *= num1260;
            NPC.velocity.X = (NPC.velocity.X * 100f + num1258) / 101f;
            NPC.velocity.Y = (NPC.velocity.Y * 100f + num1259) / 101f;
        }

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * balance);
		}

		public override bool CheckActive()
		{
			return CalamityGlobalNPC.voidBoss < 0 || !Main.npc[CalamityGlobalNPC.voidBoss].active;
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
			target.AddBuff(BuffID.VortexDebuff, 20, true);
		}

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
