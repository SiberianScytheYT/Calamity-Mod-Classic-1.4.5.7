using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class DraconicElixir : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Draconic Elixir");
/*
            Tooltip.SetDefault("Greatly increases wing flight time and speed and increases defense by 16\n" +
                "God slayer revival heals you to half HP instead of 150 HP when triggered\n" +
                "Silva invincibility heals you to half HP when triggered\n" +
                "If you trigger the above heals you cannot drink this potion again for 60 seconds and you gain 30 seconds of potion sickness");
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
            Item.buffType = ModContent.BuffType<DraconicSurgeBuff>();
            Item.buffTime = 18000;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override bool CanUseItem(Player player) => !player.Calamity().draconicSurgeCooldown;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<HellcasterFragment>());
            recipe.AddIngredient(ItemID.Daybloom);
            recipe.AddIngredient(ItemID.Moonglow);
            recipe.AddIngredient(ItemID.Fireblossom);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 50);
            recipe.AddIngredient(ModContent.ItemType<HellcasterFragment>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
