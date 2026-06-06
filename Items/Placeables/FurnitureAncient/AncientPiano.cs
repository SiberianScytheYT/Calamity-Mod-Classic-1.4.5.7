using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.FurnitureAncient
{
    public class AncientPiano : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.SetNameOverride("Ancient Pipe Organ");
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 3;
            Item.consumable = true;
            Item.value = 0;
            Item.createTile = ModContent.TileType<Tiles.FurnitureAncient.AncientPiano>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.Bone, 4);
            recipe.AddIngredient(ModContent.ItemType<BrimstoneSlag>(), 15);
            recipe.AddIngredient(ItemID.Book);
            recipe.AddTile(ModContent.TileType<AncientAltar>());
            recipe.Register();
        }
    }
}
