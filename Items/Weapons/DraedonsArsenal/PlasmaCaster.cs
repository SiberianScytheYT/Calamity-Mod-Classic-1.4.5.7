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
	// still awkward that the item called Plasma Rifle is the same class and exact same tier as this item
	public class PlasmaCaster : ModItem
	{
		public const int BaseDamage = 1100;
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Plasma Caster");
/*
			Tooltip.SetDefault("Industrial tool used to fuse metal together with super-heated plasma\n" +
				"Deals more damage against enemies with high defenses\n" +
				"Right click for turbo mode");
*/
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.width = 62;
			Item.height = 30;
			Item.DamageType = DamageClass.Magic;
			Item.damage = BaseDamage;
			Item.knockBack = 7f;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.autoReuse = true;
			Item.mana = 24;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/PlasmaCasterFire");
			Item.noMelee = true;

			Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.shoot = ModContent.ProjectileType<PlasmaCasterShot>();
			Item.shootSpeed = 5f;

			modItem.UsesCharge = true;
			modItem.MaxCharge = 190f;
			modItem.ChargePerUse = 0.32f;
			modItem.ChargePerAltUse = 0.12f; // turbo mode is more energy inefficient
		}

		public override bool AltFunctionUse(Player player) => true;

		public override float UseSpeedMultiplier(Player player)
		{
			if (player.altFunctionUse == 2)
				return 3f;
			return 1f;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.altFunctionUse == 2)
				mult /= 3f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
			if (velocity1.Length() > 5f)
			{
				velocity1.Normalize();
				velocity1 *= 5f;
			}

			float SpeedX = velocity1.X + (float)Main.rand.Next(-3, 4) * 0.05f;
			float SpeedY = velocity1.Y + (float)Main.rand.Next(-3, 4) * 0.05f;
			float damageMult = 1f;
			float kbMult = 1f;
			if (player.altFunctionUse == 2)
			{
				SpeedX = velocity1.X + (float)Main.rand.Next(-15, 16) * 0.05f;
				SpeedY = velocity1.Y + (float)Main.rand.Next(-15, 16) * 0.05f;
				damageMult = 0.3333f;
				kbMult = 3f / 7f;
			}

			Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<PlasmaCasterShot>(), (int)(damage * damageMult), Item.knockBack * kbMult, player.whoAmI, 0f, 0f);
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
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
