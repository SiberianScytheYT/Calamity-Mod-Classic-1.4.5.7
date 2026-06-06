using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class CosmicPlushie : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cosmic Plushie");
/*
            Tooltip.SetDefault("Summons the devourer of the cosmos...?\n"
                               +"Sharp objects possibly included\n"
                               +"Suppresses friendly red devils");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;
            Item.width = 28;
            Item.height = 36;
            Item.value = Item.sellPrice(0, 7, 0, 0);
            Item.shoot = ModContent.ProjectileType<ChibiiDoggo>();
            Item.buffType = ModContent.BuffType<ChibiiBuff>();
            Item.rare = 10;
            Item.UseSound = SoundID.Meowmere;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
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
