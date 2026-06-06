using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatBuffs
{
    public class AbyssalDivingSuitPlates : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abyssal Diving Suit Plates");
            // Description.SetDefault("The plates will absorb 15% damage");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().abyssalDivingSuitPlates = true;
        }
    }
}
