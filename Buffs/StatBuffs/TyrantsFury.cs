using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatBuffs
{
    public class TyrantsFury : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tyrant's Fury");
            // Description.SetDefault("30% increased melee damage and 10% increased melee crit chance");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().tFury = true;
        }
    }
}
