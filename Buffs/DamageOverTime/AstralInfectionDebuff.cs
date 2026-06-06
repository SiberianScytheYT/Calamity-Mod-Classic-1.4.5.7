using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.DamageOverTime
{
    public class AstralInfectionDebuff : ModBuff
    {
        public static int DefenseReduction = 6;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Infection");
            // Description.SetDefault("Your flesh is melting off");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().astralInfection = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().astralInfection < npc.buffTime[buffIndex])
				npc.Calamity().astralInfection = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }
    }
}
