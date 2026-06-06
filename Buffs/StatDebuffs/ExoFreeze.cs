using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatDebuffs
{
    public class ExoFreeze : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Exo Freeze");
            // Description.SetDefault("Cannot move");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().eFreeze < npc.buffTime[buffIndex])
				npc.Calamity().eFreeze = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().eFreeze = true;
        }
    }
}
