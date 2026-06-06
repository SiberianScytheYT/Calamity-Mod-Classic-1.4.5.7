using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using CalRD.Items.Fishing.SunkenSeaCatches;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class TitanScalePotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Titan Scale Potion");
/*
            Tooltip.SetDefault("Increases knockback, defense by 5, and damage reduction by 5%\n" +
				"Increases defense by 25 and damage reduction by 10% for a few seconds after a true melee strike");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 34;
            Item.useTurn = true;
            Item.maxStack = 999;
            Item.rare = 3;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<TitanScale>();
            Item.buffTime = 18000;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ItemID.TitanPotion, 4);
            recipe.AddIngredient(ItemID.BeetleHusk);
            recipe.AddIngredient(ModContent.ItemType<CoralskinFoolfish>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            recipe = CreateRecipe(4);
            recipe.AddIngredient(ItemID.BottledWater, 4);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 40);
            recipe.AddIngredient(ItemID.BeetleHusk);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
