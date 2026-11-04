using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalRD.UI
{
    public class DraedonLogHellGUI : DraedonsLogGUI
    {
        public override int TotalPages => 3;
        public override string GetTextByPage()
        {
            switch (Page)
            {
                case 0:
                    return CalamityUtils.GetTextValue("UI.HellLog.PageOne");
                case 1:
                    return CalamityUtils.GetTextValue("UI.HellLog.PageTwo");
                default:
                    return CalamityUtils.GetTextValue("UI.HellLog.PageThree");
            }
        }
        public override Texture2D GetTextureByPage()
        {
            switch (Page)
            {
                case 0:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogCragsBiome", AssetRequestMode.ImmediateLoad).Value;
                case 1:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogWallOfFlesh", AssetRequestMode.ImmediateLoad).Value;
                default:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogMurasamaPhaseslayer", AssetRequestMode.ImmediateLoad).Value;
            }
        }
    }
}
