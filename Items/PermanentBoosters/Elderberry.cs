using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.PermanentBoosters
{
    public class Elderberry : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Elderberry");
/*
            Tooltip.SetDefault("A tangy, tart flavor with a somewhat earthen touch\n" +
                               "Permanently increases maximum life by 25\n" +
                               "Can only be used if the max amount of life fruit has been consumed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useAnimation = 30;
            Item.rare = ItemRarityID.Red;
			Item.Calamity().customRarity = CalamityRarity.Turquoise;
			Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item4;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (modPlayer.eBerry || player.statLifeMax < 500)
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
                modPlayer.eBerry = true;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LifeFruit, 5);
			recipe.AddIngredient(ItemID.BlueBerries);
			recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 10);
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>(), 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
