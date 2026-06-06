using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Cooldowns
{
	public class PrismaticCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Prismatic Cooldown");
			// Description.SetDefault("Your laser attack is recharging");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.NurseCannotRemoveDebuff instead, and invert the logic */ = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.Calamity().prismaticCooldown = true;
		}
	}
}
