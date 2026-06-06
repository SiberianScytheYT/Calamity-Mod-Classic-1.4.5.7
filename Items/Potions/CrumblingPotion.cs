using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class CrumblingPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crumbling Potion");
/*
            Tooltip.SetDefault("Increases melee and rogue critical strike chance by 5%\n" +
                "Melee and rogue attacks break enemy armor");
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
            Item.buffType = ModContent.BuffType<ArmorCrumbling>();
            Item.buffTime = 18000;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(5);
            recipe.AddIngredient(ItemID.BottledWater, 5);
            recipe.AddIngredient(ModContent.ItemType<AncientBoneDust>());
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 20);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
