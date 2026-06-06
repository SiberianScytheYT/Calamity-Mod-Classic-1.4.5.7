using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class Purge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nano Purge");
/*
            Tooltip.SetDefault("Fires a barrage of nano lasers");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.width = 20;
            Item.height = 12;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PurgeProj>();
            Item.shootSpeed = 24f;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FragmentVortex, 20);
            recipe.AddIngredient(ItemID.LaserMachinegun);
            recipe.AddIngredient(ItemID.Nanites, 100);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<PurgeProj>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
