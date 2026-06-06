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
	public class GaussRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Gauss Rifle");
/*
			Tooltip.SetDefault("A large and slow weapon, the concussive force of its projectiles do well in clearing large groups.\n" +
			"Fires a devastating high velocity blast with absurd knockback");
*/
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.width = 112;
			Item.height = 36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 150;
			Item.knockBack = 30f;
			Item.useTime = Item.useAnimation = 32;
			Item.autoReuse = true;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/GaussWeaponFire");
			Item.noMelee = true;

			Item.value = CalamityGlobalItem.Rarity8BuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.shoot = ModContent.ProjectileType<GaussRifleBlast>();
			Item.shootSpeed = 27f;

			modItem.UsesCharge = true;
			modItem.MaxCharge = 135f;
			modItem.ChargePerUse = 0.1125f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<GaussRifleBlast>(), damage, Item.knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 18);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 12);
			recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 5);
			recipe.AddIngredient(ModContent.ItemType<InfectedArmorPlating>(), 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
