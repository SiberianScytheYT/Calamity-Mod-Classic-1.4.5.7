using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class ForbiddenSun : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Sun");
/*
            Tooltip.SetDefault("Casts a fire orb that emits a gigantic explosion on death");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 33;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ForbiddenSunProjectile>();
            Item.shootSpeed = 9f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 6);
            recipe.AddIngredient(ItemID.LivingFireBlock, 50);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
