using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
	public class TeslaPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Tesla Potion");
/*
			Tooltip.SetDefault("Summons an aura of electricity that electrifies and slows enemies\n" +
				"Slowdown does not work on bosses\n" +
				"Reduces the duration of the Electrified debuff");
*/
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.useTurn = true;
			Item.maxStack = 999;
			Item.rare = 3;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = ModContent.BuffType<TeslaBuff>();
			Item.buffTime = 18000;
			Item.value = Item.buyPrice(0, 2, 0, 0);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BottledWater);
			recipe.AddIngredient(ModContent.ItemType<SeaPrism>());
			recipe.AddRecipeGroup("AnyGoldOre", 2);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BottledWater);
			recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 10);
			recipe.AddTile(TileID.AlchemyTable);
			recipe.Register();
		}
	}
}
