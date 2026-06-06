using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Scorpion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scorpio");
/*
            Tooltip.SetDefault("BOOM\n" +
                "Right click to fire a nuke");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 58;
            Item.height = 26;
            Item.useTime = Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6.5f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<MiniRocket>();
            Item.useAmmo = AmmoID.Rocket;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

		public override float UseSpeedMultiplier(Player player)
		{
			if (player.altFunctionUse == 2)
                return 1 / 3f;
				//return 13f/42f;
			return 1f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<BigNuke>(), (int)(damage * 1.25), Item.knockBack, player.whoAmI, 0f, 0f);
                return false;
            }
            else
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<MiniRocket>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
                return false;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SnowmanCannon);
            recipe.AddIngredient(ItemID.GrenadeLauncher);
            recipe.AddIngredient(ItemID.RocketLauncher);
            recipe.AddIngredient(ItemID.FragmentVortex, 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
