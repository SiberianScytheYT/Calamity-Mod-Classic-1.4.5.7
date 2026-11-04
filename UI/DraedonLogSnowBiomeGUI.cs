using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalRD.UI
{
    public class DraedonLogSnowBiomeGUI : DraedonsLogGUI
    {
        public override int TotalPages => 3;
        public override string GetTextByPage()
        {
            switch (Page)
            {
                case 0:
                    return CalamityUtils.GetTextValue("UI.SnowLog.PageOne");
                case 1:
                    return CalamityUtils.GetTextValue("UI.SnowLog.PageTwo");
                default:
                    return CalamityUtils.GetTextValue("UI.SnowLog.PageThree");
            }
        }
        public override Texture2D GetTextureByPage()
        {
            switch (Page)
            {
                case 0:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogIceBiome", AssetRequestMode.ImmediateLoad).Value;
                case 1:
                    return null; // No image exists for Daedalus' ruins at the moment.
                default:
                    return ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/DraedonsLogPermafrost", AssetRequestMode.ImmediateLoad).Value;
            }
        }
    }
}
