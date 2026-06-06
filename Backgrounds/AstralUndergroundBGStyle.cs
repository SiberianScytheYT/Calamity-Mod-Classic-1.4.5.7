using Terraria;
using Terraria.ModLoader;

namespace CalRD.Backgrounds
{
    public class AstralUndergroundBGStyle : ModUndergroundBackgroundStyle
    {
        public override void FillTextureArray(int[] textureSlots)
        {
            for (int i = 0; i <= 3; i++)
            {
                textureSlots[i] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Backgrounds/AstralUG" + i.ToString());
            }
        }
    }
}
