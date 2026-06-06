using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class JoyfulHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Joyful Heart");
/*
            Tooltip.SetDefault("It's oddly warm. Attracts the forbidden one.");
*/
        }
        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.shoot = ModContent.ProjectileType<LadShark>();
            Item.buffType = ModContent.BuffType<LadBuff>();
            Item.rare = 5;
            Item.UseSound = SoundID.Item2;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 15, true);
            }
        }
    }
}
