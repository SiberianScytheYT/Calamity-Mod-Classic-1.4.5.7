using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
	public class GiantTortoiseShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Giant Tortoise Shell");
/*
            Tooltip.SetDefault("10% reduced movement speed\n" +
                "Enemies take damage when they hit you");
*/
        }

        public override void SetDefaults()
        {
            Item.defense = 8;
            Item.width = 20;
            Item.height = 24;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed -= 0.1f;
            player.thorns += 0.25f;
        }
    }
}
