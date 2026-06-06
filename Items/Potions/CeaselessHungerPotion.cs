using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class CeaselessHungerPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ceaseless Hunger Potion");
/*
            Tooltip.SetDefault("Causes you to suck up all items in the world");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useTurn = true;
            Item.maxStack = 999;
            Item.rare = 3;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<CeaselessHunger>();
            Item.buffTime = 600;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ItemID.BottledWater, 4);
            recipe.AddIngredient(ModContent.ItemType<DarkPlasma>());
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            recipe = CreateRecipe(4);
            recipe.AddIngredient(ItemID.BottledWater, 4);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 20);
            recipe.AddIngredient(ModContent.ItemType<DarkPlasma>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
