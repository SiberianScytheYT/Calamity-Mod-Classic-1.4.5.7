using CalRD.Buffs.StatDebuffs;
using CalRD.Events;
using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Pets;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.Projectiles.Boss;
using CalRD.Tiles.Ores;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace CalRD.NPCs.Perforator
{
    [AutoloadBossHead]
    public class PerforatorHive : ModNPC
    {
        private bool small = false;
        private bool medium = false;
        private bool large = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Perforator Hive");
            Main.npcFrameCount[NPC.type] = 10;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers();
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
	        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
	        {
		        BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
		        new FlavorTextBestiaryInfoElement("Birthed in the crimson, though unknown if it's a servant of it, as its worms tear through the crimson flesh regularly.")
	        });
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 18f;
			NPC.GetNPCDamage();
			NPC.width = 110;
            NPC.height = 100;
            NPC.defense = 4;
            NPC.LifeMaxNERB(3750, 5400, 2700000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.buffImmune[ModContent.BuffType<GlacialState>()] = true;
            NPC.buffImmune[ModContent.BuffType<TemporalSadness>()] = true;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 6, 0, 0);
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit13;
            NPC.DeathSound = SoundID.NPCDeath19;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/BloodCoagulant");
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
			CalamityGlobalNPC.perfHive = NPC.whoAmI;

			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];

			bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;

			float enrageScale = 0f;
			if ((NPC.position.Y / 16f) < Main.worldSurface)
				enrageScale += 1f;
			if (!Main.player[NPC.target].ZoneCrimson)
				enrageScale += 1f;

			if (BossRushEvent.BossRushActive)
				enrageScale = 0f;

			if (!player.active || player.dead || Vector2.Distance(player.Center, NPC.Center) > 5600f)
			{
				NPC.TargetClosest(false);
				player = Main.player[NPC.target];
				if (!player.active || player.dead || Vector2.Distance(player.Center, NPC.Center) > 5600f)
				{
					NPC.rotation = NPC.velocity.X * 0.04f;

					if (NPC.velocity.Y < -3f)
						NPC.velocity.Y = -3f;
					NPC.velocity.Y += 0.1f;
					if (NPC.velocity.Y > 12f)
						NPC.velocity.Y = 12f;

					if (NPC.timeLeft > 60)
						NPC.timeLeft = 60;
					return;
				}
			}
			else if (NPC.timeLeft < 1800)
				NPC.timeLeft = 1800;

			int wormsAlive = 0;
			bool largeWormAlive = false;
			if (NPC.AnyNPCs(ModContent.NPCType<PerforatorHeadLarge>()))
			{
				wormsAlive++;
				largeWormAlive = true;
			}
			if (NPC.AnyNPCs(ModContent.NPCType<PerforatorHeadMedium>()))
				wormsAlive++;
			if (NPC.AnyNPCs(ModContent.NPCType<PerforatorHeadSmall>()))
				wormsAlive++;

			NPC.dontTakeDamage = largeWormAlive && expertMode;

			float playerLocation = NPC.Center.X - player.Center.X;
			NPC.direction = playerLocation < 0 ? 1 : -1;
			NPC.spriteDirection = NPC.direction;

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int shoot = (revenge ? 6 : 4) - wormsAlive;
				NPC.localAI[0] += Main.rand.Next(shoot);
				if (NPC.localAI[0] >= Main.rand.Next(300, 901) && NPC.position.Y + NPC.height < player.position.Y && Vector2.Distance(player.Center, NPC.Center) > 80f)
				{
					NPC.localAI[0] = 0f;
					SoundEngine.PlaySound(SoundID.NPCHit20, NPC.position);

					for (int num621 = 0; num621 < 8; num621++)
					{
						int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 170, 0f, 0f, 100, default, 1f);
						Main.dust[num622].velocity *= 3f;
						if (Main.rand.NextBool(2))
						{
							Main.dust[num622].scale = 0.25f;
							Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
						}
					}

					for (int num623 = 0; num623 < 16; num623++)
					{
						int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 5, 0f, 0f, 100, default, 1.5f);
						Main.dust[num624].noGravity = true;
						Main.dust[num624].velocity *= 5f;
						num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 5, 0f, 0f, 100, default, 1f);
						Main.dust[num624].velocity *= 2f;
					}

					int type = Main.rand.NextBool(2) ? ModContent.ProjectileType<IchorShot>() : ModContent.ProjectileType<BloodGeyser>();
					int damage = NPC.GetProjectileDamage(type);
					int totalProjectiles = death ? 16 : revenge ? 14 : expertMode ? 12 : 10;
					float maxVelocity = 8f;
					float velocityAdjustment = maxVelocity * 1.5f / totalProjectiles;
					Vector2 start = new Vector2(NPC.Center.X, NPC.Center.Y + 30f);
					Vector2 destination = wormsAlive > 0 ? new Vector2(Vector2.Normalize(player.Center - start).X, 0f) * maxVelocity * 0.4f : Vector2.Zero;
					Vector2 velocity = destination + Vector2.UnitY * -maxVelocity;
					for (int i = 0; i < totalProjectiles + 1; i++)
					{
						Projectile.NewProjectile(NPC.GetSource_FromThis(), start, velocity, type, damage, 0f, Main.myPlayer, 0f, 0f);
						velocity.X += velocityAdjustment * NPC.direction;
					}
				}
			}

			NPC.rotation = NPC.velocity.X * 0.04f;

			if (revenge)
			{
				if (wormsAlive == 1)
				{
					Movement(player, 4f, 1f, BossRushEvent.BossRushActive ? 0.2f : 0.15f, 160f, 300f, 400f, false);
					NPC.ai[0] = 0f;
				}
				else
				{
					if (NPC.ai[0] == 1f)
					{
						if (large || death)
						{
							Movement(player, 3.5f + 1.5f * enrageScale, 1f + 0.5f * enrageScale, BossRushEvent.BossRushActive ? 0.195f : death ? 0.15f : 0.13f, 360f, 10f, 50f, true);
						}
						else if (medium)
						{
							Movement(player, 4.5f + 1.5f * enrageScale, 1.5f + 0.5f * enrageScale, BossRushEvent.BossRushActive ? 0.18f : death ? 0.14f : 0.12f, 340f, 15f, 50f, true);
						}
						else if (small)
						{
							Movement(player, 5.5f + 1.5f * enrageScale, 2f + 0.5f * enrageScale, BossRushEvent.BossRushActive ? 0.165f : death ? 0.13f : 0.11f, 320f, 20f, 50f, true);
						}
						else
						{
							Movement(player, 6.5f + 1.5f * enrageScale, 2.5f + 0.5f * enrageScale, BossRushEvent.BossRushActive ? 0.15f : death ? 0.12f : 0.1f, 300f, 25f, 50f, true);
						}
					}
					else
					{
						NPC.velocity.X += NPC.Center.X <= player.Center.X ? -0.1f : 0.1f;
						if (NPC.Center.X > player.Center.X + 320f || NPC.Center.X < player.Center.X - 320f)
						{
							NPC.ai[0] = 1f;
						}
					}
				}
			}
			else
			{
				Movement(player, 2.5f + 1.5f * enrageScale, 0.5f + 0.5f * enrageScale, 0.1f, 160f, 300f, 400f, false);
			}

			if (NPC.ai[3] == 0f && NPC.life > 0)
			{
				NPC.ai[3] = NPC.lifeMax;
			}
			if (NPC.life > 0)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int num660 = (int)(NPC.lifeMax * 0.3);
					if ((NPC.life + num660) < NPC.ai[3])
					{
						NPC.ai[3] = NPC.life;
						int wormType = ModContent.NPCType<PerforatorHeadSmall>();
						if (!small)
						{
							small = true;
						}
						else if (!medium)
						{
							medium = true;
							wormType = ModContent.NPCType<PerforatorHeadMedium>();
						}
						else if (!large)
						{
							large = true;
							wormType = ModContent.NPCType<PerforatorHeadLarge>();
						}
						NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)(NPC.Center.Y + 800f), wormType, 1);
					}
				}
			}
		}

		private void Movement(Player target, float velocityX, float velocityY, float acceleration, float x, float y, float y2, bool charging)
		{
			if (NPC.position.Y > target.position.Y - y)
			{
				if (NPC.velocity.Y > 0f)
				{
					NPC.velocity.Y *= 0.98f;
				}
				NPC.velocity.Y -= acceleration;
				if (NPC.velocity.Y > velocityY)
				{
					NPC.velocity.Y = velocityY;
				}
			}
			else if (NPC.position.Y < target.position.Y - y2)
			{
				if (NPC.velocity.Y < 0f)
				{
					NPC.velocity.Y *= 0.98f;
				}
				NPC.velocity.Y += acceleration;
				if (NPC.velocity.Y < -velocityY)
				{
					NPC.velocity.Y = -velocityY;
				}
			}

			if (NPC.Center.X > target.Center.X + x)
			{
				if (NPC.velocity.X > 0f)
				{
					NPC.velocity.X *= 0.98f;
				}
				NPC.velocity.X -= acceleration;
				if (NPC.velocity.X > velocityX)
				{
					NPC.velocity.X = velocityX;
				}
			}
			else if (NPC.Center.X < target.Center.X - x)
			{
				if (NPC.velocity.X < 0f)
				{
					NPC.velocity.X *= 0.98f;
				}
				NPC.velocity.X += acceleration;
				if (NPC.velocity.X < -velocityX)
				{
					NPC.velocity.X = -velocityX;
				}
			}

			if (charging)
			{
				if (NPC.Center.X <= target.Center.X + x && NPC.Center.X >= target.Center.X - x)
				{
					NPC.velocity.X += (NPC.Center.X <= target.Center.X ? acceleration : -acceleration) * 0.25f;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));

			Vector2 vector43 = NPC.Center - screenPos;
			vector43 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/Perforator/PerforatorHiveGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Yellow, 0.5f);

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override bool CheckDead()
        {
            if (NPC.AnyNPCs(ModContent.NPCType<PerforatorHeadLarge>()))
            {
                return false;
            }
            return true;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<PerforatorBag>()));

            npcLoot.Add(ModContent.ItemType<PerforatorTrophy>(), 10);
            npcLoot.AddConditionalPerPlayer(() => !CalamityWorld.downedPerforator, ModContent.ItemType<KnowledgePerforators>(), 1);
            npcLoot.AddResidentEvilAmmo(info => !CalamityWorld.downedPerforator, 2, 0, 0);

			// All other drops are contained in the bag, so they only drop directly on Normal
			var normalOnly = npcLoot.DefineNormalOnlyDropSet();
            {
                // Materials
                normalOnly.Add(ModContent.ItemType<BloodSample>(), 1, 7, 14);
                normalOnly.Add(ItemID.CrimtaneBar, 1, 2, 5);
                normalOnly.Add(ItemID.Vertebrae, 1, 3, 9);
                normalOnly.AddIf(() => Main.hardMode, ItemID.Ichor, 1, 10, 20);

				// Weapons
				normalOnly.Add(DropHelper.CalamityStyle(DropHelper.DirectWeaponDropRateFraction, new DropHelper.WeightedItemStack[]
				{
					ModContent.ItemType<VeinBurster>(),
					ModContent.ItemType<BloodyRupture>(),
					ModContent.ItemType<SausageMaker>(),
					ModContent.ItemType<Aorta>(),
					ModContent.ItemType<Eviscerator>(),
					ModContent.ItemType<BloodBath>(),
					ModContent.ItemType<BloodClotStaff>(),
					new DropHelper.WeightedItemStack(ModContent.ItemType<ToothBall>(), 1f, 30, 50)
				}));

				//Equipment
				normalOnly.Add(ModContent.ItemType<BloodstainedGlove>(), 4);

                // Vanity
                normalOnly.Add(ModContent.ItemType<PerforatorMask>(), 7);
                normalOnly.Add(ModContent.ItemType<BloodyVein>(), 10);
            }

            
        }

        public override void OnKill()
        {
	        CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Dryad }, CalamityWorld.downedPerforator);
	        
	        // If neither The Hive Mind nor The Perforator Hive have been killed yet, notify players of Aerialite Ore
	        if (!CalamityWorld.downedHiveMind && !CalamityWorld.downedPerforator)
	        {
		        string key = "The ground is glittering with cyan light.";
		        Color messageColor = Color.Cyan;
		        WorldGenerationMethods.SpawnOre(ModContent.TileType<AerialiteOre>(), 12E-05, .4f, .6f);

		        CalamityUtils.DisplayLocalizedText(key, messageColor);
	        }

	        // Mark The Perforator Hive as dead
	        CalamityWorld.downedPerforator = true;
	        CalamityNetcode.SyncWorld();
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < hit.Damage / NPC.lifeMax * 100.0; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
	            if (Main.netMode != NetmodeID.Server)
	            {
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hive").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hive2").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hive3").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hive4").Type, 1f);
                }
                NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
                NPC.width = 100;
                NPC.height = 100;
                NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 5, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 5, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 5, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }
    }
}
