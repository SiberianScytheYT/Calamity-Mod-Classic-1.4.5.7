using CalRD.CalPlayer;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.BiomeManagers
{
	public class Sulphur : ModBiome
	{
		public override int Music 
		{
			get
			{
				int music = Main.curMusic;
				bool acidRain = CalamityWorld.rainingAcid;
				if (acidRain)
					music = MusicLoader.GetMusicSlot(
							CalamityWorld.downedPolterghast
								? "CalRD/Sounds/Music/AcidRain2" // Acid Rain Tier 3
								: "CalRD/Sounds/Music/AcidRain1"); // Acid Rain Tier 1 + 2
				else
					music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/Sulphur");
				return music;
			}
		}

	public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
		
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRD/SulphuricWater");
		
		public override string BestiaryIcon => "CalRD/BiomeManagers/SulpherousSeaIcon";

		public override string MapBackground => "CalRD/Backgrounds/MapBackgrounds/SulphurBG";
		
		public override string BackgroundPath => "CalRD/Backgrounds/MapBackgrounds/SulphurBG";
		
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("CalRD/SulphurSeaSurfaceBGStyle");

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Sulphurous Sea");
		}
		
		public override bool IsBiomeActive(Player player)
		{
			Point point = player.Center.ToTileCoordinates();
			bool sulphurPosX = false;
			if (CalamityWorld.abyssSide)
			{
				if (point.X < 380)
				{
					sulphurPosX = true;
				}
			}
			else
			{
				if (point.X > Main.maxTilesX - 380)
				{
					sulphurPosX = true;
				}
			}
			return (CalamityWorld.sulphurTiles > 30 || (player.ZoneOverworldHeight && sulphurPosX)) && !player.GetModPlayer<CalamityPlayer>().ZoneAbyss;
		}
	}
}