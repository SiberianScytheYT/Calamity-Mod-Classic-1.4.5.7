using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class BloodSample : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Sample");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 22;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 0, 50, 0);
            Item.rare = 2;
        }
    }
}
