using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class HolyWrathPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Holy Wrath Potion");
/*
            Tooltip.SetDefault("Increases damage by 12% and increases movement and horizontal flight speed by 5%\n" +
                "Attacks inflict holy fire\n" +
                "While this potion's buff is active the Wrath Potion's buff is disabled");
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
            Item.buffType = ModContent.BuffType<HolyWrathBuff>();
            Item.buffTime = 10800;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WrathPotion);
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>());
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 40);
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
