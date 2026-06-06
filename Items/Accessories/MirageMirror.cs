using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class MirageMirror : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mirage Mirror");
/*
            Tooltip.SetDefault("Bend light around you\n" +
                "Reduces enemy aggression outside of the abyss\n" +
                "Stealth generates 30% faster when standing still and 20% faster while moving");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.stealthGenStandstill += 0.3f;
            modPlayer.stealthGenMoving += 0.2f;
            player.aggro -= 200;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MagicMirror);
            recipe.AddIngredient(ItemID.BlackLens);
            recipe.AddIngredient(ItemID.Bone, 50);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IceMirror);
            recipe.AddIngredient(ItemID.BlackLens);
            recipe.AddIngredient(ItemID.Bone, 50);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
