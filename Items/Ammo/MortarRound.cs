using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Ammo
{
    public class MortarRound : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mortar Round");
/*
            Tooltip.SetDefault("Large blast radius. Will destroy tiles\n" +
                "Used by normal guns");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 20;
            Item.height = 14;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 7.5f;
            Item.value = 500;
            Item.rare = 3;
            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<MortarRoundProj>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(100);
            recipe.AddIngredient(ItemID.RocketIV, 100);
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
