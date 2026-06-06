using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class Acidwood : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Acidwood");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.maxStack = 999;
            Item.value = 0;
        }
    }
}
