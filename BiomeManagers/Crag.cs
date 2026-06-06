using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.BiomeManagers
{
	public class Crag : ModBiome
	{
		Mod _musicMod = ModLoader.HasMod("CalamityModMusic") ? ModLoader.GetMod("CalamityModMusic") : null;

		public override int Music => (_musicMod != null) ? MusicLoader.GetMusicSlot(_musicMod, "Sounds/Music/BrimstoneCrags") : MusicID.Eerie;
		
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		
		public override string BestiaryIcon => "CalRD/BiomeManagers/BrimstoneCragsIcon";
		
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Brimstone Crags");
		}
		
		public override bool IsBiomeActive(Player player) => CalamityWorld.calamityTiles > 50;
	}
}