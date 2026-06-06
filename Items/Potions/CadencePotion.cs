using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class CadencePotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cadance Potion");
/*
            Tooltip.SetDefault("Gives the cadance buff which reduces enemy aggro\n" +
                               "Increases life regen and increases max life by 25%\n" +
                               "Increases heart pickup range\n" +
                                "While this potion's buff is active, Regeneration Potion and Lifeforce Potion buffs are disabled");
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
            Item.buffType = ModContent.BuffType<Cadence>();
            Item.buffTime = 18000;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LovePotion);
            recipe.AddIngredient(ItemID.HeartreachPotion);
            recipe.AddIngredient(ItemID.LifeforcePotion);
            recipe.AddIngredient(ItemID.RegenerationPotion);
            recipe.AddIngredient(ItemID.CalmingPotion);
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
