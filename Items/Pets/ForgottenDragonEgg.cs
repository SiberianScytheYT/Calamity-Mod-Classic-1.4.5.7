using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class ForgottenDragonEgg : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forgotten Dragon Egg");
/*
            Tooltip.SetDefault("Calls Akato, son of Yharon, to your side");
*/
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<Akato>();
            Item.buffType = ModContent.BuffType<AkatoYharonBuff>();
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
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
