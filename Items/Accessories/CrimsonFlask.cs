using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class CrimsonFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crimson Flask");
/*
            Tooltip.SetDefault("7% increased damage reduction and +3 defense while in the crimson");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.Rarity2BuyPrice;
            Item.rare = 2;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.ZoneCrimson)
            {
                player.statDefense += 3;
                player.endurance += 0.07f;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ViciousPowder, 15);
            recipe.AddIngredient(ItemID.Vertebrae, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
