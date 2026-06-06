using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
	public class UltraLiquidator : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Ultra Liquidator");
/*
			Tooltip.SetDefault("Summons liquidation blades that summon more blades on enemy hits\n" +
							   "The blades inflict ichor, cursed inferno, on fire, and frostburn");
*/
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 145;
			Item.crit = 30;
			Item.knockBack = 7f;
			Item.useTime = 3;
			Item.reuseDelay = Item.useAnimation = 15;
			Item.mana = 25;
			Item.DamageType = DamageClass.Magic;
			Item.autoReuse = true;
			Item.shootSpeed = 16f;
			Item.shoot = ModContent.ProjectileType<LiquidBlade>();

			Item.width = Item.height = 16;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item9;
			Item.value = Item.buyPrice(1, 20, 0, 0);
			Item.rare = 10;
			Item.Calamity().customRarity = CalamityRarity.Turquoise;
		}

		public override Vector2? HoldoutOrigin() => new Vector2(15, 15);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<InfernalRift>());
			recipe.AddRecipeGroup("CursedFlameIchor", 20);
			recipe.AddIngredient(ItemID.AquaScepter);
			recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
			recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 playerPos = player.RotatedRelativePoint(player.MountedCenter, true);
			float speed = Item.shootSpeed;
			float xVec = (float)Main.mouseX + Main.screenPosition.X - playerPos.X;
			float yVec = (float)Main.mouseY + Main.screenPosition.Y - playerPos.Y;
			float f = Main.rand.NextFloat() * MathHelper.TwoPi;
			float lowerBoundOffset = 20f;
			float upperBoundOffset = 60f;
			Vector2 source1 = playerPos + f.ToRotationVector2() * MathHelper.Lerp(lowerBoundOffset, upperBoundOffset, Main.rand.NextFloat());
			for (int i = 0; i < 50; i++)
			{
				source1 = playerPos + f.ToRotationVector2() * MathHelper.Lerp(lowerBoundOffset, upperBoundOffset, Main.rand.NextFloat());
				if (Collision.CanHit(playerPos, 0, 0, source1 + (source1 - playerPos).SafeNormalize(Vector2.UnitX) * 8f, 0, 0))
				{
					break;
				}
				f = Main.rand.NextFloat() * MathHelper.TwoPi;
			}
			Vector2 velocity1 = Main.MouseWorld - source1;
			Vector2 upperVelocityLimit = new Vector2(xVec, yVec).SafeNormalize(Vector2.UnitY) * speed;
			velocity1 = velocity1.SafeNormalize(upperVelocityLimit) * speed;
			velocity1 = Vector2.Lerp(velocity1, upperVelocityLimit, 0.25f);
			Projectile.NewProjectile(source, source1, velocity1, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}
