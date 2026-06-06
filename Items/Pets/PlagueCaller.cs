using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class PlagueCaller : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plague Caller");
/*
            Tooltip.SetDefault("Summons a baby Plaguebringer pet");
*/
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<PlaguebringerBab>();
            Item.buffType = ModContent.BuffType<PlaguebringerBabBuff>();
            Item.rare = 7;
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
