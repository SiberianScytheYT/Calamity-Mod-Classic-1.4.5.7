using CalRD.Buffs.Potions;
using CalRD.Items.Fishing.BrimstoneCragCatches;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class CalamitasBrew : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Calamitas' Brew");
/*
            Tooltip.SetDefault("Adds abyssal flames to your melee projectiles and melee attacks\n" +
                               "Increases your movement speed by 15%");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.useTurn = true;
            Item.maxStack = 999;
            Item.rare = 3;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<AbyssalWeapon>();
            Item.buffTime = 36000;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.BottledWater, 3);
            recipe.AddIngredient(ModContent.ItemType<BrimstoneFish>());
            recipe.AddIngredient(ModContent.ItemType<CalamityDust>(), 3);
            recipe.AddTile(TileID.ImbuingStation);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 20);
            recipe.AddIngredient(ModContent.ItemType<CalamityDust>());
            recipe.AddTile(TileID.ImbuingStation);
            recipe.Register();
        }
    }
}
