using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class PhotosynthesisPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Photosynthesis Potion");
/*
            Tooltip.SetDefault("You regen life quickly while not moving, this effect is five times as strong during daytime\n" +
                "Dropped hearts heal more HP");
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
            Item.buffType = ModContent.BuffType<PhotosynthesisBuff>();
            Item.buffTime = 18000;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<BeetleJuice>(), 2);
            recipe.AddIngredient(ItemID.Vine);
            recipe.AddIngredient(ModContent.ItemType<TrapperBulb>());
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 40);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
