using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Dualpoon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dualpoon");
/*
            Tooltip.SetDefault("Launches two harpoons");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 56;
            Item.height = 28;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6.5f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item10;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<DualpoonProj>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 source1 = player.RotatedRelativePoint(player.MountedCenter, true);
			float piOverTen = MathHelper.Pi * 0.1f;
			int projCount = 2;
			Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
			velocity1.Normalize();
			velocity1 *= 40f;
			bool canHit = Collision.CanHit(source1, 0, 0, source1 + velocity1, 0, 0);
			for (int projIndex = 0; projIndex < projCount; projIndex++)
			{
				float num120 = projIndex - (projCount - 1f) / 2f;
				Vector2 offset = velocity1.RotatedBy((double)(piOverTen * num120), default);
				if (!canHit)
				{
					offset -= velocity1;
				}
				Projectile.NewProjectile(source, source1.X + offset.X, source1.Y + offset.Y, velocity1.X, velocity1.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
			}
			return false;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Harpoon, 2);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
