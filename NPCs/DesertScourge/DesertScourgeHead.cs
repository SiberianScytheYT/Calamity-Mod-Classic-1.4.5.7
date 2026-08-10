using CalRD.Events;
using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.TownNPCs;
using CalRD.World;
using Microsoft.Xna.Framework;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace CalRD.NPCs.DesertScourge
{
    [AutoloadBossHead]
    public class DesertScourgeHead : ModNPC
    {
        private bool flies = false;
        private bool TailSpawned = false;
		public bool playRoarSound = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Desert Scourge");
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 12f;
            NPC.width = 32;
            NPC.height = 80;
            NPC.LifeMaxNERB(2300, 2650, 16500000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = 6;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.value = Item.buyPrice(0, 2, 0, 0);
            NPC.alpha = 255;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/DesertScourge");

			if (CalamityWorld.death || BossRushEvent.BossRushActive)
				NPC.scale = 1.25f;
			else if (CalamityWorld.revenge)
				NPC.scale = 1.15f;
			else if (Main.expertMode)
                NPC.scale = 1.1f;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.dontTakeDamage);
            writer.Write(playRoarSound);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.dontTakeDamage = reader.ReadBoolean();
            playRoarSound = reader.ReadBoolean();
        }

        public override void AI()
        {
            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;

			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				NPC.TargetClosest(true);

			Player player = Main.player[NPC.target];

			float enrageScale = 0f;
			if (!player.ZoneDesert)
				enrageScale += 2f;

			if (BossRushEvent.BossRushActive)
				enrageScale = 0f;

			// Percent life remaining
			float lifeRatio = NPC.life / (float)NPC.lifeMax;

			if (revenge || lifeRatio < (expertMode ? 0.75f : 0.5f))
				NPC.Calamity().newAI[0] += 1f;

			float burrowTimeGateValue = death ? 420f : 540f;
			bool burrow = NPC.Calamity().newAI[0] >= burrowTimeGateValue;
			bool resetTime = NPC.Calamity().newAI[0] >= burrowTimeGateValue + 600f;
			bool lungeUpward = burrow && NPC.Calamity().newAI[1] == 1f;
			bool quickFall = NPC.Calamity().newAI[1] == 2f;

			float speed = 8f;
			float turnSpeed = 0.08f;

			if (expertMode)
			{
				float velocityScale = death ? 9f : 6f;
				speed += velocityScale * (1f - lifeRatio);
				float accelerationScale = death ? 0.09f : 0.06f;
				turnSpeed += accelerationScale * (1f - lifeRatio);
			}

			speed += 4f * enrageScale;
			turnSpeed += 0.04f * enrageScale;

			if (lungeUpward)
			{
				speed *= 2f;
				turnSpeed *= 2f;
			}

			if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
			{
				speed *= 1.25f;
				turnSpeed *= 1.25f;
			}

			if (BossRushEvent.BossRushActive)
			{
				speed *= 1.25f;
				turnSpeed *= 1.25f;
			}

			if (NPC.ai[3] > 0f)
                NPC.realLife = (int)NPC.ai[3];

            NPC.alpha -= 42;
            if (NPC.alpha < 0)
                NPC.alpha = 0;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!TailSpawned && NPC.ai[0] == 0f)
                {
                    int Previous = NPC.whoAmI;
					int minLength = death ? 40 : revenge ? 35 : expertMode ? 30 : 25;
                    for (int num36 = 0; num36 < minLength + 1; num36++)
                    {
                        int lol;
                        if (num36 >= 0 && num36 < minLength)
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<DesertScourgeBody>(), NPC.whoAmI);
                        }
                        else
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<DesertScourgeTail>(), NPC.whoAmI);
                        }
                        Main.npc[lol].realLife = NPC.whoAmI;
                        Main.npc[lol].ai[2] = NPC.whoAmI;
                        Main.npc[lol].ai[1] = Previous;
                        Main.npc[Previous].ai[0] = lol;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, lol, 0f, 0f, 0f, 0);
                        Previous = lol;
                    }
                    TailSpawned = true;
                }
            }

            int num180 = (int)(NPC.position.X / 16f) - 1;
            int num181 = (int)((NPC.position.X + NPC.width) / 16f) + 2;
            int num182 = (int)(NPC.position.Y / 16f) - 1;
            int num183 = (int)((NPC.position.Y + NPC.height) / 16f) + 2;
            if (num180 < 0)
                num180 = 0;
            if (num181 > Main.maxTilesX)
                num181 = Main.maxTilesX;
            if (num182 < 0)
                num182 = 0;
            if (num183 > Main.maxTilesY)
                num183 = Main.maxTilesY;

            bool flag94 = flies || lungeUpward;
			if (!flag94)
			{
				for (int num952 = num180; num952 < num181; num952++)
				{
					for (int num953 = num182; num953 < num183; num953++)
					{
						if (Main.tile[num952, num953] != null && ((Main.tile[num952, num953].HasUnactuatedTile && (Main.tileSolid[Main.tile[num952, num953].TileType] || (Main.tileSolidTop[Main.tile[num952, num953].TileType] && Main.tile[num952, num953].TileFrameY == 0))) || Main.tile[num952, num953].LiquidAmount > 64))
						{
							Vector2 vector105;
							vector105.X = num952 * 16;
							vector105.Y = num953 * 16;
							if (NPC.position.X + NPC.width > vector105.X && NPC.position.X < vector105.X + 16f && NPC.position.Y + NPC.height > vector105.Y && NPC.position.Y < vector105.Y + 16f)
							{
								flag94 = true;
								break;
							}
						}
					}
				}
			}

			if (!flag94)
			{
				Rectangle rectangle12 = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
				int num954 = (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive)) ? 500 : 1000;
				if (enrageScale > 0f)
					num954 = 200;
				if (BossRushEvent.BossRushActive)
					num954 /= 2;

				bool flag95 = true;
				if (NPC.position.Y > player.position.Y)
				{
					int rectWidth = num954 * 2;
					int rectHeight = num954 * 2;
					for (int num955 = 0; num955 < Main.maxPlayers; num955++)
					{
						if (Main.player[num955].active)
						{
							int rectX = (int)Main.player[num955].position.X - num954;
							int rectY = (int)Main.player[num955].position.Y - num954;
							Rectangle rectangle13 = new Rectangle(rectX, rectY, rectWidth, rectHeight);
							if (rectangle12.Intersects(rectangle13))
							{
								flag95 = false;
								break;
							}
						}
					}

					if (flag95)
						flag94 = true;
				}
			}

            if (player.dead)
            {
				NPC.TargetClosest(false);
				flag94 = false;
                NPC.velocity.Y += 1f;
                if (NPC.position.Y > Main.worldSurface * 16.0)
                {
                    NPC.velocity.Y += 1f;
                }
                if (NPC.position.Y > Main.rockLayer * 16.0)
                {
                    for (int num957 = 0; num957 < Main.maxNPCs; num957++)
                    {
                        if (Main.npc[num957].aiStyle == NPC.aiStyle)
                        {
                            Main.npc[num957].active = false;
                        }
                    }
                }
            }

            float num188 = speed;
            float num189 = turnSpeed;
			float burrowTarget = player.Center.Y + 1000f;
			float lungeTarget = player.Center.Y - 600f;
			Vector2 vector18 = NPC.Center;
            float num191 = player.Center.X;
            float num192 = lungeUpward ? lungeTarget : burrow ? burrowTarget : player.Center.Y;
            num191 = (int)(num191 / 16f) * 16;
            num192 = (int)(num192 / 16f) * 16;
            vector18.X = (int)(vector18.X / 16f) * 16;
            vector18.Y = (int)(vector18.Y / 16f) * 16;
            num191 -= vector18.X;
            num192 -= vector18.Y;
            float num193 = (float)System.Math.Sqrt(num191 * num191 + num192 * num192);

			// Lunge up towards target
			if (burrow && NPC.Center.Y >= burrowTarget - 16f)
			{
				NPC.Calamity().newAI[1] = 1f;
				if (!playRoarSound)
				{
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/DesertScourgeRoar"), player.Center);
					playRoarSound = true;
				}
			}

			// Quickly fall back down once above target
			if (lungeUpward && NPC.Center.Y <= player.Center.Y - 420f)
			{
				NPC.Calamity().newAI[1] = 2f;
				playRoarSound = false;
			}

			// Quickly fall and reset variables once at target's Y position
			if (quickFall)
			{
				NPC.velocity.Y += 1f;
				if (NPC.Center.Y >= player.Center.Y)
				{
					NPC.Calamity().newAI[0] = 0f;
					NPC.Calamity().newAI[1] = 0f;
					playRoarSound = false;
				}
			}

			// Reset variables if the burrow and lunge attack is taking too long
			if (resetTime)
			{
				NPC.Calamity().newAI[0] = 0f;
				NPC.Calamity().newAI[1] = 0f;
			}

			if (!flag94)
            {
                NPC.TargetClosest(true);

                NPC.velocity.Y += turnSpeed * 0.75f;
                if (NPC.velocity.Y > num188)
                    NPC.velocity.Y = num188;

                if ((System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < num188 * 0.4)
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X -= num189 * 1.1f;
                    }
                    else
                    {
                        NPC.velocity.X += num189 * 1.1f;
                    }
                }
                else if (NPC.velocity.Y == num188)
                {
                    if (NPC.velocity.X < num191)
                    {
                        NPC.velocity.X += num189;
                    }
                    else if (NPC.velocity.X > num191)
                    {
                        NPC.velocity.X -= num189;
                    }
                }
                else if (NPC.velocity.Y > 4f)
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X += num189 * 0.9f;
                    }
                    else
                    {
                        NPC.velocity.X -= num189 * 0.9f;
                    }
                }
            }
            else
            {
                if (!flies && NPC.behindTiles && NPC.soundDelay == 0)
                {
                    float num195 = num193 / 40f;
                    if (num195 < 10f)
                    {
                        num195 = 10f;
                    }
                    if (num195 > 20f)
                    {
                        num195 = 20f;
                    }
                    NPC.soundDelay = (int)num195;
					//Play the worm digging sound.  No, I don't know why it's the same ID (but different style) as the generic boss roar and a scream
                    SoundEngine.PlaySound(SoundID.WormDig, NPC.position);
                }
                num193 = (float)System.Math.Sqrt(num191 * num191 + num192 * num192);
                float num196 = System.Math.Abs(num191);
                float num197 = System.Math.Abs(num192);
                float num198 = num188 / num193;
                num191 *= num198;
                num192 *= num198;
                bool flag21 = false;
                if (!flag21)
                {
                    if ((NPC.velocity.X > 0f && num191 > 0f) || (NPC.velocity.X < 0f && num191 < 0f) || (NPC.velocity.Y > 0f && num192 > 0f) || (NPC.velocity.Y < 0f && num192 < 0f))
                    {
                        if (NPC.velocity.X < num191)
                        {
                            NPC.velocity.X += num189;
                        }
                        else
                        {
                            if (NPC.velocity.X > num191)
                            {
                                NPC.velocity.X -= num189;
                            }
                        }
                        if (NPC.velocity.Y < num192)
                        {
                            NPC.velocity.Y += num189;
                        }
                        else
                        {
                            if (NPC.velocity.Y > num192)
                            {
                                NPC.velocity.Y -= num189;
                            }
                        }
                        if (System.Math.Abs(num192) < num188 * 0.2 && ((NPC.velocity.X > 0f && num191 < 0f) || (NPC.velocity.X < 0f && num191 > 0f)))
                        {
                            if (NPC.velocity.Y > 0f)
                            {
                                NPC.velocity.Y += num189 * 2f;
                            }
                            else
                            {
                                NPC.velocity.Y -= num189 * 2f;
                            }
                        }
                        if (System.Math.Abs(num191) < num188 * 0.2 && ((NPC.velocity.Y > 0f && num192 < 0f) || (NPC.velocity.Y < 0f && num192 > 0f)))
                        {
                            if (NPC.velocity.X > 0f)
                            {
                                NPC.velocity.X += num189 * 2f;
                            }
                            else
                            {
                                NPC.velocity.X -= num189 * 2f;
                            }
                        }
                    }
                    else
                    {
                        if (num196 > num197)
                        {
                            if (NPC.velocity.X < num191)
                            {
                                NPC.velocity.X += num189 * 1.1f;
                            }
                            else if (NPC.velocity.X > num191)
                            {
                                NPC.velocity.X -= num189 * 1.1f;
                            }
                            if ((System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < num188 * 0.5)
                            {
                                if (NPC.velocity.Y > 0f)
                                {
                                    NPC.velocity.Y += num189;
                                }
                                else
                                {
                                    NPC.velocity.Y -= num189;
                                }
                            }
                        }
                        else
                        {
                            if (NPC.velocity.Y < num192)
                            {
                                NPC.velocity.Y += num189 * 1.1f;
                            }
                            else if (NPC.velocity.Y > num192)
                            {
                                NPC.velocity.Y -= num189 * 1.1f;
                            }
                            if ((System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < num188 * 0.5)
                            {
                                if (NPC.velocity.X > 0f)
                                {
                                    NPC.velocity.X += num189;
                                }
                                else
                                {
                                    NPC.velocity.X -= num189;
                                }
                            }
                        }
                    }
                }

                NPC.rotation = (float)System.Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + MathHelper.PiOver2;

                if (flag94)
                {
                    if (NPC.localAI[0] != 1f)
                        NPC.netUpdate = true;

                    NPC.localAI[0] = 1f;
                }
                else
                {
                    if (NPC.localAI[0] != 0f)
                        NPC.netUpdate = true;

                    NPC.localAI[0] = 0f;
                }

                if (((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f) || (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f) || (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f) || (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f)) && !NPC.justHit)
                {
                    NPC.netUpdate = true;
                }
            }
        }

        #region Loot
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SandBlock;
        }

        public override bool SpecialOnKill()
        {
            int closestSegmentID = DropHelper.FindClosestWormSegment(NPC.GetSource_FromThis(), NPC,
                ModContent.NPCType<DesertScourgeHead>(),
                ModContent.NPCType<DesertScourgeBody>(),
                ModContent.NPCType<DesertScourgeTail>());
            NPC.position = Main.npc[closestSegmentID].position;
            return false;
        }

        public override void OnKill()
        {
            DropHelper.DropBags(ModContent.ItemType<DesertScourgeBag>(), NPC);

            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ItemID.LesserHealingPotion, 8, 14);
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<DesertScourgeTrophy>(), 10);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeDesertScourge>(), true, !CalamityWorld.downedDesertScourge);
            DropHelper.DropResidentEvilAmmo(NPC.GetSource_FromThis(), NPC, CalamityWorld.downedDesertScourge, 2, 0, 0);

			CalamityGlobalTownNPC.SetNewShopVariable(new int[] { ModContent.NPCType<SEAHOE>() }, CalamityWorld.downedDesertScourge);

			// All other drops are contained in the bag, so they only drop directly on Normal
			if (!Main.expertMode)
            {
                // Materials
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<VictoryShard>(), 7, 14);
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ItemID.Coral, 5, 9);
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ItemID.Seashell, 5, 9);
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ItemID.Starfish, 5, 9);

                // Weapons
                // Set up the base drop set, which includes Scourge of the Desert at its normal drop chance.
                float w = DropHelper.DirectWeaponDropRateFloat;
                DropHelper.WeightedItemStack[] weapons =
                {
                    DropHelper.WeightStack<AquaticDischarge>(w),
                    DropHelper.WeightStack<Barinade>(w),
                    DropHelper.WeightStack<StormSpray>(w),
                    DropHelper.WeightStack<SeaboundStaff>(w),
                    DropHelper.WeightStack<ScourgeoftheDesert>(w),
                };

                // If the RIV roll for Dune Hopper succeeds, REPLACE Scourge of the Desert with a guaranteed Dune Hopper.
                float duneHopperChance = DropHelper.RareVariantDropRateFloat;
                if (Main.rand.NextFloat() < duneHopperChance)
                    weapons[4] = DropHelper.WeightStack<DuneHopper>();

                DropHelper.DropEntireWeightedSet(NPC.GetSource_FromThis(), NPC, weapons);

                // Equipment
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<AeroStone>(), 10);
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<SandCloak>(), 10);
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<DeepDiver>(), DropHelper.RareVariantDropRateInt);

                // Vanity
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<DesertScourgeMask>(), 7);

                // Fishing
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<SandyAnglingKit>());
            }

            // If Desert Scourge has not been killed yet, notify players that the Sunken Sea is open and Sandstorms can happen
            if (!CalamityWorld.downedDesertScourge)
            {
                string key = "The depths of the underground desert are rumbling...";
                Color messageColor = Color.Aquamarine;
                string key2 = "The desert wind is blowing furiously!";
                Color messageColor2 = Color.PaleGoldenrod;

                CalamityUtils.DisplayLocalizedText(key, messageColor);
                CalamityUtils.DisplayLocalizedText(key2, messageColor2);

                if (!Terraria.GameContent.Events.Sandstorm.Happening)
                    Terraria.GameContent.Events.Sandstorm.StartSandstorm();
            }

            // Mark Desert Scourge as dead
            CalamityWorld.downedDesertScourge = true;
            CalamityNetcode.SyncWorld();
        }
        #endregion

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
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScourgeHead").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScourgeHead2").Type, 1f);
                }
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 300, true);
        }
    }
}
