using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Placeables.Ores
{
    public class AuricOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Auric Ore");
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.Ores.AuricOre>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 10;
            Item.height = 10;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 2);
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }
    }
}
