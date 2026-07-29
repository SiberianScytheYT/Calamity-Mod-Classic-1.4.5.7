using CalRD.Buffs.StatDebuffs;
using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.HiveMind
{
	[AutoloadBossHead]
    public class HiveMind : ModNPC
    {
        int burrowTimer = 420;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Hive Mind");
            Main.npcFrameCount[NPC.type] = 16;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
			NPC.GetNPCDamage();
			NPC.width = 150;
            NPC.height = 120;
            NPC.defense = 10;
            NPC.LifeMaxNERB(1200, 1800, 350000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.buffImmune[ModContent.BuffType<GlacialState>()] = true;
            NPC.buffImmune[ModContent.BuffType<TemporalSadness>()] = true;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.value = 0f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/HiveMind");
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.dontTakeDamage);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.dontTakeDamage = reader.ReadBoolean();
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1f / 6f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
			if (!player.active || player.dead)
			{
				NPC.TargetClosest(false);
				player = Main.player[NPC.target];
				if (!player.active || player.dead)
				{
					if (NPC.timeLeft > 60)
						NPC.timeLeft = 60;
					if (NPC.localAI[3] < 120f)
					{
						float[] aiArray = NPC.localAI;
						int number = 3;
						float num244 = aiArray[number];
						aiArray[number] = num244 + 1f;
					}
					if (NPC.localAI[3] > 60f)
					{
						NPC.velocity.Y += (NPC.localAI[3] - 60f) * 0.5f;
						NPC.noGravity = true;
						NPC.noTileCollide = true;
						if (burrowTimer > 30)
							burrowTimer = 30;
					}
					return;
				}
			}
			else if (NPC.timeLeft < 1800)
				NPC.timeLeft = 1800;

            if (NPC.localAI[3] > 0f)
            {
                float[] aiArray = NPC.localAI;
                int number = 3;
                float num244 = aiArray[number];
                aiArray[number] = num244 - 1f;
                return;
            }

            NPC.noGravity = false;
            NPC.noTileCollide = false;

            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
            bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
			CalamityGlobalNPC.hiveMind = NPC.whoAmI;

			float enrageScale = 0f;
			if ((NPC.position.Y / 16f) < Main.worldSurface)
				enrageScale += 1f;
			if (!player.ZoneCorrupt)
				enrageScale += 1f;

			if (BossRushEvent.BossRushActive)
				enrageScale = 0f;

			if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.localAI[0] == 0f)
                {
                    NPC.localAI[0] = 1f;
					int maxBlobs = death ? 15 : revenge ? 7 : expertMode ? 6 : 5;
                    for (int i = 0; i < maxBlobs; i++)
                    {
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<HiveBlob>(), NPC.whoAmI);
                    }
                }
            }

            bool flag100 = false;
            int num568 = 0;
            if (expertMode)
            {
                for (int num569 = 0; num569 < Main.maxNPCs; num569++)
                {
                    if (Main.npc[num569].active && Main.npc[num569].type == ModContent.NPCType<DankCreeper>())
                    {
                        flag100 = true;
                        num568++;
                    }
                }

                NPC.defense += num568 * 25;

				if (!flag100)
					NPC.defense = NPC.defDefense;
			}

            if (NPC.ai[3] == 0f && NPC.life > 0)
            {
                NPC.ai[3] = NPC.lifeMax;
            }
            if (NPC.life > 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int num660 = (int)(NPC.lifeMax * 0.25);
                    if ((NPC.life + num660) < NPC.ai[3])
                    {
                        NPC.ai[3] = NPC.life;
						int maxSpawns = death ? 5 : revenge ? 4 : expertMode ? Main.rand.Next(3, 5) : Main.rand.Next(2, 4);
						int maxDankSpawns = death ? Main.rand.Next(2, 4) : revenge ? 2 : expertMode ? Main.rand.Next(1, 3) : 1;
						for (int num662 = 0; num662 < maxSpawns; num662++)
                        {
                            int x = (int)(NPC.position.X + Main.rand.Next(NPC.width - 32));
                            int y = (int)(NPC.position.Y + Main.rand.Next(NPC.height - 32));
                            int type = ModContent.NPCType<HiveBlob>();
                            if (NPC.CountNPCS(ModContent.NPCType<DankCreeper>()) < maxDankSpawns || NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
                            {
                                type = ModContent.NPCType<DankCreeper>();
                            }
                            int num664 = NPC.NewNPC(NPC.GetSource_FromThis(), x, y, type);
                            Main.npc[num664].SetDefaults(type);
                            if (Main.netMode == NetmodeID.Server && num664 < Main.maxNPCs)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                            }
                        }
                        return;
                    }
                }
            }

            burrowTimer--;
            if (burrowTimer < -120)
            {
                burrowTimer = (death ? 180 : revenge ? 300 : expertMode ? 360 : 420) - (int)enrageScale * 60;
                NPC.scale = 1f;
                NPC.alpha = 0;
                NPC.dontTakeDamage = false;
                NPC.damage = NPC.defDamage;
            }
            else if (burrowTimer < -60)
            {
                NPC.scale += 0.0165f;
                NPC.alpha -= 4;
                int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.Center.Y), NPC.width, NPC.height / 2, 14, 0f, -3f, 100, default, 2.5f * NPC.scale);
                Main.dust[num622].velocity *= 2f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
                for (int i = 0; i < 2; i++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.Center.Y), NPC.width, NPC.height / 2, 14, 0f, -3f, 100, default, 3.5f * NPC.scale);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 3.5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.Center.Y), NPC.width, NPC.height / 2, 14, 0f, -3f, 100, default, 2.5f * NPC.scale);
                    Main.dust[num624].velocity *= 1f;
                }
            }
            else if (burrowTimer == -60)
            {
                NPC.scale = 0.01f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.Center = player.Center;
                    NPC.position.Y = player.position.Y - NPC.height;
                    int tilePosX = (int)NPC.Center.X / 16;
                    int tilePosY = (int)(NPC.position.Y + NPC.height) / 16 + 1;
                    while (!(Main.tile[tilePosX, tilePosY].HasUnactuatedTile && Main.tileSolid[Main.tile[tilePosX, tilePosY].TileType]))
                    {
                        tilePosY++;
                        NPC.position.Y += 16;
                    }
                }
                NPC.netUpdate = true;
            }
            else if (burrowTimer < 0)
            {
                NPC.scale -= 0.0165f;
                NPC.alpha += 4;
                int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.Center.Y), NPC.width, NPC.height / 2, 14, 0f, -3f, 100, default, 2.5f * NPC.scale);
                Main.dust[num622].velocity *= 2f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
                for (int i = 0; i < 2; i++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.Center.Y), NPC.width, NPC.height / 2, 14, 0f, -3f, 100, default, 3.5f * NPC.scale);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 3.5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.Center.Y), NPC.width, NPC.height / 2, 14, 0f, -3f, 100, default, 2.5f * NPC.scale);
                    Main.dust[num624].velocity *= 1f;
                }
            }
            else if (burrowTimer == 0)
            {
                if (!player.active || player.dead)
                {
                    burrowTimer = 30;
                }
                else
                {
                    NPC.dontTakeDamage = true;
                    NPC.damage = 0;
                }
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
            {
                if (NPC.CountNPCS(NPCID.EaterofSouls) < 3 && NPC.CountNPCS(NPCID.DevourerHead) < 1)
                {
                    if (Main.rand.NextBool(60) && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 spawnAt = NPC.Center + new Vector2(0f, NPC.height / 2f);
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)spawnAt.X, (int)spawnAt.Y, NPCID.EaterofSouls);
                    }
                    if (Main.rand.NextBool(150) && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 spawnAt = NPC.Center + new Vector2(0f, NPC.height / 2f);
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)spawnAt.X, (int)spawnAt.Y, NPCID.DevourerHead);
                    }
                }
                int num285 = 0;
                while (num285 < hit.Damage / NPC.lifeMax * 100.0)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 14, hit.HitDirection, -1f, 0, default, 1f);
                    num285++;
                }
            }
            else
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    int goreAmount = 7;
                    for (int i = 1; i <= goreAmount; i++)
                        Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HiveMindGore" + i).Type, 1f);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (NPC.CountNPCS(ModContent.NPCType<HiveMindP2>()) < 1)
                    {
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<HiveMindP2>(), NPC.whoAmI, 0f, 0f, 0f, 0f, NPC.target);
                        SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                    }
                }
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.scale == 1f; //no damage when shrunk
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return NPC.scale == 1f;
        }

        public override bool PreKill()
        {
            return false;
        }
    }
}
