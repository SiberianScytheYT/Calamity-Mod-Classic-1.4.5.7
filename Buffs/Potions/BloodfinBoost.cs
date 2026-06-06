using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Potions
{
    public class BloodfinBoost : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloodfin Boost");
            // Description.SetDefault("Don't let the blood get to your head");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().bloodfinBoost = true;
        }
    }
}
