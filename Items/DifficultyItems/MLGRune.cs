using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.DifficultyItems
{
    public class MLGRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Demon Trophy");
/*
            Tooltip.SetDefault("Boosts spawn rate by 1.25 times\n" +
                               "Effects cannot be reversed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 99;
            Item.rare = 1;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item119;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !CalamityWorld.demonMode;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            // This world syncing code should only be run by one entity- the server, to prevent a race condition
            // with the packets.
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return true;

            CalamityWorld.demonMode = true;
            CalamityNetcode.SyncWorld();
            return true;
        }
    }
}
