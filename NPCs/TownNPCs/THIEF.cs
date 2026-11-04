using CalRD.Items.Accessories;
using CalRD.Items.Pets;
using CalRD.Items.Weapons.Rogue;
using CalRD.Projectiles.Rogue;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using CalRD.Items.Placeables.MusicBoxes;
using CalRD.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.GameContent.Events;
using Terraria.Localization;
using Terraria.ModLoader;
namespace CalRD.NPCs.TownNPCs
{
    [AutoloadHead]
    public class THIEF : ModNPC
    {
        string npcName;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bandit");

            Main.npcFrameCount[NPC.type] = 23;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 500;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 60;
            NPCID.Sets.AttackAverageChance[NPC.type] = 10;
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
            NPC.lavaImmune = false;
            NPC.width = 18;
            NPC.height = 44;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250; //Im not special :(
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.PartyGirl;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement("Mods.CalRD.Bestiary.Bandit")
            });
        }

		public override void AI()
		{
			if (!CalamityWorld.spawnedBandit)
			{
				CalamityWorld.spawnedBandit = true;
			}
		}

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
				bool rich = player.InventoryHas(ItemID.PlatinumCoin) || player.PortableStorageHas(ItemID.PlatinumCoin);
                if (player.active && rich)
                {
                    return NPC.downedBoss3 || CalamityWorld.spawnedBandit;
                }
            }
            return CalamityWorld.spawnedBandit;
        }

        public override List<string> SetNPCNameList() => new List<string>()
        {
            //Patron names
            "Xplizzy", // <@!98826096237109248> Whitegiraffe #6342

            //Original names
            this.GetLocalizedValue("Name.Laura"),
            this.GetLocalizedValue("Name.Mie"),
            this.GetLocalizedValue("Name.Bonnie"),
            this.GetLocalizedValue("Name.Sarah"),
            this.GetLocalizedValue("Name.Diane"),
            this.GetLocalizedValue("Name.Kate"),
            this.GetLocalizedValue("Name.Penelope"),
            this.GetLocalizedValue("Name.Marisa"),
            this.GetLocalizedValue("Name.Maribel"),
            this.GetLocalizedValue("Name.Valerie"),
            this.GetLocalizedValue("Name.Jessica"),
            this.GetLocalizedValue("Name.Rowan"),
            this.GetLocalizedValue("Name.Jessie"),
            this.GetLocalizedValue("Name.Jade"),
            this.GetLocalizedValue("Name.Hearn"),
            this.GetLocalizedValue("Name.Amber"),
            this.GetLocalizedValue("Name.Anne"),
            this.GetLocalizedValue("Name.Indiana")
        };

        public override string GetChat()
        {
            List<string> PossibleDialogs = new List<string>();
            if (!Main.dayTime && Main.bloodMoon)
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.BloodMoon1"));
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.BloodMoon2"));
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.BloodMoon3"));
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.BloodMoon4"));
            }
            else if (!Main.dayTime && !Main.bloodMoon)
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.Night1"));
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.Night2"));
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.Night3"));
            }

            if (BirthdayParty.PartyIsUp)
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.Party"));
            }
            if (NPC.GivenName == "Laura")
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.NamedLaura"));
            }
            if (NPC.GivenName == "Penelope")
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.NamedPenelope"));
            }
            if (NPC.GivenName == "Valerie")
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.NamedValerie"));
            }
            if (NPC.GivenName == "Rowan")
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.NamedRowan"));
            }

            PossibleDialogs.Add(this.GetLocalizedValue("Chat.Normal1"));
            PossibleDialogs.Add(this.GetLocalizedValue("Chat.Normal2"));
            PossibleDialogs.Add(this.GetLocalizedValue("Chat.Normal3"));
            PossibleDialogs.Add(this.GetLocalizedValue("Chat.Normal4"));
            PossibleDialogs.Add(this.GetLocalizedValue("Chat.Normal5"));
            PossibleDialogs.Add(this.GetLocalizedValue("Chat.Normal6"));
            PossibleDialogs.Add(this.GetLocalizedValue("Chat.Normal7"));
            PossibleDialogs.Add(this.GetLocalizedValue("Chat.Normal8"));
            PossibleDialogs.Add(this.GetLocalizedValue("Chat.Normal9"));

            if (Main.LocalPlayer.InventoryHas(ItemID.BoneGlove))
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.HasBoneGlove"));
            }
            if (Main.hardMode)
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.Hardmode1"));
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.Hardmode2"));
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.Hardmode3"));
            }
            if (NPC.downedMoonlord)
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.MoonLordDefeated1"));
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.MoonLordDefeated2"));
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.MoonLordDefeated3"));
            }
            if (Main.LocalPlayer.InventoryHas(ModContent.ItemType<Valediction>()) ||
                Main.LocalPlayer.InventoryHas(ModContent.ItemType<TheReaper>()))
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.HasValedictionorReaper"));
            }
            if (CalamityWorld.downedDoG)
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.DoGDefeated"));
            }
            if (Main.LocalPlayer.ZoneJungle)
            {
                PossibleDialogs.Add(this.GetLocalizedValue("Chat.Jungle"));
            }

            int merchantIndex = NPC.FindFirstNPC(NPCID.Merchant);
            if (merchantIndex != -1)
            {
                NPC nerd = Main.npc[merchantIndex];
                PossibleDialogs.Add(this.GetLocalization("Chat.Merchant").Format(nerd.GivenName));
            }

            int cirrusIndex = NPC.FindFirstNPC(ModContent.NPCType<FAP>());
            if (cirrusIndex != -1)
            {
                NPC cirrus = Main.npc[cirrusIndex]; //please help me I'm stuck in a children's video game - Fabsol
                PossibleDialogs.Add(this.GetLocalization("Chat.DrunkPrincess").Format(cirrus.GivenName));
            }

            int armsDealerIndex = NPC.FindFirstNPC(NPCID.ArmsDealer);
            int nurseIndex = NPC.FindFirstNPC(NPCID.Nurse);
            if (armsDealerIndex != -1 && nurseIndex != -1)
            {
                NPC cheeseMachine = Main.npc[nurseIndex];
                NPC minisharkMan = Main.npc[armsDealerIndex];
                PossibleDialogs.Add(this.GetLocalization("Chat.NurseArmsDealer").Format(cheeseMachine.GivenName, minisharkMan.GivenName));
            }

            return PossibleDialogs[Main.rand.Next(PossibleDialogs.Count)];
        }
        
        public string Refund(NPC bandit)
        {
            int goblinIndex = NPC.FindFirstNPC(NPCID.GoblinTinkerer);
            if (goblinIndex != -1 && CalamityWorld.Reforges >= 1)
            {
                CalamityWorld.Reforges = 0;
                int[] coinCounts = Utils.CoinsSplit(CalamityWorld.MoneyStolenByBandit);
                if (coinCounts[0] > 0)
                    Item.NewItem(new EntitySource_Gift(bandit), bandit.Hitbox, ItemID.CopperCoin, coinCounts[0]);
                if (coinCounts[1] > 0)
                    Item.NewItem(new EntitySource_Gift(bandit), bandit.Hitbox, ItemID.SilverCoin, coinCounts[1]);
                if (coinCounts[2] > 0)
                    Item.NewItem(new EntitySource_Gift(bandit), bandit.Hitbox, ItemID.GoldCoin, coinCounts[2]);
                if (coinCounts[3] > 0)
                    Item.NewItem(new EntitySource_Gift(bandit), bandit.Hitbox, ItemID.PlatinumCoin, coinCounts[3]);
                CalamityWorld.MoneyStolenByBandit = 0;
                NPC goblinFucker = Main.npc[goblinIndex];
                SoundEngine.PlaySound(SoundID.Coins); // Money dink sound
                switch (Main.rand.Next(2))
                {
                    case 0:
                        return this.GetLocalization("Refund1").Format(goblinFucker.GivenName);
                    case 1:
                        return this.GetLocalizedValue("Refund2");
                }
                CalamityNetcode.SyncWorld();
            }
            return this.GetLocalizedValue("NoRefund");
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var something = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/NPCs/TownNPCs/THIEF" + (BirthdayParty.PartyIsUp ? "Alt" : "")).Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY) - new Vector2(0f, 6f), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, something, 0);
            return false;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = this.GetLocalizedValue("RefundButton");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
				Main.LocalPlayer.Calamity().newBanditInventory = false;
				shopName = "Shop";
            }
            else
            {
                Main.npcChatText = Refund(NPC);
            }
        }

        public override void AddShops()
        {
            NPCShop shop = new(Type);
            shop.AddWithCustomValue(ModContent.ItemType<Cinquedea>(), Item.buyPrice(0, 9, 0, 0))
            .AddWithCustomValue(ModContent.ItemType<Glaive>(), Item.buyPrice(0, 3, 0, 0))
            .AddWithCustomValue(ModContent.ItemType<Kylie>(), Item.buyPrice(0, 9, 0, 0))
            .AddWithCustomValue(ModContent.ItemType<OldDie>(), Item.buyPrice(0, 40, 0, 0))
            .Add(ItemID.TigerClimbingGear)
            
            .Add(ModContent.ItemType<GelDart>(), CalamityGlobalTownNPC.downedSlimeGod)
           
            .AddWithCustomValue(ModContent.ItemType<SlickCane>(), Item.buyPrice(0, 25, 0, 0), Condition.Hardmode)
            
            .AddWithCustomValue(ModContent.ItemType<ThiefsDime>(), Item.buyPrice(1, 0, 0, 0), Condition.DownedPirates)
            
            .AddWithCustomValue(ModContent.ItemType<MomentumCapacitor>(), Item.buyPrice(0, 60, 0, 0), Condition.DownedDestroyer, Condition.DownedTwins, Condition.DownedSkeletronPrime)
            
            .Add(ModContent.ItemType<BouncingBetty>(), Condition.DownedMechBossAny)
            .Add(ModContent.ItemType<LatcherMine>(), Condition.DownedMechBossAny)
            
			.Add(ModContent.ItemType<DeepWounder>(), CalamityGlobalTownNPC.downedCalamitas)
            
            .Add(ModContent.ItemType<MonkeyDarts>(), Condition.DownedPlantera)
            .Add(ModContent.ItemType<GloveOfPrecision>(), Condition.DownedPlantera)
            .Add(ModContent.ItemType<GloveOfRecklessness>(), Condition.DownedPlantera)
            
            .AddWithCustomValue(ModContent.ItemType<EtherealExtorter>(), Item.buyPrice(1, 0, 0, 0), Condition.DownedGolem)
			
            .AddWithCustomValue(ModContent.ItemType<CelestialReaper>(), Item.buyPrice(2, 0, 0, 0), Condition.DownedMoonLord)
            
            .AddWithCustomValue(ModContent.ItemType<SylvanSlasher>(), Item.buyPrice(5, 0, 0, 0), CalamityGlobalTownNPC.downedProvidence)
            
            .AddWithCustomValue(ModContent.ItemType<VeneratedLocket>(), Item.buyPrice(25, 0, 0, 0), CalamityGlobalTownNPC.downedDoG)
            
            .AddWithCustomValue(ModContent.ItemType<DragonScales>(), Item.buyPrice(40, 0, 0, 0), CalamityGlobalTownNPC.dragonScalesAvailable)
            
            //:BearWatchingYou:
			.Add(ModContent.ItemType<BearEye>())
            .Register();
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Bandit").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Bandit2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Bandit3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Bandit4").Type, 1f);
                }
            }
        }

        // Make this Town NPC teleport to the Queen statue when triggered.
        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 50;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 180;
            randExtraCooldown = 60;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<CinquedeaProj>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 2f;
        }
    }
}
