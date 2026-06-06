using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class AeroStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aero Stone");
/*
            Tooltip.SetDefault("One of the ancient relics\n" +
                "Increases movement speed by 10%, jump speed by 20%, and all damage by 3%");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 8));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 50;
            Item.value = CalamityGlobalItem.Rarity2BuyPrice;
            Item.rare = 2;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            bool autoJump = Main.player[Main.myPlayer].autoJump;
			string jumpAmt = autoJump ? "5" : "20";
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
                {
                    line2.Text = "Increases movement speed by 10%, jump speed by " + jumpAmt + "%, and all damage by 3%";
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Lighting.AddLight((int)player.Center.X / 16, (int)player.Center.Y / 16, 0f, 0.425f, 0.425f);
            player.moveSpeed += 0.1f;
            player.jumpSpeedBoost += player.autoJump ? 0.25f : 1.0f;
            player.GetDamage(DamageClass.Generic) += 0.03f;
        }
    }
}
