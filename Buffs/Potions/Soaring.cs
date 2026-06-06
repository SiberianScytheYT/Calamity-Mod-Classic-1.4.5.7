using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Potions
{
    public class Soaring : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Soaring");
            /* Description.SetDefault("Increased wing flight time and speed\n" +
				"True melee hits restore wing flight time"); */
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().soaring = true;
        }
    }
}
