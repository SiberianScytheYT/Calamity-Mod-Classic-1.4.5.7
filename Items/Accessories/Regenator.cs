using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class Regenator : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Regenator");
/*
            Tooltip.SetDefault("Reduces max HP by 50% but greatly improves life regeneration");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.defense = 6;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.regenator = true;
        }
    }
}
