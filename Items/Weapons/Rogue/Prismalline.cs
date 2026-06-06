using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class Prismalline : RogueWeapon
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Prismalline");
/*
			Tooltip.SetDefault("Throws daggers that split after a while\n" +
			"Stealth strikes additionally explode into prism shards and briefly stun enemies");
*/
		}

		public override void SafeSetDefaults()
		{
			Item.width = 46;
			Item.damage = 18;
			Item.crit += 4;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 16;
			Item.knockBack = 5f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.height = 46;
			Item.value = Item.buyPrice(0, 36, 0, 0);
			Item.rare = 5;
			Item.shoot = ModContent.ProjectileType<PrismallineProj>();
			Item.shootSpeed = 16f;
			Item.Calamity().rogue = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.Calamity().StealthStrikeAvailable())
			{
				int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
				Main.projectile[proj].Calamity().stealthStrike = true;
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Crystalline>());
			recipe.AddIngredient(ModContent.ItemType<MolluskHusk>(), 5);
			recipe.AddIngredient(ModContent.ItemType<SeaPrism>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
