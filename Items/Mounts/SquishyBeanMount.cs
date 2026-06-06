using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Mounts
{
    public class SquishyBeanMount : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Suspicious Looking Jelly Bean");
/*
            Tooltip.SetDefault("JELLY BEAN");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 9;
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.expert = true;
            Item.UseSound = SoundID.Item3;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<SquishyBean>();
        }
    }
}
