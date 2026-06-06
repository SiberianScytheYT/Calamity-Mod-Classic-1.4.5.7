using CalRD.Items.Mounts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Mounts
{
    public class TundraLeashBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Angry Dog");
            // Description.SetDefault("You are riding an angry dog");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<AngryDogMount>(), player);
            player.buffTime[buffIndex] = 10;
            player.Calamity().angryDog = true;
        }
    }
}
