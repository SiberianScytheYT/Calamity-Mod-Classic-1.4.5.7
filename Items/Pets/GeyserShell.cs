using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class GeyserShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Geyser Shell");
/*
            Tooltip.SetDefault("Summons a little flak hermit");
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
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.shoot = ModContent.ProjectileType<FlakPet>();
            Item.buffType = ModContent.BuffType<FlakPetBuff>();
            Item.rare = 6;
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
