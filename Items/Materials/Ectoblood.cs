using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class Ectoblood : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ectoblood");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 32;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 16);
            Item.rare = 7;
        }
    }
}
