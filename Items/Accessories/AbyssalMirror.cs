using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class AbyssalMirror : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abyssal Mirror");
/*
            Tooltip.SetDefault("Light does not reach the depths of the ocean\n" +
                "Significantly reduces enemy aggression, even in the abyss\n" +
                "Stealth generates 30% faster when standing still and 20% faster while moving\n" +
                "Grants a slight chance to evade attacks, releasing a cloud of lumenyl fluid which damages and stuns nearby enemies\n" +
                "Evading an attack grants a lot of stealth\n" +
                "This evade has a 20s cooldown before it can occur again");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 38;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = 7;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.stealthGenStandstill += 0.3f;
            modPlayer.stealthGenMoving += 0.2f;
            modPlayer.abyssalMirror = true;
            player.aggro -= 450;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MirageMirror>());
            recipe.AddIngredient(ModContent.ItemType<InkBomb>());
            recipe.AddIngredient(ItemID.SpectreBar, 8);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
