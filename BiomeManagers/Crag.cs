using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.BiomeManagers
{
	public class Crag : ModBiome
	{
		public override int Music => MusicLoader.GetMusicSlot("CalRD/Sounds/Music/Crag");
		
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		
		public override string BestiaryIcon => "CalRD/BiomeManagers/BrimstoneCragsIcon";
		
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Brimstone Crags");
		}
		
		public override bool IsBiomeActive(Player player) => CalamityWorld.calamityTiles > 50;
	}
}