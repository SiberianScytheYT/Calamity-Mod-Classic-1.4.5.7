using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class SamuraiBadge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Samurai Badge");
/*
            Tooltip.SetDefault("Increases melee damage, true melee damage and melee speed the closer you are to enemies\n" +
				"Max boost is 30% increased melee damage, true melee damage and melee speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.badgeOfBraveryRare = true;
        }
    }
}
