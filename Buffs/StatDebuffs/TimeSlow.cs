using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatDebuffs
{
    public class TimeSlow : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Time Distortion");
            // Description.SetDefault("Time is slowed");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.NurseCannotRemoveDebuff instead, and invert the logic */ = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().timeSlow < npc.buffTime[buffIndex])
				npc.Calamity().timeSlow = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }

        /*public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().timeSlow = true;
        }*/
    }
}
