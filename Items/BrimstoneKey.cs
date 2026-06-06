using Terraria.ModLoader;
namespace CalRD.Items
{
    public class BrimstoneKey : ModItem
    {
        public override void SetStaticDefaults()
        {
/*
            Tooltip.SetDefault("Opens locked ashen chests");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 100;
            Item.rare = 1;
        }
    }
}
