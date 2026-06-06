using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class LivingShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Living Shard");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 80);
            Item.rare = 7;
        }
    }
}
