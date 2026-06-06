using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class ZenPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Zen Potion");
/*
            Tooltip.SetDefault("Vastly decreases enemy spawn rate");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useTurn = true;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<Zen>();
            Item.buffTime = 36000;
            Item.value = Item.buyPrice(gold: 2);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddConsumeItemCallback(Recipe.ConsumptionRules.Alchemy);
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 2);
            recipe.AddIngredient(ModContent.ItemType<EbonianGel>(), 2);
            recipe.AddIngredient(ItemID.PinkGel);
            recipe.AddIngredient(ItemID.Daybloom);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();

            // Alternative recipe is not "alchemy" because none of the blood orb recipes are
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 20);
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 2);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
