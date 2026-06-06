using CalRD.CalPlayer;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Waters
{
    public class SulphuricWater : ModWaterStyle
    {
        /*
        public override bool ChooseWaterStyle()
        {
            int biomeWidth;
            // Small world
            if (Main.maxTilesX == 4200)
            {
                biomeWidth = 270;
            }
            // Medium world
            else if (Main.maxTilesX == 6400)
            {
                biomeWidth = 365;
            }
            // Large world
            else
            {
                biomeWidth = 430;
            }
            biomeWidth += 25;
            bool inXZone = Main.LocalPlayer.Center.X < biomeWidth * 16f;
            if (!CalamityWorld.abyssSide)
                inXZone = Main.LocalPlayer.Center.X > Main.maxTilesX * 16f - biomeWidth * 16f;

            bool inYZone = Main.LocalPlayer.Center.Y < Main.rockLayer * 16f - 320 && Main.LocalPlayer.Center.Y >= 5800f;

			CalamityPlayer modPlayer = Main.LocalPlayer.Calamity();
			bool greenWater = (inXZone && inYZone) || modPlayer.ZoneSulphur;
            return greenWater;
        }
        */

        public override int ChooseWaterfallStyle() => ModContent.Find<ModWaterfallStyle>("SulphuricWaterflow").Slot;

        public override int GetSplashDust() => 101;

        public override int GetDropletGore() => 708;

        public override Color BiomeHairColor() => Color.Turquoise;
    }
}
