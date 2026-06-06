using CalRD.Items.Materials;
using CalRD.Buffs.DamageOverTime;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class PlagueReaperVest : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plague Reaper Vest");
/*
            Tooltip.SetDefault("Reduces the damage caused to you by the plague\n" +
                "15% increased ranged damage and 5% increased ranged critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 24, 0, 0);
            Item.rare = 8;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.15f;
            player.GetCritChance(DamageClass.Ranged) += 5;
            player.Calamity().reducedPlagueDmg = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 29); //I like prime numbers =)
			recipe.AddIngredient(ItemID.NecroBreastplate);
			recipe.AddIngredient(ItemID.Nanites, 19); //Change this to 30 and 20 and I will hunt you down
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
