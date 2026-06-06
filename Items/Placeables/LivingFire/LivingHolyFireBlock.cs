using CalRD.Items.Materials;
using CalRD.Tiles.LivingFire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables.LivingFire
{
	public class LivingHolyFireBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Living Holy Fire Block");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<LivingHolyFireBlockTile>();
        }

        public override void PostUpdate()
        {
            Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 1f, 1f, 0f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(20);
            recipe.AddIngredient(ItemID.LivingFireBlock, 20);
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>());
            recipe.Register();
        }
    }
}
