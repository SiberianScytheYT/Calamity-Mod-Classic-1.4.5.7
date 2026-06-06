using CalRD.Items.Materials;
using CalRD.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
	public class LaserRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Heavy Laser Rifle");
/*
			Tooltip.SetDefault("Laser weapon used by heavy infantry units in Yharim's army\n" +
							   "Deals less damage against enemies with high defense");
*/
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.width = 84;
			Item.height = 28;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 270;
			Item.knockBack = 4f;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.autoReuse = true;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/LaserRifleFire");
			Item.noMelee = true;

			Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.shoot = ModContent.ProjectileType<LaserRifleShot>();
			Item.shootSpeed = 5f;

			modItem.UsesCharge = true;
			modItem.MaxCharge = 190f;
			modItem.ChargePerUse = 0.125f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
			if (velocity1.Length() > 5f)
			{
				velocity1.Normalize();
				velocity1 *= 5f;
			}
			for (int i = 0; i < 2; i++)
			{
				float SpeedX = velocity1.X + Main.rand.Next(-1, 2) * 0.05f;
				float SpeedY = velocity1.Y + Main.rand.Next(-1, 2) * 0.05f;
				Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<LaserRifleShot>(), damage, Item.knockBack, player.whoAmI, i, 0f);
			}
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-20, 0);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 15);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 15);
			recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 8);
			recipe.AddIngredient(ItemID.LunarBar, 4);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
