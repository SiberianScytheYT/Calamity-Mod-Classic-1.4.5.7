using CalRD.CalPlayer;
using CalRD.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Buffs.Summon
{
    public class Calamari : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Calamari");
            // Description.SetDefault("The squid will protect you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CalamariMinion>()] > 0)
            {
                modPlayer.calamari = true;
            }
            if (!modPlayer.calamari)
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
