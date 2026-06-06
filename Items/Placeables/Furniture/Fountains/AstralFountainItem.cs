using Terraria;
using Terraria.ModLoader;
using CalRD.Tiles.Furniture.Fountains;
using Terraria.ID;

namespace CalRD.Items.Placeables.Furniture.Fountains
{
    public class AstralFountainItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Water Fountain");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 42;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 0;
            Item.createTile = ModContent.TileType<AstralFountainTile>();
        }
    }
}
