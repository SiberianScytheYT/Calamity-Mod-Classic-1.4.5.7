using CalRD.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.DraedonMisc
{
    public class DraedonsLogHell : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Draedon's Log - Hell");
/*
            Tooltip.SetDefault("Click to view its contents");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.rare = ItemRarityID.Red;
            Item.Calamity().customRarity = CalamityRarity.DraedonRust;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (Main.myPlayer == player.whoAmI)
                PopupGUIManager.FlipActivityOfGUIWithType(typeof(DraedonLogHellGUI));
            return true;
        }
    }
}
