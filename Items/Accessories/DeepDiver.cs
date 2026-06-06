using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class DeepDiver : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Deep Diver");
/*
            Tooltip.SetDefault("15% increased damage, defense, and movement speed when underwater\n" +
                                "While underwater you gain the ability to dash great distances");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = CalamityGlobalItem.Rarity2BuyPrice;
            Item.rare = 2;
            Item.defense = 2;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                CalamityPlayer modPlayer = player.Calamity();
                modPlayer.deepDiver = true;
                modPlayer.dashMod = 5;
            }
        }
    }
}
