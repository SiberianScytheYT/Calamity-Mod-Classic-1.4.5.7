using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools
{
	public class BallAndChain : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Ball and Chain");
/*
			Tooltip.SetDefault("So heavy...\n" +
				"Favorite this item to disable any dashes granted by equipment.");
*/
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 50;
			Item.rare = ItemRarityID.Blue;
		}

		public override bool CanUseItem(Player player) => false;

		public override void UpdateInventory(Player player)
		{
			if (Item.favorited)
				player.Calamity().blockAllDashes = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 10);
			recipe.AddIngredient(ItemID.Chain);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
