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
	public class PulsePistol : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Pulse Pistol");
/*
			Tooltip.SetDefault("Fires a pulse that arcs to a new target on enemy hits\n" +
							   "Inflicts more damage to inorganic targets");
*/
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.width = 62;
			Item.height = 22;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 22;
			Item.knockBack = 0f;
			Item.useTime = Item.useAnimation = 20;
			Item.autoReuse = true;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/PulseRifleFire");
			Item.noMelee = true;

			Item.value = CalamityGlobalItem.Rarity3BuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.shoot = ModContent.ProjectileType<PulseRifleShot>();
			Item.shootSpeed = 5.2f; // This may seem low but the shot has 10 extra updates.

			modItem.UsesCharge = true;
			modItem.MaxCharge = 50f;
			modItem.ChargePerUse = 0.05f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<PulsePistolShot>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(10f, 0f);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 5);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 7);
			recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 4);
			recipe.AddIngredient(ItemID.MeteoriteBar, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
