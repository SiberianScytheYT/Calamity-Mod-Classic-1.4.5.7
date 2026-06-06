using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.PermanentBoosters
{
    public class PhantomHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Phantom Heart");
/*
            Tooltip.SetDefault("Permanently increases maximum mana by 50");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item29;
            Item.consumable = true;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override bool CanUseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (modPlayer.pHeart)
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
                    player.ManaEffect(50);
                }
                CalamityPlayer modPlayer = player.Calamity();
                modPlayer.pHeart = true;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Phantoplasm>(), 100);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
