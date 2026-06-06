using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class RadiantOoze : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Radiant Ooze");
/*
            Tooltip.SetDefault("You emit light and regen life more quickly at night");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;
            Item.rare = 4;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Calamity().rOoze = true;
            if (!Main.dayTime)
            {
                Lighting.AddLight((int)(player.position.X + (float)(player.width / 2)) / 16, (int)(player.position.Y + (float)(player.height / 2)) / 16, 1f, 1f, 0.6f);
                player.lifeRegen += 1;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MurkySludge>(), 5);
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
