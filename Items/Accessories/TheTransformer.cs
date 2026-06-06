using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class TheTransformer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Transformer");
/*
            Tooltip.SetDefault("Taking damage releases a blast of sparks\n" +
                                "Sparks do extra damage in Hardmode\n" +
                                "Immunity to Electrified and you resist all electrical projectile and enemy damage\n" +
                                "Enemy bullets do half damage to you and are reflected back at the enemy for 800% their original damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = 1;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.aSparkRare = true;
            modPlayer.aSpark = true;
        }
    }
}
