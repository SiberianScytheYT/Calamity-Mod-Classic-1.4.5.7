using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Ammo
{
    public class ElysianArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Elysian Arrow");
/*
            Tooltip.SetDefault("Summons meteors from the sky on death");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 36;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = 2000;
            Item.shoot = ModContent.ProjectileType<ElysianArrowProj>();
            Item.shootSpeed = 10f;
            Item.ammo = AmmoID.Arrow;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(150);
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>());
            recipe.AddIngredient(ItemID.HolyArrow, 150);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
