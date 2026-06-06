using CalRD.Items.Materials;
using CalRD.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
	public class StarSwallowerContainmentUnit : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Star Swallower Containment Unit");
/*
			Tooltip.SetDefault("Small novelties created to easily transport and fire plasma, strangely popular with humans.\n" +
			"Summons a biomechanical frog that vomits plasma onto enemies\n" +
			"Deals more damage against enemies with high defenses");
*/
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.shootSpeed = 10f;
			Item.damage = 24;
			Item.mana = 10;
			Item.width = 18;
			Item.height = 28;
			Item.useTime = Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noMelee = true;
			Item.knockBack = 2.25f;
			Item.value = CalamityGlobalItem.Rarity3BuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;
			Item.UseSound = SoundID.Item15;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<StarSwallowerSummon>();
			Item.shootSpeed = 10f;
			Item.DamageType = DamageClass.Summon;

			modItem.UsesCharge = true;
			modItem.MaxCharge = 50f;
			modItem.ChargePerUse = 0.8f;
			modItem.ChargePerAltUse = 0f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Point mouseTileCoords = Main.MouseWorld.ToTileCoordinates();
			if (!CalamityUtils.ParanoidTileRetrieval(mouseTileCoords.X, mouseTileCoords.Y).HasTile)
			{
				Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 8);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 4);
			recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 4);
			recipe.AddIngredient(ItemID.MeteoriteBar, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
