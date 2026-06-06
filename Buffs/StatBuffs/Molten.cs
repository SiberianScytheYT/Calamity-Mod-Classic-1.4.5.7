using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatBuffs
{
    public class Molten : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Molten");
            // Description.SetDefault("Resistant to cold effects");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().molten = true;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
			if (CalamityWorld.death)
				tip += ". Provides cold protection in Death Mode";
		}
    }
}
