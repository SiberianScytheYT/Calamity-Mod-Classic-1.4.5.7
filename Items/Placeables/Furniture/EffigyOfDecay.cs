using CalRD.Items.Materials;
using CalRD.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables.Furniture
{
    public class EffigyOfDecay : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Effigy of Decay");
/*
            Tooltip.SetDefault("When placed down, nearby players can breathe underwater\n" +
                               "This effect does not work in the abyss\n" +
                               "Nearby players are also immune to the sulphuric poisoning");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 9, 0, 0);
            Item.rare = 3;
            Item.createTile = ModContent.TileType<EffigyOfDecayPlaceable>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SulfuricScale>(), 20);
            recipe.AddRecipeGroup("IronBar", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
