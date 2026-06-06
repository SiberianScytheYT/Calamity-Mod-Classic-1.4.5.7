using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Rogue
{
	public class MythrilKnife : RogueWeapon
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Mythril Knife");
/*
			Tooltip.SetDefault("Stealth strikes inflict a wide assortment of debuffs");
*/
		}

		public override void SafeSetDefaults()
		{
			Item.width = 12;
			Item.damage = 40;
			Item.noMelee = true;
			Item.consumable = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.knockBack = 1.75f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.height = 30;
			Item.maxStack = 999;
			Item.value = 1100;
			Item.rare = 4;
			Item.shoot = ModContent.ProjectileType<MythrilKnifeProjectile>();
			Item.shootSpeed = 12f;
			Item.Calamity().rogue = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.Calamity().StealthStrikeAvailable())
			{
				int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
				Main.projectile[stealth].Calamity().stealthStrike = true;
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(40);
			recipe.AddIngredient(ItemID.MythrilBar);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
