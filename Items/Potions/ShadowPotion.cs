using CalRD.Buffs.Potions;
using CalRD.Items.Fishing.BrimstoneCragCatches;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class ShadowPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shadow Potion");
/*
            Tooltip.SetDefault("Causes the player to disappear while not attacking\n" +
			"Holding different types of rogue weapons give the player boosts\n" +
			"Different types of rogue weapons spawn different projectiles on hit\n" +
			"Stealth generation is increased by 10%");
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
            Item.useStyle = 2;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<ShadowBuff>();
            Item.buffTime = 18000; //5 minutes
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ModContent.ItemType<Shadowfish>());
            recipe.AddIngredient(ItemID.InvisibilityPotion);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 10);
            recipe.AddIngredient(ItemID.InvisibilityPotion);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
