using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class AncientBoneDust : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ancient Bone Dust");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = 1;
        }
    }
}
