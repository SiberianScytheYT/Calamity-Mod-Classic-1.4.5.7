using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.DamageOverTime
{
    public class AbyssalFlames : ModBuff
    {
        public static int DefenseReduction = 6;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abyssal Flames");
            // Description.SetDefault("Your soul is being consumed");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.NurseCannotRemoveDebuff instead, and invert the logic */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().aFlames = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().aFlames < npc.buffTime[buffIndex])
				npc.Calamity().aFlames = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }
    }
}
