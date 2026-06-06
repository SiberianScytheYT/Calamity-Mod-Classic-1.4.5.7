using CalRD.Items.Materials;
using CalRD.Projectiles.DraedonsArsenal;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
	public class GaussPistol : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Gauss Pistol");
/*
			Tooltip.SetDefault("A simple pistol that utilizes magic power; a weapon for the more magically adept.\n" +
			"Fires a devastating high velocity blast with extreme knockback");
*/
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.width = 40;
			Item.height = 22;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 6;
			Item.damage = 110;
			Item.knockBack = 11f;
			Item.useTime = Item.useAnimation = 20;
			Item.autoReuse = true;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/GaussWeaponFire");
			Item.noMelee = true;

			Item.value = CalamityGlobalItem.Rarity5BuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.shoot = ModContent.ProjectileType<GaussPistolShot>();
			Item.shootSpeed = 14f;

			modItem.MaxCharge = 85f;
			modItem.UsesCharge = true;
			modItem.ChargePerUse = 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 12);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 8);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.SoulofMight, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
