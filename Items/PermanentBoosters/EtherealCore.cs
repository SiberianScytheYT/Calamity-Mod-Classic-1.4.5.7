using CalRD.CalPlayer;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.PermanentBoosters
{
    public class EtherealCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ethereal Core");
/*
            Tooltip.SetDefault("Permanently increases maximum mana by 50");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useAnimation = 30;
            Item.rare = 10;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item29;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (modPlayer.eCore)
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
                modPlayer.eCore = true;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 25);
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 25);
            recipe.AddIngredient(ItemID.FragmentNebula, 20);
            recipe.AddIngredient(ItemID.FallenStar, 50);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
