using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class FathomSwarmerBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fathom Swarmer Greaves");
/*
            Tooltip.SetDefault("4% increased minion damage\n" +
				"Grants the ability to swim\n" +
                "Speed greatly increased while submerged in liquid");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 18, 0, 0);
            Item.rare = 7;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.04f;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.moveSpeed += 0.5f;
            }
			player.ignoreWater = true;
            if (player.wingTime <= 0) //ignore flippers while the player can fly
				player.accFlipper = true;
        }

        /*
		public override void UpdateVanity(Player player, EquipType type)
		{
			player.Calamity().fathomSwarmerTail = true;
		}
		*/

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpiderGreaves);
            recipe.AddIngredient(ModContent.ItemType<VictideBar>(), 9);
            recipe.AddIngredient(ModContent.ItemType<PlantyMush>(), 8);
            recipe.AddIngredient(ModContent.ItemType<AbyssGravel>(), 15);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}