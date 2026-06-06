using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatDebuffs
{
    public class Clamity : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Clamity");
            // Description.SetDefault("The clams have been angered!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.NurseCannotRemoveDebuff instead, and invert the logic */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().clamity = true;
        }
    }
}
