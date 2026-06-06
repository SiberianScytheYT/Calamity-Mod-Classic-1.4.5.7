using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class Phantoplasm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Phantoplasm");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 5));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.rare = 10;
            Item.value = Item.sellPrice(gold: 1);
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, 0);
    }
}
