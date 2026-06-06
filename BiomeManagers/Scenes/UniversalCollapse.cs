using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace CalamityModClassicPreTrailer.BiomeManagers.Scenes
{
	public class UniversalCollapse : ModSceneEffect
	{
		Mod _musicMod = ModLoader.HasMod("CalamityModMusic") ? ModLoader.GetMod("CalamityModMusic") : null;
 	    public override int Music => (_musicMod != null) ? MusicLoader.GetMusicSlot(_musicMod, "Sounds/Music/DevourerofGodsPhase2") : MusicID.LunarBoss;
		public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;

		public virtual bool SetSceneEffect(Player player) => CalamityWorld.DoGSecondStageCountdown <= 540 &&
		                                                     CalamityWorld.DoGSecondStageCountdown > 60;
		public override bool IsSceneEffectActive(Player player) => SetSceneEffect(player);
	}
}