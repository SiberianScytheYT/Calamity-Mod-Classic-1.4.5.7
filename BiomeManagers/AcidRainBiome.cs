using Terraria.ModLoader;

namespace CalRD.BiomeManagers
{
    // bestiary classification biome, nothing more, nothing less
    public class AcidRainBiome : ModBiome
    {
        public override string BestiaryIcon => "CalRD/BiomeManagers/AcidRainIcon";
        
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Acid Rain");
        }
    }
}
