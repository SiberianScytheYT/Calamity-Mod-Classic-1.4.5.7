using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.PermanentBoosters
{
    public class Dragonfruit : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dragonfruit");
/*
            Tooltip.SetDefault("Though somewhat bland, what taste can be described is unlike any other experienced\n" +
                               "Permanently increases maximum life by 25\n" +
                               "Can only be used if the max amount of life fruit has been consumed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item4;
            Item.consumable = true;
            Item.rare = ItemRarityID.Red;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool CanUseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (modPlayer.dFruit || player.statLifeMax < 500)
            {
                return false;
            }
            return true;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (player.itemAnimation > 0 && player.itemTime == 0)
            {
                player.itemTime = Item.useTime;
                if (Main.myPlayer == player.whoAmI)
                {
                    player.HealEffect(25);
                }
                CalamityPlayer modPlayer = player.Calamity();
                modPlayer.dFruit = true;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LifeFruit, 5);
			recipe.AddIngredient(ItemID.SkyBlueFlower);
            recipe.AddIngredient(ItemID.FragmentSolar, 15);
			recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 10);
			recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
