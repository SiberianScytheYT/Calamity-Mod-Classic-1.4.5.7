using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.BiomeManagers
{
	public class Astral : ModBiome
	{
		Mod _musicMod = ModLoader.HasMod("CalamityModMusic") ? ModLoader.GetMod("CalamityModMusic") : null;

		public override int Music {
			get
			{
				if (_musicMod != null)
					if (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight ||
					    Main.LocalPlayer.ZoneUnderworldHeight)
						return MusicLoader.GetMusicSlot(_musicMod, "Sounds/Music/AstralInfectionUnderGround");
					else
						return MusicLoader.GetMusicSlot(_musicMod, "Sounds/Music/AstralInfection");
				return MusicID.Space;
			}
		}

	public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

		public override ModWaterStyle WaterStyle =>
			ModContent.Find<ModWaterStyle>("CalRD/AstralWater");

		public override string BestiaryIcon => "CalRD/BiomeManagers/AstralInfectionIcon";

		public override string MapBackground => "CalRD/Backgrounds/MapBackgrounds/AstralBG";
		
		public override string BackgroundPath => "CalRD/Backgrounds/MapBackgrounds/AstralBG";
		
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Astral Infection");
		}

		public override bool IsBiomeActive(Player player) => !player.ZoneDungeon &&
		                                                     (CalamityWorld.astralTiles > 950 ||
		                                                      (player.ZoneSnow && CalamityWorld.astralTiles >
			                                                      300));
		
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle
		{
			get
			{
				if (Main.LocalPlayer.ZoneSnow)
				{
					return ModContent.Find<ModSurfaceBackgroundStyle>(
						"CalRD/AstralSnowSurfaceBGStyle");
				}
				else if (Main.LocalPlayer.ZoneDesert && !Main.LocalPlayer.ZoneSnow)
				{
					return ModContent.Find<ModSurfaceBackgroundStyle>(
						"CalRD/AstralDesertSurfaceBGStyle");
				}
				else
				{
					return ModContent.Find<ModSurfaceBackgroundStyle>(
						"CalRD/AstralSurfaceBGStyle");
				}
			}
		}

		public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle 
		{ 
			get
			{
				if (Main.LocalPlayer.ZoneSnow)
				{
					return ModContent.Find<ModUndergroundBackgroundStyle>("CalRD/AstralUndergroundBGStyle"); // Could use its own unique background
				}
				return ModContent.Find<ModUndergroundBackgroundStyle>("CalRD/AstralUndergroundBGStyle");
			}
		}
}
}