using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class WulfrumHeadgear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wulfrum Headgear");
/*
            Tooltip.SetDefault("3% increased ranged damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 75, 0);
            Item.rare = 1;
            Item.defense = 2; //7
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<WulfrumArmor>() && legs.type == ModContent.ItemType<WulfrumLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+3 defense\n" +
                "+5 defense when below 50% life";
            player.statDefense += 3; //10
            if (player.statLife <= (player.statLifeMax2 * 0.5f))
            {
                player.statDefense += 5; //15
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.03f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<WulfrumShard>(), 5);
            recipe.AddIngredient(ModContent.ItemType<EnergyCore>());
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
