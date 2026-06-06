using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
	public class ApoctosisArray : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Apoctosis Array");
/*
			Tooltip.SetDefault("Fires ion blasts that speed up and then explode\n" +
				"The higher your mana the more damage they will do\n" +
				"Astral steroids can inhibit the potential of this weapon");
*/
		}

		public override void SetDefaults()
		{
			Item.width = 98;
			Item.damage = 55;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 12;
			Item.useAnimation = 7;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6.75f;
			Item.UseSound = SoundID.Item91;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.height = 34;
			Item.value = Item.buyPrice(1, 20, 0, 0);
			Item.rare = 10;
			Item.shoot = ModContent.ProjectileType<IonBlast>();
			Item.shootSpeed = 8f;
			Item.Calamity().customRarity = CalamityRarity.Turquoise;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-25, 0);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float manaAmount = (float)player.statMana * 0.01f;
			float damageMult = manaAmount;
			float injectionNerf = player.Calamity().astralInjection ? 0.6f : 1f;
			int projectile = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, (int)(damage * damageMult * injectionNerf), Item.knockBack, player.whoAmI);
			Main.projectile[projectile].scale = manaAmount * 0.375f;
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<IonBlaster>());
			recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
