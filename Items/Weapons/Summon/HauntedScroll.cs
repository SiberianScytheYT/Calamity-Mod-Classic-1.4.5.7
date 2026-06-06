using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
	public class HauntedScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Haunted Scroll");
/*
            Tooltip.SetDefault("Summons a stack of haunted dishes to fight for you");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.mana = 10;
            Item.width = 52;
            Item.height = 52;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HauntedDishes>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                position = Main.MouseWorld;
                velocity.X = 0;
                velocity.Y = 0;
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 30f);
				//projectile.ai[1] is attack cooldown.  Setting it here prevents immediate attacks
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup(RecipeGroupID.Wood, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.Bowl);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
