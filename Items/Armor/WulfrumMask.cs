using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.CalPlayer;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class WulfrumMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wulfrum Mask");
/*
            Tooltip.SetDefault("3% increased rogue damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 75, 0);
            Item.rare = 1;
            Item.defense = 1; //6
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<WulfrumArmor>() && legs.type == ModContent.ItemType<WulfrumLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.setBonus = CalamityUtils.GetTextValue("Misc.CalamitySetBonus.WulfrumMask");
            player.statDefense += 3; //9
            if (player.statLife <= (int)(player.statLifeMax2 * 0.5))
            {
                player.statDefense += 5; //14
            }
            modPlayer.rogueStealthMax += 0.5f;
            modPlayer.wearingRogueArmor = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().throwingDamage += 0.03f;
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
