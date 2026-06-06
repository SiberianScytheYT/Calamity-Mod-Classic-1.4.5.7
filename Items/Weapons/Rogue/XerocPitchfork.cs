using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class XerocPitchfork : RogueWeapon
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Shard of Antumbra");
/*
			Tooltip.SetDefault("Stealth strikes leave homing stars in their wake");
*/
		}

		public override void SafeSetDefaults()
		{
			Item.width = 48;
			Item.damage = 280;
			Item.noMelee = true;
			Item.consumable = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 19;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 19;
			Item.knockBack = 8f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.height = 48;
			Item.maxStack = 999;
			Item.value = 10000;
			Item.rare = 9;
			Item.shoot = ModContent.ProjectileType<AntumbraShardProjectile>();
			Item.shootSpeed = 16f;
			Item.Calamity().rogue = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.Calamity().StealthStrikeAvailable())
			{
				int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI);
				Main.projectile[stealth].Calamity().stealthStrike = true;
			}
			return !player.Calamity().StealthStrikeAvailable();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(20);
			recipe.AddIngredient(ModContent.ItemType<MeldiateBar>());
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
