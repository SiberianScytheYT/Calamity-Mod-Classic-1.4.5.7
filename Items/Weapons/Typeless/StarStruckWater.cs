using CalRD.Projectiles.Typeless;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Typeless
{
	public class StarStruckWater : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star Struck Water");
/*
            Tooltip.SetDefault("Spreads the astral infection to some blocks");
*/
        }

        public override void SetDefaults()
        {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 14f;
			Item.rare = 3;
			Item.damage = 20;
			Item.shoot = ModContent.ProjectileType<StarStruckWaterBottle>();
			Item.width = 18;
			Item.height = 20;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.knockBack = 3f;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.value = 200;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ItemID.BottledWater, 10);
            recipe.AddIngredient(ModContent.ItemType<AstralSand>());
            recipe.AddIngredient(ModContent.ItemType<AstralMonolith>());
            recipe.Register();
        }
    }
}
