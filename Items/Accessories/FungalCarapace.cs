using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class FungalCarapace : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fungal Carapace");
/*
            Tooltip.SetDefault("You emit a mushroom spore explosion when you are hit");
*/
        }

        public override void SetDefaults()
        {
            Item.defense = 2;
            Item.width = 20;
            Item.height = 24;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;
            Item.rare = 4;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.fCarapace = true;
        }
    }
}
