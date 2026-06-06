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
	public class EclipsesFall : RogueWeapon
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Eclipse's Fall");
/*
			Tooltip.SetDefault("When the sun goes dark, you will know judgment\n" +
			"Summons spears from the sky on hit\n" +
			"Stealth strikes impale enemies and summon a constant barrage of spears over time");
*/
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 1060;
			Item.knockBack = 3.5f;
			Item.useAnimation = Item.useTime = 21;
			Item.autoReuse = true;
			Item.Calamity().rogue = true;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<EclipsesFallMain>();

			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = Item.height = 72;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.value = Item.buyPrice(2, 50, 0, 0);
			Item.rare = 10;
			Item.Calamity().customRarity = CalamityRarity.Violet;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.Calamity().StealthStrikeAvailable())
			{
				type = ModContent.ProjectileType<EclipsesStealth>();
			}
			int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI);
			Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<NightsGaze>());
			recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 15);
			recipe.AddIngredient(ModContent.ItemType<CoreofCinder>(), 6);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
			recipe.Register();
		}
	}
}
