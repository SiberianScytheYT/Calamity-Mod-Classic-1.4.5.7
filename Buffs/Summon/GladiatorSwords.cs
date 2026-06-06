using CalRD.CalPlayer;
using CalRD.Projectiles.Typeless;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Summon
{
    public class GladiatorSwords : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gladiator Swords");
            // Description.SetDefault("The gladiator swords will protect you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<GladiatorSword>()] > 0)
            {
                modPlayer.glSword = true;
            }
            if (!modPlayer.glSword)
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
