using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatBuffs
{
    public class AbsoluteRage : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Absolute Rage");
            // Description.SetDefault("Anger hardens the heart. Boosts max life by 5%.");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().absoluteRage = true;
        }
    }
}
