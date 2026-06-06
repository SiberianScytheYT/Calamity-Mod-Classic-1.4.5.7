using CalRD.Projectiles.Typeless;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Materials;
namespace CalRD.Items.Fishing.FishingRods
{
	public class VerstaltiteFishingRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Verstaltite Fishing Rod");
/*
            Tooltip.SetDefault("Increased fishing power when in the tundra.\n" +
				"The ancient alloy's prismatic qualities are perfect for attracting fish.");
*/
        }

        public override void SetDefaults()
        {
			Item.width = 24;
			Item.height = 28;
			Item.useAnimation = 8;
			Item.useTime = 8;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.fishingPole = 35;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<VerstaltiteBobber>();
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
