using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class SpiritGenerator : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spirit Glyph");
/*
            Tooltip.SetDefault("Whenever your minions hit an enemy you will gain a random buff\n" +
                "These buffs will either boost your defense, summon damage, or life regen for a while");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity2BuyPrice;
            Item.rare = 2;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.sGenerator = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Diamond, 5);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
