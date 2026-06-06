using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Placeables
{
    public class YellowDamageCandle : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spite");
            // Description.SetDefault("Its hateful glow flickers with ire");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.NurseCannotRemoveDebuff instead, and invert the logic */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().yellowCandle = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
			if (npc.Calamity().yellowCandle < npc.buffTime[buffIndex])
				npc.Calamity().yellowCandle = npc.buffTime[buffIndex];
			npc.DelBuff(buffIndex);
			buffIndex--;
        }
    }
}
