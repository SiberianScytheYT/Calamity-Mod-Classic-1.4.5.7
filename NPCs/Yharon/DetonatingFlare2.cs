using CalRD.Buffs.StatDebuffs;
using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.Yharon
{
	public class DetonatingFlare2 : ModNPC
    {
        float randomSpeed = 0f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Detonating Flame");
            Main.npcFrameCount[NPC.type] = 5;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
		}

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
			NPC.GetNPCDamage();
			NPC.width = 50;
            NPC.height = 50;
            NPC.defense = 75;
            NPC.lifeMax = 13000;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
			NPC.HitSound = SoundID.NPCHit52;
            NPC.DeathSound = SoundID.NPCDeath55;
            NPC.alpha = 255;
        }

        public override void AI()
        {
            NPC.alpha -= 3;
            bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            if (NPC.localAI[3] == 0f)
            {
                switch (Main.rand.Next(6))
                {
                    case 0:
                        randomSpeed = 10f;
                        break;
                    case 1:
                        randomSpeed = 11.5f;
                        break;
                    case 2:
                        randomSpeed = 13f;
                        break;
                    case 3:
                        randomSpeed = 14.5f;
                        break;
                    case 4:
                        randomSpeed = 16f;
                        break;
                    case 5:
                        randomSpeed = 17.5f;
                        break;
                }
                NPC.localAI[3] = 1f;
            }
            float speed = randomSpeed + (revenge ? 1f : 0f);
            CalamityAI.DungeonSpiritAI(NPC, Mod, speed, -MathHelper.PiOver2);
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return new Color(255, Main.DiscoG, 53, 0);
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			Color color36 = new Color(255, Main.DiscoG, 53, 0);
			float amount9 = 0.5f;
			int num153 = 10;

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
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, SpriteEffects.None, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, SpriteEffects.None, 0f);

			return false;
		}

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override bool PreKill()
        {
            return false;
        }
    }
}
