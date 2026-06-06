using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.BrimstoneCragCatches
{
    public class Shadowfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shadowfish"); //Future potion ingredient
/*
            Tooltip.SetDefault("Darkness spreads");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 8);
            Item.rare = 1;
        }
    }
}
