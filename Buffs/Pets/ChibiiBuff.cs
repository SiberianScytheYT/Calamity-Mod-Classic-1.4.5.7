using CalRD.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Pets
{
    public class ChibiiBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Chibii Devourer");
            // Description.SetDefault("What? Were you expecting someone else?");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;

            player.Calamity().chibii = true;

            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<ChibiiDoggo>()] <= 0;

            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<ChibiiDoggo>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
