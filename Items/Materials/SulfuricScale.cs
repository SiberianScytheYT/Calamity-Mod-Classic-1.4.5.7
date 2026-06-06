using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class SulfuricScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sulphuric Scale");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 3);
            Item.rare = 1;
        }
    }
}
