using CalRD.Buffs.Potions;
using CalRD.Items.Fishing.SunkenSeaCatches;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class SoaringPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Soaring Potion");
/*
            Tooltip.SetDefault("Increases flight time and horizontal flight speed by 10%\n" +
				"Restores a fraction of your wing flight time after a true melee strike\n" +
				"The amount of flight time restored scales with your melee stats and weapon swing speed");
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
            Item.buffType = ModContent.BuffType<Soaring>();
            Item.buffTime = 14400;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.Feather);
            recipe.AddIngredient(ItemID.SoulofFlight);
            recipe.AddIngredient(ModContent.ItemType<SunkenSailfish>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 30);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
