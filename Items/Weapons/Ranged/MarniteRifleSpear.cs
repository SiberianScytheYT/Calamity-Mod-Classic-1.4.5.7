using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class MarniteRifleSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Marnite Bayonet");
/*
            Tooltip.SetDefault("The gun damages enemies that touch it");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 72;
            Item.height = 20;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2.25f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shootSpeed = 22f;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.PurificationPowder;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AnyGoldBar", 7);
            recipe.AddIngredient(ItemID.Granite, 5);
            recipe.AddIngredient(ItemID.Marble, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
