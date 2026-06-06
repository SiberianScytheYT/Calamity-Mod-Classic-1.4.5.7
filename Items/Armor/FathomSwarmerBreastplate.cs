using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class FathomSwarmerBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fathom Swarmer Breastplate");
/*
            Tooltip.SetDefault("12% increased damage reduction\n" +
				"6% increased minion damage and +1 max minion\n" +
                "Boosted defense and regen increased while submerged in liquid\n" +
				"Reduces defense loss within the Abyss");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 24, 0, 0);
            Item.rare = 7;
            Item.defense = 22;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.06f;
            player.endurance += 0.12f;
            player.maxMinions++;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.statDefense += 10;
                player.lifeRegen += 5;
            }
			player.Calamity().fathomSwarmerBreastplate = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpiderBreastplate);
            recipe.AddIngredient(ModContent.ItemType<VictideBar>(), 12);
            recipe.AddIngredient(ModContent.ItemType<PlantyMush>(), 10);
            recipe.AddIngredient(ModContent.ItemType<AbyssGravel>(), 18);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}