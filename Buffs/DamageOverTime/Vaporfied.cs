using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.DamageOverTime
{
    public class Vaporfied : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vaporfied");
            // Description.SetDefault("Vape");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			npc.Calamity().vaporfied = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().vaporfied = true;
        }
    }
}
