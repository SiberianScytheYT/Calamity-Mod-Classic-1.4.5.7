using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables
{
    public class PlantyMush : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Planty Mush");
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.Abyss.PlantyMush>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 6);
            Item.rare = 3;
        }

		public override void CaughtFishStack(ref int stack)
		{
			stack = Main.rand.Next(5,16);
		}
    }
}
