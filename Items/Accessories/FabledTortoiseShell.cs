using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class FabledTortoiseShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fabled Tortoise Shell");
/*
            Tooltip.SetDefault("50% reduced movement speed\n" +
                                "Enemies take damage when they hit you\n" +
                                "You move quickly for a short time if you take damage");
*/
        }

        public override void SetDefaults()
        {
            Item.defense = 35;
            Item.width = 20;
            Item.height = 24;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.fabledTortoise = true;
            player.moveSpeed -= 0.5f;
            player.thorns += 0.25f;
        }
    }
}
