using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
	public class IonBlaster : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Ion Blaster");
/*
			Tooltip.SetDefault("Fires ion blasts that speed up and then explode\n" +
				"The higher your mana the more damage they will do\n" +
				"Astral steroids can inhibit the potential of this weapon");
*/
		}

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.damage = 30;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 6;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5.5f;
			Item.UseSound = SoundID.Item91;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.height = 28;
			Item.value = Item.buyPrice(0, 36, 0, 0);
			Item.rare = 5;
			Item.shoot = ModContent.ProjectileType<IonBlast>();
			Item.shootSpeed = 3f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-5, 0);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float manaAmount = (float)player.statMana * 0.01f;
			float damageMult = manaAmount * 0.75f;
			float injectionNerf = player.Calamity().astralInjection ? 0.6f : 1f;
			int projectile = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, (int)(damage * damageMult * injectionNerf), Item.knockBack, player.whoAmI);
			Main.projectile[projectile].scale = manaAmount;
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofFright, 10);
			recipe.AddRecipeGroup("AnyAdamantiteBar", 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
