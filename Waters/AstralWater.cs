using CalRD.CalPlayer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Waters
{
    public class AstralWater : ModWaterStyle
    {
        public override int ChooseWaterfallStyle() => ModContent.Find<ModWaterfallStyle>("AstralWaterflow").Slot;

        public override int GetSplashDust() => 52; //corruption water?

        public override int GetDropletGore() => Mod.Find<ModGore>("AstralWaterDroplet").Type;

        public override Color BiomeHairColor() => Color.MediumPurple;
    }
}
