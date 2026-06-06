using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.DamageOverTime
{
    public class Shred : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shred");
            // Description.SetDefault("Blood");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().pShred < npc.buffTime[buffIndex])
				npc.Calamity().pShred = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }
    }
}
