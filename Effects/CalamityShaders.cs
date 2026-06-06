using CalRD.Skies;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalRD.Effects
{
    public class CalamityShaders
    {
        public static Effect AstralFogShader;
        public static Effect LightShader;
        public static Effect TentacleShader;
        public static Effect LightDistortionShader;
        
        public static void LoadShaders()
        {
            if (Main.dedServ)
                return;
            AstralFogShader = ModContent.Request<Effect>("CalRD/Effects/CustomShader", AssetRequestMode.ImmediateLoad).Value;
            LightShader = ModContent.Request<Effect>("CalRD/Effects/LightBurstShader", AssetRequestMode.ImmediateLoad).Value;
            TentacleShader = ModContent.Request<Effect>("CalRD/Effects/TentacleShader", AssetRequestMode.ImmediateLoad).Value;
            LightDistortionShader = ModContent.Request<Effect>("CalRD/Effects/DistortionShader", AssetRequestMode.ImmediateLoad).Value;

            Filters.Scene["CalRD:Astral"] = new Filter(new AstralScreenShaderData(new Ref<Effect>(AstralFogShader), "AstralPass").UseColor(0.18f, 0.08f, 0.24f), EffectPriority.VeryHigh);

            Filters.Scene["CalRD:LightBurst"] = new Filter(new ScreenShaderData(new Ref<Effect>(LightShader), "BurstPass"), EffectPriority.VeryHigh);
            Filters.Scene["CalRD:LightBurst"].Load();

            GameShaders.Misc["CalRD:SubsumingTentacle"] = new MiscShaderData(new Ref<Effect>(TentacleShader), "BurstPass");
            GameShaders.Misc["CalRD:LightDistortion"] = new MiscShaderData(new Ref<Effect>(LightDistortionShader), "DistortionPass");
        }
    }
}
