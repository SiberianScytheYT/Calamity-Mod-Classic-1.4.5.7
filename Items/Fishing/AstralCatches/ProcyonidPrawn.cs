using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.AstralCatches
{
    public class ProcyonidPrawn : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Procyonid Prawn");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = 1;
        }
    }
}
