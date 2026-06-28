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
		Mod _musicMod = ModLoader.HasMod("CalamityModMusic") ? ModLoader.GetMod("CalamityModMusic") : null;

		public override int Music 
		{
			get
			{
				int music = Main.curMusic;
				bool acidRain = CalamityWorld.rainingAcid;
				if (acidRain)
				{
					if (_musicMod != null)
						music = MusicLoader.GetMusicSlot(_musicMod,
							CalamityWorld.downedPolterghast
								? "Sounds/Music/AcidRainTier3" // Acid Rain Tier 3
								: "Sounds/Music/AcidRainTier1"); // Acid Rain Tier 1 + 2
					else
						music = (CalamityWorld.downedPolterghast) ? MusicID.Monsoon : MusicID.OldOnesArmy;
				}
				else
					music = (_musicMod != null) ? MusicLoader.GetMusicSlot(_musicMod, "Sounds/Music/SulphurousSeaDay") : MusicID.Desert;
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