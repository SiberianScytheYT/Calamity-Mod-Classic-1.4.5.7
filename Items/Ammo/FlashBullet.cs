using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Ammo
{
    public class FlashBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flash Round");
/*
            Tooltip.SetDefault("Gives off a concussive blast that confuses enemies in a large area for a short time");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 12;
            Item.height = 18;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1.15f;
            Item.value = 250;
            Item.rare = 1;
            Item.shoot = ModContent.ProjectileType<FlashBulletProj>();
            Item.shootSpeed = 12f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ItemID.Glass, 3);
            recipe.AddIngredient(ItemID.Grenade);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
