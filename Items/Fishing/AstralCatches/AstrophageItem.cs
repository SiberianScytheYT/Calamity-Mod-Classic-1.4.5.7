using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.AstralCatches
{
    public class AstrophageItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astrophage");
/*
            Tooltip.SetDefault("Summons an astrophage to follow you around");
*/
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<Astrophage>();
            Item.buffType = ModContent.BuffType<AstrophageBuff>();
            Item.rare = 5;
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
