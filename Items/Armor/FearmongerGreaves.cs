using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class FearmongerGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fearmonger Greaves");
/*
            Tooltip.SetDefault("+2 max minions and 6% increased damage\n" +
			"50% increased minion knockback\n" +
			"15% increased movement speed\n" +
			"Taking damage makes you move very fast for a short time");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 45);
            Item.defense = 44;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 2;
            player.GetDamage(DamageClass.Generic) += 0.06f;
            player.GetKnockback(DamageClass.Summon).Base += 0.5f;
            player.moveSpeed += 0.15f;
            player.panic = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpookyLeggings);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 2);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}