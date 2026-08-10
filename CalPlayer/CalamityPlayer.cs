using CalRD.Buffs;
using CalRD.Buffs.Cooldowns;
using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.Potions;
using CalRD.Buffs.StatBuffs;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Events;
using CalRD.Items.Accessories;
using CalRD.Items.Accessories.Vanity;
using CalRD.Items.Armor;
using CalRD.Items.DifficultyItems;
using CalRD.Items.Dyes;
using CalRD.Items.Mounts;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Typeless;
using CalRD.NPCs;
using CalRD.NPCs.Abyss;
using CalRD.NPCs.AcidRain;
using CalRD.NPCs.Astral;
using CalRD.NPCs.Calamitas;
using CalRD.NPCs.Crags;
using CalRD.NPCs.Cryogen;
using CalRD.NPCs.DevourerofGods;
using CalRD.NPCs.GreatSandShark;
using CalRD.NPCs.Leviathan;
using CalRD.NPCs.NormalNPCs;
using CalRD.NPCs.PlaguebringerGoliath;
using CalRD.NPCs.Polterghast;
using CalRD.NPCs.Providence;
using CalRD.NPCs.Ravager;
using CalRD.NPCs.StormWeaver;
using CalRD.NPCs.SulphurousSea;
using CalRD.NPCs.SunkenSea;
using CalRD.NPCs.SupremeCalamitas;
using CalRD.NPCs.Yharon;
using CalRD.Projectiles.Boss;
using CalRD.Projectiles.DraedonsArsenal;
using CalRD.Projectiles.Enemy;
using CalRD.Projectiles.Environment;
using CalRD.Projectiles.Melee;
using CalRD.Projectiles.Ranged;
using CalRD.Projectiles.Rogue;
using CalRD.Projectiles.Summon;
using CalRD.Projectiles.Typeless;
using CalRD.Tiles;
using CalRD.UI;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CalRD.BiomeManagers;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using SunkenSea = CalRD.World.SunkenSea;

namespace CalRD.CalPlayer
{
	public enum ClassType
	{
		Melee = 0,
		Ranged = 1,
		Magic = 2,
		Summon = 3,
		Rogue = 4
	}

	public enum GaelSwitchPhase
    {
        LoseRage = 0,
        None = 1
    }

    public enum AnimationType
    {
        Idle,
        Jump,
        Walk
    }

    public enum AndromedaPlayerState
    {
        Inactive,
        SmallRobot,
        LargeRobot,
        SpecialAttack
    }

    public class CalamityPlayer : ModPlayer
    {
        #region Variables

        #region No Category
        public static bool areThereAnyDamnBosses = false;
        public static bool areThereAnyDamnEvents = false;
        public bool drawBossHPBar = true;
        public bool shouldDrawSmallText = true;
        private const int saveVersion = 0;
        public int dashMod;
        public int projTypeJustHitBy;
        public int sCalDeathCount = 0;
        public int sCalKillCount = 0;
        public int deathCount = 0;
        public int actualMaxLife = 0;
        public int deathModeUnderworldTime = 0;
        public int deathModeBlizzardTime = 0;
        public static int chaosStateDuration = 360;
        public static int chaosStateDurationBoss = 600;
        public bool killSpikyBalls = false;
        public Projectile lastProjectileHit;
        public double acidRoundMultiplier = 1D;
        public int waterLeechTarget = -1;
        public float KameiTrailXScale = 0.1f;
        public int KameiBladeUseDelay = 0;
        public Vector2[] KameiOldPositions = new Vector2[4];
		public double trueMeleeDamage = 0D;
		public double contactDamageReduction = 0D;
		public double projectileDamageReduction = 0D;
		public bool brimlashBusterBoost = false;
		public float animusBoost = 1f;
		public int potionTimer = 0;
		public bool blockAllDashes = false;
		public bool resetHeightandWidth = false;
		public bool noLifeRegen = false;
        #endregion

        #region Tile Entity Trackers
        public int CurrentlyViewedFactoryID = -1;
        public int CurrentlyViewedChargerID = -1;
        public int CurrentlyViewedHologramID = -1;
        public string CurrentlyViewedHologramText;
        #endregion

        #region External variables -- Only set by Mod.Call
        public int externalAbyssLight = 0;
        public bool externalColdImmunity = false;
        public bool externalHeatImmunity = false;
		#endregion

		#region Town NPC Shop Variables
		public bool newMerchantInventory = false;
		public bool newPainterInventory = false;
		public bool newDyeTraderInventory = false;
		public bool newPartyGirlInventory = false;
		public bool newStylistInventory = false;
		public bool newDemolitionistInventory = false;
		public bool newDryadInventory = false;
		public bool newTavernkeepInventory = false;
		public bool newArmsDealerInventory = false;
		public bool newGoblinTinkererInventory = false;
		public bool newWitchDoctorInventory = false;
		public bool newClothierInventory = false;
		public bool newMechanicInventory = false;
		public bool newPirateInventory = false;
		public bool newTruffleInventory = false;
		public bool newWizardInventory = false;
		public bool newSteampunkerInventory = false;
		public bool newCyborgInventory = false;
		public bool newSkeletonMerchantInventory = false;
		public bool newPermafrostInventory = false;
		public bool newCirrusInventory = false;
		public bool newAmidiasInventory = false;
		public bool newBanditInventory = false;
		#endregion

		#region Stat Meter
		public int[] damageStats = new int[6];
        public int[] critStats = new int[4];
		public float actualMeleeDamageStat = 0f;
        public int defenseStat = 0;
        public int DRStat = 0;
        public int meleeSpeedStat = 0;
        public int manaCostStat = 0;
        public int rogueVelocityStat = 0;
        public int minionSlotStat = 0;
        public int lifeRegenStat = 0;
        public int manaRegenStat = 0;
        public int ammoReductionRanged = 0;
        public int ammoReductionRogue = 0;
        public int armorPenetrationStat = 0;
        public float wingFlightTimeStat = 0f;
		public float jumpSpeedStat = 0f;
        public int adrenalineDamageStat = 0;
		public int adrenalineDRStat = 0;
        public int rageDamageStat = 0;
        public int moveSpeedStat = 0;
        public int abyssLightLevelStat = 0;
        public int abyssBreathLossStat = 0;
        public int abyssBreathLossRateStat = 0;
        public int abyssLifeLostAtZeroBreathStat = 0;
        public int abyssDefenseLossStat = 0;
        public int stealthStat = 0;
        public float standingRegenStat = 0f;
        public float movingRegenStat = 0f;
        public float stealthUIAlpha = 1f;
        #endregion

        #region Timer and Counter
        public int bossRushImmunityFrameCurseTimer = 0;
        public int aBulwarkRareMeleeBoostTimer = 0;
        public int nebulaManaNerfCounter = 0;
        public int alcoholPoisonLevel = 0;
        public int modStealthTimer;
        public int dashTimeMod;
        public int hInfernoBoost = 0;
        public int pissWaterBoost = 0;
        public int gaelRageCooldown = 0;
        public int packetTimer = 0;
        public int navyRodAuraTimer = 0;
        public int brimLoreInfernoTimer = 0;
        public int tarraLifeAuraTimer = 0;
        public int bloodflareHeartTimer = 180;
        public int bloodflareManaTimer = 180;
        public int polarisBoostCounter = 0;
        public int gaelSwipes = 0;
        public float modStealth = 1f;
        public float aquaticBoost = 1f;
        public float shieldInvinc = 5f;
        public GaelSwitchPhase gaelSwitchTimer = 0;
        public int galileoCooldown = 0;
        public int soundCooldown = 0;
        public int planarSpeedBoost = 0;
        public int profanedSoulWeaponUsage = 0;
        public int profanedSoulWeaponType = 0;
        public int hurtSoundTimer = 0;
        public int danceOfLightCharge = 0;
        public int shadowPotCooldown = 0;
        public int dogTextCooldown = 0;
		public float auralisStealthCounter = 0f;
		public int auralisAuroraCounter = 0;
		public int auralisAuroraCooldown = 0;
		public int auralisAurora = 0;
		public int fungalSymbioteTimer = 0;
		public bool canFireReaverRangedProjectile = false;
		public bool canFireAtaxiaRangedProjectile = false;
		public bool canFireAtaxiaRogueProjectile = false;
		public bool canFireGodSlayerRangedProjectile = false;
		public bool canFireBloodflareMageProjectile = false;
		public bool canFireBloodflareRangedProjectile = false;
        #endregion

        #region Sound
        public bool playRogueStealthSound = false;
        public bool playFullRageSound = true;
        public bool playFullAdrenalineSound = true;
		#endregion

		#region Proficiency
		private const int levelTier1 = 1500;
		private const int levelTier2 = 5500;
		private const int levelTier3 = 12500;
		public int meleeLevel = 0;
        public int rangedLevel = 0;
        public int magicLevel = 0;
        public int rogueLevel = 0;
        public int summonLevel = 0;
        public bool shootFireworksLevelUpMelee = true;
        public bool shootFireworksLevelUpRanged = true;
        public bool shootFireworksLevelUpMagic = true;
        public bool shootFireworksLevelUpSummon = true;
        public bool shootFireworksLevelUpRogue = true;
        public int exactMeleeLevel = 0;
        public int exactRangedLevel = 0;
        public int exactMagicLevel = 0;
        public int exactSummonLevel = 0;
        public int exactRogueLevel = 0;
        public int gainLevelCooldown = 120;
        #endregion

        #region Rogue
        public float rogueStealth = 0f;
        public float rogueStealthMax = 0f;
        public float stealthGenStandstill = 1f;
        public float stealthGenMoving = 1f;
        public const float StealthAccelerationCap = 2f;
        public float stealthAcceleration = 1f;
        public bool stealthStrikeThisFrame = false;
        public bool stealthStrikeHalfCost = false;
        public bool stealthStrike75Cost = false;
        public bool stealthStrikeAlwaysCrits = false;
        public bool wearingRogueArmor = false;
        public float accStealthGenBoost = 0f;

        public float throwingDamage = 1f;
        public float throwingVelocity = 1f;
        public int throwingCrit = 0;
		public float throwingAmmoCost = 1f;
        #endregion

        #region Mount
        public bool onyxExcavator = false;
        public bool angryDog = false;
        public bool fab = false;
        public bool crysthamyr = false;
        public AndromedaPlayerState andromedaState;
        public int andromedaCripple;
        #endregion

        #region Pet
        public bool thirdSage = false;
        public bool thirdSageH = true; // Third sage healing
        public bool perfmini = false;
        public bool akato = false;
        public bool leviPet = false;
        public bool plaguebringerBab = false;
        public bool rotomPet = false;
        public bool ladShark = false;
        public int ladHearts = 0;
        public bool sparks = false;
        public bool sirenPet = false;
        public bool fox = false;
        public bool chibii = false;
        public bool brimling = false;
        public bool bearPet = false;
        public bool kendra = false;
        public bool trashMan = false;
        public int trashManChest = -1;
        public bool astrophage = false;
        public bool flakPet = false;
        public bool babyGhostBell = false;
        public bool radiator = false;
        public bool scalPet = false;
        public bool bendyPet = false;
        #endregion

        #region Rage
        public float rage = 0f;
        public float rageMax = 10000f;
        public const int RageDuration = 300;
        public const float AbsoluteRageThreshold = 0.98f; // 98% or higher for Absolute Rage
        public bool rageModeActive = false;
        public int gainRageCooldown = 60;
        #endregion

        #region Adrenaline
        public float adrenaline = 0f;
        public float adrenalineMax = 10000f;
        public const int AdrenalineDuration = 300;
        public bool adrenalineModeActive = false;
        #endregion

        #region Permanent Buff
        public bool extraAccessoryML = false;
        public bool eCore = false;
        public bool pHeart = false;
        public bool cShard = false;
        public bool mFruit = false;
        public bool bOrange = false;
        public bool eBerry = false;
        public bool dFruit = false;
        public bool revJamDrop = false;
        public bool rageBoostOne = false;
        public bool rageBoostTwo = false;
        public bool rageBoostThree = false;
        public bool adrenalineBoostOne = false;
        public bool adrenalineBoostTwo = false;
        public bool adrenalineBoostThree = false;
        public bool healToFull = false;
        #endregion

        #region Lore
        public bool kingSlimeLore = false;
        public bool desertScourgeLore = false;
        public bool crabulonLore = false;
        public bool eaterOfWorldsLore = false;
        public bool hiveMindLore = false;
        public bool perforatorLore = false;
        public bool queenBeeLore = false;
        public bool skeletronLore = false;
        // This lore boolean is a bit different from the others. It just stops Slime God lore effects from stacking.
        public bool slimeGodLore = false;
        public bool wallOfFleshLore = false;
        public bool twinsLore = false;
        public bool destroyerLore = false;
        public bool aquaticScourgeLore = false;
        public bool skeletronPrimeLore = false;
        public bool brimstoneElementalLore = false;
        public bool calamitasLore = false;
        public bool planteraLore = false;
        public bool leviathanAndSirenLore = false;
        public bool astrumAureusLore = false;
        public bool astrumDeusLore = false;
        public bool golemLore = false;
        public bool plaguebringerGoliathLore = false;
        public bool dukeFishronLore = false;
        public bool boomerDukeLore = false;
        public bool ravagerLore = false;
        public bool lunaticCultistLore = false;
        public bool moonLordLore = false;
        public bool providenceLore = false;
        public bool polterghastLore = false;
        public bool DoGLore = false;
        public bool yharonLore = false;
        public bool SCalLore = false;
        public bool oceanLore = false;
        public bool corruptionLore = false;
        public bool crimsonLore = false;
        public bool underworldLore = false;
        #endregion

        #region Accessory
        public bool fasterMeleeLevel = false;
        public bool fasterRangedLevel = false;
        public bool fasterMagicLevel = false;
        public bool fasterSummonLevel = false;
        public bool fasterRogueLevel = false;
        public bool luxorsGift = false;
        public bool fungalSymbiote = false;
        public bool trinketOfChi = false;
        public bool gladiatorSword = false;
        public bool unstablePrism = false;
        public bool regenator = false;
        public bool theBee = false;
        public int theBeeCooldown = 0;
        public bool alluringBait = false;
        public bool enchantedPearl = false;
        public bool fishingStation = false;
        public bool rBrain = false;
        public bool bloodyWormTooth = false;
        public bool afflicted = false;
        public bool affliction = false;
        public bool stressPills = false;
        public bool laudanum = false;
        public bool heartOfDarkness = false;
        public bool draedonsHeart = false;
        public bool rampartOfDeities = false;
        public bool vexation = false;
        public bool fBulwark = false;
        public bool dodgeScarf = false;
        public bool evasionScarf = false;
        public bool badgeOfBravery = false;
        public bool badgeOfBraveryRare = false;
        public bool scarfCooldown = false;
        public bool eScarfCooldown = false;
        public bool cryogenSoul = false;
        public bool yInsignia = false;
        public bool eGauntlet = false;
        public bool eTalisman = false;
        public bool statisBeltOfCurses = false;
        public int statisTimer = 0;
        public bool nucleogenesis = false;
        public bool nuclearRod = false;
        public bool elysianAegis = false;
        public bool elysianGuard = false;
        public bool nCore = false;
        public bool deepDiver = false;
        public bool abyssalDivingSuitPlates = false;
        public bool abyssalDivingSuitCooldown = false;
        public int abyssalDivingSuitPlateHits = 0;
        public bool sirenWaterBuff = false;
        public bool sirenIce = false;
        public bool sirenIceCooldown = false;
        public bool aSpark = false;
        public bool aSparkRare = false;
        public bool aBulwark = false;
        public bool aBulwarkRare = false;
        public bool dAmulet = false;
        public bool fCarapace = false;
        public bool gShell = false;
        public bool seaShell = false;
        public bool absorber = false;
        public bool aAmpoule = false;
        public bool rOoze = false;
        public bool pAmulet = false;
        public bool fBarrier = false;
        public bool aBrain = false;
        public bool amalgam = false;
        public bool lol = false;
        public bool raiderTalisman = false;
        public int raiderStack = 0;
        public int raiderCooldown = 0;
        public bool gSabaton = false;
        public int gSabatonFall = 0;
        public int gSabatonCooldown = 0;
        public bool sGenerator = false;
        public bool sDefense = false;
        public bool sPower = false;
        public bool sRegen = false;
        public bool IBoots = false;
        public bool elysianFire = false;
        public bool sTracers = false;
        public bool eTracers = false;
        public bool cTracers = false;
        public bool frostFlare = false;
        public bool beeResist = false;
        public bool uberBees = false;
        public bool projRef = false;
        public bool projRefRare = false;
        public int projRefRareLifeRegenCounter = 0;
        public bool nanotech = false;
        public bool eQuiver = false;
        public bool shadowMinions = false;
        public bool tearMinions = false;
        public bool alchFlask = false;
        public bool reducedPlagueDmg = false;
		public bool abaddon = false;
        public bool community = false;
        public bool fleshTotem = false;
        public bool fleshTotemCooldown = false;
        public bool bloodPact = false;
		public bool bloodPactBoost = false;
        public bool bloodflareCore = false;
        public int bloodflareCoreLostDefense = 0;
        public bool coreOfTheBloodGod = false;
        public bool elementalHeart = false;
        public bool crownJewel = false;
        public bool celestialJewel = false;
        public bool astralArcanum = false;
        public bool harpyRing = false;
        public bool harpyWingBoost = false; //harpy wings + harpy ring
        public bool ironBoots = false;
        public bool depthCharm = false;
        public bool anechoicPlating = false;
        public bool jellyfishNecklace = false;
		public bool abyssDivingGear = false;
        public bool abyssalAmulet = false;
        public bool lumenousAmulet = false;
        public bool aquaticEmblem = false;
        public bool darkSunRing = false;
        public bool calamityRing = false;
        public bool voidOfExtinction = false;
        public bool eArtifact = false;
        public bool dArtifact = false;
        public bool gArtifact = false;
        public bool pArtifact = false;
        public bool giantPearl = false;
        public bool normalityRelocator = false;
        public bool fabledTortoise = false;
        public bool manaOverloader = false;
        public bool royalGel = false;
        public bool handWarmer = false;
        public bool oldDie = false;
        public bool ursaSergeant = false;
        public bool scuttlersJewel = false;
        public bool thiefsDime = false;
        public bool dynamoStemCells = false;
        public bool etherealExtorter = false;
        public bool blazingCore = false;
        public bool voltaicJelly = false;
        public bool jellyChargedBattery = false;
        public float jellyDmg;
        public bool dukeScales = false;
        public bool sandWaifu = false;
        public bool sandBoobWaifu = false;
        public bool cloudWaifu = false;
        public bool brimstoneWaifu = false;
        public bool sirenWaifu = false;
        public bool fungalClump = false;
        public bool howlsHeart = false;
        public bool darkGodSheath = false;
        public bool inkBomb = false;
        public bool inkBombCooldown = false;
        public bool abyssalMirror = false;
        public bool abyssalMirrorCooldown = false;
        public bool eclipseMirror = false;
        public bool eclipseMirrorCooldown = false;
        public bool featherCrown = false;
        public bool moonCrown = false;
        public int featherCrownCooldown = 0;
        public int moonCrownCooldown = 0;
        public int nanoFlareCooldown = 0;
        public bool dragonScales = false;
        public bool gloveOfPrecision = false;
        public bool gloveOfRecklessness = false;
        public bool momentumCapacitor = false;
        public bool vampiricTalisman = false;
        public bool electricianGlove = false;
        public bool bloodyGlove = false;
        public bool filthyGlove = false;
        public bool sandCloak = false;
        public bool sandCloakCooldown = false;
        public bool spectralVeil = false;
        public int spectralVeilImmunity = 0;
        public bool hasJetpack = false;
        public int jetPackCooldown = 0;
        public bool plaguedFuelPack = false;
        public int plaguedFuelPackDash = 0;
        public int plaguedFuelPackDirection = 0;
        public bool blunderBooster = false;
        public int blunderBoosterDash = 0;
        public int blunderBoosterDirection = 0;
        public bool veneratedLocket = false;
        public bool camper = false;
        public bool corrosiveSpine = false;
        public bool miniOldDuke = false;
        public bool starbusterCore = false;
        public bool starTaintedGenerator = false;
        public bool hallowedRune = false;
		public int hallowedRuneCooldown = 0;
		public bool silvaWings = false;
		public int icicleCooldown = 0;
        public bool rustyMedal = false;
        public bool noStupidNaturalARSpawns = false;
        public bool burdenBreakerYeet = false;
		public bool roverDrive = false;
		public int roverDriveTimer = 0;
		public int roverFrameCounter = 0;
		public int roverFrame = 0;
        #endregion

        #region Armor Set
        public bool desertProwler = false;
        public bool snowRuffianSet = false;
        public bool forbiddenCirclet = false;
		public int forbiddenCooldown = 0;
		public int tornadoCooldown = 0;
        public bool eskimoSet = false; //vanilla armor
        public bool meteorSet = false; //vanilla armor, for space gun nerf
        public bool victideSet = false;
        public bool sulfurSet = false;
        public bool sulfurJump = false;
		public bool jumpAgainSulfur = false;
		public int sulphurBubbleCooldown = 0;
        public bool aeroSet = false;
        public bool statigelSet = false;
        public bool statigelJump = false;
		public bool jumpAgainStatigel = false;
        public bool tarraSet = false;
        public bool tarraMelee = false;
        public bool tarragonCloak = false;
        public bool tarragonCloakCooldown = false;
        public int tarraDefenseTime = 600;
        public bool tarraMage = false;
        public int tarraMageHealCooldown = 0;
        public int tarraCrits = 0;
        public bool tarraRanged = false;
        public bool tarraThrowing = false;
        public bool tarragonImmunityCooldown = false;
        public bool tarragonImmunity = false;
        public int tarraThrowingCrits = 0;
        public bool tarraSummon = false;
        public bool bloodflareSet = false;
        public bool bloodflareMelee = false;
        public bool bloodflareFrenzy = false;
        public bool bloodFrenzyCooldown = false;
        public int bloodflareMeleeHits = 0;
        public bool bloodflareRanged = false;
        public bool bloodflareSoulCooldown = false;
        public int bloodflareSoulTimer = 0;
        public bool bloodflareThrowing = false;
        public bool bloodflareMage = false;
        public int bloodflareMageCooldown = 0;
        public bool bloodflareSummon = false;
        public int bloodflareSummonTimer = 0;
        public bool godSlayer = false;
        public bool godSlayerDamage = false;
        public bool godSlayerMage = false;
        public bool godSlayerRanged = false;
        public bool godSlayerThrowing = false;
        public bool godSlayerSummon = false;
        public float godSlayerDmg;
        public bool godSlayerReflect = false;
        public bool godSlayerCooldown = false;
        public bool ataxiaBolt = false;
        public bool ataxiaFire = false;
        public bool ataxiaVolley = false;
        public bool ataxiaBlaze = false;
        public bool hydrothermalSmoke = false;
        public bool daedalusAbsorb = false;
        public bool daedalusShard = false;
        public bool brimflameSet = false;
        public bool brimflameFrenzy = false;
        public bool brimflameFrenzyCooldown = false;
        public int brimflameFrenzyTimer = 0;
        public bool reaverSpore = false;
        public bool reaverDoubleTap = false;
        public bool flamethrowerBoost = false;
        public bool hoverboardBoost = false; //hoverboard + shroomite visage
        public bool shadeRegen = false;
        public bool shadowSpeed = false;
        public bool dsSetBonus = false;
        public bool auricBoost = false;
        public bool daedalusReflect = false;
        public bool daedalusSplit = false;
        public bool titanHeartSet = false;
        public bool titanHeartMask = false;
        public bool titanHeartMantle = false;
        public bool titanHeartBoots = false;
        public int titanCooldown = 0;
        public bool umbraphileSet = false;
        public bool reaverBlast = false;
        public bool reaverBurst = false;
        public bool fathomSwarmer = false;
        public bool fathomSwarmerVisage = false;
        public bool fathomSwarmerBreastplate = false;
        public bool fathomSwarmerTail = false;
        public int tailFrameUp = 0;
		public int tailFrame = 0;
        public bool astralStarRain = false;
        public int astralStarRainCooldown = 0;
        public bool plagueReaper = false;
        public int plagueReaperCooldown = 0;
        public bool plaguebringerPatronSet = false;
        public bool plaguebringerCarapace = false;
        public bool plaguebringerPistons = false;
        public int pistonsCounter = 0;
        public float ataxiaDmg;
        public bool ataxiaMage = false;
        public bool ataxiaGeyser = false;
        public float xerocDmg;
        public bool xerocSet = false;
        public bool prismaticSet = false;
        public bool prismaticHelmet = false;
        public bool prismaticRegalia = false;
        public bool prismaticGreaves = false;
		public int prismaticLasers = 0;
        public bool silvaSet = false;
        public bool silvaMelee = false;
        public bool silvaRanged = false;
        public bool silvaThrowing = false;
        public bool silvaMage = false;
        public bool silvaSummon = false;
        public bool hasSilvaEffect = false;
        public int silvaCountdown = 600;
        public int silvaHitCounter = 0;
        public bool auricSet = false;
        public bool omegaBlueChestplate = false;
        public bool omegaBlueSet = false;
        public bool omegaBlueHentai = false;
        public int omegaBlueCooldown = 0;
        public bool urchin = false;
        public bool valkyrie = false;
        public bool slimeGod = false;
        public bool molluskSet = false;
        public bool fearmongerSet = false;
        public int fearmongerRegenFrames = 0;
        public bool daedalusCrystal = false;
        public bool reaverOrb = false;
        public bool chaosSpirit = false;
        public bool redDevil = false;
        #endregion

        #region Debuff
        public bool alcoholPoisoning = false;
        public bool shadowflame = false;
        public bool wDeath = false;
        public bool lethalLavaBurn = false;
        public bool aCrunch = false;
        public bool absoluteRage = false;
        public bool irradiated = false;
        public bool bFlames = false;
        public bool aFlames = false;
        public bool gsInferno = false;
        public bool astralInfection = false;
        public bool pFlames = false;
        public bool hFlames = false;
        public bool hInferno = false;
        public bool gState = false;
        public bool bBlood = false;
        public bool eGravity = false;
        public bool weakPetrification = false;
        public bool vHex = false;
        public bool eGrav = false;
        public bool warped = false;
        public bool cDepth = false;
        public bool fishAlert = false;
        public bool bOut = false;
        public bool clamity = false;
        public bool sulphurPoison = false;
        public bool nightwither = false;
        public bool eFreeze = false;
        public bool silvaStun = false;
        public bool wCleave = false;
        public bool eutrophication = false;
        public bool iCantBreathe = false; //Frozen Lungs debuff
        public bool cragsLava = false;
        public bool vaporfied = false;
        public bool energyShellCooldown = false;
        public bool prismaticCooldown = false;
        public bool waterLeechBleeding = false;
        #endregion

        #region Buff
        public bool trinketOfChiBuff = false;
        public int chiBuffTimer = 0;
        public bool corrEffigy = false;
        public bool crimEffigy = false;
        public bool decayEffigy = false;
        public bool rRage = false;
        public bool tRegen = false;
        public bool xRage = false;
        public bool xWrath = false;
        public bool graxDefense = false;
        public bool encased = false;
        public bool sMeleeBoost = false;
        public bool eScarfBoost = false;
        public bool tFury = false;
        public bool cadence = false;
        public bool omniscience = false;
        public bool zerg = false;
        public bool zen = false;
        public bool bossZen = false;
        public bool yPower = false;
        public bool aWeapon = false;
        public bool tScale = false;
        public int titanBoost = 0;
        public bool fabsolVodka = false;
        public bool mushy = false;
        public bool molten = false;
        public bool shellBoost = false;
        public bool cFreeze = false;
        public bool invincible = false;
        public bool shine = false;
        public bool anechoicCoating = false;
        public bool enraged = false;
        public bool revivify = false;
        public bool permafrostsConcoction = false;
        public bool armorCrumbling = false;
        public bool armorShattering = false;
        public bool ceaselessHunger = false;
        public bool calcium = false;
        public bool soaring = false;
        public bool bounding = false;
        public bool triumph = false;
        public bool penumbra = false;
        public bool shadow = false;
        public bool photosynthesis = false;
        public bool astralInjection = false;
        public bool gravityNormalizer = false;
        public bool holyWrath = false;
        public bool profanedRage = false;
        public bool draconicSurge = false;
        public bool draconicSurgeCooldown = false;
        public bool tesla = false;
        public bool teslaFreeze = false;
        public bool sulphurskin = false;
        public bool baguette = false;
        public bool vodka = false;
        public bool redWine = false;
        public bool grapeBeer = false;
        public bool moonshine = false;
        public bool rum = false;
        public bool whiskey = false;
        public bool fireball = false;
        public bool everclear = false;
        public bool bloodyMary = false;
        public bool tequila = false;
        public bool caribbeanRum = false;
        public bool cinnamonRoll = false;
        public bool tequilaSunrise = false;
        public bool margarita = false;
        public bool starBeamRye = false;
        public bool screwdriver = false;
        public bool moscowMule = false;
        public bool whiteWine = false;
        public bool evergreenGin = false;
        public bool tranquilityCandle = false;
        public bool chaosCandle = false;
        public bool purpleCandle = false;
        public bool blueCandle = false;
        public bool pinkCandle = false;
        public double pinkCandleHealFraction = 0D;
        public bool yellowCandle = false;
        public bool trippy = false;
        public bool amidiasBlessing = false;
        public bool polarisBoost = false;
        public bool polarisBoostTwo = false;
        public bool polarisBoostThree = false;
        public bool bloodfinBoost = false;
        public int bloodfinTimer = 30;
        public bool hallowedDefense = false;
        public bool hallowedPower = false;
        public bool hallowedRegen = false;
        public bool kamiBoost = false;
        #endregion

        #region Minion
        public bool wDroid = false;
        public bool resButterfly = false;
        public bool glSword = false;
        public bool mWorm = false;
        public bool iClasper = false;
        public bool magicHat = false;
        public bool herring = false;
        public bool blackhawk = false;
        public bool cosmicViper = false;
        public bool calamari = false;
        public bool cEyes = false;
        public bool cSlime = false;
        public bool cSlime2 = false;
        public bool aSlime = false;
        public bool bStar = false;
        public bool aStar = false;
        public bool SP = false;
        public bool dCreeper = false;
        public bool bClot = false;
        public bool eAxe = false;
        public bool endoCooper = false;
        public bool SPG = false;
        public bool sirius = false;
        public bool aChicken = false;
        public bool cLamp = false;
        public bool pGuy = false;
        public bool sandnado = false;
        public bool plantera = false;
        public bool aProbe = false;
        public bool gDefense = false;
        public bool gOffense = false;
        public bool gHealer = false;
        public bool cEnergy = false;
        public int healCounter = 300;
        public bool shellfish = false;
        public bool hCrab = false;
        public bool tDime = false;
        public bool allWaifus = false;
        public bool sCrystal = false;
        public bool sWaifu = false;
        public bool dWaifu = false;
        public bool cWaifu = false;
        public bool bWaifu = false;
        public bool slWaifu = false;
        public bool fClump = false;
        public bool rDevil = false;
        public bool aValkyrie = false;
        public bool apexShark = false;
        public bool gastricBelcher = false;
        public bool squirrel = false;
        public bool hauntedDishes = false;
        public bool stormjaw = false;
        public bool sGod = false;
        public bool vUrchin = false;
        public bool cSpirit = false;
        public bool rOrb = false;
        public bool dCrystal = false;
        public bool endoHydra = false;
        public bool powerfulRaven = false;
        public bool dragonFamily = false;
        public bool providenceStabber = false;
        public bool radiantResolution = false;
        public bool plaguebringerMK2 = false;
        public bool igneousExaltation = false;
        public bool coldDivinity = false;
        public bool youngDuke = false;
        public bool virili = false;
        public bool frostBlossom = false;
        public bool cinderBlossom = false;
        public bool belladonaSpirit = false;
        public bool vileFeeder = false;
        public bool scabRipper = false;
        public bool midnightUFO = false;
        public bool plagueEngine = false;
        public bool brimseeker = false;
        public bool necrosteocytesDudes = false;
        public bool gammaHead = false;
        public List<int> GammaCanisters = new List<int>();
        public bool rustyDrone = false;
        public bool tundraFlameBlossom = false;
        public bool starSwallowerPetFroge = false;
        public bool snakeEyes = false;
        public bool poleWarper = false;
        public bool causticDragon = false;
        public bool plaguebringerPatronSummon = false;
        public bool howlTrio = false;
        public bool mountedScanner = false;
        #endregion

        #region Biome
        public bool ZoneCalamity => Player.InModBiome(ModContent.GetInstance<Crag>());
        public bool ZoneAstral => Player.InModBiome(ModContent.GetInstance<Astral>());
        public bool ZoneSunkenSea => Player.InModBiome(ModContent.GetInstance<BiomeManagers.SunkenSea>());
        public bool ZoneSulphur => Player.InModBiome(ModContent.GetInstance<Sulphur>());
        public bool ZoneAbyss => ZoneAbyssLayer1 || ZoneAbyssLayer2 || ZoneAbyssLayer3 || ZoneAbyssLayer4;
        public bool ZoneAbyssLayer1 => Player.InModBiome(ModContent.GetInstance<AbyssLayer1Biome>());
        public bool ZoneAbyssLayer2 => Player.InModBiome(ModContent.GetInstance<AbyssLayer2Biome>());
        public bool ZoneAbyssLayer3 => Player.InModBiome(ModContent.GetInstance<AbyssLayer3Biome>());
        public bool ZoneAbyssLayer4 => Player.InModBiome(ModContent.GetInstance<AbyssLayer4Biome>());
        public bool abyssDeath = false;
        public int abyssBreathCD;
        public float caveDarkness = 0f;
        #endregion

        #region Transformation
        public bool abyssalDivingSuitPrevious;
        public bool abyssalDivingSuit;
        public bool abyssalDivingSuitHide;
        public bool abyssalDivingSuitForce;
        public bool abyssalDivingSuitPower;
        public bool profanedCrystal;
        public bool profanedCrystalPrevious;
        public bool profanedCrystalForce;
        public bool profanedCrystalBuffs;
        public bool profanedCrystalHide;
        public KeyValuePair<int, int> profanedCrystalWingCounter = new KeyValuePair<int, int>(0, 10);
        public KeyValuePair<int, int> profanedCrystalAnimCounter = new KeyValuePair<int, int>(0, 10);
        public bool sirenBoobsPrevious;
        public bool sirenBoobs;
        public bool sirenBoobsHide;
        public bool sirenBoobsForce;
        public bool sirenBoobsPower;
        public bool snowmanPrevious;
        public bool snowman;
        public bool snowmanHide;
        public bool snowmanForce;
        public bool snowmanNoseless;
        public bool snowmanPower;
        public bool meldTransformationPrevious;
        public bool meldTransformation;
        public bool meldTransformationForce;
        public bool meldTransformationPower;
        public bool omegaBlueTransformationPrevious;
        public bool omegaBlueTransformation;
        public bool omegaBlueTransformationForce;
        public bool omegaBlueTransformationPower;
        #endregion

        #endregion

        #region SavingAndLoading
        public override void Initialize()
		{
			extraAccessoryML = false;
			eCore = false;
			mFruit = false;
			bOrange = false;
			eBerry = false;
			dFruit = false;
			pHeart = false;
			cShard = false;
			revJamDrop = false;
			rageBoostOne = false;
			rageBoostTwo = false;
			rageBoostThree = false;
			adrenalineBoostOne = false;
			adrenalineBoostTwo = false;
			adrenalineBoostThree = false;
			drawBossHPBar = true;
			shouldDrawSmallText = true;
			healToFull = false;

			newMerchantInventory = false;
			newPainterInventory = false;
			newDyeTraderInventory = false;
			newPartyGirlInventory = false;
			newStylistInventory = false;
			newDemolitionistInventory = false;
			newDryadInventory = false;
			newTavernkeepInventory = false;
			newArmsDealerInventory = false;
			newGoblinTinkererInventory = false;
			newWitchDoctorInventory = false;
			newClothierInventory = false;
			newMechanicInventory = false;
			newPirateInventory = false;
			newTruffleInventory = false;
			newWizardInventory = false;
			newSteampunkerInventory = false;
			newCyborgInventory = false;
			newSkeletonMerchantInventory = false;
			newPermafrostInventory = false;
			newCirrusInventory = false;
			newAmidiasInventory = false;
			newBanditInventory = false;
		}

        public override void SaveData(TagCompound tag)/* tModPorter Suggestion: Edit tag parameter instead of returning new TagCompound */
        {
            var boost = new List<string>();
            boost.AddWithCondition("extraAccessoryML", extraAccessoryML);
            boost.AddWithCondition("etherealCore", eCore);
            boost.AddWithCondition("miracleFruit", mFruit);
            boost.AddWithCondition("bloodOrange", bOrange);
            boost.AddWithCondition("elderBerry", eBerry);
            boost.AddWithCondition("dragonFruit", dFruit);
            boost.AddWithCondition("phantomHeart", pHeart);
            boost.AddWithCondition("cometShard", cShard);
            boost.AddWithCondition("revJam", revJamDrop);
            boost.AddWithCondition("rageOne", rageBoostOne);
            boost.AddWithCondition("rageTwo", rageBoostTwo);
            boost.AddWithCondition("rageThree", rageBoostThree);
            boost.AddWithCondition("adrenalineOne", adrenalineBoostOne);
            boost.AddWithCondition("adrenalineTwo", adrenalineBoostTwo);
            boost.AddWithCondition("adrenalineThree", adrenalineBoostThree);
            boost.AddWithCondition("bossHPBar", drawBossHPBar);
            boost.AddWithCondition("drawSmallText", shouldDrawSmallText);
            boost.AddWithCondition("fullHPRespawn", healToFull);

			boost.AddWithCondition("newMerchantInventory", newMerchantInventory);
			boost.AddWithCondition("newPainterInventory", newPainterInventory);
			boost.AddWithCondition("newDyeTraderInventory", newDyeTraderInventory);
			boost.AddWithCondition("newPartyGirlInventory", newPartyGirlInventory);
			boost.AddWithCondition("newStylistInventory", newStylistInventory);
			boost.AddWithCondition("newDemolitionistInventory", newDemolitionistInventory);
			boost.AddWithCondition("newDryadInventory", newDryadInventory);
			boost.AddWithCondition("newTavernkeepInventory", newTavernkeepInventory);
			boost.AddWithCondition("newArmsDealerInventory", newArmsDealerInventory);
			boost.AddWithCondition("newGoblinTinkererInventory", newGoblinTinkererInventory);
			boost.AddWithCondition("newWitchDoctorInventory", newWitchDoctorInventory);
			boost.AddWithCondition("newClothierInventory", newClothierInventory);
			boost.AddWithCondition("newMechanicInventory", newMechanicInventory);
			boost.AddWithCondition("newPirateInventory", newPirateInventory);
			boost.AddWithCondition("newTruffleInventory", newTruffleInventory);
			boost.AddWithCondition("newWizardInventory", newWizardInventory);
			boost.AddWithCondition("newSteampunkerInventory", newSteampunkerInventory);
			boost.AddWithCondition("newCyborgInventory", newCyborgInventory);
			boost.AddWithCondition("newSkeletonMerchantInventory", newSkeletonMerchantInventory);
			boost.AddWithCondition("newPermafrostInventory", newPermafrostInventory);
			boost.AddWithCondition("newCirrusInventory", newCirrusInventory);
			boost.AddWithCondition("newAmidiasInventory", newAmidiasInventory);
			boost.AddWithCondition("newBanditInventory", newBanditInventory);

			tag["boost"] = boost;
			tag["stress"] = rage;
			tag["adrenaline"] = adrenaline;
			tag["sCalDeathCount"] = sCalDeathCount;
			tag["sCalKillCount"] = sCalKillCount;
			tag["meleeLevel"] = meleeLevel;
			tag["exactMeleeLevel"] = exactMeleeLevel;
			tag["rangedLevel"] = rangedLevel;
			tag["exactRangedLevel"] = exactRangedLevel;
			tag["magicLevel"] = magicLevel;
			tag["exactMagicLevel"] = exactMagicLevel;
			tag["summonLevel"] = summonLevel;
			tag["exactSummonLevel"] = exactSummonLevel;
			tag["rogueLevel"] = rogueLevel;
			tag["exactRogueLevel"] = exactRogueLevel;
			tag["deathCount"] = deathCount;
			tag["deathModeUnderworldTime"] = deathModeUnderworldTime;
			tag["deathModeBlizzardTime"] = deathModeBlizzardTime;
        }

        public override void LoadData(TagCompound tag)
        {
            var boost = tag.GetList<string>("boost");
            extraAccessoryML = boost.Contains("extraAccessoryML");
            eCore = boost.Contains("etherealCore");
            mFruit = boost.Contains("miracleFruit");
            bOrange = boost.Contains("bloodOrange");
            eBerry = boost.Contains("elderBerry");
            dFruit = boost.Contains("dragonFruit");
            pHeart = boost.Contains("phantomHeart");
            cShard = boost.Contains("cometShard");
            revJamDrop = boost.Contains("revJam");
            rageBoostOne = boost.Contains("rageOne");
            rageBoostTwo = boost.Contains("rageTwo");
            rageBoostThree = boost.Contains("rageThree");
            adrenalineBoostOne = boost.Contains("adrenalineOne");
            adrenalineBoostTwo = boost.Contains("adrenalineTwo");
            adrenalineBoostThree = boost.Contains("adrenalineThree");
            drawBossHPBar = boost.Contains("bossHPBar");
            shouldDrawSmallText = boost.Contains("drawSmallText");
            healToFull = boost.Contains("fullHPRespawn");

			newMerchantInventory = boost.Contains("newMerchantInventory");
			newPainterInventory = boost.Contains("newPainterInventory");
			newDyeTraderInventory = boost.Contains("newDyeTraderInventory");
			newPartyGirlInventory = boost.Contains("newPartyGirlInventory");
			newStylistInventory = boost.Contains("newStylistInventory");
			newDemolitionistInventory = boost.Contains("newDemolitionistInventory");
			newDryadInventory = boost.Contains("newDryadInventory");
			newTavernkeepInventory = boost.Contains("newTavernkeepInventory");
			newArmsDealerInventory = boost.Contains("newArmsDealerInventory");
			newGoblinTinkererInventory = boost.Contains("newGoblinTinkererInventory");
			newWitchDoctorInventory = boost.Contains("newWitchDoctorInventory");
			newClothierInventory = boost.Contains("newClothierInventory");
			newMechanicInventory = boost.Contains("newMechanicInventory");
			newPirateInventory = boost.Contains("newPirateInventory");
			newTruffleInventory = boost.Contains("newTruffleInventory");
			newWizardInventory = boost.Contains("newWizardInventory");
			newSteampunkerInventory = boost.Contains("newSteampunkerInventory");
			newCyborgInventory = boost.Contains("newCyborgInventory");
			newSkeletonMerchantInventory = boost.Contains("newSkeletonMerchantInventory");
			newPermafrostInventory = boost.Contains("newPermafrostInventory");
			newCirrusInventory = boost.Contains("newCirrusInventory");
			newAmidiasInventory = boost.Contains("newAmidiasInventory");
			newBanditInventory = boost.Contains("newBanditInventory");

			rage = tag.GetAsInt("stress");
            adrenaline = tag.GetAsInt("adrenaline");
            sCalDeathCount = tag.GetInt("sCalDeathCount");
            sCalKillCount = tag.GetInt("sCalKillCount");
            deathCount = tag.GetInt("deathCount");

            // These two variables are no longer used, as the code was moved into CalamityWorld.cs to support multiplayer.
            // As a result, their values are simply fed into a discard.

            _ = tag.GetInt("moneyStolenByBandit");
            _ = tag.GetInt("reforges");

            deathModeUnderworldTime = tag.GetInt("deathModeUnderworldTime");
            deathModeBlizzardTime = tag.GetInt("deathModeBlizzardTime");

            meleeLevel = tag.GetInt("meleeLevel");
            rangedLevel = tag.GetInt("rangedLevel");
            magicLevel = tag.GetInt("magicLevel");
            summonLevel = tag.GetInt("summonLevel");
            rogueLevel = tag.GetInt("rogueLevel");
            exactMeleeLevel = tag.GetInt("exactMeleeLevel");
            exactRangedLevel = tag.GetInt("exactRangedLevel");
            exactMagicLevel = tag.GetInt("exactMagicLevel");
            exactSummonLevel = tag.GetInt("exactSummonLevel");
            exactRogueLevel = tag.GetInt("exactRogueLevel");
        }
        #endregion

        #region ResetEffects
        public override void ResetEffects()
        {
            // Max health bonuses
            if (absorber)
                Player.statLifeMax2 += 20;
            Player.statLifeMax2 +=
                (mFruit ? 25 : 0) +
                (bOrange ? 25 : 0) +
                (eBerry ? 25 : 0) +
                (dFruit ? 25 : 0);
            if (ZoneAbyss && abyssalAmulet)
                Player.statLifeMax2 += Player.statLifeMax2 / 5 / 20 * (lumenousAmulet ? 25 : 10);
            if (coreOfTheBloodGod)
                Player.statLifeMax2 += Player.statLifeMax2 / 5 / 20 * 10;
            if (bloodPact)
                Player.statLifeMax2 += Player.statLifeMax2 / 5 / 20 * 100;
            if (leviathanAndSirenLore)
            {
                if (sirenBoobsPrevious)
                    Player.statLifeMax2 += Player.statLifeMax2 / 5 / 20 * 5;
            }
            if (absoluteRage)
                Player.statLifeMax2 += Player.statLifeMax / 5 / 20 * 5;
            if (affliction || afflicted)
                Player.statLifeMax2 += Player.statLifeMax / 5 / 20 * 10;
            if (cadence)
                Player.statLifeMax2 += Player.statLifeMax / 5 / 20 * 25;
            if (community)
            {
                float floatTypeBoost = 0.05f +
                    (NPC.downedSlimeKing ? 0.01f : 0f) +
                    (NPC.downedBoss1 ? 0.01f : 0f) +
                    (NPC.downedBoss2 ? 0.01f : 0f) +
                    (NPC.downedQueenBee ? 0.01f : 0f) +
                    (NPC.downedBoss3 ? 0.01f : 0f) + // 0.1
					(Main.hardMode ? 0.01f : 0f) +
                    (NPC.downedMechBossAny ? 0.01f : 0f) +
                    (NPC.downedPlantBoss ? 0.01f : 0f) +
                    (NPC.downedGolemBoss ? 0.01f : 0f) +
                    (NPC.downedFishron ? 0.01f : 0f) + // 0.15
					(NPC.downedAncientCultist ? 0.01f : 0f) +
                    (NPC.downedMoonlord ? 0.01f : 0f) +
                    (CalamityWorld.downedProvidence ? 0.01f : 0f) +
                    (CalamityWorld.downedDoG ? 0.01f : 0f) +
                    (CalamityWorld.downedYharon ? 0.01f : 0f); // 0.2
                int integerTypeBoost = (int)(floatTypeBoost * 50f);
                Player.statLifeMax2 += Player.statLifeMax / 5 / 20 * integerTypeBoost;
            }

            // Max health reductions
            if (crimEffigy)
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.8);
            if (regenator)
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.5);
            if (skeletronLore)
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.9);
            if (calamitasLore)
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.75);
            if (providenceLore)
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.8);

            // Extra accessory slots
			// This is probably fucked in 1.4
            if (extraAccessoryML)
                Player.extraAccessorySlots = 1;
            if (extraAccessoryML && Player.extraAccessory && (Main.expertMode || Main.gameMenu))
                Player.extraAccessorySlots = 2;
            if (BossRushEvent.BossRushActive)
            {
                if (CalamityConfig.Instance.BossRushAccessoryCurse)
                {
                    Player.extraAccessorySlots = 0;
                }
            }

            ResetRogueStealth();

			contactDamageReduction = 0D;
			projectileDamageReduction = 0D;

            throwingDamage = 1f;
            throwingVelocity = 1f;
            throwingCrit = 0;
			throwingAmmoCost = 1f;
			accStealthGenBoost = 0f;

			trueMeleeDamage = 0D;

            dashMod = 0;
            externalAbyssLight = 0;
            externalColdImmunity = externalHeatImmunity = false;
            alcoholPoisonLevel = 0;
			noLifeRegen = false;

            thirdSage = false;
            if (Player.immuneTime <= 0)
                thirdSageH = false;

            perfmini = false;
            akato = false;
            leviPet = false;
            plaguebringerBab = false;
            rotomPet = false;
            ladShark = false;
            sparks = false;
            sirenPet = false;
            fox = false;
            chibii = false;
            brimling = false;
            bearPet = false;
            kendra = false;
            trashMan = false;
            astrophage = false;
            flakPet = false;
            babyGhostBell = false;
            radiator = false;
            scalPet = false;
            bendyPet = false;
            onyxExcavator = false;
            angryDog = false;
            fab = false;
            crysthamyr = false;
            miniOldDuke = false;

            abyssalDivingSuitPlates = false;
            abyssalDivingSuitCooldown = false;

            sirenWaterBuff = false;
            sirenIce = false;
            sirenIceCooldown = false;

            draedonsHeart = false;

            afflicted = false;
            affliction = false;

            fasterMeleeLevel = false;
            fasterRangedLevel = false;
            fasterMagicLevel = false;
            fasterSummonLevel = false;
            fasterRogueLevel = false;

            dodgeScarf = false;
			evasionScarf = false;
            scarfCooldown = false;
            eScarfCooldown = false;

            elysianAegis = false;

            nCore = false;

            godSlayer = false;
            godSlayerDamage = false;
            godSlayerMage = false;
            godSlayerRanged = false;
            godSlayerThrowing = false;
            godSlayerSummon = false;
            godSlayerReflect = false;
            godSlayerCooldown = false;

            silvaSet = false;
            silvaMelee = false;
            silvaRanged = false;
            silvaThrowing = false;
            silvaMage = false;
            silvaSummon = false;

            auricSet = false;
            auricBoost = false;

            omegaBlueChestplate = false;
            omegaBlueSet = false;
            omegaBlueHentai = false;

            molluskSet = false;
            fearmongerSet = false;

            ataxiaBolt = false;
            ataxiaGeyser = false;
            ataxiaFire = false;
            ataxiaVolley = false;
            ataxiaBlaze = false;
            ataxiaMage = false;

            shadeRegen = false;

            flamethrowerBoost = false;
            hoverboardBoost = false; //hoverboard + shroomite visage

            shadowSpeed = false;
            dsSetBonus = false;
            wearingRogueArmor = false;

			blockAllDashes = false;

            kingSlimeLore = false;
            desertScourgeLore = false;
            crabulonLore = false;
            eaterOfWorldsLore = false;
            hiveMindLore = false;
            perforatorLore = false;
            queenBeeLore = false;
            skeletronLore = false;
            slimeGodLore = false;
            wallOfFleshLore = false;
            twinsLore = false;
            destroyerLore = false;
            aquaticScourgeLore = false;
            skeletronPrimeLore = false;
            brimstoneElementalLore = false;
            calamitasLore = false;
            planteraLore = false;
            leviathanAndSirenLore = false;
            astrumAureusLore = false;
            astrumDeusLore = false;
            golemLore = false;
            plaguebringerGoliathLore = false;
            dukeFishronLore = false;
            boomerDukeLore = false;
            ravagerLore = false;
            lunaticCultistLore = false;
            moonLordLore = false;
            providenceLore = false;
            polterghastLore = false;
            DoGLore = false;
            yharonLore = false;
            SCalLore = false;
			oceanLore = false;
			corruptionLore = false;
			crimsonLore = false;
			underworldLore = false;

            luxorsGift = false;
            fungalSymbiote = false;
            trinketOfChi = false;
            gladiatorSword = false;
            unstablePrism = false;
            regenator = false;
            deepDiver = false;
            theBee = false;
            alluringBait = false;
            enchantedPearl = false;
            fishingStation = false;
            rBrain = false;
            bloodyWormTooth = false;
            rampartOfDeities = false;
            vexation = false;
            fBulwark = false;
            badgeOfBravery = false;
            badgeOfBraveryRare = false;
            aSpark = false;
            aSparkRare = false;
            aBulwark = false;
            aBulwarkRare = false;
            dAmulet = false;
            fCarapace = false;
            gShell = false;
            seaShell = false;
            absorber = false;
            aAmpoule = false;
            rOoze = false;
            pAmulet = false;
            fBarrier = false;
            aBrain = false;
            amalgam = false;
            frostFlare = false;
            beeResist = false;
            uberBees = false;
            projRef = false;
            projRefRare = false;
            nanotech = false;
            eQuiver = false;
            cryogenSoul = false;
            yInsignia = false;
            eGauntlet = false;
            eTalisman = false;
            statisBeltOfCurses = false;
            nucleogenesis = false;
            nuclearRod = false;
            heartOfDarkness = false;
            shadowMinions = false;
            tearMinions = false;
            alchFlask = false;
            reducedPlagueDmg = false;
			abaddon = false;
            community = false;
            stressPills = false;
            laudanum = false;
            fleshTotem = false;
            fleshTotemCooldown = false;
            bloodPact = false;
            bloodflareCore = false;
            coreOfTheBloodGod = false;
            elementalHeart = false;
            crownJewel = false;
            celestialJewel = false;
            astralArcanum = false;
            harpyRing = false;
			harpyWingBoost = false; //harpy wings + harpy ring
            darkSunRing = false;
            calamityRing = false;
            voidOfExtinction = false;
            eArtifact = false;
            dArtifact = false;
            gArtifact = false;
            pArtifact = false;
            giantPearl = false;
            normalityRelocator = false;
            fabledTortoise = false;
            manaOverloader = false;
            royalGel = false;
            handWarmer = false;
            lol = false;
            raiderTalisman = false;
            gSabaton = false;
            sGenerator = false;
            sDefense = false;
            sRegen = false;
            sPower = false;
            hallowedRune = false;
            hallowedDefense = false;
            hallowedRegen = false;
            hallowedPower = false;
            kamiBoost = false;
            IBoots = false;
            elysianFire = false;
            sTracers = false;
            eTracers = false;
            cTracers = false;
            oldDie = false;
            ursaSergeant = false;
            scuttlersJewel = false;
            thiefsDime = false;
            dynamoStemCells = false;
            etherealExtorter = false;
            dukeScales = false;
            blazingCore = false;
            voltaicJelly = false;
            jellyChargedBattery = false;
            starbusterCore = false;
            starTaintedGenerator = false;
            camper = false;
			silvaWings = false;
			corrosiveSpine = false;
            rustyMedal = false;
            noStupidNaturalARSpawns = false;
            burdenBreakerYeet = false;
			roverDrive = false;

            daedalusReflect = false;
            daedalusSplit = false;
            daedalusAbsorb = false;
            daedalusShard = false;

            brimflameSet = false;
            brimflameFrenzy = false;
            brimflameFrenzyCooldown = false;

            reaverSpore = false;
            reaverDoubleTap = false;
            reaverBlast = false;
            reaverBurst = false;

            ironBoots = false;
            depthCharm = false;
            anechoicPlating = false;
            jellyfishNecklace = false;
			abyssDivingGear = false;
            abyssalAmulet = false;
            lumenousAmulet = false;
            aquaticEmblem = false;

            astralStarRain = false;

            desertProwler = false;

            snowRuffianSet = false;

            forbiddenCirclet = false;

            eskimoSet = false; //vanilla armor
            meteorSet = false; //vanilla armor, for Space Gun nerf

            victideSet = false;

            sulfurSet = false;
			sulfurJump = false;

            aeroSet = false;

            statigelSet = false;
			statigelJump = false;

            titanHeartSet = false;
            titanHeartMask = false;
            titanHeartMantle = false;
            titanHeartBoots = false;
            umbraphileSet = false;
            plagueReaper = false;
			plaguebringerPatronSet = false;
			plaguebringerCarapace = false;
			plaguebringerPistons = false;
            fathomSwarmer = false;
            fathomSwarmerVisage = false;
            fathomSwarmerBreastplate = false;
            fathomSwarmerTail = false;
            prismaticSet = false;
            prismaticHelmet = false;
            prismaticRegalia = false;
            prismaticGreaves = false;

            tarraSet = false;
            tarraMelee = false;
            tarragonCloak = false;
            tarragonCloakCooldown = false;
            tarraMage = false;
            tarraRanged = false;
            tarraThrowing = false;
            tarragonImmunity = false;
            tarragonImmunityCooldown = false;
            tarraSummon = false;

            bloodflareSet = false;
            bloodflareMelee = false;
            bloodflareFrenzy = false;
            bloodFrenzyCooldown = false;
            bloodflareRanged = false;
            bloodflareSoulCooldown = false;
            bloodflareThrowing = false;
            bloodflareMage = false;
            bloodflareSummon = false;

            xerocSet = false;

            weakPetrification = false;

            inkBomb = false;
            inkBombCooldown = false;
            darkGodSheath = false;
            abyssalMirror = false;
            abyssalMirrorCooldown = false;
            eclipseMirror = false;
            eclipseMirrorCooldown = false;
            featherCrown = false;
            moonCrown = false;
            dragonScales = false;
            gloveOfPrecision = false;
            gloveOfRecklessness = false;
            momentumCapacitor = false;
            vampiricTalisman = false;
            electricianGlove = false;
            bloodyGlove = false;
            filthyGlove = false;
            sandCloak = false;
            sandCloakCooldown = false;
            spectralVeil = false;
            hasJetpack = false;
            plaguedFuelPack = false;
            blunderBooster = false;
            veneratedLocket = false;

            alcoholPoisoning = false;
            shadowflame = false;
            wDeath = false;
            lethalLavaBurn = false;
            aCrunch = false;
            absoluteRage = false;
            irradiated = false;
            bFlames = false;
            aFlames = false;
            gsInferno = false;
            astralInfection = false;
            pFlames = false;
            hFlames = false;
            hInferno = false;
            gState = false;
            bBlood = false;
            eGravity = false;
            vHex = false;
            eGrav = false;
            warped = false;
            cDepth = false;
            fishAlert = false;
            bOut = false;
            clamity = false;
            enraged = false;
            snowmanNoseless = false;
            sulphurPoison = false;
            nightwither = false;
            eFreeze = false;
            silvaStun = false;
            wCleave = false;
            eutrophication = false;
            iCantBreathe = false;
            cragsLava = false;
            vaporfied = false;
			energyShellCooldown = false;
			prismaticCooldown = false;
            waterLeechBleeding = false;

            revivify = false;
            trinketOfChiBuff = false;
            corrEffigy = false;
            crimEffigy = false;
            decayEffigy = false;
            rRage = false;
            xRage = false;
            xWrath = false;
            graxDefense = false;
            encased = false;
            sMeleeBoost = false;
            eScarfBoost = false;
            tFury = false;
            cadence = false;
            omniscience = false;
            zerg = false;
            zen = false;
            bossZen = false;
            permafrostsConcoction = false;
            armorCrumbling = false;
            armorShattering = false;
            ceaselessHunger = false;
            calcium = false;
            soaring = false;
            bounding = false;
            triumph = false;
            penumbra = false;
            shadow = false;
            photosynthesis = false;
            astralInjection = false;
            gravityNormalizer = false;
            holyWrath = false;
            profanedRage = false;
            draconicSurge = false;
            draconicSurgeCooldown = false;
            tesla = false;
            teslaFreeze = false;
            sulphurskin = false;
            baguette = false;
            trippy = false;
            amidiasBlessing = false;
            yPower = false;
            aWeapon = false;
            tScale = false;
            fabsolVodka = false;
            invincible = false;
            shine = false;
            anechoicCoating = false;
            mushy = false;
            molten = false;
            shellBoost = false;
            cFreeze = false;
            tRegen = false;
            polarisBoost = false;
            polarisBoostTwo = false;
            polarisBoostThree = false;
            bloodfinBoost = false;
			bloodPactBoost = false;

            killSpikyBalls = false;

            vodka = false;
            redWine = false;
            grapeBeer = false;
            moonshine = false;
            rum = false;
            whiskey = false;
            fireball = false;
            everclear = false;
            bloodyMary = false;
            tequila = false;
            caribbeanRum = false;
            cinnamonRoll = false;
            tequilaSunrise = false;
            margarita = false;
            starBeamRye = false;
            screwdriver = false;
            moscowMule = false;
            whiteWine = false;
            evergreenGin = false;

            tranquilityCandle = false;
            chaosCandle = false;
            purpleCandle = false;
            blueCandle = false;
            pinkCandle = false;
            yellowCandle = false;

            wDroid = false;
            resButterfly = false;
            glSword = false;
            mWorm = false;
            iClasper = false;
            magicHat = false;
            herring = false;
            blackhawk = false;
            cosmicViper = false;
            calamari = false;
            cEyes = false;
            cSlime = false;
            cSlime2 = false;
            aSlime = false;
            bStar = false;
            aStar = false;
            SP = false;
            dCreeper = false;
            bClot = false;
            eAxe = false;
            endoCooper = false;
            apexShark = false;
            gastricBelcher = false;
            squirrel = false;
            hauntedDishes = false;
            stormjaw = false;
            SPG = false;
            sirius = false;
            aChicken = false;
            cLamp = false;
            pGuy = false;
            cEnergy = false;
            gDefense = false;
            gOffense = false;
            gHealer = false;
            sWaifu = false;
            dWaifu = false;
            cWaifu = false;
            bWaifu = false;
            slWaifu = false;
            fClump = false;
            rDevil = false;
            aValkyrie = false;
            sCrystal = false;
            sGod = false;
            sandnado = false;
            plantera = false;
            aProbe = false;
            vUrchin = false;
            cSpirit = false;
            rOrb = false;
            dCrystal = false;
            youngDuke = false;
            sandWaifu = false;
            sandBoobWaifu = false;
            cloudWaifu = false;
            brimstoneWaifu = false;
            sirenWaifu = false;
            allWaifus = false;
            fungalClump = false;
            howlsHeart = false;
            redDevil = false;
            valkyrie = false;
            slimeGod = false;
            urchin = false;
            chaosSpirit = false;
            reaverOrb = false;
            daedalusCrystal = false;
            shellfish = false;
            hCrab = false;
            tDime = false;
            endoHydra = false;
            powerfulRaven = false;
            dragonFamily = false;
            providenceStabber = false;
            plaguebringerMK2 = false;
            igneousExaltation = false;
            coldDivinity = false;
            radiantResolution = false;
            virili = false;
            frostBlossom = false;
            cinderBlossom = false;
            belladonaSpirit = false;
            vileFeeder = false;
            scabRipper = false;
            midnightUFO = false;
            plagueEngine = false;
            brimseeker = false;
            necrosteocytesDudes = false;
            gammaHead = false;
            rustyDrone = false;
            tundraFlameBlossom = false;
            starSwallowerPetFroge = false;
            snakeEyes = false;
            poleWarper = false;
            causticDragon = false;
			plaguebringerPatronSummon = false;
			howlTrio = false;
            mountedScanner = false;

            abyssalDivingSuitPrevious = abyssalDivingSuit;
            abyssalDivingSuit = abyssalDivingSuitHide = abyssalDivingSuitForce = abyssalDivingSuitPower = false;

            sirenBoobsPrevious = sirenBoobs;
            sirenBoobs = sirenBoobsHide = sirenBoobsForce = sirenBoobsPower = false;

            profanedCrystalPrevious = profanedCrystal;
            profanedCrystal = profanedCrystalBuffs = profanedCrystalForce = profanedCrystalHide = false;

            snowmanPrevious = snowman;
            snowman = snowmanHide = snowmanForce = snowmanPower = false;

            meldTransformationPrevious = meldTransformation;
            meldTransformation = meldTransformationForce = meldTransformationPower = false;

            omegaBlueTransformationPrevious = omegaBlueTransformation;
            omegaBlueTransformation = omegaBlueTransformationForce = omegaBlueTransformationPower = false;

            rageModeActive = false;
            adrenalineModeActive = false;

            lastProjectileHit = null;
        }
        #endregion

        #region Screen Position Movements
        public override void ModifyScreenPosition()
        {
            if (CalamityWorld.ScreenShakeSpots.Count > 0)
            {
                // Fail-safe to ensure that spots don't last forever.
                Dictionary<int, ScreenShakeSpot> screenShakeSpots = new Dictionary<int, ScreenShakeSpot>();
                List<int> screenShakeUUIDs = CalamityWorld.ScreenShakeSpots.Keys.ToList();
                for (int i = 0; i < CalamityWorld.ScreenShakeSpots.Count; i++)
                {
                    int uuid = screenShakeUUIDs[i];
                    if (Main.projectile[uuid].active)
                    {
                        screenShakeSpots.Add(uuid, CalamityWorld.ScreenShakeSpots[uuid]);
                    }
                }
                CalamityWorld.ScreenShakeSpots = screenShakeSpots;

                foreach (var spot in CalamityWorld.ScreenShakeSpots)
                {
                    float maxPower = Utils.GetLerpValue(1300f, 0f, Vector2.Distance(spot.Value.Position, Player.Center), true) * spot.Value.ScreenShakePower;
                    Main.screenPosition += Main.rand.NextVector2Circular(maxPower, maxPower);
                }
            }
        }
        #endregion

        #region UpdateDead
        public override void UpdateDead()
        {
            #region Debuffs
            deathModeBlizzardTime = 0;
            deathModeUnderworldTime = 0;
            gaelRageCooldown = 0;
            gaelSwipes = 0;
            gaelSwitchTimer = 0;
            andromedaState = AndromedaPlayerState.Inactive;
            planarSpeedBoost = 0;
            galileoCooldown = 0;
            soundCooldown = 0;
            shadowPotCooldown = 0;
			dogTextCooldown = 0;
			auralisStealthCounter = 0f;
			auralisAuroraCounter = 0;
			auralisAuroraCooldown = 0;
			auralisAurora = 0;
			fungalSymbioteTimer = 0;
            rage = 0;
            adrenaline = 0;
            raiderStack = 0;
            raiderCooldown = 0;
            gSabatonFall = 0;
            gSabatonCooldown = 0;
            astralStarRainCooldown = 0;
            bloodflareMageCooldown = 0;
            tarraMageHealCooldown = 0;
            bossRushImmunityFrameCurseTimer = 0;
            aBulwarkRareMeleeBoostTimer = 0;
            acidRoundMultiplier = 1D;
            externalAbyssLight = 0;
            externalColdImmunity = externalHeatImmunity = false;
            polarisBoostCounter = 0;
            spectralVeilImmunity = 0;
            jetPackCooldown = 0;
            blunderBoosterDash = 0;
            blunderBoosterDirection = 0;
            plaguedFuelPackDash = 0;
            plaguedFuelPackDirection = 0;
            andromedaCripple = 0;
            theBeeCooldown = 0;
            killSpikyBalls = false;
            moonCrownCooldown = 0;
            featherCrownCooldown = 0;
            nanoFlareCooldown = 0;
            fleshTotemCooldown = false;
            sandCloakCooldown = false;
			icicleCooldown = 0;
			statisTimer = 0;
			hallowedRuneCooldown = 0;
			sulphurBubbleCooldown = 0;
			ladHearts = 0;
			prismaticLasers = 0;
			roverDriveTimer = 0;
			resetHeightandWidth = false;
			noLifeRegen = false;

            alcoholPoisoning = false;
            shadowflame = false;
            wDeath = false;
            lethalLavaBurn = false;
            aCrunch = false;
            absoluteRage = false;
            irradiated = false;
            bFlames = false;
            aFlames = false;
            gsInferno = false;
            astralInfection = false;
            pFlames = false;
            hFlames = false;
            hInferno = false;
            gState = false;
            bBlood = false;
            eGravity = false;
            vHex = false;
            eGrav = false;
            warped = false;
            cDepth = false;
            fishAlert = false;
            bOut = false;
            clamity = false;
            snowmanNoseless = false;
            scarfCooldown = false;
            eScarfCooldown = false;
            godSlayerCooldown = false;
            abyssalDivingSuitCooldown = false;
            abyssalDivingSuitPlateHits = 0;
            sirenIceCooldown = false;
            inkBombCooldown = false;
            abyssalMirrorCooldown = false;
            eclipseMirrorCooldown = false;
            sulphurPoison = false;
            nightwither = false;
            eFreeze = false;
            silvaStun = false;
            wCleave = false;
            eutrophication = false;
            iCantBreathe = false;
            cragsLava = false;
            vaporfied = false;
			energyShellCooldown = false;
			prismaticCooldown = false;
            waterLeechBleeding = false;
            #endregion

            #region Rogue
            // Stealth
            rogueStealth = 0f;
            rogueStealthMax = 0f;
            stealthAcceleration = 1f;

            throwingDamage = 1f;
            throwingVelocity = 1f;
            throwingCrit = 0;
			throwingAmmoCost = 1f;
            #endregion

            #region UI
            if (stealthUIAlpha > 0f)
            {
                stealthUIAlpha -= 0.035f;
                stealthUIAlpha = MathHelper.Clamp(stealthUIAlpha, 0f, 1f);
            }
            #endregion

            #region Buffs
            sDefense = false;
            sRegen = false;
            sPower = false;
            hallowedDefense = false;
            hallowedRegen = false;
            hallowedPower = false;
            onyxExcavator = false;
            angryDog = false;
            fab = false;
            crysthamyr = false;
            abyssalDivingSuitPlates = false;
            sirenWaterBuff = false;
            sirenIce = false;
            trinketOfChiBuff = false;
            chiBuffTimer = 0;
            corrEffigy = false;
            crimEffigy = false;
            rRage = false;
            xRage = false;
            xWrath = false;
            kamiBoost = false;
            graxDefense = false;
            encased = false;
            sMeleeBoost = false;
            eScarfBoost = false;
            tFury = false;
            cadence = false;
            omniscience = false;
            zerg = false;
            zen = false;
            bossZen = false;
            permafrostsConcoction = false;
            armorCrumbling = false;
            armorShattering = false;
            ceaselessHunger = false;
            calcium = false;
            soaring = false;
            bounding = false;
            triumph = false;
            penumbra = false;
            shadow = false;
            photosynthesis = false;
            astralInjection = false;
            gravityNormalizer = false;
            holyWrath = false;
            profanedRage = false;
            tesla = false;
            teslaFreeze = false;
            sulphurskin = false;
            baguette = false;
            draconicSurge = false;
            draconicSurgeCooldown = false;
            yPower = false;
            aWeapon = false;
            tScale = false;
			titanBoost = 0;
            fabsolVodka = false;
            invincible = false;
            shine = false;
            anechoicCoating = false;
            mushy = false;
            molten = false;
            enraged = false;
            shellBoost = false;
            cFreeze = false;
            tRegen = false;
            rageModeActive = false;
            adrenalineModeActive = false;
            vodka = false;
            redWine = false;
            grapeBeer = false;
            moonshine = false;
            rum = false;
            whiskey = false;
            fireball = false;
            everclear = false;
            bloodyMary = false;
            tequila = false;
            caribbeanRum = false;
            cinnamonRoll = false;
            tequilaSunrise = false;
            margarita = false;
            starBeamRye = false;
            screwdriver = false;
            moscowMule = false;
            whiteWine = false;
            evergreenGin = false;
            tranquilityCandle = false;
            chaosCandle = false;
            purpleCandle = false;
            blueCandle = false;
            pinkCandle = false;
            pinkCandleHealFraction = 0D;
            yellowCandle = false;
            trippy = false;
            amidiasBlessing = false;
            polarisBoost = false;
            polarisBoostTwo = false;
            polarisBoostThree = false;
            bloodfinBoost = false;
            bloodfinTimer = 0;
            revivify = false;
            healCounter = 300;
            danceOfLightCharge = 0;
			bloodPactBoost = false;
            #endregion

            #region Armorbonuses
            flamethrowerBoost = false;
            hoverboardBoost = false; //hoverboard + shroomite visage
            shadowSpeed = false;
            godSlayer = false;
            godSlayerDamage = false;
            godSlayerMage = false;
            godSlayerRanged = false;
            godSlayerThrowing = false;
            godSlayerSummon = false;
            godSlayerReflect = false;
            auricBoost = false;
            silvaSet = false;
            silvaMelee = false;
            silvaRanged = false;
            silvaThrowing = false;
            silvaMage = false;
            silvaSummon = false;
            hasSilvaEffect = false;
            silvaCountdown = 600;
            silvaHitCounter = 0;
            auricSet = false;
            omegaBlueChestplate = false;
            omegaBlueSet = false;
            omegaBlueCooldown = 0;
            molluskSet = false;
            fearmongerSet = false;
            daedalusReflect = false;
            daedalusSplit = false;
            daedalusAbsorb = false;
            daedalusShard = false;
            brimflameSet = false;
            brimflameFrenzy = false;
            brimflameFrenzyCooldown = false;
			brimflameFrenzyTimer = 0;
            reaverSpore = false;
            reaverDoubleTap = false;
            shadeRegen = false;
            dsSetBonus = false;
            titanHeartSet = false;
            titanHeartMask = false;
            titanHeartMantle = false;
            titanHeartBoots = false;
			titanCooldown = 0;
            umbraphileSet = false;
            reaverBlast = false;
            reaverBurst = false;
            fathomSwarmer = false;
            fathomSwarmerVisage = false;
            fathomSwarmerBreastplate = false;
            fathomSwarmerTail = false;
            prismaticSet = false;
            prismaticHelmet = false;
            prismaticRegalia = false;
            prismaticGreaves = false;
            astralStarRain = false;
            plagueReaper = false;
            plagueReaperCooldown = 0;
			plaguebringerPatronSet = false;
			plaguebringerCarapace = false;
			plaguebringerPistons = false;
            pistonsCounter = 0;
            ataxiaMage = false;
            ataxiaBolt = false;
            ataxiaGeyser = false;
            ataxiaFire = false;
            ataxiaVolley = false;
            ataxiaBlaze = false;
            hydrothermalSmoke = false;
            desertProwler = false;
            snowRuffianSet = false;
            forbiddenCirclet = false;
			forbiddenCooldown = 0;
			tornadoCooldown = 0;
            eskimoSet = false; //vanilla armor
            meteorSet = false; //vanilla armor, for Space Gun nerf
            victideSet = false;
            aeroSet = false;
			sulfurSet = false;
			sulfurJump = false;
			jumpAgainSulfur = false;
            statigelSet = false;
			statigelJump = false;
			jumpAgainStatigel = false;
            tarraSet = false;
            tarraMelee = false;
            tarragonCloak = false;
            tarragonCloakCooldown = false;
            tarraDefenseTime = 600;
            tarraMage = false;
            tarraRanged = false;
            tarraThrowing = false;
            tarragonImmunity = false;
            tarragonImmunityCooldown = false;
            tarraThrowingCrits = 0;
            tarraSummon = false;
            bloodflareSet = false;
            bloodflareMelee = false;
            bloodflareFrenzy = false;
            bloodFrenzyCooldown = false;
            bloodflareMeleeHits = 0;
            bloodflareRanged = false;
            bloodflareSoulCooldown = false;
			bloodflareSoulTimer = 0;
            bloodflareThrowing = false;
            bloodflareMage = false;
            bloodflareSummon = false;
            bloodflareSummonTimer = 0;
            fearmongerSet = false;
            fearmongerRegenFrames = 0;
            xerocSet = false;
            IBoots = false;
            elysianFire = false;
            elysianAegis = false;
            elysianGuard = false;
            #endregion

            CurrentlyViewedFactoryID = -1;
            CurrentlyViewedChargerID = -1;
            CurrentlyViewedHologramID = -1;
            CurrentlyViewedHologramText = string.Empty;

            KameiBladeUseDelay = 0;
            lastProjectileHit = null;
			brimlashBusterBoost = false;
			animusBoost = 1f;
			potionTimer = 0;
            bloodflareCoreLostDefense = 0;

            if (BossRushEvent.BossRushActive)
            {
                if (!CalamityGlobalNPC.AnyLivingPlayers())
                {
                    BossRushEvent.BossRushActive = false;
                    BossRushEvent.BossRushStage = 0;
                    CalamityNetcode.SyncWorld();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var netMessage = Mod.GetPacket();
                        netMessage.Write((byte)CalRDMessageType.BossRushStage);
                        netMessage.Write(BossRushEvent.BossRushStage);
                        netMessage.Send();
                    }
                    for (int doom = 0; doom < Main.maxNPCs; doom++)
                    {
                        if (Main.npc[doom].active && Main.npc[doom].boss)
                        {
                            Main.npc[doom].active = false;
                            Main.npc[doom].netUpdate = true;
                        }
                    }
                }
            }
            if (CalamityWorld.armageddon && !areThereAnyDamnBosses)
            {
                Player.respawnTimer -= 5;
            }
            else if (Player.respawnTimer > 300 && Main.expertMode) //600 normal 900 expert
            {
                Player.respawnTimer--;
            }
        }
        #endregion

        #region InventoryStartup
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)/* tModPorter Suggestion: Return an Item array to add to the players starting items. Use ModifyStartingInventory for modifying them if needed */
        {
            Item createItem(int type)
            {
                Item i = new Item();
                i.SetDefaults(type);
                return i;
            }

            if (!mediumCoreDeath)
            {
	            return new Item[]
	            {
		            createItem(ModContent.ItemType<StarterBag>()),
		            createItem(ModContent.ItemType<Revenge>()),
		            createItem(ModContent.ItemType<IronHeart>())
	            };
            }
            return null;
        }
        #endregion

        #region Life Regen
        public override void UpdateBadLifeRegen()
        {
            CalamityPlayerLifeRegen.CalamityUpdateBadLifeRegen(Player, Mod);
        }

        public override void UpdateLifeRegen()
        {
            CalamityPlayerLifeRegen.CalamityUpdateLifeRegen(Player, Mod);
        }
        #endregion

        #region HotKeys
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (CalRD.MomentumCapacitatorHotkey.JustPressed && momentumCapacitor && Main.myPlayer == Player.whoAmI && rogueStealth >= rogueStealthMax * 0.3f &&
                wearingRogueArmor && rogueStealthMax > 0 && CalamityUtils.CountProjectiles(ModContent.ProjectileType<MomentumCapacitorOrb>()) == 0)
            {
                rogueStealth -= rogueStealthMax * 0.3f;
                Vector2 fieldSpawnCenter = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
                Projectile.NewProjectile(Player.GetSource_FromThis(), fieldSpawnCenter, Vector2.Zero, ModContent.ProjectileType<MomentumCapacitorOrb>(), 0, 0f, Player.whoAmI, 0f, 0f);
            }
            if (CalRD.NormalityRelocatorHotKey.JustPressed && normalityRelocator && Main.myPlayer == Player.whoAmI)
            {
                if (!Player.chaosState)
                {
                    Vector2 teleportLocation;
                    teleportLocation.X = (float)Main.mouseX + Main.screenPosition.X;
                    if (Player.gravDir == 1f)
                    {
                        teleportLocation.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)Player.height;
                    }
                    else
                    {
                        teleportLocation.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                    }
                    teleportLocation.X -= (float)(Player.width / 2);
                    if (teleportLocation.X > 50f && teleportLocation.X < (float)(Main.maxTilesX * 16 - 50) && teleportLocation.Y > 50f && teleportLocation.Y < (float)(Main.maxTilesY * 16 - 50))
                    {
                        int x = (int)(teleportLocation.X / 16f);
                        int y = (int)(teleportLocation.Y / 16f);
                        if (!Collision.SolidCollision(teleportLocation, Player.width, Player.height))
                        {
                            Player.Teleport(teleportLocation, 4, 0);
                            NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, (float)Player.whoAmI, teleportLocation.X, teleportLocation.Y, 1, 0, 0);

							int duration = chaosStateDuration;
							if (areThereAnyDamnBosses || areThereAnyDamnEvents)
								duration = chaosStateDurationBoss;
							if (eScarfCooldown)
								duration = (int)(duration * 1.5);
							else if (scarfCooldown)
								duration *= 2;
							Player.AddBuff(BuffID.ChaosState, duration, true);
                        }
                    }
                }
            }
            if (CalRD.SandCloakHotkey.JustPressed && sandCloak && Main.myPlayer == Player.whoAmI && rogueStealth >= rogueStealthMax * 0.25f &&
                wearingRogueArmor && rogueStealthMax > 0 && !sandCloakCooldown)
            {
                Player.AddBuff(ModContent.BuffType<SandCloakCooldown>(), 1800, false); //30 seconds
                rogueStealth -= rogueStealthMax * 0.25f;
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<SandCloakVeil>(), 7, 8, Player.whoAmI, 0, 0);
                SoundEngine.PlaySound(SoundID.Item45, Player.position);
            }
            if (CalRD.SpectralVeilHotKey.JustPressed && spectralVeil && Main.myPlayer == Player.whoAmI && rogueStealth >= rogueStealthMax * 0.25f &&
                wearingRogueArmor && rogueStealthMax > 0)
            {
                if (!Player.chaosState)
                {
                    float teleportRange = 320f;
                    Vector2 teleportLocation;
                    teleportLocation.X = (float)Main.mouseX + Main.screenPosition.X;
                    if (Player.gravDir == 1f)
                    {
                        teleportLocation.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)Player.height;
                    }
                    else
                    {
                        teleportLocation.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                    }
                    teleportLocation.X -= (float)(Player.width / 2);
                    Vector2 playerToTeleport = teleportLocation - Player.position;
                    if (playerToTeleport.Length() > teleportRange)
                    {
                        playerToTeleport.Normalize();
                        playerToTeleport *= teleportRange;
                        teleportLocation = Player.position + playerToTeleport;
                    }
                    if (teleportLocation.X > 50f && teleportLocation.X < (float)(Main.maxTilesX * 16 - 50) && teleportLocation.Y > 50f && teleportLocation.Y < (float)(Main.maxTilesY * 16 - 50))
                    {
                        int x = (int)(teleportLocation.X / 16f);
                        int y = (int)(teleportLocation.Y / 16f);
                        if (!Collision.SolidCollision(teleportLocation, Player.width, Player.height))
                        {
                            rogueStealth -= rogueStealthMax * 0.25f;

                            Player.Teleport(teleportLocation, 1, 0);
                            NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, (float)Player.whoAmI, teleportLocation.X, teleportLocation.Y, 1, 0, 0);

							int duration = chaosStateDuration;
							if (areThereAnyDamnBosses || areThereAnyDamnEvents)
								duration = chaosStateDurationBoss;
							if (eScarfCooldown)
								duration = (int)(duration * 1.5);
							else if (scarfCooldown)
								duration *= 2;
							Player.AddBuff(BuffID.ChaosState, duration, true);

                            int numDust = 40;
                            Vector2 step = playerToTeleport / numDust;
                            for (int i = 0; i < numDust; i++)
                            {
                                int dustIndex = Dust.NewDust(Player.Center - (step * i), 1, 1, 21, step.X, step.Y);
                                Main.dust[dustIndex].noGravity = true;
                                Main.dust[dustIndex].noLight = true;
                            }

                            Player.immune = true;
                            Player.immuneTime = 120;
                            spectralVeilImmunity = 120;
                            for (int k = 0; k < Player.hurtCooldowns.Length; k++)
                            {
                                Player.hurtCooldowns[k] = Player.immuneTime;
                            }
                        }
                    }
                }
            }
            if (CalRD.PlaguePackHotKey.JustPressed && hasJetpack && Main.myPlayer == Player.whoAmI && rogueStealth >= rogueStealthMax * 0.25f &&
                wearingRogueArmor && rogueStealthMax > 0 && jetPackCooldown == 0 && !Player.mount.Active)
            {
				if (blunderBooster)
				{
					jetPackCooldown = 90;
					blunderBoosterDash = 15;
					blunderBoosterDirection = Player.direction;
					rogueStealth -= rogueStealthMax * 0.25f;
					SoundEngine.PlaySound(SoundID.Item66, Player.Center);
					SoundEngine.PlaySound(SoundID.Item34, Player.Center);
				}
				else if (plaguedFuelPack)
				{
					jetPackCooldown = 90;
					plaguedFuelPackDash = 10;
					plaguedFuelPackDirection = Player.direction;
					rogueStealth -= rogueStealthMax * 0.25f;
					SoundEngine.PlaySound(SoundID.Item66, Player.Center);
					SoundEngine.PlaySound(SoundID.Item34, Player.Center);
				}
            }
            if (CalRD.TarraHotKey.JustPressed)
            {
                if (brimflameSet && !brimflameFrenzyCooldown)
                {
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        if (brimflameFrenzy)
                        {
                            brimflameFrenzy = false;
                            Player.ClearBuff(ModContent.BuffType<BrimflameFrenzyBuff>());
                        }
                        else
                        {
                            brimflameFrenzy = true;
                            Player.AddBuff(ModContent.BuffType<BrimflameFrenzyBuff>(), 10 * 60, true);
							SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/BrimflameAbility"), Player.Center);
                            for (int num502 = 0; num502 < 36; num502++)
                            {
                                int dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 16f), Player.width, Player.height - 16, (int)CalamityDusts.Brimstone, 0f, 0f, 0, default, 1f);
                                Main.dust[dust].velocity *= 3f;
                                Main.dust[dust].scale *= 1.15f;
                            }
                            int num226 = 36;
                            for (int num227 = 0; num227 < num226; num227++)
                            {
                                Vector2 vector6 = Vector2.Normalize(Player.velocity) * new Vector2((float)Player.width / 2f, (float)Player.height) * 0.75f;
                                vector6 = vector6.RotatedBy((double)((float)(num227 - (num226 / 2 - 1)) * MathHelper.TwoPi / (float)num226), default) + Player.Center;
                                Vector2 vector7 = vector6 - Player.Center;
                                int num228 = Dust.NewDust(vector6 + vector7, 0, 0, (int)CalamityDusts.Brimstone, vector7.X * 1.5f, vector7.Y * 1.5f, 100, default, 1.4f);
                                Main.dust[num228].noGravity = true;
                                Main.dust[num228].noLight = true;
                                Main.dust[num228].velocity = vector7;
                            }
                        }
                    }
                }
                if (tarraMelee && !tarragonCloakCooldown && !tarragonCloak)
                {
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        Player.AddBuff(ModContent.BuffType<TarragonCloak>(), 602, false);
                    }
                }
                if (bloodflareRanged && !bloodflareSoulCooldown)
                {
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        Player.AddBuff(ModContent.BuffType<BloodflareSoulCooldown>(), 1800, false);
						bloodflareSoulTimer = 1800;
                    }
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/BloodflareRangerActivation"), Player.Center);
                    for (int d = 0; d < 64; d++)
                    {
                        int dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 16f), Player.width, Player.height - 16, (int)CalamityDusts.Phantoplasm, 0f, 0f, 0, default, 1f);
                        Main.dust[dust].velocity *= 3f;
                        Main.dust[dust].scale *= 1.15f;
                    }
                    int dustAmt = 36;
                    for (int d = 0; d < dustAmt; d++)
                    {
                        Vector2 source = Vector2.Normalize(Player.velocity) * new Vector2((float)Player.width / 2f, (float)Player.height) * 0.75f;
                        source = source.RotatedBy((double)((float)(d - (dustAmt / 2 - 1)) * MathHelper.TwoPi / (float)dustAmt), default) + Player.Center;
                        Vector2 dustVel = source - Player.Center;
                        int phanto = Dust.NewDust(source + dustVel, 0, 0, (int)CalamityDusts.Phantoplasm, dustVel.X * 1.5f, dustVel.Y * 1.5f, 100, default, 1.4f);
                        Main.dust[phanto].noGravity = true;
                        Main.dust[phanto].noLight = true;
                        Main.dust[phanto].velocity = dustVel;
                    }
                    float spread = 45f * 0.0174f;
                    double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;
                    int damage = (int)(800 * Player.RangedDamage());
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            float ai1 = Main.rand.NextFloat() + 0.5f;
                            float randomSpeed = (float)Main.rand.Next(1, 7);
                            float randomSpeed2 = (float)Main.rand.Next(1, 7);
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f) + randomSpeed, ModContent.ProjectileType<BloodflareSoul>(), damage, 0f, Player.whoAmI, 0f, ai1);
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f) + randomSpeed2, ModContent.ProjectileType<BloodflareSoul>(), damage, 0f, Player.whoAmI, 0f, ai1);
                        }
                    }
                }
                if (omegaBlueSet && omegaBlueCooldown <= 0)
                {
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        Player.AddBuff(ModContent.BuffType<AbyssalMadness>(), 300, false);
                    }
                    omegaBlueCooldown = 1800;
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/OmegaBlueAbility"), Player.Center);
                    for (int i = 0; i < 66; i++)
                    {
                        int d = Dust.NewDust(Player.position, Player.width, Player.height, 20, 0, 0, 100, Color.Transparent, 2.6f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].noLight = true;
                        Main.dust[d].fadeIn = 1f;
                        Main.dust[d].velocity *= 6.6f;
                    }
                }
                if (dsSetBonus)
                {
                    SoundEngine.PlaySound(SoundID.Zombie104, Player.position);
                    for (int num502 = 0; num502 < 36; num502++)
                    {
                        int dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 16f), Player.width, Player.height - 16, (int)CalamityDusts.Brimstone, 0f, 0f, 0, default, 1f);
                        Main.dust[dust].velocity *= 3f;
                        Main.dust[dust].scale *= 1.15f;
                    }
                    int num226 = 36;
                    for (int num227 = 0; num227 < num226; num227++)
                    {
                        Vector2 vector6 = Vector2.Normalize(Player.velocity) * new Vector2((float)Player.width / 2f, (float)Player.height) * 0.75f;
                        vector6 = vector6.RotatedBy((double)((float)(num227 - (num226 / 2 - 1)) * MathHelper.TwoPi / (float)num226), default) + Player.Center;
                        Vector2 vector7 = vector6 - Player.Center;
                        int num228 = Dust.NewDust(vector6 + vector7, 0, 0, (int)CalamityDusts.Brimstone, vector7.X * 1.5f, vector7.Y * 1.5f, 100, default, 1.4f);
                        Main.dust[num228].noGravity = true;
                        Main.dust[num228].noLight = true;
                        Main.dust[num228].velocity = vector7;
                    }
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        Player.AddBuff(ModContent.BuffType<Enraged>(), 600, false);
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int l = 0; l < Main.maxNPCs; l++)
                        {
                            NPC npc = Main.npc[l];
                            if (npc.active && !npc.friendly && !npc.dontTakeDamage && Vector2.Distance(Player.Center, npc.Center) <= 3000f)
                            {
                                npc.AddBuff(ModContent.BuffType<Enraged>(), 600, false);
                            }
                        }
                    }
                }
                if (plagueReaper && plagueReaperCooldown <= 0)
				{
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/PlagueReaperAbility"), Player.Center);
                    plagueReaperCooldown = 1800;
				}
				if (forbiddenCirclet && forbiddenCooldown <= 0)
				{
					forbiddenCooldown = 45;
                    int stormMana = (int)(ForbiddenCirclet.manaCost * Player.manaCost);
                    if (Player.statMana < stormMana)
                    {
                        if (Player.manaFlower)
                        {
                            Player.QuickMana();
                        }
                    }
                    if (Player.statMana >= stormMana && !Player.silence)
                    {
                        Player.manaRegenDelay = (int)Player.maxRegenDelay;
                        Player.statMana -= stormMana;
						float dmgMult = Player.RogueDamage() + Player.GetDamage(DamageClass.Summon).Additive - 1f;
						int damage = (int)(ForbiddenCirclet.tornadoBaseDmg * dmgMult);
                        if (Player.HasBuff(BuffID.ManaSickness))
                        {
                            int sickPenalty = (int)(damage * (0.05f * ((Player.buffTime[Player.FindBuffIndex(BuffID.ManaSickness)] + 60) / 60)));
                            damage -= sickPenalty;
                        }
						float kBack = ForbiddenCirclet.tornadoBaseKB + Player.GetKnockback(DamageClass.Summon).Base;
						if (Player.whoAmI == Main.myPlayer)
						{
							Projectile.NewProjectile(Entity.GetSource_FromThis(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<CircletMark>(), damage, kBack, Player.whoAmI, 0f, 0f);
						}
					}
				}
				if (prismaticSet && !prismaticCooldown && prismaticLasers <= 0)
					prismaticLasers = CalamityUtils.SecondsToFrames(35f);
            }
            if (CalRD.AstralArcanumUIHotkey.JustPressed && astralArcanum)
            {
                AstralArcanumUI.Toggle();
            }
            if (CalRD.AstralTeleportHotKey.JustPressed)
            {
                if (celestialJewel)
                {
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        Player.TeleportationPotion();
                        SoundEngine.PlaySound(SoundID.Item6, Player.position);
                    }
                    else if (Main.netMode == NetmodeID.MultiplayerClient && Player.whoAmI == Main.myPlayer)
                    {
                        NetMessage.SendData(MessageID.RequestTeleportationByServer, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
            if (CalRD.AegisHotKey.JustPressed)
            {
                if (elysianAegis && !Player.mount.Active)
                {
                    elysianGuard = !elysianGuard;
                }
            }
            if (CalRD.RageHotKey.JustPressed)
            {
                if (gaelRageCooldown == 0 && Player.ActiveItem().type == ModContent.ItemType<GaelsGreatsword>() &&
                    rage > 0)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SilvaDispel"), Player.Center);
                    for (int i = 0; i < 3; i++)
                    {
                        Dust.NewDust(Player.position, 120, 120, 218, 0f, 0f, 100, default, 1.5f);
                    }
                    for (int i = 0; i < 30; i++)
                    {
                        float angle = MathHelper.TwoPi * i / 30f;
                        int dustIndex = Dust.NewDust(Player.position, 120, 120, 218, 0f, 0f, 0, default, 2f);
                        Main.dust[dustIndex].noGravity = true;
                        Main.dust[dustIndex].velocity *= 4f;
                        dustIndex = Dust.NewDust(Player.position, 120, 120, 218, 0f, 0f, 100, default, 1f);
                        Main.dust[dustIndex].velocity *= 2.25f;
                        Main.dust[dustIndex].noGravity = true;
                        Dust.NewDust(Player.Center + angle.ToRotationVector2() * 160f, 0, 0, 218, 0f, 0f, 100, default, 1f);
                    }
                    gaelRageCooldown = 60 * GaelsGreatsword.SkullsplosionCooldownSeconds;
                    float rageRatio = rage / rageMax;
                    int damage = (int)(rageRatio * GaelsGreatsword.MaxRageBoost * GaelsGreatsword.BaseDamage * Player.MeleeDamage());
                    float skullCount = 5f;
                    float skullSpeed = 5f;
                    if (CalamityWorld.downedYharon)
                    {
                        skullCount = 20f;
                        skullSpeed = 12f;
                    }
                    else if (NPC.downedMoonlord)
                    {
                        skullCount = 13f;
                        skullSpeed = 10f;
                    }
                    else if (Main.hardMode)
                    {
                        skullCount = 9f;
                        skullSpeed = 6.8f;
                    }
                    for (float i = 0; i < skullCount; i += 1f)
                    {
                        float angle = MathHelper.TwoPi * i / skullCount;
                        Vector2 initialVelocity = angle.ToRotationVector2().RotatedByRandom(MathHelper.ToRadians(12f)) * skullSpeed * new Vector2(0.82f, 1.5f) *
                            Main.rand.NextFloat(0.8f, 1.2f) * (i < skullCount / 2  ? 0.25f : 1f);
                        int projectileIndex = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center + initialVelocity * 3f, initialVelocity, ModContent.ProjectileType<GaelSkull2>(), damage, 2f, Player.whoAmI);
                        Main.projectile[projectileIndex].tileCollide = false;
                        Main.projectile[projectileIndex].localAI[1] = (Main.projectile[projectileIndex].velocity.Y < 0f).ToInt();
                    }
                    rage = 0;
                }
                if (rage == rageMax && CalamityConfig.Instance.Rippers && !rageModeActive)
                {
                    SoundEngine.PlaySound(SoundID.Zombie104, Player.position);
                    for (int num502 = 0; num502 < 64; num502++)
                    {
                        int dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 16f), Player.width, Player.height - 16, (int)CalamityDusts.Brimstone, 0f, 0f, 0, default, 1f);
                        Main.dust[dust].velocity *= 3f;
                        Main.dust[dust].scale *= 1.15f;
                    }
                    int num226 = 36;
                    for (int num227 = 0; num227 < num226; num227++)
                    {
                        Vector2 vector6 = Vector2.Normalize(Player.velocity) * new Vector2((float)Player.width / 2f, (float)Player.height) * 0.75f;
                        vector6 = vector6.RotatedBy((double)((float)(num227 - (num226 / 2 - 1)) * 6.28318548f / (float)num226), default) + Player.Center;
                        Vector2 vector7 = vector6 - Player.Center;
                        int num228 = Dust.NewDust(vector6 + vector7, 0, 0, (int)CalamityDusts.Brimstone, vector7.X * 1.5f, vector7.Y * 1.5f, 100, default, 1.4f);
                        Main.dust[num228].noGravity = true;
                        Main.dust[num228].noLight = true;
                        Main.dust[num228].velocity = vector7;
                    }
                    Player.AddBuff(ModContent.BuffType<RageMode>(), RageDuration);
                }
            }
            if (CalRD.AdrenalineHotKey.JustPressed && CalamityConfig.Instance.Rippers && CalamityWorld.revenge)
            {
                if (adrenaline == adrenalineMax && !adrenalineModeActive)
                {
                    SoundEngine.PlaySound(SoundID.Zombie104, Player.position);
                    for (int num502 = 0; num502 < 64; num502++)
                    {
                        int dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 16f), Player.width, Player.height - 16, 206, 0f, 0f, 0, default, 1f);
                        Main.dust[dust].velocity *= 3f;
                        Main.dust[dust].scale *= 2f;
                    }
                    int num226 = 36;
                    for (int num227 = 0; num227 < num226; num227++)
                    {
                        Vector2 vector6 = Vector2.Normalize(Player.velocity) * new Vector2((float)Player.width / 2f, (float)Player.height) * 0.75f;
                        vector6 = vector6.RotatedBy((double)((float)(num227 - (num226 / 2 - 1)) * 6.28318548f / (float)num226), default) + Player.Center;
                        Vector2 vector7 = vector6 - Player.Center;
                        int num228 = Dust.NewDust(vector6 + vector7, 0, 0, 206, vector7.X * 1.5f, vector7.Y * 1.5f, 100, default, 1.4f);
                        Main.dust[num228].noGravity = true;
                        Main.dust[num228].noLight = true;
                        Main.dust[num228].velocity = vector7;
                    }
                    Player.AddBuff(ModContent.BuffType<AdrenalineMode>(), AdrenalineDuration);
                }
            }


			bool mountCheck = true;
			if (Player.mount != null && Player.mount.Active)
				mountCheck = Player.mount.BlockExtraJumps;
			bool canJump = (!Player.GetJumpState(ExtraJump.CloudInABottle).Enabled ||  !VanillaExtraJump.CloudInABottle.CanStart(Player)) &&
			(!Player.GetJumpState(ExtraJump.SandstormInABottle).Enabled || !VanillaExtraJump.SandstormInABottle.CanStart(Player)) &&
			(!Player.GetJumpState(ExtraJump.BlizzardInABottle).Enabled || !VanillaExtraJump.BlizzardInABottle.CanStart(Player)) &&
			(!Player.GetJumpState(ExtraJump.FartInAJar).Enabled || !VanillaExtraJump.FartInAJar.CanStart(Player)) &&
			(!Player.GetJumpState(ExtraJump.TsunamiInABottle).Enabled || !VanillaExtraJump.TsunamiInABottle.CanStart(Player)) &&
			(!Player.GetJumpState(ExtraJump.UnicornMount).Enabled || !VanillaExtraJump.UnicornMount.CanStart(Player)) &&
			CalamityUtils.CountHookProj() <= 0 && (Player.rocketTime == 0 || Player.wings > 0) && mountCheck;
			if (PlayerInput.Triggers.JustPressed.Jump && Player.position.Y != Player.oldPosition.Y && canJump)
			{
				if (statigelJump && jumpAgainStatigel)
				{
					jumpAgainStatigel = false;
					int offset = Player.height;
					if (Player.gravDir == -1f)
						offset = 0;
					SoundEngine.PlaySound(SoundID.DoubleJump, Player.position);
					Player.velocity.Y = -Player.jumpSpeed * Player.gravDir;
					Player.jump = (int)(Player.jumpHeight * 1.25);
					for (int d = 0; d < 30; ++d)
					{
						int goo = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + offset), Player.width, 12, 4, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, 100, new Color(0, 80, 255, 100), 1.5f);
						if (d % 2 == 0)
							Main.dust[goo].velocity.X += (float)Main.rand.Next(30, 71) * 0.1f;
						else
							Main.dust[goo].velocity.X -= (float)Main.rand.Next(30, 71) * 0.1f;
						Main.dust[goo].velocity.Y += (float)Main.rand.Next(-10, 31) * 0.1f;
						Main.dust[goo].noGravity = true;
						Main.dust[goo].scale += (float)Main.rand.Next(-10, 41) * 0.01f;
						Main.dust[goo].velocity *= Main.dust[goo].scale * 0.7f;
					}
				}
				else if (sulfurJump && jumpAgainSulfur)
				{
					jumpAgainSulfur = false;
					int offset = Player.height;
					if (Player.gravDir == -1f)
						offset = 0;
					SoundEngine.PlaySound(SoundID.DoubleJump, Player.position);
					Player.velocity.Y = -Player.jumpSpeed * Player.gravDir;
					Player.jump = (int)(Player.jumpHeight * 1.5);
					for (int d = 0; d < 30; ++d)
					{
						int sulfur = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + offset), Player.width, 12, 31, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, 100, default, 1.5f);
						if (d % 2 == 0)
							Main.dust[sulfur].velocity.X += (float)Main.rand.Next(30, 71) * 0.1f;
						else
							Main.dust[sulfur].velocity.X -= (float)Main.rand.Next(30, 71) * 0.1f;
						Main.dust[sulfur].velocity.Y += (float)Main.rand.Next(-10, 31) * 0.1f;
						Main.dust[sulfur].noGravity = true;
						Main.dust[sulfur].scale += (float)Main.rand.Next(-10, 41) * 0.01f;
						Main.dust[sulfur].velocity *= Main.dust[sulfur].scale * 0.7f;
					}
					if (sulphurBubbleCooldown <= 0)
					{
						int bubble = Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(Player.position.X, Player.position.Y + (Player.gravDir == -1f ? 20 : -20)), Vector2.Zero, ModContent.ProjectileType<SulphuricAcidBubbleFriendly>(), (int)(20f * Player.RogueDamage()), 0f, Player.whoAmI, 1f, 0f);
						Main.projectile[bubble].Calamity().forceRogue = true;
						sulphurBubbleCooldown = 20;
					}
				}
			}
        }
        #endregion

        #region TeleportMethods
        public void HandleTeleport(int teleportType, bool forceHandle = false, int whoAmI = 0)
        {
            bool syncData = forceHandle || Main.netMode == NetmodeID.SinglePlayer;
            if (syncData)
            {
                TeleportPlayer(teleportType, forceHandle, whoAmI);
            }
            else
            {
                SyncTeleport(teleportType);
            }
        }

        public static void TeleportPlayer(int teleportType, bool syncData = false, int whoAmI = 0)
        {
            Player player;
            if (!syncData)
            {
                player = Main.LocalPlayer;
            }
            else
            {
                player = Main.player[whoAmI];
            }
            switch (teleportType)
            {
                case 0:
                    UnderworldTeleport(player, syncData);
                    break;
                case 1:
                    DungeonTeleport(player, syncData);
                    break;
                case 2:
                    JungleTeleport(player, syncData);
                    break;
                default:
                    break;
            }
        }

        public void SyncTeleport(int teleportType)
        {
            ModPacket netMessage = Mod.GetPacket();
            netMessage.Write((byte)CalRDMessageType.TeleportPlayer);
            netMessage.Write(teleportType);
            netMessage.Send();
        }

        public static void UnderworldTeleport(Player player, bool syncData = false)
        {
            int teleportStartX = 100;
            int teleportRangeX = Main.maxTilesX - 200;
            int teleportStartY = Main.maxTilesY - 200;
            int teleportRangeY = 50;
            bool flag = false;
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int width = player.width;
            Vector2 vector = new Vector2((float)num2, (float)num3) * 16f + new Vector2((float)(-(float)width / 2 + 8), (float)-(float)player.height);
            while (!flag && num < 1000)
            {
                num++;
                num2 = teleportStartX + Main.rand.Next(teleportRangeX);
                num3 = teleportStartY + Main.rand.Next(teleportRangeY);
                vector = new Vector2((float)num2, (float)num3) * 16f + new Vector2((float)(-(float)width / 2 + 8), (float)-(float)player.height);
                if (!Collision.SolidCollision(vector, width, player.height))
                {
                    int i = 0;
                    while (i < 100)
                    {
                        Tile tile = Main.tile[num2, num3 + i];
                        vector = new Vector2((float)num2, (float)(num3 + i)) * 16f + new Vector2((float)(-(float)width / 2 + 8), (float)-(float)player.height);
                        Vector4 vector2 = Collision.SlopeCollision(vector, player.velocity, width, player.height, player.gravDir, false);
                        bool arg_1FF_0 = !Collision.SolidCollision(vector, width, player.height);
                        if (vector2.Z == player.velocity.X && vector2.Y == player.velocity.Y && vector2.X == vector.X)
                        {
                            bool arg_1FE_0 = vector2.Y == vector.Y;
                        }
                        if (arg_1FF_0)
                        {
                            i++;
                        }
                        else
                        {
                            if (tile.HasTile && !tile.IsActuated && Main.tileSolid[(int)tile.TileType])
                            {
                                break;
                            }
                            i++;
                        }
                    }
                    if (!Collision.LavaCollision(vector, width, player.height) && Collision.HurtTiles(vector, width, player.height, player).y <= 0f)
                    {
                        Collision.SlopeCollision(vector, player.velocity, width, player.height, player.gravDir, false);
                        if (Collision.SolidCollision(vector, width, player.height) && i < 99)
                        {
                            Vector2 vector3 = Vector2.UnitX * 16f;
                            if (!(Collision.TileCollision(vector - vector3, vector3, player.width, player.height, false, false, (int)player.gravDir) != vector3))
                            {
                                vector3 = -Vector2.UnitX * 16f;
                                if (!(Collision.TileCollision(vector - vector3, vector3, player.width, player.height, false, false, (int)player.gravDir) != vector3))
                                {
                                    vector3 = Vector2.UnitY * 16f;
                                    if (!(Collision.TileCollision(vector - vector3, vector3, player.width, player.height, false, false, (int)player.gravDir) != vector3))
                                    {
                                        vector3 = -Vector2.UnitY * 16f;
                                        if (!(Collision.TileCollision(vector - vector3, vector3, player.width, player.height, false, false, (int)player.gravDir) != vector3))
                                        {
                                            flag = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!flag)
            {
                return;
            }
            ModTeleport(player, vector, syncData, false);
        }

        public static void DungeonTeleport(Player player, bool syncData = false)
        {
            ModTeleport(player, new Vector2(Main.dungeonX, Main.dungeonY), syncData, true);
        }

        public static void JungleTeleport(Player player, bool syncData = false)
        {
            int teleportStartX = CalamityWorld.abyssSide ? (int)(Main.maxTilesX * 0.65) : (int)(Main.maxTilesX * 0.2);
            int teleportRangeX = (int)(Main.maxTilesX * 0.15);

            int teleportStartY = (int)Main.worldSurface - 75;
            int teleportRangeY = 50;

            bool flag = false;
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int width = player.width;
            Vector2 vector = new Vector2((float)num2, (float)num3) * 16f + new Vector2((float)(-(float)width / 2 + 8), (float)-(float)player.height);
            while (!flag && num < 1000)
            {
                num++;
                num2 = teleportStartX + Main.rand.Next(teleportRangeX);
                num3 = teleportStartY + Main.rand.Next(teleportRangeY);
                vector = new Vector2((float)num2, (float)num3) * 16f + new Vector2((float)(-(float)width / 2 + 8), (float)-(float)player.height);
                if (!Collision.SolidCollision(vector, width, player.height))
                {
                    int i = 0;
                    while (i < 100)
                    {
                        Tile tile = Main.tile[num2, num3 + i];
                        vector = new Vector2((float)num2, (float)(num3 + i)) * 16f + new Vector2((float)(-(float)width / 2 + 8), (float)-(float)player.height);
                        Vector4 vector2 = Collision.SlopeCollision(vector, player.velocity, width, player.height, player.gravDir, false);
                        bool arg_1FF_0 = !Collision.SolidCollision(vector, width, player.height);
                        if (vector2.Z == player.velocity.X && vector2.Y == player.velocity.Y && vector2.X == vector.X)
                        {
                            bool arg_1FE_0 = vector2.Y == vector.Y;
                        }
                        if (arg_1FF_0)
                        {
                            i++;
                        }
                        else
                        {
                            if (tile.HasTile && !tile.IsActuated && Main.tileSolid[(int)tile.TileType])
                            {
                                break;
                            }
                            i++;
                        }
                    }
                    if (!Collision.LavaCollision(vector, width, player.height) && Collision.HurtTiles(vector, width, player.height, player).y <= 0f)
                    {
                        Collision.SlopeCollision(vector, player.velocity, width, player.height, player.gravDir, false);
                        if (Collision.SolidCollision(vector, width, player.height) && i < 99)
                        {
                            Vector2 vector3 = Vector2.UnitX * 16f;
                            if (!(Collision.TileCollision(vector - vector3, vector3, player.width, player.height, false, false, (int)player.gravDir) != vector3))
                            {
                                vector3 = -Vector2.UnitX * 16f;
                                if (!(Collision.TileCollision(vector - vector3, vector3, player.width, player.height, false, false, (int)player.gravDir) != vector3))
                                {
                                    vector3 = Vector2.UnitY * 16f;
                                    if (!(Collision.TileCollision(vector - vector3, vector3, player.width, player.height, false, false, (int)player.gravDir) != vector3))
                                    {
                                        vector3 = -Vector2.UnitY * 16f;
                                        if (!(Collision.TileCollision(vector - vector3, vector3, player.width, player.height, false, false, (int)player.gravDir) != vector3))
                                        {
                                            flag = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!flag)
            {
                return;
            }

            ModTeleport(player, vector, syncData, false);
        }

        public static void ModTeleport(Player player, Vector2 pos, bool syncData = false, bool convertFromTiles = false)
        {
            bool postImmune = player.immune;
            int postImmunteTime = player.immuneTime;
            if (convertFromTiles)
            {
                pos = new Vector2(pos.X * 16 + 8 - player.width / 2, pos.Y * 16 - player.height);
            }
            player.grappling[0] = -1;
            player.grapCount = 0;
            for (int index = 0; index < Main.maxProjectiles; ++index)
            {
                if (Main.projectile[index].active && Main.projectile[index].owner == player.whoAmI && Main.projectile[index].aiStyle == 7)
                {
                    Main.projectile[index].Kill();
                }
            }
            player.Teleport(pos, 2, 0);
            player.velocity = Vector2.Zero;
            player.immune = postImmune;
            player.immuneTime = postImmunteTime;
            for (int index = 0; index < 100; ++index)
            {
                Main.dust[Dust.NewDust(player.position, player.width, player.height, 164, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
            }
            Main.TeleportEffect(player.getRect(), 1);
            Main.TeleportEffect(player.getRect(), 3);
            SoundEngine.PlaySound(SoundID.Item6, player.position);
            if (Main.netMode != NetmodeID.Server)
            {
                return;
            }
            if (syncData)
            {
                RemoteClient.CheckSection(player.whoAmI, player.position, 1);
                NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, (float)player.whoAmI, pos.X, pos.Y, 3, 0, 0);
            }
        }
        #endregion

        #region UpdateEquips
        public override void UpdateVisibleVanityAccessories()
        {
            for (int n = 13; n < 18 + Player.extraAccessorySlots; n++)
            {
                Item item = Player.armor[n];
                if (item.type == ModContent.ItemType<Popo>())
                {
                    snowmanHide = false;
                    snowmanForce = true;
                }
                else if (item.type == ModContent.ItemType<AbyssalDivingSuit>())
                {
                    abyssalDivingSuitHide = false;
                    abyssalDivingSuitForce = true;
                }
                else if (item.type == ModContent.ItemType<SirensHeart>())
                {
                    sirenBoobsHide = false;
                    sirenBoobsForce = true;
                }
                else if (item.type == ModContent.ItemType<ProfanedSoulCrystal>())
                {
                    profanedCrystalHide = false;
                    profanedCrystalForce = true;
                }
                else if (item.type == ModContent.ItemType<AbyssalDivingGear>())
                {
                    abyssDivingGear = true;
                }
            }
        }

        public override void UpdateEquips()
        {
            if (CalamityConfig.Instance.BossHealthBar)
            {
                drawBossHPBar = true;
            }
            else
            {
                drawBossHPBar = false;
            }
            if (CalamityConfig.Instance.BossHealthBarExtraInfo)
            {
                shouldDrawSmallText = true;
            }
            else
            {
                shouldDrawSmallText = false;
            }

            if (CalamityConfig.Instance.MiningSpeedBoost)
            {
                Player.pickSpeed *= 0.75f;
            }

            #region MeleeSpeed
            float meleeSpeedMult = 0f;
            if (bBlood)
            {
                meleeSpeedMult += 0.025f;
            }
            if (rRage)
            {
                meleeSpeedMult += 0.05f;
            }
            if (graxDefense)
            {
                meleeSpeedMult += 0.1f;
            }
            if (sMeleeBoost)
            {
                meleeSpeedMult += 0.05f;
            }
            if (eScarfBoost)
            {
                meleeSpeedMult += 0.15f;
            }
            if (yPower)
            {
                meleeSpeedMult += 0.05f;
            }
            if (darkSunRing)
            {
                meleeSpeedMult += 0.12f;
            }
            if (badgeOfBravery)
            {
                meleeSpeedMult += 0.15f;
            }
			if (badgeOfBraveryRare)
			{
				float maxDistance = 480f; // 30 tile distance
				float meleeSpeedBoost = 0f;
				for (int l = 0; l < Main.maxNPCs; l++)
				{
					NPC nPC = Main.npc[l];
					if (nPC.active && !nPC.friendly && (nPC.damage > 0 || nPC.boss) && !nPC.dontTakeDamage && Vector2.Distance(Player.Center, nPC.Center) <= maxDistance)
					{
						meleeSpeedMult += MathHelper.Lerp(0f, 0.3f, 1f - (Vector2.Distance(Player.Center, nPC.Center) / maxDistance));

						if (meleeSpeedBoost >= 0.3f)
						{
							meleeSpeedBoost = 0.3f;
							break;
						}
					}
				}
				meleeSpeedMult += meleeSpeedBoost;
			}
			if (eGauntlet)
            {
                meleeSpeedMult += 0.15f;
            }
            if (yInsignia)
            {
                meleeSpeedMult += 0.1f;
            }
            if (bloodyMary)
            {
                if (Main.bloodMoon)
                {
                    meleeSpeedMult += 0.15f;
                }
            }
            if (community)
            {
				float floatTypeBoost = 0.05f +
					(NPC.downedSlimeKing ? 0.01f : 0f) +
					(NPC.downedBoss1 ? 0.01f : 0f) +
					(NPC.downedBoss2 ? 0.01f : 0f) +
					(NPC.downedQueenBee ? 0.01f : 0f) +
					(NPC.downedBoss3 ? 0.01f : 0f) + // 0.1
					(Main.hardMode ? 0.01f : 0f) +
					(NPC.downedMechBossAny ? 0.01f : 0f) +
					(NPC.downedPlantBoss ? 0.01f : 0f) +
					(NPC.downedGolemBoss ? 0.01f : 0f) +
					(NPC.downedFishron ? 0.01f : 0f) + // 0.15
					(NPC.downedAncientCultist ? 0.01f : 0f) +
					(NPC.downedMoonlord ? 0.01f : 0f) +
					(CalamityWorld.downedProvidence ? 0.01f : 0f) +
					(CalamityWorld.downedDoG ? 0.01f : 0f) +
					(CalamityWorld.downedYharon ? 0.01f : 0f); // 0.2
				meleeSpeedMult += floatTypeBoost * 0.25f;
            }
            if (eArtifact)
            {
                meleeSpeedMult += 0.1f;
            }
			if (bloodyWormTooth)
			{
				if (Player.statLife < (int)(Player.statLifeMax2 * 0.5))
					meleeSpeedMult += 0.1f;
				else
					meleeSpeedMult += 0.05f;
			}
			if (aquaticScourgeLore)
			{
				if (Player.wellFed)
					meleeSpeedMult += 0.025f;
				else
					meleeSpeedMult -= 0.025f;
			}
			if (CalamityConfig.Instance.Proficiency)
            {
                meleeSpeedMult += GetMeleeSpeedBonus();
            }
            Player.GetAttackSpeed(DamageClass.Melee) += meleeSpeedMult;

			if (Player.ActiveItem().type == ModContent.ItemType<AstralBlade>() || Player.ActiveItem().type == ModContent.ItemType<MantisClaws>() ||
				Player.ActiveItem().type == ModContent.ItemType<Omniblade>() || Player.ActiveItem().type == ModContent.ItemType<BladeofEnmity>())
			{
				float newMeleeSpeed = 1f + ((Player.GetAttackSpeed(DamageClass.Melee) - 1f) * 0.25f);
				Player.GetAttackSpeed(DamageClass.Melee) = newMeleeSpeed;
			}
			#endregion

			if (snowman)
            {
                if (Player.whoAmI == Main.myPlayer && !snowmanNoseless)
                {
                    Player.AddBuff(ModContent.BuffType<PopoBuff>(), 60, true);
                }
            }
            if (abyssalDivingSuit)
            {
                Player.AddBuff(ModContent.BuffType<AbyssalDivingSuitBuff>(), 60, true);
                if (Player.whoAmI == Main.myPlayer)
                {
                    if (abyssalDivingSuitCooldown)
                    {
                        for (int l = 0; l < Player.MaxBuffs; l++)
                        {
                            int hasBuff = Player.buffType[l];
                            if (Player.buffTime[l] < 30 && hasBuff == ModContent.BuffType<AbyssalDivingSuitPlatesBroken>())
                            {
                                abyssalDivingSuitPlateHits = 0;
                                Player.DelBuff(l);
                                l = -1;
                            }
                        }
                    }
                    else
                    {
                        Player.AddBuff(ModContent.BuffType<AbyssalDivingSuitPlates>(), 2);
                    }
                }
            }
            if (sirenBoobs)
            {
                Player.AddBuff(ModContent.BuffType<SirenBobs>(), 60, true);
            }
            if (sirenBoobs && NPC.downedBoss3)
            {
                if (Player.whoAmI == Main.myPlayer && !sirenIceCooldown)
                {
                    Player.AddBuff(ModContent.BuffType<IceShieldBuff>(), 2);
                }
            }
            if (profanedCrystal)
            {
                Player.AddBuff(ModContent.BuffType<ProfanedCrystalBuff>(), 60, true);
            }
        }
        #endregion

        #region PreUpdate
        public override void PreUpdate()
        {
            tailFrameUp++;
            if (tailFrameUp == 8)
            {
                tailFrame++;
                if (tailFrame >= 4)
                {
                    tailFrame = 0;
                }
                tailFrameUp = 0;
            }

            int frameAmt = 11;
            if (roverFrameCounter >= 7)
            {
                roverFrameCounter = -1;
                roverFrame = roverFrame == frameAmt - 1 ? 0 : roverFrame + 1;
            }
            roverFrameCounter++;

            for (int i = 0; i < Player.dye.Length; i++)
            {
                if (Player.dye[i].type == ModContent.ItemType<ProfanedMoonlightDye>())
                {
                    GameShaders.Armor.GetSecondaryShader(Player.dye[i].dye, Player)?.UseColor(CalamityPlayerDrawEffects.GetCurrentMoonlightDyeColor());
                }
            }
        }
        #endregion

        #region PreUpdateBuffs
        public override void PreUpdateBuffs()
        {
            // Remove the mighty wind buff if the player is in the astral desert or if a boss is alive.
            if (Player.ZoneDesert && (ZoneAstral || areThereAnyDamnBosses) && Player.HasBuff(BuffID.WindPushed))
            {
                Player.ClearBuff(BuffID.WindPushed);
            }
        }
        #endregion

        #region PostUpdateBuffs
        public override void PostUpdateBuffs()
        {
            if (CalamityWorld.defiled)
                Defiled();

            if (weakPetrification)
                WeakPetrification();

            if (lol || (silvaCountdown > 0 && hasSilvaEffect && silvaSet))
            {
                if (Player.lifeRegen < 0)
                    Player.lifeRegen = 0;
            }

			if (boomerDukeLore)
				Player.buffImmune[ModContent.BuffType<Irradiated>()] = false;

            if (Player.ownedProjectileCounts[ModContent.ProjectileType<GiantIbanRobotOfDoom>()] > 0)
                Player.yoraiz0rEye = 0;

			if (blockAllDashes || CalamityConfig.Instance.BossRushDashCurse && BossRushEvent.BossRushActive)
				DisableAllDashes();
        }
        #endregion

        #region PostUpdateEquips
        public override void PostUpdateEquips()
        {
            if (CalamityWorld.defiled)
                Defiled();

            if (weakPetrification)
                WeakPetrification();

			if (lol || (silvaCountdown > 0 && hasSilvaEffect && silvaSet))
            {
                if (Player.lifeRegen < 0)
                    Player.lifeRegen = 0;
            }

			if (boomerDukeLore)
				Player.buffImmune[ModContent.BuffType<Irradiated>()] = false;

            if (Player.ownedProjectileCounts[ModContent.ProjectileType<GiantIbanRobotOfDoom>()] > 0)
                Player.yoraiz0rEye = 0;

			if (blockAllDashes || CalamityConfig.Instance.BossRushDashCurse && BossRushEvent.BossRushActive)
				DisableAllDashes();
        }
		#endregion

		#region PostUpdate

		public override void PostUpdateMiscEffects()
        {
			if(Main.netMode != NetmodeID.Server)
            {
	            bool useNebula = NPC.AnyNPCs(ModContent.NPCType<DevourerofGodsHead>());
	            Player.ManageSpecialBiomeVisuals("CalRD:DevourerofGodsHead", useNebula);

	            bool useNebulaS = NPC.AnyNPCs(ModContent.NPCType<DevourerofGodsHeadS>());
	            Player.ManageSpecialBiomeVisuals("CalRD:DevourerofGodsHeadS", useNebulaS);

	            bool useBrimstone = NPC.AnyNPCs(ModContent.NPCType<CalamitasRun3>());
	            Player.ManageSpecialBiomeVisuals("CalRD:CalamitasRun3", useBrimstone);

	            bool usePlague = NPC.AnyNPCs(ModContent.NPCType<PlaguebringerGoliath>());
	            Player.ManageSpecialBiomeVisuals("CalRD:PlaguebringerGoliath", usePlague);

	            bool useCryogen = NPC.AnyNPCs(ModContent.NPCType<Cryogen>());
	            if (SkyManager.Instance["CalRD:Cryogen"] != null && useCryogen != SkyManager.Instance["CalRD:Cryogen"].IsActive())
	            {
		            if (useCryogen)
		            {
			            SkyManager.Instance.Activate("CalRD:Cryogen", Player.Center);
		            }
		            else
		            {
			            SkyManager.Instance.Deactivate("CalRD:Cryogen");
		            }
	            }

	            Point point = Player.Center.ToTileCoordinates();
	            bool aboveGround = point.Y > Main.maxTilesY - 320;
	            bool overworld = Player.ZoneOverworldHeight && (point.X < 380 || point.X > Main.maxTilesX - 380);
	            bool useFire = NPC.AnyNPCs(ModContent.NPCType<Yharon>());
	            Player.ManageSpecialBiomeVisuals("CalRD:Yharon", useFire);
	            Player.ManageSpecialBiomeVisuals("HeatDistortion", Main.UseHeatDistortion && (useFire || trippy ||
		            aboveGround || (point.Y < Main.worldSurface && Player.ZoneDesert && !overworld && !Main.raining && !Filters.Scene["Sandstorm"].IsActive())));

	            bool useWater = NPC.AnyNPCs(ModContent.NPCType<Leviathan>());
	            Player.ManageSpecialBiomeVisuals("CalRD:Leviathan", useWater);

	            bool useHoly = NPC.AnyNPCs(ModContent.NPCType<Providence>());
	            Player.ManageSpecialBiomeVisuals("CalRD:Providence", useHoly);

	            bool useSBrimstone = NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitas>());
	            Player.ManageSpecialBiomeVisuals("CalRD:SupremeCalamitas", useSBrimstone);

	            bool inAstral = ZoneAstral;
	            Player.ManageSpecialBiomeVisuals("CalRD:Astral", inAstral);

	            bool cryogenActive = NPC.AnyNPCs(ModContent.NPCType<Cryogen>());

	            if (SkyManager.Instance["CalRD:Cryogen"] != null && cryogenActive != SkyManager.Instance["CalRD:Cryogen"].IsActive())
	            {
		            if (cryogenActive)
		            {
			            SkyManager.Instance.Activate("CalRD:Cryogen");
		            }
		            else
		            {
			            SkyManager.Instance.Deactivate("CalRD:Cryogen");
		            }
	            }
            }
            CalamityPlayerMiscEffects.CalamityPostUpdateMiscEffects(Player, Mod);

            if (Player.ActiveItem().type == ModContent.ItemType<GaelsGreatsword>())
            {
                gaelSwitchTimer = GaelSwitchPhase.LoseRage;
                rage += (int)MathHelper.Min(5, 10000 - rage);
            }
            else if (Player.ActiveItem().type != ModContent.ItemType<GaelsGreatsword>() && gaelSwitchTimer == GaelSwitchPhase.LoseRage)
            {
                rage = 0;
                gaelSwitchTimer = GaelSwitchPhase.None;
            }
        }

        #region Dragon Scale Logic
        public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (item.type == ModContent.ItemType<DragonScales>() && !CalamityWorld.dragonScalesBought)
            {
                CalamityWorld.dragonScalesBought = true;
            }
        }
        #endregion

        #region Shop Restrictions

        public override bool CanBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (item.type == ModContent.ItemType<DragonScales>())
            {
                return !CalamityWorld.dragonScalesBought;
            }
            return base.CanBuyItem(vendor, shopInventory, item);
        }

        public override bool CanSellItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (item.type == ModContent.ItemType<ProfanedSoulCrystal>())
                return CalamityWorld.downedSCal; //no easy moneycoins for post doggo/yhar
            return base.CanSellItem(vendor, shopInventory, item);
        }

        #endregion

        public override void PostUpdateRunSpeeds()
        {
            #region SpeedBoosts
            float runAccMult = 1f +
                (shadowSpeed ? 0.5f : 0f) +
                (stressPills ? 0.05f : 0f) +
                ((abyssalDivingSuit && Player.IsUnderwater()) ? 0.05f : 0f) +
                (sirenWaterBuff ? 0.15f : 0f) +
                ((frostFlare && Player.statLife < (int)(Player.statLifeMax2 * 0.25)) ? 0.15f : 0f) +
                (auricSet ? 0.1f : 0f) +
                (dragonScales ? 0.1f : 0f) +
                (kamiBoost ? KamiBuff.RunAccelerationBoost : 0f) +
				(slimeGodLore ? 0.1f : 0f) +
                (cTracers ? 0.1f : 0f) +
                (silvaSet ? 0.05f : 0f) +
                (eTracers ? 0.05f : 0f) +
                (blueCandle ? 0.05f : 0f) +
                (etherealExtorter && Player.ZoneBeach ? 0.05f : 0f) +
                (planarSpeedBoost > 0 ? (0.01f * planarSpeedBoost) : 0f) +
                ((deepDiver && Player.IsUnderwater()) ? 0.15f : 0f) +
                (rogueStealthMax > 0f ? (rogueStealth >= rogueStealthMax ? rogueStealth * 0.05f : rogueStealth * 0.025f) : 0f);

            float runSpeedMult = 1f +
                (shadowSpeed ? 0.5f : 0f) +
                ((abyssalDivingSuit && Player.IsUnderwater()) ? 0.05f : 0f) +
                ((frostFlare && Player.statLife < (int)(Player.statLifeMax2 * 0.25)) ? 0.15f : 0f) +
                (sirenWaterBuff ? 0.15f : 0f) +
                (auricSet ? 0.1f : 0f) +
                (dragonScales ? 0.1f : 0f) +
                (cTracers ? 0.1f : 0f) +
                (silvaSet ? 0.05f : 0f) +
                (eTracers ? 0.05f : 0f) +
                (kamiBoost ? KamiBuff.RunSpeedBoost : 0f) +
				(slimeGodLore ? 0.1f : 0f) +
				(etherealExtorter && Player.ZoneBeach ? 0.05f : 0f) +
                (stressPills ? 0.05f : 0f) +
                (planarSpeedBoost > 0 ? (0.01f * planarSpeedBoost) : 0f) +
                ((deepDiver && Player.IsUnderwater()) ? 0.15f : 0f) +
                (rogueStealthMax > 0f ? (rogueStealth >= rogueStealthMax ? rogueStealth * 0.05f : rogueStealth * 0.025f) : 0f);

            if (destroyerLore)
            {
                runAccMult *= 0.95f;
            }
            if (twinsLore)
            {
                if (Player.statLife < (int)(Player.statLifeMax2 * 0.5))
                    runAccMult *= 0.95f;
            }
            if (skeletronPrimeLore)
            {
                runAccMult *= 0.95f;
            }
            if (abyssalDivingSuit && !Player.IsUnderwater())
            {
                runAccMult *= 0.4f;
                runSpeedMult *= 0.4f;
            }
            if (fabledTortoise)
            {
                runAccMult *= 0.5f;
                runSpeedMult *= 0.5f;
            }
            if (ursaSergeant)
            {
                runAccMult *= 0.65f;
                runSpeedMult *= 0.65f;
            }
            if (elysianGuard)
            {
                runAccMult *= 0.5f;
                runSpeedMult *= 0.5f;
            }
			if (CalamityWorld.revenge)
			{
				if (Player.powerrun)
				{
					runSpeedMult *= 0.6666667f;
				}
				if ((Player.slippy || Player.slippy2) && Player.iceSkate)
				{
					runAccMult *= 0.6666667f;
				}
			}
			if (CalamityWorld.death && deathModeBlizzardTime > 0)
            {
                float speedMult = (3600 - deathModeBlizzardTime) / 3600f;
                runAccMult *= speedMult;
                runSpeedMult *= speedMult;
            }

            Player.runAcceleration *= runAccMult;
            Player.maxRunSpeed *= runSpeedMult;

			if (slimeGodLore)
			{
				if (!Player.iceSkate)
					Player.runSlowdown *= 0.1f;
			}
            #endregion

            #region DashEffects
            if (Player.pulley && dashMod > 0)
            {
                ModDashMovement();
			}
            else if (Player.grappling[0] == -1 && !Player.tongued)
            {
                ModHorizontalMovement();

				if (dashMod > 0)
                    ModDashMovement();

				if (pAmulet && modStealth < 1f)
                {
                    float num43 = Player.maxRunSpeed / 2f * (1f - modStealth);
                    Player.maxRunSpeed -= num43;
                    Player.accRunSpeed = Player.maxRunSpeed;
                }
            }
            #endregion
        }
        #endregion

        #region Rogue Mirrors
        public void AbyssMirrorEvade()
        {
            if (Player.whoAmI == Main.myPlayer && abyssalMirror && !abyssalMirrorCooldown && !eclipseMirror)
            {
                Player.AddBuff(ModContent.BuffType<AbyssalMirrorCooldown>(), 1200);
                Player.immune = true;
                Player.immuneTime = Player.longInvince ? 100 : 60;
                Player.noKnockback = true;
                rogueStealth += 0.5f;

                for (int k = 0; k < Player.hurtCooldowns.Length; k++)
                {
                    Player.hurtCooldowns[k] = Player.immuneTime;
                }

                SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SilvaActivation"), Player.Center);

                for (int i = 0; i < 10; i++)
                {
                    int lumenyl = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), ModContent.ProjectileType<AbyssalMirrorProjectile>(), (int)(55 * Player.RogueDamage()), 0, Player.whoAmI);
                    Main.projectile[lumenyl].rotation = Main.rand.NextFloat(0, 360);
                    Main.projectile[lumenyl].frame = Main.rand.Next(0, 4);
                }

                if (Player.whoAmI == Main.myPlayer)
                {
                    NetMessage.SendData(MessageID.Dodge, -1, -1, null, Player.whoAmI, 1f, 0f, 0f, 0, 0, 0);
                }
            }
        }

        public void EclipseMirrorEvade()
        {
            if (Player.whoAmI == Main.myPlayer && eclipseMirror && !eclipseMirrorCooldown)
            {
                Player.AddBuff(ModContent.BuffType<EclipseMirrorCooldown>(), 1200);
                Player.immune = true;
                Player.immuneTime = Player.longInvince ? 100 : 60;
                Player.noKnockback = true;
                rogueStealth = rogueStealthMax;

                for (int k = 0; k < Player.hurtCooldowns.Length; k++)
                {
                    Player.hurtCooldowns[k] = Player.immuneTime;
                }

                SoundEngine.PlaySound(SoundID.Item68, Player.Center);
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<EclipseMirrorBurst>(), (int)(7000 * Player.RogueDamage()), 0, Player.whoAmI);

                if (Player.whoAmI == Main.myPlayer)
                {
                    NetMessage.SendData(MessageID.Dodge, -1, -1, null, Player.whoAmI, 1f, 0f, 0f, 0, 0, 0);
                }
            }
        }
        #endregion

        #region Pre Kill
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            PopupGUIManager.SuspendAll();
            if (Player.Calamity().andromedaState == AndromedaPlayerState.LargeRobot)
            {
                if (!Main.dedServ)
                {
                    for (int i = 0; i < 40; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(Player.Center + Utils.NextVector2Circular(Main.rand, 60f, 90f), 133);
                        dust.velocity = Utils.NextVector2Circular(Main.rand, 4f, 4f);
                        dust.noGravity = true;
                        dust.scale = Main.rand.NextFloat(1.2f, 1.35f);
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        Utils.PoofOfSmoke(Player.Center + Utils.NextVector2Circular(Main.rand, 20f, 30f));
                    }
                }
            }
            if (invincible && Player.ActiveItem().type != ModContent.ItemType<ColdheartIcicle>())
            {
                if (Player.statLife <= 0)
                {
                    Player.statLife = 1;
                }
                return false;
            }
            if (hInferno)
            {
                for (int x = 0; x < Main.maxNPCs; x++)
                {
                    if (Main.npc[x].active && Main.npc[x].type == ModContent.NPCType<Providence>())
                    {
                        Main.npc[x].active = false;
                    }
                }
            }
            if (nCore && Main.rand.NextBool(10))
            {
                SoundEngine.PlaySound(SoundID.Item67, Player.position);
                for (int j = 0; j < 25; j++)
                {
                    int num = Dust.NewDust(Player.position, Player.width, Player.height, 173, 0f, 0f, 100, default, 2f);
                    Dust dust = Main.dust[num];
                    dust.position.X += (float)Main.rand.Next(-20, 21);
                    dust.position.Y += (float)Main.rand.Next(-20, 21);
                    dust.velocity *= 0.9f;
                    dust.scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                    dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cWaist, Player);
                    if (Main.rand.NextBool(2))
                    {
                        dust.scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                    }
                }
                Player.statLife += 100;
                Player.HealEffect(100);
                if (Player.statLife > Player.statLifeMax2)
                {
                    Player.statLife = Player.statLifeMax2;
                }
                return false;
            }
            if (godSlayer && !godSlayerCooldown)
            {
                SoundEngine.PlaySound(SoundID.Item67, Player.position);
                for (int j = 0; j < 50; j++)
                {
                    int num = Dust.NewDust(Player.position, Player.width, Player.height, 173, 0f, 0f, 100, default, 2f);
                    Dust dust = Main.dust[num];
                    dust.position.X += (float)Main.rand.Next(-20, 21);
                    dust.position.Y += (float)Main.rand.Next(-20, 21);
                    dust.velocity *= 0.9f;
                    dust.scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                    dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cWaist, Player);
                    if (Main.rand.NextBool(2))
                    {
                        dust.scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                    }
                }
                int heal = draconicSurge && !draconicSurgeCooldown ? (Player.statLifeMax2 / 2) : 150;
                Player.statLife += heal;
                Player.HealEffect(heal);
                if (Player.statLife > Player.statLifeMax2)
                {
                    Player.statLife = Player.statLifeMax2;
                }
                if (Player.FindBuffIndex(ModContent.BuffType<DraconicSurgeBuff>()) > -1)
                {
                    Player.ClearBuff(ModContent.BuffType<DraconicSurgeBuff>());
                    Player.AddBuff(ModContent.BuffType<DraconicSurgeCooldown>(), CalamityUtils.SecondsToFrames(60f));

					// Additional potion sickness time
					int additionalTime = 0;
					for (int i = 0; i < Player.MaxBuffs; i++)
					{
						if (Player.buffType[i] == BuffID.PotionSickness)
							additionalTime = Player.buffTime[i];
					}
					float potionSicknessTime = 30f + (float)Math.Ceiling(additionalTime / 60D);
					Player.AddBuff(BuffID.PotionSickness, CalamityUtils.SecondsToFrames(potionSicknessTime));
				}
                Player.AddBuff(ModContent.BuffType<GodSlayerCooldown>(), CalamityUtils.SecondsToFrames(45f));
                return false;
            }
            if (silvaSet && silvaCountdown > 0)
            {
                if (hasSilvaEffect)
                {
                    silvaHitCounter++;
                }
                if (Player.FindBuffIndex(ModContent.BuffType<SilvaRevival>()) == -1)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SilvaActivation"), Player.position);
                    Player.AddBuff(ModContent.BuffType<SilvaRevival>(), auricSet ? 300 : 600);
                    if (draconicSurge && !draconicSurgeCooldown)
                    {
                        Player.statLife += Player.statLifeMax2 / 2;
                        Player.HealEffect(Player.statLifeMax2 / 2);
                        if (Player.statLife > Player.statLifeMax2)
                        {
                            Player.statLife = Player.statLifeMax2;
                        }
                        if (Player.FindBuffIndex(ModContent.BuffType<DraconicSurgeBuff>()) > -1)
                        {
                            Player.ClearBuff(ModContent.BuffType<DraconicSurgeBuff>());
                            Player.AddBuff(ModContent.BuffType<DraconicSurgeCooldown>(), CalamityUtils.SecondsToFrames(60f));

							// Additional potion sickness time
							int additionalTime = 0;
							for (int i = 0; i < Player.MaxBuffs; i++)
							{
								if (Player.buffType[i] == BuffID.PotionSickness)
									additionalTime = Player.buffTime[i];
							}
							float potionSicknessTime = 30f + (float)Math.Ceiling(additionalTime / 60D);
							Player.AddBuff(BuffID.PotionSickness, CalamityUtils.SecondsToFrames(potionSicknessTime));
						}
                    }
					else if (silvaWings)
					{
                        Player.statLife += Player.statLifeMax2 / 2;
                        Player.HealEffect(Player.statLifeMax2 / 2);
                        if (Player.statLife > Player.statLifeMax2)
                        {
                            Player.statLife = Player.statLifeMax2;
                        }
					}
                }
                hasSilvaEffect = true;
                if (Player.statLife < 1)
                {
                    Player.statLife = 1;
                }
                return false;
            }
            if (permafrostsConcoction && Player.FindBuffIndex(ModContent.BuffType<ConcoctionCooldown>()) == -1)
            {
                Player.AddBuff(ModContent.BuffType<ConcoctionCooldown>(), CalamityUtils.SecondsToFrames(180f));
                Player.AddBuff(ModContent.BuffType<Encased>(), CalamityUtils.SecondsToFrames(3f));
                Player.statLife = Player.statLifeMax2 * 3 / 10;
                SoundEngine.PlaySound(SoundID.Item92, Player.position);
                for (int i = 0; i < 60; i++)
                {
                    int d = Dust.NewDust(Player.position, Player.width, Player.height, 88, 0f, 0f, 0, default, 2.5f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 5f;
                }
                return false;
            }

            //Custom Death Messages

            if (alcoholPoisoning && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                if (Main.rand.Next(2) == 0)
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + " downed too many shots.");
                else
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + "'s liver failed.");
            }
            if (vHex && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was charred by the brimstone inferno.");
            }
            if ((ZoneCalamity && Player.lavaWet) && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + "'s soul was released by the lava.");
            }
            if (gsInferno && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + "'s soul was extinguished.");
            }
            if (sulphurPoison && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                if (Main.rand.NextBool(2))
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was melted by the toxic waste.");
                else
                    damageSource = PlayerDeathReason.ByOther(9);
            }
            if (lethalLavaBurn && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " disintegrated into ashes.");
            }
            if (hInferno && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was turned to ashes by the Profaned Goddess.");
            }
            if (hFlames && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " fell prey to their sins.");
            }
            if (waterLeechBleeding && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " lost too much blood.");
            }
            if (shadowflame && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + "'s spirit was turned to ash.");
            }
            if (bBlood && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " became a blood geyser.");
            }
            if (cDepth && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                if (Main.rand.NextBool(2))
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was crushed by the pressure.");
                else
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + "'s lungs collapsed.");
            }
            if ((bFlames || aFlames) && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was consumed by the black flames.");
            }
            if (pFlames && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                if (Main.rand.NextBool(2))
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + "'s flesh was melted by the plague.");
                else
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + " didn't vaccinate.");
            }
            if (astralInfection && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                if (Main.rand.NextBool(2))
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + "'s infection spread too far.");
                else
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + "'s skin was replaced by the astral virus.");
            }
            if (nightwither && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was incinerated by lunar rays.");
            }
            if (vaporfied && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " vaporized into thin air.");
            }
            if (manaOverloader && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + "'s life was completely converted into mana.");
            }
            if ((bloodyMary || everclear || evergreenGin || fireball || margarita || moonshine || moscowMule || redWine || screwdriver || starBeamRye || tequila || tequilaSunrise || vodka || whiteWine)
                && damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " succumbed to alcohol sickness.");
            }
            if (profanedCrystalBuffs && !profanedCrystalHide)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was summoned too soon.");
            }

            if (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitas>()))
            {
                if (sCalDeathCount < 51)
                {
                    sCalDeathCount++;
                }
            }

			if (CalamityWorld.ironHeart)
			{
				KillPlayer();
				return false;
			}

			deathCount++;
            if (Player.whoAmI == Main.myPlayer && Main.netMode == NetmodeID.MultiplayerClient)
            {
                DeathPacket(false);
            }

            return true;
        }
        #endregion

        #region On Respawn
        public override void OnRespawn()
        {
            thirdSageH = true;

            // The player rotation can be off if the player dies at the right time when using Final Dawn.
            Player.fullRotation = 0f;
        }
        #endregion

        #region Use Speed Mult
        public override float UseSpeedMultiplier(Item item)
        {
            if (silvaRanged)
            {
                if (item.CountsAsClass(DamageClass.Ranged) && item.useTime > 3)
                    return 1.1f;
            }
            if (silvaThrowing)
            {
                if (Player.statLife > (int)(Player.statLifeMax2 * 0.5) &&
                    item.Calamity().rogue && item.useTime > 3)
                    return 1.1f;
            }
            if (etherealExtorter)
            {
                if (Main.moonPhase == 1 && item.Calamity().rogue && item.useTime > 3) //Waning gibbous
                    return 1.1f;
            }
            return 1f;
        }
		#endregion

		#region Get Heal Life
		public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
		{
			double healMult = 1D +
					(coreOfTheBloodGod ? 0.15 : 0) +
					(bloodPactBoost ? 0.5 : 0);
			healValue = (int)(healValue * healMult);
			if (CalamityWorld.ironHeart)
				healValue = 0;
		}
		#endregion

		#region Get Weapon Damage And KB
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (item.type == ModContent.ItemType<GaelsGreatsword>())
            {
                damage += GaelsGreatsword.BaseDamage / (float)GaelsGreatsword.BaseDamage - 1f;
            }
            if (flamethrowerBoost && item.CountsAsClass(DamageClass.Ranged) && (item.useAmmo == AmmoID.Gel || CalamityLists.flamethrowerList.Contains(item.type)))
            {
	            damage += hoverboardBoost ? 0.35f : 0.25f;
            }
            if (cinnamonRoll && CalamityLists.fireWeaponList.Contains(item.type))
            {
	            damage += 0.15f;
            }
            if (evergreenGin && CalamityLists.natureWeaponList.Contains(item.type))
            {
	            damage += 0.15f;
            }
            if (fireball && CalamityLists.fireWeaponList.Contains(item.type))
            {
	            damage += 0.1f;
            }
            if (eskimoSet && CalamityLists.iceWeaponList.Contains(item.type))
            {
	            damage += 0.1f;
            }
            if (etherealExtorter && Player.ZoneDungeon && item.Calamity().rogue && !item.consumable)
            {
	            damage += 0.05f;
            }

            if (item.CountsAsClass(DamageClass.Ranged))
            {
                acidRoundMultiplier = item.useTime / 20D;
            }
            else
            {
                acidRoundMultiplier = 1D;
            }
			//Prismatic Breaker is a weird hybrid melee-ranged weapon so include it too.  Why are you using desert prowler post-Yharon? don't ask me
			if (desertProwler && (item.CountsAsClass(DamageClass.Ranged) || item.type == ModContent.ItemType<PrismaticBreaker>()) && item.ammo == AmmoID.None)
			{
				damage.Flat += 1f;
			}
        }

        public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback)
        {
            if (auricBoost)
            {
                knockback *= 1f + (1f - modStealth) * 0.5f;
            }
            if (whiskey)
            {
                knockback *= 1.04f;
            }
            if (tequila && Main.dayTime)
            {
                knockback *= 1.03f;
            }
            if (tequilaSunrise && Main.dayTime)
            {
                knockback *= 1.07f;
            }
            if (moscowMule)
            {
                knockback *= 1.09f;
            }
            if (titanHeartMask && item.Calamity().rogue)
            {
                knockback *= 1.05f;
            }
            if (titanHeartMantle && item.Calamity().rogue)
            {
                knockback *= 1.05f;
            }
            if (titanHeartBoots && item.Calamity().rogue)
            {
                knockback *= 1.05f;
            }
            if (titanHeartSet && item.Calamity().rogue)
            {
                knockback *= 1.2f;
            }
            if (titanHeartSet && StealthStrikeAvailable() && item.Calamity().rogue)
            {
                knockback *= 2f;
            }
            bool ZoneForest = !ZoneAbyss && !ZoneSulphur && !ZoneAstral && !ZoneCalamity && !ZoneSunkenSea && !Player.ZoneSnow && !Player.ZoneCorrupt && !Player.ZoneCrimson && !Player.ZoneHallow && !Player.ZoneDesert && !Player.ZoneUndergroundDesert && !Player.ZoneGlowshroom && !Player.ZoneDungeon && !Player.ZoneBeach && !Player.ZoneMeteor;
            if (etherealExtorter)
            {
                if (Player.ZoneOverworldHeight && ZoneForest)
                {
                    knockback *= 1.15f;
                }
            }
        }
        #endregion

        #region Modify Mana Cost
        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            if (item.type == ItemID.SpaceGun && meteorSet)
            {
                mult *= 0.5f;
            }
        }
        #endregion

        #region Melee Effects
        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (!item.CountsAsClass(DamageClass.Melee) && !item.noMelee && (!item.noUseGraphic && Player.meleeEnchant > 0))
            {
                if (Player.meleeEnchant == 7)
                {
                    if (Main.rand.NextBool(20))
                    {
                        int confettiDust = Main.rand.Next(139, 143);
                        int confetti = Dust.NewDust(new Vector2(hitbox.X,hitbox.Y), hitbox.Width, hitbox.Height, confettiDust, Player.velocity.X, Player.velocity.Y, 0, new Color(), 1.2f);
                        Main.dust[confetti].velocity.X *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.01);
                        Main.dust[confetti].velocity.Y *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.01);
                        Main.dust[confetti].velocity.X += Main.rand.Next(-50, 51) * 0.05f;
                        Main.dust[confetti].velocity.Y += Main.rand.Next(-50, 51) * 0.05f;
                        Main.dust[confetti].scale *= (float)(1.0 + Main.rand.Next(-30, 31) * 0.01);
                    }
                    if (Main.rand.NextBool(40))
                    {
                        int confettiGore = Main.rand.Next(276, 283);
                        int confetti = Gore.NewGore(Entity.GetSource_FromThis(), new Vector2(hitbox.X, hitbox.Y), Player.velocity, confettiGore, 1f);
                        Main.gore[confetti].velocity.X *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.01);
                        Main.gore[confetti].velocity.Y *= (float)(1.0 + Main.rand.Next(-50, 51) * 0.01);
                        Main.gore[confetti].scale *= (float)(1.0 + Main.rand.Next(-20, 21) * 0.01);
                        Main.gore[confetti].velocity.X += Main.rand.Next(-50, 51) * 0.05f;
                        Main.gore[confetti].velocity.Y += Main.rand.Next(-50, 51) * 0.05f;
                    }
                }
            }
            if (item.CountsAsClass(DamageClass.Melee))
            {
                if (fungalSymbiote && Player.whoAmI == Main.myPlayer && fungalSymbioteTimer == 0)
                {
					if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.1) ||
                        Player.itemAnimation == (int)(Player.itemAnimationMax * 0.3) ||
                        Player.itemAnimation == (int)(Player.itemAnimationMax * 0.5) ||
                        Player.itemAnimation == (int)(Player.itemAnimationMax * 0.7) ||
                        Player.itemAnimation == (int)(Player.itemAnimationMax * 0.9))
                    {
						fungalSymbioteTimer = 3;
						float yVel = 0f;
                        float xVel = 0f;
                        float yOffset = 0f;
                        float xOffset = 0f;
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.9))
                        {
                            yVel = -7f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.7))
                        {
                            yVel = -6f;
                            xVel = 2f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.5))
                        {
                            yVel = -4f;
                            xVel = 4f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.3))
                        {
                            yVel = -2f;
                            xVel = 6f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.1))
                        {
                            xVel = 7f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.7))
                        {
                            xOffset = 26f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.3))
                        {
                            xOffset -= 4f;
                            yOffset -= 20f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.1))
                        {
                            yOffset += 6f;
                        }
                        if (Player.direction == -1)
                        {
                            if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.9))
                            {
                                xOffset -= 8f;
                            }
                            if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.7))
                            {
                                xOffset -= 6f;
                            }
                        }
                        yVel *= 1.5f;
                        xVel *= 1.5f;
                        xOffset *= (float)Player.direction;
                        yOffset *= Player.gravDir;
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), (float)(hitbox.X + hitbox.Width / 2) + xOffset, (float)(hitbox.Y + hitbox.Height / 2) + yOffset, (float)Player.direction * xVel, yVel * Player.gravDir, ProjectileID.Mushroom, CalamityUtils.DamageSoftCap(item.damage * 0.25 * Player.MeleeDamage(), 100), 0f, Player.whoAmI, 0f, 0f);
                    }
                }
                if (aWeapon)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<BrimstoneFlame>(), Player.velocity.X * 0.2f + Player.direction * 3f, Player.velocity.Y * 0.2f, 100, default, 0.75f);
                    }
                }
                if (eGauntlet)
                {
                    if (Main.rand.NextBool(3))
                    {
                        int element = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 66, Player.velocity.X * 0.2f + Player.direction * 3f, Player.velocity.Y * 0.2f, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.25f);
                        Main.dust[element].noGravity = true;
                    }
                }
                if (cryogenSoul)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 67, Player.velocity.X * 0.2f + Player.direction * 3f, Player.velocity.Y * 0.2f, 100, default, 0.75f);
                    }
                }
                if (xerocSet)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 58, Player.velocity.X * 0.2f + Player.direction * 3f, Player.velocity.Y * 0.2f, 100, default, 1.25f);
                    }
                }
                if (reaverBlast)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 74, Player.velocity.X * 0.2f + Player.direction * 3f, Player.velocity.Y * 0.2f, 100, default, 0.75f);
                    }
                }
                if (dsSetBonus)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 27, Player.velocity.X * 0.2f + Player.direction * 3f, Player.velocity.Y * 0.2f, 100, default, 2.5f);
                    }
                }
            }
        }
        #endregion

        #region On Hit NPC
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
			if (desertProwler && item.CountsAsClass(DamageClass.Ranged) && hit.Crit) //for obscure stuff like marnite bayonet
			{
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<DesertMark>()] < 1 && Player.ownedProjectileCounts[ModContent.ProjectileType<DesertTornado>()] < 1)
				{
					if (Main.rand.NextBool(15))
					{
						if (Player.whoAmI == Main.myPlayer)
						{
							Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<DesertMark>(), CalamityUtils.DamageSoftCap(item.damage * Player.RangedDamage(), 50), item.knockBack, Player.whoAmI, 0f, 0f);
						}
					}
				}
			}
            if (!item.CountsAsClass(DamageClass.Melee) && Player.meleeEnchant == 7)
                Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center, target.velocity, ProjectileID.ConfettiMelee, 0, 0f, Player.whoAmI, 0f, 0f);

            if (omegaBlueChestplate)
                target.AddBuff(ModContent.BuffType<CrushDepth>(), 240);
            if (sulfurSet)
                target.AddBuff(BuffID.Poisoned, 120);
            switch (item.type)
            {
                case ItemID.IceSickle:
                case ItemID.Frostbrand:
                    target.AddBuff(BuffID.Frostburn, 600);
                    break;

                case ItemID.IceBlade:
                    if (Main.rand.NextBool(5))
                        target.AddBuff(BuffID.Frostburn, 360);
                    else if (Main.rand.NextBool(3))
                        target.AddBuff(BuffID.Frostburn, 120);
                    break;
            }

            if (item.CountsAsClass(DamageClass.Melee)) //prevents Deep Sea Dumbell from snagging true melee debuff memes
            {
				titanBoost = 600;
                if (eGauntlet)
                {
					int duration = 90;
                    target.AddBuff(BuffID.CursedInferno, duration / 2, false);
                    target.AddBuff(BuffID.Frostburn, duration, false);
                    target.AddBuff(BuffID.Ichor, duration, false);
                    target.AddBuff(BuffID.Venom, duration, false);
                    target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), duration, false);
                    target.AddBuff(ModContent.BuffType<AbyssalFlames>(), duration, false);
                    target.AddBuff(ModContent.BuffType<HolyFlames>(), duration, false);
                    target.AddBuff(ModContent.BuffType<Plague>(), duration, false);
                    target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), duration, false);
                    if (Main.rand.NextBool(5))
                    {
                        target.AddBuff(ModContent.BuffType<GlacialState>(), duration, false);
                    }
                }
                if (cryogenSoul || frostFlare)
                {
					CalamityUtils.Inflict246DebuffsNPC(target, BuffID.Frostburn);
                }
                if (yInsignia)
                {
					CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<HolyFlames>());
                }
                if (ataxiaFire)
                {
					CalamityUtils.Inflict246DebuffsNPC(target, BuffID.OnFire, 4f);
                }
				if (aWeapon)
				{
					CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<AbyssalFlames>());
				}
            }
            if (abyssalAmulet)
            {
				CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<CrushDepth>());
            }
            if (dsSetBonus)
            {
				CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<DemonFlames>());
            }
            if (alchFlask)
            {
				CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<Plague>());
            }
            if (armorCrumbling || armorShattering)
            {
                if (item.CountsAsClass(DamageClass.Melee) || item.Calamity().rogue)
                {
					CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<ArmorCrunch>());
                }
            }
            if (item.Calamity().rogue)
            {
				switch (Player.meleeEnchant)
				{
					case 1:
						target.AddBuff(BuffID.Venom, 60 * Main.rand.Next(5, 10), false);
						break;
					case 2:
						target.AddBuff(BuffID.CursedInferno, 60 * Main.rand.Next(3, 7), false);
						break;
					case 3:
						target.AddBuff(BuffID.OnFire, 60 * Main.rand.Next(3, 7), false);
						break;
					case 5:
						target.AddBuff(BuffID.Ichor, 60 * Main.rand.Next(10, 20), false);
						break;
					case 6:
						target.AddBuff(BuffID.Confused, 60 * Main.rand.Next(1, 4), false);
						break;
					case 8:
						target.AddBuff(BuffID.Poisoned, 60 * Main.rand.Next(5, 10), false);
						break;
					case 4:
						target.AddBuff(BuffID.Midas, 120, false);
						break;
				}
				if (titanHeartMask)
				{
					target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 60 * Main.rand.Next(1,6), false); // 1 to 5 seconds
				}
            }
            if (holyWrath)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 600, false);
            }
            if (vexation)
            {
                if ((Player.armor[0].type == ModContent.ItemType<ReaverCap>() || Player.armor[0].type == ModContent.ItemType<ReaverHelm>() ||
                    Player.armor[0].type == ModContent.ItemType<ReaverHelmet>() || Player.armor[0].type == ModContent.ItemType<ReaverMask>() ||
                    Player.armor[0].type == ModContent.ItemType<ReaverVisage>()) &&
                    Player.armor[1].type == ModContent.ItemType<ReaverScaleMail>() && Player.armor[2].type == ModContent.ItemType<ReaverCuisses>())
                {
                    target.AddBuff(BuffID.CursedInferno, 90, false);
                    target.AddBuff(BuffID.Venom, 120, false);
                }
            }
        }
        #endregion

        #region On Hit NPC With Proj
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            switch (proj.type)
            {
                case ProjectileID.BoneArrow:
                    target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 300);
                    break;

                case ProjectileID.FrostBlastFriendly:
                case ProjectileID.NorthPoleWeapon:
                    target.AddBuff(BuffID.Frostburn, 600);
                    break;

                case ProjectileID.FrostBoltStaff:
                case ProjectileID.IceSickle:
                case ProjectileID.FrostBoltSword:
                case ProjectileID.FrostArrow:
                case ProjectileID.NorthPoleSpear:
                    target.AddBuff(BuffID.Frostburn, 480);
                    break;

                case ProjectileID.Blizzard:
                case ProjectileID.NorthPoleSnowflake:
                    target.AddBuff(BuffID.Frostburn, 240);
                    break;

                case ProjectileID.SnowBallFriendly:
                    if (Main.rand.NextBool(10))
                        target.AddBuff(BuffID.Frostburn, 120);
                    else if (Main.rand.NextBool(5))
                        target.AddBuff(BuffID.Frostburn, 60);
                    break;

                case ProjectileID.IceBoomerang:
                case ProjectileID.IceBolt:
                case ProjectileID.FrostDaggerfish:
                    if (Main.rand.NextBool(5))
                        target.AddBuff(BuffID.Frostburn, 240);
                    else if (Main.rand.NextBool(3))
                        target.AddBuff(BuffID.Frostburn, 120);
                    break;
            }

            if (!proj.npcProj && !proj.trap)
            {
				if (desertProwler && proj.CountsAsClass(DamageClass.Ranged) && hit.Crit)
				{
					if (Player.ownedProjectileCounts[ModContent.ProjectileType<DesertMark>()] < 1 && Player.ownedProjectileCounts[ModContent.ProjectileType<DesertTornado>()] < 1)
					{
						if (Main.rand.NextBool(15))
						{
							if (Player.whoAmI == Main.myPlayer)
							{
								Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<DesertMark>(), CalamityUtils.DamageSoftCap(proj.damage, 50), proj.knockBack, Player.whoAmI, 0f, 0f);
							}
						}
					}
				}

				if (proj.Calamity().trueMelee)
					titanBoost = 600;

                if (sulfurSet && proj.friendly && !target.friendly)
                    target.AddBuff(BuffID.Poisoned, 120);

                if (omegaBlueChestplate && proj.friendly && !target.friendly)
                    target.AddBuff(ModContent.BuffType<CrushDepth>(), 240);

                if (proj.CountsAsClass(DamageClass.Melee) && silvaMelee && Main.rand.NextBool(4))
                    target.AddBuff(ModContent.BuffType<SilvaStun>(), 20);

                if (abyssalAmulet)
                {
					CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<CrushDepth>());
                }
                if (dsSetBonus)
                {
					CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<DemonFlames>());
                }
                if ((plaguebringerCarapace || uberBees) && CalamityLists.friendlyBeeList.Contains(proj.type))
                {
                    target.AddBuff(ModContent.BuffType<Plague>(), 360);
                }
                else if (alchFlask)
                {
					CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<Plague>());
                }
                if (proj.CountsAsClass(DamageClass.Melee))
                {
                    if (eGauntlet)
                    {
						int duration = 90;
                        target.AddBuff(BuffID.CursedInferno, duration / 2, false);
                        target.AddBuff(BuffID.Frostburn, duration, false);
                        target.AddBuff(BuffID.Ichor, duration, false);
                        target.AddBuff(BuffID.Venom, duration, false);
                        target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), duration, false);
                        target.AddBuff(ModContent.BuffType<AbyssalFlames>(), duration, false);
                        target.AddBuff(ModContent.BuffType<HolyFlames>(), duration, false);
                        target.AddBuff(ModContent.BuffType<Plague>(), duration, false);
                        target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), duration, false);
                        if (Main.rand.NextBool(5))
                        {
                            target.AddBuff(ModContent.BuffType<GlacialState>(), duration, false);
                        }
                    }
                    if (aWeapon)
                    {
						CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<AbyssalFlames>());
                    }
                    if (cryogenSoul || frostFlare)
                    {
						CalamityUtils.Inflict246DebuffsNPC(target, BuffID.Frostburn);
                    }
                    if (yInsignia)
                    {
						CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<HolyFlames>());
                    }
                    if (ataxiaFire)
                    {
						CalamityUtils.Inflict246DebuffsNPC(target, BuffID.OnFire, 4f);
                    }
                }
                if (armorCrumbling || armorShattering)
                {
                    if (proj.CountsAsClass(DamageClass.Melee) || proj.Calamity().rogue)
                    {
						CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<ArmorCrunch>());
                    }
                }
                if (perforatorLore)
                {
                    target.AddBuff(BuffID.Ichor, 90);
                }
                if (hiveMindLore)
                {
                    target.AddBuff(BuffID.CursedInferno, 90);
                }
                if (holyWrath)
                {
                    target.AddBuff(ModContent.BuffType<HolyFlames>(), 600, false);
                }
                else if (providenceLore)
                {
                    target.AddBuff(ModContent.BuffType<HolyFlames>(), 420, false);
                }
                if (proj.Calamity().rogue)
                {
					switch (Player.meleeEnchant)
					{
						case 1:
							target.AddBuff(BuffID.Venom, 60 * Main.rand.Next(5, 10), false);
							break;
						case 2:
							target.AddBuff(BuffID.CursedInferno, 60 * Main.rand.Next(3, 7), false);
							break;
						case 3:
							target.AddBuff(BuffID.OnFire, 60 * Main.rand.Next(3, 7), false);
							break;
						case 5:
							target.AddBuff(BuffID.Ichor, 60 * Main.rand.Next(10, 20), false);
							break;
						case 6:
							target.AddBuff(BuffID.Confused, 60 * Main.rand.Next(1, 4), false);
							break;
						case 8:
							target.AddBuff(BuffID.Poisoned, 60 * Main.rand.Next(5, 10), false);
							break;
						case 4:
							target.AddBuff(BuffID.Midas, 120, false);
							break;
					}
                    if (etherealExtorter)
                    {
                        if (ZoneSunkenSea)
                        {
                            target.AddBuff(ModContent.BuffType<TemporalSadness>(), 60, false);
                        }
                        if (ZoneSulphur)
                        {
                            target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 120, false);
                            target.AddBuff(ModContent.BuffType<Irradiated>(), 300, false);
                        }
                        if (Main.moonPhase == 6) //first quarter
                        {
                            target.AddBuff(BuffID.Midas, 120, false);
                        }
                        if (ZoneCalamity && CalamityLists.fireWeaponList.Contains(Player.ActiveItem().type))
                        {
                            target.AddBuff(ModContent.BuffType<AbyssalFlames>(), 240, false);
                        }
                    }
					if (titanHeartMask)
					{
						target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 60 * Main.rand.Next(1,6), false); // 1 to 5 seconds
					}
                }
                if (vexation)
                {
                    if ((Player.armor[0].type == ModContent.ItemType<ReaverCap>() || Player.armor[0].type == ModContent.ItemType<ReaverHelm>() ||
                        Player.armor[0].type == ModContent.ItemType<ReaverHelmet>() || Player.armor[0].type == ModContent.ItemType<ReaverMask>() ||
                        Player.armor[0].type == ModContent.ItemType<ReaverVisage>()) &&
                        Player.armor[1].type == ModContent.ItemType<ReaverScaleMail>() && Player.armor[2].type == ModContent.ItemType<ReaverCuisses>())
                    {
                        target.AddBuff(BuffID.CursedInferno, 90, false);
                        target.AddBuff(BuffID.Venom, 120, false);
                    }
                }
            }
			proj.Calamity().stealthStrikeHitCount++;
        }
        #endregion

        #region PvP
        
        
        //public override void OnHitPvp(Item item, Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHurt on the receiving player and check info.PvP. Use info.DamageSource.SourcePlayerIndex to get the attacking player */
        /*
        {
			if (desertProwler && item.CountsAsClass(DamageClass.Ranged) && crit) //for obscure stuff like Marnite Bayonet
			{
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<DesertMark>()] < 1 && Player.ownedProjectileCounts[ModContent.ProjectileType<DesertTornado>()] < 1)
				{
					if (Main.rand.NextBool(15))
					{
						if (Player.whoAmI == Main.myPlayer)
						{
							Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<DesertMark>(), CalamityUtils.DamageSoftCap(item.damage * Player.RangedDamage(), 50), item.knockBack, Player.whoAmI, 0f, 0f);
						}
					}
				}
			}

            if (!item.CountsAsClass(DamageClass.Melee) && Player.meleeEnchant == 7)
                Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center, target.velocity, ProjectileID.ConfettiMelee, 0, 0f, Player.whoAmI, 0f, 0f);

            if (omegaBlueChestplate)
                target.AddBuff(ModContent.BuffType<CrushDepth>(), 240);
            if (sulfurSet)
                target.AddBuff(BuffID.Poisoned, 120);
            switch (item.type)
            {
                case ItemID.IceSickle:
                case ItemID.Frostbrand:
                    target.AddBuff(BuffID.Frostburn, 600);
                    break;

                case ItemID.IceBlade:
                    if (Main.rand.NextBool(5))
                        target.AddBuff(BuffID.Frostburn, 360);
                    else if (Main.rand.NextBool(3))
                        target.AddBuff(BuffID.Frostburn, 120);
                    break;
            }

            if (item.CountsAsClass(DamageClass.Melee))
            {
				titanBoost = 600;
                if (eGauntlet)
                {
					int duration = 90;
                    target.AddBuff(BuffID.CursedInferno, duration / 2, false);
                    target.AddBuff(BuffID.Frostburn, duration, false);
                    target.AddBuff(BuffID.Ichor, duration, false);
                    target.AddBuff(BuffID.Venom, duration, false);
                    target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), duration, false);
                    target.AddBuff(ModContent.BuffType<AbyssalFlames>(), duration, false);
                    target.AddBuff(ModContent.BuffType<HolyFlames>(), duration, false);
                    target.AddBuff(ModContent.BuffType<Plague>(), duration, false);
                    target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), duration, false);
                    if (Main.rand.NextBool(5))
                    {
                        target.AddBuff(ModContent.BuffType<GlacialState>(), duration, false);
                    }
                }
                if (aWeapon)
                {
					CalamityUtils.Inflict246DebuffsPvp(target, ModContent.BuffType<AbyssalFlames>());
                }
                if (cryogenSoul || frostFlare)
                {
					CalamityUtils.Inflict246DebuffsPvp(target, BuffID.Frostburn);
                }
                if (yInsignia)
                {
					CalamityUtils.Inflict246DebuffsPvp(target, ModContent.BuffType<HolyFlames>());
                }
                if (ataxiaFire)
                {
					CalamityUtils.Inflict246DebuffsPvp(target, BuffID.OnFire, 4f);
                }
            }
            if (alchFlask)
            {
				CalamityUtils.Inflict246DebuffsPvp(target, ModContent.BuffType<Plague>());
            }
			if (abyssalAmulet)
			{
				CalamityUtils.Inflict246DebuffsPvp(target, ModContent.BuffType<CrushDepth>());
			}
            if (item.Calamity().rogue)
            {
				switch (Player.meleeEnchant)
				{
					case 1:
						target.AddBuff(BuffID.Venom, 60 * Main.rand.Next(5, 10), false);
						break;
					case 2:
						target.AddBuff(BuffID.CursedInferno, 60 * Main.rand.Next(3, 7), false);
						break;
					case 3:
						target.AddBuff(BuffID.OnFire, 60 * Main.rand.Next(3, 7), false);
						break;
					case 5:
						target.AddBuff(BuffID.Ichor, 60 * Main.rand.Next(10, 20), false);
						break;
					case 6:
						target.AddBuff(BuffID.Confused, 60 * Main.rand.Next(1, 4), false);
						break;
					case 8:
						target.AddBuff(BuffID.Poisoned, 60 * Main.rand.Next(5, 10), false);
						break;
					/*case 4:
						target.AddBuff(BuffID.Midas, 120, false);
						break;*/
				/*}
				if (titanHeartMask)
				{
					target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 60 * Main.rand.Next(1,6), false); // 1 to 5 seconds
				}
            }
            if (holyWrath)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 600, false);
            }
            if (vexation)
            {
                if ((Player.armor[0].type == ModContent.ItemType<ReaverCap>() || Player.armor[0].type == ModContent.ItemType<ReaverHelm>() ||
                    Player.armor[0].type == ModContent.ItemType<ReaverHelmet>() || Player.armor[0].type == ModContent.ItemType<ReaverMask>() ||
                    Player.armor[0].type == ModContent.ItemType<ReaverVisage>()) &&
                    Player.armor[1].type == ModContent.ItemType<ReaverScaleMail>() && Player.armor[2].type == ModContent.ItemType<ReaverCuisses>())
                {
                    target.AddBuff(BuffID.CursedInferno, 90, false);
                    target.AddBuff(BuffID.Venom, 120, false);
                }
            }
        }
		*/

        //public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHurt on the receiving player and check info.PvP. Use info.DamageSource.SourcePlayerIndex to get the attacking player */
        /*
		{
            switch (proj.type)
            {
                case ProjectileID.BoneArrow:
                    target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 300);
                    break;

                case ProjectileID.FrostBlastFriendly:
                case ProjectileID.NorthPoleWeapon:
                    target.AddBuff(BuffID.Frostburn, 600);
                    break;

                case ProjectileID.FrostBoltStaff:
                case ProjectileID.IceSickle:
                case ProjectileID.FrostBoltSword:
                case ProjectileID.FrostArrow:
                case ProjectileID.NorthPoleSpear:
                    target.AddBuff(BuffID.Frostburn, 480);
                    break;

                case ProjectileID.Blizzard:
                case ProjectileID.NorthPoleSnowflake:
                    target.AddBuff(BuffID.Frostburn, 240);
                    break;

                case ProjectileID.SnowBallFriendly:
                    if (Main.rand.NextBool(10))
                        target.AddBuff(BuffID.Frostburn, 120);
                    else if (Main.rand.NextBool(5))
                        target.AddBuff(BuffID.Frostburn, 60);
                    break;

                case ProjectileID.IceBoomerang:
                case ProjectileID.IceBolt:
                case ProjectileID.FrostDaggerfish:
                    if (Main.rand.NextBool(5))
                        target.AddBuff(BuffID.Frostburn, 240);
                    else if (Main.rand.NextBool(3))
                        target.AddBuff(BuffID.Frostburn, 120);
                    break;
            }

            if (!proj.npcProj && !proj.trap)
            {
				if (desertProwler && proj.CountsAsClass(DamageClass.Ranged) && crit)
				{
					if (Player.ownedProjectileCounts[ModContent.ProjectileType<DesertMark>()] < 1 && Player.ownedProjectileCounts[ModContent.ProjectileType<DesertTornado>()] < 1)
					{
						if (Main.rand.NextBool(15))
						{
							if (Player.whoAmI == Main.myPlayer)
							{
								Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<DesertMark>(), CalamityUtils.DamageSoftCap(proj.damage, 50), proj.knockBack, Player.whoAmI, 0f, 0f);
							}
						}
					}
				}

				if (proj.Calamity().trueMelee)
					titanBoost = 600;

                if (sulfurSet && proj.friendly)
                    target.AddBuff(BuffID.Poisoned, 120);

                if (omegaBlueChestplate && proj.friendly)
                    target.AddBuff(ModContent.BuffType<CrushDepth>(), 240);

                if (proj.CountsAsClass(DamageClass.Melee) && silvaMelee && Main.rand.NextBool(4))
                    target.AddBuff(ModContent.BuffType<SilvaStun>(), 20);


                if (abyssalAmulet)
                {
					CalamityUtils.Inflict246DebuffsPvp(target, ModContent.BuffType<CrushDepth>());
                }
                if ((plaguebringerCarapace || uberBees) && CalamityLists.friendlyBeeList.Contains(proj.type))
                {
                    target.AddBuff(ModContent.BuffType<Plague>(), 360);
                }
                else if (alchFlask)
                {
					CalamityUtils.Inflict246DebuffsPvp(target, ModContent.BuffType<Plague>());
                }
                if (proj.CountsAsClass(DamageClass.Melee))
                {
                    if (eGauntlet)
                    {
						int duration = 90;
                        target.AddBuff(BuffID.CursedInferno, duration / 2, false);
                        target.AddBuff(BuffID.Frostburn, duration, false);
                        target.AddBuff(BuffID.Ichor, duration, false);
                        target.AddBuff(BuffID.Venom, duration, false);
                        target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), duration, false);
                        target.AddBuff(ModContent.BuffType<AbyssalFlames>(), duration, false);
                        target.AddBuff(ModContent.BuffType<HolyFlames>(), duration, false);
                        target.AddBuff(ModContent.BuffType<Plague>(), duration, false);
                        target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), duration, false);
                        if (Main.rand.NextBool(5))
                        {
                            target.AddBuff(ModContent.BuffType<GlacialState>(), duration, false);
                        }
                    }
                    if (aWeapon)
                    {
						CalamityUtils.Inflict246DebuffsPvp(target, ModContent.BuffType<AbyssalFlames>());
                    }
                    if (cryogenSoul || frostFlare)
                    {
						CalamityUtils.Inflict246DebuffsPvp(target, BuffID.Frostburn);
                    }
                    if (yInsignia)
                    {
						CalamityUtils.Inflict246DebuffsPvp(target, ModContent.BuffType<HolyFlames>());
                    }
                    if (ataxiaFire)
                    {
						CalamityUtils.Inflict246DebuffsPvp(target, BuffID.OnFire, 4f);
                    }
                }
                if (armorCrumbling || armorShattering)
                {
                    if (proj.CountsAsClass(DamageClass.Melee) || proj.Calamity().rogue)
                    {
						CalamityUtils.Inflict246DebuffsPvp(target, ModContent.BuffType<ArmorCrunch>());
                    }
                }
                if (perforatorLore)
                {
                    target.AddBuff(BuffID.Ichor, 90);
                }
                if (hiveMindLore)
                {
                    target.AddBuff(BuffID.CursedInferno, 90);
                }
                if (holyWrath)
                {
                    target.AddBuff(ModContent.BuffType<HolyFlames>(), 600, false);
                }
                else if (providenceLore)
                {
                    target.AddBuff(ModContent.BuffType<HolyFlames>(), 420, false);
                }
                if (proj.Calamity().rogue)
                {
					switch (Player.meleeEnchant)
					{
						case 1:
							target.AddBuff(BuffID.Venom, 60 * Main.rand.Next(5, 10), false);
							break;
						case 2:
							target.AddBuff(BuffID.CursedInferno, 60 * Main.rand.Next(3, 7), false);
							break;
						case 3:
							target.AddBuff(BuffID.OnFire, 60 * Main.rand.Next(3, 7), false);
							break;
						case 5:
							target.AddBuff(BuffID.Ichor, 60 * Main.rand.Next(10, 20), false);
							break;
						case 6:
							target.AddBuff(BuffID.Confused, 60 * Main.rand.Next(1, 4), false);
							break;
						case 8:
							target.AddBuff(BuffID.Poisoned, 60 * Main.rand.Next(5, 10), false);
							break;
						/*case 4:
							target.AddBuff(BuffID.Midas, 120, false);
							break;*/
					/*}
                    if (etherealExtorter)
                    {
                        if (ZoneSunkenSea)
                        {
                            target.AddBuff(ModContent.BuffType<TemporalSadness>(), 60, false);
                        }
                        if (ZoneSulphur)
                        {
                            target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 120, false);
                            target.AddBuff(ModContent.BuffType<Irradiated>(), 300, false);
                        }
                        /*if (Main.moonPhase == 6) //first quarter
                        {
                            target.AddBuff(BuffID.Midas, 120, false);
                        }*/
                        /*if (ZoneCalamity && CalamityLists.fireWeaponList.Contains(Player.ActiveItem().type))
                        {
                            target.AddBuff(ModContent.BuffType<AbyssalFlames>(), 240, false);
                        }
                    }
					if (titanHeartMask)
					{
						target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 60 * Main.rand.Next(1,6), false); // 1 to 5 seconds
					}
                }
                if (vexation)
                {
                    if ((Player.armor[0].type == ModContent.ItemType<ReaverCap>() || Player.armor[0].type == ModContent.ItemType<ReaverHelm>() ||
                        Player.armor[0].type == ModContent.ItemType<ReaverHelmet>() || Player.armor[0].type == ModContent.ItemType<ReaverMask>() ||
                        Player.armor[0].type == ModContent.ItemType<ReaverVisage>()) &&
                        Player.armor[1].type == ModContent.ItemType<ReaverScaleMail>() && Player.armor[2].type == ModContent.ItemType<ReaverCuisses>())
                    {
                        target.AddBuff(BuffID.CursedInferno, 90, false);
                        target.AddBuff(BuffID.Venom, 120, false);
                    }
                }
                if (proj.type == ProjectileID.IchorArrow && Player.ActiveItem().type == ModContent.ItemType<RaidersGlory>())
                {
                    target.AddBuff(BuffID.Midas, 300, false);
                }
            }
        }*/
        #endregion

        #region Modify Hit NPC
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Item, consider using ModifyHitNPC instead */
        {
            #region MultiplierBoosts
            double damageMult = 1.0;
            if (silvaMelee && Main.rand.NextBool(4) && item.CountsAsClass(DamageClass.Melee))
            {
                damageMult += 4.0;
            }
			if (item.CountsAsClass(DamageClass.Melee))
			{
                damageMult += trueMeleeDamage;
			}
            if (enraged && !CalamityConfig.Instance.BossRushXerocCurse)
            {
                damageMult += 1.25;
            }
            if (CalamityWorld.revenge && CalamityConfig.Instance.Rippers)
            {
                bool DHorHoD = draedonsHeart || heartOfDarkness;
                if (rageModeActive && adrenalineModeActive)
                {
                    if (item.CountsAsClass(DamageClass.Melee))
                    {
                        damageMult += DHorHoD ? 3.1 : 2.8;
                    }
                }
                else if (rageModeActive)
                {
                    if (item.CountsAsClass(DamageClass.Melee))
                    {
                        double rageDamageBoost = 0.0 +
                            (rageBoostOne ? 0.15 : 0.0) +
                            (rageBoostTwo ? 0.15 : 0.0) +
                            (rageBoostThree ? 0.15 : 0.0);
                        double rageDamage = (DHorHoD ? 0.65 : 0.5) + rageDamageBoost;
                        damageMult += rageDamage;
                    }
                }
                else if (adrenalineModeActive)
                {
                    if (item.CountsAsClass(DamageClass.Melee))
                    {
						double adrenalineDamageBoost = 0D +
							(adrenalineBoostOne ? 0.15 : 0D) +
							(adrenalineBoostTwo ? 0.15 : 0D) +
							(adrenalineBoostThree ? 0.15 : 0D);
						damageMult += 2D + adrenalineDamageBoost;
					}
                }
            }
            modifiers.SourceDamage *= (float)damageMult;

            if (oldDie)
            {
                float diceMult = Main.rand.NextFloat(0.8824f, 1.1565f);
                if (item.Calamity().rogue || wearingRogueArmor)
                {
                    float roll2 = Main.rand.NextFloat(0.8824f, 1.1565f);
                    diceMult = roll2 > diceMult ? roll2 : diceMult;
                }
                modifiers.SourceDamage *= (float)diceMult;
            }
            #endregion

            #region AdditiveBoosts
            if (item.CountsAsClass(DamageClass.Melee) && badgeOfBravery)
            {
                if ((Player.armor[0].type == ModContent.ItemType<TarragonHelmet>() || Player.armor[0].type == ModContent.ItemType<TarragonHelm>() ||
                    Player.armor[0].type == ModContent.ItemType<TarragonHornedHelm>() || Player.armor[0].type == ModContent.ItemType<TarragonMask>() ||
                    Player.armor[0].type == ModContent.ItemType<TarragonVisage>()) &&
                    Player.armor[1].type == ModContent.ItemType<TarragonBreastplate>() && Player.armor[2].type == ModContent.ItemType<TarragonLeggings>())
                {
                    int penetratableDefense = (int)Math.Max(target.defense - Player.GetArmorPenetration(DamageClass.Generic), 0);
                    int penetratedDefense = Math.Min(penetratableDefense, 10);
                    modifiers.SourceDamage += (int)(0.5f * penetratedDefense);
                }
            }
            #endregion

            if (yharonLore)
	            modifiers.SourceDamage *= 0.75f;

            if ((target.damage > 5 || target.boss) && Player.whoAmI == Main.myPlayer && !target.SpawnedFromStatue)
            {
                if (item.CountsAsClass(DamageClass.Melee) && soaring)
                {
                    double useTimeMultiplier = 0.85 + (item.useTime * item.useAnimation / 3600D); //28 * 28 = 784 is average so that equals 784 / 3600 = 0.217777 + 1 = 21.7% boost
                    double wingTimeFraction = Player.wingTimeMax / 20D;
                    double meleeStatMultiplier = (double)(Player.GetDamage(DamageClass.Melee).Multiplicative * (float)(Player.GetCritChance(DamageClass.Melee) / 10D));

                    if (Player.wingTime < Player.wingTimeMax)
                        Player.wingTime += (int)(useTimeMultiplier * (wingTimeFraction + meleeStatMultiplier));

                    if (Player.wingTime > Player.wingTimeMax)
                        Player.wingTime = Player.wingTimeMax;
                }
                if (item.CountsAsClass(DamageClass.Melee) && !item.noMelee && !item.noUseGraphic)
                {
                    if (ataxiaGeyser)
                    {
                        if (Player.ownedProjectileCounts[ModContent.ProjectileType<ChaosGeyser>()] < 3)
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<ChaosGeyser>(), CalamityUtils.DamageSoftCap((double)modifiers.SourceDamage.Multiplicative * 0.15, 45), 2f, Player.whoAmI, 0f, 0f);
                        }
                    }
				}
				if (unstablePrism && modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit)
				{
					for (int s = 0; s < 3; s++)
					{
						Vector2 velocity = CalamityUtils.RandomVelocity(50f, 30f, 60f);
						Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center, velocity, ModContent.ProjectileType<UnstableSpark>(), CalamityUtils.DamageSoftCap(item.damage * 0.15, 30), 0f, Player.whoAmI);
					}
				}
                if (astralStarRain && modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit && astralStarRainCooldown <= 0)
                {
                    astralStarRainCooldown = 60;
                    for (int n = 0; n < 3; n++)
                    {
						int projectileType = Utils.SelectRandom(Main.rand, new int[]
						{
							ModContent.ProjectileType<AstralStar>(),
							ProjectileID.HallowStar,
							ModContent.ProjectileType<FallenStarProj>()
						});
						CalamityUtils.ProjectileRain(target.GetSource_FromThis(), target.Center, 400f, 100f, 500f, 800f, 25f, projectileType, (int)(120 * Player.AverageDamage()), 5f, Player.whoAmI, 6);
                    }
                }
                if (bloodflareMelee && item.CountsAsClass(DamageClass.Melee))
                {
                    if (bloodflareMeleeHits < 15 && !bloodflareFrenzy && !bloodFrenzyCooldown)
                    {
                        bloodflareMeleeHits++;
                    }
                    if (Player.whoAmI == Main.myPlayer && target.canGhostHeal)
                    {
                        int healAmount = Main.rand.Next(3) + 1;
                        Player.statLife += healAmount;
                        Player.HealEffect(healAmount);
                    }
                }
                if (CalamityConfig.Instance.Proficiency)
                {
                    if (gainLevelCooldown <= 0)
                    {
                        gainLevelCooldown = 120;
                        if (item.CountsAsClass(DamageClass.Melee) && meleeLevel <= 12500)
                        {
							if (!ReduceCooldown((int)ClassType.Melee))
							{
								if (!Main.hardMode && meleeLevel >= 1500)
									gainLevelCooldown = 1200; //20 seconds
								if (!NPC.downedMoonlord && meleeLevel >= 5500)
									gainLevelCooldown = 2400; //40 seconds
							}
							else
								gainLevelCooldown /= 2;

							if (fasterMeleeLevel)
								gainLevelCooldown /= 2;

                            meleeLevel++;
                            shootFireworksLevelUpMelee = true;

                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                LevelPacket(false, (int)ClassType.Melee);
                        }
                    }
                }
                if (CalamityWorld.revenge && CalamityConfig.Instance.Rippers)
                {
                    if (item.CountsAsClass(DamageClass.Melee))
                    {
                        int stressGain = (int)(item.damage * 0.1);
                        int stressMaxGain = 10;
                        if (stressGain < 1)
                        {
                            stressGain = 1;
                        }
                        if (stressGain > stressMaxGain)
                        {
                            stressGain = stressMaxGain;
                        }
                        rage += stressGain;
                        if (rage >= rageMax)
                        {
                            rage = rageMax;
                        }
                    }
                }
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
        {
            if (proj.npcProj || proj.trap)
                return;

            bool isTrueMelee = proj.Calamity().trueMelee;
            bool isSummon = proj.IsSummon();
            bool hasClassType = proj.CountsAsClass(DamageClass.Melee) || proj.CountsAsClass(DamageClass.Ranged) || proj.CountsAsClass(DamageClass.Magic) || isSummon || proj.Calamity().rogue;

            Item heldItem = Player.ActiveItem();

            if (isTrueMelee && soaring)
            {
                double useTimeMultiplier = 0.85 + (heldItem.useTime * heldItem.useAnimation / 3600D); //28 * 28 = 784 is average so that equals 784 / 3600 = 0.217777 + 1 = 21.7% boost
                double wingTimeFraction = Player.wingTimeMax / 20D;
                double meleeStatMultiplier = Player.GetDamage(DamageClass.Melee).Multiplicative * (float)(Player.GetCritChance(DamageClass.Melee) / 10D);

                if (Player.wingTime < Player.wingTimeMax)
                    Player.wingTime += (int)(useTimeMultiplier * (wingTimeFraction + meleeStatMultiplier));

                if (Player.wingTime > Player.wingTimeMax)
                    Player.wingTime = Player.wingTimeMax;
            }

            #region MultiplierBoosts
            double damageMult = 1.0;
            if (isSummon)
            {
                if (heldItem.type > ItemID.None)
                {
                    if (heldItem.CountsAsClass(DamageClass.Summon) && !heldItem.CountsAsClass(DamageClass.Melee) && !heldItem.CountsAsClass(DamageClass.Ranged) && !heldItem.CountsAsClass(DamageClass.Magic) && !heldItem.Calamity().rogue)
                    {
                        damageMult += 0.1;
                    }
                }
            }
			if (isTrueMelee)
			{
                damageMult += trueMeleeDamage;
			}
            if (screwdriver)
            {
                if (proj.penetrate > 1 || proj.penetrate == -1)
                    damageMult += 0.1;
            }
            if (sPower)
            {
                if (isSummon)
                    damageMult += 0.1;
            }
            if (hallowedPower)
            {
                if (isSummon)
                    damageMult += 0.15;
            }
            if (providenceLore && hasClassType)
            {
                damageMult += 0.1;
            }
            if (silvaMelee && Main.rand.NextBool(4) && isTrueMelee)
            {
                damageMult += 4.0;
            }
            if (enraged && !CalamityConfig.Instance.BossRushXerocCurse)
            {
                damageMult += 1.25;
            }
            if (auricSet)
            {
                if (silvaThrowing && proj.Calamity().rogue &&
                    modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit && Player.statLife > (int)(Player.statLifeMax2 * 0.5))
                {
                    damageMult += 0.25;
                }
                if (silvaMelee && proj.CountsAsClass(DamageClass.Melee))
                {
                    double multiplier = (double)Player.statLife / (double)Player.statLifeMax2;
                    damageMult += multiplier * 0.2;
                }
            }
            if (godSlayerRanged && modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit && proj.CountsAsClass(DamageClass.Ranged))
            {
                // 100 min to 15 max with cap (prevents crit hyperscaling)
                int randomChance = 100 - (int)Player.GetCritChance(DamageClass.Ranged);
                if (randomChance < 15)
                    randomChance = 15;
                if (Main.rand.NextBool(randomChance))
                    damageMult += 1.0;
            }
            if (silvaCountdown <= 0 && hasSilvaEffect && silvaRanged && proj.CountsAsClass(DamageClass.Ranged))
            {
                damageMult += 0.1;
            }
            if (silvaCountdown <= 0 && hasSilvaEffect && silvaThrowing && proj.Calamity().rogue)
            {
                damageMult += 0.1;
            }
            if (silvaCountdown <= 0 && hasSilvaEffect && silvaMage && proj.CountsAsClass(DamageClass.Magic))
            {
                damageMult += 0.1;
            }
            if (silvaCountdown <= 0 && hasSilvaEffect && silvaSummon && isSummon)
            {
                damageMult += 0.1;
            }
            if (proj.type == ModContent.ProjectileType<FrostsparkBulletProj>())
            {
                if (target.buffImmune[ModContent.BuffType<GlacialState>()])
                    damageMult += 0.1;
            }
            else if (proj.type == ProjectileID.InfernoFriendlyBlast)
            {
                damageMult += 0.33;
            }
            if (brimflameFrenzy && brimflameSet)
            {
                if (proj.CountsAsClass(DamageClass.Magic))
                {
                    damageMult += 0.5;
                }
            }
            if (CalamityWorld.revenge && CalamityConfig.Instance.Rippers)
            {
                bool DHorHoD = draedonsHeart || heartOfDarkness;
                if (rageModeActive && adrenalineModeActive)
                {
                    if (hasClassType)
                    {
                        damageMult += DHorHoD ? 3.1 : 2.8;
                    }
                }
                else if (rageModeActive)
                {
                    if (hasClassType)
                    {
                        double rageDamageBoost = 0D +
                            (rageBoostOne ? 0.15 : 0D) +
                            (rageBoostTwo ? 0.15 : 0D) +
                            (rageBoostThree ? 0.15 : 0D);
                        double rageDamage = (DHorHoD ? 0.65 : 0.5) + rageDamageBoost;
                        damageMult += rageDamage;
                    }
                }
                else if (adrenalineModeActive)
                {
                    if (hasClassType)
                    {
						double adrenalineDamageBoost = 0D +
							(adrenalineBoostOne ? 0.15 : 0D) +
							(adrenalineBoostTwo ? 0.15 : 0D) +
							(adrenalineBoostThree ? 0.15 : 0D);
						damageMult += 2D + adrenalineDamageBoost;
                    }
                }
            }
            if ((filthyGlove || electricianGlove) && proj.Calamity().stealthStrike && proj.Calamity().rogue)
            {
                if (nanotech)
                    damageMult += 0.05;
                else
                    damageMult += 0.1;
            }
            if (etherealExtorter && proj.Calamity().rogue)
            {
                bool ZoneForest = !ZoneAbyss && !ZoneSulphur && !ZoneAstral && !ZoneCalamity && !ZoneSunkenSea && !Player.ZoneSnow && !Player.ZoneCorrupt && !Player.ZoneCrimson && !Player.ZoneHallow && !Player.ZoneDesert && !Player.ZoneUndergroundDesert && !Player.ZoneGlowshroom && !Player.ZoneDungeon && !Player.ZoneBeach && !Player.ZoneMeteor;
                if (Main.moonPhase == 7 && modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit) //Waxing Gibbous
                {
                    damageMult += 0.05;
                }
                if (Main.moonPhase == 5) //Waxing Cresent
                {
                    if (proj.penetrate == -1)
                        damageMult += 0.1;
                    else if (proj.penetrate >= 5)
                        damageMult += 0.08;
                    else if (proj.penetrate == 4)
                        damageMult += 0.06;
                    else if (proj.penetrate == 3)
                        damageMult += 0.03;
                    else if (proj.penetrate == 2)
                        damageMult += 0.02;
                }
                if (Player.ZoneDirtLayerHeight && ZoneForest)
                {
                    if (Main.rand.NextBool(20) && !modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit) //5% chance to minicrit
                        damageMult += 0.5;
                }
            }
            modifiers.SourceDamage *= (float)damageMult;

            if (oldDie)
            {
                float diceMult = Main.rand.NextFloat(0.8824f, 1.1565f);
                if (proj.Calamity().rogue || wearingRogueArmor)
                {
                    float roll2 = Main.rand.NextFloat(0.8824f, 1.1565f);
                    diceMult = roll2 > diceMult ? roll2 : diceMult;
                }
                modifiers.SourceDamage *= diceMult;
            }
            #endregion

            #region AdditiveBoosts
            if (proj.type == ModContent.ProjectileType<AcidBulletProj>())
            {
                int defenseAdd = (int)(target.defense * 0.05 * (proj.damage / 50D) * acidRoundMultiplier); //100 defense * 0.05 = 5
                modifiers.SourceDamage += defenseAdd;
            }
            if (uberBees && CalamityLists.friendlyBeeList.Contains(proj.type))
            {
	            modifiers.SourceDamage += Main.rand.Next(20, 31);
            }
			if (plaguebringerPatronSummon)
			{
				if (isSummon && proj.active && proj.friendly && !proj.npcProj && !proj.trap && proj.damage > 0)
				{
					if (proj.type != ModContent.ProjectileType<DirectStrike>() && proj.type != ModContent.ProjectileType<PlaguebringerSummon>())
					{
						for (int j = 0; j < Main.maxProjectiles; j++)
						{
							Projectile miniPBG = Main.projectile[j];
							if (miniPBG.type == ModContent.ProjectileType<PlaguebringerSummon>() && Vector2.Distance(proj.Center, miniPBG.Center) <= PlaguebringerSummon.auraRange && miniPBG.owner == proj.owner)
							{
								modifiers.SourceDamage += Main.rand.Next(10, 21);
								break;
							}
						}
					}
				}
			}
			int penetrateAmt = 0;
            if (proj.Calamity().stealthStrike && proj.Calamity().rogue)
            {
				if (nanotech)
					penetrateAmt += 20; //nanotech is weaker
				else if (electricianGlove)
					penetrateAmt += 30;
				else if (filthyGlove || bloodyGlove)
					penetrateAmt += 10;
            }
            if (proj.Calamity().rogue && etherealExtorter)
            {
                if (CalamityLists.boomerangProjList.Contains(proj.type) && Player.ZoneCorrupt)
                {
					penetrateAmt += 6;
                }
            }
            if (proj.CountsAsClass(DamageClass.Melee) && badgeOfBravery)
            {
                if ((Player.armor[0].type == ModContent.ItemType<TarragonHelmet>() || Player.armor[0].type == ModContent.ItemType<TarragonHelm>() ||
                    Player.armor[0].type == ModContent.ItemType<TarragonHornedHelm>() || Player.armor[0].type == ModContent.ItemType<TarragonMask>() ||
                    Player.armor[0].type == ModContent.ItemType<TarragonVisage>()) &&
                    Player.armor[1].type == ModContent.ItemType<TarragonBreastplate>() && Player.armor[2].type == ModContent.ItemType<TarragonLeggings>())
                {
					penetrateAmt += 10;
                }
            }
			int penetratableDefense = (int)Math.Max(target.defense - Player.GetArmorPenetration(DamageClass.Generic), 0); //if find how much defense we can penetrate
			int penetratedDefense = Math.Min(penetratableDefense, penetrateAmt); //if we have more penetrate than enemy defense, use enemy defense
			modifiers.SourceDamage += (int)(0.5f * penetratedDefense);
            #endregion

            #region MultiplicativeReductions

            // Fearmonger armor reduces the summoner cross-class nerf
			// Forbidden armor reduces said nerf when holding the respective helmet's preferred weapon type
            // Profaned Soul Crystal encourages use of other weapons, nerfing the damage would not make sense.

            bool forbidden = Player.head == ArmorIDs.Head.AncientBattleArmor && Player.body == ArmorIDs.Body.AncientBattleArmor && Player.legs == ArmorIDs.Legs.AncientBattleArmor;
			bool reducedNerf = fearmongerSet || (forbidden && heldItem.CountsAsClass(DamageClass.Magic));

			double summonNerfMult = reducedNerf ? 0.75 : 0.5;
            if (isSummon && !profanedCrystalBuffs)
            {
				if (heldItem.type > ItemID.None)
				{
					if (!heldItem.CountsAsClass(DamageClass.Summon) &&
						(heldItem.CountsAsClass(DamageClass.Melee) || heldItem.CountsAsClass(DamageClass.Ranged) || heldItem.CountsAsClass(DamageClass.Magic) || heldItem.Calamity().rogue) &&
						heldItem.hammer == 0 && heldItem.pick == 0 && heldItem.axe == 0 && heldItem.useStyle != 0 && 
						!heldItem.accessory && heldItem.ammo == AmmoID.None)
					{
						proj.damage = (int)(proj.damage * summonNerfMult);
					}
				}
            }

            if (proj.CountsAsClass(DamageClass.Ranged))
            {
				// Nerfed in prehardmode due to bullet damage being a balance meme
				if (heldItem.type == ModContent.ItemType<HalibutCannon>())
				{
					if (!Main.hardMode)
						proj.damage = (int)(proj.damage * 0.5);
					if (proj.type == ProjectileID.IchorBullet || proj.type == ModContent.ProjectileType<AcidBulletProj>())
						proj.damage = (int)(proj.damage * 0.85);
				}

                switch (proj.type)
                {
                    case ProjectileID.CrystalShard:
	                    proj.damage = (int)(proj.damage * 0.6);
                        break;
                    case ProjectileID.ChlorophyteBullet:
                        proj.damage = (int)(proj.damage * 0.8);
                        break;
                    case ProjectileID.HallowStar:
	                    proj.damage = (int)(proj.damage * 0.7);
                        break;
                }

                if (proj.type == ModContent.ProjectileType<AcidBulletProj>() && heldItem.type == ModContent.ItemType<P90>())
	                proj.damage = (int)(proj.damage * 0.75);
            }

            if (proj.type == ProjectileID.SpectreWrath && Player.ghostHurt)
	            proj.damage = (int)(proj.damage * 0.7);

            if (yharonLore)
	            proj.damage = (int)(proj.damage * 0.75);

            #endregion

            if (tarraMage && modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit && proj.CountsAsClass(DamageClass.Magic))
            {
                tarraCrits++;
            }
            if (tarraThrowing && !tarragonImmunity && !tarragonImmunityCooldown && tarraThrowingCrits < 25 && modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit && proj.Calamity().rogue)
            {
                tarraThrowingCrits++;
            }

            if ((target.damage > 5 || target.boss) && Player.whoAmI == Main.myPlayer && !target.SpawnedFromStatue)
            {
                if (theBee && Player.statLife >= Player.statLifeMax2)
                {
                    SoundEngine.PlaySound(SoundID.Item110, proj.Center);
                }
                if (unstablePrism && modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit)
                {
                    for (int s = 0; s < 3; s++)
                    {
						Vector2 velocity = CalamityUtils.RandomVelocity(50f, 30f, 60f);
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center, velocity, ModContent.ProjectileType<UnstableSpark>(), CalamityUtils.DamageSoftCap(proj.damage * 0.15, 30), 0f, Player.whoAmI);
                    }
                }
                if (electricianGlove && proj.Calamity().stealthStrike && proj.Calamity().rogue && proj.Calamity().stealthStrikeHitCount < 5)
                {
                    for (int s = 0; s < 3; s++)
                    {
						Vector2 velocity = CalamityUtils.RandomVelocity(50f, 30f, 60f);
                        int spark = Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center, velocity, ModContent.ProjectileType<Spark>(), CalamityUtils.DamageSoftCap(proj.damage * 0.1, 30), 0f, Player.whoAmI);
                        Main.projectile[spark].Calamity().forceRogue = true;
                        Main.projectile[spark].localNPCHitCooldown = -1;
                    }
                }
                if (astralStarRain && modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit && astralStarRainCooldown <= 0)
                {
                    astralStarRainCooldown = 60;
                    for (int n = 0; n < 3; n++)
                    {
						int projectileType = Utils.SelectRandom(Main.rand, new int[]
						{
							ModContent.ProjectileType<AstralStar>(),
							ProjectileID.HallowStar,
							ModContent.ProjectileType<FallenStarProj>()
						});
						CalamityUtils.ProjectileRain(target.GetSource_FromThis(), target.Center, 400f, 100f, 500f, 800f, 25f, projectileType, (int)(120 * Player.AverageDamage()), 5f, Player.whoAmI, 6);
                    }
                }
                if (tarraRanged && modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit && proj.CountsAsClass(DamageClass.Ranged))
                {
                    int leafAmt = Main.rand.Next(2, 4);
                    for (int l = 0; l < leafAmt; l++)
                    {
						Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                        int FUCKYOU = Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center, velocity, ProjectileID.Leaf, CalamityUtils.DamageSoftCap(proj.damage * 0.25, 60), 0f, Player.whoAmI);
                        Main.projectile[FUCKYOU].Calamity().forceTypeless = true;
                        Main.projectile[FUCKYOU].netUpdate = true;
                    }
                }
                if (bloodflareThrowing && proj.Calamity().rogue && modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit && Main.rand.NextBool(2))
                {
                    if (target.canGhostHeal)
                    {
                        float projHitMult = 0.03f;
                        projHitMult -= (float)proj.numHits * 0.015f;
                        if (projHitMult < 0f)
                        {
                            projHitMult = 0f;
                        }
                        float cooldownMult = proj.damage * projHitMult;
                        if (cooldownMult < 0f)
                        {
                            cooldownMult = 0f;
                        }
                        if (Player.lifeSteal > 0f)
                        {
                            Player.statLife += 1;
                            Player.HealEffect(1);
                            Player.lifeSteal -= cooldownMult * 2f;
                        }
                    }
                }
                if (bloodflareMage && bloodflareMageCooldown <= 0 && modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit && proj.CountsAsClass(DamageClass.Magic))
                {
                    bloodflareMageCooldown = 120;
                    for (int i = 0; i < 3; i++)
                    {
						Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                        int fire = Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center, velocity, ProjectileID.BallofFire, CalamityUtils.DamageSoftCap(proj.damage * 0.5, 120), 0f, Player.whoAmI);
                        Main.projectile[fire].Calamity().forceTypeless = true;
                        Main.projectile[fire].netUpdate = true;
                    }
                }
                if (umbraphileSet && proj.Calamity().rogue && (Main.rand.NextBool(4) || (proj.Calamity().stealthStrike && proj.Calamity().stealthStrikeHitCount < 5)) && proj.type != ModContent.ProjectileType<UmbraphileBoom>())
                {
                    Projectile.NewProjectile(proj.GetSource_FromThis(), proj.Center, Vector2.Zero, ModContent.ProjectileType<UmbraphileBoom>(), CalamityUtils.DamageSoftCap(proj.damage * 0.25, 50), 0f, Player.whoAmI);
                }
                if (bloodflareMelee && isTrueMelee)
                {
                    if (bloodflareMeleeHits < 15 && !bloodflareFrenzy && !bloodFrenzyCooldown)
                    {
                        bloodflareMeleeHits++;
                    }
                    if (Player.whoAmI == Main.myPlayer && target.canGhostHeal)
                    {
                        int healAmount = Main.rand.Next(3) + 1;
                        Player.statLife += healAmount;
                        Player.HealEffect(healAmount);
                    }
                }
                if (proj.type == ModContent.ProjectileType<PolarStar>())
                {
                    polarisBoostCounter += 1;
                }
                if (CalamityConfig.Instance.Proficiency)
                {
                    if (gainLevelCooldown <= 0) //max is 12501 to avoid setting off fireworks forever
                    {
                        gainLevelCooldown = 120; //2 seconds
                        if (proj.CountsAsClass(DamageClass.Melee) && meleeLevel <= 12500)
                        {
							if (!ReduceCooldown((int)ClassType.Melee))
							{
								if (!Main.hardMode && meleeLevel >= 1500)
									gainLevelCooldown = 1200; //20 seconds
								if (!NPC.downedMoonlord && meleeLevel >= 5500)
									gainLevelCooldown = 2400; //40 seconds
							}
							else
								gainLevelCooldown /= 2;

							if (fasterMeleeLevel)
								gainLevelCooldown /= 2;

							meleeLevel++;
                            shootFireworksLevelUpMelee = true;

                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                LevelPacket(false, (int)ClassType.Melee);
                        }
                        else if (proj.CountsAsClass(DamageClass.Ranged) && rangedLevel <= 12500)
                        {
							if (!ReduceCooldown((int)ClassType.Ranged))
							{
								if (!Main.hardMode && rangedLevel >= 1500)
									gainLevelCooldown = 1200; //20 seconds
								if (!NPC.downedMoonlord && rangedLevel >= 5500)
									gainLevelCooldown = 2400; //40 seconds
							}
							else
								gainLevelCooldown /= 2;

							if (fasterRangedLevel)
								gainLevelCooldown /= 2;

							rangedLevel++;
                            shootFireworksLevelUpRanged = true;

                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                LevelPacket(false, (int)ClassType.Ranged);
                        }
                        else if (proj.CountsAsClass(DamageClass.Magic) && magicLevel <= 12500)
                        {
							if (!ReduceCooldown((int)ClassType.Magic))
							{
								if (!Main.hardMode && magicLevel >= 1500)
									gainLevelCooldown = 1200; //20 seconds
								if (!NPC.downedMoonlord && magicLevel >= 5500)
									gainLevelCooldown = 2400; //40 seconds
							}
							else
								gainLevelCooldown /= 2;

							if (fasterMagicLevel)
								gainLevelCooldown /= 2;

							magicLevel++;
                            shootFireworksLevelUpMagic = true;

                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                LevelPacket(false, (int)ClassType.Magic);
                        }
                        else if (isSummon && summonLevel <= 12500)
                        {
							if (!ReduceCooldown((int)ClassType.Summon))
							{
								if (!Main.hardMode && summonLevel >= 1500)
									gainLevelCooldown = 1200; //20 seconds
								if (!NPC.downedMoonlord && summonLevel >= 5500)
									gainLevelCooldown = 2400; //40 seconds
							}
							else
								gainLevelCooldown /= 2;

							if (fasterSummonLevel)
								gainLevelCooldown /= 2;

							summonLevel++;
                            shootFireworksLevelUpSummon = true;

                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                LevelPacket(false, (int)ClassType.Summon);
                        }
                        else if (proj.Calamity().rogue && rogueLevel <= 12500)
                        {
							if (!ReduceCooldown((int)ClassType.Rogue))
							{
								if (!Main.hardMode && rogueLevel >= 1500)
									gainLevelCooldown = 1200; //20 seconds
								if (!NPC.downedMoonlord && rogueLevel >= 5500)
									gainLevelCooldown = 2400; //40 seconds
							}
							else
								gainLevelCooldown /= 2;

							if (fasterRogueLevel)
								gainLevelCooldown /= 2;

							rogueLevel++;
                            shootFireworksLevelUpRogue = true;

                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                LevelPacket(false, (int)ClassType.Rogue);
                        }
                    }
                }
                if (raiderTalisman && raiderStack < 150 && proj.Calamity().rogue && modifiers.ToHitInfo(modifiers.FinalDamage.Base, true, modifiers.Knockback.Base, false, 0f).Crit && raiderCooldown <= 0)
                {
                    raiderStack++;
                    raiderCooldown = 30;
                }
                if (CalamityWorld.revenge && CalamityConfig.Instance.Rippers)
                {
                    if (isTrueMelee)
                    {
                        int stressGain = (int)(proj.damage * 0.1);
                        int stressMaxGain = 10;
                        if (stressGain < 1)
                        {
                            stressGain = 1;
                        }
                        if (stressGain > stressMaxGain)
                        {
                            stressGain = stressMaxGain;
                        }
                        rage += stressGain;
                        if (rage >= rageMax)
                        {
                            rage = rageMax;
                        }
                    }
                }
            }
        }
		#endregion

		#region Modify Hit By NPC
		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
		{
			int bossRushDamage = (Main.expertMode ? 400 : 240) + (BossRushEvent.BossRushStage * 2);
			if (BossRushEvent.BossRushActive)
			{
				if (npc.damage < bossRushDamage)
					npc.damage = bossRushDamage;
			}

			if (areThereAnyDamnBosses && CalRD.bossVelocityDamageScaleValues.ContainsKey(npc.type))
			{
				CalRD.bossVelocityDamageScaleValues.TryGetValue(npc.type, out float velocityScalar);

				if (((npc.type == NPCID.EyeofCthulhu || npc.type == NPCID.Spazmatism) && npc.ai[0] >= 2f) || (npc.type == NPCID.Plantera && npc.life / (float)npc.lifeMax <= 0.5f))
					velocityScalar = CalRD.bitingEnemeyVelocityScale;

				if (npc.velocity == Vector2.Zero)
				{
					contactDamageReduction += 1f - velocityScalar;
				}
				else
				{
					float amount = npc.velocity.Length() / (npc.Calamity().maxVelocity * 0.5f);
					if (amount > 1f)
						amount = 1f;

					float damageReduction = MathHelper.Lerp(velocityScalar, 1.1f, amount);
					if (damageReduction < 1f)
						contactDamageReduction += 1f - damageReduction;
					else
						npc.damage = (int)(npc.damage * damageReduction);
				}
			}

            if (triumph)
                contactDamageReduction += 0.15 * (1D - (npc.life / (double)npc.lifeMax));

            if (aSparkRare)
            {
                if (npc.type == NPCID.BlueJellyfish || npc.type == NPCID.PinkJellyfish || npc.type == NPCID.GreenJellyfish ||
                    npc.type == NPCID.FungoFish || npc.type == NPCID.BloodJelly || npc.type == NPCID.AngryNimbus || npc.type == NPCID.GigaZapper ||
                    npc.type == NPCID.MartianTurret || npc.type == ModContent.NPCType<StormlionCharger>() || npc.type == ModContent.NPCType<GhostBell>() || npc.type == ModContent.NPCType<BoxJellyfish>())
                    contactDamageReduction += 0.5;
            }

            if (fleshTotem && !fleshTotemCooldown)
            {
                Player.AddBuff(ModContent.BuffType<FleshTotemCooldown>(), 1200, false); //20 seconds
				contactDamageReduction += 0.5;
			}

            if (tarragonCloak && !tarragonCloakCooldown && tarraMelee)
				contactDamageReduction += 0.5;

            if (bloodflareMelee && bloodflareFrenzy && !bloodFrenzyCooldown)
				contactDamageReduction += 0.5;

            if (silvaMelee && silvaCountdown <= 0 && hasSilvaEffect)
				contactDamageReduction += 0.2;

			if (npc.Calamity().tSad > 0)
				contactDamageReduction += 0.5;

			if (npc.Calamity().relicOfResilienceWeakness > 0)
			{
				contactDamageReduction += RelicOfResilience.WeaknessDR;
				npc.Calamity().relicOfResilienceWeakness = 0;
			}

			if (beeResist)
			{
				if (CalamityLists.beeEnemyList.Contains(npc.type))
					contactDamageReduction += 0.25;
			}

			if (eskimoSet)
			{
				if (npc.coldDamage)
					contactDamageReduction += 0.1;
			}

			if (trinketOfChiBuff)
				contactDamageReduction += 0.15;

			// Fearmonger set provides 15% multiplicative DR that ignores caps during the Holiday Moons.
			// To prevent abuse, this effect does not work if there are any bosses alive.
			if (fearmongerSet && !areThereAnyDamnBosses && (Main.pumpkinMoon || Main.snowMoon))
				contactDamageReduction += 0.15;

			if (abyssalDivingSuitPlates)
				contactDamageReduction += 0.15;

			if (sirenIce)
				contactDamageReduction += 0.2;

			if (encased)
				contactDamageReduction += 0.3;

			if (Player.ownedProjectileCounts[ModContent.ProjectileType<EnergyShell>()] > 0 && Player.ActiveItem().type == ModContent.ItemType<LionHeart>())
				contactDamageReduction += 0.5;

			if (theBee && Player.statLife >= Player.statLifeMax2 && theBeeCooldown <= 0)
			{
				contactDamageReduction += 0.5;
				theBeeCooldown = 600;
			}

			if (CalamityWorld.revenge)
			{
				if (!CalamityWorld.downedBossAny)
					contactDamageReduction += 0.2;

				if (CalamityConfig.Instance.Rippers)
				{
					if (adrenaline == adrenalineMax && !adrenalineModeActive)
					{
						double adrenalineDRBoost = 0D +
							(adrenalineBoostOne ? 0.05 : 0D) +
							(adrenalineBoostTwo ? 0.05 : 0D) +
							(adrenalineBoostThree ? 0.05 : 0D);
						contactDamageReduction += 0.5 + adrenalineDRBoost;
					}
				}
			}

			if (Player.mount.Active && (Player.mount.Type == ModContent.MountType<AngryDogMount>() || Player.mount.Type == ModContent.MountType<OnyxExcavator>()) && Math.Abs(Player.velocity.X) > Player.mount.RunSpeed / 2f)
				contactDamageReduction += 0.1;

			if (leviathanAndSirenLore)
			{
				if (!Player.IsUnderwater())
					contactDamageReduction -= 0.05;
			}

			if (vHex)
				contactDamageReduction -= 0.3;

			if (irradiated)
				contactDamageReduction -= 0.1;

			if (corrEffigy)
				contactDamageReduction -= 0.2;

			if (calamityRing && !voidOfExtinction)
				contactDamageReduction -= 0.15;

			// 10% is converted to 9%, 25% is converted to 20%, 50% is converted to 33%, 75% is converted to 43%, 100% is converted to 50%
			if (contactDamageReduction > 0D)
			{
				if (aCrunch)
					contactDamageReduction *= 0.33;

				if (wCleave)
					contactDamageReduction *= 0.75;

				// Scale with base damage reduction
				if (DRStat > 0)
					contactDamageReduction *= 1f - (DRStat * 0.01f);

				contactDamageReduction = 1D / (1D + contactDamageReduction);
				npc.damage = (int)(npc.damage * contactDamageReduction);
			}

			if (Main.hardMode && Main.expertMode)
			{
				bool reduceChaosBallDamage = npc.type == NPCID.ChaosBall && !NPC.AnyNPCs(NPCID.GoblinSummoner);

				if (reduceChaosBallDamage || npc.type == NPCID.BurningSphere || npc.type == NPCID.WaterSphere)
					npc.damage = (int)(npc.damage * 0.6);
			}

			if (CalamityWorld.ironHeart)
			{
				int damageMin = 40 + (Player.statLifeMax2 / 10);
				if (npc.damage < damageMin)
				{
					Player.endurance = 0f;
					npc.damage = damageMin;
				}
			}

			if (aBulwarkRare)
			{
				aBulwarkRareMeleeBoostTimer += 3 * npc.damage;
				if (aBulwarkRareMeleeBoostTimer > 900)
					aBulwarkRareMeleeBoostTimer = 900;
			}

			if (Player.whoAmI == Main.myPlayer && gainRageCooldown <= 0)
            {
                if (CalamityWorld.revenge && CalamityConfig.Instance.Rippers && !npc.SpawnedFromStatue)
                {
                    gainRageCooldown = 60;
                    int stressGain = npc.damage * (profanedRage ? 3 : 2);
                    int stressMaxGain = 2500;
                    if (stressGain < 1)
                    {
                        stressGain = 1;
                    }
                    if (stressGain > stressMaxGain)
                    {
                        stressGain = stressMaxGain;
                    }
                    rage += stressGain;
                    if (rage >= rageMax)
                    {
                        rage = rageMax;
                    }
                }
            }
		}
        #endregion

        #region Modify Hit By Proj
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
			if (CalamityLists.projectileDestroyExceptionList.TrueForAll(x => proj.type != x))
			{
				if (Player.ActiveItem().type == ModContent.ItemType<GaelsGreatsword>()
					&& proj.active && proj.hostile && Player.altFunctionUse == 2 && Main.rand.NextBool(2))
				{
					for (int j = 0; j < 3; j++)
					{
						int dustIndex = Dust.NewDust(proj.position, proj.width, proj.height, 31, 0f, 0f, 0, default, 1f);
						Main.dust[dustIndex].velocity *= 0.3f;
					}
					int damage2 = (int)(GaelsGreatsword.BaseDamage * Player.MeleeDamage());
					proj.hostile = false;
					proj.friendly = true;
					proj.velocity *= -1f;
					proj.damage = damage2;
					proj.penetrate = 1;
					Player.immune = true;
					Player.immuneNoBlink = true;
					Player.immuneTime = 4;
					proj.damage = 0;
					return;
				}
			}

			if (auralisAuroraCounter >= 300)
			{
				proj.damage -= 100;
				if (proj.damage < 1)
					proj.damage = 1;
				auralisAuroraCounter = 0;
				auralisAuroraCooldown = CalamityUtils.SecondsToFrames(30f);
			}

			if (proj.type == ModContent.ProjectileType<BirbAura>())
			{
				proj.damage = 0;
				return;
			}

			// Reduce damage from vanilla traps
			// 350 in normal, 450 in expert
			if (proj.type == ProjectileID.Explosives)
				proj.damage = (int)(proj.damage * (Main.expertMode ? 0.225 : 0.35));
			if (Main.expertMode)
			{
				// 140 in normal, 182 in expert
				if (proj.type == ProjectileID.Boulder)
					proj.damage = (int)(proj.damage * 0.65);
			}

			// Reduce the bullshit damage Ichor Stickers do
			if (proj.type == ProjectileID.GoldenShowerHostile)
				proj.damage = (int)(proj.damage * 0.35);

			if (CalamityWorld.revenge)
			{
				double damageMultiplier = 1D;
				bool containsProjectile = false;
				if (CalamityLists.revengeanceProjectileBuffList25Percent.Contains(proj.type))
				{
					damageMultiplier += 0.25;
					containsProjectile = true;
				}
				else if (CalamityLists.revengeanceProjectileBuffList20Percent.Contains(proj.type))
				{
					damageMultiplier += 0.2;
					containsProjectile = true;
				}
				else if (CalamityLists.revengeanceProjectileBuffList15Percent.Contains(proj.type))
				{
					damageMultiplier += 0.15;
					containsProjectile = true;
				}

				if (containsProjectile)
				{
					if (CalamityWorld.death)
						damageMultiplier += (damageMultiplier - 1D) * 0.6;

					proj.damage = (int)(proj.damage * damageMultiplier);
				}
			}

			int bossRushDamage = (Main.expertMode ? 90 : 110) + (BossRushEvent.BossRushStage / 2);
			if (BossRushEvent.BossRushActive)
			{
				if (proj.damage < bossRushDamage)
					proj.damage = bossRushDamage;
			}

			// Reduce projectile damage based on banner type
			// IMPORTANT NOTE: Rework this in 1.4!
			Point point = Player.Center.ToTileCoordinates();
			int buffScanAreaWidth = (Main.maxScreenW + 800) / 16 - 1;
			int buffScanAreaHeight = (Main.maxScreenH + 800) / 16 - 1;
			Rectangle rectangle = CalamityUtils.ClampToWorld(tileRectangle: new Rectangle(point.X - buffScanAreaWidth / 2, point.Y - buffScanAreaHeight / 2, buffScanAreaWidth, buffScanAreaHeight));
			bool[] NPCBannerBuff = new bool[Main.MaxBannerTypes];
			bool hasBanner = false;

			// Scan area around the player for banners
			for (int i = rectangle.Left; i < rectangle.Right; i++)
			{
				for (int j = rectangle.Top; j < rectangle.Bottom; j++)
				{
					if (!rectangle.Contains(i, j))
						continue;

					Tile tile = Main.tile[i, j];
					if (!tile.HasTile)
						continue;

					if (tile.TileType == TileID.Banners && (tile.TileFrameX >= 396 || tile.TileFrameY >= 54))
					{
						int bannerType = tile.TileFrameX / 18 - 21;
						for (int k = tile.TileFrameY; k >= 54; k -= 54)
						{
							bannerType += 90;
							bannerType += 21;
						}

						int bannerItemType = Item.BannerToItem(bannerType);
						if (ItemID.Sets.BannerStrength[bannerItemType].Enabled)
						{
							NPCBannerBuff[bannerType] = true;
							hasBanner = true;
						}
					}
				}
			}

			// Reduce damage
			if (hasBanner)
				BannerProjectileDamageReduction(proj, ref proj.damage, NPCBannerBuff);

            if (projRefRare)
            {
                if (proj.type == projTypeJustHitBy)
                    projectileDamageReduction += 0.15;
            }

            if (aSparkRare)
            {
                if (proj.type == ProjectileID.MartianTurretBolt || proj.type == ProjectileID.GigaZapperSpear || proj.type == ProjectileID.CultistBossLightningOrbArc || proj.type == ModContent.ProjectileType<LightningMark>() || proj.type == ProjectileID.VortexLightning ||
                    proj.type == ProjectileID.BulletSnowman || proj.type == ProjectileID.BulletDeadeye || proj.type == ProjectileID.SniperBullet || proj.type == ProjectileID.VortexLaser)
                    projectileDamageReduction += 0.5;
            }
            

            if (beeResist)
            {
                if (CalamityLists.beeProjectileList.Contains(proj.type))
                    projectileDamageReduction += 0.25;
            }

            if (Main.hardMode && Main.expertMode && !CalamityWorld.spawnedHardBoss && proj.active && !proj.friendly && proj.hostile && proj.damage > 0)
            {
                if (CalamityLists.hardModeNerfList.Contains(proj.type))
                    projectileDamageReduction += 0.25;
            }

			if (trinketOfChiBuff)
				projectileDamageReduction += 0.15;

			// Fearmonger set provides 15% multiplicative DR that ignores caps during the Holiday Moons.
			// To prevent abuse, this effect does not work if there are any bosses alive.
			if (fearmongerSet && !areThereAnyDamnBosses && (Main.pumpkinMoon || Main.snowMoon))
				projectileDamageReduction += 0.15;

			if (abyssalDivingSuitPlates)
				projectileDamageReduction += 0.15;

			if (sirenIce)
				projectileDamageReduction += 0.2;

			if (encased)
				projectileDamageReduction += 0.3;

			if (Player.ownedProjectileCounts[ModContent.ProjectileType<EnergyShell>()] > 0 && Player.ActiveItem().type == ModContent.ItemType<LionHeart>())
				projectileDamageReduction += 0.5;

			if (theBee && Player.statLife >= Player.statLifeMax2 && theBeeCooldown <= 0)
			{
				projectileDamageReduction += 0.5;
				theBeeCooldown = 600;
			}

			if (CalamityWorld.revenge)
			{
				if (!CalamityWorld.downedBossAny)
					projectileDamageReduction += 0.2;

				if (CalamityConfig.Instance.Rippers)
				{
					if (adrenaline == adrenalineMax && !adrenalineModeActive)
					{
						double adrenalineDRBoost = 0D +
							(adrenalineBoostOne ? 0.05 : 0D) +
							(adrenalineBoostTwo ? 0.05 : 0D) +
							(adrenalineBoostThree ? 0.05 : 0D);
						projectileDamageReduction += 0.5 + adrenalineDRBoost;
					}
				}
			}

			if (Player.mount.Active && (Player.mount.Type == ModContent.MountType<AngryDogMount>() || Player.mount.Type == ModContent.MountType<OnyxExcavator>()) && Math.Abs(Player.velocity.X) > Player.mount.RunSpeed / 2f)
				projectileDamageReduction += 0.1;

			if (leviathanAndSirenLore)
			{
				if (!Player.IsUnderwater())
					projectileDamageReduction -= 0.05;
			}

			if (vHex)
				projectileDamageReduction -= 0.3;

			if (irradiated)
				projectileDamageReduction -= 0.1;

			if (corrEffigy)
				projectileDamageReduction -= 0.2;

			if (calamityRing && !voidOfExtinction)
				projectileDamageReduction -= 0.15;

			// 10% is converted to 9%, 25% is converted to 20%, 50% is converted to 33%, 75% is converted to 43%, 100% is converted to 50%
			if (projectileDamageReduction > 0D)
			{
				if (aCrunch)
					projectileDamageReduction *= 0.33;

				if (wCleave)
					projectileDamageReduction *= 0.75;

				// Scale with base damage reduction
				if (DRStat > 0)
					projectileDamageReduction *= 1f - (DRStat * 0.01f);

				projectileDamageReduction = 1D / (1D + projectileDamageReduction);
				proj.damage = (int)(proj.damage * projectileDamageReduction);
			}

			if (CalamityWorld.ironHeart)
			{
				int damageMin = (Main.expertMode ? 10 : 20) + (Player.statLifeMax2 / (Main.expertMode ? 40 : 20));
				if (proj.damage < damageMin)
				{
					Player.endurance = 0f;
					proj.damage = damageMin;
				}
			}

			if (Player.whoAmI == Main.myPlayer && gainRageCooldown <= 0)
            {
                if (CalamityWorld.revenge && CalamityConfig.Instance.Rippers && !CalamityLists.trapProjectileList.Contains(proj.type))
                {
                    gainRageCooldown = 60;
                    int stressGain = proj.damage * (profanedRage ? 3 : 2);
                    int stressMaxGain = 2500;
                    if (stressGain < 1)
                    {
                        stressGain = 1;
                    }
                    if (stressGain > stressMaxGain)
                    {
                        stressGain = stressMaxGain;
                    }
                    rage += stressGain;
                    if (rage >= rageMax)
                    {
                        rage = rageMax;
                    }
                }
            }
        }
		#endregion

		#region Banner Projectile Damage Reduction
		private void BannerProjectileDamageReduction(Projectile proj, ref int damage, bool[] NPCBannerBuffs)
		{
			bool? reduceDamage = null;
			double bannerDamageMultiplier = Main.expertMode ? 0.5 : 0.75;

			for (int l = 0; l < Main.MaxBannerTypes; l++)
			{
				int bannerNPCType = Item.BannerToNPC(l);
				if (bannerNPCType != 0 && NPCBannerBuffs[l])
				{
					if (proj.type == ModContent.ProjectileType<BelchingCoralSpike>())
					{
						if (bannerNPCType == ModContent.NPCType<BelchingCoral>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<CrabBoulder>())
					{
						if (bannerNPCType == ModContent.NPCType<AnthozoanCrab>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<EarthRockBig>() || proj.type == ModContent.ProjectileType<EarthRockSmall>())
					{
						if (bannerNPCType == ModContent.NPCType<Horse>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<FlakAcid>())
					{
						if (bannerNPCType == ModContent.NPCType<FlakCrab>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<FlameBurstHostile>())
					{
						if (bannerNPCType == ModContent.NPCType<ImpiousImmolator>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<GammaAcid>() || proj.type == ModContent.ProjectileType<GammaBeam>())
					{
						if (bannerNPCType == ModContent.NPCType<GammaSlime>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<HorsWaterBlast>())
					{
						if (bannerNPCType == ModContent.NPCType<Cnidrion>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<InkBombHostile>() || proj.type == ModContent.ProjectileType<InkPoisonCloud>() || proj.type == ModContent.ProjectileType<InkPoisonCloud2>() || proj.type == ModContent.ProjectileType<InkPoisonCloud3>())
					{
						if (bannerNPCType == ModContent.NPCType<ColossalSquid>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<MantisRing>())
					{
						if (bannerNPCType == ModContent.NPCType<Mantis>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<NuclearToadGoo>())
					{
						if (bannerNPCType == ModContent.NPCType<NuclearToad>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<OrthoceraStream>())
					{
						if (bannerNPCType == ModContent.NPCType<Orthocera>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<PearlBurst>() || proj.type == ModContent.ProjectileType<PearlRain>())
					{
						if (bannerNPCType == ModContent.NPCType<GiantClam>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<PufferExplosion>())
					{
						if (bannerNPCType == ModContent.NPCType<ChaoticPuffer>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<StormMarkHostile>() || proj.type == ModContent.ProjectileType<TornadoHostile>())
					{
						if (bannerNPCType == ModContent.NPCType<ThiccWaifu>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<SulphuricAcidBubble>() || proj.type == ModContent.ProjectileType<SulphuricAcidMist>())
					{
						if (bannerNPCType == ModContent.NPCType<Mauler>() || (bannerNPCType == ModContent.NPCType<Flounder>() && proj.type == ModContent.ProjectileType<SulphuricAcidMist>()))
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<ToxicMinnowCloud>())
					{
						if (bannerNPCType == ModContent.NPCType<ToxicMinnow>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<TrilobiteSpike>())
					{
						if (bannerNPCType == ModContent.NPCType<Trilobite>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<BrimstoneLaser>() || proj.type == ModContent.ProjectileType<BrimstoneLaserSplit>())
					{
						if (bannerNPCType == ModContent.NPCType<SoulSlurper>())
							reduceDamage = !NPC.AnyNPCs(ModContent.NPCType<Calamitas>()) && !NPC.AnyNPCs(ModContent.NPCType<CalamitasRun3>());
					}
					else if (proj.type == ModContent.ProjectileType<GreatSandBlast>())
					{
						if (bannerNPCType == ModContent.NPCType<GreatSandShark>())
							reduceDamage = true;
					}
					else if (proj.type == ModContent.ProjectileType<PhantomGhostShot>())
					{
						if (bannerNPCType == ModContent.NPCType<PhantomSpiritL>())
							reduceDamage = !NPC.AnyNPCs(ModContent.NPCType<Polterghast>());
					}
					else if (proj.type == ModContent.ProjectileType<PlagueStingerGoliathV2>())
					{
						if (bannerNPCType == ModContent.NPCType<PlaguedJungleSlime>() || bannerNPCType == ModContent.NPCType<PlaguebringerShade>())
							reduceDamage = !NPC.AnyNPCs(ModContent.NPCType<PlaguebringerGoliath>());
					}
					else if (proj.type == ModContent.ProjectileType<HiveBombGoliath>())
					{
						if (bannerNPCType == ModContent.NPCType<PlaguebringerShade>())
							reduceDamage = !NPC.AnyNPCs(ModContent.NPCType<PlaguebringerGoliath>());
					}
					else if (proj.type == ModContent.ProjectileType<HolyBomb>() || proj.type == ModContent.ProjectileType<HolyFlare>())
					{
						if (bannerNPCType == ModContent.NPCType<ProfanedEnergyBody>())
							reduceDamage = !NPC.AnyNPCs(ModContent.NPCType<Providence>());
					}
					else if (proj.type == ProjectileID.EyeBeam)
					{
						if (bannerNPCType == ModContent.NPCType<Laserfish>())
							reduceDamage = !NPC.AnyNPCs(NPCID.Golem) && !NPC.AnyNPCs(ModContent.NPCType<RavagerBody>());
					}
					else if (proj.type == ProjectileID.CultistBossIceMist || proj.type == ProjectileID.CultistBossLightningOrbArc)
					{
						if (bannerNPCType == ModContent.NPCType<EidolonWyrmHead>() || bannerNPCType == ModContent.NPCType<Eidolist>())
							reduceDamage = !NPC.AnyNPCs(ModContent.NPCType<StormWeaverHead>()) && !NPC.AnyNPCs(ModContent.NPCType<StormWeaverHeadNaked>()) && !NPC.AnyNPCs(NPCID.CultistBoss) && proj.Calamity().lineColor != 1;
					}
					else if (proj.type == ProjectileID.SaucerScrap)
					{
						if (bannerNPCType == ModContent.NPCType<ArmoredDiggerHead>())
							reduceDamage = Main.invasionType != InvasionID.MartianMadness && (!NPC.AnyNPCs(NPCID.TheDestroyer) || !CalamityWorld.revenge);
					}
					else
					{
						switch (proj.type)
						{
							case ProjectileID.Stinger:
								if (CalamityLists.hornetList.Contains(bannerNPCType) || CalamityLists.mossHornetList.Contains(bannerNPCType))
								{
									reduceDamage = !NPC.AnyNPCs(NPCID.QueenBee);
								}
								break;

							case ProjectileID.PinkLaser:
								if (bannerNPCType == NPCID.Gastropod || bannerNPCType == ModContent.NPCType<AstralProbe>())
								{
									reduceDamage = !NPC.AnyNPCs(NPCID.TheDestroyer) && !NPC.AnyNPCs(ModContent.NPCType<LifeSeeker>()) && !NPC.AnyNPCs(ModContent.NPCType<StormWeaverHead>()) && !NPC.AnyNPCs(ModContent.NPCType<StormWeaverHeadNaked>());
								}
								break;

							case ProjectileID.SandBallFalling:
								if (bannerNPCType == NPCID.Antlion)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.DemonSickle:
								if (bannerNPCType == NPCID.Demon || bannerNPCType == NPCID.VoodooDemon)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.HarpyFeather:
								if (bannerNPCType == NPCID.Harpy)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.JavelinHostile:
								if (bannerNPCType == NPCID.GreekSkeleton)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.IceSpike:
								if (bannerNPCType == NPCID.SpikedIceSlime)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.JungleSpike:
								if (bannerNPCType == NPCID.SpikedJungleSlime)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.WebSpit:
								if (bannerNPCType == NPCID.BlackRecluse || bannerNPCType == NPCID.BlackRecluseWall)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.CursedFlameHostile:
								if (bannerNPCType == NPCID.Clinger)
								{
									reduceDamage = !NPC.AnyNPCs(NPCID.Spazmatism) && (!NPC.AnyNPCs(NPCID.EaterofWorldsHead) || !CalamityWorld.revenge);
								}
								break;

							case ProjectileID.DesertDjinnCurse:
								if (bannerNPCType == NPCID.DesertDjinn)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.InfernoHostileBlast:
							case ProjectileID.InfernoHostileBolt:
								if (bannerNPCType == NPCID.DiabolistRed || bannerNPCType == NPCID.DiabolistWhite)
								{
									reduceDamage = !NPC.AnyNPCs(NPCID.Golem) || !CalamityWorld.revenge;
								}
								break;

							case ProjectileID.Shadowflames:
								if (bannerNPCType == NPCID.GiantCursedSkull)
								{
									reduceDamage = !NPC.AnyNPCs(NPCID.SkeletronHead) || !CalamityWorld.revenge;
								}
								break;

							case ProjectileID.FrostBlastHostile:
								if (bannerNPCType == NPCID.IceElemental || bannerNPCType == ModContent.NPCType<IceClasper>())
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.IcewaterSpit:
								if (bannerNPCType == NPCID.IcyMerman)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.GoldenShowerHostile:
								if (bannerNPCType == NPCID.IchorSticker)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.ShadowBeamHostile:
								if (bannerNPCType == NPCID.Necromancer || bannerNPCType == NPCID.NecromancerArmored)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.PaladinsHammerHostile:
								if (bannerNPCType == NPCID.Paladin)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.LostSoulHostile:
								if (bannerNPCType == NPCID.RaggedCaster || bannerNPCType == NPCID.RaggedCasterOpenCoat)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.UnholyTridentHostile:
								if (bannerNPCType == NPCID.RedDevil)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.RuneBlast:
								if (bannerNPCType == NPCID.RuneWizard)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.WoodenArrowHostile:
								if (bannerNPCType == NPCID.GoblinArcher || bannerNPCType == NPCID.CultistArcherBlue || bannerNPCType == NPCID.CultistArcherWhite)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.FlamingArrow:
								if (bannerNPCType == NPCID.SkeletonArcher || bannerNPCType == NPCID.PirateCrossbower || bannerNPCType == NPCID.ElfArcher)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.SalamanderSpit:
								if (bannerNPCType >= NPCID.Salamander && bannerNPCType <= NPCID.Salamander9)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.SkeletonBone:
								if (bannerNPCType == NPCID.Skeleton)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.RocketSkeleton:
								if (bannerNPCType == NPCID.SkeletonCommando)
								{
									reduceDamage = !NPC.AnyNPCs(NPCID.SkeletronPrime) || !CalamityWorld.revenge;
								}
								break;

							case ProjectileID.SniperBullet:
								if (bannerNPCType == NPCID.SkeletonSniper)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.BulletDeadeye:
								if (bannerNPCType == NPCID.TacticalSkeleton || bannerNPCType == NPCID.SnowmanGangsta || bannerNPCType == NPCID.PirateCaptain || bannerNPCType == NPCID.PirateDeadeye || bannerNPCType == NPCID.ElfCopter)
								{
									reduceDamage = !NPC.AnyNPCs(NPCID.SantaNK1);
								}
								break;

							case ProjectileID.RainNimbus:
								if (bannerNPCType == NPCID.AngryNimbus)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.FrostShard:
								if (bannerNPCType == NPCID.AngryNimbus)
								{
									reduceDamage = !NPC.AnyNPCs(NPCID.IceQueen) && CalamityWorld.death;
								}
								break;

							case ProjectileID.FrostBeam:
								if (bannerNPCType == NPCID.IceGolem)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.SandnadoHostile:
							case ProjectileID.SandnadoHostileMark:
								if (bannerNPCType == NPCID.SandElemental)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.SnowBallHostile:
								if (bannerNPCType == NPCID.SnowBalla)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.CannonballHostile:
								if (bannerNPCType == NPCID.PirateCaptain)
								{
									reduceDamage = !NPC.AnyNPCs(NPCID.PirateShip);
								}
								break;

							case ProjectileID.DrManFlyFlask:
								if (bannerNPCType == NPCID.DrManFly)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.EyeLaser:
								if (bannerNPCType == NPCID.Eyezor)
								{
									reduceDamage = !NPC.AnyNPCs(NPCID.Retinazer) && !NPC.AnyNPCs(NPCID.WallofFleshEye);
								}
								break;

							case ProjectileID.Nail:
								if (bannerNPCType == NPCID.Nailhead)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.NebulaSphere:
								if (bannerNPCType == NPCID.NebulaBeast)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.NebulaLaser:
								if (bannerNPCType == NPCID.NebulaBrain)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.NebulaBolt:
								if (bannerNPCType == NPCID.NebulaSoldier)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.StardustJellyfishSmall:
								if (bannerNPCType == NPCID.StardustJellyfishBig)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.StardustSoldierLaser:
								if (bannerNPCType == NPCID.StardustSoldier)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.Twinkle:
								if (bannerNPCType == NPCID.StardustSpiderBig)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.VortexAcid:
								if (bannerNPCType == NPCID.VortexHornetQueen)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.VortexLaser:
								if (bannerNPCType == NPCID.VortexRifleman)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.VortexLightning:
								if (bannerNPCType == NPCID.VortexSoldier)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.RayGunnerLaser:
								if (bannerNPCType == NPCID.RayGunner)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.BrainScramblerBolt:
								if (bannerNPCType == NPCID.BrainScrambler)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.MartianWalkerLaser:
								if (bannerNPCType == NPCID.MartianWalker)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.MartianTurretBolt:
								if ((bannerNPCType == NPCID.MartianTurret && Main.invasionType == InvasionID.MartianMadness) || ((bannerNPCType == ModContent.NPCType<ShockstormShuttle>() || bannerNPCType == ModContent.NPCType<WulfrumDrone>()) && Main.invasionType != InvasionID.MartianMadness))
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.SaucerLaser:
								if (bannerNPCType == ModContent.NPCType<ShockstormShuttle>() && Main.invasionType != InvasionID.MartianMadness)
								{
									reduceDamage = true;
								}
								break;

							case ProjectileID.HappyBomb:
								if (bannerNPCType == NPCID.Clown)
								{
									reduceDamage = true;
								}
								break;
						}
					}

					if (reduceDamage.HasValue)
					{
						if (reduceDamage.Value)
							damage = (int)(damage * bannerDamageMultiplier);

						break;
					}
				}
			}
		}
		#endregion

		#region On Hit
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (sulfurSet)
                npc.AddBuff(BuffID.Poisoned, 120);
            if (CalamityWorld.revenge)
            {
                if (npc.type == NPCID.ShadowFlameApparition || (npc.type == NPCID.ChaosBall && (Main.hardMode || areThereAnyDamnBosses)))
                {
                    Player.AddBuff(ModContent.BuffType<Shadowflame>(), 180);
                }
                else if (npc.type == NPCID.Spazmatism && npc.ai[0] != 1f && npc.ai[0] != 2f && npc.ai[0] != 0f)
                {
                    Player.AddBuff(BuffID.Bleeding, 300);
                }
                else if (npc.type == NPCID.Plantera && npc.life < npc.lifeMax / 2)
                {
                    Player.AddBuff(BuffID.Poisoned, 180);
					Player.AddBuff(BuffID.Venom, 180);
					Player.AddBuff(BuffID.Bleeding, 300);
                }
                else if (npc.type == NPCID.PlanterasTentacle)
                {
                    Player.AddBuff(BuffID.Poisoned, 120);
					Player.AddBuff(BuffID.Venom, 120);
					Player.AddBuff(BuffID.Bleeding, 180);
                }
				else if (npc.type == NPCID.AncientDoom)
				{
					Player.AddBuff(ModContent.BuffType<Shadowflame>(), 120);
				}
				else if (npc.type == NPCID.AncientLight)
				{
					Player.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
				}
			}
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (sulfurSet && !proj.friendly)
            {
                if (Main.player[proj.owner] is null)
                {
                    if (!Main.npc[proj.owner].friendly)
                        Main.npc[proj.owner].AddBuff(BuffID.Poisoned, 120);
                }
                else
                {
                    Player p = Main.player[proj.owner];
                    if (p.hostile && Player.hostile && (Player.team != p.team || p.team == 0))
                        p.AddBuff(BuffID.Poisoned, 120);
                }
            }

            if (CalamityWorld.revenge && proj.hostile)
            {
                if (proj.type == ProjectileID.Explosives)
                {
                    Player.AddBuff(BuffID.OnFire, 600);
                }
                else if (proj.type == ProjectileID.Boulder)
                {
                    Player.AddBuff(BuffID.BrokenArmor, 600);
                }
                else if (proj.type == ProjectileID.FrostBeam && !Player.frozen && !gState)
                {
                    Player.AddBuff(ModContent.BuffType<GlacialState>(), 120);
                }
                else if (proj.type == ProjectileID.DeathLaser || proj.type == ProjectileID.RocketSkeleton)
                {
                    Player.AddBuff(BuffID.OnFire, 240);
                }
                else if (proj.type == ProjectileID.Skull)
                {
                    Player.AddBuff(BuffID.Weak, 180);
                }
                else if (proj.type == ProjectileID.ThornBall)
                {
                    Player.AddBuff(BuffID.Poisoned, 240);
					Player.AddBuff(BuffID.Venom, 120);
				}
                else if (proj.type == ProjectileID.CultistBossIceMist)
                {
                    Player.AddBuff(BuffID.Frozen, 60);
                    Player.AddBuff(BuffID.Chilled, 120);
                    Player.AddBuff(BuffID.Frostburn, 240);
                }
                else if (proj.type == ProjectileID.CultistBossLightningOrbArc)
                {
					int deathModeDuration = NPC.downedMoonlord ? 80 : NPC.downedPlantBoss ? 40 : Main.hardMode ? 20 : 10;
					Player.AddBuff(BuffID.Electrified, proj.Calamity().lineColor == 1 ? deathModeDuration : 120);
                    // Scaled duration for DM lightning, 2 seconds for Storm Weaver/Cultist lightning
                }
				else if (proj.type == ProjectileID.AncientDoomProjectile)
				{
					Player.AddBuff(ModContent.BuffType<Shadowflame>(), 120);
				}
				else if (proj.type == ProjectileID.CultistBossFireBallClone)
				{
					Player.AddBuff(ModContent.BuffType<Shadowflame>(), 120);
				}
			}
			if (CalamityLists.projectileDestroyExceptionList.TrueForAll(x => proj.type != x))
			{
				if (projRef && proj.active && !proj.friendly && proj.hostile && proj.damage > 0 && Main.rand.NextBool(20))
				{
					Player.statLife += hurtInfo.Damage;
					Player.HealEffect(hurtInfo.Damage);
					proj.hostile = false;
					proj.friendly = true;
					proj.velocity.X = -proj.velocity.X;
					proj.velocity.Y = -proj.velocity.Y;
				}
				if (projRefRare && proj.active && !proj.friendly && proj.hostile && proj.damage > 0 && Main.rand.NextBool(2))
				{
					proj.hostile = false;
					proj.friendly = true;
					proj.velocity.X = -proj.velocity.X * 2f;
					proj.velocity.Y = -proj.velocity.Y * 2f;
					proj.damage *= 10;
					projRefRareLifeRegenCounter = 120;
					projTypeJustHitBy = proj.type;
				}
				if (aSparkRare && proj.active && !proj.friendly && proj.hostile && proj.damage > 0)
				{
					if (proj.type == ProjectileID.BulletSnowman || proj.type == ProjectileID.BulletDeadeye || proj.type == ProjectileID.SniperBullet || proj.type == ProjectileID.VortexLaser)
					{
						proj.hostile = false;
						proj.friendly = true;
						proj.velocity.X = -proj.velocity.X;
						proj.velocity.Y = -proj.velocity.Y;
						proj.damage *= 8;
					}
				}
				if (daedalusReflect && proj.active && !proj.friendly && proj.hostile && proj.damage > 0 && Main.rand.NextBool(3))
				{
					int healAmt = hurtInfo.Damage / 5;
					Player.statLife += healAmt;
					Player.HealEffect(healAmt);
					proj.hostile = false;
					proj.friendly = true;
					proj.velocity.X = -proj.velocity.X;
					proj.velocity.Y = -proj.velocity.Y;
				}
			}
        }
        #endregion

        #region Can Hit
        public override bool? CanHitNPCWithItem(Item item, NPC target)
        {
            if (camper && !Player.StandingStill())
            {
                return false;
            }
            return null;
        }

        public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
        {
            if (camper && !Player.StandingStill())
            {
                return false;
            }
            return null;
        }
        #endregion

        #region Fishing
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            CalamityPlayerFishing.CalamityCatchFish(Player, attempt, ref itemDrop, ref npcSpawn, ref sonar, ref sonarPosition);
        }

        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {
            CalamityPlayerFishing.CalamityGetFishingLevel(Player, ref fishingRod, ref bait, ref fishingLevel);
        }
        #endregion

        #region Shoot
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity1, int type, int damage, float knockback)
        {
            if (veneratedLocket)
            {
                if (item.Calamity().rogue && item.type != ModContent.ItemType<SylvanSlasher>())
                {
                    float num72 = item.shootSpeed;
                    Vector2 vector2 = Player.RotatedRelativePoint(Player.MountedCenter, true);
                    float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                    float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                    if (Player.gravDir == -1f)
                    {
                        num79 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector2.Y;
                    }
                    float num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
                    if ((float.IsNaN(num78) && float.IsNaN(num79)) || (num78 == 0f && num79 == 0f))
                    {
                        num78 = (float)Player.direction;
                        num79 = 0f;
                        num80 = num72;
                    }
                    else
                    {
                        num80 = num72 / num80;
                    }

                    vector2 = new Vector2(Player.position.X + (float)Player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)Player.direction) + ((float)Main.mouseX + Main.screenPosition.X - Player.position.X), Player.MountedCenter.Y - 600f);
                    vector2.X = (vector2.X + Player.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                    vector2.Y -= 100f;
                    num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                    num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                    if (num79 < 0f)
                    {
                        num79 *= -1f;
                    }
                    if (num79 < 20f)
                    {
                        num79 = 20f;
                    }
                    num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
                    num80 = num72 / num80;
                    num78 *= num80;
                    num79 *= num80;
                    float speedX4 = num78 + (float)Main.rand.Next(-30, 31) * 0.02f;
                    float speedY5 = num79 + (float)Main.rand.Next(-30, 31) * 0.02f;
                    int p = Projectile.NewProjectile(Entity.GetSource_FromThis(), vector2.X, vector2.Y, speedX4, speedY5, type, CalamityUtils.DamageSoftCap(damage * 0.15, 75), item.knockBack * 0.5f, Player.whoAmI);
                    Main.projectile[p].Calamity().forceRogue = true; //in case melee/rogue variants bug out
					if (item.type == ModContent.ItemType<FinalDawn>())
					{
						Main.projectile[p].ai[1] = 1f;
					}
                    if (StealthStrikeAvailable())
                    {
                        int knifeCount = 15;
                        int knifeDamage = (int)(150 * Player.RogueDamage());
                        float angleStep = MathHelper.TwoPi / knifeCount;
                        float speed = 15f;

                        for (int i = 0; i < knifeCount; i++)
                        {
                            Vector2 velocity = new Vector2(0f, speed);
                            velocity = velocity.RotatedBy(angleStep * i);
                            int knifeCol = Main.rand.Next(0, 2);

                            Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center, velocity, ModContent.ProjectileType<VeneratedKnife>(), knifeDamage, 0f, Player.whoAmI, knifeCol, 0);
                        }
                    }
                }
            }

            if (rustyMedal)
            {
                if (item.CountsAsClass(DamageClass.Ranged))
                {
                    if (Main.rand.NextBool(5))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 startingPosition = Main.MouseWorld - Vector2.UnitY.RotatedByRandom(0.4f) * 1250f;
                            Vector2 directionToMouse = (startingPosition - Main.MouseWorld).SafeNormalize(Vector2.UnitY).RotatedByRandom(0.1f);
                            Projectile.NewProjectileDirect(source, startingPosition, directionToMouse * 12f, ModContent.ProjectileType<ToxicannonDrop>(), CalamityUtils.DamageSoftCap(damage * 0.3, 30), 0f, Player.whoAmI).penetrate = 2;
                        }
                    }
                }
            }

            return true;
        }
        #endregion

		#region Frame Effects
		public override void FrameEffects()
		{
			if (Player.Calamity().andromedaState == AndromedaPlayerState.LargeRobot ||
				Player.Calamity().andromedaState == AndromedaPlayerState.SpecialAttack)
			{
				Player.head = EquipLoader.GetEquipSlot(Mod, "NoHead", EquipType.Head); // To make the head invisible on the map. The map was having a hissy fit because of hitbox changes.
			}
			else if ((profanedCrystal || profanedCrystalForce) && !profanedCrystalHide)
			{
				Player.legs = EquipLoader.GetEquipSlot(Mod, "ProfanedSoulCrystal", EquipType.Legs);
				Player.body = EquipLoader.GetEquipSlot(Mod, "ProfanedSoulCrystal", EquipType.Body);
				Player.head = EquipLoader.GetEquipSlot(Mod, "ProfanedSoulCrystal", EquipType.Head);
				Player.wings = EquipLoader.GetEquipSlot(Mod, "ProfanedSoulCrystal", EquipType.Wings);
				Player.face = -1;

				bool enrage = !profanedCrystalForce && profanedCrystalBuffs && Player.statLife <= (int)(Player.statLifeMax2 * 0.5);

				if (profanedCrystalWingCounter.Value == 0)
				{
					int key = profanedCrystalWingCounter.Key;
					profanedCrystalWingCounter = new KeyValuePair<int, int>(key == 3 ? 0 : key + 1, enrage ? 5 : 7);
				}

				Player.wingFrame = profanedCrystalWingCounter.Key;
				profanedCrystalWingCounter = new KeyValuePair<int, int>(profanedCrystalWingCounter.Key, profanedCrystalWingCounter.Value - 1);
				Player.armorEffectDrawOutlines = true;
				if (profanedCrystalBuffs)
				{
					Player.armorEffectDrawShadow = true;
					if (enrage)
					{
						Player.armorEffectDrawOutlinesForbidden = true;
					}
				}
			}
			else if ((snowmanPower || snowmanForce) && !snowmanHide)
			{
				Player.legs = EquipLoader.GetEquipSlot(Mod, "Popo", EquipType.Legs);
				Player.body = EquipLoader.GetEquipSlot(Mod, "Popo", EquipType.Body);
				Player.head = snowmanNoseless ? EquipLoader.GetEquipSlot(Mod, "PopoNoselessHead", EquipType.Head) : EquipLoader.GetEquipSlot(Mod, "Popo", EquipType.Head);
				Player.face = -1;
			}
			else if ((abyssalDivingSuitPower || abyssalDivingSuitForce) && !abyssalDivingSuitHide)
			{
				Player.legs = EquipLoader.GetEquipSlot(Mod, "AbyssalDivingSuit", EquipType.Legs);
				Player.body = EquipLoader.GetEquipSlot(Mod, "AbyssalDivingSuit", EquipType.Body);
				Player.head = EquipLoader.GetEquipSlot(Mod, "AbyssalDivingSuit", EquipType.Head);
				Player.face = -1;
			}
			else if ((sirenBoobsPower || sirenBoobsForce) && !sirenBoobsHide)
			{
				Player.legs = EquipLoader.GetEquipSlot(Mod, "SirensHeart", EquipType.Legs);
				Player.body = EquipLoader.GetEquipSlot(Mod, "SirensHeart", EquipType.Body);
				Player.head = EquipLoader.GetEquipSlot(Mod, "SirensHeart", EquipType.Head);
				Player.face = -1;
			}
            else if (meldTransformationPower || meldTransformationForce)
            {
                Player.legs = EquipLoader.GetEquipSlot(Mod, "MeldTransformation", EquipType.Legs);
                Player.body = EquipLoader.GetEquipSlot(Mod, "MeldTransformation", EquipType.Body);
                Player.neck = (sbyte)EquipLoader.GetEquipSlot(Mod, "MeldTransformation", EquipType.Neck);
                Player.head = EquipLoader.GetEquipSlot(Mod, "MeldTransformation", EquipType.Head);
                Player.face = -1;
            }
            else if ((omegaBlueTransformationPower || omegaBlueTransformationForce) && omegaBlueCooldown > 1500)
            {
                Player.head = EquipLoader.GetEquipSlot(Mod, "OmegaBlueTransformation", EquipType.Head);
            }
			else
			{
				if (profanedCrystalWingCounter.Key != 1)
					profanedCrystalWingCounter = new KeyValuePair<int, int>(1, 7);
				if (profanedCrystalAnimCounter.Key != 0)
					profanedCrystalAnimCounter = new KeyValuePair<int, int>(0, 10);
			}
			if (snowRuffianSet)
			{
				Player.wings = EquipLoader.GetEquipSlot(Mod, "SnowRuffWings", EquipType.Wings);
				bool falling = Player.gravDir == -1 ? Player.velocity.Y < 0.05f : Player.velocity.Y > 0.05f;
				if (Player.controlJump && falling)
				{
					Player.velocity.Y *= 0.9f;
					Player.wingFrame = 3;
					Player.noFallDmg = true;
					Player.fallStart = (int)(Player.position.Y / 16f);
				}
			}
			if (abyssDivingGear && (Player.head == -1 || Player.head == ArmorIDs.Head.FamiliarWig))
			{
				Player.head = EquipLoader.GetEquipSlot(Mod, "AbyssDivingGear", EquipType.Head);
				Player.face = -1;
			}

			if (CalamityWorld.defiled)
				Defiled();

			if (weakPetrification)
				WeakPetrification();
		}
		#endregion

        #region Limitations
        private void WeakPetrification()
        {
            Player.GetJumpState(ExtraJump.CloudInABottle).Disable();
            Player.GetJumpState(ExtraJump.SandstormInABottle).Disable();
            Player.GetJumpState(ExtraJump.BlizzardInABottle).Disable();
            Player.GetJumpState(ExtraJump.TsunamiInABottle).Disable();
            Player.GetJumpState(ExtraJump.FartInAJar).Disable();
			statigelJump = false;
			sulfurJump = false;
            Player.rocketBoots = 0;
            Player.jumpBoost = false;
            Player.slowFall = false;
            Player.gravControl = false;
            Player.gravControl2 = false;
            Player.jumpSpeedBoost = 0f;
            Player.wingTimeMax = (int)(Player.wingTimeMax * 0.5);
            Player.balloon = -1;
            weakPetrification = true;
        }

        private void Defiled()
        {
            Player.wingTimeMax = 0;
        }
        #endregion

        #region Disable All Dashes
        private const int DashDisableCooldown = 12;
		private void DisableAllDashes()
		{
			// Set the player to have no registered dashes.
			Player.dash = 0;
			dashMod = 0;

			// Put the player in a permanent state of dash cooldown. This is removed 1/5 of a second after disabling the effect.
			// This is necessary so that arbitrary dashes from other mods are also blocked by Calamity.
            if (Player.dashDelay >= 0 && Player.dashDelay < DashDisableCooldown)
			    Player.dashDelay = DashDisableCooldown;

			// Prevent the possibility of Shield of Cthulhu invulnerability exploits.
			Player.eocHit = -1;
			if (Player.eocDash != 0)
			{
				Player.eocDash = 0;
			}
		}
		#endregion

        #region Pre Hurt
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (CalamityWorld.armageddon || SCalLore || (BossRushEvent.BossRushActive && bossRushImmunityFrameCurseTimer > 0))
            {
                if (areThereAnyDamnBosses || SCalLore || (BossRushEvent.BossRushActive && bossRushImmunityFrameCurseTimer > 0))
                {
                    if (SCalLore)
                    {
                        string key = "Go to hell.";
                        Color messageColor = Color.Orange;
                        CalamityUtils.DisplayLocalizedText(key, messageColor);
                    }
                    if (CalamityWorld.DoGSecondStageCountdown > 0)
                    {
                        CalamityWorld.DoGSecondStageCountdown = 0;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            var netMessage = Mod.GetPacket();
                            netMessage.Write((byte)CalRDMessageType.DoGCountdownSync);
                            netMessage.Write(CalamityWorld.DoGSecondStageCountdown);
                            netMessage.Send();
                        }
                    }
                    KillPlayer();
                }
            }

            if (lol || (invincible && Player.ActiveItem().type != ModContent.ItemType<ColdheartIcicle>()))
            {
	            modifiers.FinalDamage *= 0f;
            }
            if (godSlayerReflect && Main.rand.NextBool(50))
            {
	            modifiers.FinalDamage *= 0f;
            }
            if (hurtSoundTimer == 0) //hurtsounds
            {
                if ((profanedCrystal || profanedCrystalForce) && !profanedCrystalHide)
                {
	                modifiers.DisableSound();
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/NPCHit/ProvidenceHurt"), Player.position);
                    hurtSoundTimer = 20;
                }
                else if ((abyssalDivingSuitPower || abyssalDivingSuitForce) && !abyssalDivingSuitHide)
                {
	                modifiers.DisableSound();
                    SoundEngine.PlaySound(SoundID.NPCHit4, Player.position); //metal hit noise
                    hurtSoundTimer = 10;
                }
                else if ((sirenBoobsPower || sirenBoobsForce) && !sirenBoobsHide)
                {
	                modifiers.DisableSound();
                    SoundEngine.PlaySound(SoundID.FemaleHit, Player.position); //female hit noise
                    hurtSoundTimer = 10;
                }
				else if (titanHeartSet)
				{
					modifiers.DisableSound();
					SoundStyle atlasHurt = Utils.SelectRandom(Main.rand, new SoundStyle[]
					{
						new SoundStyle("CalRD/Sounds/NPCHit/AtlasHurt0"),
						new SoundStyle("CalRD/Sounds/NPCHit/AtlasHurt1"),
						new SoundStyle("CalRD/Sounds/NPCHit/AtlasHurt2")
					});
					SoundEngine.PlaySound(atlasHurt, Player.position);
					hurtSoundTimer = 10;
				}
            }

            #region MultiplierBoosts
            double damageMult = 1D +
                (dArtifact ? 0.15 : 0D) +
                (DoGLore ? 0.05 : 0D) +
                ((Player.beetleDefense && Player.beetleOrbs > 0) ? (0.05 * Player.beetleOrbs) : 0D) +
                (enraged ? 0.25 : 0D) +
                ((CalamityWorld.defiled && Main.rand.NextBool(4)) ? 0.5 : 0D);

			if (bloodPact && Main.rand.NextBool(4))
			{
				Player.AddBuff(ModContent.BuffType<BloodyBoost>(), 600);
				damageMult += 1.25;
			}

            // Equivalent to reducing the player's DR by 20% because they have Cursed Inferno.
			if (CalamityWorld.revenge && Player.onFire2)
				damageMult += 0.2;

            modifiers.FinalDamage *= (float)damageMult;
            #endregion

            if (CalamityWorld.revenge)
            {
				double defenseMultiplier = /*Main.masterMode ? 1D :*/ 0.75;
                double newDamage = modifiers.SourceDamage.Base - (Player.statDefense * defenseMultiplier);
				double bossDamageLimitIncrease = CalamityWorld.death ? 40D : 20D; // dude why would you even do that? super uncool fab, 0/10, you should jump off a bridge for having the audacity to do something like this :(((((
				double newDamageLimit = NPC.downedMoonlord ? 20D : (NPC.downedPlantBoss || CalamityWorld.downedCalamitas) ? 15D : Main.hardMode ? 10D : 5D;
				/*if (areThereAnyDamnBosses && Main.masterMode)
					newDamageLimit += bossDamageLimitIncrease;*/

                if (newDamage < newDamageLimit)
                    newDamage = newDamageLimit;

                modifiers.FinalDamage.Base = (int)newDamage;
            }

			if (CalamityWorld.ironHeart)
			{
				int damageMin = 80 + (Player.statLifeMax2 / 10);
				modifiers.DisableSound();
				hurtSoundTimer = 20;
				if (modifiers.FinalDamage.Base <= damageMin)
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/IronHeartHurt"), Player.position);
				else
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/IronHeartBigHurt"), Player.position);
			}

			if (purpleCandle)
				modifiers.FinalDamage -= (float)(Player.statDefense * 0.05);

			if ((godSlayerDamage && modifiers.FinalDamage.Base <= 80) || modifiers.FinalDamage.Base < 1)
				modifiers.FinalDamage.Base = 1f;

            #region HealingEffects
            if (revivify)
            {
                int healAmt = (int)modifiers.SourceDamage.Base / 15;
                Player.statLife += healAmt;
                Player.HealEffect(healAmt);
            }
            if (daedalusAbsorb && Main.rand.NextBool(10))
            {
                int healAmt = (int)modifiers.SourceDamage.Base / 2;
                Player.statLife += healAmt;
                Player.HealEffect(healAmt);
            }
            if (absorber)
            {
                int healAmt = (int)modifiers.SourceDamage.Base / 20;
                Player.statLife += healAmt;
                Player.HealEffect(healAmt);
            }
            #endregion
        }
        #endregion

        #region Hurt
        public override void OnHurt(Player.HurtInfo info)
        {
            modStealth = 1f;
            if (Player.whoAmI == Main.myPlayer)
            {
                if (CalamityConfig.Instance.Rippers && CalamityWorld.revenge)
                {
                    if (!adrenalineModeActive && info.Damage > 0) //to prevent paladin's shield ruining adren even with 0 dmg taken
					{
                        adrenaline -= stressPills ? adrenalineMax / 2 : adrenalineMax;
						if (adrenaline < 0)
							adrenaline = 0;
					}
                }
                if (amidiasBlessing)
                {
                    Player.ClearBuff(ModContent.BuffType<AmidiasBlessing>());
                    SoundEngine.PlaySound(SoundID.Item96, Player.position);
                }
                if ((gShell || fabledTortoise) && !Player.panic)
                {
                    Player.AddBuff(ModContent.BuffType<ShellBoost>(), 300);
                }
                if (abyssalDivingSuitPlates && info.Damage > 50)
                {
                    abyssalDivingSuitPlateHits++;
                    if (abyssalDivingSuitPlateHits >= 3)
                    {
                        SoundEngine.PlaySound(SoundID.NPCDeath14, Player.position);
                        Player.AddBuff(ModContent.BuffType<AbyssalDivingSuitPlatesBroken>(), 10830);
                        for (int d = 0; d < 20; d++)
                        {
                            int dust = Dust.NewDust(Player.position, Player.width, Player.height, 31, 0f, 0f, 100, default, 2f);
                            Main.dust[dust].velocity *= 3f;
                            if (Main.rand.NextBool(2))
                            {
                                Main.dust[dust].scale = 0.5f;
                                Main.dust[dust].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                            }
                        }
                        for (int d = 0; d < 35; d++)
                        {
                            int fire = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                            Main.dust[fire].noGravity = true;
                            Main.dust[fire].velocity *= 5f;
                            fire = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                            Main.dust[fire].velocity *= 2f;
                        }
						CalamityUtils.ExplosionGores(Player.GetSource_FromThis(), Player.Center, 3);
                    }
                }
                if (sirenIce)
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath7, Player.Center);
                    Player.AddBuff(ModContent.BuffType<IceShieldBrokenBuff>(), 1800);
                    for (int d = 0; d < 10; d++)
                    {
                        int ice = Dust.NewDust(Player.position, Player.width, Player.height, 67, 0f, 0f, 100, default, 2f);
                        Main.dust[ice].velocity *= 3f;
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[ice].scale = 0.5f;
                            Main.dust[ice].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                        }
                    }
                    for (int d = 0; d < 15; d++)
                    {
                        int ice = Dust.NewDust(Player.position, Player.width, Player.height, 67, 0f, 0f, 100, default, 3f);
                        Main.dust[ice].noGravity = true;
                        Main.dust[ice].velocity *= 5f;
                        ice = Dust.NewDust(Player.position, Player.width, Player.height, 67, 0f, 0f, 100, default, 2f);
                        Main.dust[ice].velocity *= 2f;
                    }
                }
                if (tarraMelee)
                {
                    if (Main.rand.NextBool(4))
                    {
                        Player.AddBuff(ModContent.BuffType<TarraLifeRegen>(), 120);
                    }
                }
                else if (xerocSet)
                {
                    Player.AddBuff(ModContent.BuffType<XerocRage>(), 240);
                    Player.AddBuff(ModContent.BuffType<XerocWrath>(), 240);
                }
                else if (reaverBlast)
                {
                    Player.AddBuff(ModContent.BuffType<ReaverRage>(), 180);
                }
                if (fBarrier || (sirenBoobs && NPC.downedBoss3))
                {
                    SoundEngine.PlaySound(SoundID.Item27, Player.position);
                    for (int m = 0; m < Main.maxNPCs; m++)
                    {
						NPC npc = Main.npc[m];
						if (!npc.active || npc.friendly || npc.dontTakeDamage)
							continue;
						float npcDist = (npc.Center - Player.Center).Length();
						float freezeDist = (float)Main.rand.Next(200 + (int)info.Damage / 2, 301 + (int)info.Damage * 2);
						if (freezeDist > 500f)
						{
							freezeDist = 500f + (freezeDist - 500f) * 0.75f;
						}
						if (freezeDist > 700f)
						{
							freezeDist = 700f + (freezeDist - 700f) * 0.5f;
						}
						if (freezeDist > 900f)
						{
							freezeDist = 900f + (freezeDist - 900f) * 0.25f;
						}
						if (npcDist < freezeDist)
						{
							float duration = (float)Main.rand.Next(90 + (int)info.Damage / 3, 240 + (int)info.Damage / 2);
							npc.AddBuff(ModContent.BuffType<GlacialState>(), (int)duration, false);
						}
                    }
                }
                if (aBrain || amalgam)
                {
                    for (int m = 0; m < Main.maxNPCs; m++)
                    {
						NPC npc = Main.npc[m];
						if (!npc.active || npc.friendly || npc.dontTakeDamage)
							continue;
						float npcDist = (npc.Center - Player.Center).Length();
						float range = (float)Main.rand.Next(200 + (int)info.Damage / 2, 301 + (int)info.Damage * 2);
						if (range > 500f)
						{
							range = 500f + (range - 500f) * 0.75f;
						}
						if (range > 700f)
						{
							range = 700f + (range - 700f) * 0.5f;
						}
						if (range > 900f)
						{
							range = 900f + (range - 900f) * 0.25f;
						}
						if (npcDist < range)
						{
							float duration = (float)Main.rand.Next(90 + (int)info.Damage / 3, 300 + (int)info.Damage / 2);
							npc.AddBuff(BuffID.Confused, (int)duration, false);
							if (amalgam)
							{
								npc.AddBuff(ModContent.BuffType<BrimstoneFlames>(), (int)duration, false);
								npc.AddBuff(ModContent.BuffType<GodSlayerInferno>(), (int)duration, false);
								npc.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), (int)duration, false);
								npc.AddBuff(ModContent.BuffType<Irradiated>(), (int)duration, false);
							}
						}
                    }
					//Spawn the harmless brain images that are actually projectiles
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X + (float)Main.rand.Next(-40, 40), Player.Center.Y - (float)Main.rand.Next(20, 60), Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, ProjectileID.BrainOfConfusion, 0, 0f, Player.whoAmI, 0f, 0f);
                }
                if (polarisBoost)
                {
                    polarisBoostCounter = 0;
                    polarisBoost = false;
                    polarisBoostTwo = false;
                    polarisBoostThree = false;
                    if (Player.FindBuffIndex(ModContent.BuffType<PolarisBuff>()) > -1)
                    { Player.ClearBuff(ModContent.BuffType<PolarisBuff>()); }
                }
            }
            if (Player.ownedProjectileCounts[ModContent.ProjectileType<DrataliornusBow>()] != 0)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<DrataliornusBow>() && Main.projectile[i].owner == Player.whoAmI)
                    {
                        Main.projectile[i].Kill();
                        break;
                    }
                }
                if (Player.wingTime > Player.wingTimeMax / 2)
                    Player.wingTime = Player.wingTimeMax / 2;
            }
        }
        #endregion

        #region Post Hurt
        public override void PostHurt(Player.HurtInfo info)
        {
            if (!profanedCrystal && pArtifact)
            {
                Player.AddBuff(ModContent.BuffType<BurntOut>(), 300, true);
            }

            // Bloodflare Core defense shattering
            if (bloodflareCore)
            {
                // Shattered defense caps at half of total defense. Every hit adds its damage as shattered defense.
                bloodflareCoreLostDefense = Math.Min(bloodflareCoreLostDefense + (int)info.Damage, Player.statDefense / 2);

                // Play a sound and make dust to signify that defense has been shattered
                SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, Player.Center);
                for (int i = 0; i < 36; ++i)
                {
                    float speed = Main.rand.NextFloat(1.8f, 8f);
                    Vector2 dustVel = new Vector2(speed, speed);
                    Dust d = Dust.NewDustDirect(Player.position, Player.width, Player.height, 90);
                    d.velocity = dustVel;
                    d.noGravity = true;
                    d.scale *= Main.rand.NextFloat(1.1f, 1.4f);
                    Dust.CloneDust(d).velocity = dustVel.RotatedBy(MathHelper.PiOver2);
                    Dust.CloneDust(d).velocity = dustVel.RotatedBy(MathHelper.Pi);
                    Dust.CloneDust(d).velocity = dustVel.RotatedBy(MathHelper.Pi * 1.5f);
                }
            }

            bool hardMode = Main.hardMode;
            int iFramesToAdd = 0;
            if (Player.whoAmI == Main.myPlayer)
            {
                if (cTracers && info.Damage > 200)
                {
                    iFramesToAdd += 60;
                }
                if (godSlayerThrowing && info.Damage > 80)
                {
                    iFramesToAdd += 30;
                }
                if (statigelSet && info.Damage > 100)
                {
                    iFramesToAdd += 30;
                }
                if (dAmulet)
                {
                    if (info.Damage == 1.0)
                    {
                        iFramesToAdd += 10;
                    }
                    else
                    {
                        iFramesToAdd += 20;
                    }
                }
                if (fabsolVodka)
                {
                    if (info.Damage == 1.0)
                    {
                        iFramesToAdd += 5;
                    }
                    else
                    {
                        iFramesToAdd += 10;
                    }
                }
                if (BossRushEvent.BossRushActive && CalamityConfig.Instance.BossRushImmunityFrameCurse)
                {
                    bossRushImmunityFrameCurseTimer = 180 + Player.immuneTime;
                }
                if (info.Damage > 25)
                {
                    if (aeroSet)
                    {
                        for (int n = 0; n < 4; n++)
                        {
							CalamityUtils.ProjectileRain(Player.GetSource_FromThis(), Player.Center, 400f, 100f, 500f, 800f, 20f, ModContent.ProjectileType<StickyFeatherAero>(), (int)(20 * Player.AverageDamage()), 1f, Player.whoAmI);
                        }
                    }
                }
                if (aBulwark)
                {
                    if (aBulwarkRare)
                    {
                        SoundEngine.PlaySound(SoundID.Item74, Player.position);
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, 0f, ModContent.ProjectileType<GodSlayerBlaze>(), (int)(25 * Player.AverageDamage()), 5f, Player.whoAmI, 0f, 1f);
                    }
                    int starAmt = aBulwarkRare ? 12 : 5;
                    for (int n = 0; n < starAmt; n++)
                    {
						CalamityUtils.ProjectileRain(Player.GetSource_FromThis(), Player.Center, 400f, 100f, 500f, 800f, 29f, ModContent.ProjectileType<AstralStar>(), (int)(320 * Player.AverageDamage()), 5f, Player.whoAmI);
                    }
                }
                if (dAmulet)
                {
                    for (int n = 0; n < 3; n++)
                    {
						CalamityUtils.ProjectileRain(Player.GetSource_FromThis(), Player.Center, 400f, 100f, 500f, 800f, 29f, ProjectileID.HallowStar, (int)(130 * Player.AverageDamage()), 4f, Player.whoAmI, 6, 1, 5);
                    }

                    /*int num = 1;
					if (Main.rand.NextBool(3))
						++num;
					if (Main.rand.NextBool(3))
						++num;
					if (player.strongBees && Main.rand.NextBool(3))
						++num;
					for (int index = 0; index < num; ++index)
					{
						int bee = Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X, player.position.Y, (float) Main.rand.Next(-35, 36) * 0.02f, (float) Main.rand.Next(-35, 36) * 0.02f, player.beeType(), player.beeDamage(7), player.beeKB(0f), Main.myPlayer, 0f, 0f);
                        Main.projectile[bee].usesLocalNPCImmunity = true;
                        Main.projectile[bee].localNPCHitCooldown = 5;
					}*/
                }
                if (theBee)
                {
                    for (int n = 0; n < 3; n++)
                    {
						CalamityUtils.ProjectileRain(Player.GetSource_FromThis(), Player.Center, 400f, 100f, 500f, 800f, 29f, ProjectileID.HallowStar, (int)(150 * Player.AverageDamage()), 4f, Player.whoAmI, 6, 1, 5);
                    }
                    int num = 1;
                    if (Main.rand.NextBool(3))
                        ++num;
                    if (Main.rand.NextBool(3))
                        ++num;
                    if (Player.strongBees && Main.rand.NextBool(3))
                        ++num;
                    for (int index = 0; index < num; ++index)
                    {
                        int bee = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.position.X, Player.position.Y, (float) Main.rand.Next(-35, 36) * 0.02f, (float) Main.rand.Next(-35, 36) * 0.02f, Main.rand.NextBool(4) ? ModContent.ProjectileType<PlaguenadeBee>() : Player.beeType(), Player.beeDamage(7), Player.beeKB(0f), Main.myPlayer, 0f, 0f);
                        Main.projectile[bee].usesLocalNPCImmunity = true;
                        Main.projectile[bee].localNPCHitCooldown = 5;
                    }
                }
            }
            if (fCarapace)
            {
                if (info.Damage > 0)
                {
                    SoundEngine.PlaySound(SoundID.NPCHit45, Player.position);
                    float spread = 45f * 0.0174f;
                    double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;
                    int fDamage = (int)(56 * Player.AverageDamage());
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            float xPos = Main.rand.NextBool(2) ? Player.Center.X + 100 : Player.Center.X - 100;
                            Vector2 spawnPos = new Vector2(xPos, Player.Center.Y + Main.rand.Next(-100, 101));
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            int spore1 = Projectile.NewProjectile(Player.GetSource_FromThis(),spawnPos.X, spawnPos.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ProjectileID.TruffleSpore, fDamage, 1.25f, Player.whoAmI, 0f, 0f);
                            int spore2 = Projectile.NewProjectile(Player.GetSource_FromThis(),spawnPos.X, spawnPos.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ProjectileID.TruffleSpore, fDamage, 1.25f, Player.whoAmI, 0f, 0f);
                            Main.projectile[spore1].timeLeft = 120;
                            Main.projectile[spore2].timeLeft = 120;
                        }
                    }
                }
            }
            if (aSpark)
            {
                if (info.Damage > 0)
                {
                    SoundEngine.PlaySound(SoundID.Item93, Player.position);
                    float spread = 45f * 0.0174f;
                    double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;
                    int sDamage = hardMode ? 36 : 6;
                    if (aSparkRare)
                        sDamage += hardMode ? 12 : 2;
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            int spark1 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<Spark>(), (int)(sDamage * Player.AverageDamage()), 1.25f, Player.whoAmI, 0f, 0f);
                            int spark2 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<Spark>(), (int)(sDamage * Player.AverageDamage()), 1.25f, Player.whoAmI, 0f, 0f);
                            Main.projectile[spark1].timeLeft = 120;
                            Main.projectile[spark2].timeLeft = 120;
                            Main.projectile[spark1].Calamity().forceTypeless = true;
                            Main.projectile[spark2].Calamity().forceTypeless = true;
                        }
                    }
                }
            }
            if (inkBomb && !abyssalMirror && !eclipseMirror)
            {
                if (Player.whoAmI == Main.myPlayer && !inkBombCooldown)
                {
                    Player.AddBuff(ModContent.BuffType<InkBombCooldown>(), 1200);
                    rogueStealth += 0.5f;
                    for (int i = 0; i < 5; i++)
                    {
                        SoundEngine.PlaySound(SoundID.Item61, new Vector2(Main.player[Main.myPlayer].position.X, Main.player[Main.myPlayer].position.Y));
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-0f, -4f), ModContent.ProjectileType<InkBombProjectile>(), 0, 0, Player.whoAmI);
                    }
                }
            }
            if (blazingCore)
            {
                if (Player.ownedProjectileCounts[ModContent.ProjectileType<BlazingSun>()] < 1 && Player.ownedProjectileCounts[ModContent.ProjectileType<BlazingSun2>()] < 1)
                {
                    for (int i = 0; i < 360; i += 3)
                    {
                        Vector2 BCDSpeed = new Vector2(5f, 5f).RotatedBy(MathHelper.ToRadians(i));
                        Dust.NewDust(Player.Center, 1, 1, 244, BCDSpeed.X, BCDSpeed.Y, 0, default, 1.1f);
                    }
                    SoundEngine.PlaySound(SoundID.Item14, Player.Center);
                    int blazingSun = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<BlazingSun>(), (int)(1690 * Player.AverageDamage()), 0f, Player.whoAmI, 0f, 0f);
                    Main.projectile[blazingSun].Center = Player.Center;
                    int blazingSun2 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<BlazingSun2>(), 0, 0f, Player.whoAmI, 0f, 0f);
                    Main.projectile[blazingSun2].Center = Player.Center;
                }
            }
            if (ataxiaBlaze && Main.rand.NextBool(5))
            {
                if (info.Damage > 0)
                {
                    SoundEngine.PlaySound(SoundID.Item74, Player.position);
                    int eDamage = (int)(100 * Player.AverageDamage());
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<ChaosBlaze>(), eDamage, 1f, Player.whoAmI, 0f, 0f);
                    }
                }
            }
            else if (daedalusShard)
            {
                if (info.Damage > 0)
                {
                    SoundEngine.PlaySound(SoundID.Item27, Player.position);
                    float spread = 45f * 0.0174f;
                    double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;
                    int sDamage = (int)(27 * Player.RangedDamage()); //daedalus ranged helm
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            float randomSpeed = (float)Main.rand.Next(1, 7);
                            float randomSpeed2 = (float)Main.rand.Next(1, 7);
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            int shard = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f) + randomSpeed, ProjectileID.CrystalShard, sDamage, 1f, Player.whoAmI, 0f, 0f);
                            int shard2 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f) + randomSpeed2, ProjectileID.CrystalShard, sDamage, 1f, Player.whoAmI, 0f, 0f);
                            Main.projectile[shard].Calamity().forceTypeless = true;
                            Main.projectile[shard2].Calamity().forceTypeless = true;
                        }
                    }
                }
            }
            else if (reaverSpore)
            {
                if (info.Damage > 0)
                {
                    SoundEngine.PlaySound(SoundID.Item1, Player.position);
                    float spread = 45f * 0.0174f;
                    double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;
                    int rDamage = (int)(58 * Player.RogueDamage()); //Reaver rogue helm
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            float xPos = Main.rand.NextBool(2) ? Player.Center.X + 100 : Player.Center.X - 100;
                            Vector2 spawnPos = new Vector2(xPos, Player.Center.Y + Main.rand.Next(-100, 101));
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            int rspore1 = Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPos.X, spawnPos.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<ReaverSpore>(), rDamage, 2f, Player.whoAmI, 0f, 0f);
                            Main.projectile[rspore1].usesLocalNPCImmunity = true;
                            Main.projectile[rspore1].localNPCHitCooldown = 60;
                            int rspore2 = Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPos.X, spawnPos.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<ReaverSpore>(), rDamage, 2f, Player.whoAmI, 1f, 0f);
                            Main.projectile[rspore2].usesLocalNPCImmunity = true;
                            Main.projectile[rspore2].localNPCHitCooldown = 60;
                        }
                    }
                }
            }
            else if (godSlayerDamage) //god slayer melee helm
            {
                if (info.Damage > 80)
                {
                    SoundEngine.PlaySound(SoundID.Item73, Player.position);
                    float spread = 45f * 0.0174f;
                    double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<GodKiller>(), (int)(900 * Player.MeleeDamage()), 5f, Player.whoAmI, 0f, 0f);
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<GodKiller>(), (int)(900 * Player.MeleeDamage()), 5f, Player.whoAmI, 0f, 0f);
                        }
                    }
                }
            }
            else if (godSlayerMage)
            {
                if (info.Damage > 0)
                {
                    SoundEngine.PlaySound(SoundID.Item74, Player.position);
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<GodSlayerBlaze>(), (int)(1200 * Player.MagicDamage()), 1f, Player.whoAmI, 0f, 0f);
                    }
                }
            }
            else if (dsSetBonus)
            {
                if (Player.whoAmI == Main.myPlayer)
                {
                    for (int l = 0; l < 2; l++)
                    {
						CalamityUtils.ProjectileRain(Player.GetSource_FromThis(), Player.Center, 400f, 100f, 500f, 800f, 22f, ProjectileID.ShadowBeamFriendly, (int)(3000 * Player.AverageDamage()), 7f, Player.whoAmI, 6, 1);
                    }
                    for (int l = 0; l < 5; l++)
                    {
						CalamityUtils.ProjectileRain(Player.GetSource_FromThis(), Player.Center, 400f, 100f, 500f, 800f, 22f, ProjectileID.DemonScythe, (int)(5000 * Player.AverageDamage()), 7f, Player.whoAmI, 6, 1);
                    }
                }
            }
            if (lastProjectileHit != null)
            {
                switch (lastProjectileHit.ModProjectile.CooldownSlot)
                {
                    case 0:
                    case 1:
                        Player.hurtCooldowns[lastProjectileHit.ModProjectile.CooldownSlot] += iFramesToAdd;
                        break;
                    case -1:
                    default:
                        Player.immuneTime += iFramesToAdd;
                        break;
                }
            }
            else
            {
                Player.immuneTime += iFramesToAdd;
            }
        }
        #endregion

        #region Kill Player
        public void KillPlayer()
        {
            deathCount++;
            if (Player.whoAmI == Main.myPlayer && Main.netMode == NetmodeID.MultiplayerClient)
            {
                DeathPacket(false);
            }
            Player.lastDeathPostion = Player.Center;
            Player.lastDeathTime = DateTime.Now;
            Player.showLastDeath = true;
            bool specialDeath = CalamityWorld.ironHeart;
            int coinsOwned = (int)Utils.CoinsCount(out bool flag, Player.inventory, new int[0]);
            if (Main.myPlayer == Player.whoAmI)
            {
                Player.lostCoins = coinsOwned;
                Player.lostCoinString = Main.ValueToCoins(Player.lostCoins);
            }
            if (Main.myPlayer == Player.whoAmI)
            {
                Main.mapFullscreen = false;
            }
            if (Main.myPlayer == Player.whoAmI)
            {
                Player.trashItem.SetDefaults(0, false);
                if (Player.difficulty == 0)
                {
                    for (int i = 0; i < 59; i++)
                    {
                        if (Player.inventory[i].stack > 0 && ((Player.inventory[i].type >= ItemID.LargeAmethyst && Player.inventory[i].type <= ItemID.LargeDiamond) || Player.inventory[i].type == ItemID.LargeAmber))
                        {
                            int num = Item.NewItem(Player.GetSource_FromThis(), (int)Player.position.X, (int)Player.position.Y, Player.width, Player.height, Player.inventory[i].type, 1, false, 0, false, false);
                            Main.item[num].netDefaults(Player.inventory[i].netID);
                            Main.item[num].Prefix((int)Player.inventory[i].prefix);
                            Main.item[num].stack = Player.inventory[i].stack;
                            Main.item[num].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
                            Main.item[num].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
                            Main.item[num].noGrabDelay = 100;
                            Main.item[num].favorited = false;
                            Main.item[num].newAndShiny = false;
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num, 0f, 0f, 0f, 0, 0, 0);
                            }
                            Player.inventory[i].SetDefaults(0, false);
                        }
                    }
                }
                else if (Player.difficulty == 1)
                {
                    Player.DropItems();
                }
                else if (Player.difficulty == 2)
                {
                    Player.DropItems();
                    Player.KillMeForGood();
                }
            }
            if (specialDeath)
            {
                SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Item/IronHeartDeath"), Player.position);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.PlayerKilled, Player.position);
            }
            Player.headVelocity.Y = (float)Main.rand.Next(-40, -10) * 0.1f;
            Player.bodyVelocity.Y = (float)Main.rand.Next(-40, -10) * 0.1f;
            Player.legVelocity.Y = (float)Main.rand.Next(-40, -10) * 0.1f;
            Player.headVelocity.X = (float)Main.rand.Next(-20, 21) * 0.1f + (float)(2 * 0);
            Player.bodyVelocity.X = (float)Main.rand.Next(-20, 21) * 0.1f + (float)(2 * 0);
            Player.legVelocity.X = (float)Main.rand.Next(-20, 21) * 0.1f + (float)(2 * 0);
            if (Player.stoned)
            {
                Player.headPosition = Vector2.Zero;
                Player.bodyPosition = Vector2.Zero;
                Player.legPosition = Vector2.Zero;
            }
            for (int j = 0; j < 100; j++)
            {
                Dust.NewDust(Player.position, Player.width, Player.height, specialDeath ? 91 : 235, (float)(2 * 0), -2f, 0, default, 1f);
            }
            Player.mount.Dismount(Player);
            Player.dead = true;
            Player.respawnTimer = 600;
            if (Main.expertMode)
            {
                Player.respawnTimer = (int)(Player.respawnTimer * 1.5);
            }
            Player.immuneAlpha = 0;
            Player.palladiumRegen = false;
            Player.iceBarrier = false;
            Player.crystalLeaf = false;

            PlayerDeathReason damageSource = PlayerDeathReason.ByOther(Player.Male ? 14 : 15);
            if (abyssDeath)
            {
                if (Main.rand.NextBool(2))
                {
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + " is food for the Wyrms.");
                }
                else
                {
                    damageSource = PlayerDeathReason.ByCustomReason("Oxygen failed to reach " + Player.name + " from the depths of the Abyss.");
                }
            }
            else if (specialDeath)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was defeated.");
            }
            else if (CalamityWorld.death && deathModeBlizzardTime > 1980)
            {
                deathModeBlizzardTime = 0;
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was chilled to the bone by the frigid environment.");
            }
            else if (SCalLore)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.Male ? Player.name + " was consumed by his inner hatred." : Player.name + " was consumed by her inner hatred.");
            }
            else if (CalamityWorld.armageddon && areThereAnyDamnBosses)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " failed the challenge at hand.");
            }
            else if (BossRushEvent.BossRushActive && bossRushImmunityFrameCurseTimer > 0)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was destroyed by a mysterious force.");
            }
            NetworkText deathText = damageSource.GetDeathText(Player.name);
            if (Main.netMode == NetmodeID.MultiplayerClient && Player.whoAmI == Main.myPlayer)
            {
                NetMessage.SendPlayerDeath(Player.whoAmI, damageSource, (int)1000.0, 0, false, -1, -1);
            }
            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(deathText, new Color(225, 25, 25), -1);
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(deathText.ToString(), 225, 25, 25);
            }

            if (Player.whoAmI == Main.myPlayer && Player.difficulty == 0)
            {
                Player.DropCoins();
            }
            Player.DropTombstone(coinsOwned, deathText, 0);

            if (Player.whoAmI == Main.myPlayer)
            {
                try
                {
                    WorldGen.saveToonWhilePlaying();
                }
                catch
                {
                }
            }
        }
        #endregion

        #region Dash Stuff
        public bool dashInactive;
        public void ModDashMovement()
        {
            if (dashMod == 6 && dashInactive && Player.whoAmI == Main.myPlayer) //cryo lore
            {
                Rectangle rectangle = new Rectangle((int)((double)Player.position.X + (double)Player.velocity.X * 0.5 - 4.0), (int)((double)Player.position.Y + (double)Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
                for (int i = 0; i < Main.maxNPCs; i++)
                {
					NPC npc = Main.npc[i];
                    if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.immune[Player.whoAmI] <= 0)
                    {
                        Rectangle rect = npc.getRect();
                        if (rectangle.Intersects(rect) && (npc.noTileCollide || Player.CanHit(npc)))
                        {
                            float num = 50f * Player.AverageDamage();
                            float num2 = 3f;
                            bool crit = false;
                            if (Player.kbGlove)
                            {
                                num2 *= 2f;
                            }
                            if (Player.kbBuff)
                            {
                                num2 *= 1.5f;
                            }
                            if (Main.rand.Next(100) < Player.GetCritChance(DamageClass.Melee))
                            {
                                crit = true;
                            }
                            int direction = Player.direction;
                            if (Player.velocity.X < 0f)
                            {
                                direction = -1;
                            }
                            if (Player.velocity.X > 0f)
                            {
                                direction = 1;
                            }
                            if (Player.whoAmI == Main.myPlayer)
                            {
                                Player.ApplyDamageToNPC(npc, (int)num, num2, direction, crit);
                            }
							if (npc.immune[Player.whoAmI] < 6)
								npc.immune[Player.whoAmI] = 6;
                            npc.AddBuff(ModContent.BuffType<GlacialState>(), 300);
                            Player.immune = true;
                            Player.immuneNoBlink = true;
							if (Player.immuneTime < 4)
								Player.immuneTime = 4;
							for (int k = 0; k < Player.hurtCooldowns.Length; k++)
							{
								Player.hurtCooldowns[k] = Player.immuneTime;
							}
                        }
                    }
                }
            }
            if (dashMod == 4 && dashInactive && Player.whoAmI == Main.myPlayer) //Asgardian Aegis
            {
                Rectangle rectangle = new Rectangle((int)((double)Player.position.X + (double)Player.velocity.X * 0.5 - 4.0), (int)((double)Player.position.Y + (double)Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
                for (int i = 0; i < Main.maxNPCs; i++)
                {
					NPC npc = Main.npc[i];
                    if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.immune[Player.whoAmI] <= 0)
                    {
                        Rectangle rect = npc.getRect();
                        if (rectangle.Intersects(rect) && (npc.noTileCollide || Player.CanHit(npc)))
                        {
                            float num = 500f * Player.AverageDamage();
                            float num2 = 15f;
                            bool crit = false;
                            if (Player.kbGlove)
                            {
                                num2 *= 2f;
                            }
                            if (Player.kbBuff)
                            {
                                num2 *= 1.5f;
                            }
                            if (Main.rand.Next(100) < Player.GetCritChance(DamageClass.Melee))
                            {
                                crit = true;
                            }
                            int direction = Player.direction;
                            if (Player.velocity.X < 0f)
                            {
                                direction = -1;
                            }
                            if (Player.velocity.X > 0f)
                            {
                                direction = 1;
                            }
                            if (Player.whoAmI == Main.myPlayer)
                            {
                                Player.ApplyDamageToNPC(npc, (int)num, num2, direction, crit);
                                Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, 0f, ModContent.ProjectileType<HolyExplosionSupreme>(), (int)(300 * Player.AverageDamage()), 20f, Main.myPlayer, 0f, 0f);
                                Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, 0f, ModContent.ProjectileType<HolyEruption>(), (int)(200 * Player.AverageDamage()), 5f, Main.myPlayer, 0f, 0f);
                            }
							if (npc.immune[Player.whoAmI] < 6)
								npc.immune[Player.whoAmI] = 6;
                            npc.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
                            Player.immune = true;
                            Player.immuneNoBlink = true;
							if (Player.immuneTime < 4)
								Player.immuneTime = 4;
							for (int k = 0; k < Player.hurtCooldowns.Length; k++)
							{
								Player.hurtCooldowns[k] = Player.immuneTime;
							}
                        }
                    }
                }
            }
            if (dashMod == 3 && dashInactive && Player.whoAmI == Main.myPlayer) //Elysian Aegis
            {
                Rectangle rectangle = new Rectangle((int)((double)Player.position.X + (double)Player.velocity.X * 0.5 - 4.0), (int)((double)Player.position.Y + (double)Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
                for (int i = 0; i < Main.maxNPCs; i++)
                {
					NPC npc = Main.npc[i];
                    if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.immune[Player.whoAmI] <= 0)
                    {
                        Rectangle rect = npc.getRect();
                        if (rectangle.Intersects(rect) && (npc.noTileCollide || Player.CanHit(npc)))
                        {
                            float num = 350f * Player.AverageDamage();
                            float num2 = 12f;
                            bool crit = false;
                            if (Player.kbGlove)
                            {
                                num2 *= 2f;
                            }
                            if (Player.kbBuff)
                            {
                                num2 *= 1.5f;
                            }
                            if (Main.rand.Next(100) < Player.GetCritChance(DamageClass.Melee))
                            {
                                crit = true;
                            }
                            int direction = Player.direction;
                            if (Player.velocity.X < 0f)
                            {
                                direction = -1;
                            }
                            if (Player.velocity.X > 0f)
                            {
                                direction = 1;
                            }
                            if (Player.whoAmI == Main.myPlayer)
                            {
                                Player.ApplyDamageToNPC(npc, (int)num, num2, direction, crit);
                                Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, 0f, ModContent.ProjectileType<HolyExplosionSupreme>(), (int)(210 * Player.AverageDamage()), 15f, Main.myPlayer, 0f, 0f);
                                Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, 0f, ModContent.ProjectileType<HolyEruption>(), (int)(140 * Player.AverageDamage()), 5f, Main.myPlayer, 0f, 0f);
                            }
							if (npc.immune[Player.whoAmI] < 6)
								npc.immune[Player.whoAmI] = 6;
                            Player.immune = true;
                            Player.immuneNoBlink = true;
							if (Player.immuneTime < 4)
								Player.immuneTime = 4;
							for (int k = 0; k < Player.hurtCooldowns.Length; k++)
							{
								Player.hurtCooldowns[k] = Player.immuneTime;
							}
                        }
                    }
                }
            }
            if (dashMod == 2 && dashInactive && Player.whoAmI == Main.myPlayer) //Asgard's Valor
            {
                Rectangle rectangle = new Rectangle((int)((double)Player.position.X + (double)Player.velocity.X * 0.5 - 4.0), (int)((double)Player.position.Y + (double)Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
                for (int i = 0; i < Main.maxNPCs; i++)
                {
					NPC npc = Main.npc[i];
                    if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.immune[Player.whoAmI] <= 0)
                    {
                        Rectangle rect = npc.getRect();
                        if (rectangle.Intersects(rect) && (npc.noTileCollide || Player.CanHit(npc)))
                        {
                            float num = 100f * Player.AverageDamage();
                            float num2 = 9f;
                            bool crit = false;
                            if (Player.kbGlove)
                            {
                                num2 *= 2f;
                            }
                            if (Player.kbBuff)
                            {
                                num2 *= 1.5f;
                            }
                            if (Main.rand.Next(100) < Player.GetCritChance(DamageClass.Melee))
                            {
                                crit = true;
                            }
                            int direction = Player.direction;
                            if (Player.velocity.X < 0f)
                            {
                                direction = -1;
                            }
                            if (Player.velocity.X > 0f)
                            {
                                direction = 1;
                            }
                            if (Player.whoAmI == Main.myPlayer)
                            {
                                Player.ApplyDamageToNPC(npc, (int)num, num2, direction, crit);
                                Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, 0f, ModContent.ProjectileType<HolyExplosion>(), (int)(60 * Player.AverageDamage()), 15f, Main.myPlayer, 0f, 0f);
                            }
							if (npc.immune[Player.whoAmI] < 6)
								npc.immune[Player.whoAmI] = 6;
                            Player.immune = true;
                            Player.immuneNoBlink = true;
							if (Player.immuneTime < 4)
								Player.immuneTime = 4;
							for (int k = 0; k < Player.hurtCooldowns.Length; k++)
							{
								Player.hurtCooldowns[k] = Player.immuneTime;
							}
                        }
                    }
                }
            }
            if (dashMod == 8 && dashInactive && Player.whoAmI == Main.myPlayer) //plaguebringer armor
            {
                Rectangle rectangle = new Rectangle((int)(Player.position.X + Player.velocity.X * 0.5f - 4f), (int)(Player.position.Y + Player.velocity.Y * 0.5f - 4f), Player.width + 8, Player.height + 8);
                for (int i = 0; i < Main.maxNPCs; i++)
                {
					NPC npc = Main.npc[i];
                    if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.immune[Player.whoAmI] <= 0)
                    {
                        Rectangle rect = npc.getRect();
                        if (rectangle.Intersects(rect) && (npc.noTileCollide || Player.CanHit(npc)))
                        {
                            float num = 50f * Player.MinionDamage();
                            float num2 = 3f;
                            bool crit = false;
                            int direction = Player.direction;
                            if (Player.velocity.X < 0f)
                            {
                                direction = -1;
                            }
                            if (Player.velocity.X > 0f)
                            {
                                direction = 1;
                            }
                            if (Player.whoAmI == Main.myPlayer)
                            {
                                Player.ApplyDamageToNPC(npc, (int)num, num2, direction, crit);
                            }
							if (npc.immune[Player.whoAmI] < 6)
								npc.immune[Player.whoAmI] = 6;
                            npc.AddBuff(ModContent.BuffType<Plague>(), 300);
                            Player.immune = true;
                            Player.immuneNoBlink = true;
							if (Player.immuneTime < 4)
								Player.immuneTime = 4;
							for (int k = 0; k < Player.hurtCooldowns.Length; k++)
							{
								Player.hurtCooldowns[k] = Player.immuneTime;
							}
                        }
                    }
                }
            }
            if (dashMod == 1 && dashInactive && Player.whoAmI == Main.myPlayer) //Counter Scarf
            {
                Rectangle rectangle = new Rectangle((int)((double)Player.position.X + (double)Player.velocity.X * 0.5 - 4.0), (int)((double)Player.position.Y + (double)Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
                for (int i = 0; i < Main.maxNPCs; i++)
                {
					NPC npc = Main.npc[i];
                    if (npc.active && !npc.dontTakeDamage && !npc.friendly && !npc.townNPC && npc.immune[Player.whoAmI] <= 0 && npc.damage > 0)
                    {
                        Rectangle rect = npc.getRect();
                        if (rectangle.Intersects(rect) && (npc.noTileCollide || Player.CanHit(npc)))
                        {
                            OnDodge();
                            break;
                        }
                    }
                }
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
					Projectile proj = Main.projectile[i];
                    if (proj.active && !proj.friendly && proj.hostile && proj.damage > 0)
                    {
                        Rectangle rect = proj.getRect();
                        if (rectangle.Intersects(rect))
                        {
                            OnDodge();
                            break;
                        }
                    }
                }
            }
            if (Player.dashDelay > 0)
            {
                return;
            }
            if (dashInactive)
            {
                float num7 = 12f;
                float num8 = 0.985f;
                float num9 = Math.Max(Player.accRunSpeed, Player.maxRunSpeed);
                float num10 = 0.94f;
                int delay = 20;
				if (dashMod == 1) //Counter Scarf
                {
                    for (int k = 0; k < 2; k++)
                    {
                        int num12;
                        if (Player.velocity.Y == 0f)
                        {
                            num12 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)Player.height - 4f), Player.width, 8, 235, 0f, 0f, 100, default, 1.4f);
                        }
                        else
                        {
                            num12 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)(Player.height / 2) - 8f), Player.width, 16, 235, 0f, 0f, 100, default, 1.4f);
                        }
                        Main.dust[num12].velocity *= 0.1f;
                        Main.dust[num12].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                        Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                }
                else if (dashMod == 2) //Asgard's Valor
                {
                    for (int m = 0; m < 4; m++)
                    {
                        int num14 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 4f), Player.width, Player.height - 8, 246, 0f, 0f, 100, default, 2.75f);
                        Main.dust[num14].velocity *= 0.1f;
                        Main.dust[num14].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                        Main.dust[num14].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                        Main.dust[num14].noGravity = true;
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[num14].fadeIn = 0.5f;
                        }
                    }
                }
                else if (dashMod == 3) //Elysian Aegis
                {
                    for (int m = 0; m < 12; m++)
                    {
                        int num14 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 4f), Player.width, Player.height - 8, 244, 0f, 0f, 100, default, 2.75f);
                        Main.dust[num14].velocity *= 0.1f;
                        Main.dust[num14].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                        Main.dust[num14].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                        Main.dust[num14].noGravity = true;
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[num14].fadeIn = 0.5f;
                        }
                    }
                    num7 = 14f; //14
                }
                else if (dashMod == 4) //Asgardian Aegis
                {
                    for (int m = 0; m < 24; m++)
                    {
                        int num14 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 4f), Player.width, Player.height - 8, 244, 0f, 0f, 100, default, 2.75f);
                        Main.dust[num14].velocity *= 0.1f;
                        Main.dust[num14].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                        Main.dust[num14].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                        Main.dust[num14].noGravity = true;
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[num14].fadeIn = 0.5f;
                        }
                    }
                    num7 = 16f; //14
                }
                else if (dashMod == 5) //Deep Diver
                {
                    for (int m = 0; m < 24; m++)
                    {
                        int num14 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 4f), Player.width, Player.height - 8, 33, 0f, 0f, 100, default, 2.75f);
                        Main.dust[num14].velocity *= 0.1f;
                        Main.dust[num14].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                        Main.dust[num14].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                        Main.dust[num14].noGravity = true;
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[num14].fadeIn = 0.5f;
                        }
                    }
                    num7 = 18f; //14
                }
                else if (dashMod == 6) //Cryogen Lore
                {
                    for (int m = 0; m < 24; m++)
                    {
                        int num14 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 4f), Player.width, Player.height - 8, 67, 0f, 0f, 100, default, 1f);
                        Main.dust[num14].velocity *= 0.1f;
                        Main.dust[num14].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                        Main.dust[num14].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                        Main.dust[num14].noGravity = true;
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[num14].fadeIn = 0.5f;
                        }
                    }
                    num7 = 12.5f; //14
					delay = 30;
                }
                else if (dashMod == 7) //Statis' Belt of Curses
                {
					statisTimer++;
                    for (int k = 0; k < 2; k++)
                    {
                        int num12;
                        if (Player.velocity.Y == 0f)
                        {
                            num12 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)Player.height - 4f), Player.width, 8, 70, 0f, 0f, 100, default, 1.4f);
                        }
                        else
                        {
                            num12 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)(Player.height / 2) - 8f), Player.width, 16, 70, 0f, 0f, 100, default, 1.4f);
                        }
                        Main.dust[num12].velocity *= 0.1f;
                        Main.dust[num12].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                        Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    num7 = 14f; //14
					if (statisTimer % 5 == 0)
					{
						int scythe = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<CosmicScythe>(), (int)(500 * Player.AverageDamage()), 5f, Player.whoAmI);
						Main.projectile[scythe].Calamity().forceTypeless = true;
						Main.projectile[scythe].usesIDStaticNPCImmunity = true;
						Main.projectile[scythe].idStaticNPCHitCooldown = 10;
					}
                }
                else if (dashMod == 8) //Plaguebringer armor
                {
                    for (int m = 0; m < 24; m++)
                    {
                        int num14 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 4f), Player.width, Player.height - 8, 89, 0f, 0f, 100, default, 1f);
                        Main.dust[num14].velocity *= 0.1f;
                        Main.dust[num14].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                        Main.dust[num14].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                        Main.dust[num14].noGravity = true;
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[num14].fadeIn = 0.5f;
                        }
                    }
                    num7 = 12.5f; //14
                }
                if (dashMod > 0)
                {
                    Player.vortexStealthActive = false;
                    if (Player.velocity.X > num7 || Player.velocity.X < -num7)
                    {
                        Player.velocity.X = Player.velocity.X * num8;
                        return;
                    }
                    if (Player.velocity.X > num9 || Player.velocity.X < -num9)
                    {
                        Player.velocity.X = Player.velocity.X * num10;
                        return;
                    }
                    Player.dashDelay = delay;
                    dashInactive = false;
                    if (Player.velocity.X < 0f)
                    {
                        Player.velocity.X = -num9;
                        return;
                    }
                    if (Player.velocity.X > 0f)
                    {
                        Player.velocity.X = num9;
                        return;
                    }
                }
            }
            else if (dashMod > 0 && !Player.mount.Active)
            {
                float dashDistance;
                if (dashMod == 1) //Counter and Evasion Scarf
                {
                    dashDistance = evasionScarf ? 16.3f : 14.5f;
                    int direction = 0;
                    bool justDashed = false;
                    if (dashTimeMod > 0)
                    {
                        dashTimeMod--;
                    }
                    if (dashTimeMod < 0)
                    {
                        dashTimeMod++;
                    }
                    if (Player.controlRight && Player.releaseRight)
                    {
                        if (dashTimeMod > 0)
                        {
                            direction = 1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = 15;
                        }
                    }
                    else if (Player.controlLeft && Player.releaseLeft)
                    {
                        if (dashTimeMod < 0)
                        {
                            direction = -1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = -15;
                        }
                    }
                    if (justDashed)
                    {
                        Player.velocity.X = dashDistance * (float)direction; //eoc dash amount (evasion = asgard's)
                        Point point = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), Player.gravDir * (float)-(float)Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                        Point point2 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), 0f)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y))
                        {
                            Player.velocity.X = Player.velocity.X / 2f;
                        }
                        dashInactive = true;
                        for (int num17 = 0; num17 < 20; num17++)
                        {
                            int num18 = Dust.NewDust(Player.position, Player.width, Player.height, 235, 0f, 0f, 100, default, 2f);
                            Dust dust = Main.dust[num18];
                            dust.position.X += (float)Main.rand.Next(-5, 6);
                            dust.position.Y += (float)Main.rand.Next(-5, 6);
                            dust.velocity *= 0.2f;
                            dust.scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                            dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                        }
                        return;
                    }
                }
                else if (dashMod == 2) //Asgard's Valor
                {
                    dashDistance = 16.9f;
                    int direction = 0;
                    bool justDashed = false;
                    if (dashTimeMod > 0)
                    {
                        dashTimeMod--;
                    }
                    if (dashTimeMod < 0)
                    {
                        dashTimeMod++;
                    }
                    if (Player.controlRight && Player.releaseRight)
                    {
                        if (dashTimeMod > 0)
                        {
                            direction = 1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = 15;
                        }
                    }
                    else if (Player.controlLeft && Player.releaseLeft)
                    {
                        if (dashTimeMod < 0)
                        {
                            direction = -1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = -15;
                        }
                    }
                    if (justDashed)
                    {
                        Player.velocity.X = dashDistance * (float)direction; //tabi dash amount
                        Point point5 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), Player.gravDir * (float)-(float)Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                        Point point6 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), 0f)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(point5.X, point5.Y) || WorldGen.SolidOrSlopedTile(point6.X, point6.Y))
                        {
                            Player.velocity.X = Player.velocity.X / 2f;
                        }
                        dashInactive = true;
                        for (int num24 = 0; num24 < 20; num24++)
                        {
                            int num25 = Dust.NewDust(Player.position, Player.width, Player.height, 246, 0f, 0f, 100, default, 3f);
                            Dust dust = Main.dust[num25];
                            dust.position.X += (float)Main.rand.Next(-5, 6);
                            dust.position.Y += (float)Main.rand.Next(-5, 6);
                            dust.velocity *= 0.2f;
                            dust.scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                            dust.shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                            dust.noGravity = true;
                            dust.fadeIn = 0.5f;
                        }
                    }
                }
                else if (dashMod == 3) //Elysian Aegis
                {
                    dashDistance = 21.9f;
                    int direction = 0;
                    bool justDashed = false;
                    if (dashTimeMod > 0)
                    {
                        dashTimeMod--;
                    }
                    if (dashTimeMod < 0)
                    {
                        dashTimeMod++;
                    }
                    if (Player.controlRight && Player.releaseRight)
                    {
                        if (dashTimeMod > 0)
                        {
                            direction = 1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = 15;
                        }
                    }
                    else if (Player.controlLeft && Player.releaseLeft)
                    {
                        if (dashTimeMod < 0)
                        {
                            direction = -1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = -15;
                        }
                    }
                    if (justDashed)
                    {
                        Player.velocity.X = dashDistance * (float)direction; //solar dash amount
                        Point point5 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), Player.gravDir * (float)-(float)Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                        Point point6 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), 0f)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(point5.X, point5.Y) || WorldGen.SolidOrSlopedTile(point6.X, point6.Y))
                        {
                            Player.velocity.X = Player.velocity.X / 2f;
                        }
                        dashInactive = true;
                        for (int num24 = 0; num24 < 40; num24++)
                        {
                            int num25 = Dust.NewDust(Player.position, Player.width, Player.height, 244, 0f, 0f, 100, default, 3f);
                            Dust dust = Main.dust[num25];
                            dust.position.X += (float)Main.rand.Next(-5, 6);
                            dust.position.Y += (float)Main.rand.Next(-5, 6);
                            dust.velocity *= 0.2f;
                            dust.scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                            dust.shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                            dust.noGravity = true;
                            dust.fadeIn = 0.5f;
                        }
                    }
                }
                else if (dashMod == 4) //Asgardian Aegis
                {
                    dashDistance = 22.3f;
                    int direction = 0;
                    bool justDashed = false;
                    if (dashTimeMod > 0)
                    {
                        dashTimeMod--;
                    }
                    if (dashTimeMod < 0)
                    {
                        dashTimeMod++;
                    }
                    if (Player.controlRight && Player.releaseRight)
                    {
                        if (dashTimeMod > 0)
                        {
                            direction = 1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = 15;
                        }
                    }
                    else if (Player.controlLeft && Player.releaseLeft)
                    {
                        if (dashTimeMod < 0)
                        {
                            direction = -1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = -15;
                        }
                    }
                    if (justDashed)
                    {
                        Player.velocity.X = dashDistance * (float)direction; //slighty more powerful solar dash
                        Point point5 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), Player.gravDir * (float)-(float)Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                        Point point6 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), 0f)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(point5.X, point5.Y) || WorldGen.SolidOrSlopedTile(point6.X, point6.Y))
                        {
                            Player.velocity.X = Player.velocity.X / 2f;
                        }
                        dashInactive = true;
                        for (int num24 = 0; num24 < 60; num24++)
                        {
                            int num25 = Dust.NewDust(Player.position, Player.width, Player.height, 244, 0f, 0f, 100, default, 3f);
                            Dust dust = Main.dust[num25];
                            dust.position.X += (float)Main.rand.Next(-5, 6);
                            dust.position.Y += (float)Main.rand.Next(-5, 6);
                            dust.velocity *= 0.2f;
                            dust.scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                            dust.shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                            dust.noGravity = true;
                            dust.fadeIn = 0.5f;
                        }
                    }
                }
                else if (dashMod == 5) //Deep Diver
                {
                    dashDistance = 25.9f;
                    int direction = 0;
                    bool justDashed = false;
                    if (dashTimeMod > 0)
                    {
                        dashTimeMod--;
                    }
                    if (dashTimeMod < 0)
                    {
                        dashTimeMod++;
                    }
                    if (Player.controlRight && Player.releaseRight)
                    {
                        if (dashTimeMod > 0)
                        {
                            direction = 1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = 15;
                        }
                    }
                    else if (Player.controlLeft && Player.releaseLeft)
                    {
                        if (dashTimeMod < 0)
                        {
                            direction = -1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = -15;
                        }
                    }
                    if (justDashed)
                    {
                        Player.velocity.X = dashDistance * (float)direction;
                        Point point5 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), Player.gravDir * (float)-(float)Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                        Point point6 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), 0f)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(point5.X, point5.Y) || WorldGen.SolidOrSlopedTile(point6.X, point6.Y))
                        {
                            Player.velocity.X = Player.velocity.X / 2f;
                        }
                        dashInactive = true;
                        for (int num24 = 0; num24 < 60; num24++)
                        {
                            int num25 = Dust.NewDust(Player.position, Player.width, Player.height, 33, 0f, 0f, 100, default, 3f);
                            Dust dust = Main.dust[num25];
                            dust.position.X += (float)Main.rand.Next(-5, 6);
                            dust.position.Y += (float)Main.rand.Next(-5, 6);
                            dust.velocity *= 0.2f;
                            dust.scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                            dust.shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                            dust.noGravity = true;
                            dust.fadeIn = 0.5f;
                        }
                    }
                }
                else if (dashMod == 6) //Cryogen Lore
                {
                    dashDistance = 15.7f;
                    int direction = 0;
                    bool justDashed = false;
                    if (dashTimeMod > 0)
                    {
                        dashTimeMod--;
                    }
                    if (dashTimeMod < 0)
                    {
                        dashTimeMod++;
                    }
                    if (Player.controlRight && Player.releaseRight)
                    {
                        if (dashTimeMod > 0)
                        {
                            direction = 1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = 15;
                        }
                    }
                    else if (Player.controlLeft && Player.releaseLeft)
                    {
                        if (dashTimeMod < 0)
                        {
                            direction = -1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = -15;
                        }
                    }
                    if (justDashed)
                    {
                        Player.velocity.X = dashDistance * (float)direction;
                        Point point5 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), Player.gravDir * (float)-(float)Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                        Point point6 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), 0f)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(point5.X, point5.Y) || WorldGen.SolidOrSlopedTile(point6.X, point6.Y))
                        {
                            Player.velocity.X = Player.velocity.X / 2f;
                        }
                        dashInactive = true;
                        for (int num24 = 0; num24 < 60; num24++)
                        {
                            int num25 = Dust.NewDust(Player.position, Player.width, Player.height, 67, 0f, 0f, 100, default, 1.25f);
                            Dust dust = Main.dust[num25];
                            dust.position.X += (float)Main.rand.Next(-5, 6);
                            dust.position.Y += (float)Main.rand.Next(-5, 6);
                            dust.velocity *= 0.2f;
                            dust.scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                            dust.shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                            dust.noGravity = true;
                            dust.fadeIn = 0.5f;
                        }
                    }
                }
                else if (dashMod == 7) //Statis' Belt of Curses
                {
                    dashDistance = 23.9f;
                    int direction = 0;
                    bool justDashed = false;
                    if (dashTimeMod > 0)
                    {
                        dashTimeMod--;
                    }
                    if (dashTimeMod < 0)
                    {
                        dashTimeMod++;
                    }
                    if (Player.controlRight && Player.releaseRight)
                    {
                        if (dashTimeMod > 0)
                        {
                            direction = 1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = 15;
                        }
                    }
                    else if (Player.controlLeft && Player.releaseLeft)
                    {
                        if (dashTimeMod < 0)
                        {
                            direction = -1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = -15;
                        }
                    }
                    if (justDashed)
                    {
                        Player.velocity.X = dashDistance * (float)direction; //solar dash amount
                        Point point = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), Player.gravDir * (float)-(float)Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                        Point point2 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), 0f)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y))
                        {
                            Player.velocity.X = Player.velocity.X / 2f;
                        }
                        dashInactive = true;
                        for (int num17 = 0; num17 < 20; num17++)
                        {
                            int num18 = Dust.NewDust(Player.position, Player.width, Player.height, 70, 0f, 0f, 100, default, 2f);
                            Dust dust = Main.dust[num18];
                            dust.position.X += (float)Main.rand.Next(-5, 6);
                            dust.position.Y += (float)Main.rand.Next(-5, 6);
                            dust.velocity *= 0.2f;
                            dust.scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                            dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                        }
                        return;
                    }
                }
                else if (dashMod == 8) //Plaguebringer armor
                {
                    dashDistance = 19f;
                    int direction = 0;
                    bool justDashed = false;
                    if (dashTimeMod > 0)
                    {
                        dashTimeMod--;
                    }
                    if (dashTimeMod < 0)
                    {
                        dashTimeMod++;
                    }
                    if (Player.controlRight && Player.releaseRight)
                    {
                        if (dashTimeMod > 0)
                        {
                            direction = 1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = 15;
                        }
                    }
                    else if (Player.controlLeft && Player.releaseLeft)
                    {
                        if (dashTimeMod < 0)
                        {
                            direction = -1;
                            justDashed = true;
                            dashTimeMod = 0;
                        }
                        else
                        {
                            dashTimeMod = -15;
                        }
                    }
                    if (justDashed)
                    {
                        Player.velocity.X = dashDistance * (float)direction;
                        Point point5 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), Player.gravDir * (float)-(float)Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                        Point point6 = (Player.Center + new Vector2((float)(direction * Player.width / 2 + 2), 0f)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(point5.X, point5.Y) || WorldGen.SolidOrSlopedTile(point6.X, point6.Y))
                        {
                            Player.velocity.X = Player.velocity.X / 2f;
                        }
                        dashInactive = true;
                        for (int num24 = 0; num24 < 60; num24++)
                        {
                            int num25 = Dust.NewDust(Player.position, Player.width, Player.height, 89, 0f, 0f, 100, default, 1.25f);
                            Dust dust = Main.dust[num25];
                            dust.position.X += (float)Main.rand.Next(-5, 6);
                            dust.position.Y += (float)Main.rand.Next(-5, 6);
                            dust.velocity *= 0.2f;
                            dust.scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                            dust.shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                            dust.noGravity = true;
                            dust.fadeIn = 0.5f;
                        }
                    }
                }
            }
        }

        private void OnDodge()
        {
            if (Player.whoAmI == Main.myPlayer && dodgeScarf && !scarfCooldown && !eScarfCooldown)
            {
				if (evasionScarf)
				{
					Player.AddBuff(ModContent.BuffType<EvasionScarfBoost>(), CalamityUtils.SecondsToFrames(9f));
					Player.AddBuff(ModContent.BuffType<EvasionScarfCooldown>(), Player.chaosState ? CalamityUtils.SecondsToFrames(20f) : CalamityUtils.SecondsToFrames(13f));
				}
				else
				{
					Player.AddBuff(ModContent.BuffType<ScarfMeleeBoost>(), 540);
					Player.AddBuff(ModContent.BuffType<ScarfCooldown>(), Player.chaosState ? 1800 : 900);
				}
                Player.immune = true;
                Player.immuneTime = Player.longInvince ? 100 : 60;
                for (int k = 0; k < Player.hurtCooldowns.Length; k++)
                {
                    Player.hurtCooldowns[k] = Player.immuneTime;
                }
                for (int j = 0; j < 100; j++)
                {
                    int num = Dust.NewDust(Player.position, Player.width, Player.height, 235, 0f, 0f, 100, default, 2f);
                    Dust dust = Main.dust[num];
                    dust.position.X += (float)Main.rand.Next(-20, 21);
                    dust.position.Y += (float)Main.rand.Next(-20, 21);
                    dust.velocity *= 0.4f;
                    dust.scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                    dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cWaist, Player);
                    if (Main.rand.NextBool(2))
                    {
                        dust.scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                        dust.noGravity = true;
                    }
                }
                if (Player.whoAmI == Main.myPlayer)
                {
                    NetMessage.SendData(MessageID.Dodge, -1, -1, null, Player.whoAmI, 1f, 0f, 0f, 0, 0, 0);
                }
            }
        }

        public void ModHorizontalMovement()
        {
            float num = (Player.accRunSpeed + Player.maxRunSpeed) / 2f;
            if (Player.controlLeft && Player.velocity.X > -Player.accRunSpeed && Player.dashDelay >= 0)
            {
                if (Player.velocity.X < -num && Player.velocity.Y == 0f && !Player.mount.Active)
                {
                    int num3 = 0;
                    if (Player.gravDir == -1f)
                    {
                        num3 -= Player.height;
                    }
					if (dashMod == 1)
                    {
                        int num7 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num3), Player.width + 8, 4, 235, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 1.5f);
                        Main.dust[num7].velocity.X *= 0.2f;
                        Main.dust[num7].velocity.Y *= 0.2f;
                        Main.dust[num7].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 2)
                    {
                        int num7 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num3), Player.width + 8, 4, 246, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 2.5f);
                        Main.dust[num7].velocity.X *= 0.2f;
                        Main.dust[num7].velocity.Y *= 0.2f;
                        Main.dust[num7].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 3)
                    {
                        int num7 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num3), Player.width + 8, 4, 244, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 3f);
                        Main.dust[num7].velocity.X *= 0.2f;
                        Main.dust[num7].velocity.Y *= 0.2f;
                        Main.dust[num7].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 4)
                    {
                        int num7 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num3), Player.width + 8, 4, 244, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 3f);
                        Main.dust[num7].velocity.X *= 0.2f;
                        Main.dust[num7].velocity.Y *= 0.2f;
                        Main.dust[num7].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 5)
                    {
                        int num7 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num3), Player.width + 8, 4, 33, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 3f);
                        Main.dust[num7].velocity.X *= 0.2f;
                        Main.dust[num7].velocity.Y *= 0.2f;
                        Main.dust[num7].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 6)
                    {
                        int num7 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num3), Player.width + 8, 4, 67, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 1.25f);
                        Main.dust[num7].velocity.X *= 0.2f;
                        Main.dust[num7].velocity.Y *= 0.2f;
                        Main.dust[num7].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 7)
                    {
                        int num7 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num3), Player.width + 8, 4, 70, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 1.5f);
                        Main.dust[num7].velocity.X *= 0.2f;
                        Main.dust[num7].velocity.Y *= 0.2f;
                        Main.dust[num7].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 8)
                    {
                        int num7 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num3), Player.width + 8, 4, 89, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 1.25f);
                        Main.dust[num7].velocity.X *= 0.2f;
                        Main.dust[num7].velocity.Y *= 0.2f;
                        Main.dust[num7].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                }
            }
            else if (Player.controlRight && Player.velocity.X < Player.accRunSpeed && Player.dashDelay >= 0)
            {
                if (Player.velocity.X > num && Player.velocity.Y == 0f && !Player.mount.Active)
                {
                    int num8 = 0;
                    if (Player.gravDir == -1f)
                    {
                        num8 -= Player.height;
                    }
					if (dashMod == 1)
                    {
                        int num12 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num8), Player.width + 8, 4, 235, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 1.5f);
                        Main.dust[num12].velocity.X = Main.dust[num12].velocity.X * 0.2f;
                        Main.dust[num12].velocity.Y = Main.dust[num12].velocity.Y * 0.2f;
                        Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 2)
                    {
                        int num12 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num8), Player.width + 8, 4, 246, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 2.5f);
                        Main.dust[num12].velocity.X *= 0.2f;
                        Main.dust[num12].velocity.Y *= 0.2f;
                        Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 3)
                    {
                        int num12 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num8), Player.width + 8, 4, 244, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 3f);
                        Main.dust[num12].velocity.X *= 0.2f;
                        Main.dust[num12].velocity.Y *= 0.2f;
                        Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 4)
                    {
                        int num12 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num8), Player.width + 8, 4, 244, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 3f);
                        Main.dust[num12].velocity.X *= 0.2f;
                        Main.dust[num12].velocity.Y *= 0.2f;
                        Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
					else if (dashMod == 5)
                    {
                        int num12 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num8), Player.width + 8, 4, 33, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 3f);
                        Main.dust[num12].velocity.X *= 0.2f;
                        Main.dust[num12].velocity.Y *= 0.2f;
                        Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 6)
                    {
                        int num12 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num8), Player.width + 8, 4, 67, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 1.25f);
                        Main.dust[num12].velocity.X *= 0.2f;
                        Main.dust[num12].velocity.Y *= 0.2f;
                        Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 7)
                    {
                        int num12 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num8), Player.width + 8, 4, 70, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 1.5f);
                        Main.dust[num12].velocity.X *= 0.2f;
                        Main.dust[num12].velocity.Y *= 0.2f;
                        Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                    else if (dashMod == 8)
                    {
                        int num12 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num8), Player.width + 8, 4, 89, -Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 50, default, 1.25f);
                        Main.dust[num12].velocity.X *= 0.2f;
                        Main.dust[num12].velocity.Y *= 0.2f;
                        Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    }
                }
            }

            if (Player.mount.Active && Player.mount.Type == ModContent.MountType<AlicornMount>() && Math.Abs(Player.velocity.X) > Player.mount.DashSpeed - Player.mount.RunSpeed / 2f)
            {
                Rectangle rect = Player.getRect();

                if (Player.direction == 1)
                    rect.Offset(Player.width - 1, 0);

                rect.Width = 2;
                rect.Inflate(6, 12);
                float damage = 800f * Player.MinionDamage();
                float knockback = 10f;
                int nPCImmuneTime = 30;
                int playerImmuneTime = 6;
                ModCollideWithNPCs(rect, damage,knockback, nPCImmuneTime, playerImmuneTime);
            }

            if (Player.mount.Active && Player.mount.Type == ModContent.MountType<AngryDogMount>() && Math.Abs(Player.velocity.X) > Player.mount.RunSpeed / 2f)
            {
                Rectangle rect2 = Player.getRect();

                if (Player.direction == 1)
                    rect2.Offset(Player.width - 1, 0);

                rect2.Width = 2;
                rect2.Inflate(6, 12);
                float damage2 = 50f * Player.MinionDamage();
                float knockback2 = 8f;
                int nPCImmuneTime2 = 30;
                int playerImmuneTime2 = 6;
                ModCollideWithNPCs(rect2, damage2, knockback2, nPCImmuneTime2, playerImmuneTime2);
            }

            if (Player.mount.Active && Player.mount.Type == ModContent.MountType<OnyxExcavator>() && Math.Abs(Player.velocity.X) > Player.mount.RunSpeed / 2f)
            {
                Rectangle rect2 = Player.getRect();

                if (Player.direction == 1)
                    rect2.Offset(Player.width - 1, 0);

                rect2.Width = 2;
                rect2.Inflate(6, 12);
                float damage2 = 25f * Player.MinionDamage();
                float knockback2 = 5f;
                int nPCImmuneTime2 = 30;
                int playerImmuneTime2 = 6;
                ModCollideWithNPCs(rect2, damage2, knockback2, nPCImmuneTime2, playerImmuneTime2);
            }
        }

        private int ModCollideWithNPCs(Rectangle myRect, float Damage, float Knockback, int NPCImmuneTime, int PlayerImmuneTime)
        {
            int num = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC nPC = Main.npc[i];
                if (nPC.active && !nPC.dontTakeDamage && !nPC.friendly && nPC.immune[Player.whoAmI] == 0)
                {
                    Rectangle rect = nPC.getRect();
                    if (myRect.Intersects(rect) && (nPC.noTileCollide || Collision.CanHit(Player.position, Player.width, Player.height, nPC.position, nPC.width, nPC.height)))
                    {
                        int direction = Player.direction;
                        if (Player.velocity.X < 0f)
                        {
                            direction = -1;
                        }
                        if (Player.velocity.X > 0f)
                        {
                            direction = 1;
                        }
                        if (Player.whoAmI == Main.myPlayer)
                        {
                            Player.ApplyDamageToNPC(nPC, (int)Damage, Knockback, direction, false);
                        }
                        nPC.immune[Player.whoAmI] = NPCImmuneTime;
                        Player.immune = true;
                        Player.immuneNoBlink = true;
                        Player.immuneTime = PlayerImmuneTime;
                        num++;
                        break;
                    }
                }
            }
            return num;
        }
        #endregion

        #region Nurse Modifications
        public override bool ModifyNurseHeal(NPC nurse, ref int health, ref bool removeDebuffs, ref string chatText)
        {
            if (CalamityWorld.death && areThereAnyDamnBosses)
            {
                chatText = "Now is not the time!";
                return false;
            }

            return true;
        }

        public override void ModifyNursePrice(NPC nurse, int health, bool removeDebuffs, ref int price)
        {
            // In Rev+, nurse costs scale as the game progresses.
            // Base:            300     3 silver
            // EoC:             900     9 silver
            // Skeletron:       1200    12 silver
            // Hardmode:        2400    24 silver
            // Any Mech Boss:   4000    40 silver
            // Plantera/Cal:    6000    60 silver
            // Golem:           9000    90 silver
            // Fish/PBG/Rav:    12000   1 gold 20 silver
            // Moon Lord:       20000   2 gold
            // Providence:      32000   3 gold 20 silver
            // DoG:             60000   6 gold
            // Yharon:          90000   9 gold

            if (CalamityWorld.revenge && price > 0)
            {
                // start with a vanilla cost of zero instead of 3 silver
                price -= Item.buyPrice(0, 0, 3, 0);

                if (CalamityWorld.downedYharon)
                    price += Item.buyPrice(0, 9, 0, 0);
                else if (CalamityWorld.downedDoG)
                    price += Item.buyPrice(0, 6, 0, 0);
                else if (CalamityWorld.downedProvidence)
                    price += Item.buyPrice(0, 3, 20, 0);
                else if (NPC.downedMoonlord)
                    price += Item.buyPrice(0, 2, 0, 0);
                else if (NPC.downedFishron || CalamityWorld.downedPlaguebringer || CalamityWorld.downedScavenger)
                    price += Item.buyPrice(0, 1, 20, 0);
                else if (NPC.downedGolemBoss)
                    price += Item.buyPrice(0, 0, 90, 0);
                else if (NPC.downedPlantBoss || CalamityWorld.downedCalamitas)
                    price += Item.buyPrice(0, 0, 60, 0);
                else if (NPC.downedMechBossAny)
                    price += Item.buyPrice(0, 0, 40, 0);
                else if (Main.hardMode)
                    price += Item.buyPrice(0, 0, 24, 0);
                else if (NPC.downedBoss3)
                    price += Item.buyPrice(0, 0, 12, 0);
                else if (NPC.downedBoss1)
                    price += Item.buyPrice(0, 0, 6, 0);
                else
                    price += Item.buyPrice(0, 0, 3, 0);

                if (areThereAnyDamnBosses)
                    price *= 5;
            }
        }
        #endregion

        #region All-Class Crit Boost
        public void AllCritBoost(int boost)
        {
            Player.GetCritChance(DamageClass.Melee) += boost;
            Player.GetCritChance(DamageClass.Ranged) += boost;
            Player.GetCritChance(DamageClass.Magic) += boost;
            Player.GetCritChance(DamageClass.Throwing) += boost;
            // Rogue weapons benefit from throwing crit AND rogue crit, so don't add both.
            // throwingCrit += boost;
        }
        #endregion

        #region Rogue Stealth
        private void ResetRogueStealth()
        {
            // rogueStealth doesn't reset every frame because it's a continuously building resource

            // these other parameters are rebuilt every frame based on the items you have equipped
            rogueStealthMax = 0f;
            stealthGenStandstill = 1f;
            stealthGenMoving = 1f;
            stealthStrikeThisFrame = false;
            stealthStrikeHalfCost = false;
            stealthStrike75Cost = false;
            stealthStrikeAlwaysCrits = false;

            // stealthAcceleration only resets if you don't have either of the accelerator accessories equipped
            if (!darkGodSheath && !eclipseMirror)
                stealthAcceleration = 1f;
        }

        public void UpdateRogueStealth()
        {
            // If the player un-equips rogue armor, then reset the sound so it'll play again when they re-equip it
            if (!wearingRogueArmor)
            {
                rogueStealth = 0f;
                playRogueStealthSound = false;
                return;
            }

            // Sound plays upon hitting full stealth, not upon having stealth strike available (this can occur at lower than 100% stealth)
            if (playRogueStealthSound && rogueStealth >= rogueStealthMax)
            {
                playRogueStealthSound = false;
                SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/RogueStealth"), Player.position);
            }

            // If the player isn't at full stealth, reset the sound so it'll play again when they hit full stealth.
            else if (rogueStealth < rogueStealthMax)
                playRogueStealthSound = true;

            // Calculate stealth generation and gain stealth accordingly
            if (wearingRogueArmor)
            {
                // 1f is normal speed, anything higher is faster. Default stealth generation is 3 seconds while standing still.
                float currentStealthGen = UpdateStealthGenStats();
                rogueStealth += rogueStealthMax * (currentStealthGen / 180f); // 180 frames = 3 seconds
                if (rogueStealth > rogueStealthMax)
                    rogueStealth = rogueStealthMax;
            }

            ProvideStealthStatBonuses();

            // If the player is using an item that deals damage and is on their first frame of doing so,
            // consume stealth if a stealth strike wasn't triggered manually by item code.

            // This doesn't trigger stealth strike effects (ConsumeStealthStrike instead of StealthStrike)
            // so non-rogue weapons can't call lasers down from the sky and such.
            // Using any item which deals no damage or is a tool doesn't consume stealth.
            Item it = Player.ActiveItem();
            bool hasDamage = it.damage > 0;
            bool hasHitboxes = !it.noMelee || it.shoot > ProjectileID.None;
            bool isPickaxe = it.pick > 0;
            bool isAxe = it.axe > 0;
            bool isHammer = it.hammer > 0;
            bool isPlaced = it.createTile != -1;
            bool isChannelable = it.channel;
            bool hasNonWeaponFunction = isPickaxe || isAxe || isHammer || isPlaced || isChannelable;
            bool playerUsingWeapon = hasDamage && hasHitboxes && !hasNonWeaponFunction;
            if (!stealthStrikeThisFrame && Player.itemAnimation == Player.itemAnimationMax - 1 && playerUsingWeapon)
                ConsumeStealthByAttacking();
        }

        private void ProvideStealthStatBonuses()
        {
            // At full stealth, you get a higher damage bonus than at any partial level of stealth.
            if (rogueStealth >= rogueStealthMax)
                throwingDamage += rogueStealth * 0.6666666f;
            else
                throwingDamage += rogueStealth * 0.5f;

            // Crit increases based on your stealth value. With certain gear, it's locked at 100% for stealth strikes.
            if (stealthStrikeAlwaysCrits && StealthStrikeAvailable())
                throwingCrit = 100;
            else
                throwingCrit += (int)(rogueStealth * 20f);

            // Stealth slightly increases movement speed and decreases aggro.
            if (wearingRogueArmor && rogueStealthMax > 0)
            {
                Player.moveSpeed += rogueStealth * 0.05f;
                Player.aggro -= (int)(rogueStealth * 400f);
            }
        }

        private float UpdateStealthGenStats()
        {
			int finalDawnProjCount = Player.ownedProjectileCounts[ModContent.ProjectileType<FinalDawnProjectile>()] +
			Player.ownedProjectileCounts[ModContent.ProjectileType<FinalDawnFireSlash>()] +
			Player.ownedProjectileCounts[ModContent.ProjectileType<FinalDawnHorizontalSlash>()] +
			Player.ownedProjectileCounts[ModContent.ProjectileType<FinalDawnThrow>()] +
			Player.ownedProjectileCounts[ModContent.ProjectileType<FinalDawnThrow2>()];

            // If you are actively using an item, you cannot gain stealth.
            if (Player.itemAnimation > 0 || finalDawnProjCount > 0)
                return 0f;

            // Penumbra Potion provides various boosts to rogue stealth generation
            if (penumbra)
            {
                if (Main.eclipse || umbraphileSet)
                {
                    stealthGenStandstill += 0.2f;
                    stealthGenMoving += 0.2f;
                }
                else if (!Main.dayTime)
                {
                    stealthGenStandstill += 0.15f;
                    stealthGenMoving += 0.15f;
                }
                else // daytime
                    stealthGenMoving += 0.15f;
            }

            if (CalamityLists.daggerList.Contains(Player.ActiveItem().type) && Player.invis)
            {
                stealthGenStandstill += 0.08f;
                stealthGenMoving += 0.08f;
            }
			if (shadow)
            {
                stealthGenStandstill += 0.1f;
                stealthGenMoving += 0.1f;
            }

            if (etherealExtorter && Main.moonPhase == 3) // 3 = Waning Crescent
                stealthGenStandstill += 0.15f;

			//Accessory modifiers can boost these stats
			stealthGenStandstill += accStealthGenBoost;
			stealthGenMoving += accStealthGenBoost;

            //
            // Other code which affects stealth generation goes here.
            // Increase stealthGenStandstill (default 1.0) to give a % boost to stealth gen while standing still.
            // Increase stealthGenMoving (default 1.0) to give a % boost to stealth gen while moving.
            //

            // Update Dark God's Sheath and Eclipse Mirror's stealth acceleration
            /*
             * T = frame counter
             * DGS  = (100% + 1% * T)
             * EM   = (100% + 1% * T) * 1.0084^T
             * BOTH = (100% + 1.5% * T) * 1.0084^T
             * 
             * DGS alone caps in 100 frames
             * EM alone caps in 41 frames
             * Both together caps in 32 frames
             */
            if (darkGodSheath && eclipseMirror)
            {
                stealthAcceleration += 0.015f;
                stealthAcceleration *= 1.0084f;
            }
            else if (eclipseMirror)
            {
                stealthAcceleration += 0.01f;
                stealthAcceleration *= 1.0084f;
            }
            else if (darkGodSheath)
                stealthAcceleration += 0.01f;
            stealthAcceleration = MathHelper.Clamp(stealthAcceleration, 1f, StealthAccelerationCap);

            // You get 100% stealth regen while standing still and not on a mount. Otherwise, you get your stealth regeneration while moving.
            // Stealth only regenerates at 1/3 speed while moving.
            bool standstill = Player.StandingStill(0.1f) && !Player.mount.Active;
            return standstill ? stealthGenStandstill : stealthGenMoving * 0.333333f * stealthAcceleration;
        }

        public bool StealthStrikeAvailable()
        {
            if (rogueStealthMax <= 0f)
                return false;
			float consumptionMult = 1f;
			if (stealthStrikeHalfCost)
				consumptionMult = 0.5f;
			else if (stealthStrike75Cost)
				consumptionMult = 0.75f;
            return rogueStealth >= rogueStealthMax * consumptionMult;
        }

        internal void ConsumeStealthByAttacking()
        {
            stealthStrikeThisFrame = true;
            stealthAcceleration = 1f; // Reset acceleration when you attack

            if (stealthStrikeHalfCost)
            {
                rogueStealth -= 0.5f * rogueStealthMax;
                if (rogueStealth <= 0f)
                    rogueStealth = 0f;
            }
            else if (stealthStrike75Cost)
			{
                rogueStealth -= 0.75f * rogueStealthMax;
                if (rogueStealth <= 0f)
                    rogueStealth = 0f;
            }
			else
                rogueStealth = 0f;
        }
        #endregion

        #region Packet Stuff
        private void ExactLevelPacket(bool server, int levelType)
        {
            ModPacket packet = Mod.GetPacket(256);
            switch (levelType)
            {
                case 0:
                    packet.Write((byte)CalRDMessageType.ExactMeleeLevelSync);
                    packet.Write(Player.whoAmI);
                    packet.Write(exactMeleeLevel);
                    break;
                case 1:
                    packet.Write((byte)CalRDMessageType.ExactRangedLevelSync);
                    packet.Write(Player.whoAmI);
                    packet.Write(exactRangedLevel);
                    break;
                case 2:
                    packet.Write((byte)CalRDMessageType.ExactMagicLevelSync);
                    packet.Write(Player.whoAmI);
                    packet.Write(exactMagicLevel);
                    break;
                case 3:
                    packet.Write((byte)CalRDMessageType.ExactSummonLevelSync);
                    packet.Write(Player.whoAmI);
                    packet.Write(exactSummonLevel);
                    break;
                case 4:
                    packet.Write((byte)CalRDMessageType.ExactRogueLevelSync);
                    packet.Write(Player.whoAmI);
                    packet.Write(exactRogueLevel);
                    break;
            }

            if (!server)
                packet.Send();
            else
                packet.Send(-1, Player.whoAmI);
        }

        private void LevelPacket(bool server, int levelType)
        {
            ModPacket packet = Mod.GetPacket(256);
            switch (levelType)
            {
                case 0:
                    packet.Write((byte)CalRDMessageType.MeleeLevelSync);
                    packet.Write(Player.whoAmI);
                    packet.Write(meleeLevel);
                    break;
                case 1:
                    packet.Write((byte)CalRDMessageType.RangedLevelSync);
                    packet.Write(Player.whoAmI);
                    packet.Write(rangedLevel);
                    break;
                case 2:
                    packet.Write((byte)CalRDMessageType.MagicLevelSync);
                    packet.Write(Player.whoAmI);
                    packet.Write(magicLevel);
                    break;
                case 3:
                    packet.Write((byte)CalRDMessageType.SummonLevelSync);
                    packet.Write(Player.whoAmI);
                    packet.Write(summonLevel);
                    break;
                case 4:
                    packet.Write((byte)CalRDMessageType.RogueLevelSync);
                    packet.Write(Player.whoAmI);
                    packet.Write(rogueLevel);
                    break;
            }

            if (!server)
                packet.Send();
            else
                packet.Send(-1, Player.whoAmI);
        }

        public void StressPacket(bool server)
        {
            ModPacket packet = Mod.GetPacket(256);
            packet.Write((byte)CalRDMessageType.StressSync);
            packet.Write(Player.whoAmI);
            packet.Write(rage);

            if (!server)
                packet.Send();
            else
                packet.Send(-1, Player.whoAmI);
        }

        public void AdrenalinePacket(bool server)
        {
            ModPacket packet = Mod.GetPacket(256);
            packet.Write((byte)CalRDMessageType.AdrenalineSync);
            packet.Write(Player.whoAmI);
            packet.Write(adrenaline);

            if (!server)
                packet.Send();
            else
                packet.Send(-1, Player.whoAmI);
        }

        private void DeathPacket(bool server)
        {
            ModPacket packet = Mod.GetPacket(256);
            packet.Write((byte)CalRDMessageType.DeathCountSync);
            packet.Write(Player.whoAmI);
            packet.Write(deathCount);

            if (!server)
                packet.Send();
            else
                packet.Send(-1, Player.whoAmI);
        }

        public void DeathModeUnderworldTimePacket(bool server)
        {
            ModPacket packet = Mod.GetPacket(256);
            packet.Write((byte)CalRDMessageType.DeathModeUnderworldTimeSync);
            packet.Write(Player.whoAmI);
            packet.Write(deathModeUnderworldTime);

            if (!server)
                packet.Send();
            else
                packet.Send(-1, Player.whoAmI);
        }

        public void DeathModeBlizzardTimePacket(bool server)
        {
            ModPacket packet = Mod.GetPacket(256);
            packet.Write((byte)CalRDMessageType.DeathModeBlizzardTimeSync);
            packet.Write(Player.whoAmI);
            packet.Write(deathModeBlizzardTime);

            if (!server)
                packet.Send();
            else
                packet.Send(-1, Player.whoAmI);
        }

        internal void HandleExactLevels(BinaryReader reader, int levelType)
        {
            switch (levelType)
            {
                case 0:
                    exactMeleeLevel = reader.ReadInt32();
                    break;
                case 1:
                    exactRangedLevel = reader.ReadInt32();
                    break;
                case 2:
                    exactMagicLevel = reader.ReadInt32();
                    break;
                case 3:
                    exactSummonLevel = reader.ReadInt32();
                    break;
                case 4:
                    exactRogueLevel = reader.ReadInt32();
                    break;
            }

            if (Main.netMode == NetmodeID.Server)
                ExactLevelPacket(true, levelType);
        }

        internal void HandleLevels(BinaryReader reader, int levelType)
        {
            switch (levelType)
            {
                case 0:
                    meleeLevel = reader.ReadInt32();
                    break;
                case 1:
                    rangedLevel = reader.ReadInt32();
                    break;
                case 2:
                    magicLevel = reader.ReadInt32();
                    break;
                case 3:
                    summonLevel = reader.ReadInt32();
                    break;
                case 4:
                    rogueLevel = reader.ReadInt32();
                    break;
            }

            if (Main.netMode == NetmodeID.Server)
                LevelPacket(true, levelType);
        }

        internal void HandleStress(BinaryReader reader)
        {
            rage = reader.ReadInt32();
            if (Main.netMode == NetmodeID.Server)
                StressPacket(true);
        }

        internal void HandleAdrenaline(BinaryReader reader)
        {
            adrenaline = reader.ReadInt32();
            if (Main.netMode == NetmodeID.Server)
                AdrenalinePacket(true);
        }

        internal void HandleDeathCount(BinaryReader reader)
        {
            deathCount = reader.ReadInt32();
            if (Main.netMode == NetmodeID.Server)
                DeathPacket(true);
        }

        internal void HandleDeathModeUnderworldTime(BinaryReader reader)
        {
            deathModeUnderworldTime = reader.ReadInt32();
            if (Main.netMode == NetmodeID.Server)
                DeathModeUnderworldTimePacket(true);
        }

        internal void HandleDeathModeBlizzardTime(BinaryReader reader)
        {
            deathModeBlizzardTime = reader.ReadInt32();
            if (Main.netMode == NetmodeID.Server)
                DeathModeBlizzardTimePacket(true);
        }

        public override void OnEnterWorld()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ExactLevelPacket(false, 0);
                ExactLevelPacket(false, 1);
                ExactLevelPacket(false, 2);
                ExactLevelPacket(false, 3);
                ExactLevelPacket(false, 4);
                LevelPacket(false, 0);
                LevelPacket(false, 1);
                LevelPacket(false, 2);
                LevelPacket(false, 3);
                LevelPacket(false, 4);
                StressPacket(false);
                AdrenalinePacket(false);
                DeathPacket(false);
                DeathModeUnderworldTimePacket(false);
                DeathModeBlizzardTimePacket(false);
            }
        }
        #endregion

        #region Proficiency Stuff
		private bool ReduceCooldown(int classType)
		{
			switch (classType)
			{
				case (int)ClassType.Melee:

					if (meleeLevel < levelTier3 && (rangedLevel >= levelTier3 || magicLevel >= levelTier3 || summonLevel >= levelTier3 || rogueLevel >= levelTier3))
						return true;
					if (meleeLevel < levelTier2 && (rangedLevel >= levelTier2 || magicLevel >= levelTier2 || summonLevel >= levelTier2 || rogueLevel >= levelTier2))
						return true;
					if (meleeLevel < levelTier1 && (rangedLevel >= levelTier1 || magicLevel >= levelTier1 || summonLevel >= levelTier1 || rogueLevel >= levelTier1))
						return true;

					break;

				case (int)ClassType.Ranged:

					if (rangedLevel < levelTier3 && (meleeLevel >= levelTier3 || magicLevel >= levelTier3 || summonLevel >= levelTier3 || rogueLevel >= levelTier3))
						return true;
					if (rangedLevel < levelTier2 && (meleeLevel >= levelTier2 || magicLevel >= levelTier2 || summonLevel >= levelTier2 || rogueLevel >= levelTier2))
						return true;
					if (rangedLevel < levelTier1 && (meleeLevel >= levelTier1 || magicLevel >= levelTier1 || summonLevel >= levelTier1 || rogueLevel >= levelTier1))
						return true;

					break;

				case (int)ClassType.Magic:

					if (magicLevel < levelTier3 && (rangedLevel >= levelTier3 || meleeLevel >= levelTier3 || summonLevel >= levelTier3 || rogueLevel >= levelTier3))
						return true;
					if (magicLevel < levelTier2 && (rangedLevel >= levelTier2 || meleeLevel >= levelTier2 || summonLevel >= levelTier2 || rogueLevel >= levelTier2))
						return true;
					if (magicLevel < levelTier1 && (rangedLevel >= levelTier1 || meleeLevel >= levelTier1 || summonLevel >= levelTier1 || rogueLevel >= levelTier1))
						return true;

					break;

				case (int)ClassType.Summon:

					if (summonLevel < levelTier3 && (rangedLevel >= levelTier3 || magicLevel >= levelTier3 || meleeLevel >= levelTier3 || rogueLevel >= levelTier3))
						return true;
					if (summonLevel < levelTier2 && (rangedLevel >= levelTier2 || magicLevel >= levelTier2 || meleeLevel >= levelTier2 || rogueLevel >= levelTier2))
						return true;
					if (summonLevel < levelTier1 && (rangedLevel >= levelTier1 || magicLevel >= levelTier1 || meleeLevel >= levelTier1 || rogueLevel >= levelTier1))
						return true;

					break;

				case (int)ClassType.Rogue:

					if (rogueLevel < levelTier3 && (rangedLevel >= levelTier3 || magicLevel >= levelTier3 || summonLevel >= levelTier3 || meleeLevel >= levelTier3))
						return true;
					if (rogueLevel < levelTier2 && (rangedLevel >= levelTier2 || magicLevel >= levelTier2 || summonLevel >= levelTier2 || meleeLevel >= levelTier2))
						return true;
					if (rogueLevel < levelTier1 && (rangedLevel >= levelTier1 || magicLevel >= levelTier1 || summonLevel >= levelTier1 || meleeLevel >= levelTier1))
						return true;

					break;
			}
			return false;
		}

        public void GetExactLevelUp()
        {
            if (gainLevelCooldown > 0)
                gainLevelCooldown--;

            #region MeleeLevels
            switch (meleeLevel)
            {
                case 100:
                    ExactLevelUp(0, 1, false);
                    break;
                case 300:
                    ExactLevelUp(0, 2, false);
                    break;
                case 600:
                    ExactLevelUp(0, 3, false);
                    break;
                case 1000:
                    ExactLevelUp(0, 4, false);
                    break;
                case 1500:
                    ExactLevelUp(0, 5, false);
                    break;
                case 2100:
                    ExactLevelUp(0, 6, false);
                    break;
                case 2800:
                    ExactLevelUp(0, 7, false);
                    break;
                case 3600:
                    ExactLevelUp(0, 8, false);
                    break;
                case 4500:
                    ExactLevelUp(0, 9, false);
                    break;
                case 5500:
                    ExactLevelUp(0, 10, false);
                    break;
                case 6600:
                    ExactLevelUp(0, 11, false);
                    break;
                case 7800:
                    ExactLevelUp(0, 12, false);
                    break;
                case 9100:
                    ExactLevelUp(0, 13, false);
                    break;
                case 10500:
                    ExactLevelUp(0, 14, false);
                    break;
                case 12500: //celebration or some shit for final level, yay
                    ExactLevelUp(0, 15, true);
                    break;
                default:
                    break;
            }
            #endregion

            #region RangedLevels
            switch (rangedLevel)
            {
                case 100:
                    ExactLevelUp(1, 1, false);
                    break;
                case 300:
                    ExactLevelUp(1, 2, false);
                    break;
                case 600:
                    ExactLevelUp(1, 3, false);
                    break;
                case 1000:
                    ExactLevelUp(1, 4, false);
                    break;
                case 1500:
                    ExactLevelUp(1, 5, false);
                    break;
                case 2100:
                    ExactLevelUp(1, 6, false);
                    break;
                case 2800:
                    ExactLevelUp(1, 7, false);
                    break;
                case 3600:
                    ExactLevelUp(1, 8, false);
                    break;
                case 4500:
                    ExactLevelUp(1, 9, false);
                    break;
                case 5500:
                    ExactLevelUp(1, 10, false);
                    break;
                case 6600:
                    ExactLevelUp(1, 11, false);
                    break;
                case 7800:
                    ExactLevelUp(1, 12, false);
                    break;
                case 9100:
                    ExactLevelUp(1, 13, false);
                    break;
                case 10500:
                    ExactLevelUp(1, 14, false);
                    break;
                case 12500: //celebration or some shit for final level, yay
                    ExactLevelUp(1, 15, true);
                    break;
                default:
                    break;
            }
            #endregion

            #region MagicLevels
            switch (magicLevel)
            {
                case 100:
                    ExactLevelUp(2, 1, false);
                    break;
                case 300:
                    ExactLevelUp(2, 2, false);
                    break;
                case 600:
                    ExactLevelUp(2, 3, false);
                    break;
                case 1000:
                    ExactLevelUp(2, 4, false);
                    break;
                case 1500:
                    ExactLevelUp(2, 5, false);
                    break;
                case 2100:
                    ExactLevelUp(2, 6, false);
                    break;
                case 2800:
                    ExactLevelUp(2, 7, false);
                    break;
                case 3600:
                    ExactLevelUp(2, 8, false);
                    break;
                case 4500:
                    ExactLevelUp(2, 9, false);
                    break;
                case 5500:
                    ExactLevelUp(2, 10, false);
                    break;
                case 6600:
                    ExactLevelUp(2, 11, false);
                    break;
                case 7800:
                    ExactLevelUp(2, 12, false);
                    break;
                case 9100:
                    ExactLevelUp(2, 13, false);
                    break;
                case 10500:
                    ExactLevelUp(2, 14, false);
                    break;
                case 12500: //celebration or some shit for final level, yay
                    ExactLevelUp(2, 15, true);
                    break;
                default:
                    break;
            }
            #endregion

            #region SummonLevels
            switch (summonLevel)
            {
                case 100:
                    ExactLevelUp(3, 1, false);
                    break;
                case 300:
                    ExactLevelUp(3, 2, false);
                    break;
                case 600:
                    ExactLevelUp(3, 3, false);
                    break;
                case 1000:
                    ExactLevelUp(3, 4, false);
                    break;
                case 1500:
                    ExactLevelUp(3, 5, false);
                    break;
                case 2100:
                    ExactLevelUp(3, 6, false);
                    break;
                case 2800:
                    ExactLevelUp(3, 7, false);
                    break;
                case 3600:
                    ExactLevelUp(3, 8, false);
                    break;
                case 4500:
                    ExactLevelUp(3, 9, false);
                    break;
                case 5500:
                    ExactLevelUp(3, 10, false);
                    break;
                case 6600:
                    ExactLevelUp(3, 11, false);
                    break;
                case 7800:
                    ExactLevelUp(3, 12, false);
                    break;
                case 9100:
                    ExactLevelUp(3, 13, false);
                    break;
                case 10500:
                    ExactLevelUp(3, 14, false);
                    break;
                case 12500: //celebration or some shit for final level, yay
                    ExactLevelUp(3, 15, true);
                    break;
                default:
                    break;
            }
            #endregion

            #region RogueLevels
            switch (rogueLevel)
            {
                case 100:
                    ExactLevelUp(4, 1, false);
                    break;
                case 300:
                    ExactLevelUp(4, 2, false);
                    break;
                case 600:
                    ExactLevelUp(4, 3, false);
                    break;
                case 1000:
                    ExactLevelUp(4, 4, false);
                    break;
                case 1500:
                    ExactLevelUp(4, 5, false);
                    break;
                case 2100:
                    ExactLevelUp(4, 6, false);
                    break;
                case 2800:
                    ExactLevelUp(4, 7, false);
                    break;
                case 3600:
                    ExactLevelUp(4, 8, false);
                    break;
                case 4500:
                    ExactLevelUp(4, 9, false);
                    break;
                case 5500:
                    ExactLevelUp(4, 10, false);
                    break;
                case 6600:
                    ExactLevelUp(4, 11, false);
                    break;
                case 7800:
                    ExactLevelUp(4, 12, false);
                    break;
                case 9100:
                    ExactLevelUp(4, 13, false);
                    break;
                case 10500:
                    ExactLevelUp(4, 14, false);
                    break;
                case 12500: //celebration or some shit for final level, yay
                    ExactLevelUp(4, 15, true);
                    break;
                default:
                    break;
            }
            #endregion
        }

        private void ExactLevelUp(int levelUpType, int level, bool final)
        {
            Color messageColor = Color.Orange;
            switch (levelUpType)
            {
                case 0:
                    exactMeleeLevel = level;
                    if (shootFireworksLevelUpMelee)
                    {
                        string key = final ? "Melee weapon proficiency maxed out!" : "Melee weapon proficiency level up!";
                        shootFireworksLevelUpMelee = false;
                        if (Player.whoAmI == Main.myPlayer)
                        {
                            if (final)
                            {
                                int prof = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, -5f, ProjectileID.RocketFireworkRed + Main.rand.Next(4),
                                    0, 0f, Main.myPlayer, 0f, 1f);
                            }
                            else
                            {
                                int prof = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, -5f, ProjectileID.RocketFireworksBoxRed + Main.rand.Next(4),
                                    0, 0f, Main.myPlayer, 0f, 0f);
                            }
                            Main.NewText(Language.GetTextValue(key), messageColor);
                        }
                    }
                    break;
                case 1:
                    exactRangedLevel = level;
                    if (shootFireworksLevelUpRanged)
                    {
                        string key = final ? "Ranged weapon proficiency maxed out!" : "Ranged weapon proficiency level up!";
                        messageColor = Color.GreenYellow;
                        shootFireworksLevelUpRanged = false;
                        if (Player.whoAmI == Main.myPlayer)
                        {
                            if (final)
                            {
                                int prof = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, -5f, ProjectileID.RocketFireworkRed + Main.rand.Next(4),
                                    0, 0f, Main.myPlayer, 0f, 1f);
                            }
                            else
                            {
                                int prof = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, -5f, ProjectileID.RocketFireworksBoxRed + Main.rand.Next(4),
                                    0, 0f, Main.myPlayer, 0f, 0f);
                            }
                            Main.NewText(Language.GetTextValue(key), messageColor);
                        }
                    }
                    break;
                case 2:
                    exactMagicLevel = level;
                    if (shootFireworksLevelUpMagic)
                    {
                        string key = final ? "Magic weapon proficiency maxed out!" : "Magic weapon proficiency level up!";
                        messageColor = Color.DodgerBlue;
                        shootFireworksLevelUpMagic = false;
                        if (Player.whoAmI == Main.myPlayer)
                        {
                            if (final)
                            {
                                int prof = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, -5f, ProjectileID.RocketFireworkRed + Main.rand.Next(4),
                                    0, 0f, Main.myPlayer, 0f, 1f);
                            }
                            else
                            {
                                int prof = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, -5f, ProjectileID.RocketFireworksBoxRed + Main.rand.Next(4),
                                    0, 0f, Main.myPlayer, 0f, 0f);
                            }
                            Main.NewText(Language.GetTextValue(key), messageColor);
                        }
                    }
                    break;
                case 3:
                    exactSummonLevel = level;
                    if (shootFireworksLevelUpSummon)
                    {
                        string key = final ? "Summoner weapon proficiency maxed out!" : "Summoner weapon proficiency level up!";
                        messageColor = Color.Aquamarine;
                        shootFireworksLevelUpSummon = false;
                        if (Player.whoAmI == Main.myPlayer)
                        {
                            if (final)
                            {
                                int prof = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, -5f, ProjectileID.RocketFireworkRed + Main.rand.Next(4),
                                    0, 0f, Main.myPlayer, 0f, 1f);
                            }
                            else
                            {
                                int prof = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, -5f, ProjectileID.RocketFireworksBoxRed + Main.rand.Next(4),
                                    0, 0f, Main.myPlayer, 0f, 0f);
                            }
                            Main.NewText(Language.GetTextValue(key), messageColor);
                        }
                    }
                    break;
                case 4:
                    exactRogueLevel = level;
                    if (shootFireworksLevelUpRogue)
                    {
                        string key = final ? "Rogue weapon proficiency maxed out!" : "Rogue weapon proficiency level up!";
                        messageColor = Color.Orchid;
                        shootFireworksLevelUpRogue = false;
                        if (Player.whoAmI == Main.myPlayer)
                        {
                            if (final)
                            {
                                int prof = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, -5f, ProjectileID.RocketFireworkRed + Main.rand.Next(4),
                                    0, 0f, Main.myPlayer, 0f, 1f);
                            }
                            else
                            {
                                int prof = Projectile.NewProjectile(Entity.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0f, -5f, ProjectileID.RocketFireworksBoxRed + Main.rand.Next(4),
                                    0, 0f, Main.myPlayer, 0f, 0f);
                            }
                            Main.NewText(Language.GetTextValue(key), messageColor);
                        }
                    }
                    break;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ExactLevelPacket(false, levelUpType);
            }
        }

        public void GetStatBonuses()
        {
            #region MeleeLevelBoosts
            if (meleeLevel >= 12500)
            {
                Player.GetDamage(DamageClass.Melee) += 0.12f;
                Player.GetCritChance(DamageClass.Melee) += 6;
            }
            else if (meleeLevel >= 10500)
            {
                Player.GetDamage(DamageClass.Melee) += 0.1f;
                Player.GetCritChance(DamageClass.Melee) += 5;
            }
            else if (meleeLevel >= 9100)
            {
                Player.GetDamage(DamageClass.Melee) += 0.09f;
                Player.GetCritChance(DamageClass.Melee) += 5;
            }
            else if (meleeLevel >= 7800)
            {
                Player.GetDamage(DamageClass.Melee) += 0.08f;
                Player.GetCritChance(DamageClass.Melee) += 4;
            }
            else if (meleeLevel >= 6600)
            {
                Player.GetDamage(DamageClass.Melee) += 0.07f;
                Player.GetCritChance(DamageClass.Melee) += 4;
            }
            else if (meleeLevel >= 5500) //hm limit
            {
                Player.GetDamage(DamageClass.Melee) += 0.06f;
                Player.GetCritChance(DamageClass.Melee) += 3;
            }
            else if (meleeLevel >= 4500)
            {
                Player.GetDamage(DamageClass.Melee) += 0.05f;
                Player.GetCritChance(DamageClass.Melee) += 3;
            }
            else if (meleeLevel >= 3600)
            {
                Player.GetDamage(DamageClass.Melee) += 0.05f;
                Player.GetCritChance(DamageClass.Melee) += 2;
            }
            else if (meleeLevel >= 2800)
            {
                Player.GetDamage(DamageClass.Melee) += 0.04f;
                Player.GetCritChance(DamageClass.Melee) += 2;
            }
            else if (meleeLevel >= 2100)
            {
                Player.GetDamage(DamageClass.Melee) += 0.04f;
                Player.GetCritChance(DamageClass.Melee) += 1;
            }
            else if (meleeLevel >= 1500) //prehm limit
            {
                Player.GetDamage(DamageClass.Melee) += 0.03f;
                Player.GetCritChance(DamageClass.Melee) += 1;
            }
            else if (meleeLevel >= 1000)
            {
                Player.GetDamage(DamageClass.Melee) += 0.03f;
                Player.GetCritChance(DamageClass.Melee) += 1;
            }
            else if (meleeLevel >= 600)
            {
                Player.GetDamage(DamageClass.Melee) += 0.02f;
            }
            else if (meleeLevel >= 300)
                Player.GetDamage(DamageClass.Melee) += 0.02f;
            else if (meleeLevel >= 100)
                Player.GetDamage(DamageClass.Melee) += 0.01f;
            #endregion

            #region RangedLevelBoosts
            if (rangedLevel >= 12500)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.12f;
                Player.moveSpeed += 0.12f;
                Player.GetCritChance(DamageClass.Ranged) += 6;
            }
            else if (rangedLevel >= 10500)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.1f;
                Player.moveSpeed += 0.1f;
                Player.GetCritChance(DamageClass.Ranged) += 5;
            }
            else if (rangedLevel >= 9100)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.09f;
                Player.moveSpeed += 0.09f;
                Player.GetCritChance(DamageClass.Ranged) += 5;
            }
            else if (rangedLevel >= 7800)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.08f;
                Player.moveSpeed += 0.08f;
                Player.GetCritChance(DamageClass.Ranged) += 4;
            }
            else if (rangedLevel >= 6600)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.07f;
                Player.moveSpeed += 0.07f;
                Player.GetCritChance(DamageClass.Ranged) += 4;
            }
            else if (rangedLevel >= 5500)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.06f;
                Player.moveSpeed += 0.06f;
                Player.GetCritChance(DamageClass.Ranged) += 3;
            }
            else if (rangedLevel >= 4500)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.05f;
                Player.moveSpeed += 0.05f;
                Player.GetCritChance(DamageClass.Ranged) += 3;
            }
            else if (rangedLevel >= 3600)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.05f;
                Player.moveSpeed += 0.05f;
                Player.GetCritChance(DamageClass.Ranged) += 2;
            }
            else if (rangedLevel >= 2800)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.04f;
                Player.moveSpeed += 0.04f;
                Player.GetCritChance(DamageClass.Ranged) += 2;
            }
            else if (rangedLevel >= 2100)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.04f;
                Player.moveSpeed += 0.03f;
                Player.GetCritChance(DamageClass.Ranged) += 1;
            }
            else if (rangedLevel >= 1500)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.03f;
                Player.moveSpeed += 0.02f;
                Player.GetCritChance(DamageClass.Ranged) += 1;
            }
            else if (rangedLevel >= 1000)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.03f;
                Player.moveSpeed += 0.01f;
                Player.GetCritChance(DamageClass.Ranged) += 1;
            }
            else if (rangedLevel >= 600)
            {
                Player.GetDamage(DamageClass.Ranged) += 0.02f;
                Player.GetCritChance(DamageClass.Ranged) += 1;
            }
            else if (rangedLevel >= 300)
                Player.GetDamage(DamageClass.Ranged) += 0.02f;
            else if (rangedLevel >= 100)
                Player.GetDamage(DamageClass.Ranged) += 0.01f;
            #endregion

            #region MagicLevelBoosts
            if (magicLevel >= 12500)
            {
                Player.GetDamage(DamageClass.Magic) += 0.12f;
                Player.manaCost *= 0.88f;
                Player.GetCritChance(DamageClass.Magic) += 6;
            }
            else if (magicLevel >= 10500)
            {
                Player.GetDamage(DamageClass.Magic) += 0.1f;
                Player.manaCost *= 0.9f;
                Player.GetCritChance(DamageClass.Magic) += 5;
            }
            else if (magicLevel >= 9100)
            {
                Player.GetDamage(DamageClass.Magic) += 0.09f;
                Player.manaCost *= 0.91f;
                Player.GetCritChance(DamageClass.Magic) += 5;
            }
            else if (magicLevel >= 7800)
            {
                Player.GetDamage(DamageClass.Magic) += 0.08f;
                Player.manaCost *= 0.92f;
                Player.GetCritChance(DamageClass.Magic) += 4;
            }
            else if (magicLevel >= 6600)
            {
                Player.GetDamage(DamageClass.Magic) += 0.07f;
                Player.manaCost *= 0.93f;
                Player.GetCritChance(DamageClass.Magic) += 4;
            }
            else if (magicLevel >= 5500)
            {
                Player.GetDamage(DamageClass.Magic) += 0.06f;
                Player.manaCost *= 0.94f;
                Player.GetCritChance(DamageClass.Magic) += 3;
            }
            else if (magicLevel >= 4500)
            {
                Player.GetDamage(DamageClass.Magic) += 0.05f;
                Player.manaCost *= 0.95f;
                Player.GetCritChance(DamageClass.Magic) += 3;
            }
            else if (magicLevel >= 3600)
            {
                Player.GetDamage(DamageClass.Magic) += 0.05f;
                Player.manaCost *= 0.95f;
                Player.GetCritChance(DamageClass.Magic) += 2;
            }
            else if (magicLevel >= 2800)
            {
                Player.GetDamage(DamageClass.Magic) += 0.04f;
                Player.manaCost *= 0.96f;
                Player.GetCritChance(DamageClass.Magic) += 2;
            }
            else if (magicLevel >= 2100)
            {
                Player.GetDamage(DamageClass.Magic) += 0.04f;
                Player.manaCost *= 0.97f;
                Player.GetCritChance(DamageClass.Magic) += 1;
            }
            else if (magicLevel >= 1500)
            {
                Player.GetDamage(DamageClass.Magic) += 0.03f;
                Player.manaCost *= 0.98f;
                Player.GetCritChance(DamageClass.Magic) += 1;
            }
            else if (magicLevel >= 1000)
            {
                Player.GetDamage(DamageClass.Magic) += 0.03f;
                Player.manaCost *= 0.99f;
                Player.GetCritChance(DamageClass.Magic) += 1;
            }
            else if (magicLevel >= 600)
            {
                Player.GetDamage(DamageClass.Magic) += 0.02f;
                Player.manaCost *= 0.99f;
            }
            else if (magicLevel >= 300)
                Player.GetDamage(DamageClass.Magic) += 0.02f;
            else if (magicLevel >= 100)
                Player.GetDamage(DamageClass.Magic) += 0.01f;
            #endregion

            #region SummonLevelBoosts
            if (summonLevel >= 12500)
            {
                Player.GetDamage(DamageClass.Summon) += 0.12f;
                Player.GetKnockback(DamageClass.Summon).Base += 3.0f;
                Player.maxMinions += 2;
            }
            else if (summonLevel >= 10500)
            {
                Player.GetDamage(DamageClass.Summon) += 0.1f;
                Player.GetKnockback(DamageClass.Summon).Base += 3.0f;
                Player.maxMinions++;
            }
            else if (summonLevel >= 9100)
            {
                Player.GetDamage(DamageClass.Summon) += 0.09f;
                Player.GetKnockback(DamageClass.Summon).Base += 2.7f;
                Player.maxMinions++;
            }
            else if (summonLevel >= 7800)
            {
                Player.GetDamage(DamageClass.Summon) += 0.08f;
                Player.GetKnockback(DamageClass.Summon).Base += 2.4f;
                Player.maxMinions++;
            }
            else if (summonLevel >= 6600)
            {
                Player.GetDamage(DamageClass.Summon) += 0.07f;
                Player.GetKnockback(DamageClass.Summon).Base += 2.1f;
                Player.maxMinions++;
            }
            else if (summonLevel >= 5500)
            {
                Player.GetDamage(DamageClass.Summon) += 0.06f;
                Player.GetKnockback(DamageClass.Summon).Base += 1.8f;
                Player.maxMinions++;
            }
            else if (summonLevel >= 4500)
            {
                Player.GetDamage(DamageClass.Summon) += 0.06f;
                Player.GetKnockback(DamageClass.Summon).Base += 1.8f;
                Player.maxMinions++;
            }
            else if (summonLevel >= 3600)
            {
                Player.GetDamage(DamageClass.Summon) += 0.05f;
                Player.GetKnockback(DamageClass.Summon).Base += 1.5f;
            }
            else if (summonLevel >= 2800)
            {
                Player.GetDamage(DamageClass.Summon) += 0.04f;
                Player.GetKnockback(DamageClass.Summon).Base += 1.2f;
            }
            else if (summonLevel >= 2100)
            {
                Player.GetDamage(DamageClass.Summon) += 0.04f;
                Player.GetKnockback(DamageClass.Summon).Base += 0.9f;
            }
            else if (summonLevel >= 1500)
            {
                Player.GetDamage(DamageClass.Summon) += 0.03f;
                Player.GetKnockback(DamageClass.Summon).Base += 0.6f;
            }
            else if (summonLevel >= 1000)
            {
                Player.GetDamage(DamageClass.Summon) += 0.03f;
                Player.GetKnockback(DamageClass.Summon).Base += 0.3f;
            }
            else if (summonLevel >= 600)
            {
                Player.GetDamage(DamageClass.Summon) += 0.02f;
                Player.GetKnockback(DamageClass.Summon).Base += 0.3f;
            }
            else if (summonLevel >= 300)
                Player.GetDamage(DamageClass.Summon) += 0.02f;
            else if (summonLevel >= 100)
                Player.GetDamage(DamageClass.Summon) += 0.01f;
            #endregion

            #region RogueLevelBoosts
            if (rogueLevel >= 12500)
            {
                throwingDamage += 0.12f;
                throwingVelocity += 0.12f;
                throwingCrit += 6;
            }
            else if (rogueLevel >= 10500)
            {
                throwingDamage += 0.1f;
                throwingVelocity += 0.1f;
                throwingCrit += 5;
            }
            else if (rogueLevel >= 9100)
            {
                throwingDamage += 0.09f;
                throwingVelocity += 0.09f;
                throwingCrit += 5;
            }
            else if (rogueLevel >= 7800)
            {
                throwingDamage += 0.08f;
                throwingVelocity += 0.08f;
                throwingCrit += 4;
            }
            else if (rogueLevel >= 6600)
            {
                throwingDamage += 0.07f;
                throwingVelocity += 0.07f;
                throwingCrit += 4;
            }
            else if (rogueLevel >= 5500)
            {
                throwingDamage += 0.06f;
                throwingVelocity += 0.06f;
                throwingCrit += 3;
            }
            else if (rogueLevel >= 4500)
            {
                throwingDamage += 0.05f;
                throwingVelocity += 0.05f;
                throwingCrit += 3;
            }
            else if (rogueLevel >= 3600)
            {
                throwingDamage += 0.05f;
                throwingVelocity += 0.05f;
                throwingCrit += 2;
            }
            else if (rogueLevel >= 2800)
            {
                throwingDamage += 0.04f;
                throwingVelocity += 0.04f;
                throwingCrit += 2;
            }
            else if (rogueLevel >= 2100)
            {
                throwingDamage += 0.04f;
                throwingVelocity += 0.03f;
                throwingCrit += 1;
            }
            else if (rogueLevel >= 1500)
            {
                throwingDamage += 0.03f;
                throwingVelocity += 0.02f;
                throwingCrit += 1;
            }
            else if (rogueLevel >= 1000)
            {
                throwingDamage += 0.03f;
                throwingVelocity += 0.01f;
                throwingCrit += 1;
            }
            else if (rogueLevel >= 600)
            {
                throwingDamage += 0.02f;
                throwingVelocity += 0.01f;
            }
            else if (rogueLevel >= 300)
                throwingDamage += 0.02f;
            else if (rogueLevel >= 100)
                throwingDamage += 0.01f;
            #endregion
        }

        private float GetMeleeSpeedBonus()
        {
            float meleeSpeedBonus = 0f;
            if (meleeLevel >= 12500)
            {
                meleeSpeedBonus += 0.12f;
            }
            else if (meleeLevel >= 10500)
            {
                meleeSpeedBonus += 0.1f;
            }
            else if (meleeLevel >= 9100)
            {
                meleeSpeedBonus += 0.09f;
            }
            else if (meleeLevel >= 7800)
            {
                meleeSpeedBonus += 0.08f;
            }
            else if (meleeLevel >= 6600)
            {
                meleeSpeedBonus += 0.07f;
            }
            else if (meleeLevel >= 5500) //hm limit
            {
                meleeSpeedBonus += 0.06f;
            }
            else if (meleeLevel >= 4500)
            {
                meleeSpeedBonus += 0.05f;
            }
            else if (meleeLevel >= 3600)
            {
                meleeSpeedBonus += 0.05f;
            }
            else if (meleeLevel >= 2800)
            {
                meleeSpeedBonus += 0.04f;
            }
            else if (meleeLevel >= 2100)
            {
                meleeSpeedBonus += 0.03f;
            }
            else if (meleeLevel >= 1500) //prehm limit
            {
                meleeSpeedBonus += 0.02f;
            }
            else if (meleeLevel >= 1000)
            {
                meleeSpeedBonus += 0.01f;
            }
            else if (meleeLevel >= 600)
            {
                meleeSpeedBonus += 0.01f;
            }
            return meleeSpeedBonus;
        }
        #endregion

        #region Profaned Soul Crystal Stuffs

        internal void rollBabSpears(int randAmt, bool chaseable)
        {
            if (Player.whoAmI == Main.myPlayer && !endoCooper && randAmt > 0 && Main.rand.NextBool(randAmt) && chaseable)
            {
                int spearsFired = 0;
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (spearsFired == 2)
                        break;
                    if (Main.projectile[i].friendly && Main.projectile[i].owner == Player.whoAmI)
                    {
                        bool attack = Main.projectile[i].type == ModContent.ProjectileType<MiniGuardianAttack>() && Main.projectile[i].owner == Player.whoAmI;
                        if (attack || (Main.projectile[i].type == ModContent.ProjectileType<MiniGuardianDefense>() && Main.projectile[i].owner == Player.whoAmI))
                        {
                            int numSpears = attack ? 12 : 6;
                            int dam = Main.projectile[i].damage;
                            if (!attack)
                                dam = (int)(dam * 0.5f);
                            float angleVariance = MathHelper.TwoPi / (float)numSpears;
                            float spinOffsetAngle = MathHelper.Pi / (2f * numSpears);
                            Vector2 posVec = new Vector2(8f, 0f).RotatedByRandom(MathHelper.TwoPi);
                            for (int x = 0; x < numSpears; x++)
                            {
                                posVec = posVec.RotatedBy(angleVariance);
                                Vector2 velocity = new Vector2(posVec.X, posVec.Y).RotatedBy(spinOffsetAngle);
                                velocity.Normalize();
                                velocity *= 8f;
                                Projectile.NewProjectile(Player.GetSource_FromThis(), Main.projectile[i].Center + posVec, velocity, ModContent.ProjectileType<MiniGuardianSpear>(), dam, 0f, Player.whoAmI, 0f, 0f);
                            }
                            spearsFired++;
                        }
                    }
                }
            }
        }

        private bool IsValidTransitionFrame(AnimationType currentAnim, AnimationType newAnim, int frame, int counter) //this exists so it doesn't loop through the entire walk/idle anim just to find one frame for switching.
        {
            bool result = newAnim != AnimationType.Jump && currentAnim != AnimationType.Jump;
            if (currentAnim == AnimationType.Walk && newAnim == AnimationType.Idle)
            {
                result = counter <= 0 && (frame == 11 || frame == 15 || frame == 19);
            }
            else if (currentAnim == AnimationType.Idle && newAnim == AnimationType.Walk)
            {
                result = counter <= 0 && (frame == 2 || frame == 6);
            }
            return currentAnim != newAnim && result; //swapping to jumps should be instant, no need to check the counter here
        }

        private int HandlePSCAnimationFrames(AnimationType newType)
        {
            int key = profanedCrystalAnimCounter.Key; //0-based indexing 
            int value = profanedCrystalAnimCounter.Value - 1;
            AnimationType currentType = key < 8 ? AnimationType.Idle : key == 8 ? AnimationType.Jump : AnimationType.Walk;

            bool isInvalidTransFrame = !IsValidTransitionFrame(currentType, newType, key, value); //to make the transition between walk and idle frames less jarring and smoother
            AnimationType type = isInvalidTransFrame ? newType : currentType;
            int frameCount = type == AnimationType.Walk || (!profanedCrystalForce && Player.statLife <= (int)(Player.statLifeMax2 * 0.5)) ? 7 : 10;
            int lowerRange = type == AnimationType.Idle ? 0 : type == AnimationType.Jump ? 8 : 9;
            int upperRange = type == AnimationType.Idle ? 7 : type == AnimationType.Jump ? 8 : 22;
            if (value <= 0 || !isInvalidTransFrame)
            {
                value = frameCount;
                if (key >= lowerRange && key < upperRange)
                    key++;
                else
                    key = lowerRange;
            }
            profanedCrystalAnimCounter = new KeyValuePair<int, int>(key, value);
            return profanedCrystalAnimCounter.Key;
        }

        public override void PostUpdate() //needs to be here else it doesn't work properly, otherwise i'd have stuck it with the wing anim stuffs
        {
            if ((profanedCrystal || profanedCrystalForce) && !profanedCrystalHide && Player.legs == EquipLoader.GetEquipSlot(Mod, "ProfanedSoulCrystal", EquipType.Legs))
            {
                bool usingCarpet = Player.carpetTime > 0 && Player.controlJump; //doesn't make sense for carpet to use jump frame since you have solid ground
                AnimationType animType = AnimationType.Walk;
                if ((Player.sliding || Player.velocity.Y != 0 || Player.mount.Active || Player.grappling[0] != -1 || Player.GoingDownWithGrapple) && !usingCarpet)
                    animType = AnimationType.Jump;
                else if (Player.velocity.X == 0 || usingCarpet)
                    animType = AnimationType.Idle;
                int frame = HandlePSCAnimationFrames(animType);
                Player.legFrame.Y = Player.legFrame.Height * frame;
            }
            waterLeechTarget = -1;
        }

        #endregion

        #region Misc Stuff

        /// <summary>
        /// Returns the range at which an abyss enemy can detect the player
        /// </summary>
        /// <param name="defaultRange">The default detection range</param>
        /// <param name="anechoicRange">The detection range set by the player having anechoic plating/coating</param>
        /// <returns></returns>
        public float GetAbyssAggro(float defaultRange, float anechoicRange)
        {
            /* ((Main.player[npc.target].GetCalamityPlayer().anechoicPlating ||
                    Main.player[npc.target].GetCalamityPlayer().anechoicCoating) ? 200f : 600f) *
                    (Main.player[npc.target].GetCalamityPlayer().fishAlert ? 3f : 1f) *
                    (Main.player[npc.target].GetCalamityPlayer().abyssalMirror ? 0.7f : 1f) *
                    (Main.player[npc.target].GetCalamityPlayer().eclipseMirror ? 0.3f : 1f) */
            float range = anechoicPlating || anechoicCoating ? anechoicRange : defaultRange;
            range *= fishAlert ? 3f : 1f;
            range *= abyssalMirror ? 0.65f : 1f;
            range *= eclipseMirror ? 0.3f : 1f;
            return range;
        }

        /// <summary>
        /// Calculates and returns the player's total light strength. This is used for Abyss darkness, among other things.<br/>
        /// The Stat Meter also reports this stat.
        /// </summary>
        /// <returns>The player's total light strength.</returns>
        public int GetTotalLightStrength()
        {
            int light = externalAbyssLight;
            bool underwater = Player.IsUnderwater();
            bool miningHelmet = Player.head == ArmorIDs.Head.MiningHelmet;

            // The campfire bonus does not apply while in the Abyss.
            if (!ZoneAbyss && (Player.HasBuff(BuffID.Campfire) || Main.SceneMetrics.HasCampfire))
                light += 1;
            if (camper) //inherits Campfire so really +2
                light += 1;
            if (miningHelmet)
                light += 1;
            if (Player.lightOrb)
                light += 1;
            if (Player.crimsonHeart)
                light += 1;
            if (Player.magicLantern)
                light += 1;
            if (giantPearl)
                light += 1;
            if (radiator)
                light += 1;
            if (bendyPet)
                light += 1;
            if (sparks)
                light += 1;
            if (fathomSwarmerVisage)
                light += 1;
            if (sirenBoobs)
                light += 1;
            if (aAmpoule) // sponge inherits this and doesn't stack with ampoule
                light += 1;
            else if (rOoze && !Main.dayTime) // radiant ooze and ampoule/higher don't stack
                light += 1;
            if (aquaticEmblem && underwater)
                light += 1;
            if (Player.arcticDivingGear && underwater) //inherited by abyssal diving gear/suit, also gives jellyfish necklace so really +2
                light += 1;
            if (jellyfishNecklace && underwater) //inherited by deific amulet+, jellyfish diving gear+
                light += 1;
            if (lumenousAmulet && underwater)
                light += 2;
            if (shine)
                light += 2;
            if (blazingCore)
                light += 2;
            if (Player.redFairy || Player.greenFairy || Player.blueFairy)
                light += 2;
            if (babyGhostBell)
                light += underwater ? 2 : 1;
            if (Player.petFlagDD2Ghost)
                light += 2;
            if (sirenPet)
                light += underwater ? 3 : 1;
            if (Player.wisp)
                light += 3;
            if (Player.suspiciouslookingTentacle)
                light += 3;
            if (profanedCrystalBuffs && !ZoneAbyss)
                light += Main.dayTime || Player.lavaWet ? 2 : 1;
            return light;
        }

        #endregion
    }
}
