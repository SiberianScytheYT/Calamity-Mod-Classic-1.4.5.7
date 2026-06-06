using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
	public class AstrealDefeat : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astreal Defeat");
/*
            Tooltip.SetDefault("Ethereal bow of the tyrant king's mother\n" +
                       "The mother strongly discouraged acts of violence throughout her life\n" +
                       "Though she kept this bow close to protect her family in times of great disaster\n" +
					   "Fires Astreal Arrows that emit flames as they travel");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 140;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 78;
            Item.useTime = 4;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item102;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AstrealArrow>();
            Item.shootSpeed = 4f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
			if (velocity1.Length() > 8f)
			{
				velocity1.Normalize();
				velocity1 *= 8f;
			}

			float ai0 = (float)Main.rand.Next(4);
            Projectile.NewProjectile(source, position.X, position.Y, velocity1.X, velocity1.Y, ModContent.ProjectileType<AstrealArrow>(), damage, Item.knockBack, player.whoAmI, ai0, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpiritFlame);
            recipe.AddIngredient(ItemID.ShadowFlameBow);
            recipe.AddIngredient(ModContent.ItemType<GreatbowofTurmoil>());
            recipe.AddIngredient(ModContent.ItemType<BladedgeGreatbow>());
            recipe.AddIngredient(ModContent.ItemType<DarkechoGreatbow>());
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
