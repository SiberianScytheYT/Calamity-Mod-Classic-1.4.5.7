using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Accessories;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
    public class PlaguebringerCarapace : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plaguebringer Carapace");
/*
            Tooltip.SetDefault("Reduces the damage caused to you by the plague\n" +
			"12% increased minion damage and +1 max minions\n" +
			"Friendly bees inflict the plague");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 17;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().plaguebringerCarapace = true;
            player.maxMinions += 1;
            player.GetDamage(DamageClass.Summon) += 0.12f;
            player.Calamity().reducedPlagueDmg = true;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BeeBreastplate);
			recipe.AddIngredient(ModContent.ItemType<AlchemicalFlask>(), 2);
			recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 7);
			recipe.AddIngredient(ModContent.ItemType<InfectedArmorPlating>(), 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}