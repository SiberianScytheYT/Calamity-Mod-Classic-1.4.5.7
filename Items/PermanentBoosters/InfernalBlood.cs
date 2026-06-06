using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.PermanentBoosters
{
    public class InfernalBlood : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Infernal Blood");
/*
            Tooltip.SetDefault("Permanently makes Rage Mode do 15% more damage\n" +
                "Revengeance drop");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useAnimation = 30;
            Item.rare = 8;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item122;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (modPlayer.rageBoostTwo)
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
                modPlayer.rageBoostTwo = true;
            }
            return true;
        }
    }
}
