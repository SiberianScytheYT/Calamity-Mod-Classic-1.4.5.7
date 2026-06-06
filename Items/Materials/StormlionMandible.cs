using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class StormlionMandible : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stormlion Mandible");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 1, copper: 40);
            Item.rare = 1;
        }
    }
}
