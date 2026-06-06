using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class MolluskShelleggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mollusk Shelleggings");
/*
            Tooltip.SetDefault("12% increased damage and 4% increased critical strike chance\n" +
                               "7% decreased movement speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = 5;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.Calamity().AllCritBoost(4);
            player.moveSpeed -= 0.07f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SeaPrism>(), 20);
            recipe.AddIngredient(ModContent.ItemType<MolluskHusk>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
