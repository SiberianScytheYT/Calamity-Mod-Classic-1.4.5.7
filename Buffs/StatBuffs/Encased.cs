using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatBuffs
{
    public class Encased : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Encased");
            // Description.SetDefault("30 defense and +30% damage reduction, but...");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.BuffID.Sets.LongerExpertDebuff[Type] = true; // instead */ = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.NurseCannotRemoveDebuff instead, and invert the logic */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().encased = true;
            if (player.buffTime[buffIndex] == 2)
            {
                SoundEngine.PlaySound(SoundID.Item27, player.position);
                player.immune = true;
                player.immuneNoBlink = false;
                player.immuneTime = 90;
            }
        }
    }
}
