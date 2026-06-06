using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class ReaperToothNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Reaper Tooth Necklace");
/*
            Tooltip.SetDefault("A grisly trophy from the ultimate predator\n" + "12% increased damage\n" + "Increases armor penetration by 100\n");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 50;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.Rarity13BuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.GetArmorPenetration(DamageClass.Generic) += 100;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SharkToothNecklace);
            recipe.AddIngredient(ItemID.AvengerEmblem);
            recipe.AddIngredient(ModContent.ItemType<ReaperTooth>(), 6);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 15);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 15);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
