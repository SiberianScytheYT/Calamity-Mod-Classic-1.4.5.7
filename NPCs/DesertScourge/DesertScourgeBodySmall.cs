using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.DesertScourge
{
	public class DesertScourgeBodySmall : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Desert Scourge");
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 5f;
            NPC.width = 40;
            NPC.height = 40;
            NPC.defense = 4;
            NPC.lifeMax = 800;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 350000;
            }
            NPC.aiStyle = 6;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.alpha = 255;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.dontCountMe = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void AI()
        {
            if (!Main.npc[(int)NPC.ai[1]].active)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;
            }
            if (Main.npc[(int)NPC.ai[1]].alpha < 128)
            {
                NPC.alpha -= 42;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreKill()
        {
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    float randomSpread = (float)(Main.rand.Next(-100, 100) / 100);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread * Main.rand.NextFloat(), Mod.Find<ModGore>("ScourgeBody").Type, 0.65f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread * Main.rand.NextFloat(), Mod.Find<ModGore>("ScourgeBody2").Type, 0.65f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread * Main.rand.NextFloat(), Mod.Find<ModGore>("ScourgeBody3").Type, 0.65f);
                }
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.7f * balance);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 60, true);
        }
    }
}
