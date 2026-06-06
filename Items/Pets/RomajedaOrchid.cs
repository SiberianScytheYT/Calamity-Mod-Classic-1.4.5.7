using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class RomajedaOrchid : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Romajeda Orchid");
/*
            Tooltip.SetDefault("Summons a never forgotten friend");
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
            Item.value = Item.buyPrice(gold: 40);
            Item.shoot = ModContent.ProjectileType<KendraPet>();
            Item.buffType = ModContent.BuffType<Kendra>();
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item44;
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
