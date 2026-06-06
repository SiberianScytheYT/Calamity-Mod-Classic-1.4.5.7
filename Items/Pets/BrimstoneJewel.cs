using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class BrimstoneJewel : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimstone Jewel");
/*
            Tooltip.SetDefault("The ultimate reward for defeating such a beast...\n" +
			"Who knew she'd be so darn cute!");
*/
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<SCalPet>();
            Item.buffType = ModContent.BuffType<SCalPetBuff>();
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
