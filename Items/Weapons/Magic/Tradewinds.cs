using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class Tradewinds : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tradewinds");
/*
            Tooltip.SetDefault("Casts fast moving sunlight feathers");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item7;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TradewindsProjectile>();
            Item.shootSpeed = 25f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 6);
            recipe.AddIngredient(ItemID.SunplateBlock, 5);
            recipe.AddIngredient(ItemID.Feather, 3);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
