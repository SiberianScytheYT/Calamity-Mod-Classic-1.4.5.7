using CalRD.CalPlayer;
using CalRD.Projectiles.DraedonsArsenal;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Summon
{
    public class PoleWarperBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pole Warper");
            // Description.SetDefault("Sentient magnets are attracted to you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<PoleWarperSummon>()] > 0)
            {
                modPlayer.poleWarper = true;
            }
            if (!modPlayer.poleWarper)
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
