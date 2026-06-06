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
	public class PulseRifle : ModItem
	{
		private int BaseDamage = 10780;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Pulse Rifle");
/*
			Tooltip.SetDefault("Draedon's former pulse rifle, used in emergencies for creations which turned against him.\n" +
				"When the pulse hits a target it will arc to another nearby target\n" +
				"Inflicts exceptional damage against inorganic targets");
*/
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.width = 62;
			Item.height = 22;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = BaseDamage;
			Item.knockBack = 0f;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.autoReuse = true;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/PulseRifleFire");
			Item.noMelee = true;

			Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.shoot = ModContent.ProjectileType<PulseRifleShot>();
			Item.shootSpeed = 5f;

			modItem.UsesCharge = true;
			modItem.MaxCharge = 250f;
			modItem.ChargePerUse = 0.24f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
			if (velocity1.Length() > 5f)
			{
				velocity1.Normalize();
				velocity1 *= 5f;
			}

			float SpeedX = velocity1.X + (float)Main.rand.Next(-1, 2) * 0.05f;
			float SpeedY = velocity1.Y + (float)Main.rand.Next(-1, 2) * 0.05f;

			Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<PulseRifleShot>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 20);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 20);
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
			recipe.Register();
		}
	}
}
