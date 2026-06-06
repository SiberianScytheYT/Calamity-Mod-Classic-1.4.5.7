using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.BrimstoneCragCatches
{
    public class CharredLasher : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Charred Lasher");
/*
            Tooltip.SetDefault("This elusive fish is a prized commodity");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 36;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = 3;
        }
    }
}
