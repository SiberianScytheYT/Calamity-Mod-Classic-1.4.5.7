using CalRD.CalPlayer;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class TheFirstShadowflame : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The First Shadowflame");
/*
            Tooltip.SetDefault("It is said that in the past, Prometheus descended from the heavens to grant man fire.\n" +
                "If that were true, then it is surely the demons of hell that would have risen from below to do the same.\n" +
                "Minions inflict shadowflame on enemy hits.");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.shadowMinions = true;
        }
    }
}
