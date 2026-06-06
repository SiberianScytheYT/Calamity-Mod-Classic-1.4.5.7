using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class VictideBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Victide Breastplate");
/*
            Tooltip.SetDefault("5% increased damage reduction and critical strike chance\n" +
                "Defense increased while submerged in liquid");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 2;
            Item.defense = 5; //9
        }

        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.05f;
            player.Calamity().AllCritBoost(5);
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.statDefense += 5;
                player.endurance += 0.1f;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VictideBar>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
