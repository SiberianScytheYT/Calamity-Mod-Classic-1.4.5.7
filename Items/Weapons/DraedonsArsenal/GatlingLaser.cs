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
	public class GatlingLaser : ModItem
	{
		// This is the amount of charge consumed every time the holdout projectile fires a laser.
		public const float HoldoutChargeUse = 0.0075f;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Gatling Laser");
/*
			Tooltip.SetDefault("Large laser cannon used primarily by Yharim's fleet and base defense force\n" +
							   "Deals less damage against enemies with high defense");
*/
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.width = 43;
			Item.height = 24;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 43;
			Item.knockBack = 1f;
			Item.useTime = 2;
			Item.useAnimation = 2;
			Item.noUseGraphic = true;
			Item.autoReuse = false;
			Item.channel = true;
			Item.mana = 6;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/GatlingLaserFireStart");
			Item.noMelee = true;

			Item.value = CalamityGlobalItem.Rarity8BuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.shoot = ModContent.ProjectileType<GatlingLaserProj>();
			Item.shootSpeed = 24f;

			modItem.UsesCharge = true;
			modItem.MaxCharge = 135f;
			modItem.ChargePerUse = 0f; // This weapon is a holdout. Charge is consumed by the holdout projectile.
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<GatlingLaserProj>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-20, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 15);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 15);
			recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 5);
			recipe.AddIngredient(ModContent.ItemType<InfectedArmorPlating>(), 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
