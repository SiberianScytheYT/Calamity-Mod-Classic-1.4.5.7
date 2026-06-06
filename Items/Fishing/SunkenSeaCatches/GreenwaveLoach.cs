using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.SunkenSeaCatches
{
    public class GreenwaveLoach : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Greenwave Loach");
/*
            Tooltip.SetDefault("An endangered fish that is highly prized in the market");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 38;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = 3;
        }
    }
}
