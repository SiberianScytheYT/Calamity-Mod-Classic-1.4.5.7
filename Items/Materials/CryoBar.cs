using CalRD.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class CryoBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frigid Bar");
/*
            Tooltip.SetDefault("Cold to the touch");
*/
        }

        public override void SetDefaults()
        {
			Item.createTile = ModContent.TileType<FrigidBar>();
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = 5;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
        }
    }
}
