using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.PermanentBoosters
{
    public class StarlightFuelCell : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Starlight Fuel Cell");
/*
            Tooltip.SetDefault("Permanently increases Adrenaline Mode damage by 15% and damage reduction by 5%\n" +
                "Revengeance drop");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useAnimation = 30;
            Item.rare = 7;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item122;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (modPlayer.adrenalineBoostTwo)
            {
                return false;
            }
            return true;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (player.itemAnimation > 0 && player.itemTime == 0)
            {
                player.itemTime = Item.useTime;
                CalamityPlayer modPlayer = player.Calamity();
                modPlayer.adrenalineBoostTwo = true;
            }
            return true;
        }
    }
}
