using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class BlightedLens : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blighted Lens");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 22;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 56);
            Item.rare = 5;
        }
    }
}
