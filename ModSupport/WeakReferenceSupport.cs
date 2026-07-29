using CalRD.Buffs.Summon;
using CalRD.Events;
using CalRD.Items;
using CalRD.Items.Accessories;
using CalRD.Items.Accessories.Wings;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.DifficultyItems;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Mounts;
using CalRD.Items.PermanentBoosters;
using CalRD.Items.Pets;
using CalRD.Items.Placeables;
using CalRD.Items.Placeables.Furniture;
using CalRD.Items.Placeables.Furniture.CraftingStations;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.Placeables.FurnitureCosmilite;
using CalRD.Items.Potions;
using CalRD.Items.Potions.Alcohol;
using CalRD.Items.SummonItems;
using CalRD.Items.SummonItems.Invasion;
using CalRD.Items.Tools;
using CalRD.Items.Tools.ClimateChange;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.DraedonsArsenal;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.Items.Weapons.Typeless;
using CalRD.NPCs.AcidRain;
using CalRD.NPCs.AquaticScourge;
using CalRD.NPCs.AstrumAureus;
using CalRD.NPCs.AstrumDeus;
using CalRD.NPCs.BrimstoneElemental;
using CalRD.NPCs.Bumblebirb;
using CalRD.NPCs.Calamitas;
using CalRD.NPCs.CeaselessVoid;
using CalRD.NPCs.Crabulon;
using CalRD.NPCs.Cryogen;
using CalRD.NPCs.DesertScourge;
using CalRD.NPCs.DevourerofGods;
using CalRD.NPCs.GreatSandShark;
using CalRD.NPCs.HiveMind;
using CalRD.NPCs.Leviathan;
using CalRD.NPCs.OldDuke;
using CalRD.NPCs.Perforator;
using CalRD.NPCs.PlaguebringerGoliath;
using CalRD.NPCs.Polterghast;
using CalRD.NPCs.ProfanedGuardians;
using CalRD.NPCs.Providence;
using CalRD.NPCs.Ravager;
using CalRD.NPCs.Signus;
using CalRD.NPCs.SlimeGod;
using CalRD.NPCs.StormWeaver;
using CalRD.NPCs.SunkenSea;
using CalRD.NPCs.SupremeCalamitas;
using CalRD.NPCs.TownNPCs;
using CalRD.NPCs.Yharon;
using CalRD.Projectiles.DraedonsArsenal;
using CalRD.Projectiles.Summon;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using CalRD.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

using static CalRD.Downed;
using static Terraria.ModLoader.ModContent;

namespace CalRD
{
	internal class WeakReferenceSupport
	{
		private static readonly Dictionary<string, float> BossDifficulty = new Dictionary<string, float>
		{
			{ "DesertScourge", 1.5f },
			{ "GiantClam", 1.6f },
			{ "Crabulon", 2.5f },
			{ "HiveMind", 3.5f },
			{ "Perforators", 3.51f },
			{ "SlimeGod", 5.5f },
			{ "Cryogen", 8.5f },
			{ "AquaticScourge", 9.5f },
			{ "BrimstoneElemental", 10.5f },
			{ "Calamitas", 11.7f },
			{ "GreatSandShark", 12.09f },
			{ "Leviathan", 12.5f },
			{ "AstrumAureus", 12.6f },
			{ "PlaguebringerGoliath", 13.5f },
			{ "Ravager", 14.5f },
			{ "AstrumDeus", 15.5f },
			{ "ProfanedGuardians", 18.5f },
			{ "Dragonfolly", 18.6f },
			{ "Providence", 19.01f }, // Thorium's Ragnarok is 15f
			{ "CeaselessVoid", 19.5f },
			{ "StormWeaver", 19.51f },
			{ "Signus", 19.52f },
			{ "Polterghast", 20f },
			{ "OldDuke", 20.5f },
			{ "DevourerOfGods", 21f },
			{ "Yharon", 22f },
			// { "Draedon", 18.5f },
			{ "SupremeCalamitas", 23f },
			// { "Yharim", 20f },
			// { "Noxus", 120f },
			// { "Xeroc", 121f },
		};

		private static readonly Dictionary<string, float> InvasionDifficulty = new Dictionary<string, float>
		{
			{ "AcidRainInitial", 2.4f },
			{ "AcidRainAquaticScourge", 9.51f },
			{ "AcidRainPolterghast", 20.49f }
		};

		public static void Setup()
		{
			BossChecklistSupport();
			FargosSupport();
			CensusSupport();
			SummonersAssociationSupport();
		}

		// Wrapper function to add bosses to Boss Checklist.
		private static void AddBoss(Mod bossChecklist, Mod hostMod, string name, float difficulty, Func<bool> downed, object npcTypes, Dictionary<string, object> extraInfo)
			=> bossChecklist.Call("LogBoss", hostMod, name, difficulty, downed, npcTypes, extraInfo);
		
		// Wrapper function to add minibosses to Boss Checklist.
		private static void AddMiniBoss(Mod bossChecklist, Mod hostMod, string name, float difficulty, Func<bool> downed, int npcType, Dictionary<string, object> extraInfo)
			=> bossChecklist.Call("LogMiniBoss", hostMod, name, difficulty, downed, npcType, extraInfo);

		// Wrapper function to add events to Boss Checklist.
		private static void AddEvent(Mod bossChecklist, Mod hostMod, string name, float difficulty, Func<bool> downed, List<int> npcTypes, Dictionary<string, object> extraInfo)
			=> bossChecklist.Call("LogEvent", hostMod, name, difficulty, downed, npcTypes, extraInfo);
		
		private static LocalizedText GetDisplayName(string entryName) => CalamityLocalization.GetText($"BossChecklistIntegration.{entryName}.EntryName");
		private static LocalizedText GetSpawnInfo(string entryName) => CalamityLocalization.GetText($"BossChecklistIntegration.{entryName}.SpawnInfo");
		private static LocalizedText GetDespawnMessage(string entryName) => CalamityLocalization.GetText($"BossChecklistIntegration.{entryName}.DespawnMessage");

		
		/// <summary>
		/// 1.0 = King Slime<br />
		/// 2.0 = Eye of Cthulhu<br />
		/// 3.0 = Eater of Worlds / Brain of Cthulhu<br />
		/// 4.0 = Queen Bee<br />
		/// 5.0 = Skeletron<br />
		/// 6.0 = Wall of Flesh<br />
		/// 7.0 = The Twins<br />
		/// 8.0 = The Destroyer<br />
		/// 9.0 = Skeletron Prime<br />
		/// 10.0 = Plantera<br />
		/// 11.0 = Golem<br />
		/// 12.0 = Duke Fishron<br />
		/// 13.0 = Lunatic Cultist<br />
		/// 14.0 = Moon Lord
		/// </summary>
		private static void BossChecklistSupport()
		{
			ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist);
			Mod calamity = GetInstance<CalRD>();

			if (bossChecklist is null)
				return;

			// Adds every single Calamity boss and miniboss to Boss Checklist's Boss Log.
			AddCalamityBosses(bossChecklist, calamity);

			// Adds every single Calamity invasion to the Boss Checklist's Invasion Log.
			AddCalamityInvasions(bossChecklist, calamity);

			// Loot which Calamity adds to vanilla bosses and events is also added to Boss Checklist's Boss Log.
			RegisterCalamityExtraInfo(bossChecklist, calamity);
			//AddCalamityEventLoot(bossChecklist);
		}

		private static void AddCalamityBosses(Mod bossChecklist, Mod calamity)
		{
			// Desert Scourge
			{
				string entryName = "DesertScourge";
				BossDifficulty.TryGetValue(entryName, out float order);
				List<int> segments = new List<int>() { NPCType<DesertScourgeHead>(), NPCType<DesertScourgeBody>(), NPCType<DesertScourgeTail>() };
				//List<int> loot = new List<int>() { ItemType<DesertScourgeBag>(), ItemID.SandBlock, ItemType<VictoryShard>(), ItemID.Coral, ItemID.Seashell, ItemID.Starfish, ItemType<AquaticDischarge>(), ItemType<Barinade>(), ItemType<StormSpray>(), ItemType<SeaboundStaff>(), ItemType<ScourgeoftheDesert>(), ItemType<DuneHopper>(), ItemType<AeroStone>(), ItemType<SandCloak>(), ItemType<DeepDiver>(), ItemType<OceanCrest>(),  ItemType<SandyAnglingKit>(), ItemID.LesserHealingPotion };
				List<int> collection = new List<int>() { ItemType<DesertScourgeTrophy>(), ItemType<DesertScourgeMask>(), ItemType<KnowledgeDesertScourge>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/NPCs/DesertScourge/DesertScourge_BossChecklist").Value;
					Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				};
				AddBoss(bossChecklist, calamity, entryName, order, DownedDesertScourge, segments, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<DriedSeafood>(),
					["collectibles"] = collection,
					["customPortrait"] = portrait
				});
			}

			// Giant Clam
			{
				string entryName = "GiantClam";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<GiantClam>();
				//List<int> loot = new List<int>() { ItemType<Navystone>(), ItemType<MolluskHusk>(), ItemType<ClamCrusher>(), ItemType<ClamorRifle>(), ItemType<Poseidon>(), ItemType<ShellfishStaff>(), ItemType<GiantPearl>(), ItemType<AmidiasPendant>() };
				AddMiniBoss(bossChecklist, calamity, entryName, order, DownedGiantClam, type, new Dictionary<string, object>()
				{
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName)
				});
			}

			// Crabulon
			{
				string entryName = "Crabulon";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<CrabulonIdle>();
				//List<int> loot = new List<int>() { ItemType<CrabulonBag>(), ItemID.GlowingMushroom, ItemID.MushroomGrassSeeds, ItemType<MycelialClaws>(), ItemType<Fungicide>(), ItemType<HyphaeRod>(), ItemType<Mycoroot>(), ItemType<Shroomerang>(), ItemType<FungalClump>(), ItemType<MushroomPlasmaRoot>(), ItemID.LesserHealingPotion };
				List<int> collection = new List<int>() { ItemType<CrabulonTrophy>(), ItemType<CrabulonMask>(), ItemType<KnowledgeCrabulon>() };
				AddBoss(bossChecklist, calamity, entryName, order, DownedCrabulon, type, new Dictionary<string, object>()
				{
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<DecapoditaSprout>(),
					["collectibles"] = collection
				});
				
			}

			// Hive Mind
			{
				string entryName = "HiveMind";
				BossDifficulty.TryGetValue(entryName, out float order);
				List<int> phases = new List<int>() { NPCType<HiveMind>(), NPCType<HiveMindP2>() };
				//List<int> loot = new List<int>() { ItemType<HiveMindBag>(), ItemType<TrueShadowScale>(), ItemID.DemoniteBar, ItemID.RottenChunk, ItemID.CursedFlame, ItemType<PerfectDark>(), ItemType<LeechingDagger>(), ItemType<Shadethrower>(), ItemType<ShadowdropStaff>(), ItemType<ShaderainStaff>(), ItemType<DankStaff>(), ItemType<RotBall>(), ItemType<FilthyGlove>(), ItemType<RottenBrain>(), ItemID.LesserHealingPotion };
				List<int> collection = new List<int>() { ItemType<HiveMindTrophy>(), ItemType<HiveMindMask>(), ItemType<KnowledgeHiveMind>() };
				AddBoss(bossChecklist, calamity, entryName, order, DownedHiveMind, phases, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<Teratoma>(),
					["collectibles"] = collection,
					["overrideHeadTextures"] = "CalRD/NPCs/HiveMind/HiveMindP2_Head_Boss"
				});
				
			}

			// Perforators
			{
				string entryName = "Perforators";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<PerforatorHive>();
				//List<int> loot = new List<int>() { ItemType<PerforatorBag>(), ItemType<BloodSample>(), ItemID.CrimtaneBar, ItemID.Vertebrae, ItemID.Ichor, ItemType<VeinBurster>(), ItemType<BloodyRupture>(), ItemType<SausageMaker>(), ItemType<Aorta>(), ItemType<Eviscerator>(), ItemType<BloodBath>(), ItemType<BloodClotStaff>(), ItemType<ToothBall>(), ItemType<BloodstainedGlove>(), ItemType<BloodyWormTooth>(), ItemID.LesserHealingPotion };
				List<int> collection = new List<int>() { ItemType<PerforatorTrophy>(), ItemType<PerforatorMask>(), ItemType<KnowledgePerforators>(), ItemType<BloodyVein>() };
				AddBoss(bossChecklist, calamity, entryName, order, DownedPerfs, type, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<BloodyWormFood>(),
					["collectibles"] = collection
				});
			}

			// Slime God
			{
				string entryName = "SlimeGod";
				BossDifficulty.TryGetValue(entryName, out float order);
				List<int> bosses = new List<int>() { NPCType<SlimeGodCore>(), NPCType<SlimeGod>(), NPCType<SlimeGodRun>() };
				//List<int> loot = new List<int>() { ItemType<SlimeGodBag>(), ItemID.Gel, ItemType<PurifiedGel>(), ItemType<OverloadedBlaster>(), ItemType<AbyssalTome>(), ItemType<EldritchTome>(), ItemType<CorroslimeStaff>(), ItemType<CrimslimeStaff>(), ItemType<GelDart>(), ItemType<ManaOverloader>(), ItemType<ElectrolyteGelPack>(), ItemType<PurifiedJam>(), ItemID.HealingPotion };
				List<int> collection = new List<int>() { ItemType<SlimeGodTrophy>(), ItemType<SlimeGodMask>(), ItemType<SlimeGodMask2>(), ItemType<KnowledgeSlimeGod>() };
				AddBoss(bossChecklist, calamity, entryName, order, DownedSlimeGod, bosses, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<OverloadedSludge>(),
					["collectibles"] = collection
				});
			}

			// Cryogen
			{
				string entryName = "Cryogen";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<Cryogen>();
				//List<int> loot = new List<int>() { ItemType<CryogenBag>(), ItemType<CryoBar>(), ItemType<EssenceofEleum>(), ItemID.FrostCore, ItemType<Avalanche>(), ItemType<GlacialCrusher>(), ItemType<EffluviumBow>(), ItemType<BittercoldStaff>(), ItemType<SnowstormStaff>(), ItemType<Icebreaker>(), ItemType<IceStar>(), ItemType<ColdDivinity>(), ItemType<CryoStone>(), ItemType<Regenator>(), ItemType<SoulofCryogen>(), ItemType<FrostFlare>(), ItemID.FrozenKey, ItemID.GreaterHealingPotion };
				List<int> collection = new List<int>() { ItemType<CryogenTrophy>(), ItemType<CryogenMask>(), ItemType<KnowledgeCryogen>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/NPCs/Cryogen/Cryogen_BossChecklist").Value;
					Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				};
				AddBoss(bossChecklist, calamity, entryName, order, DownedCryogen, type, new Dictionary<string, object>()
				{
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<CryoKey>(),
					["collectibles"] = collection,
					["overrideHeadTextures"] = "CalRD/NPCs/Cryogen/Cryogen_Phase1_Head_Boss",
					["customPortrait"] = portrait
				});
			}

			// Aquatic Scourge
			{
				string entryName = "AquaticScourge";
				BossDifficulty.TryGetValue(entryName, out float order);
				List<int> segments = new List<int>() { NPCType<AquaticScourgeHead>(), NPCType<AquaticScourgeBody>(), NPCType<AquaticScourgeBodyAlt>(), NPCType<AquaticScourgeTail>() };
				//List<int> loot = new List<int>() { ItemType<AquaticScourgeBag>(), ItemType<SulphurousSand>(), ItemType<VictoryShard>(), ItemID.Coral, ItemID.Seashell, ItemID.Starfish, ItemType<SubmarineShocker>(), ItemType<Barinautical>(), ItemType<Downpour>(), ItemType<DeepseaStaff>(), ItemType<ScourgeoftheSeas>(), ItemType<SeasSearing>(), ItemType<AeroStone>(), ItemType<AquaticEmblem>(), ItemType<CorrosiveSpine>(), ItemType<BleachedAnglingKit>(), ItemID.GreaterHealingPotion };
				List<int> collection = new List<int>() { ItemType<AquaticScourgeTrophy>(), ItemType<AquaticScourgeMask>(), ItemType<KnowledgeAquaticScourge>(), ItemType<KnowledgeSulphurSea>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/NPCs/AquaticScourge/AquaticScourge_BossChecklist").Value;
					Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				};
				AddBoss(bossChecklist, calamity, entryName, order, DownedAquaticScourge, segments, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<Seafood>(),
					["collectibles"] = collection,
					["customPortrait"] = portrait
				});
			}

			// Brimstone Elemental
			{
				string entryName = "BrimstoneElemental";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<BrimstoneElemental>();
				//List<int> loot = new List<int>() { ItemType<BrimstoneWaifuBag>(), ItemType<EssenceofChaos>(), ItemType<Bloodstone>(), ItemType<Brimlance>(), ItemType<DormantBrimseeker>(), ItemType<SeethingDischarge>(), ItemType<Abaddon>(), ItemType<RoseStone>(), ItemType<Gehenna>(), ItemType<Brimrose>(), ItemID.GreaterHealingPotion };
				List<int> collection = new List<int>() { ItemType<BrimstoneElementalTrophy>(), ItemType<BrimstoneWaifuMask>(), ItemType<KnowledgeBrimstoneCrag>(), ItemType<KnowledgeBrimstoneElemental>(), ItemType<CharredRelic>() };
				AddBoss(bossChecklist, calamity, entryName, order, DownedBrimstoneElemental, type, new Dictionary<string, object>()
				{
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<CharredIdol>(),
					["collectibles"] = collection
				});
			}

			// Calamitas
			{
				string entryName = "Calamitas";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<CalamitasRun3>();
				//List<int> loot = new List<int>() { ItemType<CalamitasBag>(), ItemType<EssenceofChaos>(), ItemType<CalamityDust>(), ItemType<BlightedLens>(), ItemType<Bloodstone>(), ItemType<CalamitasInferno>(), ItemType<TheEyeofCalamitas>(), ItemType<BlightedEyeStaff>(), ItemType<Animosity>(), ItemType<BrimstoneFlamesprayer>(), ItemType<BrimstoneFlameblaster>(), ItemType<CrushsawCrasher>(), ItemType<ChaosStone>(), ItemType<CalamityRing>(), ItemID.BrokenHeroSword, ItemID.GreaterHealingPotion };
				List<int> collection = new List<int>() { ItemType<CalamitasTrophy>(), ItemType<CataclysmTrophy>(), ItemType<CatastropheTrophy>(), ItemType<KnowledgeCalamitasClone>() };
				AddBoss(bossChecklist, calamity, entryName, order, DownedCalamitas, type, new Dictionary<string, object>()
				{
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<BlightedEyeball>(),
					["collectibles"] = collection
				});
				
			}

			// Great Sand Shark
			{
				string entryName = "GreatSandShark";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<GreatSandShark>();
				//List<int> loot = new List<int>() { ItemType<GrandScale>(), ItemID.AncientBattleArmorMaterial };
				List<int> collection = new List<int>() { ItemID.MusicBoxSandstorm };
				AddMiniBoss(bossChecklist, calamity, entryName, order, DownedGSS, type, new Dictionary<string, object>()
				{
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<SandstormsCore>(),
					["collectibles"] = collection
				});
			}

			// Siren and Leviathan
			{
				string entryName = "Leviathan";
				BossDifficulty.TryGetValue(entryName, out float order);
				List<int> bosses = new List<int>() { NPCType<Leviathan>(), NPCType<Siren>() };
				//List<int> loot = new List<int>() { ItemType<LeviathanBag>(), ItemType<Greentide>(), ItemType<Leviatitan>(), ItemType<SirensSong>(), ItemType<Atlantis>(), ItemType<GastricBelcherStaff>(), ItemType<BrackishFlask>(), ItemType<LeviathanTeeth>(), ItemType<LureofEnthrallment>(), ItemType<LeviathanAmbergris>(), ItemType<TheCommunity>(), ItemID.HotlineFishingHook, ItemID.BottomlessBucket, ItemID.SuperAbsorbantSponge, ItemID.FishingPotion, ItemID.SonarPotion, ItemID.CratePotion, ItemID.GreaterHealingPotion };
				List<int> collection = new List<int>() { ItemType<LeviathanTrophy>(), ItemType<LeviathanMask>(), ItemType<KnowledgeOcean>(), ItemType<KnowledgeLeviathanandSiren>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/NPCs/Leviathan/SirenandLevi_BossChecklist").Value;
					Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				};
				AddBoss(bossChecklist, calamity, entryName, order, DownedLeviathan, bosses, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["collectibles"] = collection,
					["customPortrait"] = portrait
				});
			}

			// Astrum Aureus
			{
				string entryName = "AstrumAureus";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<AstrumAureus>();
				//List<int> loot = new List<int>() { ItemType<AstrageldonBag>(), ItemType<Stardust>(), ItemID.FallenStar, ItemType<Nebulash>(), ItemType<AuroraBlazer>(), ItemType<AlulaAustralis>(), ItemType<BorealisBomber>(), ItemType<AuroradicalThrow>(), ItemType<LeonidProgenitor>(), ItemType<GravistarSabaton>(), ItemType<AstralJelly>(), ItemID.HallowedKey, ItemType<StarlightFuelCell>(), ItemID.GreaterHealingPotion };
				List<int> collection = new List<int>() { ItemType<AstrageldonTrophy>(), ItemType<AureusMask>(), ItemType<KnowledgeAstrumAureus>() };
				string bossLogTex = "CalRD/NPCs/AstrumAureus/AstrumAureus_BossChecklist";
				AddBoss(bossChecklist, calamity, entryName, order, DownedAureus, type, new Dictionary<string, object>()
				{
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<AstralChunk>(),
					["collectibles"] = collection
				});
			}

			// Plaguebringer Goliath
			{
				string entryName = "PlaguebringerGoliath";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<PlaguebringerGoliath>();
				//List<int> loot = new List<int>() { ItemType<PlaguebringerGoliathBag>(), ItemType<PlagueCellCluster>(), ItemType<InfectedArmorPlating>(), ItemID.Stinger, ItemType<VirulentKatana>(), ItemType<DiseasedPike>(), ItemType<ThePlaguebringer>(), ItemType<Malevolence>(), ItemType<PestilentDefiler>(), ItemType<TheHive>(), ItemType<MepheticSprayer>(), ItemType<PlagueStaff>(), ItemType<TheSyringe>(), ItemType<FuelCellBundle>(), ItemType<InfectedRemote>(), ItemType<Malachite>(), ItemType<BloomStone>(), ItemType<ToxicHeart>(), ItemID.GreaterHealingPotion };
				List<int> collection = new List<int>() { ItemType<PlaguebringerGoliathTrophy>(), ItemType<PlaguebringerGoliathMask>(), ItemType<KnowledgePlaguebringerGoliath>(), ItemType<PlagueCaller>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/NPCs/PlaguebringerGoliath/PlaguebringerGoliath_BossChecklist").Value;
					Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				};
				AddBoss(bossChecklist, calamity, entryName, order, DownedPBG, type, new Dictionary<string, object>()
				{
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<Abomination>(),
					["collectibles"] = collection,
					["customPortrait"] = portrait
				});
			}

			// Ravager
			{
				string entryName = "Ravager";
				BossDifficulty.TryGetValue(entryName, out float order);
				List<int> segments = new List<int>() { NPCType<RavagerBody>(), NPCType<RavagerClawLeft>(), NPCType<RavagerClawRight>(), NPCType<RavagerHead>(), NPCType<RavagerLegLeft>(), NPCType<RavagerLegRight>() };
				//List<int> loot = new List<int>() { ItemType<RavagerBag>(), ItemType<FleshyGeodeT1>(), ItemType<FleshyGeodeT2>(), ItemType<UltimusCleaver>(), ItemType<RealmRavager>(), ItemType<Hematemesis>(), ItemType<SpikecragStaff>(), ItemType<CraniumSmasher>(), ItemType<BloodPact>(), ItemType<FleshTotem>(), ItemType<BloodflareCore>(), ItemType<InfernalBlood>(), ItemID.GreaterHealingPotion };
				List<int> collection = new List<int>() { ItemType<RavagerTrophy>(), ItemType<RavagerMask>(), ItemType<KnowledgeRavager>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/NPCs/Ravager/Ravager_BossChecklist").Value;
					Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				};
				AddBoss(bossChecklist, calamity, entryName, order, DownedRavager, segments, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<AncientMedallion>(),
					["collectibles"] = collection,
					["customPortrait"] = portrait
				});
			}

			// Astrum Deus
			{
				string entryName = "AstrumDeus";
				BossDifficulty.TryGetValue(entryName, out float order);
				List<int> segments = new List<int>() { NPCType<AstrumDeusHeadSpectral>(), NPCType<AstrumDeusBodySpectral>(), NPCType<AstrumDeusTailSpectral>() };
				List<int> summons = new List<int>() { ItemType<TitanHeart>(), ItemType<Starcore>() };
				//List<int> loot = new List<int>() { ItemType<AstrumDeusBag>(), ItemType<Stardust>(), ItemID.FallenStar, ItemType<TheMicrowave>(), ItemType<StarSputter>(), ItemType<Starfall>(), ItemType<GodspawnHelixStaff>(), ItemType<RegulusRiot>(), ItemType<Quasar>(), ItemType<AstralBulwark>(), ItemType<HideofAstrumDeus>(), ItemID.FragmentSolar, ItemID.FragmentVortex, ItemID.FragmentNebula, ItemID.FragmentStardust, ItemID.GreaterHealingPotion };
				List<int> collection = new List<int>() { ItemType<AstrumDeusTrophy>(), ItemType<AstrumDeusMask>(), ItemType<KnowledgeAstrumDeus>(), ItemType<KnowledgeAstralInfection>(), ItemType<ChromaticOrb>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/NPCs/AstrumDeus/AstrumDeus_BossChecklist").Value;
					Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				};
				AddBoss(bossChecklist, calamity, entryName, order, DownedDeus, segments, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = summons,
					["collectibles"] = collection,
					["customPortrait"] = portrait
				});
			}

			// Profaned Guardians
			{
				string entryName = "ProfanedGuardians";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<ProfanedGuardianBoss>();
				//List<int> loot = new List<int>() { ItemType<RelicOfResilience>(), ItemType<RelicOfConvergence>(), ItemType<RelicOfDeliverance>(), ItemType<ProfanedCore>(), ItemID.GreaterHealingPotion };
				List<int> collection = new List<int>() { ItemType<ProfanedGuardianTrophy>(), ItemType<ProfanedGuardianMask>(), ItemType<KnowledgeProfanedGuardians>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/NPCs/ProfanedGuardians/ProfanedGuardians_BossChecklist").Value;
					float scale = 0.7f;
					Vector2 centered = new Vector2(rect.Center.X - texture.Width * scale / 2, rect.Center.Y - texture.Height * scale / 2);
					sb.Draw(texture, centered, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
				};
				AddBoss(bossChecklist, calamity, entryName, order, DownedGuardians, type, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<ProfanedShard>(),
					["collectibles"] = collection,
					["customPortrait"] = portrait,
					["overrideHeadTextures"] = "CalRD/NPCs/ProfanedGuardians/ProfanedGuardianCommander_Head_Boss"
				});
			}

			// Dragonfolly
			{
				string entryName = "Dragonfolly";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<Bumblefuck>();
				//List<int> loot = new List<int>() { ItemType<BumblebirbBag>(), ItemType<EffulgentFeather>(), ItemType<GildedProboscis>(), ItemType<GoldenEagle>(), ItemType<RougeSlash>(), ItemType<Swordsplosion>(), ItemType<BirdSeed>(), ItemType<DynamoStemCells>(), ItemType<RedLightningContainer>(), ItemID.SuperHealingPotion };
				List<int> collection = new List<int>() { ItemType<BumblebirbTrophy>(), ItemType<BumblefuckMask>(), ItemType<KnowledgeBumblebirb>() };
				AddBoss(bossChecklist, calamity, entryName, order, DownedBirb, type, new Dictionary<string, object>()
				{
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<BirbPheromones>(),
					["collectibles"] = collection
				});
			}

			// Providence
			{
				string entryName = "Providence";
				BossDifficulty.TryGetValue(entryName, out float order);
				List<int> bosses = new List<int>() { NPCType<Providence>(), NPCType<ProvSpawnOffense>(), NPCType<ProvSpawnDefense>(), NPCType<ProvSpawnHealer>() };
				List<int> summons = new List<int>() { ItemType<ProfanedCore>(), ItemType<ProfanedCoreUnlimited>() };
				//List<int> loot = new List<int>() { ItemType<ProvidenceBag>(), ItemType<UnholyEssence>(), ItemType<DivineGeode>(), ItemType<HolyCollider>(), ItemType<SolarFlare>(), ItemType<TelluricGlare>(), ItemType<BlissfulBombardier>(), ItemType<PurgeGuzzler>(), ItemType<MoltenAmputator>(), ItemType<DazzlingStabberStaff>(), ItemType<PristineFury>(), ItemType<ElysianWings>(), ItemType<ElysianAegis>(), ItemType<SamuraiBadge>(), ItemType<BlazingCore>(), ItemType<RuneofCos>(), ItemID.SuperHealingPotion };
				List<int> collection = new List<int>() { ItemType<ProvidenceTrophy>(), ItemType<ProvidenceMask>(), ItemType<KnowledgeProvidence>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/NPCs/Providence/Providence_BossChecklist").Value;
					Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				};
				AddBoss(bossChecklist, calamity, entryName, order, DownedProvidence, bosses, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = summons,
					["collectibles"] = collection,
					["customPortrait"] = portrait
				});
			}

			// Ceaseless Void
			{
				string entryName = "CeaselessVoid";
				BossDifficulty.TryGetValue(entryName, out float order);
				List<int> bosses = new List<int>() { NPCType<CeaselessVoid>(), NPCType<DarkEnergy>(), NPCType<DarkEnergy2>(), NPCType<DarkEnergy3>() };
				//List<int> loot = new List<int>() { ItemType<DarkPlasma>(), ItemType<MirrorBlade>(), ItemType<ArcanumoftheVoid>(), ItemType<TheEvolution>(), ItemID.SuperHealingPotion };
				List<int> collection = new List<int>() { ItemType<CeaselessVoidTrophy>(), ItemType<CeaselessVoidMask>(), ItemType<KnowledgeSentinels>() };
				AddBoss(bossChecklist, calamity, entryName, order, DownedCeaselessVoid, bosses, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<RuneofCos>(),
					["collectibles"] = collection
				});
			}

			// Storm Weaver
			{
				string entryName = "StormWeaver";
				BossDifficulty.TryGetValue(entryName, out float order);
				List<int> segments = new List<int>() { NPCType<StormWeaverHead>(), NPCType<StormWeaverBody>(), NPCType<StormWeaverTail>(), NPCType<StormWeaverHeadNaked>(), NPCType<StormWeaverBodyNaked>(), NPCType<StormWeaverTailNaked>() };
				//List<int> loot = new List<int>() { ItemType<ArmoredShell>(), ItemType<TheStorm>(), ItemType<StormDragoon>(), ItemID.SuperHealingPotion };
				List<int> collection = new List<int>() { ItemType<WeaverTrophy>(), ItemType<StormWeaverMask>(), ItemType<KnowledgeSentinels>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/NPCs/StormWeaver/StormWeaver_BossChecklist").Value;
					Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				};
				AddBoss(bossChecklist, calamity, entryName, order, DownedStormWeaver, segments, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<RuneofCos>(),
					["collectibles"] = collection,
					["customPortrait"] = portrait,
					["overrideHeadTextures"] = "CalRD/NPCs/StormWeaver/StormWeaverHead_Head_Boss"
				});
			}

			// Signus
			{
				string entryName = "Signus";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<Signus>();
				//List<int> loot = new List<int>() { ItemType<TwistingNether>(), ItemType<Cosmilamp>(), ItemType<CosmicKunai>(), ItemType<LanternoftheSoul>(), ItemType<SpectralVeil>(), ItemID.SuperHealingPotion };
				List<int> collection = new List<int>() { ItemType<SignusTrophy>(), ItemType<SignusMask>(), ItemType<KnowledgeSentinels>() };
				AddBoss(bossChecklist, calamity, entryName, order, DownedSignus, type, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<RuneofCos>(),
					["collectibles"] = collection
				});
			}

			// Polterghast
			{
				string entryName = "Polterghast";
				BossDifficulty.TryGetValue(entryName, out float order);
				List<int> bosses = new List<int>() { NPCType<Polterghast>(), NPCType<PolterPhantom>() };
				//List<int> loot = new List<int>() { ItemType<PolterghastBag>(), ItemType<RuinousSoul>(), ItemType<Phantoplasm>(), ItemType<TerrorBlade>(), ItemType<BansheeHook>(), ItemType<DaemonsFlame>(), ItemType<FatesReveal>(), ItemType<GhastlyVisage>(), ItemType<EtherealSubjugator>(), ItemType<GhoulishGouger>(), ItemType<Affliction>(), ItemType<Ectoheart>(), ItemID.SuperHealingPotion };
				List<int> collection = new List<int>() { ItemType<PolterghastTrophy>(), ItemType<PolterghastMask>(), ItemType<KnowledgePolterghast>() };
				AddBoss(bossChecklist, calamity, entryName, order, DownedPolterghast, bosses, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<NecroplasmicBeacon>(),
					["collectibles"] = collection
				});
			}

			// Old Duke
			{
				string entryName = "OldDuke";
				BossDifficulty.TryGetValue(entryName, out float order);
				List<int> bosses = new List<int>() { NPCType<OldDuke>() };
				//List<int> loot = new List<int>() { ItemType<OldDukeBag>(), ItemType<InsidiousImpaler>(), ItemType<SepticSkewer>(), ItemType<FetidEmesis>(), ItemType<VitriolicViper>(), ItemType<CadaverousCarrion>(), ItemType<ToxicantTwister>(), ItemType<DukeScales>(), ItemType<MutatedTruffle>(), ItemID.SuperHealingPotion };
				List<int> collection = new List<int>() { ItemType<OldDukeTrophy>(), ItemType<OldDukeMask>(), ItemType<KnowledgeOldDuke>() };
				string instructions = $"Defeat the Acid Rain event post-Polterghast or fish using a [i:CalRD/BloodwormItem] in the Sulphurous Sea";
				string despawn = CalamityUtils.ColorMessage("The old duke disappears amidst the acidic downpour.", new Color(0xF0, 0xE6, 0x8C));
				AddBoss(bossChecklist, calamity, entryName, order, DownedBoomerDuke, bosses, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<BloodwormItem>(),
					["collectibles"] = collection
				});
			}

			// Devourer of Gods
			{
				string entryName = "DevourerOfGods";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<DevourerofGodsHeadS>();
				//List<int> loot = new List<int>() { ItemType<DevourerofGodsBag>(), ItemType<CosmiliteBar>(), ItemType<CosmiliteBrick>(), ItemType<Excelsus>(), ItemType<EradicatorMelee>(), ItemType<TheObliterator>(), ItemType<Deathwind>(), ItemType<DeathhailStaff>(), ItemType<StaffoftheMechworm>(), ItemType<Eradicator>(), ItemType<Skullmasher>(), ItemType<Norfleet>(), ItemType<CosmicDischarge>(), ItemType<NebulousCore>(), ItemType<Fabsol>(), ItemType<SupremeHealingPotion>() };
				List<int> collection = new List<int>() { ItemType<DevourerofGodsTrophy>(), ItemType<DevourerofGodsMask>(), ItemType<KnowledgeDevourerofGods>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/NPCs/DevourerofGods/DevourerofGods_BossChecklist").Value;
					Vector2 centered = new Vector2(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
					sb.Draw(texture, centered, color);
				};
				string bossHeadTex = "CalRD/NPCs/DevourerofGods/DevourerofGodsHead_Head_Boss";
				AddBoss(bossChecklist, calamity, entryName, order, DownedDoG, type, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<CosmicWorm>(),
					["collectibles"] = collection,
					["customPortrait"] = portrait,
					["overrideHeadTextures"] = "CalRD/NPCs/DevourerofGods/DevourerofGodsHead_Head_Boss"
				});
			}

			// Yharon
			{
				string entryName = "Yharon";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<Yharon>();
				int summon = ItemType<ChickenEgg>();
				//List<int> loot = new List<int>() { ItemType<YharonBag>(), ItemType<HellcasterFragment>(), ItemType<DragonRage>(), ItemType<TheBurningSky>(), ItemType<DragonsBreath>(), ItemType<ChickenCannon>(), ItemType<PhoenixFlameBarrage>(), ItemType<AngryChickenStaff>(), ItemType<ProfanedTrident>(), ItemType<FinalDawn>(), ItemType<VoidVortex>(), ItemType<YharimsCrystal>(), ItemType<YharimsGift>(), ItemType<DrewsWings>(), /*ItemType<BossRush>(), */ItemType<OmegaHealingPotion>() };
				List<int> collection = new List<int>() { ItemType<YharonTrophy>(), ItemType<YharonMask>(), ItemType<KnowledgeYharon>(), ItemType<ForgottenDragonEgg>(), ItemType<FoxDrive>() };
				// TODO -- this setup code is only run once, so the despawn message can't be changed post-eclipse. Find a way around this.
				AddBoss(bossChecklist, calamity, entryName, order, DownedYharon, type, new Dictionary<string, object>()
				{
					["displayName"] = GetDisplayName(entryName),
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<ChickenEgg>(),
					["collectibles"] = collection,
					["overrideHeadTextures"] = "CalRD/NPCs/Yharon/Yharon_BossChecklist"
				});
			}

			// Supreme Calamitas
			{
				string entryName = "SupremeCalamitas";
				BossDifficulty.TryGetValue(entryName, out float order);
				int type = NPCType<SupremeCalamitas>();
				//List<int> loot = new List<int>() { ItemType<CalamitousEssence>(), ItemType<Animus>(), ItemType<Azathoth>(), ItemType<Contagion>(), ItemType<CrystylCrusher>(), ItemType<Judgement>(), ItemType<DraconicDestruction>(), ItemType<Earth>(), ItemType<Endogenesis>(), ItemType<Fabstaff>(), ItemType<RoyalKnivesMelee>(), ItemType<RoyalKnives>(), ItemType<NanoblackReaperMelee>(), ItemType<NanoblackReaperRogue>(), ItemType<RedSun>(), ItemType<ScarletDevil>(), ItemType<SomaPrime>(), ItemType<BlushieStaff>(), ItemType<Svantechnical>(), ItemType<BensUmbrella>(), ItemType<TriactisTruePaladinianMageHammerofMightMelee>(), ItemType<TriactisTruePaladinianMageHammerofMight>(), ItemType<Megafleet>(), ItemType<PrototypeAndromechaRing>(), ItemType<Vehemenc>(), ItemType<OmegaHealingPotion>() };
				List<int> collection = new List<int>() { ItemType<SupremeCalamitasTrophy>(), ItemType<KnowledgeCalamitas>(), ItemType<BrimstoneJewel>(), ItemType<Levi>() };
				AddBoss(bossChecklist, calamity, entryName, order, DownedSCal, type, new Dictionary<string, object>()
				{
					["spawnInfo"] = GetSpawnInfo(entryName),
					["despawnMessage"] = GetDespawnMessage(entryName),
					["spawnItems"] = ItemType<EyeofExtinction>(),
					["collectibles"] = collection
				});
			}
		}
		
		private static void AddCalamityInvasions(Mod bossChecklist, Mod calamity)
		{
			// Initial Acid Rain
			{
				string entryName = "AcidRainInitial";
				InvasionDifficulty.TryGetValue(entryName, out float order);
				List<int> enemies = AcidRainEvent.PossibleEnemiesPreHM.Select(enemy => enemy.Key).ToList();
				//List<int> loot = new List<int>() { ItemType<SulfuricScale>(), ItemType<ParasiticSceptor>() };
				string bossLogTex = "CalRD/Events/AcidRainT1_BossChecklist";
				string iconTexture = "CalRD/ExtraTextures/UI/AcidRainIcon";
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/Events/AcidRainT1_BossChecklist").Value;
					float scale = 1f;
					Vector2 centered = new Vector2(rect.Center.X - texture.Width * scale / 2, rect.Center.Y - texture.Height * scale / 2);
					sb.Draw(texture, centered, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
				};
				AddEvent(bossChecklist, calamity, entryName, order, DownedAcidRainInitial, enemies, new Dictionary<string, object>()
				{
					["spawnItems"] = ItemType<CausticTear>(),
					["collectibles"] = ItemType<RadiatingCrystal>(),
					["customPortrait"] = portrait,
					["overrideHeadTextures"] = "CalRD/ExtraTextures/UI/AcidRainIcon"
				});
			}
			// Post-Aquatic Scourge Acid Rain
			{
				string entryName = "AcidRainAquaticScourge";
				InvasionDifficulty.TryGetValue(entryName, out float order);
				List<int> enemies = AcidRainEvent.PossibleEnemiesAS.Select(enemy => enemy.Key).ToList();
				enemies.Add(NPCType<IrradiatedSlime>());
				enemies.AddRange(AcidRainEvent.PossibleMinibossesAS.Select(miniboss => miniboss.Key));
				List<int> summons = new List<int>() { ItemType<CausticTear>(), ItemType<CausticTearNonConsumable>() };
				//List<int> loot = new List<int>() { ItemType<SulfuricScale>(), ItemType<CorrodedFossil>(), ItemType<LeadCore>(), ItemType<NuclearRod>(), ItemType<ParasiticSceptor>(), ItemType<FlakToxicannon>(), ItemType<OrthoceraShell>(), ItemType<SkyfinBombers>(), ItemType<SlitheringEels>(), ItemType<SpentFuelContainer>(), ItemType<SulphurousGrabber>() };
				List<int> collection = new List<int>() { ItemType<RadiatingCrystal>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = ModContent.Request<Texture2D>("CalRD/Events/AcidRainT2_BossChecklist").Value;
					float scale = 0.9f;
					Vector2 centered = new Vector2(rect.Center.X - texture.Width * scale / 2, rect.Center.Y - texture.Height * scale / 2);
					sb.Draw(texture, centered, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
				};
				AddEvent(bossChecklist, calamity, entryName, order, DownedAcidRainHardmode, enemies, new Dictionary<string, object>()
				{
					["spawnItems"] = summons,
					["collectibles"] = collection,
					["customPortrait"] = portrait,
					["overrideHeadTextures"] = "CalRD/ExtraTextures/UI/AcidRainIcon",
					["availability"] = DownedAcidRainInitial
				});
			}
			// Post-Polterghast Acid Rain
			{
				string entryName = "AcidRainPolterghast";
				InvasionDifficulty.TryGetValue(entryName, out float order);
				List<int> enemies = AcidRainEvent.PossibleEnemiesPolter.Select(enemy => enemy.Key).ToList();
				enemies.AddRange(AcidRainEvent.PossibleMinibossesPolter.Select(miniboss => miniboss.Key));
				List<int> summons = new List<int>() { ItemType<CausticTear>(), ItemType<CausticTearNonConsumable>() };
				//List<int> loot = new List<int>() { ItemType<SulfuricScale>(), ItemType<CorrodedFossil>(), ItemType<LeadCore>(), ItemType<NuclearRod>(), ItemType<ParasiticSceptor>(), ItemType<FlakToxicannon>(), ItemType<OrthoceraShell>(), ItemType<SkyfinBombers>(), ItemType<SlitheringEels>(), ItemType<SpentFuelContainer>(), ItemType<SulphurousGrabber>(), ModContent.ItemType<GammaHeart>() };
				List<int> collection = new List<int>() { ItemType<RadiatingCrystal>() };
				Action<SpriteBatch, Rectangle, Color> portrait = (SpriteBatch sb, Rectangle rect, Color color) => {
					Texture2D texture = Request<Texture2D>("CalRD/Events/AcidRainT3_BossChecklist").Value;
					float scale = 0.9f;
					Vector2 centered = new Vector2(rect.Center.X - texture.Width * scale / 2, rect.Center.Y - texture.Height * scale / 2);
					sb.Draw(texture, centered, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
				};
				AddEvent(bossChecklist, calamity, entryName, order, DownedBoomerDuke, enemies, new Dictionary<string, object>()
				{
					["spawnItems"] = summons,
					["collectibles"] = collection,
					["customPortrait"] = portrait,
					["overrideHeadTextures"] = "CalRD/ExtraTextures/UI/AcidRainIcon",
					["availability"] = DownedAcidRainHardmode
				});
			}
		}

		private static void RegisterCalamityExtraInfo(Mod bossChecklist, Mod calamity)
		{
			/*
			// King Slime
			AddLoot(bossChecklist, "KingSlime",
				new List<int>() { ItemType<CrownJewel>() },
				new List<int>() { ItemType<KnowledgeKingSlime>() }
			);

			// Eye of Cthulhu
			AddLoot(bossChecklist, "EyeofCthulhu",
				new List<int>() { ItemType<VictoryShard>(), ItemType<TeardropCleaver>(), ItemType<CounterScarf>() },
				new List<int>() { ItemType<KnowledgeEyeofCthulhu>() }
			);

			// Eater of Worlds
			AddLoot(bossChecklist, "EaterofWorldsHead",
				null,
				new List<int>() { ItemType<KnowledgeEaterofWorlds>(), ItemType<KnowledgeCorruption>() }
			);

			// Brain of Cthulhu
			AddLoot(bossChecklist, "BrainofCthulhu",
				null,
				new List<int>() { ItemType<KnowledgeBrainofCthulhu>(), ItemType<KnowledgeCrimson>() }
			);

			// Queen Bee
			AddLoot(bossChecklist, "QueenBee",
				new List<int>() { ItemType<HardenedHoneycomb>(), ItemID.Stinger },
				new List<int>() { ItemType<KnowledgeQueenBee>() }
			);

			// Skeletron
			AddLoot(bossChecklist, "SkeletronHead",
				new List<int>() { ItemType<ClothiersWrath>() },
				new List<int>() { ItemType<KnowledgeSkeletron>() }
			);

			// Wall of Flesh
			AddLoot(bossChecklist, "WallofFlesh",
				new List<int>() { ItemType<Meowthrower>(), ItemType<BlackHawkRemote>(), ItemType<BlastBarrel>(), ItemType<RogueEmblem>(), ItemType<MLGRune>(), ItemID.CorruptionKey, ItemID.CrimsonKey },
				new List<int>() { ItemType<KnowledgeWallofFlesh>(), ItemType<KnowledgeUnderworld>(), ItemType<IbarakiBox>() }
			);

			// The Twins
			AddLoot(bossChecklist, "TheTwins",
				null,
				new List<int>() { ItemType<KnowledgeTwins>(), ItemType<KnowledgeMechs>() }
			);

			// The Destroyer
			AddLoot(bossChecklist, "TheDestroyer",
				new List<int>() { ItemType<SHPC>() },
				new List<int>() { ItemType<KnowledgeDestroyer>(), ItemType<KnowledgeMechs>() }
			);

			// Skeletron Prime
			AddLoot(bossChecklist, "SkeletronPrime",
				null,
				new List<int>() { ItemType<KnowledgeSkeletronPrime>(), ItemType<KnowledgeMechs>() }
			);

			// Plantera
			AddLoot(bossChecklist, "Plantera",
				new List<int>() { ItemType<LivingShard>(), ItemType<BlossomFlux>(), ItemID.JungleKey },
				new List<int>() { ItemType<KnowledgePlantera>() }
			);
			AddSummons(bossChecklist, "Plantera", new List<int>() { ItemType<BulbofDoom>() });

			// Golem
			AddLoot(bossChecklist, "Golem",
				new List<int>() { ItemType<EssenceofCinder>(), ItemType<AegisBlade>() },
				new List<int>() { ItemType<KnowledgeGolem>() }
			);
			AddSummons(bossChecklist, "Golem", new List<int>() { ItemType<OldPowerCell>() });

			// Duke Fishron
			AddLoot(bossChecklist, "DukeFishron",
				new List<int>() { ItemType<DukesDecapitator>(), ItemType<BrinyBaron>() },
				new List<int>() { ItemType<KnowledgeDukeFishron>() }
			);

			// Betsy
			AddLoot(bossChecklist, "DD2Betsy",
				null,
				new List<int>() { ItemType<Vesuvius>() }
			);

			// Lunatic Cultist
			AddLoot(bossChecklist, "CultistBoss",
				new List<int>() { ItemType<StardustStaff>(), ItemType<ThornBlossom>() },
				new List<int>() { ItemType<KnowledgeLunaticCultist>(), ItemType<KnowledgeBloodMoon>() }
			);
			AddSummons(bossChecklist, "CultistBoss", new List<int>() { ItemType<EidolonTablet>() });

			// Moon Lord
			AddLoot(bossChecklist, "MoonLord",
				new List<int>() { ItemType<UtensilPoker>(), ItemType<GrandDad>(), ItemType<Infinity>(), ItemType<MLGRune2>() },
				new List<int>() { ItemType<KnowledgeMoonLord>() }
			);
			*/
			bossChecklist.Call("SubmitEntrySpawnItems", calamity, new Dictionary<string, object>()
			{
				{ "Terraria Plantera", ItemType<BulbofDoom>() },
				{ "Terraria Golem", ItemType<OldPowerCell>() },
				{ "Terraria CultistBoss", ItemType<EidolonTablet>() }
			});
		/*}

		private static void AddCalamityEventLoot(Mod bossChecklist)
		{*/
			/*
			// Blood Moon
			AddLoot(bossChecklist, "Blood Moon",
				new List<int>() { ItemType<BloodOrb>(), ItemType<BouncingEyeball>(), ItemType<Carnage>() },
				null
			);
			AddSummons(bossChecklist, "Blood Moon", new List<int>() { ItemType<BloodIdol>() });

			// Goblin Army
			AddLoot(bossChecklist, "Goblin Army",
				new List<int>() { ItemType<PlasmaRod>(), ItemType<Warblade>(), ItemType<TheFirstShadowflame>(), ItemType<BurningStrife>() },
				null
			);

			// Pirates
			AddLoot(bossChecklist, "Pirate Invasion",
				new List<int>() { ItemType<RaidersGlory>(), ItemType<Arbalest>(), ItemType<ProporsePistol>() },
				null
			);

			// Solar Eclipse
			AddLoot(bossChecklist, "Solar Eclipse",
				new List<int>() { ItemType<SolarVeil>(), ItemType<DefectiveSphere>(), ItemType<DarksunFragment>() },
				null
			);

			// Pumpkin Moon
			AddLoot(bossChecklist, "Pumpkin Moon",
				new List<int>() { ItemType<NightmareFuel>() },
				null
			);
			AddLoot(bossChecklist, "Pumpking",
				new List<int>() { ItemType<NightmareFuel>() },
				null
			);

			// Frost Moon
			AddLoot(bossChecklist, "Frost Moon",
				new List<int>() { ItemType<HolidayHalberd>(), ItemType<EndothermicEnergy>() },
				null
			);
			AddLoot(bossChecklist, "Ice Queen",
				new List<int>() { ItemType<EndothermicEnergy>() },
				null
			);

			// Martian Madness
			AddLoot(bossChecklist, "Martian Madness",
				new List<int>() { ItemType<Wingman>(), ItemType<ShockGrenade>(), ItemType<NullificationRifle>() },
				null
			);
			AddLoot(bossChecklist, "Martian Saucer",
				new List<int>() { ItemType<NullificationRifle>() },
				null
			);

			// Lunar Events
			AddLoot(bossChecklist, "Lunar Event",
				new List<int>() { ItemType<MeldBlob>(), ItemType<TrueConferenceCall>() },
				null
			);
			*/
			bossChecklist.Call("SubmitEntryCollectibles", calamity, new Dictionary<string, object>()
            {
                { "Terraria KingSlime", new List<int>() { ItemType<KnowledgeKingSlime>() } },
                { "Terraria EyeofCthulhu", new List<int>() { ItemType<KnowledgeEyeofCthulhu>() } },
                { "Terraria EaterofWorldsHead", new List<int>() { ItemType<KnowledgeEaterofWorlds>(), ItemType<KnowledgeCorruption>() } },
                { "Terraria BrainofCthulhu", new List<int>() { ItemType<KnowledgeBrainofCthulhu>(), ItemType<KnowledgeCrimson>() } },
                { "Terraria QueenBee", new List<int>() { ItemType<KnowledgeQueenBee>() } },
                { "Terraria SkeletronHead", new List<int>() { ItemType<KnowledgeSkeletron>() } },
                { "Terraria WallofFlesh", new List<int>() { ItemType<KnowledgeWallofFlesh>(), ItemType<KnowledgeUnderworld>(), ItemType<IbarakiBox>() } },
                { "Terraria TheTwins", new List<int>() { ItemType<KnowledgeTwins>(), ItemType<KnowledgeMechs>() } },
                { "Terraria TheDestroyer", new List<int>() { ItemType<KnowledgeDestroyer>(), ItemType<KnowledgeMechs>() } },
                { "Terraria SkeletronPrime", new List<int>() { ItemType<KnowledgeSkeletronPrime>(), ItemType<KnowledgeMechs>() } },
                { "Terraria Plantera", new List<int>() { ItemType<KnowledgePlantera>() } },
                { "Terraria Golem", new List<int>() { ItemType<KnowledgeGolem>() } },
                { "Terraria DukeFishron", new List<int>() { ItemType<KnowledgeDukeFishron>() } },
                { "Terraria CultistBoss", new List<int>() { ItemType<KnowledgeLunaticCultist>() } },
                { "Terraria MoonLord", new List<int>() { ItemType<KnowledgeMoonLord>() } }
            });
		}

		private static void FargosSupport()
		{
			ModLoader.TryGetMod("Fargowiltas", out Mod fargos);
			if (fargos is null)
				return;

			// Mark Fargo's Mutant Mod as loaded so that Calamity doesn't add ANY boss summons to vanilla NPCs, even for its own bosses
			GetInstance<CalRD>().fargosMutant = true;

			void AddToMutantShop(string bossName, string summonItemName, Func<bool> downed, int price)
			{
				BossDifficulty.TryGetValue(bossName, out float order);
				fargos.Call("AddSummon", order, "CalRD", summonItemName, downed, price);
			}

			void AddToAbomShop(float order, string summonItemName, Func<bool> downed, int price)
			{
				fargos.Call("AddEventSummon", order, "CalRD", summonItemName, downed, price);
			}

			fargos.Call("AbominationnClearEvents", "CalRD", CalamityWorld.rainingAcid, true);

			AddToMutantShop("DesertScourge", "DriedSeafood", DownedDesertScourge, Item.buyPrice(gold: 2));
			AddToMutantShop("Crabulon", "DecapoditaSprout", DownedCrabulon, Item.buyPrice(gold: 4));
			AddToMutantShop("HiveMind", "Teratoma", DownedHiveMind, Item.buyPrice(gold: 10));
			AddToMutantShop("Perforators", "BloodyWormFood", DownedPerfs, Item.buyPrice(gold: 10));
			AddToMutantShop("SlimeGod", "OverloadedSludge", DownedSlimeGod, Item.buyPrice(gold: 15));
			AddToMutantShop("Cryogen", "CryoKey", DownedCryogen, Item.buyPrice(gold: 15));
			AddToMutantShop("AquaticScourge", "Seafood", DownedAquaticScourge, Item.buyPrice(gold: 20));
			AddToMutantShop("BrimstoneElemental", "CharredIdol", DownedBrimstoneElemental, Item.buyPrice(gold: 20));
			AddToMutantShop("AstrumAureus", "AstralChunk", DownedAureus, Item.buyPrice(gold: 25));
			AddToMutantShop("PlaguebringerGoliath", "Abomination", DownedPBG, Item.buyPrice(gold: 50));
			AddToMutantShop("Ravager", "AncientMedallion", DownedRavager, Item.buyPrice(gold: 50));
			AddToMutantShop("ProfanedGuardians", "ProfanedShard", DownedGuardians, Item.buyPrice(platinum: 5));
			AddToMutantShop("Dragonfolly", "BirbPheromones", DownedBirb, Item.buyPrice(platinum: 5));
			AddToMutantShop("OldDuke", "BloodwormItem", DownedBoomerDuke, Item.buyPrice(platinum: 8));

			AddToAbomShop(InvasionDifficulty["AcidRainInitial"], "CausticTear", DownedAcidRainInitial, Item.buyPrice(gold: 3));
		}

		private static void CensusSupport()
		{
			
			Mod censusMod = ModLoader.HasMod("Census") ? ModLoader.GetMod("Census") : null;
			if (censusMod != null)
			{
				censusMod.Call("TownNPCCondition", NPCType<SEAHOE>(), "Defeat a Giant Clam after defeating the Desert Scourge");
				censusMod.Call("TownNPCCondition", NPCType<THIEF>(), "Have a [i:" + ItemID.PlatinumCoin + "] in your inventory after defeating Skeletron");
				censusMod.Call("TownNPCCondition", NPCType<FAP>(), "Have [i:" + ItemType<FabsolsVodka>() + "] in your inventory in Hardmode");
				censusMod.Call("TownNPCCondition", NPCType<DILF>(), "Defeat Cryogen");
			}
		}

		private static void SummonersAssociationSupport()
		{
			Mod sAssociation = ModLoader.HasMod("SummonersAssociation") ? ModLoader.GetMod("SummonersAssociation") : null;
			if (sAssociation is null)
				return;

			void RegisterSummon(int summonItem, int summonBuff, int summonProjectile)
			{
				sAssociation.Call("AddMinionInfo", summonItem, summonBuff, summonProjectile);
			}
			RegisterSummon(ItemType<SquirrelSquireStaff>(), BuffType<SquirrelSquireBuff>(), ProjectileType<SquirrelSquireMinion>());
			RegisterSummon(ItemType<WulfrumController>(), BuffType<WulfrumDroidBuff>(), ProjectileType<WulfrumDroid>());
			RegisterSummon(ItemType<SunSpiritStaff>(), BuffType<SolarSpirit>(), ProjectileType<SolarPixie>());
			RegisterSummon(ItemType<FrostBlossomStaff>(), BuffType<FrostBlossomBuff>(), ProjectileType<FrostBlossom>());
			RegisterSummon(ItemType<BelladonnaSpiritStaff>(), BuffType<BelladonnaSpiritBuff>(), ProjectileType<BelladonnaSpirit>());
			RegisterSummon(ItemType<StormjawStaff>(), BuffType<StormjawBuff>(), ProjectileType<StormjawBaby>());
			RegisterSummon(ItemType<RustyBeaconPrototype>(), BuffType<RustyDroneBuff>(), ProjectileType<RustyDrone>());
			RegisterSummon(ItemType<SeaboundStaff>(), BuffType<BrittleStar>(), ProjectileType<BrittleStarMinion>());
			RegisterSummon(ItemType<MagicalConch>(), BuffType<HermitCrab>(), ProjectileType<HermitCrabMinion>());
			RegisterSummon(ItemType<VileFeeder>(), BuffType<VileFeederBuff>(), ProjectileType<VileFeederSummon>());
			RegisterSummon(ItemType<ScabRipper>(), BuffType<ScabRipperBuff>(), ProjectileType<BabyBloodCrawler>());
			RegisterSummon(ItemType<CinderBlossomStaff>(), BuffType<CinderBlossomBuff>(), ProjectileType<CinderBlossom>());
			RegisterSummon(ItemType<BloodClotStaff>(), BuffType<BloodClot>(), ProjectileType<BloodClotMinion>());
			RegisterSummon(ItemType<DankStaff>(), BuffType<DankCreeperBuff>(), ProjectileType<DankCreeperMinion>());
			RegisterSummon(ItemType<StarSwallowerContainmentUnit>(), BuffType<StarSwallowerBuff>(), ProjectileType<StarSwallowerSummon>());
			RegisterSummon(ItemType<HerringStaff>(), BuffType<Herring>(), ProjectileType<HerringMinion>());
			RegisterSummon(ItemType<CorroslimeStaff>(), BuffType<Corroslime>(), ProjectileType<CorroslimeMinion>());
			RegisterSummon(ItemType<CrimslimeStaff>(), BuffType<Crimslime>(), ProjectileType<CrimslimeMinion>());
			RegisterSummon(ItemType<BlackHawkRemote>(), BuffType<BlackHawkBuff>(), ProjectileType<BlackHawkSummon>());
			RegisterSummon(ItemType<CausticStaff>(), BuffType<CausticStaffBuff>(), ProjectileType<CausticStaffSummon>());
			RegisterSummon(ItemType<AncientIceChunk>(), BuffType<IceClasper>(), ProjectileType<IceClasperMinion>());
			RegisterSummon(ItemType<ShellfishStaff>(), BuffType<ShellfishBuff>(), ProjectileType<Shellfish>());
			RegisterSummon(ItemType<HauntedScroll>(), BuffType<HauntedDishesBuff>(), ProjectileType<HauntedDishes>());
			RegisterSummon(ItemType<ForgottenApexWand>(), BuffType<ApexSharkBuff>(), ProjectileType<ApexShark>());
			RegisterSummon(ItemType<MountedScanner>(), BuffType<MountedScannerBuff>(), ProjectileType<MountedScannerSummon>());
			RegisterSummon(ItemType<DeepseaStaff>(), BuffType<AquaticStar>(), ProjectileType<AquaticStarMinion>());
			RegisterSummon(ItemType<SunGodStaff>(), BuffType<SolarSpiritGod>(), ProjectileType<SolarGod>());
			RegisterSummon(ItemType<TundraFlameBlossomsStaff>(), BuffType<TundraFlameBlossomsBuff>(), ProjectileType<TundraFlameBlossom>());
			RegisterSummon(ItemType<DormantBrimseeker>(), BuffType<DormantBrimseekerBuff>(), ProjectileType<DormantBrimseekerBab>());
			RegisterSummon(ItemType<IgneousExaltation>(), BuffType<IgneousExaltationBuff>(), ProjectileType<IgneousBlade>());
			RegisterSummon(ItemType<PlantationStaff>(), BuffType<PlantationBuff>(), ProjectileType<PlantSummon>());
			RegisterSummon(ItemType<SandSharknadoStaff>(), BuffType<Sandnado>(), ProjectileType<SandnadoMinion>());
			RegisterSummon(ItemType<GastricBelcherStaff>(), BuffType<GastricBelcherBuff>(), ProjectileType<GastricBelcher>());
			RegisterSummon(ItemType<FuelCellBundle>(), BuffType<FuelCellBundleBuff>(), ProjectileType<PlaguebringerMK2>());
			RegisterSummon(ItemType<GodspawnHelixStaff>(), BuffType<AstralProbeBuff>(), ProjectileType<AstralProbeSummon>());
			RegisterSummon(ItemType<TacticalPlagueEngine>(), BuffType<TacticalPlagueEngineBuff>(), ProjectileType<TacticalPlagueEngineSummon>());
			RegisterSummon(ItemType<ElementalAxe>(), BuffType<ElementalAxeBuff>(), ProjectileType<ElementalAxeMinion>());
			RegisterSummon(ItemType<SnakeEyes>(), BuffType<SnakeEyesBuff>(), ProjectileType<SnakeEyesSummon>());
			RegisterSummon(ItemType<DazzlingStabberStaff>(), BuffType<DazzlingStabberBuff>(), ProjectileType<DazzlingStabber>());
			RegisterSummon(ItemType<DragonbloodDisgorger>(), BuffType<BloodDragonsBuff>(), ProjectileType<SkeletalDragonMother>());
			RegisterSummon(ItemType<Cosmilamp>(), BuffType<CosmilampBuff>(), ProjectileType<CosmilampMinion>());
			RegisterSummon(ItemType<EtherealSubjugator>(), BuffType<Phantom>(), ProjectileType<PhantomGuy>());
			RegisterSummon(ItemType<CalamarisLament>(), BuffType<Calamari>(), ProjectileType<CalamariMinion>());
			RegisterSummon(ItemType<GammaHeart>(), BuffType<GammaHeadBuff>(), ProjectileType<GammaHead>());
			RegisterSummon(ItemType<CorvidHarbringerStaff>(), BuffType<CorvidHarbringerBuff>(), ProjectileType<PowerfulRaven>());
			RegisterSummon(ItemType<EndoHydraStaff>(), BuffType<EndoHydraBuff>(), ProjectileType<EndoHydraHead>());
			RegisterSummon(ItemType<CosmicViperEngine>(), BuffType<CosmicViperEngineBuff>(), ProjectileType<CosmicViperSummon>());
			RegisterSummon(ItemType<AngryChickenStaff>(), BuffType<YharonKindleBuff>(), ProjectileType<SonOfYharon>());
			RegisterSummon(ItemType<MidnightSunBeacon>(), BuffType<MidnightSunBuff>(), ProjectileType<MidnightSunUFO>());
			RegisterSummon(ItemType<PoleWarper>(), BuffType<PoleWarperBuff>(), ProjectileType<PoleWarperSummon>());
			RegisterSummon(ItemType<CosmicImmaterializer>(), BuffType<CosmicEnergy>(), ProjectileType<CosmicEnergySpiral>());
			RegisterSummon(ItemType<BensUmbrella>(), BuffType<MagicHatBuff>(), ProjectileType<MagicHat>());
			RegisterSummon(ItemType<Endogenesis>(), BuffType<EndoCooperBuff>(), ProjectileType<EndoCooperBody>());

			sAssociation.Call("AddMinionInfo", ItemType<BlightedEyeStaff>(), BuffType<CalamitasEyes>(), new List<int>() { ProjectileType<Calamitamini>(), ProjectileType<Cataclymini>(), ProjectileType<Catastromini>()}, new List<float>() {1-(1f/3f), 2f/3f, 2f/3f});
			//Entropy's Vigil is a bruh moment
			sAssociation.Call("AddMinionInfo", ItemType<ResurrectionButterfly>(), BuffType<ResurrectionButterflyBuff>(), new List<int>() { ProjectileType<PinkButterfly>(), ProjectileType<PurpleButterfly>()});
			sAssociation.Call("AddMinionInfo", ItemType<StaffoftheMechworm>(), BuffType<Mechworm>(), ProjectileType<MechwormBody>(), 1f);
		}
	}
}
