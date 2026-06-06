using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Alcohol
{
    public class FireballBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fireball");
            // Description.SetDefault("Fire weapon damage boosted, life regen reduced");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.NurseCannotRemoveDebuff instead, and invert the logic */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().fireball = true;
        }
    }
}
