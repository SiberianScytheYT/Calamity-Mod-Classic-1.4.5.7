using CalRD.CalPlayer;
using CalRD.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Summon
{
    public class SilvaSummonSetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Silva Crystal");
            // Description.SetDefault("The crystal will protect you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SilvaCrystal>()] > 0)
            {
                modPlayer.sCrystal = true;
            }
            if (!modPlayer.sCrystal)
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
