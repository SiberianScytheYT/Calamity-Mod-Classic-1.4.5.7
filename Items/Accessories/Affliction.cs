using CalRD.Buffs.StatBuffs;
using CalRD.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class Affliction : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Affliction");
/*
            Tooltip.SetDefault("Gives you and all other players on your team +1 life regen,\n" +
                               "+10% max life, 7% damage reduction, 20 defense, and 10% increased damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 44;
            Item.value = CalamityGlobalItem.Rarity13BuyPrice;
            Item.accessory = true;
            Item.expert = true;
            Item.rare = 10;
        }

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Accessories/Affliction").Value);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.affliction = true;
            if (player.whoAmI != Main.myPlayer && player.miscCounter % 10 == 0)
            {
                if (Main.LocalPlayer.team == player.team && player.team != 0)
                {
                    Main.LocalPlayer.AddBuff(ModContent.BuffType<Afflicted>(), 20, true);
                }
            }
        }
    }
}
