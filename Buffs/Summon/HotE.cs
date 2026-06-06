using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Buffs.Summon
{
    public class HotE : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heart of the Elements");
            // Description.SetDefault("All elementals will protect you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();

            if (!modPlayer.allWaifus)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
