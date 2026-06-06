using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class Bloodstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloodstone");
        }

        public override void SetDefaults()
        {
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 60);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }
    }
}
