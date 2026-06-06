using CalRD.Items.Materials;
using CalRD.Items.Placeables.Walls;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.FurnitureOccult
{
    public class OccultStone : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.SetNameOverride("Otherworldly Stone");
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureOccult.OccultStone>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(150);
            recipe.AddIngredient(ItemID.StoneBlock, 150);
            recipe.AddIngredient(ModContent.ItemType<DarkPlasma>());
            recipe.AddIngredient(ModContent.ItemType<ArmoredShell>());
            recipe.AddIngredient(ModContent.ItemType<TwistingNether>());
            recipe.AddIngredient(ItemID.Silk, 15);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<OccultStoneWall>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<OccultPlatform>(), 2);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
