using CalRD.Items.Materials;
using CalRD.Items.Potions;
using CalRD.Tiles;
using CalRD.Tiles.Astral;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.Furniture
{
    public class AstralBeaconItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Beacon");
/*
            Tooltip.SetDefault("Summons Astrum Deus in exchange for specific offerings");
*/
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 26;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 14;
            Item.rare = 9;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<AstralBeacon>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AstralJelly>(), 5);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Stardust>(), 20);
            recipe.AddIngredient(ModContent.ItemType<AstralStone>(), 30);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
