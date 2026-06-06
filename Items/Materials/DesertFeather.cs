using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class DesertFeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Desert Feather");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(copper: 20);
            Item.rare = 1;
        }
    }
}
