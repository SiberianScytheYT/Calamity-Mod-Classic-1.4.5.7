using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Ammo
{
    public class TerraArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terra Arrow");
/*
            Tooltip.SetDefault("Travels incredibly quickly and explodes into more arrows when it hits a certain velocity");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 36;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(0, 0, 0, 40);
            Item.rare = 7;
            Item.shoot = ModContent.ProjectileType<TerraArrowMain>();
            Item.shootSpeed = 15f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(250);
            recipe.AddIngredient(ModContent.ItemType<LivingShard>());
            recipe.AddIngredient(ItemID.WoodenArrow, 250);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
