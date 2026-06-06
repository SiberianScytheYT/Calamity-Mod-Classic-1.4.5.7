using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class Celestus : RogueWeapon
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Celestus");
/*
			Tooltip.SetDefault("Throws a scythe that splits into multiple scythes on enemy hits\n" +
			"Stealth strikes reverse direction and home in on enemies after returning to the player");
*/
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 1130;
			Item.knockBack = 6f;
			Item.useAnimation = Item.useTime = 20;
			Item.Calamity().rogue = true;
			Item.autoReuse = true;
			Item.shootSpeed = 25f;
			Item.shoot = ModContent.ProjectileType<CelestusBoomerang>();

			Item.width = Item.height = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.value = Item.buyPrice(platinum: 2, gold: 50);
			Item.rare = 10;
			Item.Calamity().customRarity = CalamityRarity.Violet;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
			{
				int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI);
				Main.projectile[stealth].Calamity().stealthStrike = true;
			}
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AccretionDisk>());
			recipe.AddIngredient(ModContent.ItemType<AlphaVirus>());
			recipe.AddIngredient(ModContent.ItemType<MoltenAmputator>());
			recipe.AddIngredient(ModContent.ItemType<FrostcrushValari>());
			recipe.AddIngredient(ModContent.ItemType<EnchantedAxe>());
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
			recipe.Register();
		}
	}
}
