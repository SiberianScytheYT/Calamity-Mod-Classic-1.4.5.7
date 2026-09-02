using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Ranged;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using CalRD.BiomeManagers;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CalRD.NPCs.Abyss
{
    public class OarfishHead : ModNPC
    {
		private Vector2 patrolSpot = Vector2.Zero;
		public bool detectsPlayer = false;
		public const int minLength = 40;
        public const int maxLength = 41;
        public float speed = 3f; //10
        public float turnSpeed = 0.05f; //0.15
        bool TailSpawned = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Oarfish");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "CalRD/ExtraTextures/Bestiary/Oarfish_Bestiary",
                PortraitPositionYOverride = 20
            };
            value.Position.X += 20;
            value.Position.Y += 20;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("These colossal fish live in deep sea to avoid turbulent waters, and are likely the source for many myths.")
            });
        }

        public override void SetDefaults()
        {
            NPC.damage = 60;
            NPC.width = 59;
            NPC.height = 38;
            NPC.defense = 10;
            NPC.lifeMax = 4000;
            NPC.aiStyle = -1;
            AIType = -1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 10, 0);
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<OarfishBanner>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<AbyssLayer2Biome>().Type, ModContent.GetInstance<AbyssLayer3Biome>().Type };
        }

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(patrolSpot);
			writer.Write(detectsPlayer);
			writer.Write(NPC.chaseable);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			patrolSpot = reader.ReadVector2();
			detectsPlayer = reader.ReadBoolean();
			NPC.chaseable = reader.ReadBoolean();
		}

		public override void AI()
        {
			if ((Main.player[NPC.target].Center - NPC.Center).Length() < Main.player[NPC.target].Calamity().GetAbyssAggro(250f, 150f) ||
				NPC.justHit)
			{
				detectsPlayer = true;
			}
			NPC.chaseable = detectsPlayer;
			if (NPC.ai[3] > 0f)
            {
                NPC.realLife = (int)NPC.ai[3];
            }
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(true);
            }
            NPC.velocity.Length();
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!TailSpawned && NPC.ai[0] == 0f)
                {
                    int Previous = NPC.whoAmI;
                    for (int num36 = 0; num36 < maxLength; num36++)
                    {
                        int lol;
                        if (num36 >= 0 && num36 < minLength)
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<OarfishBody>(), NPC.whoAmI);
                        }
                        else
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<OarfishTail>(), NPC.whoAmI);
                        }
                        Main.npc[lol].realLife = NPC.whoAmI;
                        Main.npc[lol].ai[2] = (float)NPC.whoAmI;
                        Main.npc[lol].ai[1] = (float)Previous;
                        Main.npc[Previous].ai[0] = (float)lol;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, lol, 0f, 0f, 0f, 0);
                        Previous = lol;
                    }
                    TailSpawned = true;
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
            NPC.alpha -= 42;
            if (NPC.alpha < 0)
            {
                NPC.alpha = 0;
            }
            if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > 5600f || !NPC.AnyNPCs(ModContent.NPCType<OarfishTail>()))
            {
                NPC.active = false;
            }

            float num188 = speed;
            float num189 = turnSpeed;
            Vector2 vector18 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);

			if (patrolSpot == Vector2.Zero)
				patrolSpot = Main.player[NPC.target].Center;

			float num191 = detectsPlayer ? Main.player[NPC.target].Center.X : patrolSpot.X;
			float num192 = detectsPlayer ? Main.player[NPC.target].Center.Y : patrolSpot.Y;

			if (!detectsPlayer)
            {
				num192 += 300;
				if (Math.Abs(NPC.Center.X - num191) < 250f) //500
				{
					if (NPC.velocity.X > 0f)
					{
						num191 += 300f;
					}
					else
					{
						num191 -= 300f;
					}
				}
            }
			else
			{
				num188 *= 1.5f;
				num189 *= 1.5f;
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
            NPC.rotation = (float)System.Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 1.57f;
        }

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (projectile.minion && !projectile.Calamity().overridesMinionDamagePrevention)
			{
				return detectsPlayer;
			}
			return null;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.Calamity().ZoneAbyssLayer2 && spawnInfo.Water && !NPC.AnyNPCs(ModContent.NPCType<OarfishHead>()))
            {
                return SpawnCondition.CaveJellyfish.Chance * 0.3f;
            }
            if (spawnInfo.Player.Calamity().ZoneAbyssLayer3 && spawnInfo.Water && !NPC.AnyNPCs(ModContent.NPCType<OarfishHead>()))
            {
                return SpawnCondition.CaveJellyfish.Chance * 0.6f;
            }
            return 0f;
        }

        public override void OnKill()
        {
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<HalibutCannon>(), CalamityWorld.revenge, CalamityGlobalNPCLoot.halibutCannonBaseDropChance, 1, 1);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<DepthCells>(), CalamityWorld.downedCalamitas, 0.5f, 3, 5);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<DepthCells>(), CalamityWorld.downedCalamitas && Main.expertMode, 0.5f, 1, 2);
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

        public override bool CheckActive()
        {
            if (NPC.timeLeft <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int k = (int)NPC.ai[0]; k > 0; k = (int)Main.npc[k].ai[0])
                {
                    if (Main.npc[k].active)
                    {
                        Main.npc[k].active = false;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            Main.npc[k].life = 0;
                            Main.npc[k].netSkip = -1;
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, k, 0f, 0f, 0f, 0, 0, 0);
                        }
                    }
                }
            }
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 180, true);
        }
    }
}
