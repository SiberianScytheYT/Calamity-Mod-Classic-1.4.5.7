using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class VictoryShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Victory Shard");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 3);
            Item.rare = 1;
        }
    }
}
