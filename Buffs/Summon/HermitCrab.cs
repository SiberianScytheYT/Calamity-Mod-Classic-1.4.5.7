using CalRD.CalPlayer;
using CalRD.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Summon
{
    public class HermitCrab : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hermit Crab");
            // Description.SetDefault("The hermit crab will protect you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<HermitCrabMinion>()] > 0)
            {
                modPlayer.hCrab = true;
            }

            if (!modPlayer.hCrab)
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
