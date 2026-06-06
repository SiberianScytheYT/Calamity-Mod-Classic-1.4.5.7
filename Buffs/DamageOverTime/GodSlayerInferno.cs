using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.DamageOverTime
{
    public class GodSlayerInferno : ModBuff
    {
        public static int DefenseReduction = 10;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("God Slayer Inferno");
            // Description.SetDefault("Your flesh is burning off");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().gsInferno = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().gsInferno < npc.buffTime[buffIndex])
				npc.Calamity().gsInferno = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }
    }
}
