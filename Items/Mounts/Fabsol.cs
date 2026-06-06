using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Mounts
{
    public class Fabsol : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Princess Spirit in a Bottle");
/*
            Tooltip.SetDefault("Summons the spirit of Cirrus, the Drunk Princess, in her alicorn form\n" +
                "Revengeance drop");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 9;
            Item.value = Item.buyPrice(3, 0, 0, 0);
            Item.expert = true;
            Item.UseSound = SoundID.Item3;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<AlicornMount>();
        }
    }
}
