using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.DamageOverTime
{
    public class HeavyBleeding : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heavy Bleeding");
            // Description.SetDefault("You're losing a lot of blood");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex) => player.Calamity().waterLeechBleeding = true;
    }
}
