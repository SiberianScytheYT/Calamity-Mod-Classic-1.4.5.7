using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class AlchemicalFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Alchemical Flask");
/*
            Tooltip.SetDefault("All attacks inflict the plague\n" +
                "Reduces the damage caused to you by the plague\n" +
				"Removes the blindness effect caused by the plague\n" +
                "Projectiles spawn plague seekers on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = 8;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.alchFlask = true;
            modPlayer.reducedPlagueDmg = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.Bezoar);
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
