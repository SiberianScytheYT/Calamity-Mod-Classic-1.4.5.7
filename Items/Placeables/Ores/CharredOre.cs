using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Placeables.Ores
{
    public class CharredOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Charred Ore");
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.Ores.CharredOre>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 15);
            Item.rare = 6;
        }
    }
}
