using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
	public class WulfrumStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Wulfrum Staff");
/*
			Tooltip.SetDefault("Casts a wulfrum bolt");
*/
			//Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 4;
			Item.width = 44;
			Item.height = 46;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = 1;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<WulfrumBolt>();
			Item.shootSpeed = 9f;
		}

		//public override Vector2? HoldoutOrigin() => new Vector2(8, 15);

		public override Vector2? HoldoutOffset() => new Vector2(-3, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<WulfrumShard>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
