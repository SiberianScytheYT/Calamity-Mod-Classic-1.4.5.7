using CalRD.CalPlayer;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class HeartofDarkness : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heart of Darkness");
/*
            Tooltip.SetDefault("Gives 10% increased damage while you have the absolute rage buff\n" +
                "Increases your chance of getting the absolute rage buff\n" +
                "Rage mode does more damage\n" +
                "You gain rage over time\n" +
                "Revengeance drop");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.heartOfDarkness = true;
        }
    }
}
