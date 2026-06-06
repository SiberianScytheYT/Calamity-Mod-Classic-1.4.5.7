using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Items.Weapons.Rogue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class SulfurBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sulphurous Breastplate");
/*
            Tooltip.SetDefault("10% rogue damage and 5% critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 1, 15, 0);
            Item.defense = 7;
            Item.rare = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().throwingDamage += 0.1f;
            player.Calamity().throwingCrit += 5;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Acidwood>(), 20);
            recipe.AddIngredient(ModContent.ItemType<UrchinStinger>(), 50);
            recipe.AddIngredient(ModContent.ItemType<SulphurousSand>(), 20);
            recipe.AddIngredient(ModContent.ItemType<SulfuricScale>(), 20);

            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
