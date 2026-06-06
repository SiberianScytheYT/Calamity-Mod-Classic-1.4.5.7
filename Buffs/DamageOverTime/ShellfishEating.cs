using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.DamageOverTime
{
    public class ShellfishEating : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shellfish Claps");
            // Description.SetDefault("Clamfest");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().shellfishVore < npc.buffTime[buffIndex])
				npc.Calamity().shellfishVore = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }
    }
}
