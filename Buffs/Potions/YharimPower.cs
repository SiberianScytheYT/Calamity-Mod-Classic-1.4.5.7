using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Potions
{
    public class YharimPower : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Yharim's Power");
            // Description.SetDefault("You feel like you can break the world in two...with your bare hands!");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().yPower = true;
        }
    }
}
