using CalRD.CalPlayer;
using CalRD.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Summon
{
    public class GastricBelcherBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gastric Aberration");
            // Description.SetDefault("The aquatic aberration will protect you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<GastricBelcher>()] > 0)
            {
                modPlayer.gastricBelcher = true;
            }
            if (!modPlayer.gastricBelcher)
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
