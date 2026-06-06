using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatBuffs
{
    public class BossZen : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Boss Zen");
            // Description.SetDefault("The active boss is reducing spawn rates...a lot");
            Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.NurseCannotRemoveDebuff instead, and invert the logic */ = false;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().bossZen = true;
        }
    }
}
