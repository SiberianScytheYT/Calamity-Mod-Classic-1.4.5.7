using CalRD.Projectiles.Typeless;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Materials;
namespace CalRD.Items.Fishing.FishingRods
{
	public class WulfrumRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wulfrum Fishing Pole");
/*
            Tooltip.SetDefault("This barely works, but it's better than nothing.");
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
			Item.rare = 1;
			Item.fishingPole = 10;
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<WulfrumBobber>();
			Item.value = Item.buyPrice(0, 1, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<WulfrumShard>(), 9);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
