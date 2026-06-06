using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.BrimstoneCragCatches
{
    public class CragBullhead : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crag Bullhead"); //Bass substitute
/*
            Tooltip.SetDefault("Its scales are scorching hot");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 36;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = 1;
        }
    }
}
