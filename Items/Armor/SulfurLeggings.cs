using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Items.Weapons.Rogue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SulfurLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sulphurous Leggings");
/*
            Tooltip.SetDefault("Movement speed increased by 10%\n" +
                "Speed greatly increased while submerged in liquid");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.defense = 6;
            Item.rare = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.10f;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.moveSpeed += 0.6f;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Acidwood>(), 15);
            recipe.AddIngredient(ModContent.ItemType<UrchinStinger>(), 30);
            recipe.AddIngredient(ModContent.ItemType<SulphurousSand>(), 15);
            recipe.AddIngredient(ModContent.ItemType<SulfuricScale>(), 15);

            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
