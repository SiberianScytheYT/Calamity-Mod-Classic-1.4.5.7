using CalRD.Items.Placeables.Walls;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables.Plates
{
    public class Cinderplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cinderplate");
/*
            Tooltip.SetDefault("It resonates with otherworldly energy.");
*/
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.Plates.Cinderplate>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 3);
            Item.rare = 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>(), 1);
            recipe.AddIngredient(ItemID.Obsidian, 3);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CinderplateWall>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
