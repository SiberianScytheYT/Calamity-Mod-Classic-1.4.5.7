using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Crabulon
{
    public class CrabShroom : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crab Shroom");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
			NPC.GetNPCDamage();
			NPC.width = 14;
            NPC.height = 14;
            NPC.lifeMax = 25;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 150000;
            }
            AIType = -1;
            NPC.knockBackResist = 0.75f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2)) / 16f), (int)((NPC.position.Y + (NPC.height / 2)) / 16f), 0f, 0.2f, 0.4f);
            bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            float speed = revenge ? 1.25f : 1f;
            Player player = Main.player[NPC.target];
            NPC.velocity.Y += 0.02f;
            if (NPC.velocity.Y > speed)
            {
                NPC.velocity.Y = speed;
            }
            NPC.TargetClosest(true);
            if (NPC.position.X + NPC.width < player.position.X)
            {
                if (NPC.velocity.X < 0f)
                {
                    NPC.velocity.X *= 0.98f;
                }
                NPC.velocity.X += BossRushEvent.BossRushActive ? 0.2f : 0.1f;
            }
            else if (NPC.position.X > player.position.X + player.width)
            {
                if (NPC.velocity.X > 0f)
                {
                    NPC.velocity.X *= 0.98f;
                }
                NPC.velocity.X -= BossRushEvent.BossRushActive ? 0.2f : 0.1f;
            }
            if (NPC.velocity.X > (BossRushEvent.BossRushActive ? 15f : 5f) || NPC.velocity.X < (BossRushEvent.BossRushActive ? -15f : -5f))
            {
                NPC.velocity.X *= 0.97f;
            }
            NPC.rotation = NPC.velocity.X * 0.1f;
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / 2);
			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/Crabulon/CrabShroomGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Cyan, 0.5f);

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 56, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 56, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
