using CalRD.CalPlayer;
using CalRD.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Summon
{
    public class StatigelSummonSetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Baby Slime God");
            // Description.SetDefault("The slime god will protect you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CrimsonSlimeGodMinion>()] > 0)
            {
                modPlayer.sGod = true;
            }
            else if (player.ownedProjectileCounts[ModContent.ProjectileType<CorruptionSlimeGodMinion>()] > 0)
            {
                modPlayer.sGod = true;
            }
            if (!modPlayer.sGod)
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
