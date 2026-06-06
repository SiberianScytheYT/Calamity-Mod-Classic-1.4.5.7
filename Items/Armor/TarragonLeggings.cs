using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class TarragonLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tarragon Leggings");
/*
            Tooltip.SetDefault("Leggings of a fabled explorer\n" +
				"20% increased movement speed; 35% increase when below half health\n" +
                "6% increased damage and critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.defense = 32;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.20f;
            if (player.statLife <= (int)((double)player.statLifeMax2 * 0.5))
            {
                player.moveSpeed += 0.15f;
            }
            player.GetDamage(DamageClass.Generic) += 0.06f;
            player.Calamity().AllCritBoost(6);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 11);
            recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 12);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
