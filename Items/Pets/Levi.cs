using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class Levi : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Levi");
/*
            Tooltip.SetDefault("Summons a baby Leviathan pet");
*/
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<LeviPet>();
            Item.buffType = ModContent.BuffType<LeviBuff>();
            Item.rare = 10;
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
