using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.BiomeManagers
{
	public class Astral : ModBiome
	{
		public override int Music {
			get
			{
				if (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight ||
					    Main.LocalPlayer.ZoneUnderworldHeight)
						return MusicLoader.GetMusicSlot("CalRD/Sounds/Music/AstralUnderground");
				return MusicLoader.GetMusicSlot("CalRD/Sounds/Music/Astral");
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