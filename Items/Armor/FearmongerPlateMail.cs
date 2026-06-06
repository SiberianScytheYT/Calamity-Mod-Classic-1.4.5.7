using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
    public class FearmongerPlateMail : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fearmonger Plate Mail");
/*
            Tooltip.SetDefault("+100 max life and 8% increased damage reduction\n" +
			"+2 max minions\n" +
			"5% increased damage and critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 60);
            Item.defense = 50;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 100;
            player.endurance += 0.08f;
            player.maxMinions += 2;
            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.Calamity().AllCritBoost(5);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpookyBreastplate);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 3);
            recipe.AddIngredient(ItemID.SoulofFright, 12);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}