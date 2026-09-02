using CalRD.Items.Accessories;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Melee;
using CalRD.Projectiles.Enemy;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.NormalNPCs
{
    public class ThiccWaifu : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cloud Elemental");
            Main.npcFrameCount[NPC.type] = 8;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Position = new Vector2(28f, 20f),
                Scale = 0.65f,
                PortraitScale = 0.65f,
                PortraitPositionXOverride = 10f,
                PortraitPositionYOverride = 2f
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
		
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("Once a revered deity, she remains angry at the world and the people which abandoned her.")
            });
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 3f;
            NPC.damage = 38;
            NPC.width = 80;
            NPC.height = 140;
            NPC.defense = 18;
			NPC.DR_NERD(0.05f);
            NPC.lifeMax = 6000;
            NPC.knockBackResist = 0.05f;
            NPC.value = Item.buyPrice(0, 1, 50, 0);
            NPC.HitSound = SoundID.NPCHit23;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.buffImmune[20] = true;
            NPC.buffImmune[44] = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.rarity = 2;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CloudElementalBanner>();
        }

        public override void AI()
        {
            Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.375f, 0.5f, 0.625f);

            float num1457 = 0.1f;
            float num1458 = 2f;
            float num1460 = -4f;
            float num1461 = 4f;
            float num1462 = 0.1f;
            bool flag116 = false;
            float scaleFactor26 = 0.96f;
            bool flag117 = true;

            NPC.rotation = NPC.velocity.X * 0.04f;
            NPC.spriteDirection = (NPC.direction > 0) ? 1 : -1;

            float num1465 = (float)NPC.life / (float)NPC.lifeMax;
            num1461 += (1f - num1465) * 6f;
            num1462 += (1f - num1465) * 0.02f;
			if (CalamityWorld.death)
			{
				num1461 = 10f;
				num1462 = 0.15f;
			}

            if (num1465 < 0.5f || CalamityWorld.death)
                NPC.knockBackResist = 0f;

            NPC.localAI[2] = 0f;

            if (NPC.ai[0] < 0f)
                NPC.ai[0] = MathHelper.Min(NPC.ai[0] + 1f, 0f);

            if (NPC.ai[0] > 0f)
            {
                flag117 = false;
                flag116 = true;
                NPC.ai[0] += 1f;
                if (NPC.ai[0] >= 135f)
                {
                    NPC.ai[0] = -300f;
                    NPC.netUpdate = true;
                }
                Vector2 vector = NPC.Center;
                vector = Vector2.UnitX * (float)NPC.direction * 200f;
                Vector2 vector223 = NPC.Center + Vector2.UnitX * (float)NPC.direction * 50f - Vector2.UnitY * 6f;
                if (NPC.ai[0] == 54f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    List<Point> list4 = new List<Point>();
                    Vector2 vec5 = Main.player[NPC.target].Center + new Vector2(Main.player[NPC.target].velocity.X * 30f, 0f);
                    Point point14 = vec5.ToTileCoordinates();
                    int num1468 = 0;
                    while (num1468 < 1000 && list4.Count < 3)
                    {
                        bool flag118 = false;
                        int num1469 = Main.rand.Next(point14.X - 30, point14.X + 30 + 1);
                        foreach (Point current in list4)
                        {
                            if (Math.Abs(current.X - num1469) < 10)
                            {
                                flag118 = true;
                                break;
                            }
                        }
                        if (!flag118)
                        {
                            int startY = point14.Y - 20;
                            int num1470;
                            int num1471;
                            Collision.ExpandVertically(num1469, startY, out num1470, out num1471, 1, 51);
                            list4.Add(new Point(num1469, num1471 - 15));
                        }
                        num1468++;
                    }
                    foreach (Point current2 in list4)
                    {
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), (float)(current2.X * 16), (float)(current2.Y * 16), 0f, 0f, ModContent.ProjectileType<StormMarkHostile>(), 0, 0f, Main.myPlayer, 0f, 0f);
                    }
                }
                new Vector2(0.9f, 2f);
                if (NPC.ai[0] < 114f && NPC.ai[0] > 0f)
                {
                    List<Vector2> list5 = new List<Vector2>();
                    for (int num1472 = 0; num1472 < 1000; num1472++)
                    {
                        Projectile projectile9 = Main.projectile[num1472];
                        if (projectile9.active && projectile9.type == ModContent.ProjectileType<StormMarkHostile>())
                        {
                            list5.Add(projectile9.Center);
                        }
                    }
                }
            }

            if (NPC.ai[0] == 0f)
            {
                NPC.ai[0] = 1f;
                NPC.netUpdate = true;
                flag116 = true;
            }

            if (NPC.justHit)
                NPC.localAI[2] = 0f;

            if (NPC.localAI[2] >= 0f)
            {
                float num1477 = 16f;
                bool flag119 = false;
                bool flag120 = false;
                if (NPC.position.X > NPC.localAI[0] - num1477 && NPC.position.X < NPC.localAI[0] + num1477)
                {
                    flag119 = true;
                }
                else if ((NPC.velocity.X < 0f && NPC.direction > 0) || (NPC.velocity.X > 0f && NPC.direction < 0))
                {
                    flag119 = true;
                    num1477 += 24f;
                }
                if (NPC.position.Y > NPC.localAI[1] - num1477 && NPC.position.Y < NPC.localAI[1] + num1477)
                {
                    flag120 = true;
                }
                if (flag119 && flag120)
                {
                    NPC.localAI[2] += 1f;
                    if (NPC.localAI[2] >= 60f)
                    {
                        NPC.localAI[2] = -180f;
                        NPC.direction *= -1;
                        NPC.velocity.X = NPC.velocity.X * -1f;
                        NPC.collideX = false;
                    }
                }
                else
                {
                    NPC.localAI[0] = NPC.position.X;
                    NPC.localAI[1] = NPC.position.Y;
                    NPC.localAI[2] = 0f;
                }
                if (flag117)
                {
                    NPC.TargetClosest(true);
                }
            }
            else
            {
                NPC.localAI[2] += 1f;
                NPC.direction = (Main.player[NPC.target].Center.X > NPC.Center.X) ? 1 : -1;
            }

            // Slow down when spawning tornadoes
            if (flag116)
            {
                NPC.velocity *= scaleFactor26;
                return;
            }

            // Float up or down towards target
            if (NPC.position.Y > Main.player[NPC.target].position.Y - 50f)
            {
                if (NPC.velocity.Y > 0f)
                    NPC.velocity.Y = NPC.velocity.Y * 0.99f;

                NPC.velocity.Y = NPC.velocity.Y - num1457;

                if (NPC.velocity.Y > num1458)
                    NPC.velocity.Y = num1458;
            }
            else if (NPC.position.Y < Main.player[NPC.target].position.Y - 100f)
            {
                if (NPC.velocity.Y < 0f)
                    NPC.velocity.Y = NPC.velocity.Y * 0.99f;

                NPC.velocity.Y = NPC.velocity.Y + num1457;

                if (NPC.velocity.Y < num1460)
                    NPC.velocity.Y = num1460;
            }

            // Float back and forth near target
            if (NPC.position.X + (float)(NPC.width / 2) > Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) + 50f)
            {
                if (NPC.velocity.X > 0f)
                    NPC.velocity.X = NPC.velocity.X * 0.99f;

                NPC.velocity.X = NPC.velocity.X - num1462;

                if (NPC.velocity.X > num1461)
                    NPC.velocity.X = num1461;
            }
            if (NPC.position.X + (float)(NPC.width / 2) < Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - 50f)
            {
                if (NPC.velocity.X < 0f)
                    NPC.velocity.X = NPC.velocity.X * 0.99f;

                NPC.velocity.X = NPC.velocity.X + num1462;

                if (NPC.velocity.X < -num1461)
                    NPC.velocity.X = -num1461;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!NPC.active || NPC.IsABestiaryIconDummy)
                return true;
            Texture2D texture = ModContent.Request<Texture2D>("CalRD/NPCs/NormalNPCs/ThiccWaifuAttack").Value;
            if (NPC.ai[0] > 0f)
            {
                CalRD.DrawTexture(spriteBatch, texture, 0, NPC, drawColor);
            }
            else
            {
                CalRD.DrawTexture(spriteBatch, TextureAssets.Npc[NPC.type].Value, 0, NPC, drawColor);
            }
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter = NPC.frameCounter + (double)(NPC.velocity.Length() * 0.1f) + 1.0;
            if (NPC.frameCounter >= (NPC.ai[0] > 0f ? 16.0 : 8.0))
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0.0;
            }
            if (NPC.frame.Y >= frameHeight * 8)
            {
                NPC.frame.Y = 0;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !Main.hardMode || !Main.raining || NPC.AnyNPCs(ModContent.NPCType<ThiccWaifu>()))
            {
                return 0f;
            }
            return SpawnCondition.Sky.Chance * 0.1f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 16, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 50; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 16, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnKill()
        {
			DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EssenceofCinder>(), 2, 3);
			DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EyeoftheStorm>(), Main.expertMode ? 3 : 4);
			DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<StormSaber>(), 5);
        }
    }
}
