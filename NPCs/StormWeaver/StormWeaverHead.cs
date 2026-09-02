using CalRD.Dusts;
using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.StormWeaver
{
    [AutoloadBossHead]
    public class StormWeaverHead : ModNPC
    {
        private const float BoltAngleSpread = 170;
        private int BoltCountdown = 0;
        private const float speed = 10f;
        private const float turnSpeed = 0.3f;
        private bool tail = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Storm Weaver");
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.85f,
                PortraitScale = 0.75f,
                CustomTexturePath = "CalRD/ExtraTextures/Bestiary/StormWeaver_Bestiary",
                PortraitPositionXOverride = 40,
                PortraitPositionYOverride = 40
            };
            value.Position.X += 70;
            value.Position.Y += 55;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("One of the Devourer's sentinels, still young and inexperienced.. Should the day arrive where its power rivals the Devourer's, he will surely make quick work of it.")
            });
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 5f;
            NPC.width = 74;
            NPC.height = 74;
            NPC.defense = 0;
            CalamityGlobalNPC global = NPC.Calamity();
            global.DR = 0.999999f;
            global.unbreakableDR = true;
			bool notDoGFight = CalamityWorld.DoGSecondStageCountdown <= 0 || !CalamityWorld.downedSentinel2;
			NPC.LifeMaxNERB(notDoGFight ? 100000 : 20000, notDoGFight ? 100000 : 20000, 170000);
			Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/ScourgeofTheUniverse");
            if (notDoGFight)
                Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/Weaver");
            
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.alpha = 255;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.chaseable = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.netAlways = true;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }

			if (CalamityWorld.death || BossRushEvent.BossRushActive)
				NPC.scale = 1.2f;
			else if (CalamityWorld.revenge)
				NPC.scale = 1.15f;
			else if (Main.expertMode)
				NPC.scale = 1.1f;
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(BoltCountdown);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            BoltCountdown = reader.ReadInt32();
        }

        public override void AI()
        {
			bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
			if (NPC.defense < 99999 && (CalamityWorld.DoGSecondStageCountdown <= 0 || !CalamityWorld.downedSentinel2))
            {
                NPC.defense = 99999;
            }
            else
            {
                NPC.defense = 0;
            }
            if (!Main.raining && !BossRushEvent.BossRushActive && CalamityWorld.DoGSecondStageCountdown <= 0)
            {
				CalamityUtils.StartRain();
            }
            Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2)) / 16f), (int)((NPC.position.Y + (NPC.height / 2)) / 16f), 0.2f, 0.05f, 0.2f);
            if (NPC.ai[3] > 0f)
            {
                NPC.realLife = (int)NPC.ai[3];
            }
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(true);
            }
            NPC.velocity.Length();
            if (NPC.alpha != 0)
            {
                for (int num934 = 0; num934 < 2; num934++)
                {
                    int num935 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 182, 0f, 0f, 100, default, 2f);
                    Main.dust[num935].noGravity = true;
                    Main.dust[num935].noLight = true;
                }
            }
            NPC.alpha -= 12;
            if (NPC.alpha < 0)
            {
                NPC.alpha = 0;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!tail && NPC.ai[0] == 0f)
                {
                    int Previous = NPC.whoAmI;
					int totalLength = death ? 60 : revenge ? 50 : expertMode ? 40 : 30;
					for (int num36 = 0; num36 < totalLength; num36++)
                    {
                        int lol;
                        if (num36 >= 0 && num36 < totalLength - 1)
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<StormWeaverBody>(), NPC.whoAmI);
                        }
                        else
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<StormWeaverTail>(), NPC.whoAmI);
                        }
                        Main.npc[lol].realLife = NPC.whoAmI;
                        Main.npc[lol].ai[2] = NPC.whoAmI;
                        Main.npc[lol].ai[1] = Previous;
                        Main.npc[Previous].ai[0] = lol;
                        NPC.netUpdate = true;
                        Previous = lol;
                    }
                    tail = true;
                }

				if (expertMode)
				{
					NPC.localAI[0] += 1f;
					if (NPC.localAI[0] >= 360f)
					{
						NPC.localAI[0] = 0f;
						NPC.TargetClosest(true);
						NPC.netUpdate = true;
						float xPos = Main.rand.NextBool(2) ? Main.player[NPC.target].position.X + 500f : Main.player[NPC.target].position.X - 500f;
						Vector2 spawnPos = new Vector2(xPos, Main.player[NPC.target].position.Y + Main.rand.Next(-500, 501));
						int type = ProjectileID.CultistBossLightningOrb;
						int damage = NPC.GetProjectileDamage(type);
						Projectile.NewProjectile(Entity.GetSource_FromThis(), spawnPos, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);
					}
				}

                if (BoltCountdown == 0)
                {
                    BoltCountdown = 600;
                }
                if (BoltCountdown > 0)
                {
                    BoltCountdown--;
                    if (BoltCountdown == 0)
                    {
                        int speed2 = revenge ? 8 : 7;
                        if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
                        {
                            speed2 += 1;
                        }
                        float spawnX2 = Main.rand.Next(2001) - 1000f + Main.player[NPC.target].Center.X;
                        float spawnY2 = -1000f + Main.player[NPC.target].Center.Y;
                        Vector2 baseSpawn = new Vector2(spawnX2, spawnY2);
                        Vector2 baseVelocity = Main.player[NPC.target].Center - baseSpawn;
                        baseVelocity.Normalize();
                        baseVelocity *= speed2;
						int BoltProjectiles = 1;
						for (int i = 0; i < BoltProjectiles; i++)
                        {
                            Vector2 source = baseSpawn;
                            source.X += i * 30 - (BoltProjectiles * 15);
                            Vector2 velocity = baseVelocity.RotatedBy(MathHelper.ToRadians(-BoltAngleSpread / 2 + (BoltAngleSpread * i / BoltProjectiles)));
                            velocity.X = velocity.X + 3 * Main.rand.NextFloat() - 1.5f;
                            Vector2 vector94 = Main.player[NPC.target].Center - source;
                            float ai = Main.rand.Next(100);
							int type = ProjectileID.CultistBossLightningOrbArc;
							int damage = NPC.GetProjectileDamage(type);
							Projectile.NewProjectile(Entity.GetSource_FromThis(), source, velocity, type, damage, 0f, Main.myPlayer, vector94.ToRotation(), ai);
                        }
                    }
                }
            }
            if (Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(false);
                NPC.velocity.Y = NPC.velocity.Y - 10f;
                if ((double)NPC.position.Y < Main.topWorld + 16f)
                {
                    NPC.velocity.Y = NPC.velocity.Y - 10f;
                }
                if ((double)NPC.position.Y < Main.topWorld + 16f)
                {
                    CalamityWorld.DoGSecondStageCountdown = 0;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var netMessage = Mod.GetPacket();
                        netMessage.Write((byte)CalRDMessageType.DoGCountdownSync);
                        netMessage.Write(CalamityWorld.DoGSecondStageCountdown);
                        netMessage.Send();
                    }
                    for (int num957 = 0; num957 < Main.maxNPCs; num957++)
                    {
                        if (Main.npc[num957].active && (Main.npc[num957].type == ModContent.NPCType<StormWeaverBody>()
                            || Main.npc[num957].type == ModContent.NPCType<StormWeaverHead>()
                            || Main.npc[num957].type == ModContent.NPCType<StormWeaverTail>()))
                        {
                            Main.npc[num957].active = false;
                        }
                    }
                }
            }
            if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > 10000f)
            {
                CalamityWorld.DoGSecondStageCountdown = 0;
                if (Main.netMode == NetmodeID.Server)
                {
                    var netMessage = Mod.GetPacket();
                    netMessage.Write((byte)CalRDMessageType.DoGCountdownSync);
                    netMessage.Write(CalamityWorld.DoGSecondStageCountdown);
                    netMessage.Send();
                }
                for (int num957 = 0; num957 < Main.maxNPCs; num957++)
                {
                    if (Main.npc[num957].active && (Main.npc[num957].type == ModContent.NPCType<StormWeaverBody>()
                            || Main.npc[num957].type == ModContent.NPCType<StormWeaverHead>()
                            || Main.npc[num957].type == ModContent.NPCType<StormWeaverTail>()))
                    {
                        Main.npc[num957].active = false;
                    }
                }
            }
            if (NPC.velocity.X < 0f)
            {
                NPC.spriteDirection = -1;
            }
            else if (NPC.velocity.X > 0f)
            {
                NPC.spriteDirection = 1;
            }
            if (Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(false);
            }
            float num188 = speed;
            float num189 = turnSpeed;
            Vector2 vector18 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
            float num191 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2);
            float num192 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2);
            int num42 = -1;
            int num43 = (int)(Main.player[NPC.target].Center.X / 16f);
            int num44 = (int)(Main.player[NPC.target].Center.Y / 16f);
            for (int num45 = num43 - 2; num45 <= num43 + 2; num45++)
            {
                for (int num46 = num44; num46 <= num44 + 15; num46++)
                {
                    if (WorldGen.SolidTile2(num45, num46))
                    {
                        num42 = num46;
                        break;
                    }
                }
                if (num42 > 0)
                {
                    break;
                }
            }
            if (num42 > 0)
            {
                num42 *= 16;
                float num47 = (float)(num42 - 800);
                if (Main.player[NPC.target].position.Y > num47)
                {
                    num192 = num47;
                    if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) < 500f)
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            num191 = Main.player[NPC.target].Center.X + 600f;
                        }
                        else
                        {
                            num191 = Main.player[NPC.target].Center.X - 600f;
                        }
                    }
                }
            }
            else
            {
                num188 = revenge ? 11f : 10f;
                num189 = revenge ? 0.31f : 0.28f;
            }
            float num48 = num188 * 1.3f;
            float num49 = num188 * 0.7f;
            float num50 = NPC.velocity.Length();
            if (num50 > 0f)
            {
                if (num50 > num48)
                {
                    NPC.velocity.Normalize();
                    NPC.velocity *= num48;
                }
                else if (num50 < num49)
                {
                    NPC.velocity.Normalize();
                    NPC.velocity *= num49;
                }
            }
            if (num42 > 0)
            {
                for (int num51 = 0; num51 < Main.maxNPCs; num51++)
                {
                    if (Main.npc[num51].active && Main.npc[num51].type == NPC.type && num51 != NPC.whoAmI)
                    {
                        Vector2 vector3 = Main.npc[num51].Center - NPC.Center;
                        if (vector3.Length() < 400f)
                        {
                            vector3.Normalize();
                            vector3 *= 1000f;
                            num191 -= vector3.X;
                            num192 -= vector3.Y;
                        }
                    }
                }
            }
            else
            {
                for (int num52 = 0; num52 < Main.maxNPCs; num52++)
                {
                    if (Main.npc[num52].active && Main.npc[num52].type == NPC.type && num52 != NPC.whoAmI)
                    {
                        Vector2 vector4 = Main.npc[num52].Center - NPC.Center;
                        if (vector4.Length() < 60f)
                        {
                            vector4.Normalize();
                            vector4 *= 200f;
                            num191 -= vector4.X;
                            num192 -= vector4.Y;
                        }
                    }
                }
            }
            num191 = (float)((int)(num191 / 16f) * 16);
            num192 = (float)((int)(num192 / 16f) * 16);
            vector18.X = (float)((int)(vector18.X / 16f) * 16);
            vector18.Y = (float)((int)(vector18.Y / 16f) * 16);
            num191 -= vector18.X;
            num192 -= vector18.Y;
            float num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
            float num196 = System.Math.Abs(num191);
            float num197 = System.Math.Abs(num192);
            float num198 = num188 / num193;
            num191 *= num198;
            num192 *= num198;
            if ((NPC.velocity.X > 0f && num191 > 0f) || (NPC.velocity.X < 0f && num191 < 0f) || (NPC.velocity.Y > 0f && num192 > 0f) || (NPC.velocity.Y < 0f && num192 < 0f))
            {
                if (NPC.velocity.X < num191)
                {
                    NPC.velocity.X = NPC.velocity.X + num189;
                }
                else
                {
                    if (NPC.velocity.X > num191)
                    {
                        NPC.velocity.X = NPC.velocity.X - num189;
                    }
                }
                if (NPC.velocity.Y < num192)
                {
                    NPC.velocity.Y = NPC.velocity.Y + num189;
                }
                else
                {
                    if (NPC.velocity.Y > num192)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - num189;
                    }
                }
                if ((double)System.Math.Abs(num192) < (double)num188 * 0.2 && ((NPC.velocity.X > 0f && num191 < 0f) || (NPC.velocity.X < 0f && num191 > 0f)))
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y = NPC.velocity.Y + num189 * 2f;
                    }
                    else
                    {
                        NPC.velocity.Y = NPC.velocity.Y - num189 * 2f;
                    }
                }
                if ((double)System.Math.Abs(num191) < (double)num188 * 0.2 && ((NPC.velocity.Y > 0f && num192 < 0f) || (NPC.velocity.Y < 0f && num192 > 0f)))
                {
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X + num189 * 2f; //changed from 2
                    }
                    else
                    {
                        NPC.velocity.X = NPC.velocity.X - num189 * 2f; //changed from 2
                    }
                }
            }
            else
            {
                if (num196 > num197)
                {
                    if (NPC.velocity.X < num191)
                    {
                        NPC.velocity.X = NPC.velocity.X + num189 * 1.1f; //changed from 1.1
                    }
                    else if (NPC.velocity.X > num191)
                    {
                        NPC.velocity.X = NPC.velocity.X - num189 * 1.1f; //changed from 1.1
                    }
                    if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.5)
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + num189;
                        }
                        else
                        {
                            NPC.velocity.Y = NPC.velocity.Y - num189;
                        }
                    }
                }
                else
                {
                    if (NPC.velocity.Y < num192)
                    {
                        NPC.velocity.Y = NPC.velocity.Y + num189 * 1.1f;
                    }
                    else if (NPC.velocity.Y > num192)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - num189 * 1.1f;
                    }
                    if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.5)
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X + num189;
                        }
                        else
                        {
                            NPC.velocity.X = NPC.velocity.X - num189;
                        }
                    }
                }
            }
            NPC.rotation = (float)System.Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + MathHelper.PiOver2;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SWArmorHead1").Type, 1f);
                NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
                NPC.width = 30;
                NPC.height = 30;
                NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
                for (int num621 = 0; num621 < 20; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 40; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override bool CheckDead()
        {
            for (int num957 = 0; num957 < Main.maxNPCs; num957++)
            {
                if (Main.npc[num957].active && (Main.npc[num957].type == ModContent.NPCType<StormWeaverBody>() || Main.npc[num957].type == ModContent.NPCType<StormWeaverTail>()))
                {
                    Main.npc[num957].life = 0;
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.SpawnOnPlayer(Main.LocalPlayer.whoAmI, ModContent.NPCType<StormWeaverHeadNaked>());
            }
            return true;
        }

        public override bool PreKill()
        {
            return false;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
        }
    }
}
