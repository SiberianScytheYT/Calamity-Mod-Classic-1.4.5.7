using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Mounts
{
    public class Brimrose : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimrose");
/*
            Tooltip.SetDefault("Summons a brimrose mount");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(1, 50, 0, 0);
            Item.rare = 9;
            Item.expert = true;
            Item.UseSound = SoundID.Item3;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<PhuppersChair>();
        }
    }
}
