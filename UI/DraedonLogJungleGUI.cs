using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalRD.UI
{
    public class DraedonLogJungleGUI : DraedonsLogGUI
    {
        public override int TotalPages => 3;
        public override string GetTextByPage()
        {
            switch (Page)
            {
                case 0:
                    return CalamityUtils.GetTextValue("UI.JungleLog.PageOne");
                case 1:
                    return CalamityUtils.GetTextValue("UI.JungleLog.PageTwo");
                default:
                    return CalamityUtils.GetTextValue("UI.JungleLog.PageThree");
            }
        }
        public override Texture2D GetTextureByPage()
        {
            switch (Page)
            {
                case 0:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogJungleBiome", AssetRequestMode.ImmediateLoad).Value;
                case 1:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogPlagueCell", AssetRequestMode.ImmediateLoad).Value;
                default:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogPlaguebringerGoliath", AssetRequestMode.ImmediateLoad).Value;
            }
        }
    }
}
