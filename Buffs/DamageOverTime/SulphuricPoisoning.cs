using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.DamageOverTime
{
    public class SulphuricPoisoning : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sulphuric Poisoning");
            // Description.SetDefault("The acidic water burns away your flesh"); 
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().sulphurPoison = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().sulphurPoison < npc.buffTime[buffIndex])
				npc.Calamity().sulphurPoison = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }
    }
}
