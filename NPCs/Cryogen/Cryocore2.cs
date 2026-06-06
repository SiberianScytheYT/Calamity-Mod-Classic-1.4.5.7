using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Cryogen
{
    public class Cryocore2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cryocore");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.width = 66;
            NPC.height = 66;
            NPC.defense = 10;
            NPC.lifeMax = 300;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 40000;
            }
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = BossRushEvent.BossRushActive ? 0f : 0.5f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath15;
			NPC.coldDamage = true;
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
            Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.02f, 0.5f, 0.5f);
            NPC.TargetClosest(true);
            bool revenge = CalamityWorld.revenge;
            float speed = revenge ? 14f : 12f;
            if (BossRushEvent.BossRushActive)
                speed = 28f;
            Vector2 vector167 = new Vector2(NPC.Center.X + (float)(NPC.direction * 20), NPC.Center.Y + 6f);
            float num1373 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector167.X;
            float num1374 = Main.player[NPC.target].Center.Y - vector167.Y;
            float num1375 = (float)Math.Sqrt((double)(num1373 * num1373 + num1374 * num1374));
            float num1376 = speed / num1375;
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
                NPC.rotation += (float)NPC.direction * 0.3f;
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
            NPC.rotation = NPC.velocity.X * 0.15f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Frostburn, 90, true);
            target.AddBuff(BuffID.Chilled, 60, true);
        }

        public override bool PreKill()
        {
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 67, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int k = 0; k < 20; k++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, 67, hit.HitDirection, -1f, 0, default, 1f);
                    }
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Cryocore2").Type, 1f);
                }
            }
        }
    }
}
