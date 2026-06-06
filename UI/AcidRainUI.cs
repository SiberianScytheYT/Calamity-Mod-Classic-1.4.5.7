using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.UI
{
    public class AcidRainUI : InvasionProgressUI
    {
        public override bool IsActive => CalamityWorld.rainingAcid && Main.LocalPlayer.Calamity().ZoneSulphur;
        public override float CompletionRatio => 1f - CalamityWorld.AcidRainCompletionRatio;
        public override string InvasionName => "Acid Rain";
        public override Color InvasionBarColor => AcidRainEvent.TextColor;
        public override Texture2D IconTexture => ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/AcidRainIcon").Value;
    }
}
