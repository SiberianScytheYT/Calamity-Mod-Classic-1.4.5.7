using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class MolluskShellplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mollusk Shellplate");
/*
            Tooltip.SetDefault("10% increased damage and 6% increased critical strike chance\n" +
                               "15% decreased movement speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 22;
            Item.value = Item.buyPrice(0, 20, 0, 0);
            Item.rare = 5;
            Item.defense = 22;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.1f;
            player.Calamity().AllCritBoost(6);
            player.moveSpeed -= 0.15f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SeaPrism>(), 25);
            recipe.AddIngredient(ModContent.ItemType<MolluskHusk>(), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
