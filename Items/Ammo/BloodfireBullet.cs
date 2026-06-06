using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Ammo
{
	public class BloodfireBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Bloodfire Bullet");
/*
			Tooltip.SetDefault("Accelerates your life regeneration on hit\n" + "Deals bonus damage based on your current life regeneration");
*/
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 30;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.knockBack = 4.5f;
			Item.value = Item.sellPrice(copper: 80);
			Item.rare = ItemRarityID.Red;
			Item.Calamity().customRarity = CalamityRarity.PureGreen;
			Item.shoot = ModContent.ProjectileType<BloodfireBulletProj>();
			Item.shootSpeed = 4.8f;
			Item.ammo = ItemID.MusketBall;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(333);
			recipe.AddIngredient(ModContent.ItemType<BloodstoneCore>());
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
