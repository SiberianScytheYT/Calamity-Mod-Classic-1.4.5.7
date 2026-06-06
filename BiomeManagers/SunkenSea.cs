using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.BiomeManagers
{
	public class SunkenSea : ModBiome
	{
		Mod _musicMod = ModLoader.HasMod("CalamityModMusic") ? ModLoader.GetMod("CalamityModMusic") : null;
		public override int Music => (_musicMod != null) ? MusicLoader.GetMusicSlot(_musicMod, "Sounds/Music/SunkenSea") : MusicID.Temple;
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
			if (tile.WallType == Mod.Find<ModWall>("EutrophicSandWall").Type || tile.WallType == Mod.Find<ModWall>("NavystoneWall").Type)
		{
			inSunkenSea = true;
		}
		return CalamityWorld.sunkenSeaTiles > 150 || inSunkenSea;
		}
	}
}