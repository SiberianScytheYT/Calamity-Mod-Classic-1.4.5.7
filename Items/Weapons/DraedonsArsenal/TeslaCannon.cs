using CalRD.Items.Materials;
using CalRD.Projectiles.DraedonsArsenal;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
	public class TeslaCannon : ModItem
	{
		private int BaseDamage = 16200;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Tesla Cannon");
/*
			Tooltip.SetDefault("Lightweight energy cannon that blasts an intense electrical beam that explodes\n" +
				"Beams can arc to nearby targets\n" +
				"Inflicts severe nervous system damage to organic targets");
*/
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.width = 78;
			Item.height = 28;
			Item.DamageType = DamageClass.Magic;
			Item.damage = BaseDamage;
			Item.knockBack = 10f;
			Item.useTime = 90;
			Item.useAnimation = 90;
			Item.autoReuse = true;
			Item.mana = 30;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/TeslaCannonFire");
			Item.noMelee = true;

			Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.shoot = ModContent.ProjectileType<TeslaCannonShot>();
			Item.shootSpeed = 5f;

			modItem.UsesCharge = true;
			modItem.MaxCharge = 250f;
			modItem.ChargePerUse = 0.9f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
			if (velocity1.Length() > 5f)
			{
				velocity1.Normalize();
				velocity1 *= 5f;
			}

			float SpeedX = velocity1.X + (float)Main.rand.Next(-1, 2) * 0.02f;
			float SpeedY = velocity1.Y + (float)Main.rand.Next(-1, 2) * 0.02f;

			Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<TeslaCannonShot>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-20, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 25);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 15);
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
			recipe.Register();
		}
	}
}
