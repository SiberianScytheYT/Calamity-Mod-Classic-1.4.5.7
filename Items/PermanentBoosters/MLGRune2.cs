using CalRD.CalPlayer;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.PermanentBoosters
{
    public class MLGRune2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Celestial Onion");
/*
            Tooltip.SetDefault("Alien power pulses inside its layers\n"
                               +"Consuming it does something that cannot be reversed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.rare = 10;
            Item.maxStack = 99;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item4;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (modPlayer.extraAccessoryML)
            {
                return false;
            }
            return true;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.itemAnimation > 0 && !modPlayer.extraAccessoryML && player.itemTime == 0)
            {
                player.itemTime = Item.useTime;
                modPlayer.extraAccessoryML = true;
                if (!CalamityWorld.onionMode)
                {
                    CalamityWorld.onionMode = true;
                }
            }
            return true;
        }
    }

    public class OnionSlot : ModAccessorySlot
    {
        public override bool IsEnabled()
        {
            bool active = !Player.active || Main.masterMode;
            return !active && Player.GetModPlayer<CalamityPlayer>().extraAccessoryML;
        }
        public override bool IsHidden() => IsEmpty && !IsEnabled();
    }
}
