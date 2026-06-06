using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class SupremeBaitTackleBoxFishingStation : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Supreme Bait Tackle Box Fishing Station");
/*
            Tooltip.SetDefault("The ultimate fishing accessory\n" +
                "Increases fishing skill by 80\n" +
                "Fishing line will never break and decreases chance of bait consumption\n" +
				"Increases chance to catch crates\n" +
                "Sonar potion effect");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.fishingSkill += 80;
            player.accFishingLine = true;
            player.accTackleBox = true;
			player.Calamity().fishingStation = true;
            player.sonarPotion = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AnglerHat);
            recipe.AddIngredient(ItemID.AnglerVest);
            recipe.AddIngredient(ItemID.AnglerPants);
            recipe.AddIngredient(ItemID.AnglerTackleBag);
            recipe.AddIngredient(ItemID.FishingPotion, 5);
            recipe.AddIngredient(ItemID.CratePotion, 5);
            recipe.AddIngredient(ItemID.SonarPotion, 5);
            recipe.AddIngredient(ItemID.MasterBait, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
