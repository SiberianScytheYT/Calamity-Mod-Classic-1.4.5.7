using CalRD.Buffs.StatBuffs;
using CalRD.Items.SummonItems;
using CalRD.Items.SummonItems.Invasion;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.Projectiles.Rogue;
using CalRD.World;
using System.Collections.Generic;
using CalRD.Utilities;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace CalRD.NPCs.TownNPCs
{
	[AutoloadHead]
    public class SEAHOE : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sea King");

            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 5;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = 16;
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
            NPC.width = 30;
            NPC.height = 58;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 25;
            NPC.lifeMax = 7500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.65f;
            AnimationType = NPCID.Guide;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BiomeManagers.SunkenSea>().Type };
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
	        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
	        {
		        new FlavorTextBestiaryInfoElement("Mods.CalRD.Bestiary.SeaKing")
	        });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            return CalamityWorld.downedCLAM && CalamityWorld.downedDesertScourge;
        }

        public override List<string> SetNPCNameList() => new List<string>() {this.GetLocalizedValue("Name.Amidias")};

        public override void AI()
        {
            NPC.breath += 2;
        }

        public override string GetChat()
        {
            IList<string> dialogue = new List<string>();
            Player player = Main.player[Main.myPlayer];

            if (NPC.homeless)
	            return this.GetLocalizedValue("Chat.Homeless" + Main.rand.Next(1, 2 + 1));
            
            if (Main.dayTime)
            {
	            dialogue.Add(this.GetLocalizedValue("Chat.Day1"));
	            dialogue.Add(this.GetLocalizedValue("Chat.Day2"));
	            dialogue.Add(this.GetLocalizedValue("Chat.Day3"));
	            dialogue.Add(this.GetLocalizedValue("Chat.Day4"));
            }
            else
            {
	            dialogue.Add(this.GetLocalizedValue("Chat.Night1"));
	            dialogue.Add(this.GetLocalizedValue("Chat.Night2"));
	            dialogue.Add(this.GetLocalizedValue("Chat.Night3"));
            }

            int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
            if (partyGirl != -1)
	            dialogue.Add(this.GetLocalization("Chat.PartyGirl").Format(Main.npc[partyGirl].GivenName));

            int lilBitch = NPC.FindFirstNPC(NPCID.Angler);
            if (lilBitch != -1)
            {
	            dialogue.Add(this.GetLocalization("Chat.Angler1").Format(Main.npc[lilBitch].GivenName));
	            dialogue.Add(this.GetLocalization("Chat.Angler2").Format(Main.npc[lilBitch].GivenName));
            }

            int cirrus = NPC.FindFirstNPC(ModContent.NPCType<FAP>());
            if (cirrus != -1)
	            dialogue.Add(this.GetLocalization("Chat.DrunkPrincess").Format(Main.npc[cirrus].GivenName));

            if (Main.bloodMoon)
	            dialogue.Add(this.GetLocalizedValue("Chat.BloodMoon"));

            /*
            int yharim = NPC.FindFirstNPC(NPCID.Yharim);
            if (yharim != -1)
            {
                dialogue.Add("Oh, Yharim... did he ever realize what he was doing? He seems so...calm, as if his rage had been satiated.");
            }*/

            if (Main.player[Main.myPlayer].Calamity().sirenPet)
	            dialogue.Add(this.GetLocalizedValue("Chat.SirenPet"));

            if (Main.player[Main.myPlayer].Calamity().sirenBoobs && !Main.player[Main.myPlayer].Calamity().sirenBoobsHide)
	            dialogue.Add(this.GetLocalizedValue("Chat.Siren"));

            if (Main.hardMode)
            {
	            dialogue.Add(this.GetLocalizedValue("Chat.Hardmode1"));
	            dialogue.Add(this.GetLocalizedValue("Chat.Hardmode2"));
            }

            if (NPC.downedMoonlord)
            {
	            dialogue.Add(this.GetLocalizedValue("Chat.MoonLordDefeated1"));
	            dialogue.Add(this.GetLocalizedValue("Chat.MoonLordDefeated2"));
	            dialogue.Add(this.GetLocalizedValue("Chat.MoonLordDefeated3"));
            }

            if (CalamityWorld.downedDoG)
	            dialogue.Add(this.GetLocalizedValue("Chat.DoGDefeated"));

            /*if (CalamityWorld.downedYharon)
            {
                dialogue.Add("I'm worried for Lord Yharim. He was not in a particular good mood once we found out you defeated his close friend.");
            }*/

            return dialogue[Main.rand.Next(dialogue.Count)];
        }

        public string Lore()
        {
            int selector = (int)NPC.Calamity().newAI[0];

            if (CalamityWorld.downedYharon)
	            return this.GetLocalizedValue("Help.YharonDefeated" + (1 + selector % 3));
            else if (CalamityWorld.downedDoG)
	            return this.GetLocalizedValue("Help.DoGDefeated" + (1 + selector % 4));
            else if (CalamityWorld.downedPolterghast)
	            return this.GetLocalizedValue("Help.PolterghastDefeated" + (1 + selector % 2));
            else if (CalamityWorld.downedProvidence)
	            return this.GetLocalizedValue("Help.ProvidenceDefeated" + (1 + selector % 3));
            else if (NPC.downedMoonlord)
	            return this.GetLocalizedValue("Help.MoonLordDefeated" + (1 + selector % 6));
            else if (NPC.downedGolemBoss)
	            return this.GetLocalizedValue("Help.GolemDefeated" + (1 + selector % 3));
            else if (Main.hardMode)
	            return this.GetLocalizedValue("Help.Hardmode" + (1 + selector % (CalamityWorld.downedCryogen ? 8 : 9)));
            else
            {
	            int chosen = 1 + (selector % 11);
	            string worldEvil = Language.GetTextValue("LegacyMisc." + (WorldGen.crimson ? 102 : 101));
	            
	            if (chosen == 6 || chosen == 10)
		            return this.GetLocalization("Help.PreHardmode" + chosen).Format(worldEvil);
	            return this.GetLocalizedValue("Help.PreHardmode" + chosen);
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = Language.GetTextValue("LegacyInterface.51");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
				Main.LocalPlayer.Calamity().newAmidiasInventory = false;
				shopName = "Shop";
            }
            else
            {
                Main.npcChatText = Lore();
				NPC.Calamity().newAI[0]++;
                Player player = Main.player[Main.myPlayer];
                player.AddBuff(ModContent.BuffType<AmidiasBlessing>(), 36000);
            }
        }

        public override void AddShops()
        {
	        NPCShop shop = new(Type);
            shop.Add(ModContent.ItemType<Shellshooter>())
            .Add(ModContent.ItemType<SnapClam>())
            .Add(ModContent.ItemType<SandDollar>())
            .Add(ModContent.ItemType<Waywasher>())
            .Add(ModContent.ItemType<CoralCannon>())
            .Add(ModContent.ItemType<UrchinFlail>())
            .Add(ModContent.ItemType<AmidiasTrident>())
            .Add(ModContent.ItemType<MagicalConch>())
            
            .AddWithCustomValue(ModContent.ItemType<DriedSeafood>(), Item.buyPrice(0, 2, 0, 0), CalamityGlobalTownNPC.downedDesertScourge)
            
            .AddWithCustomValue(ModContent.ItemType<CausticTear>(), Item.buyPrice(gold: 3), CalamityGlobalTownNPC.downedEoCAcidRain)
            
            .AddWithCustomValue(ModContent.ItemType<Seafood>(), Item.buyPrice(0, 20, 0, 0), CalamityGlobalTownNPC.downedAquaticScourge)
            
            .AddWithCustomValue(ItemID.TruffleWorm, Item.buyPrice(0, 40, 0, 0), Condition.DownedDukeFishron, CalamityGlobalTownNPC.sellVanillaSummons)
            
            .AddWithCustomValue(ModContent.ItemType<BloodwormItem>(), Item.buyPrice(8, 0, 0, 0), CalamityGlobalTownNPC.downedBoomerDuke)
            .Register();
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
	            if (Main.netMode != NetmodeID.Server)
	            {
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Amidias").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Amidias2").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Amidias3").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Amidias4").Type, 1f);
                }
            }
        }

        // Make this Town NPC teleport to the King statue when triggered.
        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 30;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<SnapClamProj>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 16f;
            gravityCorrection = 10f;
        }
    }
}
