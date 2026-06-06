using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Ammo
{
    public class RubberMortarRound : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rubber Mortar Round");
/*
            Tooltip.SetDefault("Large blast radius\n" +
                "Will destroy tiles on each bounce\n" +
                "Used by normal guns");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 20;
            Item.height = 14;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 7.5f;
            Item.value = 1000;
            Item.rare = 5;
            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<RubberMortarRoundProj>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(100);
            recipe.AddIngredient(ModContent.ItemType<MortarRound>(), 100);
            recipe.AddIngredient(ItemID.PinkGel, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
