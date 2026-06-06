using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria.ID;

namespace CalRD.Items.Accessories
{
	public class DarkSunRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dark Sun Ring");
/*
            Tooltip.SetDefault("Contains the power of the dark sun\n" +
				"12% increase to damage and melee speed\n" +
                "+1 life regen, 15% increased pick speed, and +2 max minions\n" +
                "Increased minion knockback\n" +
                "During the day the player has +3 life regen\n" +
                "During the night the player has +30 defense\n" +
				"Both of these bonuses are granted during an eclipse");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.defense = 10;
            Item.lifeRegen = 1;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.darkSunRing = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 100);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
