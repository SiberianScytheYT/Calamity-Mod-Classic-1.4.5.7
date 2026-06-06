using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class EffulgentFeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Effulgent Feather");
/*
            Tooltip.SetDefault("It vibrates with fluffy golden energy");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 11));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 6, 50, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }
    }
}
