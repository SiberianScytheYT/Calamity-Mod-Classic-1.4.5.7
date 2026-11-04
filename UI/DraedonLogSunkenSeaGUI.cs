using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalRD.UI
{
    public class DraedonLogSunkenSeaGUI : DraedonsLogGUI
    {
        public override int TotalPages => 3;
        public override string GetTextByPage()
        {
            switch (Page)
            {
                case 0:
                    return CalamityUtils.GetTextValue("UI.SunkenLog.PageOne");
                case 1:
                    return CalamityUtils.GetTextValue("UI.SunkenLog.PageTwo");
                default:
                    return CalamityUtils.GetTextValue("UI.SunkenLog.PageThree");
            }
        }
        public override Texture2D GetTextureByPage()
        {
            switch (Page)
            {
                case 0:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogSunkenSeaBiome", AssetRequestMode.ImmediateLoad).Value;
                case 1:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogEutrophicRayGhostBell", AssetRequestMode.ImmediateLoad).Value;
                default:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogGiantClam", AssetRequestMode.ImmediateLoad).Value;
            }
        }
    }
}
