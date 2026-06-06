using CalRD.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.Furniture.Trophies
{
    public class PolterghastTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Polterghast Trophy");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 50000;
            Item.rare = 1;
            Item.createTile = ModContent.TileType<BossTrophy>();
            Item.placeStyle = 17;
        }
    }
}
