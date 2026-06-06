using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatDebuffs
{
    public class DeathModeHot : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heat Exhaustion");
            // Description.SetDefault("The overwhelming heat weakens your bodily functions. You need to look for equipment to protect you from the heat.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }
    }
}
