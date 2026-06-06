using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.DamageOverTime
{
    public class Nightwither : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nightwither");
            // Description.SetDefault("Incinerated by lunar rays");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().nightwither < npc.buffTime[buffIndex])
				npc.Calamity().nightwither = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().nightwither = true;
        }
    }
}
