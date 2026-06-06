using CalRD.Buffs.Summon;
using CalRD.CalPlayer;
using CalRD.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Items.Accessories
{
    public class EyeoftheStorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eye of the Storm");
/*
            Tooltip.SetDefault("Summons a cloud elemental to fight for you");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)/* tModPorter Suggestion: Consider using new hook CanAccessoryBeEquippedWith */
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (modPlayer.elementalHeart)
            {
                return false;
            }
            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.cloudWaifu = true;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(ModContent.BuffType<CloudyWaifu>()) == -1)
                {
                    player.AddBuff(ModContent.BuffType<CloudyWaifu>(), 3600, true);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<CloudElementalMinion>()] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<CloudElementalMinion>(), (int)(45 * player.MinionDamage()), 2f, Main.myPlayer, 0f, 0f);
                }
            }
        }
    }
}
