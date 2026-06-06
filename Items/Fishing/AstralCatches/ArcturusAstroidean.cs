using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.AstralCatches
{
    public class ArcturusAstroidean : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Arcturus Astroidean");
/*
            Tooltip.SetDefault("Increases fishing power if used in the Astral Infection or Sulphurous Sea");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 10);
            Item.rare = 3;
            Item.bait = 40;
        }
    }
}
