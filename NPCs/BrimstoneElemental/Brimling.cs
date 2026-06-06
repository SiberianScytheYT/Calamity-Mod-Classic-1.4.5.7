using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Events;
using CalRD.Projectiles.Boss;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.BrimstoneElemental
{
    public class Brimling : ModNPC
    {
        private bool boostDR = false;
        public static float normalDR = 0.15f;
        public static float boostedDR = 0.8f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimling");
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.damage = 0;
            NPC.width = 60;
            NPC.height = 60;
            NPC.defense = 0;
			NPC.DR_NERD(normalDR);
            NPC.lifeMax = 2000;
            NPC.knockBackResist = 0f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[ModContent.BuffType<MarkedforDeath>()] = false;
			NPC.buffImmune[BuffID.Frostburn] = false;
			NPC.buffImmune[BuffID.CursedInferno] = false;
            NPC.buffImmune[BuffID.Daybreak] = false;
			NPC.buffImmune[BuffID.BetsysCurse] = false;
			NPC.buffImmune[BuffID.StardustMinionBleed] = false;
			NPC.buffImmune[BuffID.DryadsWardDebuff] = false;
			NPC.buffImmune[BuffID.Oiled] = false;
			NPC.buffImmune[BuffID.BoneJavelin] = false;
			NPC.buffImmune[BuffID.SoulDrain] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
			NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<HolyFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Plague>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WarCleave>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.buffImmune[ModContent.BuffType<SulphuricPoisoning>()] = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit23;
            NPC.DeathSound = SoundID.NPCDeath39;
            if (CalamityWorld.downedProvidence)
            {
                NPC.lifeMax = 40000;
            }
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 100000;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(boostDR);
            writer.Write(NPC.chaseable);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            boostDR = reader.ReadBoolean();
            NPC.chaseable = reader.ReadBoolean();
        }

        public override void AI()
        {
            Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 1f, 0f, 0f);
            if (CalamityGlobalNPC.brimstoneElemental < 0 || !Main.npc[CalamityGlobalNPC.brimstoneElemental].active)
            {
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }
            bool goIntoShell = (double)NPC.life <= (double)NPC.lifeMax * 0.1;
            bool provy = CalamityWorld.downedProvidence && !BossRushEvent.BossRushActive;
            if (goIntoShell || Main.npc[CalamityGlobalNPC.brimstoneElemental].ai[0] == 4f)
            {
                boostDR = true;
                NPC.chaseable = false;
            }
            else
            {
                boostDR = false;
                NPC.chaseable = true;
            }

            // Set DR based on boost status
            NPC.Calamity().DR = boostDR ? boostedDR : CalamityWorld.revenge ? normalDR : 0f;

            float num1446 = goIntoShell ? 1f : (BossRushEvent.BossRushActive ? 12f : 6f);
            int num1447 = 480;
            if (NPC.localAI[1] == 1f)
            {
                NPC.localAI[1] = 0f;
                if (Main.rand.NextBool(4))
                {
                    NPC.ai[0] = (float)num1447;
                }
            }
            NPC.TargetClosest(true);
            NPC.rotation = Math.Abs(NPC.velocity.X) * (float)NPC.direction * 0.1f;
            NPC.spriteDirection = (NPC.direction > 0) ? 1 : -1;
            Vector2 value53 = NPC.Center + new Vector2((float)(NPC.direction * 20), 6f);
            Vector2 vector251 = Main.player[NPC.target].Center - value53;
            bool flag104 = Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1);
            NPC.localAI[0] += 1f;
            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[0] >= 360f && Main.npc[CalamityGlobalNPC.brimstoneElemental].ai[0] != 4f)
            {
                NPC.localAI[0] = 0f;
                if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                {
                    float speed = BossRushEvent.BossRushActive ? 7.5f : 5f;
                    Vector2 vector = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)(NPC.height / 2));
                    float num6 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector.X + (float)Main.rand.Next(-10, 11);
                    float num7 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector.Y + (float)Main.rand.Next(-10, 11);
                    float num8 = (float)Math.Sqrt((double)(num6 * num6 + num7 * num7));
                    num8 = speed / num8;
                    num6 *= num8;
                    num7 *= num8;
					int type = ModContent.ProjectileType<BrimstoneHellfireball>();
					int damage = NPC.GetProjectileDamage(type);
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, num6, num7, type, damage + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
                }
            }
            if (vector251.Length() > 400f || !flag104)
            {
                Vector2 value54 = vector251;
                if (value54.Length() > num1446)
                {
                    value54.Normalize();
                    value54 *= num1446;
                }
                int num1448 = 30;
                NPC.velocity = (NPC.velocity * (float)(num1448 - 1) + value54) / (float)num1448;
            }
            else
            {
                NPC.velocity *= 0.98f;
            }
            if (NPC.ai[2] != 0f && NPC.ai[3] != 0f)
            {
                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                for (int num1449 = 0; num1449 < 20; num1449++)
                {
                    int num1450 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 235, 0f, 0f, 100, Color.Transparent, 1f);
                    Dust dust = Main.dust[num1450];
                    dust.velocity *= 3f;
                    Main.dust[num1450].noGravity = true;
                    Main.dust[num1450].scale = 2.5f;
                }
                NPC.Center = new Vector2(NPC.ai[2] * 16f, NPC.ai[3] * 16f);
                NPC.velocity = Vector2.Zero;
                NPC.ai[2] = 0f;
                NPC.ai[3] = 0f;
				if (NPC.localAI[0] > 240f) //Lower firing cooldown to prevent firing so quickly after a teleport
					NPC.localAI[0] = 240f;
                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                for (int num1451 = 0; num1451 < 20; num1451++)
                {
                    int num1452 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 235, 0f, 0f, 100, Color.Transparent, 1f);
                    Dust dust = Main.dust[num1452];
                    dust.velocity *= 3f;
                    Main.dust[num1452].noGravity = true;
                    Main.dust[num1452].scale = 2.5f;
                }
            }
            NPC.ai[0] += 1f;
            if (NPC.ai[0] >= (float)num1447 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.localAI[0] > 260f)
                {
                    NPC.localAI[0] -= 60f;
                }
                NPC.ai[0] = 0f;
                Point point12 = NPC.Center.ToTileCoordinates();
                Point point13 = Main.player[NPC.target].Center.ToTileCoordinates();
                int num1453 = 20;
                int num1454 = 3;
                int num1455 = 10;
                int num1456 = 1;
                int num1457 = 0;
                bool flag106 = false;
                if (vector251.Length() > 2000f)
                {
                    flag106 = true;
                }
                while (!flag106 && num1457 < 100)
                {
                    num1457++;
                    int num1458 = Main.rand.Next(point13.X - num1453, point13.X + num1453 + 1);
                    int num1459 = Main.rand.Next(point13.Y - num1453, point13.Y + num1453 + 1);
                    if ((num1459 < point13.Y - num1455 || num1459 > point13.Y + num1455 || num1458 < point13.X - num1455 || num1458 > point13.X + num1455) && (num1459 < point12.Y - num1454 || num1459 > point12.Y + num1454 || num1458 < point12.X - num1454 || num1458 > point12.X + num1454) && !Main.tile[num1458, num1459].HasUnactuatedTile)
                    {
                        bool flag107 = true;
                        if (flag107 && (Main.tile[num1458, num1459].LiquidType == LiquidID.Lava))
                        {
                            flag107 = false;
                        }
                        if (flag107 && Collision.SolidTiles(num1458 - num1456, num1458 + num1456, num1459 - num1456, num1459 + num1456))
                        {
                            flag107 = false;
                        }
                        if (flag107)
                        {
                            NPC.ai[2] = (float)num1458;
                            NPC.ai[3] = (float)num1459;
                            break;
                        }
                    }
                }
                NPC.netUpdate = true;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1.0;
            if (!boostDR)
            {
                if (NPC.frameCounter > 12.0)
                {
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                    NPC.frameCounter = 0.0;
                }
                if (NPC.frame.Y >= frameHeight * 4)
                {
                    NPC.frame.Y = 0;
                }
            }
            else
            {
                if (NPC.frameCounter > 12.0)
                {
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                    NPC.frameCounter = 0.0;
                }
                if (NPC.frame.Y < frameHeight * 4)
                {
                    NPC.frame.Y = frameHeight * 4;
                }
                if (NPC.frame.Y >= frameHeight * 8)
                {
                    NPC.frame.Y = frameHeight * 4;
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
