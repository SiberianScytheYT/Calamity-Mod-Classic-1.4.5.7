using CalRD.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Pets
{
    public class SparksBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sparks");
            // Description.SetDefault("Eats butterflies");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.Calamity().sparks = true;
            bool PetProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Sparks>()] <= 0;
            if (PetProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.position.X + (player.width / 2), player.position.Y + (player.height / 2), 0f, 0f, ModContent.ProjectileType<Sparks>(), 0, 0f, player.whoAmI, 50f, 0f);
            }
        }
    }
}
