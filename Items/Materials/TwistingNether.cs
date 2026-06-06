using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class TwistingNether : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Twisting Nether");
        }

        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 12;
            Item.maxStack = 999;
            Item.rare = 10;
            Item.value = Item.buyPrice(0, 7, 0, 0);
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float num = (float)Main.rand.Next(90, 111) * 0.01f;
            num *= Main.essScale;
            Lighting.AddLight((int)((Item.position.X + (float)(Item.width / 2)) / 16f), (int)((Item.position.Y + (float)(Item.height / 2)) / 16f), 0.5f * num, 0.1f * num, 0.7f * num);
        }
    }
}
