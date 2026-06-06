using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class AquaticEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aquatic Emblem");
/*
            Tooltip.SetDefault("Most ocean enemies become friendly and provides waterbreathing\n" +
                "Being underwater slowly boosts your defense over time but also slows movement speed\n" +
                "The defense boost and movement speed reduction slowly vanish while outside of water\n" +
                "Maximum defense boost is 30, maximum movement speed reduction is 5%\n" +
                "Provides a small amount of light in the abyss\n" +
                "Moderately reduces breath loss in the abyss");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.accessory = true;
            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.aquaticEmblem = true;
            player.npcTypeNoAggro[NPCID.Shark] = true;
            player.npcTypeNoAggro[NPCID.SeaSnail] = true;
            player.npcTypeNoAggro[NPCID.PinkJellyfish] = true;
            player.npcTypeNoAggro[NPCID.Crab] = true;
            player.npcTypeNoAggro[NPCID.Squid] = true;
            player.gills = true;
        }
    }
}
