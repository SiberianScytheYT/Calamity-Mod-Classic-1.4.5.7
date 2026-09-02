using CalRD.Walls;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.BiomeManagers
{
	public class SunkenSea : ModBiome
	{
		public override int Music => MusicLoader.GetMusicSlot("CalRD/Sounds/Music/SunkenSea");
		public override string BestiaryIcon => "CalRD/BiomeManagers/SunkenSeaIcon";
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRD/SunkenSeaWater");
		public override string BackgroundPath => "CalRD/Backgrounds/MapBackgrounds/AbyssMap1";
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Sunken Sea");
		}
		public override bool IsBiomeActive(Player player)
		{
		bool inSunkenSea = false;
		int playerPosX = (int)player.Center.X / 16;
		int playerPosY = (int)player.Center.Y / 16;
		Tile tile = Framing.GetTileSafely(playerPosX, playerPosY);
		if (tile.WallType == ModContent.WallType<EutrophicSandWall>() || tile.WallType == ModContent.WallType<NavystoneWall>())
		{
			inSunkenSea = true;
		}
		return CalamityWorld.sunkenSeaTiles > 150 || inSunkenSea;
		}
	}
}