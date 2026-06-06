using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class RubicoPrime : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rubico Prime");
/*
            Tooltip.SetDefault("Semi-automatic sniper that fires in 5 second bursts\n" +
                "Fires impact rounds that have an increased crit multiplier and deal bonus damage to inorganic targets");
*/
				//would do less to organic targets if like this wasn't meant to be used against yharon lole
        }

        public override void SetDefaults()
        {
            Item.damage = 2601;
            Item.DamageType = DamageClass.Ranged;
            Item.crit += 40;
            Item.knockBack = 10f;
            Item.useTime = 30;
            Item.useAnimation = 300;
            Item.autoReuse = false;

            Item.useStyle = 5;
            Item.noMelee = true;
            Item.width = 82;
            Item.height = 28;

            Item.shoot = 10;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;

            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PestilentDefiler>());
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 10);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ImpactRound>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
