using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class FoxDrive : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fox Drive");
/*
            Tooltip.SetDefault("'It contains 1 file on it'\n"
                               +"'Fox.cs'");
*/
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<FoxPet>();
            Item.buffType = ModContent.BuffType<Fox>();
            Item.rare = 9;
            Item.expert = true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }
}
