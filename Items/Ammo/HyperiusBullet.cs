using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Ammo
{
    public class HyperiusBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hyperius Bullet");
/*
            Tooltip.SetDefault("Your enemies might have a bad time\n" +
                "Spawns additional bullets on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = 2000;
            Item.rare = 9;
            Item.shoot = ModContent.ProjectileType<HyperiusBulletProj>();
            Item.shootSpeed = 16f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(150);
            recipe.AddIngredient(ItemID.MusketBall, 150);
            recipe.AddIngredient(ModContent.ItemType<BarofLife>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
