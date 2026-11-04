using System;
using CalRD.Items;
using CalRD.Items.Accessories;
using CalRD.Items.Ammo;
using CalRD.Items.Armor;
using CalRD.Items.Dyes;
using CalRD.Items.Dyes.HairDye;
using CalRD.Items.Materials;
using CalRD.Items.Pets;
using CalRD.Items.Placeables.Furniture.Fountains;
using CalRD.Items.Placeables.Walls;
using CalRD.Items.Potions;
using CalRD.Items.Potions.Alcohol;
using CalRD.Items.SummonItems;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.NPCs.TownNPCs;
using CalRD.Utilities;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ShopHelper = CalRD.Utilities.ShopHelper;

namespace CalRD.NPCs
{
	public class CalamityGlobalTownNPC : GlobalNPC
	{
		#region Town NPC Patreon Name Sets
		private const int AnglerVanillaNames = 22;
		private static readonly string[] AnglerNames =
		{
			"Dazren",
		};
		private const int ArmsDealerVanillaNames = 24;
		private static readonly string[] ArmsDealerNames =
		{
			"Drifter",
			"Finchi",
		};
		private const int ClothierVanillaNames = 25;
		private static readonly string[] ClothierNames =
		{
			"Joeseph Jostar",
		};
		private const int CyborgVanillaNames = 22;
		private static readonly string[] CyborgNames = null;
		private const int DemolitionistVanillaNames = 22;
		private static readonly string[] DemolitionistNames = null;
		private const int DryadVanillaNames = 21;
		private static readonly string[] DryadNames =
		{
			"Rythmi",
			"Izuna",
			"Jasmine", // <@!430532867479699456> (phantasmagoria#6777)
		};
		private const int DyeTraderVanillaNames = 16;
		private static readonly string[] DyeTraderNames = null;
		private const int GoblinTinkererVanillaNames = 25;
		private static readonly string[] GoblinTinkererNames =
		{
			"Verth",
			"Gormer", // <@!287651204924833795> (Picasso's Bean#2819)
		};
		private const int GuideVanillaNames = 34;
		private static readonly string[] GuideNames =
		{
			"Lapp",
			"Ben Shapiro",
			"StreakistYT",
			"Necroplasmic",
			"Devin",
			"Woffle", // <@!185980979427540992> (Chipbeam#2268)
			"Cameron", // <@!340401981711712258> (CammyWammy#8634)
		};
		private const int MechanicVanillaNames = 24;
		private static readonly string[] MechanicNames =
		{
			"Lilly",
			"Daawn", // <@!206162323541458944> (Daawnily#3859)
			"Robin", // <@!654737510030639112> (Altzeus#8687)
		};
		private const int MerchantVanillaNames = 23;
		private static readonly string[] MerchantNames = null;
		private const int NurseVanillaNames = 24;
		private static readonly string[] NurseNames =
		{
			"Farsni",
		};
		private const int PainterVanillaNames = 18;
		private static readonly string[] PainterNames = null;
		private const int PartyGirlVanillaNames = 17;
		private static readonly string[] PartyGirlNames = null;
		private const int PirateVanillaNames = 11;
		private static readonly string[] PirateNames =
		{
			"Tyler Van Hook",
		};
		private const int SteampunkerVanillaNames = 20;
		private static readonly string[] SteampunkerNames =
		{
			"Vorbis",
			"Angel",
		};
		private const int StylistVanillaNames = 20;
		private static readonly string[] StylistNames =
		{
			"Amber", // <@!114677116473180169> (Mishiro Usui#1295)
		};
		private const int TavernkeepVanillaNames = 16;
		private static readonly string[] TavernkeepNames = null;
		private const int TaxCollectorVanillaNames = 19;
		private static readonly string[] TaxCollectorNames =
		{
			"Emmett",
		};
		private const int TruffleVanillaNames = 12;
		private static readonly string[] TruffleNames = null;
		private const int WitchDoctorVanillaNames = 10;
		private static readonly string[] WitchDoctorNames =
		{
			"Sok'ar",
			"Toxin", // <@!348174404984766465> (Toxin#9598)
		};
		private const int WizardVanillaNames = 22;
		private static readonly string[] WizardNames =
		{
			"Mage One-Trick",
			"Inorim, son of Ivukey",
			"Jensen",
			"Merasmus", // <@!288066987819663360> (Spider pee pee#3328)
			"Habolo", // <@!163028025494077441> (ChristmasGoat#7810)
		};
		#endregion


		#region Town NPC Names
		public static void ResetTownNPCNameBools(NPC npc, Mod mod)
		{
			void ResetName(int npcID, ref bool nameBool)
			{
				if (NPC.FindFirstNPC(npcID) == -1)
					nameBool = false;
			}

			ResetName(NPCID.Angler, ref CalamityWorld.anglerName);
			ResetName(NPCID.ArmsDealer, ref CalamityWorld.armsDealerName);
			ResetName(NPCID.Clothier, ref CalamityWorld.clothierName);
			ResetName(NPCID.Cyborg, ref CalamityWorld.cyborgName);
			ResetName(NPCID.Demolitionist, ref CalamityWorld.demolitionistName);
			ResetName(NPCID.Dryad, ref CalamityWorld.dryadName);
			ResetName(NPCID.DyeTrader, ref CalamityWorld.dyeTraderName);
			ResetName(NPCID.GoblinTinkerer, ref CalamityWorld.goblinTinkererName);
			ResetName(NPCID.Guide, ref CalamityWorld.guideName);
			ResetName(NPCID.Mechanic, ref CalamityWorld.mechanicName);
			ResetName(NPCID.Merchant, ref CalamityWorld.merchantName);
			ResetName(NPCID.Nurse, ref CalamityWorld.nurseName);
			ResetName(NPCID.Painter, ref CalamityWorld.painterName);
			ResetName(NPCID.PartyGirl, ref CalamityWorld.partyGirlName);
			ResetName(NPCID.Pirate, ref CalamityWorld.pirateName);
			ResetName(NPCID.Steampunker, ref CalamityWorld.steampunkerName);
			ResetName(NPCID.Stylist, ref CalamityWorld.stylistName);
			ResetName(NPCID.DD2Bartender, ref CalamityWorld.tavernkeepName);
			ResetName(NPCID.TaxCollector, ref CalamityWorld.taxCollectorName);
			ResetName(NPCID.Truffle, ref CalamityWorld.truffleName);
			ResetName(NPCID.WitchDoctor, ref CalamityWorld.witchDoctorName);
			ResetName(NPCID.Wizard, ref CalamityWorld.wizardName);
		}

		// Annoyingly, because npc.GivenName is a property, it can't be passed as a ref parameter.
		private static string ChooseName(ref bool alreadySet, string currentName, int numVanillaNames, string[] patreonNames)
		{
			if (alreadySet || patreonNames is null || patreonNames.Length == 0)
			{
				alreadySet = true;
				return currentName;
			} 

			alreadySet = true;
			int index = Main.rand.Next(numVanillaNames + patreonNames.Length);

			// If the roll isn't low enough, then a "vanilla name" was picked, meaning we change nothing.
			if (index >= patreonNames.Length)
				return currentName;

			// Change the name to be a randomly selected Patreon name if the roll is low enough.
			return patreonNames[index];
		}

		public static void SetPatreonTownNPCName(NPC npc, Mod mod)
		{
			if (npc.Calamity().setNewName)
			{
				npc.Calamity().setNewName = false;
				switch (npc.type)
				{
					case NPCID.Angler:
						npc.GivenName = ChooseName(ref CalamityWorld.anglerName, npc.GivenName, AnglerVanillaNames, AnglerNames);
						break;
					case NPCID.ArmsDealer:
						npc.GivenName = ChooseName(ref CalamityWorld.armsDealerName, npc.GivenName, ArmsDealerVanillaNames, ArmsDealerNames);
						break;
					case NPCID.Clothier:
						npc.GivenName = ChooseName(ref CalamityWorld.clothierName, npc.GivenName, ClothierVanillaNames, ClothierNames);
						break;
					case NPCID.Cyborg:
						npc.GivenName = ChooseName(ref CalamityWorld.cyborgName, npc.GivenName, CyborgVanillaNames, CyborgNames);
						break;
					case NPCID.Demolitionist:
						npc.GivenName = ChooseName(ref CalamityWorld.demolitionistName, npc.GivenName, DemolitionistVanillaNames, DemolitionistNames);
						break;
					case NPCID.Dryad:
						npc.GivenName = ChooseName(ref CalamityWorld.dryadName, npc.GivenName, DryadVanillaNames, DryadNames);
						break;
					case NPCID.DyeTrader:
						npc.GivenName = ChooseName(ref CalamityWorld.dyeTraderName, npc.GivenName, DyeTraderVanillaNames, DyeTraderNames);
						break;
					case NPCID.GoblinTinkerer:
						npc.GivenName = ChooseName(ref CalamityWorld.goblinTinkererName, npc.GivenName, GoblinTinkererVanillaNames, GoblinTinkererNames);
						break;
					case NPCID.Guide:
						npc.GivenName = ChooseName(ref CalamityWorld.guideName, npc.GivenName, GuideVanillaNames, GuideNames);
						break;
					case NPCID.Mechanic:
						npc.GivenName = ChooseName(ref CalamityWorld.mechanicName, npc.GivenName, MechanicVanillaNames, MechanicNames);
						break;
					case NPCID.Merchant:
						npc.GivenName = ChooseName(ref CalamityWorld.merchantName, npc.GivenName, MerchantVanillaNames, MerchantNames);
						break;
					case NPCID.Nurse:
						npc.GivenName = ChooseName(ref CalamityWorld.nurseName, npc.GivenName, NurseVanillaNames, NurseNames);
						break;
					case NPCID.Painter:
						npc.GivenName = ChooseName(ref CalamityWorld.painterName, npc.GivenName, PainterVanillaNames, PainterNames);
						break;
					case NPCID.PartyGirl:
						npc.GivenName = ChooseName(ref CalamityWorld.partyGirlName, npc.GivenName, PartyGirlVanillaNames, PartyGirlNames);
						break;
					case NPCID.Pirate:
						npc.GivenName = ChooseName(ref CalamityWorld.pirateName, npc.GivenName, PirateVanillaNames, PirateNames);
						break;
					case NPCID.Steampunker:
						npc.GivenName = ChooseName(ref CalamityWorld.steampunkerName, npc.GivenName, SteampunkerVanillaNames, SteampunkerNames);
						break;
					case NPCID.Stylist:
						npc.GivenName = ChooseName(ref CalamityWorld.stylistName, npc.GivenName, StylistVanillaNames, StylistNames);
						break;
					case NPCID.DD2Bartender: // Tavernkeep
						npc.GivenName = ChooseName(ref CalamityWorld.tavernkeepName, npc.GivenName, TavernkeepVanillaNames, TavernkeepNames);
						break;
					case NPCID.TaxCollector:
						npc.GivenName = ChooseName(ref CalamityWorld.taxCollectorName, npc.GivenName, TaxCollectorVanillaNames, TaxCollectorNames);
						break;
					case NPCID.Truffle:
						npc.GivenName = ChooseName(ref CalamityWorld.truffleName, npc.GivenName, TruffleVanillaNames, TruffleNames);
						break;
					case NPCID.WitchDoctor:
						npc.GivenName = ChooseName(ref CalamityWorld.witchDoctorName, npc.GivenName, WitchDoctorVanillaNames, WitchDoctorNames);
						break;
					case NPCID.Wizard:
						npc.GivenName = ChooseName(ref CalamityWorld.wizardName, npc.GivenName, WizardVanillaNames, WizardNames);
						break;
					default:
						break;
				}
			}
		}
		#endregion

		#region NPC New Shop Alert
		public static void TownNPCAlertSystem(NPC npc, Mod mod, SpriteBatch spriteBatch)
		{
			if (CalamityConfig.Instance.ShopNewAlert && npc.townNPC)
			{
				if (npc.type == NPCType<DILF>() && Main.LocalPlayer.Calamity().newPermafrostInventory)
				{
					DrawNewInventoryAlert(npc);
				}
				else if (npc.type == NPCType<FAP>() && Main.LocalPlayer.Calamity().newCirrusInventory)
				{
					DrawNewInventoryAlert(npc);
				}
				else if (npc.type == NPCType<SEAHOE>() && Main.LocalPlayer.Calamity().newAmidiasInventory)
				{
					DrawNewInventoryAlert(npc);
				}
				else if (npc.type == NPCType<THIEF>() && Main.LocalPlayer.Calamity().newBanditInventory)
				{
					DrawNewInventoryAlert(npc);
				}
				else
				{
					switch (npc.type)
					{
						case NPCID.Merchant:
							if (Main.LocalPlayer.Calamity().newMerchantInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.Painter:
							if (Main.LocalPlayer.Calamity().newPainterInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.DyeTrader:
							if (Main.LocalPlayer.Calamity().newDyeTraderInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.PartyGirl:
							if (Main.LocalPlayer.Calamity().newPartyGirlInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.Stylist:
							if (Main.LocalPlayer.Calamity().newStylistInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.Demolitionist:
							if (Main.LocalPlayer.Calamity().newDemolitionistInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.Dryad:
							if (Main.LocalPlayer.Calamity().newDryadInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.DD2Bartender:
							if (Main.LocalPlayer.Calamity().newTavernkeepInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.ArmsDealer:
							if (Main.LocalPlayer.Calamity().newArmsDealerInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.GoblinTinkerer:
							if (Main.LocalPlayer.Calamity().newGoblinTinkererInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.WitchDoctor:
							if (Main.LocalPlayer.Calamity().newWitchDoctorInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.Clothier:
							if (Main.LocalPlayer.Calamity().newClothierInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.Mechanic:
							if (Main.LocalPlayer.Calamity().newMechanicInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.Pirate:
							if (Main.LocalPlayer.Calamity().newPirateInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.Truffle:
							if (Main.LocalPlayer.Calamity().newTruffleInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.Wizard:
							if (Main.LocalPlayer.Calamity().newWizardInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.Steampunker:
							if (Main.LocalPlayer.Calamity().newSteampunkerInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.Cyborg:
							if (Main.LocalPlayer.Calamity().newCyborgInventory)
								DrawNewInventoryAlert(npc);
							break;
						case NPCID.SkeletonMerchant:
							if (Main.LocalPlayer.Calamity().newSkeletonMerchantInventory)
								DrawNewInventoryAlert(npc);
							break;
						default:
							break;
					}
				}

				void DrawNewInventoryAlert(NPC npc2)
				{
					// The position where the display is drawn
					Vector2 drawPos = npc2.Center - Main.screenPosition;

					// The height of a single frame of the npc
					float npcHeight = (float)(TextureAssets.Npc[npc2.type].Value.Height / Main.npcFrameCount[npc2.type] / 2) * npc2.scale;

					// Offset the debuff display based on the npc's graphical offset, and 16 units, to create some space between the sprite and the display
					float drawPosY = npcHeight + npc.gfxOffY + 36f;

					// Texture animation variables
					Texture2D texture = Request<Texture2D>("CalRD/ExtraTextures/UI/NPCAlertDisplay").Value;
					npc.Calamity().shopAlertAnimTimer++;
					if (npc.Calamity().shopAlertAnimTimer >= 6)
					{
						npc.Calamity().shopAlertAnimTimer = 0;

						npc.Calamity().shopAlertAnimFrame++;
						if (npc.Calamity().shopAlertAnimFrame > 4)
							npc.Calamity().shopAlertAnimFrame = 0;
					}
					int frameHeight = texture.Height / 5;
					Rectangle animRect = new Rectangle(0, frameHeight * npc.Calamity().shopAlertAnimFrame, texture.Width, frameHeight);

					spriteBatch.Draw(texture, drawPos - new Vector2(5f, drawPosY), animRect, Color.White, 0f, default, 1f, SpriteEffects.None, 0f);
				}
			}
		}

		public static void DisableAlert(NPC npc, Mod mod)
		{
			if (npc.townNPC)
			{
				switch (npc.type)
				{
					case NPCID.Merchant:
						Main.LocalPlayer.Calamity().newMerchantInventory = false;
						break;
					case NPCID.Painter:
						Main.LocalPlayer.Calamity().newPainterInventory = false;
						break;
					case NPCID.DyeTrader:
						Main.LocalPlayer.Calamity().newDyeTraderInventory = false;
						break;
					case NPCID.PartyGirl:
						Main.LocalPlayer.Calamity().newPartyGirlInventory = false;
						break;
					case NPCID.Stylist:
						Main.LocalPlayer.Calamity().newStylistInventory = false;
						break;
					case NPCID.Demolitionist:
						Main.LocalPlayer.Calamity().newDemolitionistInventory = false;
						break;
					case NPCID.Dryad:
						Main.LocalPlayer.Calamity().newDryadInventory = false;
						break;
					case NPCID.DD2Bartender:
						Main.LocalPlayer.Calamity().newTavernkeepInventory = false;
						break;
					case NPCID.ArmsDealer:
						Main.LocalPlayer.Calamity().newArmsDealerInventory = false;
						break;
					case NPCID.GoblinTinkerer:
						Main.LocalPlayer.Calamity().newGoblinTinkererInventory = false;
						break;
					case NPCID.WitchDoctor:
						Main.LocalPlayer.Calamity().newWitchDoctorInventory = false;
						break;
					case NPCID.Clothier:
						Main.LocalPlayer.Calamity().newClothierInventory = false;
						break;
					case NPCID.Mechanic:
						Main.LocalPlayer.Calamity().newMechanicInventory = false;
						break;
					case NPCID.Pirate:
						Main.LocalPlayer.Calamity().newPirateInventory = false;
						break;
					case NPCID.Truffle:
						Main.LocalPlayer.Calamity().newTruffleInventory = false;
						break;
					case NPCID.Wizard:
						Main.LocalPlayer.Calamity().newWizardInventory = false;
						break;
					case NPCID.Steampunker:
						Main.LocalPlayer.Calamity().newSteampunkerInventory = false;
						break;
					case NPCID.Cyborg:
						Main.LocalPlayer.Calamity().newCyborgInventory = false;
						break;
					case NPCID.SkeletonMerchant:
						Main.LocalPlayer.Calamity().newSkeletonMerchantInventory = false;
						break;
					default:
						break;
				}
			}
		}

		public static void SetNewShopVariable(int[] types, bool alreadySet)
		{
			if (!alreadySet)
			{
				for (int i = 0; i < types.Length; i++)
				{
					if (types[i] == NPCType<DILF>())
					{
						Main.LocalPlayer.Calamity().newPermafrostInventory = true;
					}
					else if (types[i] == NPCType<FAP>())
					{
						Main.LocalPlayer.Calamity().newCirrusInventory = true;
					}
					else if (types[i] == NPCType<SEAHOE>())
					{
						Main.LocalPlayer.Calamity().newAmidiasInventory = true;
					}
					else if (types[i] == NPCType<THIEF>())
					{
						Main.LocalPlayer.Calamity().newBanditInventory = true;
					}
					else
					{
						switch (types[i])
						{
							case NPCID.Merchant:
								Main.LocalPlayer.Calamity().newMerchantInventory = true;
								break;
							case NPCID.Painter:
								Main.LocalPlayer.Calamity().newPainterInventory = true;
								break;
							case NPCID.DyeTrader:
								Main.LocalPlayer.Calamity().newDyeTraderInventory = true;
								break;
							case NPCID.PartyGirl:
								Main.LocalPlayer.Calamity().newPartyGirlInventory = true;
								break;
							case NPCID.Stylist:
								Main.LocalPlayer.Calamity().newStylistInventory = true;
								break;
							case NPCID.Demolitionist:
								Main.LocalPlayer.Calamity().newDemolitionistInventory = true;
								break;
							case NPCID.Dryad:
								Main.LocalPlayer.Calamity().newDryadInventory = true;
								break;
							case NPCID.DD2Bartender:
								Main.LocalPlayer.Calamity().newTavernkeepInventory = true;
								break;
							case NPCID.ArmsDealer:
								Main.LocalPlayer.Calamity().newArmsDealerInventory = true;
								break;
							case NPCID.GoblinTinkerer:
								Main.LocalPlayer.Calamity().newGoblinTinkererInventory = true;
								break;
							case NPCID.WitchDoctor:
								Main.LocalPlayer.Calamity().newWitchDoctorInventory = true;
								break;
							case NPCID.Clothier:
								Main.LocalPlayer.Calamity().newClothierInventory = true;
								break;
							case NPCID.Mechanic:
								Main.LocalPlayer.Calamity().newMechanicInventory = true;
								break;
							case NPCID.Pirate:
								Main.LocalPlayer.Calamity().newPirateInventory = true;
								break;
							case NPCID.Truffle:
								Main.LocalPlayer.Calamity().newTruffleInventory = true;
								break;
							case NPCID.Wizard:
								Main.LocalPlayer.Calamity().newWizardInventory = true;
								break;
							case NPCID.Steampunker:
								Main.LocalPlayer.Calamity().newSteampunkerInventory = true;
								break;
							case NPCID.Cyborg:
								Main.LocalPlayer.Calamity().newCyborgInventory = true;
								break;
							case NPCID.SkeletonMerchant:
								Main.LocalPlayer.Calamity().newSkeletonMerchantInventory = true;
								break;
							default:
								break;
						}
					}
				}
			}
		}
		#endregion

		#region NPC Chat
		public static void NewNPCQuotes(NPC npc, Mod mod, ref string chat)
		{
			int fapsol = NPC.FindFirstNPC(NPCType<FAP>());
			int permadong = NPC.FindFirstNPC(NPCType<DILF>());
			int seahorse = NPC.FindFirstNPC(NPCType<SEAHOE>());
			int thief = NPC.FindFirstNPC(NPCType<THIEF>());
			int angelstatue = NPC.FindFirstNPC(NPCID.Merchant);

			switch (npc.type)
			{
				case NPCID.Guide:
					if (Main.hardMode)
					{
						if (Main.rand.NextBool(20))
						{
							chat = CalamityUtils.GetTextValue("Vanilla.GuideChat.Hardmode1");
						}

						if (Main.rand.NextBool(20))
						{
							chat = CalamityUtils.GetTextValue("Vanilla.GuideChat.Hardmode2");
						}
					}

					if (Main.rand.NextBool(20) && NPC.downedMoonlord)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.GuideChat.MoonLordDefeated");
					}

					if (CalamityWorld.downedProvidence)
					{
						if (Main.rand.NextBool(20))
						{
							chat = CalamityUtils.GetTextValue("Vanilla.GuideChat.ProvidenceDefeated1");
						}

						if (Main.rand.NextBool(20))
						{
							chat = CalamityUtils.GetTextValue("Vanilla.GuideChat.ProvidenceDefeated2");
						}
					}

					break;
				case NPCID.Truffle:
					if (Main.rand.NextBool(8))
					{
						chat = CalamityUtils.GetTextValue("Vanilla.TruffleChat.Normal");
					}

					if (NPC.AnyNPCs(NPCType<FAP>()))
					{
						chat = CalamityUtils.GetText("Vanilla.TruffleChat.DrunkPrincess").Format(Main.npc[fapsol].GivenName);
					}

					break;

				case NPCID.Angler:
					if (Main.rand.NextBool(5) && NPC.AnyNPCs(NPCType<SEAHOE>()))
					{
						chat = CalamityUtils.GetText("Vanilla.AnglerChat.SeaKing").Format(Main.npc[seahorse].GivenName);
					}

					break;

				case NPCID.TravellingMerchant:
					if (Main.rand.NextBool(5) && NPC.AnyNPCs(NPCType<FAP>()) && NPC.AnyNPCs(NPCID.Merchant))
					{
						chat = CalamityUtils.GetText("Vanilla.TravellingMerchantChat.DrunkPrincessMerchant").Format(Main.npc[fapsol].GivenName, Main.npc[angelstatue].GivenName);
					}

					break;

				case NPCID.SkeletonMerchant:
					if (Main.rand.NextBool(5))
					{
						chat = CalamityUtils.GetTextValue("Vanilla.SkeletonMerchantChat.Normal");
					}

					break;

				case NPCID.WitchDoctor:
					if (Main.rand.NextBool(8) && Main.LocalPlayer.ZoneJungle)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.WitchDoctorChat.Jungle");
					}

					if (Main.rand.NextBool(8) && Main.LocalPlayer.ZoneJungle &&
						Main.hardMode && !NPC.downedPlantBoss)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.WitchDoctorChat.PrePlantera");
					}

					if (Main.rand.NextBool(8) && Main.bloodMoon)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.WitchDoctorChat.BloodMoon");
					}

					break;

				case NPCID.PartyGirl:
					if (Main.rand.NextBool(10) && fapsol != -1)
					{
						chat = CalamityUtils.GetText("Vanilla.PartyGirlChat.DrunkPrincess").Format(Main.npc[fapsol].GivenName);
					}

					if (Main.eclipse)
					{
						if (Main.rand.NextBool(5))
						{
							chat = CalamityUtils.GetTextValue("Vanilla.PartyGirlChat.Eclipse1");
						}

						if (Main.rand.NextBool(5))
						{
							chat = CalamityUtils.GetTextValue("Vanilla.PartyGirlChat.Eclipse2");
						}
					}

					break;

				case NPCID.Painter:
					if (Main.rand.NextBool(4) && Main.LocalPlayer.ZoneCorrupt)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.Corruption");
					}
					if (Main.rand.NextBool(4) && Main.LocalPlayer.ZoneCrimson)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.Crimson");
					}
					if (Main.rand.NextBool(4) && Main.LocalPlayer.ZoneSnow)
					{
						if (NPC.AnyNPCs(NPCType<DILF>()) && Main.rand.NextBool(2))
						{
							chat = CalamityUtils.GetText("Vanilla.PainterChat.Permafrost").Format(Main.npc[permadong].GivenName);
						}
						else
						{
							chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.Tundra");
						}
					}
					if (Main.rand.NextBool(4) && Main.LocalPlayer.ZoneDesert)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.Desert");
					}
					if (Main.rand.NextBool(4) && Main.LocalPlayer.ZoneHallow)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.Hallow");
					}
					if (Main.rand.NextBool(4) && Main.LocalPlayer.ZoneSkyHeight)
					{
						//chat = "I can't breathe."
						chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.Space");
					}
					if (Main.rand.NextBool(4) && Main.LocalPlayer.ZoneJungle)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.Jungle");
					}
					if (Main.rand.NextBool(4) && Main.LocalPlayer.Calamity().ZoneAstral)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.Astral");
					}
					if (Main.rand.NextBool(4) && Main.LocalPlayer.ZoneUnderworldHeight)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.Underworld1");
					}
					if (Main.rand.NextBool(4) && Main.LocalPlayer.ZoneUnderworldHeight)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.Underworld2");
					}
					if (Main.rand.NextBool(4) && Main.LocalPlayer.Calamity().ZoneCalamity)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.Crags");
					}
					if (Main.rand.NextBool(4) && Main.LocalPlayer.Calamity().ZoneSulphur)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.SulphurSea");
					}
					if (Main.rand.NextBool(4) && Main.LocalPlayer.Calamity().ZoneAbyss)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PainterChat.Abyss");
					}
					break;

				case NPCID.Wizard:
					if (Main.rand.NextBool(10) && permadong != -1)
					{
						chat = CalamityUtils.GetText("Vanilla.WizardChat.Permafrost").Format(Main.npc[permadong].GivenName);
					}

					if (Main.rand.NextBool(10) && Main.hardMode)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.WizardChat.Hardmode");
					}

					break;

				case NPCID.Dryad:
					if (Main.rand.NextBool(5) && CalamityWorld.buffedEclipse && Main.eclipse)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.DryadChat.DarksunEclipse");
					}

					if (Main.rand.NextBool(5) && Main.hardMode)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.DryadChat.Hardmode");
					}

					if (Main.rand.NextBool(5) && NPC.AnyNPCs(NPCType<FAP>()) && Main.LocalPlayer.ZoneGlowshroom)
					{
						chat = CalamityUtils.GetText("Vanilla.DryadChat.DrunkPrincessShroom").Format(Main.npc[fapsol].GivenName);
					}

					if (Main.rand.NextBool(5) && Main.LocalPlayer.Calamity().ZoneSulphur)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.DryadChat.SulphurSea");
					}

					if (Main.rand.NextBool(5) && Main.LocalPlayer.ZoneGlowshroom)
					{
						//high iq drugs iirc
						chat = CalamityUtils.GetTextValue("Vanilla.DryadChat.Mushroom");
					}

					break;

				case NPCID.Stylist:
					string worldEvil = Language.GetTextValue("LegacyMisc." + (WorldGen.crimson ? 102 : 101));
					if (Main.rand.NextBool(15) && Main.hardMode)
					{
						chat = CalamityUtils.GetText("Vanilla.StylistChat.Hardmode").Format(worldEvil);
					}

					if (fapsol != -1)
					{
						if (Main.rand.NextBool(15))
						{
							chat = CalamityUtils.GetText("Vanilla.StylistChat.DrunkPrincess1").Format(Main.npc[fapsol].GivenName);
						}

						if (Main.rand.NextBool(15))
						{
							chat = CalamityUtils.GetText("Vanilla.StylistChat.DrunkPrincess2").Format(Main.npc[fapsol].GivenName);
						}
					}
					if ((npc.GivenName == "Amber" ? Main.rand.NextBool(10) : Main.rand.NextBool(15)) && Main.LocalPlayer.Calamity().pArtifact)
					{
						if (Main.LocalPlayer.Calamity().profanedCrystalBuffs)
						{
							chat = Main.rand.NextBool(2) ? CalamityUtils.GetTextValue("Vanilla.StylistChat.ProfanedSoulCrystal1") : CalamityUtils.GetTextValue("Vanilla.StylistChat.ProfanedSoulCrystal2");
						}
						else if (Main.LocalPlayer.Calamity().gDefense && Main.LocalPlayer.Calamity().gOffense)
						{
							chat = CalamityUtils.GetTextValue("Vanilla.StylistChat.ProfanedDonuts"); 
						}
					}
					break;

				case NPCID.GoblinTinkerer:
					if (Main.rand.NextBool(3) && thief != -1 && CalamityWorld.Reforges >= 1)
					{
						chat = CalamityUtils.GetText("Vanilla.GoblinTinkererChat.Bandit").Format(Main.npc[thief].GivenName);
					}
					if (Main.rand.NextBool(10) && NPC.downedMoonlord)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.GoblinTinkererChat.MoonLordDefeated");
					}

					break;

				case NPCID.ArmsDealer:
					if (Main.rand.NextBool(5) && Main.eclipse)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.ArmsDealerChat.Eclipse");
					}

					if (Main.rand.NextBool(10) && CalamityWorld.downedDoG)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.ArmsDealerChat.DoGDefeated");
					}

					break;

				case NPCID.Merchant:
					if (Main.rand.NextBool(5) && NPC.downedMoonlord)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.MerchantChat.MoonLordDefeated");
					}

					if (Main.rand.NextBool(5) && Main.eclipse)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.MerchantChat.Eclipse");
					}

					if (Main.rand.NextBool(5) && Main.raining && Main.LocalPlayer.Calamity().ZoneSulphur)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.MerchantChat.AcidRain");
					}

					if (Main.rand.NextBool(7) && thief != -1)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.MerchantChat.Bandit");
					}

					if (Main.rand.NextBool(7) && CalamityWorld.death)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.MerchantChat.DeathMode");
					}

					break;

				case NPCID.Mechanic:
					if (Main.rand.NextBool(5) && NPC.downedMoonlord)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.MechanicChat.MoonLordDefeated");
					}

					if (Main.rand.NextBool(5) && Main.eclipse)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.MechanicChat.Eclipse");
					}

					if (Main.rand.NextBool(5) && fapsol != -1)
					{
						chat = CalamityUtils.GetText("Vanilla.MechanicChat.DrunkPrincess").Format(Main.npc[fapsol].GivenName);
					}

					if (Main.rand.NextBool(5) && CalamityWorld.rainingAcid)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.MechanicChat.AcidRain");
					}

					break;

				case NPCID.DD2Bartender:
					if (Main.rand.NextBool(5) && !Main.dayTime && Main.moonPhase == 0)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.TavernkeepChat.FullMoon");
					}

					if (Main.rand.NextBool(10) && fapsol != -1)
					{
						chat = CalamityUtils.GetText("Vanilla.TavernkeepChat.DrunkPrincess").Format(Main.npc[fapsol].GivenName);
					}

					break;

				case NPCID.Pirate:
					if (Main.rand.NextBool(5) && !CalamityWorld.downedLeviathan)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PirateChat.PreLeviathan");
					}

					if (Main.rand.NextBool(5) && CalamityWorld.downedAquaticScourge)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PirateChat.AquaticScourgeDefeated");
					}

					if (Main.rand.NextBool(5) && NPC.AnyNPCs(NPCType<SEAHOE>()))
					{
						chat = CalamityUtils.GetText("Vanilla.PirateChat.SeaKing").Format(Main.npc[seahorse].GivenName);
					}

					if (Main.rand.NextBool(5) && NPC.AnyNPCs(NPCType<FAP>()))
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PirateChat.DrunkPrincess");
					}

					if (Main.rand.NextBool(5) && Main.LocalPlayer.Center.ToTileCoordinates().X < 380 && !Main.LocalPlayer.Calamity().ZoneSulphur)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PirateChat.Abyss");
					}

					if (Main.rand.NextBool(5) && Main.LocalPlayer.Calamity().ZoneSulphur)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PirateChat.SulphurSea1");
					}

					if (Main.rand.NextBool(5) && Main.LocalPlayer.Calamity().ZoneSulphur)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.PirateChat.SulphurSea2");
					}
					break;

				case NPCID.Cyborg:
					if (Main.rand.NextBool(10) && Main.raining)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.CyborgChat.Rain");
					}

					if (NPC.downedMoonlord)
					{
						if (Main.rand.NextBool(10))
						{
							chat = CalamityUtils.GetTextValue("Vanilla.CyborgChat.MoonLordDefeated1");
						}

						if (Main.rand.NextBool(10))
						{
							chat = CalamityUtils.GetTextValue("Vanilla.CyborgChat.MoonLordDefeated2");
						}
					}

					if (Main.rand.NextBool(10) && !CalamityWorld.downedPlaguebringer && NPC.downedGolemBoss)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.CyborgChat.MentionPlague");
					}

					break;

				case NPCID.Clothier:
					if (NPC.downedMoonlord)
					{
						if (Main.rand.NextBool(10))
						{
							chat = CalamityUtils.GetTextValue("Vanilla.ClothierChat.MoonLordDefeated1");
						}

						if (Main.rand.NextBool(10))
						{
							chat = CalamityUtils.GetTextValue("Vanilla.ClothierChat.MoonLordDefeated2");
						}

						if (Main.rand.NextBool(10))
						{
							chat = CalamityUtils.GetTextValue("Vanilla.ClothierChat.MoonLordDefeated3");
						}
					}

					if (Main.rand.NextBool(10) && CalamityWorld.downedPolterghast)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.ClothierChat.PolterghastDefeated");
					}

					if (Main.rand.NextBool(5) && NPC.AnyNPCs(NPCID.MoonLordCore))
					{
						chat = CalamityUtils.GetTextValue("Vanilla.ClothierChat.MoonLordPresent");
					}

					break;

				case NPCID.Steampunker:
					bool hasPortalGun = false;
					for (int k = 0; k < Main.maxPlayers; k++)
					{
						Player player = Main.player[k];
						if (player.active && player.HasItem(ItemID.PortalGun))
						{
							hasPortalGun = true;
						}
					}

					if (Main.rand.NextBool(5) && hasPortalGun)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.SteampunkerChat.PortalGun");
					}

					if (Main.rand.NextBool(5) && NPC.downedMoonlord)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.SteampunkerChat.MoonLordDefeated");
					}

					if (Main.rand.NextBool(5) && Main.LocalPlayer.Calamity().ZoneAstral)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.SteampunkerChat.Astral");
					}

					if (Main.rand.NextBool(5) && Main.LocalPlayer.ZoneHallow)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.SteampunkerChat.Hallow");
					}

					break;

				case NPCID.DyeTrader:
					if (Main.rand.NextBool(5))
					{
						chat = CalamityUtils.GetTextValue("Vanilla.DyeTraderChat.Normal");
					}

					if (Main.rand.NextBool(5) && permadong != -1)
					{
						chat = CalamityUtils.GetText("Vanilla.DyeTraderChat.Permafrost").Format(Main.npc[permadong].GivenName);
					}

					break;

				case NPCID.TaxCollector:
					int platinumCoins = 0;
					for (int k = 0; k < Main.maxPlayers; k++)
					{
						Player player = Main.player[k];
						if (player.active)
						{
							for (int j = 0; j < player.inventory.Length; j++)
							{
								if (player.inventory[j].type == ItemID.PlatinumCoin)
								{
									platinumCoins += player.inventory[j].stack;
								}
							}
						}
					}

					if (Main.rand.NextBool(5) && platinumCoins >= 100)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.TaxCollectorChat.Has100Plat");
					}

					if (Main.rand.NextBool(5) && platinumCoins >= 500)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.TaxCollectorChat.Has500Plat");
					}

					if (Main.rand.NextBool(5) && !CalamityWorld.downedBrimstoneElemental)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.TaxCollectorChat.PreBrimmy");
					}

					if (Main.rand.NextBool(10) && CalamityWorld.downedDoG)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.TaxCollectorChat.DoGDefeated");
					}

					if (Main.rand.NextBool(10) && CalamityUtils.InventoryHas(Main.LocalPlayer, ItemType<SlickCane>()))
					{
						chat = CalamityUtils.GetTextValue("Vanilla.TaxCollectorChat.SlickCane");
					}

					break;

				case NPCID.Demolitionist:
					if (Main.rand.NextBool(5) && CalamityWorld.downedDoG)
					{
						chat = CalamityUtils.GetTextValue("Vanilla.DemolitionistChat.DoGDefeated");
					}

					break;

				default:
					break;
			}
		}
		#endregion

		#region NPC Stat Changes
		public static void MakeTownNPCsTakeMoreDamage(NPC npc, Projectile projectile, Mod mod, ref int damage)
		{
			if (npc.townNPC && projectile.hostile)
				damage *= 2;
		}

		public static void NPCStatBuffs(Mod mod, ref float damageMult, ref int defense)
		{
			if (NPC.downedMoonlord)
			{
				damageMult += 0.6f;
				defense += 20;
			}
			if (CalamityWorld.downedProvidence)
			{
				damageMult += 0.2f;
				defense += 12;
			}
			if (CalamityWorld.downedPolterghast)
			{
				damageMult += 0.2f;
				defense += 12;
			}
			if (CalamityWorld.downedDoG)
			{
				damageMult += 0.2f;
				defense += 12;
			}
			if (CalamityWorld.downedYharon)
			{
				damageMult += 0.2f;
				defense += 12;
			}
			if (CalamityWorld.downedSCal)
			{
				damageMult += 0.6f;
				defense += 20;
			}
		}
		#endregion

		#region Shop Stuff
		public static void ShopSetup(NPCShop shop)
		{
			int type = shop.NpcType;
			if (type == NPCID.Merchant)
			{
				shop.Add(ItemID.Flare, hasFlareGunUpgrade)
				.Add(ItemID.BlueFlare, hasFlareGunUpgrade)
				.Add(ItemID.ApprenticeBait, Condition.DownedEyeOfCthulhu)
				.Add(ItemID.JourneymanBait, Condition.DownedSkeletron)
				.Add(WorldGen.crimson ? ItemID.Vilethorn : ItemID.CrimsonRod, downedEvilorSmashedOrb)
				.Add(WorldGen.crimson ? ItemID.BallOHurt : ItemID.TheRottedFork, downedEvilorSmashedOrb)
				.Add(ItemID.MasterBait, Condition.DownedPlantera)
				.AddWithCustomValue(ItemID.AngelStatue, Item.buyPrice(0, 5), Condition.NpcIsPresent(NPCType<THIEF>()))
				.Add(ItemID.UltrabrightTorch, deathActive);
			}

			// Because of the defiled condition, the dye trader does not receive an alert icon when hardmode starts.
			if (type == NPCID.DyeTrader)
			{
				shop.AddWithCustomValue(ItemType<DefiledFlameDye>(), Item.buyPrice(0, 10), defiledHM);
			}

			if (type == NPCID.ArmsDealer)
			{
				shop.Add(ItemID.Stake, hasImpaler)
					.Add(WorldGen.crimson ? ItemID.Musket : ItemID.TheUndertaker, downedEvilorSmashedOrb)
				.AddWithCustomValue(ItemID.Boomstick, Item.buyPrice(0, 20, 0, 0), Condition.DownedQueenBee)
				.AddWithCustomValue(ItemID.TacticalShotgun, Item.buyPrice(0, 25), Condition.DownedGolem)
				.AddWithCustomValue(ItemID.SniperRifle, Item.buyPrice(0, 25), Condition.DownedGolem);
			}

			if (type == NPCID.Stylist)
			{
				shop.Add(ItemType<StealthHairDye>(), rogueActive)
				.Add(ItemType<WingTimeHairDye>(), hasWings)
				.Add(ItemType<AdrenalineHairDye>(), rippersEnabled)
				.Add(ItemType<RageHairDye>(), rippersEnabled);
			}

			if (type == NPCID.Cyborg)
			{
				shop.AddWithCustomValue(ItemID.RocketLauncher, Item.buyPrice(0, 25), Condition.DownedGolem)
				.Add(ItemType<LionHeart>(), downedPolterghast);
			}

			if (type == NPCID.Pirate)
			{
				shop.AddWithCustomValue(ItemID.PirateMap, Item.buyPrice(gold: 5));
			}

			if (type == NPCID.Dryad)
			{
				shop.AddWithCustomValue(ItemID.JungleRose, Item.buyPrice(0, 2))
				.AddWithCustomValue(ItemID.NaturesGift, Item.buyPrice(0, 10))
				.AddWithCustomValue(ItemID.SlimeCrown, Item.buyPrice(0, 2), Condition.DownedKingSlime, sellVanillaSummons)
				.AddWithCustomValue(ItemID.SuspiciousLookingEye, Item.buyPrice(0, 3), Condition.DownedEyeOfCthulhu, sellVanillaSummons)
				.AddWithCustomValue(ItemType<DecapoditaSprout>(), Item.buyPrice(0, 4), downedCrabulon)
				.AddWithCustomValue(ItemID.BloodySpine, Item.buyPrice(0, 6), Condition.DownedBrainOfCthulhu, sellVanillaSummons)
				.AddWithCustomValue(ItemID.WormFood, Item.buyPrice(0, 6), Condition.DownedEaterOfWorlds, sellVanillaSummons)
				.Add(WorldGen.crimson ? ItemID.BandofStarpower : ItemID.PanicNecklace, downedEvilorSmashedOrb)
				.Add(WorldGen.crimson ? ItemID.WormScarf : ItemID.BrainOfConfusion, Condition.InExpertMode, downedEvil)
				.AddWithCustomValue(ItemType<BloodyWormFood>(), Item.buyPrice(0, 10), downedPerforator)
				.Add(ItemType<RottenBrain>(), downedPerforator, Condition.InExpertMode)
				.AddWithCustomValue(ItemType<Teratoma>(), Item.buyPrice(0, 10), downedHiveMind)
				.Add(ItemType<BloodyWormTooth>(), downedHiveMind, Condition.InExpertMode)
				.AddWithCustomValue(ItemType<OverloadedSludge>(), Item.buyPrice(0, 15), downedSlimeGod)
				.Add(ItemType<RomajedaOrchid>());
			}

			if (type == NPCID.GoblinTinkerer)
			{
				shop.AddWithCustomValue(ItemType<MeleeLevelMeter>(), Item.buyPrice(0, 5))
				.AddWithCustomValue(ItemType<RangedLevelMeter>(), Item.buyPrice(0, 5))
				.AddWithCustomValue(ItemType<MagicLevelMeter>(), Item.buyPrice(0, 5))
				.AddWithCustomValue(ItemType<SummonLevelMeter>(), Item.buyPrice(0, 5))
				.AddWithCustomValue(ItemType<RogueLevelMeter>(), Item.buyPrice(0, 5))
				.AddWithCustomValue(ItemType<StatMeter>(), Item.buyPrice(1))
				.AddWithCustomValue(ItemID.GoblinBattleStandard, Item.buyPrice(0, 1));
			}

			if (type == NPCID.Clothier)
			{
				shop.AddWithCustomValue(ItemType<BlueBrickWallUnsafe>(), Item.buyPrice(copper: 10))
				.AddWithCustomValue(ItemType<BlueSlabWallUnsafe>(), Item.buyPrice(copper: 10))
				.AddWithCustomValue(ItemType<BlueTiledWallUnsafe>(), Item.buyPrice(copper: 10))
				.AddWithCustomValue(ItemType<GreenBrickWallUnsafe>(), Item.buyPrice(copper: 10))
				.AddWithCustomValue(ItemType<GreenSlabWallUnsafe>(), Item.buyPrice(copper: 10))
				.AddWithCustomValue(ItemType<GreenTiledWallUnsafe>(), Item.buyPrice(copper: 10))
				.AddWithCustomValue(ItemType<PinkBrickWallUnsafe>(), Item.buyPrice(copper: 10))
				.AddWithCustomValue(ItemType<PinkSlabWallUnsafe>(), Item.buyPrice(copper: 10))
				.AddWithCustomValue(ItemType<PinkTiledWallUnsafe>(), Item.buyPrice(copper: 10))
				.AddWithCustomValue(ItemID.GoldenKey, Item.buyPrice(0, 5), Condition.Hardmode)
				.AddWithCustomValue(ItemID.PumpkinMoonMedallion, Item.buyPrice(0, 25), Condition.DownedPumpking)
				.AddWithCustomValue(ItemID.NaughtyPresent, Item.buyPrice(0, 25), Condition.DownedIceQueen);
			}

			if (type == NPCID.Painter)
			{
				shop.AddWithCustomValue(ItemID.PainterPaintballGun, Item.buyPrice(0, 15));
			}

			if (type == NPCID.Steampunker)
			{
				shop.AddWithCustomValue(ItemID.MechanicalWorm, Item.buyPrice(0, 20), Condition.DownedDestroyer, sellVanillaSummons)
					.AddWithCustomValue(ItemID.MechanicalEye, Item.buyPrice(0, 20), Condition.DownedTwins, sellVanillaSummons)
					.AddWithCustomValue(ItemID.MechanicalSkull, Item.buyPrice(0, 20), Condition.DownedSkeletronPrime, sellVanillaSummons)
					.AddWithCustomValue(ItemType<AstralSolution>(), Item.buyPrice(0, 0, 5));
			}

			if (type == NPCID.Wizard)
			{
				shop.Add(ItemType<HowlsHeart>())
				.AddWithCustomValue(ItemType<CharredIdol>(), Item.buyPrice(0, 20), downedBrimstoneElemental)
				.AddWithCustomValue(ItemType<AstralChunk>(), Item.buyPrice(0, 25), downedAstrageldon)
				.AddWithCustomValue(ItemID.MagicMissile, Item.buyPrice(0, 5))
				.AddWithCustomValue(ItemID.SpectreStaff, Item.buyPrice(0, 25), Condition.DownedGolem)
				.AddWithCustomValue(ItemID.InfernoFork, Item.buyPrice(0, 25), Condition.DownedGolem)
				.AddWithCustomValue(ItemID.ShadowbeamStaff, Item.buyPrice(0, 25), Condition.DownedGolem)
				.AddWithCustomValue(ItemID.CelestialSigil, Item.buyPrice(3), Condition.DownedMoonLord, sellVanillaSummons)
				.AddWithCustomValue(ItemType<ProfanedShard>(), Item.buyPrice(5), downedGuardians);
			}

			if (type == NPCID.WitchDoctor)
			{
				shop.Add(ItemType<SunkenSeaFountain>())
				.Add(ItemType<SulphurousFountainItem>())
				.Add(ItemType<AbyssFountainItem>(), Condition.Hardmode)
				.Add(ItemType<AstralFountainItem>(), Condition.Hardmode)
				.AddWithCustomValue(ItemID.Abeemination, Item.buyPrice(0, 8), sellVanillaSummons)
				.AddWithCustomValue(ItemType<BulbofDoom>(), Item.buyPrice(0, 20), Condition.DownedPlantera, sellVanillaSummons)
				.AddWithCustomValue(ItemID.SolarTablet, Item.buyPrice(0, 25), Condition.DownedGolem)
				.AddWithCustomValue(ItemID.LihzahrdPowerCell, Item.buyPrice(0, 30), Condition.DownedGolem, sellVanillaSummons)
				.AddWithCustomValue(ItemID.ButterflyDust, Item.buyPrice(0, 10), Condition.DownedGolem)
				.AddWithCustomValue(ItemType<AncientMedallion>(), Item.buyPrice(0, 50), downedScavenger)
				.AddWithCustomValue(ItemType<Abomination>(), Item.buyPrice(0, 50), downedPlaguebringer)
				.AddWithCustomValue(ItemType<BirbPheromones>(), Item.buyPrice(5), downedBumble);
			}

			if (type == NPCID.SkeletonMerchant)
			{
				shop.AddWithCustomValue(ItemID.Marrow, Item.buyPrice(0, 36), Condition.Hardmode);
			}
		}

		public static void TravelingMerchantShop(Mod mod, ref int[] shop, ref int nextSlot)
		{
			if (Main.moonPhase == 0)
			{
				shop[nextSlot] = ItemType<FrostBarrier>();
				nextSlot++;
			}
		}

		public static void SetShopItem(ref Chest shop, ref int nextSlot, int itemID, bool condition = true, int? price = null)
		{
			if (condition)
			{
				shop.item[nextSlot].SetDefaults(itemID);
				if (price != null)
				{
					shop.item[nextSlot].shopCustomPrice = price;
				}

				nextSlot++;
			}
		}
		
		public static readonly Condition hasFlareGunUpgrade = ShopHelper.Create("hasFlareGunUpgrade",() => (Main.LocalPlayer.HasItem(ItemType<FirestormCannon>()) || Main.LocalPlayer.HasItem(ItemType<SpectralstormCannon>())) && !Main.LocalPlayer.HasItem(ItemID.FlareGun));
		public static readonly Condition downedEvilorSmashedOrb = ShopHelper.Create("evilDownedorsmashedOrb", () => WorldGen.shadowOrbSmashed || NPC.downedBoss2);
		public static readonly Condition deathActive = ShopHelper.Create("deathActive", () => CalamityWorld.death);
		public static readonly Condition defiledHM = ShopHelper.Create("defiledHM", () => Main.hardMode && CalamityWorld.defiled);
		public static readonly Condition hasImpaler = ShopHelper.Create("hasImpaler", () => Main.LocalPlayer.HasItem(ItemType<Impaler>()));
		public static readonly Condition downedPolterghast = ShopHelper.Create("downedPolter", () => CalamityWorld.downedPolterghast);
		public static readonly Condition rogueActive = ShopHelper.Create("rogueActive", () => Main.LocalPlayer.Calamity().rogueStealthMax > 0f && Main.LocalPlayer.Calamity().wearingRogueArmor);
		public static readonly Condition hasWings = ShopHelper.Create("hasWings", () => Main.LocalPlayer.wingTimeMax > 0);
		public static readonly Condition sellVanillaSummons = ShopHelper.Create("sellVanillaSummons", () => CalamityConfig.Instance.SellVanillaSummons);
		public static readonly Condition rippersEnabled = ShopHelper.Create("rippersEnabled", () => CalamityWorld.revenge && CalamityConfig.Instance.Rippers);
		public static readonly Condition downedEvil = ShopHelper.Create("downedEvil", () => NPC.downedBoss2);
		public static readonly Condition downedCrabulon = ShopHelper.Create("downedCrabulon", () => CalamityWorld.downedCrabulon);
		public static readonly Condition downedHiveMind = ShopHelper.Create("downedHiveMind", () => CalamityWorld.downedHiveMind);
		public static readonly Condition downedPerforator = ShopHelper.Create("downedPerforator", () => CalamityWorld.downedPerforator);
		public static readonly Condition downedSlimeGod = ShopHelper.Create("downedSlimeGod", () => CalamityWorld.downedSlimeGod);
		public static readonly Condition downedBrimstoneElemental = ShopHelper.Create("downedBrimstoneElemental", () => CalamityWorld.downedBrimstoneElemental);
		public static readonly Condition downedAstrageldon = ShopHelper.Create("downedAstrageldon", () => CalamityWorld.downedAstrageldon);
		public static readonly Condition downedGuardians = ShopHelper.Create("downedGuardians", () => CalamityWorld.downedGuardians);
		public static readonly Condition downedScavenger = ShopHelper.Create("downedScavenger", () => CalamityWorld.downedScavenger);
		public static readonly Condition downedPlaguebringer = ShopHelper.Create("downedPlaguebringer", () => CalamityWorld.downedPlaguebringer);
		public static readonly Condition downedBumble = ShopHelper.Create("downedBumble", () => CalamityWorld.downedBumble);
		public static readonly Condition downedDesertScourge = ShopHelper.Create("downedDS", () => CalamityWorld.downedDesertScourge);
		public static readonly Condition downedEoCAcidRain = ShopHelper.Create("downedEoCAcidRain", () => CalamityWorld.downedEoCAcidRain);
		public static readonly Condition downedAquaticScourge = ShopHelper.Create("downedAquaticScourge", () => CalamityWorld.downedAquaticScourge);
		public static readonly Condition downedBoomerDuke = ShopHelper.Create("downedBoomerDuke", () => CalamityWorld.downedBoomerDuke);
		public static readonly Condition downedProvidence = ShopHelper.Create("downedProvidence", () => CalamityWorld.downedProvidence);
		public static readonly Condition downedDoG = ShopHelper.Create("downedDoG", () => CalamityWorld.downedDoG);
		public static readonly Condition dragonScalesAvailable = ShopHelper.Create("dragonScalesAvailable", () => CalamityWorld.buffedEclipse && !CalamityWorld.dragonScalesBought);
		public static readonly Condition downedCalamitas = ShopHelper.Create("downedCalamitas", () => CalamityWorld.downedCalamitas);
		#endregion
	}
}