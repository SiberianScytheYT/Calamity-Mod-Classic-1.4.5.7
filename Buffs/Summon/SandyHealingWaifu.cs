using CalRD.CalPlayer;
using CalRD.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Summon
{
    public class SandyHealingWaifu : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rare Sand Elemental");
            // Description.SetDefault("The sand elemental will heal you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SandElementalHealer>()] > 0)
            {
                modPlayer.dWaifu = true;
            }
            if (!modPlayer.dWaifu)
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
