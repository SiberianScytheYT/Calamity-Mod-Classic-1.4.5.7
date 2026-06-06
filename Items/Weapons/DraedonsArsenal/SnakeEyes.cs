using CalRD.Items.Materials;
using CalRD.Items.Weapons.Summon;
using CalRD.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
	public class SnakeEyes : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Snake Eyes");
/*
			Tooltip.SetDefault("Surveillance drones equipped with a strong electric field which can be directed at enemies.\n" +
			"Summons a mechanical watcher that zaps and flies around enemies.");
*/
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.shootSpeed = 10f;
			Item.damage = 55;
			Item.mana = 12;
			Item.width = 38;
			Item.height = 24;
			Item.useTime = Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noMelee = true;
			Item.knockBack = 3f;

			Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.UseSound = SoundID.Item15;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SnakeEyesSummon>();
			Item.shootSpeed = 10f;
			Item.DamageType = DamageClass.Summon;

			modItem.UsesCharge = true;
			modItem.MaxCharge = 190f;
			modItem.ChargePerUse = 1f;
			modItem.ChargePerAltUse = 0f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 18);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 12);
			recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 8);
			recipe.AddIngredient(ItemID.LunarBar, 4);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
