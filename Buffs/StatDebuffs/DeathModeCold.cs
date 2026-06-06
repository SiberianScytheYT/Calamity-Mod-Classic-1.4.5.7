using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatDebuffs
{
    public class DeathModeCold : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Freezing Weather");
            // Description.SetDefault("The weather slows your movement as you freeze to death. You need to look for equipment to protect you from the cold.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }
    }
}
