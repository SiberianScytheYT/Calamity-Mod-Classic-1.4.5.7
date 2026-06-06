using CalRD.Tiles.Furniture;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Placeables.Furniture.Trophies
{
    public class CryogenTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cryogen Trophy");
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
            Item.placeStyle = 3;
        }
    }
}
