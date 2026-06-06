using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables.Furniture
{
    public class PurpleCandle : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Resilient Candle");
/*
            Tooltip.SetDefault("When placed, nearby players' defense blocks 5% more damage\n" +
                "'Neither rain nor wind can snuff its undying flame'");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 40;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = 6;
            Item.createTile = ModContent.TileType<Tiles.Furniture.PurpleCandle>();
        }
    }
}
