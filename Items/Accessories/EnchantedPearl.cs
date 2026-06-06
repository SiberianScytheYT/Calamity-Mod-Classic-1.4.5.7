using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;

namespace CalRD.Items.Accessories
{
    public class EnchantedPearl : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Enchanted Pearl");
/*
            Tooltip.SetDefault("Increases fishing skill\n" +
				"Increases chance to catch crates");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.fishingSkill += 10;
			player.Calamity().enchantedPearl = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FishingPotion);
            recipe.AddIngredient(ItemID.CratePotion, 8);
            recipe.AddRecipeGroup("Boss2Material", 5);
            recipe.AddIngredient(ModContent.ItemType<SeaPrism>(), 10);
            recipe.AddIngredient(ModContent.ItemType<VictideBar>(), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
