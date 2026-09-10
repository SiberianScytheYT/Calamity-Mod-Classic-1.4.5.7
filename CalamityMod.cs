using CalRD.CalPlayer;
using CalRD.Effects;
using CalRD.Events;
using CalRD.ILEditing;
using CalRD.Items.Accessories;
using CalRD.Items.Accessories.Vanity;
using CalRD.Items.Armor;
using CalRD.Items.Dyes.HairDye;
using CalRD.Localization;
using CalRD.NPCs;
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
using CalRD.NPCs.SupremeCalamitas;
using CalRD.NPCs.Yharon;
using CalRD.Projectiles.Summon;
using CalRD.Schematics;
using CalRD.Skies;
using CalRD.TileEntities;
using CalRD.UI;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.Events;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;
using static Terraria.ModLoader.MusicLoader;

namespace CalRD
{
    public class CalRD : Mod
    {
        // CONSIDER -- I have been advised by Jopo that Mods should never contain static variables

        // Hotkeys
        public static ModKeybind NormalityRelocatorHotKey;
        public static ModKeybind AegisHotKey;
        public static ModKeybind TarraHotKey;
        public static ModKeybind RageHotKey;
        public static ModKeybind AdrenalineHotKey;
        public static ModKeybind AstralTeleportHotKey;
        public static ModKeybind AstralArcanumUIHotkey;
        public static ModKeybind MomentumCapacitatorHotkey;
        public static ModKeybind SandCloakHotkey;
        public static ModKeybind SpectralVeilHotKey;
        public static ModKeybind PlaguePackHotKey;

        // Boss Spawners
        public static int ghostKillCount = 0;
        public static int sharkKillCount = 0;

        // Textures
        public static Asset<Texture2D> heartOriginal2;
        public static Asset<Texture2D> heartOriginal;
        public static Asset<Texture2D> rainOriginal;
        public static Asset<Texture2D> manaOriginal;
        public static Asset<Texture2D> carpetOriginal;
        public static Asset<Texture2D> AstralCactusTexture;
        public static Asset<Texture2D> AstralCactusGlowTexture;
        public static Asset<Texture2D> AstralSky;

        // DR data structure
        public static SortedDictionary<int, float> DRValues;

        // Boss Kill Time data structure
        public static SortedDictionary<int, int> bossKillTimes;

        // Boss velocity scaling data structure
        public static SortedDictionary<int, float> bossVelocityDamageScaleValues;
        public const float velocityScaleMin = 0.5f;
        public const float bitingEnemeyVelocityScale = 0.8f;

        // TODO -- Calamity should check for other mods existing in exactly one place
        public bool fargosMutant = false;

        internal static CalRD Instance;

        #region Load
        public override void Load()
        {
            Instance = this;

            // Initialize the BossStats struct as early as it is safe to do so
            NPCStats.Load();

            heartOriginal2 = TextureAssets.Heart;
            heartOriginal = TextureAssets.Heart2;
            rainOriginal = TextureAssets.Rain;
            manaOriginal = TextureAssets.Mana;
            carpetOriginal = TextureAssets.FlyingCarpet;

            NormalityRelocatorHotKey = KeybindLoader.RegisterKeybind(this, "Normality Relocator", "Z");
            RageHotKey = KeybindLoader.RegisterKeybind(this, "Rage Mode", "V");
            AdrenalineHotKey = KeybindLoader.RegisterKeybind(this, "Adrenaline Mode", "B");
            AegisHotKey = KeybindLoader.RegisterKeybind(this, "Elysian Guard", "N");
            TarraHotKey = KeybindLoader.RegisterKeybind(this, "Armor Set Bonus", "Y");
            AstralTeleportHotKey = KeybindLoader.RegisterKeybind(this, "Astral Teleport", "P");
            AstralArcanumUIHotkey = KeybindLoader.RegisterKeybind(this, "Astral Arcanum UI Toggle", "O");
            MomentumCapacitatorHotkey = KeybindLoader.RegisterKeybind(this, "Momentum Capacitor Effect", "U");
            SandCloakHotkey = KeybindLoader.RegisterKeybind(this, "Sand Cloak Effect", "C");
            SpectralVeilHotKey = KeybindLoader.RegisterKeybind(this, "Spectral Veil Teleport", "Z");
            PlaguePackHotKey = KeybindLoader.RegisterKeybind(this, "Booster Dash", "Q");

            if (!Main.dedServ)
            {
                LoadClient();
                LoadMusic();
            }

            ILChanges.Load();
            BossRushEvent.Load();
            
            BossHealthBarManager.Load(this);

            CalamityLists.LoadLists();
            SetupVanillaDR();
            SetupBossKillTimes();
            SetupBossVelocityScalingValues();

            //CalamityLocalization.AddLocalizations();
            SchematicManager.Load();
        }

        private void LoadMusic()
        {
            //Boss Music - Alphabetised
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/AquaticScourge"), ModContent.ItemType<Items.Placeables.MusicBoxes.AquaticScourgeMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.AquaticScourgeMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Astrageldon"), ModContent.ItemType<Items.Placeables.MusicBoxes.AstrageldonMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.AstrageldonMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/AstrumDeus"), ModContent.ItemType<Items.Placeables.MusicBoxes.AstrumDeusMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.AstrumDeusMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/LeftAlone"), ModContent.ItemType<Items.Placeables.MusicBoxes.BrimmyMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.BrimmyMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Murderswarm"), ModContent.ItemType<Items.Placeables.MusicBoxes.BumblebirbMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.BumblebirbMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Calamitas"), ModContent.ItemType<Items.Placeables.MusicBoxes.CalamitasMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.CalamitasMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Void"), ModContent.ItemType<Items.Placeables.MusicBoxes.CeaselessVoidMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.CeaselessVoidMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Crabulon"), ModContent.ItemType<Items.Placeables.MusicBoxes.CrabulonMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.CrabulonMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Cryogen"), ModContent.ItemType<Items.Placeables.MusicBoxes.CryogenMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.CryogenMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/DesertScourge"), ModContent.ItemType<Items.Placeables.MusicBoxes.DesertScourgeMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.DesertScourgeMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/ScourgeofTheUniverse"), ModContent.ItemType<Items.Placeables.MusicBoxes.DoGMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.DoGMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/UniversalCollapse"), ModContent.ItemType<Items.Placeables.MusicBoxes.DoGP2Musicbox>(), ModContent.TileType<Tiles.MusicBoxes.DoGP2Musicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/HiveMind"), ModContent.ItemType<Items.Placeables.MusicBoxes.HiveMindMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.HiveMindMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/LeviathanAndSiren"), ModContent.ItemType<Items.Placeables.MusicBoxes.LeviathanMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.LeviathanMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/BoomerDuke"), ModContent.ItemType<Items.Placeables.MusicBoxes.BoomerDukeMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.BoomerDukeMusicboxTile>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/BloodCoagulant"), ModContent.ItemType<Items.Placeables.MusicBoxes.PerforatorMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.PerforatorMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/PlaguebringerGoliath"), ModContent.ItemType<Items.Placeables.MusicBoxes.PlaguebringerMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.PlaguebringerMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/RUIN"), ModContent.ItemType<Items.Placeables.MusicBoxes.PolterghastMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.PolterghastMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Guardians"), ModContent.ItemType<Items.Placeables.MusicBoxes.ProfanedGuardianMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.ProfanedGuardianMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/ProvidenceTheme"), ModContent.ItemType<Items.Placeables.MusicBoxes.ProvidenceMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.ProvidenceMusicbox>()); //Seamless
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Ravager"), ModContent.ItemType<Items.Placeables.MusicBoxes.RavagerMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.RavagerMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/SCG"), ModContent.ItemType<Items.Placeables.MusicBoxes.SCalGMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.SCalGMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/SCL"), ModContent.ItemType<Items.Placeables.MusicBoxes.SCalLMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.SCalLMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/SCE"), ModContent.ItemType<Items.Placeables.MusicBoxes.SCalEMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.SCalEMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/SCA"), ModContent.ItemType<Items.Placeables.MusicBoxes.SCalAMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.SCalAMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Signus"), ModContent.ItemType<Items.Placeables.MusicBoxes.SignusMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.SignusMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Siren"), ModContent.ItemType<Items.Placeables.MusicBoxes.SirenMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.SirenMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/SirenLure"), ModContent.ItemType<Items.Placeables.MusicBoxes.SirenIdleMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.SirenIdleMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/SlimeGod"), ModContent.ItemType<Items.Placeables.MusicBoxes.SlimeGodMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.SlimeGodMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Weaver"), ModContent.ItemType<Items.Placeables.MusicBoxes.StormWeaverMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.StormWeaverMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/YHARON"), ModContent.ItemType<Items.Placeables.MusicBoxes.Yharon1Musicbox>(), ModContent.TileType<Tiles.MusicBoxes.Yharon1Musicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/YHARONREBIRTH"), ModContent.ItemType<Items.Placeables.MusicBoxes.Yharon2Musicbox>(), ModContent.TileType<Tiles.MusicBoxes.Yharon2Musicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/DragonGod"), ModContent.ItemType<Items.Placeables.MusicBoxes.Yharon3Musicbox>(), ModContent.TileType<Tiles.MusicBoxes.Yharon3Musicbox>());

            //Biome Music
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Sulphur"), ModContent.ItemType<Items.Placeables.MusicBoxes.SulphurousMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.SulphurousMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/TheAbyss"), ModContent.ItemType<Items.Placeables.MusicBoxes.HigherAbyssMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.HigherAbyssMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/TheDeepAbyss"), ModContent.ItemType<Items.Placeables.MusicBoxes.AbyssLowerMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.AbyssLowerMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/TheVoid"), ModContent.ItemType<Items.Placeables.MusicBoxes.VoidMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.VoidMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Astral"), ModContent.ItemType<Items.Placeables.MusicBoxes.AstralMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.AstralMusicbox>()); //Seamless
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/AstralUnderground"), ModContent.ItemType<Items.Placeables.MusicBoxes.AstralUndergroundMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.AstralUndergroundMusicboxTile>()); //Seamless
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Calamity"), ModContent.ItemType<Items.Placeables.MusicBoxes.CalamityMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.CalamityMusicbox>()); //Seamless
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/Crag"), ModContent.ItemType<Items.Placeables.MusicBoxes.CragMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.CragMusicbox>());
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/SunkenSea"), ModContent.ItemType<Items.Placeables.MusicBoxes.SunkenSeaMusicbox>(), ModContent.TileType<Tiles.MusicBoxes.SunkenSeaMusicbox>());

            //Event Music
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/AcidRain1"), ModContent.ItemType<Items.Placeables.MusicBoxes.AcidRain1Musicbox>(), ModContent.TileType<Tiles.MusicBoxes.AcidRain1MusicboxTile>()); //Seamless
            AddMusicBox(this, GetMusicSlot(Name + "/" + "Sounds/Music/AcidRain2"), ModContent.ItemType<Items.Placeables.MusicBoxes.AcidRain2Musicbox>(), ModContent.TileType<Tiles.MusicBoxes.AcidRain2MusicboxTile>());
        }

        private void LoadClient()
        {
            EquipLoader.AddEquipTexture(this, "CalRD/Items/Armor/SnowRuffianWings", EquipType.Wings, name: "SnowRuffWings", equipTexture: new SnowRuffianWings());
            
            AstralCactusTexture = ModContent.Request<Texture2D>("CalRD/ExtraTextures/Tiles/AstralCactus");
            AstralCactusGlowTexture = ModContent.Request<Texture2D>("CalRD/ExtraTextures/Tiles/AstralCactusGlow");
            AstralSky = ModContent.Request<Texture2D>("CalRD/ExtraTextures/AstralSky");

            Filters.Scene["CalRD:DevourerofGodsHead"] = new Filter(new DoGScreenShaderData("FilterMiniTower").UseColor(0.4f, 0.1f, 1.0f).UseOpacity(0.5f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalRD:DevourerofGodsHead"] = new DoGSky();

            Filters.Scene["CalRD:DevourerofGodsHeadS"] = new Filter(new DoGScreenShaderDataS("FilterMiniTower").UseColor(0.4f, 0.1f, 1.0f).UseOpacity(0.5f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalRD:DevourerofGodsHeadS"] = new DoGSkyS();

            Filters.Scene["CalRD:CalamitasRun3"] = new Filter(new CalScreenShaderData("FilterMiniTower").UseColor(1.1f, 0.3f, 0.3f).UseOpacity(0.6f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalRD:CalamitasRun3"] = new CalSky();

            Filters.Scene["CalRD:PlaguebringerGoliath"] = new Filter(new PbGScreenShaderData("FilterMiniTower").UseColor(0.2f, 0.6f, 0.2f).UseOpacity(0.35f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalRD:PlaguebringerGoliath"] = new PbGSky();

            Filters.Scene["CalRD:Yharon"] = new Filter(new YScreenShaderData("FilterMiniTower").UseColor(1f, 0.4f, 0f).UseOpacity(0.75f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalRD:Yharon"] = new YSky();

            Filters.Scene["CalRD:Leviathan"] = new Filter(new LevScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0.5f).UseOpacity(0.5f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalRD:Leviathan"] = new LevSky();

            Filters.Scene["CalRD:Providence"] = new Filter(new ProvScreenShaderData("FilterMiniTower").UseColor(0.45f, 0.4f, 0.2f).UseOpacity(0.5f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalRD:Providence"] = new ProvSky();

            Filters.Scene["CalRD:SupremeCalamitas"] = new Filter(new SCalScreenShaderData("FilterMiniTower").UseColor(1.1f, 0.3f, 0.3f).UseOpacity(0.65f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalRD:SupremeCalamitas"] = new SCalSky();

            Filters.Scene["CalRD:Signus"] = new Filter(new SignusScreenShaderData("FilterMiniTower").UseColor(0.35f, 0.1f, 0.55f).UseOpacity(0.35f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalRD:Signus"] = new SignusSky();

            SkyManager.Instance["CalRD:Astral"] = new AstralSky();
            SkyManager.Instance["CalRD:Cryogen"] = new CryogenSky();

            CalamityShaders.LoadShaders();

            RipperUI.Reset();
            AstralArcanumUI.Load(this);

            GameShaders.Hair.BindShader(ModContent.ItemType<AdrenalineHairDye>(), new LegacyHairShaderData().UseLegacyMethod((Player player, Color newColor, ref bool lighting) => Color.Lerp(player.hairColor, new Color(0, 255, 171), ((float)player.Calamity().adrenaline / (float)player.Calamity().adrenalineMax))));
            GameShaders.Hair.BindShader(ModContent.ItemType<RageHairDye>(), new LegacyHairShaderData().UseLegacyMethod((Player player, Color newColor, ref bool lighting) => Color.Lerp(player.hairColor, new Color(255, 83, 48), ((float)player.Calamity().rage / (float)player.Calamity().rageMax))));
            GameShaders.Hair.BindShader(ModContent.ItemType<WingTimeHairDye>(), new LegacyHairShaderData().UseLegacyMethod((Player player, Color newColor, ref bool lighting) => 
            {
                float flightTimeInterpolant = player.wingTime / player.wingTimeMax;
                if (float.IsInfinity(flightTimeInterpolant) || float.IsNaN(flightTimeInterpolant))
                    flightTimeInterpolant = 0f;

                return Color.Lerp(player.hairColor, new Color(139, 205, 255), flightTimeInterpolant);
            }));
            GameShaders.Hair.BindShader(ModContent.ItemType<StealthHairDye>(), new LegacyHairShaderData().UseLegacyMethod((Player player, Color newColor, ref bool lighting) => 
            {
                float stealthInterpolant = player.Calamity().rogueStealth / player.Calamity().rogueStealthMax;
                if (float.IsInfinity(stealthInterpolant) || float.IsNaN(stealthInterpolant))
                    stealthInterpolant = 0f;

                return Color.Lerp(player.hairColor, new Color(186, 85, 211), stealthInterpolant);
            }));

            PopupGUIManager.LoadGUIs();
            InvasionProgressUIManager.LoadGUIs();
        }
        #endregion

        #region Unload
        public override void Unload()
        {
            NormalityRelocatorHotKey = null;
            RageHotKey = null;
            AdrenalineHotKey = null;
            AegisHotKey = null;
            TarraHotKey = null;
            AstralTeleportHotKey = null;
            AstralArcanumUIHotkey = null;
            MomentumCapacitatorHotkey = null;
            SandCloakHotkey = null;
            SpectralVeilHotKey = null;
            PlaguePackHotKey = null;

            AstralCactusTexture = null;
            AstralCactusGlowTexture = null;
            AstralSky = null;

            DRValues?.Clear();
            DRValues = null;
            bossKillTimes?.Clear();
            bossKillTimes = null;
            bossVelocityDamageScaleValues?.Clear();
            bossVelocityDamageScaleValues = null;

            CalamityLists.UnloadLists();
            NPCStats.Unload();

            fargosMutant = false;

            PopupGUIManager.UnloadGUIs();
            InvasionProgressUIManager.UnloadGUIs();
            BossRushEvent.Unload();
            SchematicManager.Unload();
            BossHealthBarManager.Unload();

            TileFraming.Unload();

            RipperUI.Reset();
            AstralArcanumUI.Unload();

            if (!Main.dedServ)
            {
                TextureAssets.Heart = heartOriginal2;
                TextureAssets.Heart2 = heartOriginal;
                TextureAssets.Rain = rainOriginal;
                TextureAssets.Mana = manaOriginal;
                TextureAssets.FlyingCarpet = carpetOriginal;
            }

            heartOriginal2 = null;
            heartOriginal = null;
            rainOriginal = null;
            manaOriginal = null;
            carpetOriginal = null;

            ILChanges.Unload();
            Instance = null;
            base.Unload();
        }
        #endregion

        #region Late Loading
        public override void PostAddRecipes()/* tModPorter Note: Removed. Use ModSystem.PostAddRecipes */
        {
            // This is placed here so that all tiles from all mods are guaranteed to be loaded at this point.
            TileFraming.Load();
        }
        #endregion

        #region ConfigCrap
        internal static void SaveConfig(CalamityConfig cfg)
        {
            // in-game ModConfig saving from mod code is not supported yet in tmodloader, and subject to change, so we need to be extra careful.
            // This code only supports client configs, and doesn't call onchanged. It also doesn't support ReloadRequired or anything else.
            MethodInfo saveMethodInfo = typeof(ConfigManager).GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic);
            if (saveMethodInfo != null)
                saveMethodInfo.Invoke(null, new object[] { cfg });
            else
                Instance.Logger.Warn("In-game SaveConfig failed, code update required");
        }
        #endregion

        #region Vanilla Enemy DR
        private void SetupVanillaDR()
        {
            DRValues = new SortedDictionary<int, float> {
                { NPCID.AngryBonesBig, 0.2f },
                { NPCID.AngryBonesBigHelmet, 0.2f },
                { NPCID.AngryBonesBigMuscle, 0.2f },
                { NPCID.AnomuraFungus, 0.1f },
                { NPCID.Antlion, 0.1f },
                { NPCID.Arapaima, 0.1f },
                { NPCID.ArmoredSkeleton, 0.15f },
                { NPCID.ArmoredViking, 0.1f },
                { NPCID.BigMimicCorruption, 0.3f },
                { NPCID.BigMimicCrimson, 0.3f },
                { NPCID.BigMimicHallow, 0.3f },
                { NPCID.BigMimicJungle, 0.3f }, // unused vanilla enemy
                { NPCID.BlueArmoredBones, 0.2f },
                { NPCID.BlueArmoredBonesMace, 0.2f },
                { NPCID.BlueArmoredBonesNoPants, 0.2f },
                { NPCID.BlueArmoredBonesSword, 0.2f },
                { NPCID.BoneLee, 0.2f },
                { NPCID.Crab, 0.05f },
                { NPCID.Crawdad, 0.2f },
                { NPCID.Crawdad2, 0.2f },
                { NPCID.CultistBoss, 0.1f },
                { NPCID.DD2Betsy, 0.1f },
                { NPCID.DD2OgreT2, 0.1f },
                { NPCID.DD2OgreT3, 0.15f },
                { NPCID.DeadlySphere, 0.4f },
                { NPCID.DiabolistRed, 0.2f },
                { NPCID.DiabolistWhite, 0.2f },
                { NPCID.DukeFishron, 0.1f },
                { NPCID.DungeonGuardian, 0.999999f },
                { NPCID.DungeonSpirit, 0.2f },
                { NPCID.ElfCopter, 0.15f },
                { NPCID.Everscream, 0.1f },
                { NPCID.FlyingAntlion, 0.05f },
                { NPCID.GiantCursedSkull, 0.2f },
                { NPCID.GiantShelly, 0.2f },
                { NPCID.GiantShelly2, 0.2f },
                { NPCID.GiantTortoise, 0.35f },
                { NPCID.Golem, 0.25f },
                { NPCID.GolemFistLeft, 0.25f },
                { NPCID.GolemFistRight, 0.25f },
                { NPCID.GolemHead, 0.25f },
                { NPCID.GolemHeadFree, 0.25f },
                { NPCID.GraniteFlyer, 0.1f },
                { NPCID.GraniteGolem, 0.15f },
                { NPCID.GreekSkeleton, 0.1f },
                { NPCID.HellArmoredBones, 0.2f },
                { NPCID.HellArmoredBonesMace, 0.2f },
                { NPCID.HellArmoredBonesSpikeShield, 0.2f },
                { NPCID.HellArmoredBonesSword, 0.2f },
                { NPCID.IceGolem, 0.1f },
                { NPCID.IceQueen, 0.1f },
                { NPCID.IceTortoise, 0.35f },
                { NPCID.HeadlessHorseman, 0.05f },
                { NPCID.MartianDrone, 0.2f },
                { NPCID.MartianSaucer, 0.2f },
                { NPCID.MartianSaucerCannon, 0.2f },
                { NPCID.MartianSaucerCore, 0.2f },
                { NPCID.MartianSaucerTurret, 0.2f },
                { NPCID.MartianTurret, 0.2f },
                { NPCID.MartianWalker, 0.35f },
                { NPCID.Mimic, 0.3f },
                { NPCID.MoonLordCore, 0.05f },
                { NPCID.MoonLordHand, 0.05f },
                { NPCID.MoonLordHead, 0.05f },
                { NPCID.Mothron, 0.2f },
                { NPCID.MothronEgg, 0.5f },
                { NPCID.MourningWood, 0.1f },
                { NPCID.Necromancer, 0.2f },
                { NPCID.NecromancerArmored, 0.2f },
                { NPCID.Paladin, 0.45f },
                { NPCID.PirateCaptain, 0.05f },
                { NPCID.PirateShipCannon, 0.15f },
                { NPCID.Plantera, 0.15f },
                { NPCID.PlanterasTentacle, 0.1f },
                { NPCID.PossessedArmor, 0.25f },
                { NPCID.PresentMimic, 0.3f },
                { NPCID.PrimeCannon, 0.2f },
                { NPCID.PrimeLaser, 0.2f },
                { NPCID.PrimeSaw, 0.2f },
                { NPCID.PrimeVice, 0.2f },
                { NPCID.Probe, 0.2f },
                { NPCID.Pumpking, 0.1f },
                { NPCID.QueenBee, 0.05f },
                { NPCID.RaggedCaster, 0.2f },
                { NPCID.RaggedCasterOpenCoat, 0.2f },
                { NPCID.Retinazer, 0.2f },
                { NPCID.RustyArmoredBonesAxe, 0.2f },
                { NPCID.RustyArmoredBonesFlail, 0.2f },
                { NPCID.RustyArmoredBonesSword, 0.2f },
                { NPCID.RustyArmoredBonesSwordNoArmor, 0.2f },
                { NPCID.SandElemental, 0.1f },
                { NPCID.SantaNK1, 0.35f },
                { NPCID.SeaSnail, 0.05f },
                { NPCID.SkeletonArcher, 0.1f },
                { NPCID.SkeletonCommando, 0.2f },
                { NPCID.SkeletonSniper, 0.2f },
                { NPCID.SkeletronHand, 0.05f },
                { NPCID.SkeletronHead, 0.05f },
                { NPCID.SkeletronPrime, 0.2f },
                { NPCID.Spazmatism, 0.2f },
                { NPCID.TacticalSkeleton, 0.2f },
                { NPCID.TheDestroyer, 0.1f },
                { NPCID.TheDestroyerBody, 0.2f },
                { NPCID.TheDestroyerTail, 0.35f },
                { NPCID.TheHungry, 0.1f },
                { NPCID.UndeadViking, 0.1f },
                { NPCID.WalkingAntlion, 0.1f },
                { NPCID.WallofFlesh, 0.5f },
            };
        }
        #endregion

        #region Boss Kill Times
        private void SetupBossKillTimes()
        {
            // 3600 = 1 minute

            bossKillTimes = new SortedDictionary<int, int> {
                { NPCID.KingSlime, 3600 },
                { NPCID.EyeofCthulhu, 5400 },
                { NPCID.EaterofWorldsHead, 7200 },
                { NPCID.EaterofWorldsBody, 7200 },
                { NPCID.EaterofWorldsTail, 7200 },
                { NPCID.BrainofCthulhu, 5400 },
                { NPCID.Creeper, 1800 },
                { NPCID.QueenBee, 7200 },
                { NPCID.SkeletronHead, 9000 },
                { NPCID.WallofFlesh, 7200 },
                { NPCID.WallofFleshEye, 7200 },
                { NPCID.Spazmatism, 10800 },
                { NPCID.Retinazer, 10800 },
                { NPCID.TheDestroyer, 10800 },
                { NPCID.TheDestroyerBody, 10800 },
                { NPCID.TheDestroyerTail, 10800 },
                { NPCID.SkeletronPrime, 10800 },
                { NPCID.Plantera, 10800 },
                { NPCID.Golem, 9000 },
                { NPCID.GolemHead, 3600 },
                { NPCID.DukeFishron, 9000 },
                { NPCID.CultistBoss, 9000 },
                { NPCID.MoonLordCore, 14400 },
                { NPCID.MoonLordHand, 7200 },
                { NPCID.MoonLordHead, 7200 },
                { ModContent.NPCType<DesertScourgeHead>(), 3600 },
                { ModContent.NPCType<DesertScourgeBody>(), 3600 },
                { ModContent.NPCType<DesertScourgeTail>(), 3600 },
                { ModContent.NPCType<CrabulonIdle>(), 5400 },
                { ModContent.NPCType<HiveMind>(), 1800 },
                { ModContent.NPCType<HiveMindP2>(), 5400 },
                { ModContent.NPCType<PerforatorHive>(), 7200 },
                { ModContent.NPCType<SlimeGodCore>(), 10800 },
                { ModContent.NPCType<SlimeGod>(), 3600 },
                { ModContent.NPCType<SlimeGodRun>(), 3600 },
                { ModContent.NPCType<SlimeGodSplit>(), 3600 },
                { ModContent.NPCType<SlimeGodRunSplit>(), 3600 },
                { ModContent.NPCType<Cryogen>(), 10800 },
                { ModContent.NPCType<AquaticScourgeHead>(), 7200 },
                { ModContent.NPCType<AquaticScourgeBody>(), 7200 },
                { ModContent.NPCType<AquaticScourgeBodyAlt>(), 7200 },
                { ModContent.NPCType<AquaticScourgeTail>(), 7200 },
                { ModContent.NPCType<BrimstoneElemental>(), 10800 },
                { ModContent.NPCType<Calamitas>(), 1200 },
                { ModContent.NPCType<CalamitasRun3>(), 11400 },
                { ModContent.NPCType<Leviathan>(), 10800 },
                { ModContent.NPCType<Siren>(), 10800 },
                { ModContent.NPCType<AstrumAureus>(), 10800 },
                { ModContent.NPCType<AstrumDeusHeadSpectral>(), 7200 },
                { ModContent.NPCType<AstrumDeusBodySpectral>(), 7200 },
                { ModContent.NPCType<AstrumDeusTailSpectral>(), 7200 },
                { ModContent.NPCType<PlaguebringerGoliath>(), 10800 },
                { ModContent.NPCType<RavagerBody>(), 10800 },
                { ModContent.NPCType<ProfanedGuardianBoss>(), 5400 },
                { ModContent.NPCType<Bumblefuck>(), 7200 },
                { ModContent.NPCType<Providence>(), 14400 },
                { ModContent.NPCType<DarkEnergy>(), 1200 },
                { ModContent.NPCType<DarkEnergy2>(), 1200 },
                { ModContent.NPCType<DarkEnergy3>(), 1200 },
                { ModContent.NPCType<StormWeaverHeadNaked>(), 5400 },
                { ModContent.NPCType<StormWeaverBodyNaked>(), 5400 },
                { ModContent.NPCType<StormWeaverTailNaked>(), 5400 },
                { ModContent.NPCType<Signus>(), 7200 },
                { ModContent.NPCType<Polterghast>(), 10800 },
                { ModContent.NPCType<OldDuke>(), 10800 },
                { ModContent.NPCType<DevourerofGodsHead>(), 5400 },
                { ModContent.NPCType<DevourerofGodsBody>(), 5400 },
                { ModContent.NPCType<DevourerofGodsTail>(), 5400 },
                { ModContent.NPCType<DevourerofGodsHeadS>(), 9000 },
                { ModContent.NPCType<DevourerofGodsBodyS>(), 9000 },
                { ModContent.NPCType<DevourerofGodsTailS>(), 9000 },
                { ModContent.NPCType<Yharon>(), 10800 },
                { ModContent.NPCType<SupremeCalamitas>(), 18000 }
            };
        }
        #endregion

        #region Boss Velocity Contact Damage Scale Values
        private void SetupBossVelocityScalingValues()
        {
            bossVelocityDamageScaleValues = new SortedDictionary<int, float> {
                { NPCID.KingSlime, velocityScaleMin },
                { NPCID.EyeofCthulhu, velocityScaleMin }, // Increases in phase 2
                { NPCID.EaterofWorldsHead, bitingEnemeyVelocityScale },
                { NPCID.EaterofWorldsBody, velocityScaleMin },
                { NPCID.EaterofWorldsTail, velocityScaleMin },
                { NPCID.Creeper, velocityScaleMin },
                { NPCID.BrainofCthulhu, velocityScaleMin },
                { NPCID.QueenBee, velocityScaleMin },
                { NPCID.SkeletronHead, velocityScaleMin },
                { NPCID.SkeletronHand, velocityScaleMin },
                { NPCID.TheHungry, bitingEnemeyVelocityScale },
                { NPCID.TheHungryII, bitingEnemeyVelocityScale },
                { NPCID.LeechHead, bitingEnemeyVelocityScale },
                { NPCID.LeechBody, velocityScaleMin },
                { NPCID.LeechTail, velocityScaleMin },
                { NPCID.Spazmatism, velocityScaleMin }, // Increases in phase 2
                { NPCID.Retinazer, velocityScaleMin },
                { NPCID.TheDestroyer, bitingEnemeyVelocityScale },
                { NPCID.TheDestroyerBody, velocityScaleMin },
                { NPCID.TheDestroyerTail, velocityScaleMin },
                { NPCID.Probe, velocityScaleMin },
                { NPCID.SkeletronPrime, velocityScaleMin },
                { NPCID.PrimeCannon, velocityScaleMin },
                { NPCID.PrimeLaser, velocityScaleMin },
                { NPCID.PrimeSaw, velocityScaleMin },
                { NPCID.PrimeVice, velocityScaleMin },
                { NPCID.Plantera, velocityScaleMin }, // Increases in phase 2
                { NPCID.PlanterasTentacle, bitingEnemeyVelocityScale },
                { NPCID.Golem, velocityScaleMin },
                { NPCID.GolemFistLeft, velocityScaleMin },
                { NPCID.GolemFistRight, velocityScaleMin },
                { NPCID.GolemHead, velocityScaleMin },
                { NPCID.DukeFishron, velocityScaleMin },
                { ModContent.NPCType<DesertScourgeHead>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<DesertScourgeBody>(), velocityScaleMin },
                { ModContent.NPCType<DesertScourgeTail>(), velocityScaleMin },
                { ModContent.NPCType<DesertScourgeHeadSmall>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<DesertScourgeBodySmall>(), velocityScaleMin },
                { ModContent.NPCType<DesertScourgeTailSmall>(), velocityScaleMin },
                { ModContent.NPCType<CrabulonIdle>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<HiveMindP2>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorHive>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorHeadLarge>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<PerforatorBodyLarge>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorTailLarge>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorHeadMedium>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<PerforatorBodyMedium>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorTailMedium>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorHeadSmall>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<PerforatorBodySmall>(), velocityScaleMin },
                { ModContent.NPCType<PerforatorTailSmall>(), velocityScaleMin },
                { ModContent.NPCType<SlimeGodCore>(), velocityScaleMin },
                { ModContent.NPCType<SlimeGod>(), velocityScaleMin },
                { ModContent.NPCType<SlimeGodRun>(), velocityScaleMin },
                { ModContent.NPCType<SlimeGodSplit>(), velocityScaleMin },
                { ModContent.NPCType<SlimeGodRunSplit>(), velocityScaleMin },
                { ModContent.NPCType<SlimeSpawnCorrupt>(), velocityScaleMin },
                { ModContent.NPCType<Cryogen>(), velocityScaleMin },
                { ModContent.NPCType<Cryocore>(), velocityScaleMin },
                { ModContent.NPCType<Cryocore2>(), velocityScaleMin },
                { ModContent.NPCType<IceMass>(), velocityScaleMin },
                { ModContent.NPCType<AquaticScourgeHead>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<AquaticScourgeBody>(), velocityScaleMin },
                { ModContent.NPCType<AquaticScourgeBodyAlt>(), velocityScaleMin },
                { ModContent.NPCType<AquaticScourgeTail>(), velocityScaleMin },
                { ModContent.NPCType<BrimstoneElemental>(), velocityScaleMin },
                { ModContent.NPCType<Calamitas>(), velocityScaleMin },
                { ModContent.NPCType<CalamitasRun3>(), velocityScaleMin },
                { ModContent.NPCType<Leviathan>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<Siren>(), velocityScaleMin },
                { ModContent.NPCType<AstrumAureus>(), velocityScaleMin },
                { ModContent.NPCType<AstrumDeusHeadSpectral>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<AstrumDeusBodySpectral>(), velocityScaleMin },
                { ModContent.NPCType<AstrumDeusTailSpectral>(), velocityScaleMin },
                { ModContent.NPCType<PlaguebringerGoliath>(), velocityScaleMin },
                { ModContent.NPCType<PlaguebringerShade>(), velocityScaleMin },
                { ModContent.NPCType<PlagueBeeG>(), velocityScaleMin },
                { ModContent.NPCType<PlagueBeeLargeG>(), velocityScaleMin },
                { ModContent.NPCType<RavagerBody>(), velocityScaleMin },
                { ModContent.NPCType<RavagerClawLeft>(), velocityScaleMin },
                { ModContent.NPCType<RavagerClawRight>(), velocityScaleMin },
                { ModContent.NPCType<RavagerLegLeft>(), velocityScaleMin },
                { ModContent.NPCType<RavagerLegRight>(), velocityScaleMin },
                { ModContent.NPCType<RockPillar>(), velocityScaleMin },
                { ModContent.NPCType<ProfanedGuardianBoss>(), velocityScaleMin },
                { ModContent.NPCType<ProfanedGuardianBoss2>(), velocityScaleMin },
                { ModContent.NPCType<ProfanedGuardianBoss3>(), velocityScaleMin },
                { ModContent.NPCType<Bumblefuck>(), velocityScaleMin },
                { ModContent.NPCType<Bumblefuck2>(), velocityScaleMin },
                { ModContent.NPCType<CeaselessVoid>(), velocityScaleMin },
                { ModContent.NPCType<DarkEnergy>(), velocityScaleMin },
                { ModContent.NPCType<DarkEnergy2>(), velocityScaleMin },
                { ModContent.NPCType<DarkEnergy3>(), velocityScaleMin },
                { ModContent.NPCType<StormWeaverHead>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<StormWeaverBody>(), velocityScaleMin },
                { ModContent.NPCType<StormWeaverTail>(), velocityScaleMin },
                { ModContent.NPCType<StormWeaverHeadNaked>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<StormWeaverBodyNaked>(), velocityScaleMin },
                { ModContent.NPCType<StormWeaverTailNaked>(), velocityScaleMin },
                { ModContent.NPCType<Signus>(), velocityScaleMin },
                { ModContent.NPCType<CosmicLantern>(), velocityScaleMin },
                { ModContent.NPCType<Polterghast>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<PolterPhantom>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<OldDuke>(), velocityScaleMin },
                { ModContent.NPCType<DevourerofGodsHead>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<DevourerofGodsBody>(), velocityScaleMin },
                { ModContent.NPCType<DevourerofGodsTail>(), velocityScaleMin },
                { ModContent.NPCType<DevourerofGodsHead2>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<DevourerofGodsBody2>(), velocityScaleMin },
                { ModContent.NPCType<DevourerofGodsTail2>(), velocityScaleMin },
                { ModContent.NPCType<DevourerofGodsHeadS>(), bitingEnemeyVelocityScale },
                { ModContent.NPCType<DevourerofGodsBodyS>(), velocityScaleMin },
                { ModContent.NPCType<DevourerofGodsTailS>(), velocityScaleMin },
                { ModContent.NPCType<Yharon>(), velocityScaleMin },
                { ModContent.NPCType<DetonatingFlare>(), velocityScaleMin },
                { ModContent.NPCType<DetonatingFlare2>(), velocityScaleMin },
                { ModContent.NPCType<SupremeCalamitas>(), velocityScaleMin }
            };
        }
        #endregion

        #region ModSupport
        public override void PostSetupContent() => WeakReferenceSupport.Setup();

        public override object Call(params object[] args) => ModCalls.Call(args);
        #endregion

        #region DrawingStuff
        public static Color GetNPCColor(NPC npc, Vector2? position = null, bool effects = true, float shadowOverride = 0f)
        {
            return npc.GetAlpha(BuffEffects(
                npc, GetLightColor(position != null ? (Vector2)position : npc.Center),
                shadowOverride != 0f ? shadowOverride : 0f, effects, npc.poisoned, npc.onFire, npc.onFire2,
                Main.player[Main.myPlayer].detectCreature, false, false, false, npc.venom, npc.midas, npc.ichor,
                npc.onFrostBurn, false, false, npc.dripping, npc.drippingSlime, npc.loveStruck, npc.stinky)
            );
        }

        public static Color GetLightColor(Vector2 position) => Lighting.GetColor((int)(position.X / 16f), (int)(position.Y / 16f));

        public static Color BuffEffects(Entity codable, Color lightColor, float shadow = 0f, bool effects = true,
            bool poisoned = false, bool onFire = false, bool onFire2 = false, bool hunter = false, bool noItems = false,
            bool blind = false, bool bleed = false, bool venom = false, bool midas = false, bool ichor = false,
            bool onFrostBurn = false, bool burned = false, bool honey = false, bool dripping = false,
            bool drippingSlime = false, bool loveStruck = false, bool stinky = false)
        {
            float cr = 1f;
            float cg = 1f;
            float cb = 1f;
            float ca = 1f;
            if (effects && honey && Main.rand.NextBool(30))
            {
                int dustID = Dust.NewDust(codable.position, codable.width, codable.height, 152, 0f, 0f, 150, default, 1f);
                Main.dust[dustID].velocity.Y = 0.3f;
                Main.dust[dustID].velocity.X *= 0.1f;
                Main.dust[dustID].scale += Main.rand.Next(3, 4) * 0.1f;
                Main.dust[dustID].alpha = 100;
                Main.dust[dustID].noGravity = true;
                Main.dust[dustID].velocity += codable.velocity * 0.1f;
            }
            if (poisoned)
            {
                if (effects && Main.rand.NextBool(30))
                {
                    int dustID = Dust.NewDust(codable.position, codable.width, codable.height, 46, 0f, 0f, 120, default, 0.2f);
                    Main.dust[dustID].noGravity = true;
                    Main.dust[dustID].fadeIn = 1.9f;
                }
                cr *= 0.65f;
                cb *= 0.75f;
            }
            if (venom)
            {
                if (effects && Main.rand.NextBool(10))
                {
                    int dustID = Dust.NewDust(codable.position, codable.width, codable.height, 171, 0f, 0f, 100, default, 0.5f);
                    Main.dust[dustID].noGravity = true;
                    Main.dust[dustID].fadeIn = 1.5f;
                }
                cg *= 0.45f;
                cr *= 0.75f;
            }
            if (midas)
            {
                cb *= 0.3f;
                cr *= 0.85f;
            }
            if (ichor)
            {
                if (codable is NPC)
                {
                    lightColor = new Color(255, 255, 0, 255);
                }
                else
                {
                    cb = 0f;
                }
            }
            if (burned)
            {
                if (effects)
                {
                    int dustID = Dust.NewDust(new Vector2(codable.position.X - 2f, codable.position.Y - 2f), codable.width + 4, codable.height + 4, 6, codable.velocity.X * 0.4f, codable.velocity.Y * 0.4f, 100, default, 2f);
                    Main.dust[dustID].noGravity = true;
                    Main.dust[dustID].velocity *= 1.8f;
                    Main.dust[dustID].velocity.Y -= 0.75f;
                }
                if (codable is Player)
                {
                    cr = 1f;
                    cb *= 0.6f;
                    cg *= 0.7f;
                }
            }
            if (onFrostBurn)
            {
                if (effects)
                {
                    if (Main.rand.Next(4) < 3)
                    {
                        int dustID = Dust.NewDust(new Vector2(codable.position.X - 2f, codable.position.Y - 2f), codable.width + 4, codable.height + 4, 135, codable.velocity.X * 0.4f, codable.velocity.Y * 0.4f, 100, default, 3.5f);
                        Main.dust[dustID].noGravity = true;
                        Main.dust[dustID].velocity *= 1.8f;
                        Main.dust[dustID].velocity.Y -= 0.5f;
                        if (Main.rand.NextBool(4))
                        {
                            Main.dust[dustID].noGravity = false;
                            Main.dust[dustID].scale *= 0.5f;
                        }
                    }
                    Lighting.AddLight((int)(codable.position.X / 16f), (int)(codable.position.Y / 16f + 1f), 0.1f, 0.6f, 1f);
                }
                if (codable is Player)
                {
                    cr *= 0.5f;
                    cg *= 0.7f;
                }
            }
            if (onFire)
            {
                if (effects)
                {
                    if (Main.rand.Next(4) != 0)
                    {
                        int dustID = Dust.NewDust(codable.position - new Vector2(2f, 2f), codable.width + 4, codable.height + 4, 6, codable.velocity.X * 0.4f, codable.velocity.Y * 0.4f, 100, default, 3.5f);
                        Main.dust[dustID].noGravity = true;
                        Main.dust[dustID].velocity *= 1.8f;
                        Main.dust[dustID].velocity.Y -= 0.5f;
                        if (Main.rand.NextBool(4))
                        {
                            Main.dust[dustID].noGravity = false;
                            Main.dust[dustID].scale *= 0.5f;
                        }
                    }
                    Lighting.AddLight((int)(codable.position.X / 16f), (int)(codable.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
                }
                if (codable is Player)
                {
                    cb *= 0.6f;
                    cg *= 0.7f;
                }
            }
            if (dripping && shadow == 0f && Main.rand.Next(4) != 0)
            {
                Vector2 position = codable.position;
                position.X -= 2f;
                position.Y -= 2f;
                if (Main.rand.NextBool(2))
                {
                    int dustID = Dust.NewDust(position, codable.width + 4, codable.height + 2, 211, 0f, 0f, 50, default, 0.8f);
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dustID].alpha += 25;
                    }
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dustID].alpha += 25;
                    }
                    Main.dust[dustID].noLight = true;
                    Main.dust[dustID].velocity *= 0.2f;
                    Main.dust[dustID].velocity.Y += 0.2f;
                    Main.dust[dustID].velocity += codable.velocity;
                }
                else
                {
                    int dustID = Dust.NewDust(position, codable.width + 8, codable.height + 8, 211, 0f, 0f, 50, default, 1.1f);
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dustID].alpha += 25;
                    }
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dustID].alpha += 25;
                    }
                    Main.dust[dustID].noLight = true;
                    Main.dust[dustID].noGravity = true;
                    Main.dust[dustID].velocity *= 0.2f;
                    Main.dust[dustID].velocity.Y += 1f;
                    Main.dust[dustID].velocity += codable.velocity;
                }
            }
            if (drippingSlime && shadow == 0f)
            {
                int alpha = 175;
                Color newColor = new Color(0, 80, 255, 100);
                if (Main.rand.Next(4) != 0)
                {
                    if (Main.rand.NextBool(2))
                    {
                        Vector2 position2 = codable.position;
                        position2.X -= 2f;
                        position2.Y -= 2f;
                        int dustID = Dust.NewDust(position2, codable.width + 4, codable.height + 2, 4, 0f, 0f, alpha, newColor, 1.4f);
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[dustID].alpha += 25;
                        }
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[dustID].alpha += 25;
                        }
                        Main.dust[dustID].noLight = true;
                        Main.dust[dustID].velocity *= 0.2f;
                        Main.dust[dustID].velocity.Y += 0.2f;
                        Main.dust[dustID].velocity += codable.velocity;
                    }
                }
                cr *= 0.8f;
                cg *= 0.8f;
            }
            if (onFire2)
            {
                if (effects)
                {
                    if (Main.rand.Next(4) != 0)
                    {
                        int dustID = Dust.NewDust(codable.position - new Vector2(2f, 2f), codable.width + 4, codable.height + 4, 75, codable.velocity.X * 0.4f, codable.velocity.Y * 0.4f, 100, default, 3.5f);
                        Main.dust[dustID].noGravity = true;
                        Main.dust[dustID].velocity *= 1.8f;
                        Main.dust[dustID].velocity.Y -= 0.5f;
                        if (Main.rand.NextBool(4))
                        {
                            Main.dust[dustID].noGravity = false;
                            Main.dust[dustID].scale *= 0.5f;
                        }
                    }
                    Lighting.AddLight((int)(codable.position.X / 16f), (int)(codable.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
                }
                if (codable is Player)
                {
                    cb *= 0.6f;
                    cg *= 0.7f;
                }
            }
            if (noItems)
            {
                cr *= 0.65f;
                cg *= 0.8f;
            }
            if (blind)
            {
                cr *= 0.7f;
                cg *= 0.65f;
            }
            if (bleed)
            {
                bool dead = codable is Player ? ((Player)codable).dead : codable is NPC ? ((NPC)codable).life <= 0 : false;
                if (effects && !dead && Main.rand.NextBool(30))
                {
                    int dustID = Dust.NewDust(codable.position, codable.width, codable.height, 5, 0f, 0f, 0, default, 1f);
                    Main.dust[dustID].velocity.Y += 0.5f;
                    Main.dust[dustID].velocity *= 0.25f;
                }
                cg *= 0.9f;
                cb *= 0.9f;
            }
            if (loveStruck && effects && shadow == 0f && Main.instance.IsActive && !Main.gamePaused && Main.rand.NextBool(5))
            {
                Vector2 value = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                value.Normalize();
                value.X *= 0.66f;
                int goreID = Gore.NewGore(codable.GetSource_FromThis(), codable.position + new Vector2(Main.rand.Next(codable.width + 1), Main.rand.Next(codable.height + 1)), value * Main.rand.Next(3, 6) * 0.33f, 331, Main.rand.Next(40, 121) * 0.01f);
                Main.gore[goreID].sticky = false;
                Main.gore[goreID].velocity *= 0.4f;
                Main.gore[goreID].velocity.Y -= 0.6f;
            }
            if (stinky && shadow == 0f)
            {
                cr *= 0.7f;
                cb *= 0.55f;
                if (effects && Main.rand.NextBool(5) && Main.instance.IsActive && !Main.gamePaused)
                {
                    Vector2 value2 = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                    value2.Normalize();
                    value2.X *= 0.66f;
                    value2.Y = Math.Abs(value2.Y);
                    Vector2 vector = value2 * Main.rand.Next(3, 5) * 0.25f;
                    int dustID = Dust.NewDust(codable.position, codable.width, codable.height, 188, vector.X, vector.Y * 0.5f, 100, default, 1.5f);
                    Main.dust[dustID].velocity *= 0.1f;
                    Main.dust[dustID].velocity.Y -= 0.5f;
                }
            }
            lightColor.R = (byte)(lightColor.R * cr);
            lightColor.G = (byte)(lightColor.G * cg);
            lightColor.B = (byte)(lightColor.B * cb);
            lightColor.A = (byte)(lightColor.A * ca);
            if (codable is NPC)
            {
                NPCLoader.DrawEffects((NPC)codable, ref lightColor);
            }
            if (hunter && (codable is NPC ? ((NPC)codable).lifeMax > 1 : true))
            {
                if (effects && !Main.gamePaused && Main.instance.IsActive && Main.rand.NextBool(50))
                {
                    int dustID = Dust.NewDust(codable.position, codable.width, codable.height, 15, 0f, 0f, 150, default, 0.8f);
                    Main.dust[dustID].velocity *= 0.1f;
                    Main.dust[dustID].noLight = true;
                }
                byte colorR = 50, colorG = 255, colorB = 50;
                if (codable is NPC && !(((NPC)codable).friendly || ((NPC)codable).catchItem > 0 || (((NPC)codable).damage == 0 && ((NPC)codable).lifeMax == 5)))
                {
                    colorR = 255;
                    colorG = 50;
                }
                if (!(codable is NPC) && lightColor.R < 150)
                {
                    lightColor.A = Main.mouseTextColor;
                }
                if (lightColor.R < colorR)
                {
                    lightColor.R = colorR;
                }
                if (lightColor.G < colorG)
                {
                    lightColor.G = colorG;
                }
                if (lightColor.B < colorB)
                {
                    lightColor.B = colorB;
                }
            }
            return lightColor;
        }

        public static void DrawTexture(object sb, Texture2D texture, int shader, Entity codable, Color? overrideColor = null, bool drawCentered = false)
        {
            Color lightColor = overrideColor != null ? (Color)overrideColor : codable is NPC ? GetNPCColor((NPC)codable, codable.Center, false) : codable is Projectile ? ((Projectile)codable).GetAlpha(GetLightColor(codable.Center)) : GetLightColor(codable.Center);
            int frameCount = codable is NPC ? Main.npcFrameCount[((NPC)codable).type] : 1;
            Rectangle frame = codable is NPC ? ((NPC)codable).frame : new Rectangle(0, 0, texture.Width, texture.Height);
            float scale = codable is NPC ? ((NPC)codable).scale : ((Projectile)codable).scale;
            float rotation = codable is NPC ? ((NPC)codable).rotation : ((Projectile)codable).rotation;
            int spriteDirection = codable is NPC ? ((NPC)codable).spriteDirection : ((Projectile)codable).spriteDirection;
            float offsetY = codable is NPC ? ((NPC)codable).gfxOffY : 0f;
            DrawTexture(sb, texture, shader, codable.position + new Vector2(0f, offsetY), codable.width, codable.height, scale, rotation, spriteDirection, frameCount, frame, lightColor, drawCentered);
        }

        public static void DrawTexture(object sb, Texture2D texture, int shader, Vector2 position, int width, int height, float scale, float rotation, int direction, int framecount, Rectangle frame, Color? overrideColor = null, bool drawCentered = false)
        {
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / framecount / 2);
            Color lightColor = overrideColor != null ? (Color)overrideColor : GetLightColor(position + new Vector2(width * 0.5f, height * 0.5f));
            if (sb is List<DrawData>)
            {
                DrawData dd = new DrawData(texture, GetDrawPosition(position, origin, width, height, texture.Width, texture.Height, framecount, scale, drawCentered), frame, lightColor, rotation, origin, scale, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0)
                {
                    shader = shader
                };
                ((List<DrawData>)sb).Add(dd);
            }
            else if (sb is SpriteBatch)
            {
                bool applyDye = shader > 0;
                if (applyDye)
                {
                    ((SpriteBatch)sb).End();
                    ((SpriteBatch)sb).Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    GameShaders.Armor.ApplySecondary(shader, Main.player[Main.myPlayer], null);
                }
                ((SpriteBatch)sb).Draw(texture, GetDrawPosition(position, origin, width, height, texture.Width, texture.Height, framecount, scale, drawCentered), frame, lightColor, rotation, origin, scale, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                if (applyDye)
                {
                    ((SpriteBatch)sb).End();
                    ((SpriteBatch)sb).Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                }
            }
        }

        public static Vector2 GetDrawPosition(Vector2 position, Vector2 origin, int width, int height, int texWidth, int texHeight, int framecount, float scale, bool drawCentered = false)
        {
            Vector2 screenPos = new Vector2((int)Main.screenPosition.X, (int)Main.screenPosition.Y);
            if (drawCentered)
            {
                Vector2 texHalf = new Vector2(texWidth / 2, texHeight / framecount / 2);
                return position + new Vector2(width * 0.5f, height * 0.5f) - (texHalf * scale) + (origin * scale) - screenPos;
            }
            return position - screenPos + new Vector2(width * 0.5f, height) - new Vector2(texWidth * scale / 2f, texHeight * scale / framecount) + (origin * scale) + new Vector2(0f, 5f);
        }
        #endregion

        #region Recipes
        public override void AddRecipeGroups()/* tModPorter Note: Removed. Use ModSystem.AddRecipeGroups */ => CalamityRecipes.AddRecipeGroups();

        public override void AddRecipes()/* tModPorter Note: Removed. Use ModSystem.AddRecipes */ => CalamityRecipes.AddRecipes();
        #endregion

        #region Seasons
        public static Season CurrentSeason
        {
            get
            {
                DateTime date = DateTime.Now;
                int day = date.DayOfYear - Convert.ToInt32(DateTime.IsLeapYear(date.Year) && date.DayOfYear > 59);

                if (day < 80 || day >= 355)
                {
                    return Season.Winter;
                }

                else if (day >= 80 && day < 172)
                {
                    return Season.Spring;
                }

                else if (day >= 172 && day < 266)
                {
                    return Season.Summer;
                }

                else
                {
                    return Season.Fall;
                }
            }
        }
        #endregion

        #region Stop Rain
        public static void StopRain()
        {
            if (!Main.raining)
                return;
            Main.raining = false;
            CalamityNetcode.SyncWorld();
        }
        #endregion

        #region Netcode
        public override void HandlePacket(BinaryReader reader, int whoAmI) => CalamityNetcode.HandlePacket(this, reader, whoAmI);
        #endregion
    }

    public enum Season : byte
    {
        Winter,
        Spring,
        Summer,
        Fall
    }
}
