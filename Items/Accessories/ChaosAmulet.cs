using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class ChaosAmulet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Chaos Amulet");
/*
            Tooltip.SetDefault("Spelunker effect and increased life regen");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 24;
            Item.lifeRegen = 2;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;
            Item.rare = 4;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.findTreasure = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<EssenceofChaos>(), 7);
            recipe.AddIngredient(ItemID.SpelunkerPotion, 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
