using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    [AutoloadEquip(EquipType.Waist)]
    public class DepthCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Depths Charm");
/*
            Tooltip.SetDefault("Reduces the damage caused by the pressure of the abyss while out of breath\n" +
                "Removes the bleed effect caused by the upper layers of the abyss");
*/
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
            modPlayer.depthCharm = true;
        }
    }
}
