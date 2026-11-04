using CalRD.Events;
using CalRD.Items.Placeables.Furniture;
using CalRD.Items.Potions.Alcohol;
using CalRD.Projectiles.Typeless;
using CalRD.World;
using System.Collections.Generic;
using CalRD.Utilities;
using Humanizer;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace CalRD.NPCs.TownNPCs
{
    [AutoloadHead]
    public class FAP : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Drunk Princess");

            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 400;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 60;
            NPCID.Sets.AttackAverageChance[NPC.type] = 15;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.lavaImmune = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 20000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Guide;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("Mods.CalRD.Bestiary.DrunkPrincess")
            });
        }

		public override void AI()
		{
			if (!CalamityWorld.spawnedCirrus)
			{
				CalamityWorld.spawnedCirrus = true;
			}
		}

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
				bool hasVodka = player.InventoryHas(ModContent.ItemType<FabsolsVodka>())/* || player.PortableStorageHas(ModContent.ItemType<FabsolsVodka>())*/;
                if (player.active && hasVodka)
                {
                    return Main.hardMode || CalamityWorld.spawnedCirrus;
                }
            }
            return CalamityWorld.spawnedCirrus;
        }

        public override List<string> SetNPCNameList() => new List<string>() {this.GetLocalizedValue("Name.Cirrus")};

        public override string GetChat()
        {
            if (BossRushEvent.BossRushActive)
                return this.GetLocalizedValue("Chat.BossRush");

            if (NPC.homeless)
            {
                this.GetLocalizedValue("Chat.Homeless" + Main.rand.Next(1, 2 + 1));
            }

            if (Main.bloodMoon)
            {
                int random = Main.rand.Next(4);
                if (random == 0)
                {
                    return this.GetLocalizedValue("Chat.BloodMoon1");
                }
                else if (random == 1)
                {
                    return this.GetLocalizedValue("Chat.BloodMoon2");
                }
                else if (random == 2)
                {
                    return this.GetLocalizedValue("Chat.BloodMoon3");
                }
                else
                {
                    Main.player[Main.myPlayer].Hurt(PlayerDeathReason.ByCustomReason(CalamityUtils.GetText("Status.Death.Slap").Format(Main.player[Main.myPlayer].name)), Main.player[Main.myPlayer].statLife / 2, -Main.player[Main.myPlayer].direction, false, false, 0, false);
                    return this.GetLocalizedValue("Chat.BloodMoonSlap");
                }
            }

            if (CalamityGlobalNPC.AnyBossNPCS())
                return this.GetLocalizedValue("Chat.BossAlive");

            IList<string> dialogue = new List<string>();

            if (Main.dayTime)
            {
                dialogue.Add(this.GetLocalizedValue("Chat.Day1"));
                dialogue.Add(this.GetLocalizedValue("Chat.Day2"));
                dialogue.Add(this.GetLocalizedValue("Chat.Day3"));
                dialogue.Add(this.GetLocalizedValue("Chat.Day4"));
                dialogue.Add(this.GetLocalizedValue("Chat.Day5"));
                dialogue.Add(this.GetLocalizedValue("Chat.Day6"));
            }
            else
            {
                dialogue.Add(this.GetLocalizedValue("Chat.Night1"));
                dialogue.Add(this.GetLocalizedValue("Chat.Night2"));
                dialogue.Add(this.GetLocalizedValue("Chat.Night3"));
                dialogue.Add(this.GetLocalizedValue("Chat.Night4"));
                dialogue.Add(this.GetLocalization("Chat.Night5").Format(Main.player[Main.myPlayer].name));
                dialogue.Add(this.GetLocalizedValue("Chat.Night6"));
            }

            dialogue.Add(this.GetLocalizedValue("Chat.Normal1"));
            dialogue.Add(this.GetLocalizedValue("Chat.Normal2"));

            if (BirthdayParty.PartyIsUp)
                dialogue.Add(this.GetLocalizedValue("Chat.Party"));

            if (Main.invasionType == InvasionID.MartianMadness)
                dialogue.Add(this.GetLocalizedValue("Chat.Martians"));

            if (CalamityWorld.downedCryogen)
                dialogue.Add(this.GetLocalizedValue("Chat.CryogenDefeated"));

            if (CalamityWorld.downedLeviathan)
                dialogue.Add(this.GetLocalizedValue("Chat.LeviathanDefeated"));

            if (NPC.downedMoonlord)
            {
                dialogue.Add(this.GetLocalizedValue("Chat.MoonLordDefeated1"));
                dialogue.Add(this.GetLocalizedValue("Chat.MoonLordDefeated2"));
            }

			if (CalamityWorld.rainingAcid)
                dialogue.Add(this.GetLocalizedValue("Chat.AcidRain"));

            if (CalamityWorld.downedPolterghast)
                dialogue.Add(this.GetLocalizedValue("Chat.PolterghastDefeated"));

            if (CalamityWorld.downedDoG)
                dialogue.Add(this.GetLocalizedValue("Chat.DoGDefeated"));

            int tavernKeep = NPC.FindFirstNPC(NPCID.DD2Bartender);
            if (tavernKeep != -1)
            {
                dialogue.Add(this.GetLocalization("Chat.Tavernkeep1").Format(Main.npc[tavernKeep].GivenName));
                dialogue.Add(this.GetLocalization("Chat.Tavernkeep2").Format(Main.npc[tavernKeep].GivenName));
            }

            int permadong = NPC.FindFirstNPC(ModContent.NPCType<DILF>());
            if (permadong != -1)
                dialogue.Add(this.GetLocalization("Chat.Archmage").Format(Main.npc[permadong].GivenName));

            int waifu = NPC.FindFirstNPC(NPCID.Stylist);
            if (waifu != -1)
            {
                dialogue.Add(this.GetLocalization("Chat.Stylist1").Format(Main.npc[waifu].GivenName));
                dialogue.Add(this.GetLocalization("Chat.Stylist2").Format(Main.npc[waifu].GivenName));
                dialogue.Add(this.GetLocalization("Chat.Stylist3").Format(Main.npc[waifu].GivenName));
				dialogue.Add(this.GetLocalization("Chat.Stylist4").Format(Main.npc[waifu].GivenName));
			}

            if (Main.player[Main.myPlayer].Calamity().chibii)
                dialogue.Add(this.GetLocalizedValue("Chat.Chibii"));

            if (Main.player[Main.myPlayer].Calamity().sirenBoobs && !Main.player[Main.myPlayer].Calamity().sirenBoobsHide)
                dialogue.Add(this.GetLocalizedValue("Chat.Siren"));

            if (Main.player[Main.myPlayer].Calamity().fabsolVodka)
                dialogue.Add(this.GetLocalizedValue("Chat.Vodka"));

            if (Main.player[Main.myPlayer].Calamity().fab)
            {
                dialogue.Add(this.GetLocalizedValue("Chat.Alicorn1"));
                dialogue.Add(this.GetLocalizedValue("Chat.Alicorn2"));
                dialogue.Add(this.GetLocalizedValue("Chat.Alicorn3"));
            }

			IList<string> donorList = new List<string>(CalamityLists.donatorList);
			string[] donors = new string[12];
			for (int i = 0; i < 12; i++)
			{
				donors[i] = donorList[Main.rand.Next(donorList.Count)];
				donorList.Remove(donors[i]);
			}

			dialogue.Add(this.GetLocalization("DonorShoutout").Format(donors));

            return dialogue[Main.rand.Next(dialogue.Count)];
        }

        public string Death()
        {
            return this.GetLocalization("DeathCount").Format(Main.player[Main.myPlayer].Calamity().deathCount);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = this.GetLocalizedValue("DeathCountButton");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
				Main.LocalPlayer.Calamity().newCirrusInventory = false;
				shopName = "Shop";
            }
            else
            {
                Main.npcChatText = Death();
            }
        }

        public override void AddShops() //charges 50% extra than the original item value
        {
	        NPCShop shop = new(Type);
            shop.AddWithCustomValue(ModContent.ItemType<GrapeBeer>(), Item.buyPrice(0, 1, 0, 0))
            .AddWithCustomValue(ModContent.ItemType<RedWine>(), Item.buyPrice(0, 3, 0, 0))
			.AddWithCustomValue(ModContent.ItemType<Whiskey>(), Item.buyPrice(0, 5, 0, 0))
			.AddWithCustomValue(ModContent.ItemType<Rum>(), Item.buyPrice(0, 7, 50, 0))
			.AddWithCustomValue(ModContent.ItemType<Tequila>(), Item.buyPrice(0, 7, 50, 0))
			.AddWithCustomValue(ModContent.ItemType<Fireball>(), Item.buyPrice(0, 10, 0, 0))
			.AddWithCustomValue(ModContent.ItemType<FabsolsVodka>(), Item.buyPrice(0, 15, 0, 0))

			.AddWithCustomValue(ModContent.ItemType<Vodka>(), Item.buyPrice(0, 5, 0, 0), Condition.DownedDestroyer, Condition.DownedTwins, Condition.DownedSkeletronPrime)
            .AddWithCustomValue(ModContent.ItemType<Screwdriver>(), Item.buyPrice(0, 25, 0, 0), Condition.DownedDestroyer, Condition.DownedTwins, Condition.DownedSkeletronPrime)
            .AddWithCustomValue(ModContent.ItemType<WhiteWine>(), Item.buyPrice(0, 25, 0, 0), Condition.DownedDestroyer, Condition.DownedTwins, Condition.DownedSkeletronPrime)

			.AddWithCustomValue(ModContent.ItemType<EvergreenGin>(), Item.buyPrice(0, 25, 0, 0), Condition.DownedPlantera)
            .AddWithCustomValue(ModContent.ItemType<CaribbeanRum>(), Item.buyPrice(0, 30, 0, 0), Condition.DownedPlantera)
            .AddWithCustomValue(ModContent.ItemType<Margarita>(), Item.buyPrice(0, 35, 0, 0), Condition.DownedPlantera)
            
            .AddWithCustomValue(ModContent.ItemType<Everclear>(), Item.buyPrice(0, 10, 0, 0), CalamityGlobalTownNPC.downedAstrageldon)

            .AddWithCustomValue(ModContent.ItemType<BloodyMary>(), Item.buyPrice(0, 15, 0, 0), Condition.BloodMoon, CalamityGlobalTownNPC.downedAstrageldon)
            .AddWithCustomValue(ModContent.ItemType<StarBeamRye>(), Item.buyPrice(0, 20, 0, 0), Condition.TimeNight, CalamityGlobalTownNPC.downedAstrageldon)

			.AddWithCustomValue(ModContent.ItemType<Moonshine>(), Item.buyPrice(0, 5, 0, 0), Condition.DownedGolem)
            .AddWithCustomValue(ModContent.ItemType<MoscowMule>(), Item.buyPrice(0, 25, 0, 0), Condition.DownedGolem)
            .AddWithCustomValue(ModContent.ItemType<CinnamonRoll>(), Item.buyPrice(0, 25, 0, 0), Condition.DownedGolem)
            .AddWithCustomValue(ModContent.ItemType<TequilaSunrise>(), Item.buyPrice(0, 30, 0, 0), Condition.DownedGolem)
			
            .AddWithCustomValue(ModContent.ItemType<BlueCandle>(), Item.buyPrice(2, 0, 0, 0))
            .AddWithCustomValue(ModContent.ItemType<PinkCandle>(), Item.buyPrice(2, 0, 0, 0))
            .AddWithCustomValue(ModContent.ItemType<PurpleCandle>(), Item.buyPrice(2, 0, 0, 0))
            .AddWithCustomValue(ModContent.ItemType<YellowCandle>(), Item.buyPrice(2, 0, 0, 0))
            .AddWithCustomValue(ModContent.ItemType<OddMushroom>(), Item.buyPrice(3, 0, 0, 0))
            .Register();
        }

        // Make this Town NPC teleport to the Queen statue when triggered.
        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 15;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 180;
            randExtraCooldown = 60;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<CirrusRay>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 2f;
        }
    }
}
