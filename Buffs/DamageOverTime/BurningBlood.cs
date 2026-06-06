using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.DamageOverTime
{
    public class BurningBlood : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Burning Blood");
            // Description.SetDefault("Your blood is on fire");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().bBlood = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().bBlood < npc.buffTime[buffIndex])
				npc.Calamity().bBlood = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }
    }
}
