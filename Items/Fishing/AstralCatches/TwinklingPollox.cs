using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.AstralCatches
{
    public class TwinklingPollox : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Twinkling Pollox"); //Bass substitute
/*
            Tooltip.SetDefault("The scales gleam like crystals");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 28;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = 1;
        }
    }
}
