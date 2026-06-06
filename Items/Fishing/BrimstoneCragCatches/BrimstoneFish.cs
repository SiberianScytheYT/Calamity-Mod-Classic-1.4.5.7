using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.BrimstoneCragCatches
{
    public class BrimstoneFish : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimstone Fish"); //Future potion ingredient
/*
            Tooltip.SetDefault("Fire is a living being");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 8);
            Item.rare = 1;
        }
    }
}
