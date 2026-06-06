using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatDebuffs
{
    public class WhisperingDeath : ModBuff
    {
        public static int DefenseReduction = 20;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Whispering Death");
            // Description.SetDefault("Death approaches; defense, attack power, and life regen reduced");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().wDeath = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().wDeath < npc.buffTime[buffIndex])
				npc.Calamity().wDeath = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }
    }
}
