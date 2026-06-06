using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class BloodstoneCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloodstone Core");
        }

        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 12;
            Item.maxStack = 999;
            Item.rare = 10;
            Item.value = Item.sellPrice(gold: 4);
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ModContent.ItemType<Bloodstone>(), 5);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 2);
            recipe.AddIngredient(ModContent.ItemType<Phantoplasm>());
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.Register();
        }
    }
}
