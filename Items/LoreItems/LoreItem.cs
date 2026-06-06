using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public abstract class LoreItem : ModItem
    {
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
