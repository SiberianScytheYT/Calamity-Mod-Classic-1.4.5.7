using CalRD.Items.Accessories;
using CalRD.Items.Accessories.Vanity;
using CalRD.Items.Ammo;
using CalRD.Items.Materials;
using CalRD.Items.Potions;
using CalRD.Items.SummonItems;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.Projectiles.Typeless;
using CalRD.World;
using System.Collections.Generic;
using CalRD.Utilities;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace CalRD.NPCs.TownNPCs
{
	[AutoloadHead]
    public class DILF : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Archmage");

            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
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
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.8f;
            AnimationType = NPCID.Guide;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                new FlavorTextBestiaryInfoElement("Despite his age, he's still a real cool guy.")
            });
        }

		public override void AI()
		{
			if (!CalamityWorld.foundHomePermafrost && !NPC.homeless)
			{
				CalamityWorld.foundHomePermafrost = true;
			}
		}

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */ => CalamityWorld.downedCryogen;

        public override List<string> SetNPCNameList() /* tModPorter Suggestion: Return a list of names */
        {
            return new List<string>() {"Permafrost"};
        }

        public override string GetChat()
        {
            if (NPC.homeless && !CalamityWorld.foundHomePermafrost)
            {
                if (Main.rand.NextBool(2))
                    return "I deeply appreciate you rescuing me from being trapped within my frozen castle... It's been many, many years...";
                else
                    return "Thank you for saving me...though now I admit I am without a home since mine got destroyed.";
            }

            IList<string> dialogue = new List<string>();

            if (Main.dayTime)
            {
                dialogue.Add("I must admit...I am not quite used to this weather. It's too warm for my taste...");
                dialogue.Add("My dear! What is it you would like to talk about today?");
                dialogue.Add("Why...I don't have to worry about any time of the day! If it is hot...then I can use my ice magic to cool down!");
                dialogue.Add("I do usually prefer a spot of humidity for my ice magic. It likes to come out as steam when it's too hot and dry...");
                dialogue.Add("Magic is a skill that must be learned and practiced! You seem to have an inherent talent for it at your age. I have spent all of my life honing it...");
                dialogue.Add("Why ice magic, you ask? Well, my parents were both pyromaniacs...");
            }
            else
            {
                dialogue.Add("There be monsters lurking in the darkness. Most...unnatural monsters.");
                dialogue.Add("You could break the icy stillness in the air tonight.");
                dialogue.Add("Hmm...some would say that an unforeseen outside force is the root of the blood moon...");
                dialogue.Add("I was once the greatest Archmage of ice that ever hailed the lands. Whether or not that is still applicable, I am not sure...");
                dialogue.Add("There used to be other Archmages of other elements. I wonder where they are now...if they are also alive...");
                dialogue.Add("Oh...I wish I could tell you all about my life and the lessons I have learned, but it appears you have a great many things to do...");
            }

            dialogue.Add("I assure you, I will do my best to act as the cool grandfather figure you always wanted.");

            if (BirthdayParty.PartyIsUp)
                dialogue.Add("Sometimes...I feel like all I'm good for during these events is making ice cubes and slushies.");

            if (NPC.downedMoonlord)
            {
                dialogue.Add("Tread carefully, my friend... Now that the Moon Lord has been defeated, many powerful creatures will crawl out to challenge you...");
                dialogue.Add("I feel the balance of nature tilting farther than ever before. Is it due to you, or because of the events leading to now...?");
            }

            if (CalamityWorld.downedPolterghast)
                dialogue.Add("I felt a sudden chill down my spine. I sense something dangerous stirring in the Abyss...");

            if (CalamityWorld.downedYharon)
                dialogue.Add("...Where is Lord Yharim? He must be up to something...");

            int dryad = NPC.FindFirstNPC(NPCID.Dryad);
            if (dryad != -1)
                dialogue.Add("Yes, I am older than " + Main.npc[dryad].GivenName + ". You can stop asking now...");

            if (Main.player[Main.myPlayer].Calamity().chibii)
                dialogue.Add("What an adorable tiny companion you have! Where in the world did you find such a...creature...? Actually, I'd rather not know.");

            if (Main.player[Main.myPlayer].Calamity().cryogenSoul)
                dialogue.Add(Main.player[Main.myPlayer].name + "...just between us, I think I forgot my soul in the ice castle. If you see it, please do let me know.");

            if (Main.hardMode)
                dialogue.Add("It wouldn't be the first time something unknown and powerful dropped from the heavens...I would tread carefully if I were you...");

            return dialogue[Main.rand.Next(dialogue.Count)];
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
			if (firstButton)
			{
				Main.LocalPlayer.Calamity().newPermafrostInventory = false;
				shopName = "Shop";
			}
        }

        public override void AddShops()
        {
            Condition hasIceBarrage = ShopHelper.Create("hasIceBarrage", () => Main.LocalPlayer.HasItem(ModContent.ItemType<IceBarrage>()));

            NPCShop shop = new(Type);
            shop.Add(ModContent.ItemType<ColdheartIcicle>())
            .Add(ModContent.ItemType<FrostbiteBlaster>())
            .Add(ModContent.ItemType<IcicleTrident>())
            .Add(ModContent.ItemType<IceStar>())
            
            .Add(ModContent.ItemType<ArcticBearPaw>(), Condition.DownedDestroyer, Condition.DownedTwins, Condition.DownedSkeletronPrime)
            .Add(ModContent.ItemType<CryogenicStaff>(), Condition.DownedDestroyer, Condition.DownedTwins, Condition.DownedSkeletronPrime)
            .Add(ModContent.ItemType<FrostyFlare>(), Condition.DownedDestroyer, Condition.DownedTwins, Condition.DownedSkeletronPrime)
			
			.Add(ModContent.ItemType<AbsoluteZero>(), Condition.DownedIceQueen, Condition.DownedEverscream, Condition.DownedSantaNK1)
            .Add(ModContent.ItemType<EternalBlizzard>(), Condition.DownedIceQueen, Condition.DownedEverscream, Condition.DownedSantaNK1)
            .Add(ModContent.ItemType<WintersFury>(), Condition.DownedIceQueen, Condition.DownedEverscream, Condition.DownedSantaNK1)
            .Add(ModContent.ItemType<IcyBullet>(), Condition.DownedIceQueen, Condition.DownedEverscream, Condition.DownedSantaNK1)
            .Add(ModContent.ItemType<IcicleArrow>(), Condition.DownedIceQueen, Condition.DownedEverscream, Condition.DownedSantaNK1)
			
            .Add(ModContent.ItemType<PermafrostsConcoction>())
            .Add(ItemID.SuperManaPotion)
            .Add(ModContent.ItemType<DeliciousMeat>())
            .Add(ModContent.ItemType<EnchantedMetal>())
            .Add()
            .AddWithCustomValue(ModContent.ItemType<Popo>(), 1000000) //I think this is 5 platinum
            .AddWithCustomValue(ModContent.ItemType<CryoKey>(), Item.buyPrice(0, 15, 0, 0))
            .Add(ModContent.ItemType<BloodRune>(), hasIceBarrage)
            .Register();
        }

        // Make this Town NPC teleport to the King statue when triggered.
        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 9f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 10;
            randExtraCooldown = 50;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<DarkIce>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 2f;
        }
    }
}
