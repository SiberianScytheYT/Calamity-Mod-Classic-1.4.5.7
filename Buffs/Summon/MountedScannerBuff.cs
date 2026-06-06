using CalRD.CalPlayer;
using CalRD.Projectiles.DraedonsArsenal;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Summon
{
    public class MountedScannerBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mounted Scanner");
            // Description.SetDefault("Powerful machinery surrounds you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MountedScannerSummon>()] > 0)
            {
                modPlayer.mountedScanner = true;
            }
            if (!modPlayer.mountedScanner)
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
