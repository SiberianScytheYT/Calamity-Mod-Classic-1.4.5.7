using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalRD.UI
{
    public class DraedonLogPlanetoidGUI : DraedonsLogGUI
    {
        public override int TotalPages => 3;
        public override string GetTextByPage()
        {
            switch (Page)
            {
                case 0:
                    return CalamityUtils.GetTextValue("UI.PlanetoidLog.PageOne");
                case 1:
                    return CalamityUtils.GetTextValue("UI.PlanetoidLog.PageTwo");
                default:
                    return CalamityUtils.GetTextValue("UI.PlanetoidLog.PageThree");
            }
        }
        public override Texture2D GetTextureByPage()
        {
            switch (Page)
            {
                case 0:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogPlanetoid", AssetRequestMode.ImmediateLoad).Value;
                case 1:
                    return null; // The Draedon's backstory page does not have an image, and probably shouldn't.
                default:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogDoGArmor", AssetRequestMode.ImmediateLoad).Value;
            }
        }
    }
}
