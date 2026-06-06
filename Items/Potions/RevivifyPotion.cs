using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using CalRD.Items.Fishing.SunkenSeaCatches;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class RevivifyPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Revivify Potion");
/*
            Tooltip.SetDefault("Causes enemy attacks to heal you for a fraction of their damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 36;
            Item.useTurn = true;
            Item.maxStack = 999;
            Item.rare = 3;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
			Item.buffType = ModContent.BuffType<Revivify>();
			Item.buffTime = 3600;
			Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(5);
            recipe.AddIngredient(ItemID.HolyWater, 5);
            recipe.AddIngredient(ModContent.ItemType<Stardust>(), 20);
            recipe.AddIngredient(ItemID.CrystalShard, 5);
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>(), 3);
            recipe.AddIngredient(ModContent.ItemType<ScarredAngelfish>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            recipe = CreateRecipe(5);
            recipe.AddIngredient(ItemID.HolyWater, 5);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 50);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
