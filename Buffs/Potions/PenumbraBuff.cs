using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Potions
{
    public class PenumbraBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Penumbra");
            /* Description.SetDefault("Stealth regenerates 15% faster while moving\n" +
                "At night, stealth additionally regenerates 15% faster while standing still\n" +
				"Both boosts increase to 20% during a solar eclipse");*/
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().penumbra = true;
        }
    }
}
