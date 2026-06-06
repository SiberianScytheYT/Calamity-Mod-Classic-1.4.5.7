using CalRD.CalPlayer;
using CalRD.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Summon
{
    public class SandyWaifu : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sand Elemental");
            // Description.SetDefault("The sand elemental will protect you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SandElementalMinion>()] > 0)
            {
                modPlayer.sWaifu = true;
            }
            if (!modPlayer.sWaifu)
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
