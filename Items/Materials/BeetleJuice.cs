using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class BeetleJuice : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Beetle Juice");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 16);
            Item.rare = 5;
        }
    }
}
