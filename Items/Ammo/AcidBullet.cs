using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Ammo
{
    public class AcidBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Acid Round");
/*
            Tooltip.SetDefault("Explodes into acid that inflicts the plague\n" +
                "Does more damage the higher the target's defense");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = 1250;
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<AcidBulletProj>();
            Item.shootSpeed = 10f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(150);
            recipe.AddIngredient(ItemID.MusketBall, 150);
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
