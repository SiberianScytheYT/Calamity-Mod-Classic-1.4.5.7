using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class ArmoredShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Armored Shell");
        }

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 34;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 7, 0, 0);
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }
    }
}
