using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class AstralBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Breastplate");
/*
            Tooltip.SetDefault("+80 max mana and +20 max life\n" +
                               "+3 max minions\n" +
                               "Creature detection");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 32, 0, 0);
            Item.rare = 9;
            Item.defense = 25;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 20;
            player.statManaMax2 += 80;
            player.maxMinions += 3;
            player.detectCreature = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 12);
			recipe.AddIngredient(ItemID.MeteoriteBar, 9);
			recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
