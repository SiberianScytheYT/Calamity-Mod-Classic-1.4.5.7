using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using CalRD.Items.Fishing.AstralCatches;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class GravityNormalizerPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gravity Normalizer Potion");
/*
            Tooltip.SetDefault("Disables the low gravity of space and grants immunity to the distorted debuff");
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
            Item.buffType = ModContent.BuffType<GravityNormalizerBuff>();
            Item.buffTime = 18000;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GravitationPotion);
            recipe.AddIngredient(ModContent.ItemType<AstralJelly>());
            recipe.AddIngredient(ModContent.ItemType<AldebaranAlewife>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 10);
            recipe.AddIngredient(ModContent.ItemType<AstralJelly>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
