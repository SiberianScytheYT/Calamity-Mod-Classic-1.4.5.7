using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatBuffs
{
    public class BloodyBoost : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloody Boost");
            /* Description.SetDefault("Increased offensive and defensive stats\n" +
			"Healing potions grant more health");*/
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.NurseCannotRemoveDebuff instead, and invert the logic */ = false;
        }

		public override void Update(Player player, ref int buffIndex)
		{
			player.Calamity().bloodPactBoost = true;
		}
	}
}
