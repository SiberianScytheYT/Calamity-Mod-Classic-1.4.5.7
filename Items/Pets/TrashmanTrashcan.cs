using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class TrashmanTrashcan : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Trash Can");
/*
            Tooltip.SetDefault("Summons the trash man");
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
            Item.shoot = ModContent.ProjectileType<DannyDevitoPet>();
            Item.buffType = ModContent.BuffType<DannyDevito>();
            Item.rare = 5;
            Item.UseSound = SoundID.NPCDeath13;
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
