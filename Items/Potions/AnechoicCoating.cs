using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class AnechoicCoating : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Anechoic Coating");
/*
            Tooltip.SetDefault("Reduces creature's ability to detect you in the abyss");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.rare = 3;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<AnechoicCoatingBuff>();
            Item.buffTime = 7200;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 10);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
