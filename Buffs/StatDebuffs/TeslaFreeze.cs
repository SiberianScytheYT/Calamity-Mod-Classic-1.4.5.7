using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatDebuffs
{
	public class TeslaFreeze : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Galvanic Corrosion");
            // Description.SetDefault("Your limbs have begun to corrode");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().tesla < npc.buffTime[buffIndex])
				npc.Calamity().tesla = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().teslaFreeze = true;
        }
    }
}
