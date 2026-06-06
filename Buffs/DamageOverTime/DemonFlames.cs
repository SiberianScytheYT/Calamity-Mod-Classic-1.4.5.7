using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.DamageOverTime
{
    public class DemonFlames : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Demon Flames");
            // Description.SetDefault("Another burning debuff");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().dFlames < npc.buffTime[buffIndex])
				npc.Calamity().dFlames = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }
    }
}
