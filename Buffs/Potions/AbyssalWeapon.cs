using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Potions
{
    public class AbyssalWeapon : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abyssal Weapon");
            // Description.SetDefault("Melee weapons inflict abyssal flames, 15% increased movement speed");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().aWeapon = true;
        }
    }
}
