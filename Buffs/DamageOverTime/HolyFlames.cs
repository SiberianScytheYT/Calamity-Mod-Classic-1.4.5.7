using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.DamageOverTime
{
    public class HolyFlames : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Holy Flames");
            // Description.SetDefault("Dissolving from holy light");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().hFlames = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().hFlames < npc.buffTime[buffIndex])
				npc.Calamity().hFlames = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }
    }
}
