using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.PlaguebringerGoliath
{
    public class PlagueMine : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plague Mine");
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 1f;
            NPC.width = 42;
            NPC.height = 42;
            NPC.defense = 10;
            NPC.lifeMax = BossRushEvent.BossRushActive ? 10000 : 100;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.dontTakeDamage);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.dontTakeDamage = reader.ReadBoolean();
        }

        public override void AI()
        {
			Lighting.AddLight(NPC.Center, 0.03f, 0.2f, 0f);
			Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            else if (NPC.timeLeft > 600)
            {
                NPC.timeLeft = 600;
            }
            Vector2 vector = Main.player[NPC.target].Center - NPC.Center;
            if (vector.Length() < 90f || NPC.ai[3] >= 900f)
            {
                NPC.dontTakeDamage = false;
                CheckDead();
                NPC.life = 0;
                return;
            }
            NPC.ai[3] += 1f;
            NPC.dontTakeDamage = NPC.ai[3] >= 180f ? false : true;
            if (NPC.ai[3] >= 480f)
            {
                NPC.velocity *= 0.985f;
                return;
            }
            NPC.TargetClosest(true);
            float num1372 = BossRushEvent.BossRushActive ? 10f : 7f;
            Vector2 vector167 = new Vector2(NPC.Center.X + (float)(NPC.direction * 20), NPC.Center.Y + 6f);
            float num1373 = player.position.X + (float)player.width * 0.5f - vector167.X;
            float num1374 = player.Center.Y - vector167.Y;
            float num1375 = (float)Math.Sqrt((double)(num1373 * num1373 + num1374 * num1374));
            float num1376 = num1372 / num1375;
            num1373 *= num1376;
            num1374 *= num1376;
            NPC.ai[0] -= 1f;
            if (num1375 < 200f || NPC.ai[0] > 0f)
            {
                if (num1375 < 200f)
                {
                    NPC.ai[0] = 20f;
                }
                if (NPC.velocity.X < 0f)
                {
                    NPC.direction = -1;
                }
                else
                {
                    NPC.direction = 1;
                }
                return;
            }
            NPC.velocity.X = (NPC.velocity.X * 50f + num1373) / 51f;
            NPC.velocity.Y = (NPC.velocity.Y * 50f + num1374) / 51f;
            if (num1375 < 350f)
            {
                NPC.velocity.X = (NPC.velocity.X * 10f + num1373) / 11f;
                NPC.velocity.Y = (NPC.velocity.Y * 10f + num1374) / 11f;
            }
            if (num1375 < 300f)
            {
                NPC.velocity.X = (NPC.velocity.X * 7f + num1373) / 8f;
                NPC.velocity.Y = (NPC.velocity.Y * 7f + num1374) / 8f;
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

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/PlaguebringerGoliath/PlagueMineGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Red, 0.5f);

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override bool CheckDead()
        {
            SoundEngine.PlaySound(SoundID.Item14, NPC.position);
            NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
            NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
            NPC.width = NPC.height = 216;
            NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
            NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
            for (int num621 = 0; num621 < 15; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 89, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
                Main.dust[num622].noGravity = true;
            }
            for (int num623 = 0; num623 < 30; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 89, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 89, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
                Main.dust[num624].noGravity = true;
            }
            return true;
        }
    }
}
