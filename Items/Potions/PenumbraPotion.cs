using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class PenumbraPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Penumbra Potion");
/*
            Tooltip.SetDefault("Rogue stealth generates 15% faster while moving\n"
                + "At night, stealth also generates 15% faster while standing still\n"
                + "During an eclipse both boosts increase to 20%");
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
            Item.buffType = ModContent.BuffType<PenumbraBuff>();
            Item.buffTime = 18000;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<SolarVeil>(), 3);
            recipe.AddIngredient(ItemID.LunarTabletFragment, 2);
            recipe.AddIngredient(ModContent.ItemType<EssenceofChaos>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 30);
            recipe.AddIngredient(ModContent.ItemType<SolarVeil>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
