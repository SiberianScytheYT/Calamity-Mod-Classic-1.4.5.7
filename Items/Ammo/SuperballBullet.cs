using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Ammo
{
    public class SuperballBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Superball Bullet");
/*
            Tooltip.SetDefault("Bounces at extreme speeds");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = 250;
            Item.rare = 1;
            Item.shoot = ModContent.ProjectileType<SuperballBulletProj>();
            Item.shootSpeed = 1f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(150);
            recipe.AddIngredient(ItemID.MeteorShot, 150);
            recipe.AddIngredient(ModContent.ItemType<VictoryShard>());
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
