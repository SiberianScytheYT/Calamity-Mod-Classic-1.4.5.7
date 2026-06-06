using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class FrostBolt : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Bolt");
/*
            Tooltip.SetDefault("Casts a slow-moving ball of frost");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FrostBoltProjectile>();
            Item.shootSpeed = 6f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AnyIceBlock", 20);
            recipe.AddIngredient(ItemID.Shiverthorn, 2);
            recipe.AddRecipeGroup("AnySnowBlock", 10);
            recipe.AddIngredient(ItemID.WaterBucket);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
