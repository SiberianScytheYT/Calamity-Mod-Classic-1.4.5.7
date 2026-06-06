using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class RuinousSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ruinous Soul");
/*
            Tooltip.SetDefault("A shard of the distant past");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 7, 0, 0);
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }
    }
}
