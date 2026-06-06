using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.PlaguebringerGoliath
{
    public class PlagueBeeG : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plague Charger");
            Main.npcFrameCount[NPC.type] = 4;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
		}

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.width = 36;
            NPC.height = 30;
            NPC.defense = 15;
            NPC.scale = 0.75f;
            NPC.lifeMax = 300;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 50000;
            }
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = BossRushEvent.BossRushActive ? 0f : 0.9f;
            AnimationType = NPCID.Bee;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.buffImmune[BuffID.ShadowFlame] = true;
            NPC.buffImmune[BuffID.Venom] = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = true;
            NPC.buffImmune[ModContent.BuffType<Plague>()] = true;
        }

        public override void AI()
        {
            bool revenge = CalamityWorld.revenge;
            Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.015f, 0.1f, 0f);
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(true);
            }
            float num = revenge ? 8f : 7f;
            float num2 = revenge ? 0.2f : 0.15f;
            if (BossRushEvent.BossRushActive)
            {
                num *= 1.5f;
                num2 *= 1.5f;
            }

            NPC.localAI[0] += 1f;
            float num3 = (NPC.localAI[0] - 60f) / 60f;
            if (num3 > 1f)
            {
                num3 = 1f;
            }
            else
            {
                if (NPC.velocity.X > 6f)
                {
                    NPC.velocity.X = 6f;
                }
                if (NPC.velocity.X < -6f)
                {
                    NPC.velocity.X = -6f;
                }
                if (NPC.velocity.Y > 6f)
                {
                    NPC.velocity.Y = 6f;
                }
                if (NPC.velocity.Y < -6f)
                {
                    NPC.velocity.Y = -6f;
                }
            }
            num2 *= num3;
            Vector2 vector = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
            float num4 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2);
            float num5 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2);
            num4 = (float)((int)(num4 / 8f) * 8);
            num5 = (float)((int)(num5 / 8f) * 8);
            vector.X = (float)((int)(vector.X / 8f) * 8);
            vector.Y = (float)((int)(vector.Y / 8f) * 8);
            num4 -= vector.X;
            num5 -= vector.Y;
            float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
            if (num6 == 0f)
            {
                num4 = NPC.velocity.X;
                num5 = NPC.velocity.Y;
            }
            else
            {
                num6 = num / num6;
                num4 *= num6;
                num5 *= num6;
            }
            NPC.ai[0] += 1f;
            if (NPC.ai[0] > 0f)
            {
                NPC.velocity.Y = NPC.velocity.Y + 0.023f;
            }
            else
            {
                NPC.velocity.Y = NPC.velocity.Y - 0.023f;
            }
            if (NPC.ai[0] < -100f || NPC.ai[0] > 100f)
            {
                NPC.velocity.X = NPC.velocity.X + 0.023f;
            }
            else
            {
                NPC.velocity.X = NPC.velocity.X - 0.023f;
            }
            if (NPC.ai[0] > 200f)
            {
                NPC.ai[0] = -200f;
            }
            if (Main.player[NPC.target].dead)
            {
                num4 = (float)NPC.direction * num / 2f;
                num5 = -num / 2f;
            }
            if (NPC.velocity.X < num4)
            {
                NPC.velocity.X = NPC.velocity.X + num2;
                if (NPC.velocity.X < 0f && num4 > 0f)
                {
                    NPC.velocity.X = NPC.velocity.X + num2;
                }
            }
            else if (NPC.velocity.X > num4)
            {
                NPC.velocity.X = NPC.velocity.X - num2;
                if (NPC.velocity.X > 0f && num4 < 0f)
                {
                    NPC.velocity.X = NPC.velocity.X - num2;
                }
            }
            if (NPC.velocity.Y < num5)
            {
                NPC.velocity.Y = NPC.velocity.Y + num2;
                if (NPC.velocity.Y < 0f && num5 > 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y + num2;
                }
            }
            else if (NPC.velocity.Y > num5)
            {
                NPC.velocity.Y = NPC.velocity.Y - num2;
                if (NPC.velocity.Y > 0f && num5 < 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y - num2;
                }
            }
            NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) - 1.57f;
            float num12 = 0.7f;
            if (NPC.collideX)
            {
                NPC.netUpdate = true;
                NPC.velocity.X = NPC.oldVelocity.X * -num12;
                if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
                {
                    NPC.velocity.X = 2f;
                }
                if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
                {
                    NPC.velocity.X = -2f;
                }
            }
            if (NPC.collideY)
            {
                NPC.netUpdate = true;
                NPC.velocity.Y = NPC.oldVelocity.Y * -num12;
                if (NPC.velocity.Y > 0f && (double)NPC.velocity.Y < 1.5)
                {
                    NPC.velocity.Y = 2f;
                }
                if (NPC.velocity.Y < 0f && (double)NPC.velocity.Y > -1.5)
                {
                    NPC.velocity.Y = -2f;
                }
            }
            if (((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f) || (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f) || (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f) || (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f)) && !NPC.justHit)
            {
                NPC.netUpdate = true;
            }
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
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

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/NormalNPCs/PlagueBeeGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Red, 0.5f);

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
				}
			}

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override bool PreKill()
        {
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 120, true);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Plague, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Plague, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
