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
                new FlavorTextBestiaryInfoElement("Mods.CalRD.Bestiary.Permafrost")
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

        public override List<string> SetNPCNameList() => new List<string>() {this.GetLocalizedValue("Name.Permafrost")};

        public override string GetChat()
        {
            if (NPC.homeless && !CalamityWorld.foundHomePermafrost)
                return this.GetLocalizedValue("Chat.Homeless" + Main.rand.Next(1, 2 + 1));

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
                dialogue.Add(this.GetLocalizedValue("Chat.Night5"));
                dialogue.Add(this.GetLocalizedValue("Chat.Night6"));
            }

            dialogue.Add(this.GetLocalizedValue("Chat.Normal"));

            if (BirthdayParty.PartyIsUp)
                dialogue.Add(this.GetLocalizedValue("Chat.Party"));

            if (NPC.downedMoonlord)
            {
                dialogue.Add(this.GetLocalizedValue("Chat.MoonLordDefeated1"));
                dialogue.Add(this.GetLocalizedValue("Chat.MoonLordDefeated2"));
            }

            if (CalamityWorld.downedPolterghast)
                dialogue.Add(this.GetLocalizedValue("Chat.PolterghastDefeated"));

            if (CalamityWorld.downedYharon)
                dialogue.Add(this.GetLocalizedValue("Chat.YharonDefeated"));

            int dryad = NPC.FindFirstNPC(NPCID.Dryad);
            if (dryad != -1)
                dialogue.Add(this.GetLocalization("Chat.DryadAlive").Format(Main.npc[dryad].GivenName));

            if (Main.player[Main.myPlayer].Calamity().chibii)
                dialogue.Add(this.GetLocalizedValue("Chat.Chibii"));

            if (Main.player[Main.myPlayer].Calamity().cryogenSoul)
                dialogue.Add(this.GetLocalization("Chat.CryogenSoul").Format(Main.player[Main.myPlayer].name));

            if (Main.hardMode)
                dialogue.Add(this.GetLocalizedValue("Chat.Hardmode"));

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
