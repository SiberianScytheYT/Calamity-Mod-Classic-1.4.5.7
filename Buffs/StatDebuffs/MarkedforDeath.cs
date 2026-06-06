using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatDebuffs
{
    public class MarkedforDeath : ModBuff
    {
        public static int DefenseReduction = 5;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Marked");
            // Description.SetDefault("Damage reduction reduced");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().marked < npc.buffTime[buffIndex])
				npc.Calamity().marked = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }
    }
}
