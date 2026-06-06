using System;
using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.PermanentBoosters
{
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