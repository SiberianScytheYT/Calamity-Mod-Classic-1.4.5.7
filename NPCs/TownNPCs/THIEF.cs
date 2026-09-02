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

        public static List<string> PossibleNames = new List<string>()
        {
			//Patron names
			"Xplizzy", // <@!98826096237109248> Whitegiraffe #6342

			//Original names
            "Laura", "Mie", "Bonnie",
            "Sarah", "Diane", "Kate",
            "Penelope", "Marisa", "Maribel",
            "Valerie", "Jessica", "Rowan",
            "Jessie", "Jade", "Hearn",
            "Amber", "Anne", "Indiana"
        };

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
                new FlavorTextBestiaryInfoElement("A slick and capable thief that prefers keeping fights at a distance.")
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

        public override List<string> SetNPCNameList()/* tModPorter Suggestion: Return a list of names */
        {
            return PossibleNames;
        }

        public override string GetChat()
        {
            List<string> PossibleDialogs = new List<string>();
            if (!Main.dayTime && Main.bloodMoon)
            {
                PossibleDialogs.Add("Oy, watch where you're going or I might just use you for dart practice.");
                PossibleDialogs.Add("Bet you'd look good as a pincushion, amiright?");
                PossibleDialogs.Add("Zombies don't dodge very well. Maybe you'll do a bit better.");
                PossibleDialogs.Add("Hey, careful over there. I've rigged the place. One wrong step and you're going to get a knife in your forehead.");
            }
            else if (!Main.dayTime && !Main.bloodMoon)
            {
                PossibleDialogs.Add("You know what's really cool? Watching the glint of throwing stars as they reflect the moon.");
                PossibleDialogs.Add("You think those stars that fall occasionally would make good throwing weapons?");
                PossibleDialogs.Add("Statis' clan's got nothing on me. Mostly cause they're all dead.");
            }

            if (BirthdayParty.PartyIsUp)
            {
                PossibleDialogs.Add("Where is my party hat? Well, I stole it of course.");
            }
            if (NPC.GivenName == "Laura")
            {
                PossibleDialogs.Add("The nice thing about maps is I can track anything that has fallen.");
            }
            if (NPC.GivenName == "Penelope")
            {
                PossibleDialogs.Add("Imagine how fast you could throw if you just had more hands.");
            }
            if (NPC.GivenName == "Valerie")
            {
                PossibleDialogs.Add("I also take food for currency.");
            }
            if (NPC.GivenName == "Rowan")
            {
                PossibleDialogs.Add("Usually I only think of animals as food or target practice, but dragons are an exception.");
            }

            PossibleDialogs.Add("Anything is a weapon if you throw it hard enough.");
            PossibleDialogs.Add("That's your chucking arm? You need to work out more.");
            PossibleDialogs.Add("Listen here. It's all in the wrist, the wrist! Oh, forget it.");
            PossibleDialogs.Add("I don't think Mom and Dad are proud of the job I have right now.");
            PossibleDialogs.Add("Eh you know how it goes; steal from the rich, give to the poor. Of course, for a price.");
            PossibleDialogs.Add("Want to hear about this one time I was stuck in a room with a rabid dog and a dead guy?");
            PossibleDialogs.Add("Argh snakes. For some reason it's always snakes.");
            PossibleDialogs.Add("Maybe I'm bitter. It's been a long time, so whatever. Just do a good job out there.");
            PossibleDialogs.Add("It's not stealing! I'm just borrowing it until I die!");

            if (Main.LocalPlayer.InventoryHas(ItemID.BoneGlove))
            {
                PossibleDialogs.Add("Wouldn't be the first time I used my friends' remains as weapons.");
            }
            if (Main.hardMode)
            {
                PossibleDialogs.Add("With all of this new stuff cropping up, looks like we got some easy loot and new items to craft up, eh? Well, YOU craft them, I'll steal em.");
                PossibleDialogs.Add("Draedon thinks he can build awesome machines, but he doesn't know how much crap I've stolen from him and sold by dismantling his drones.");
                PossibleDialogs.Add("Gramma always said never to invade ancient temples or you'll be cursed and die. Let's say both of us attest that is untrue. We're still alive. Somewhat.");
            }
            if (NPC.downedMoonlord)
            {
                PossibleDialogs.Add("I heard that there's some really neat and awesome rogue items you can get. Show em to me if you ever get the time.");
                PossibleDialogs.Add("Providence HATES it when you take her stuff. I learned that the hard way.");
                PossibleDialogs.Add("You think I can get away with looting from ghosts? It ain't like they can pick things up.");
            }
            if (Main.LocalPlayer.InventoryHas(ModContent.ItemType<Valediction>()) ||
                Main.LocalPlayer.InventoryHas(ModContent.ItemType<TheReaper>()))
            {
                PossibleDialogs.Add("Oh man, did you rip that off a shark!? Now that's a weapon!");
            }
            if (CalamityWorld.downedDoG)
            {
                PossibleDialogs.Add("I tried looting Storm Weaver's armor once. Before I could get a chunk of the stuff... well let's just say the bigger, fatter cosmic worm arrived and it didn't end well.");
            }
            if (Main.LocalPlayer.ZoneJungle)
            {
                PossibleDialogs.Add("I'd rather not be here. This place has bad vibes, y'know? It brings back some unpleasant memories.");
            }

            int merchantIndex = NPC.FindFirstNPC(NPCID.Merchant);
            if (merchantIndex != -1)
            {
                NPC nerd = Main.npc[merchantIndex];
                PossibleDialogs.Add($"Don't tell {nerd.GivenName}, but I took some of his stuff and replaced it with Angel Statues.");
            }

            int cirrusIndex = NPC.FindFirstNPC(ModContent.NPCType<FAP>());
            if (cirrusIndex != -1)
            {
                NPC cirrus = Main.npc[cirrusIndex]; //please help me I'm stuck in a children's video game - Fabsol
                PossibleDialogs.Add($"I learned never to steal {cirrus.GivenName}'s drinks. She doesn't appreciate me right now so I'll go back to hiding.");
            }

            int armsDealerIndex = NPC.FindFirstNPC(NPCID.ArmsDealer);
            int nurseIndex = NPC.FindFirstNPC(NPCID.Nurse);
            if (armsDealerIndex != -1 && nurseIndex != -1)
            {
                NPC cheeseMachine = Main.npc[nurseIndex];
                NPC minisharkMan = Main.npc[armsDealerIndex];
                PossibleDialogs.Add($"Don't tell {cheeseMachine.GivenName} that I was responsible for {minisharkMan.GivenName}'s injuries.");
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
                        return $"Want in on a little secret? Since {goblinFucker.GivenName} always gets so much cash from you, I've been stealing some of it as we go. I need you to keep quiet about it, so here.";
                    case 1:
                        return "Hey, if government officials can get tax, why can't I? The heck do you mean that these two things are nothing alike?";
                }
                CalamityNetcode.SyncWorld();
            }
            return "Sorry, I got nothing. Perhaps you could reforge something and come back later...";
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
            button2 = "Refund";
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
