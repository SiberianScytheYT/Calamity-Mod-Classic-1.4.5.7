using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatBuffs
{
    public class ScarfMeleeBoost : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scarf Boost");
            // Description.SetDefault("10% increased damage, 5% increased crit chance, and 5% increased melee speed");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().sMeleeBoost = true;
        }
    }
}
