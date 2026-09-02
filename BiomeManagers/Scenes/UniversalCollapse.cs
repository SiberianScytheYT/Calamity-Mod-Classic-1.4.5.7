using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace CalRD.BiomeManagers.Scenes
{
	public class UniversalCollapse : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot("CalRD/Sounds/Music/UniversalCollapse");
		public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;

		public virtual bool SetSceneEffect(Player player) => CalamityWorld.DoGSecondStageCountdown <= 540 &&
		                                                     CalamityWorld.DoGSecondStageCountdown > 60;
		public override bool IsSceneEffectActive(Player player) => SetSceneEffect(player);
	}
}