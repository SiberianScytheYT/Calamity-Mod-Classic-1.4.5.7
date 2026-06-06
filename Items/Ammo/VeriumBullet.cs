using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Ammo
{
    public class VeriumBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Verium Bullet");
/*
            Tooltip.SetDefault("There is no escape!\n" +
			"Homes in after striking an enemy");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1.25f;
            Item.value = 500;
            Item.rare = 3;
            Item.shoot = ModContent.ProjectileType<VeriumBulletProj>();
            Item.shootSpeed = 16f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(100);
            recipe.AddIngredient(ItemID.MusketBall, 100);
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
