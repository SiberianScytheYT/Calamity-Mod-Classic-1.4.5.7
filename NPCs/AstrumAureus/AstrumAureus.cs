using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.Potions;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Summon;
using CalRD.Items.Weapons.Rogue;
using CalRD.NPCs.TownNPCs;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.NPCs.AstrumAureus
{
	[AutoloadBossHead]
    public class AstrumAureus : ModNPC
    {
        private bool stomping = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astrum Aureus");
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.27f,
                PortraitScale = 0.45f,
                PortraitPositionYOverride = -24f
            };
            value.Position.Y -= 20f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
		
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("One of Draedon's machines, even it succumbed to the infection's spread.")
            });
        }

        public override void SetDefaults()
        {
            NPC.lavaImmune = true;
			NPC.noGravity = true;
            NPC.npcSlots = 15f;
			NPC.GetNPCDamage();
			NPC.width = 400;
            NPC.height = 280;
            NPC.defense = 50;
			NPC.DR_NERD(0.15f);
            NPC.LifeMaxNERB(84000, NPC.downedMoonlord ? 390000 : 107000, 7400000); // 30 seconds in boss rush
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 15, 0, 0);
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
			NPC.buffImmune[BuffID.Venom] = false;
			NPC.buffImmune[BuffID.SoulDrain] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<HolyFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Plague>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WarCleave>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.buffImmune[ModContent.BuffType<SulphuricPoisoning>()] = false;
            NPC.boss = true;
            NPC.DeathSound = SoundID.NPCDeath14;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/Astrageldon");
            if (NPC.downedMoonlord && CalamityWorld.revenge)
            {
                NPC.value = Item.buyPrice(0, 35, 0, 0);
            }
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            SpawnModBiomes = new int[] { ModContent.GetInstance<BiomeManagers.Astral>().Type };
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(stomping);
            writer.Write(NPC.dontTakeDamage);
            writer.Write(NPC.chaseable);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			stomping = reader.ReadBoolean();
            NPC.dontTakeDamage = reader.ReadBoolean();
            NPC.chaseable = reader.ReadBoolean();
        }

        public override void AI()
        {
            CalamityAI.AstrumAureusAI(NPC, Mod);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.alpha == 0 && NPC.ai[0] > 1f;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[0] == 3f || NPC.ai[0] == 4f)
            {
                if (NPC.velocity.Y == 0f && NPC.ai[1] >= 0f && NPC.ai[0] == 3f) //idle before jump
                {
                    if (stomping)
                    {
                        stomping = false;
                    }
                    NPC.frameCounter += 1.0;
                    if (NPC.frameCounter > 12.0)
                    {
                        NPC.frame.Y = NPC.frame.Y + frameHeight;
                        NPC.frameCounter = 0.0;
                    }
                    if (NPC.frame.Y >= frameHeight * 6)
                    {
                        NPC.frame.Y = 0;
                    }
                }
                else if (NPC.velocity.Y <= 0f || NPC.ai[1] < 0f) //prepare to jump and then jump
                {
                    NPC.frameCounter += 1.0;
                    if (NPC.frameCounter > 12.0)
                    {
                        NPC.frame.Y = NPC.frame.Y + frameHeight;
                        NPC.frameCounter = 0.0;
                    }
                    if (NPC.frame.Y >= frameHeight * 5)
                    {
                        NPC.frame.Y = frameHeight * 5;
                    }
                }
                else //stomping
                {
                    if (!stomping)
                    {
                        stomping = true;
                        NPC.frameCounter = 0.0;
                        NPC.frame.Y = 0;
                    }
                    NPC.frameCounter += 1.0;
                    if (NPC.frameCounter > 12.0)
                    {
                        NPC.frame.Y = NPC.frame.Y + frameHeight;
                        NPC.frameCounter = 0.0;
                    }
                    if (NPC.frame.Y >= frameHeight * 5)
                    {
                        NPC.frame.Y = frameHeight * 5;
                    }
                }
            }
            else if (NPC.ai[0] >= 5f)
            {
                if (stomping)
                {
                    stomping = false;
                }
                if (NPC.velocity.Y == 0f) //idle before teleport
                {
                    NPC.frameCounter += 1.0;
                    if (NPC.frameCounter > 12.0)
                    {
                        NPC.frame.Y = NPC.frame.Y + frameHeight;
                        NPC.frameCounter = 0.0;
                    }
                    if (NPC.frame.Y >= frameHeight * 6)
                    {
                        NPC.frame.Y = 0;
                    }
                }
                else //in-air
                {
                    NPC.frameCounter += 1.0;
                    if (NPC.frameCounter > 12.0)
                    {
                        NPC.frame.Y = NPC.frame.Y + frameHeight;
                        NPC.frameCounter = 0.0;
                    }
                    if (NPC.frame.Y >= frameHeight * 5)
                    {
                        NPC.frame.Y = frameHeight * 5;
                    }
                }
            }
            else
            {
                if (stomping)
                {
                    stomping = false;
                }
                NPC.frameCounter += 1.0;
                if (NPC.frameCounter > 12.0)
                {
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                    NPC.frameCounter = 0.0;
                }
                if (NPC.frame.Y >= frameHeight * 6)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D NPCTexture = TextureAssets.Npc[NPC.type].Value;
            Texture2D GlowMaskTexture = TextureAssets.Npc[NPC.type].Value;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            if (NPC.ai[0] == 0f)
            {
                NPCTexture = TextureAssets.Npc[NPC.type].Value;
                GlowMaskTexture = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumAureus/AstrumAureusGlow").Value;
            }
            else if (NPC.ai[0] == 1f) //nothing special done here
            {
                NPCTexture = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumAureus/AstrumAureusRecharge").Value;
            }
            else if (NPC.ai[0] == 2f) //nothing special done here
            {
                NPCTexture = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumAureus/AstrumAureusWalk").Value;
                GlowMaskTexture = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumAureus/AstrumAureusWalkGlow").Value;
            }
            else if (NPC.ai[0] == 3f || NPC.ai[0] == 4f) //needs to have an in-air frame
            {
                if (NPC.velocity.Y == 0f && NPC.ai[1] >= 0f && NPC.ai[0] == 3f) //idle before jump
                {
                    NPCTexture = TextureAssets.Npc[NPC.type].Value; //idle frames
                    GlowMaskTexture = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumAureus/AstrumAureusGlow").Value;
                }
                else if (NPC.velocity.Y <= 0f || NPC.ai[1] < 0f) //jump frames if flying upward or if about to jump
                {
                    NPCTexture = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumAureus/AstrumAureusJump").Value;
                    GlowMaskTexture = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumAureus/AstrumAureusJumpGlow").Value;
                }
                else //stomping
                {
                    NPCTexture = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumAureus/AstrumAureusStomp").Value;
                    GlowMaskTexture = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumAureus/AstrumAureusStompGlow").Value;
                }
            }
            else if (NPC.ai[0] >= 5f) //needs to have an in-air frame
            {
                if (NPC.velocity.Y == 0f) //idle before teleport
                {
                    NPCTexture = TextureAssets.Npc[NPC.type].Value; //idle frames
                    GlowMaskTexture = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumAureus/AstrumAureusGlow").Value;
                }
                else //in-air frames
                {
                    NPCTexture = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumAureus/AstrumAureusJump").Value;
                    GlowMaskTexture = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumAureus/AstrumAureusJumpGlow").Value;
                }
            }

			int frameCount = Main.npcFrameCount[NPC.type];
			Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / frameCount / 2);
            Rectangle frame = NPC.frame;
            float scale = NPC.scale;
            float rotation = NPC.rotation;
            float offsetY = NPC.gfxOffY;
			Color color36 = Color.White;
			float amount9 = 0.5f;
			int num153 = 7;
			if (NPC.ai[0] == 3f || NPC.ai[0] == 4f)
				num153 = 10;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += 2)
				{
					Color color38 = drawColor;
					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
					vector41 -= new Vector2(NPCTexture.Width, NPCTexture.Height / frameCount) * scale / 2f;
					vector41 += vector11 * scale + new Vector2(0f, 4f + offsetY);
					spriteBatch.Draw(NPCTexture, vector41, frame, color38, rotation, vector11, scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - screenPos;
			vector43 -= new Vector2(NPCTexture.Width, NPCTexture.Height / frameCount) * scale / 2f;
			vector43 += vector11 * scale + new Vector2(0f, 4f + offsetY);
			spriteBatch.Draw(NPCTexture, vector43, frame, NPC.GetAlpha(drawColor), rotation, vector11, scale, spriteEffects, 0f);

			if (NPC.ai[0] != 1) //draw only if not recharging
            {
                Color color = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(Color.Gold);
				Color color40 = Color.Lerp(Color.White, color, 0.5f);

				if (CalamityConfig.Instance.Afterimages)
				{
					for (int num163 = 1; num163 < num153; num163++)
					{
						Color color41 = color40;
						color41 = Color.Lerp(color41, color36, amount9);
						color41 = NPC.GetAlpha(color41);
						color41 *= (num153 - num163) / 15f;
						Vector2 vector44 = NPC.oldPos[num163] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
						vector44 -= new Vector2(GlowMaskTexture.Width, GlowMaskTexture.Height / frameCount) * scale / 2f;
						vector44 += vector11 * scale + new Vector2(0f, 4f + offsetY);
						spriteBatch.Draw(GlowMaskTexture, vector44, frame, color41, rotation, vector11, scale, spriteEffects, 0f);
					}
				}

				spriteBatch.Draw(GlowMaskTexture, vector43, frame, color40, rotation, vector11, scale, spriteEffects, 0f);
			}

            return false;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<AstrageldonBag>()));

            npcLoot.Add(ModContent.ItemType<AstrageldonTrophy>(), 10);
            npcLoot.AddConditionalPerPlayer(() => !CalamityWorld.downedAstrageldon, ModContent.ItemType<KnowledgeAstrumAureus>(), 1);
            npcLoot.AddResidentEvilAmmo(info => !CalamityWorld.downedAstrageldon, 4, 2, 1);

			// All other drops are contained in the bag, so they only drop directly on Normal
            var normalOnly = npcLoot.DefineNormalOnlyDropSet();
            {
                // Materials
                normalOnly.Add(ModContent.ItemType<Stardust>(), 1, 20, 30);
                normalOnly.Add(ItemID.FallenStar, 1, 25, 40);

                // Weapons
                int[] weapons = new int[]
                {
                    ModContent.ItemType<Nebulash>(),
                    ModContent.ItemType<AuroraBlazer>(),
                    ModContent.ItemType<AlulaAustralis>(),
                    ModContent.ItemType<BorealisBomber>(),
                    ModContent.ItemType<AuroradicalThrow>()
                };
                
                normalOnly.Add(DropHelper.CalamityStyle(DropHelper.DirectWeaponDropRateFraction, weapons));

                // Vanity
                normalOnly.Add(ModContent.ItemType<AureusMask>(), 7);

                // Other
                normalOnly.Add(ModContent.ItemType<AstralJelly>(), 1, 9, 12);
                normalOnly.Add(ItemID.HallowedKey, 5);
            }
        }

        public override void OnKill()
        {
            CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Wizard, ModContent.NPCType<FAP>() }, CalamityWorld.downedAstrageldon);
            
            // Drop an Astral Meteor if applicable
            ThreadPool.QueueUserWorkItem(WorldGenerationMethods.AstralMeteorThreadWrapper);

            // If Astrum Aureus has not yet been killed, notify players of new Astral enemy drops
            if (!CalamityWorld.downedAstrageldon)
            {
                string key = "The astral enemies have been empowered!";
                string key2 = "A faint ethereal click can be heard from the dungeon.";
                Color messageColor = Color.Gold;

                CalamityUtils.DisplayLocalizedText(key, messageColor);
                CalamityUtils.DisplayLocalizedText(key2, messageColor);
            }

            // Mark Astrum Aureus as dead
            CalamityWorld.downedAstrageldon = true;
            CalamityNetcode.SyncWorld();
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 20;
                SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/NPCHit/AstrumAureusHit"), NPC.Center);
            }

            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 150;
                NPC.height = 100;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 50; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 100; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<AstralOrange>(), 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<AstralOrange>(), 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
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
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 240, true);
        }
    }
}
