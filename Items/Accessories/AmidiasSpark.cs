using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class AmidiasSpark : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Amidias' Spark");
/*
            Tooltip.SetDefault("Taking damage releases a blast of sparks\n" +
                               "Sparks deal extra damage in Hardmode");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = 1;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.aSpark = true;
        }
    }
}
