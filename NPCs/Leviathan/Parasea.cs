using CalRD.Events;
using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.Leviathan
{
	public class Parasea : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Parasea");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
			NPC.GetNPCDamage();
			NPC.width = NPC.height = 30;
            NPC.defense = 8;
            NPC.lifeMax = 650;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 50000;
            }
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<ParaseaBanner>();
        }

        public override void AI()
        {
            bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            float speed = revenge ? 16f : 13f;
            if (BossRushEvent.BossRushActive)
                speed = 24f;
            CalamityAI.DungeonSpiritAI(NPC, Mod, speed, MathHelper.Pi);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe || spawnInfo.Player.Calamity().ZoneSulphur || (!NPC.downedPlantBoss && !CalamityWorld.downedCalamitas))
			{
				return 0f;
			}
			return SpawnCondition.OceanMonster.Chance * 0.06f;
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
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            int height = texture.Height / Main.npcFrameCount[NPC.type];
            int width = texture.Width;
			SpriteEffects spriteEffects = SpriteEffects.FlipHorizontally;
			if (NPC.spriteDirection == -1)
				spriteEffects = SpriteEffects.None;
            Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2((float)width / 2f, (float)height / 2f), NPC.scale, spriteEffects, 0f);
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Wet, 60, true);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
