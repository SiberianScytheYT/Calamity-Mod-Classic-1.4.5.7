using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatBuffs
{
    public class SirenWaterSpeed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ocean's Blessing");
            // Description.SetDefault("15% increased max speed and acceleration");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().sirenWaterBuff = true;
        }
    }
}
