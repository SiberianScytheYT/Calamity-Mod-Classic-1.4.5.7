using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class SolarVeil : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Solar Veil");
/*
            Tooltip.SetDefault("Sunlight cannot penetrate the fabric of this cloth");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 80);
            Item.rare = 6;
        }
    }
}
