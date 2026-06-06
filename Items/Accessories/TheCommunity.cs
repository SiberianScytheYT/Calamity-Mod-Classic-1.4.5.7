using CalRD.CalPlayer;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class TheCommunity : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Community");
/*
            Tooltip.SetDefault("The heart of (most of) the Terraria community\n" +
                "Legendary Accessory\n" +
                "Starts off with weak buffs to all of your stats\n" +
                "The stat buffs become more powerful as you progress\n" +
                "Reduces the DoT effects of harmful debuffs inflicted on you\n" +
                "Boosts your maximum flight time by 15%\n" +
                "Thank you to all of my supporters that made this mod a reality\n" +
                "Revengeance drop");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 10));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = 7;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.Rainbow;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.community = true;
        }
    }
}
