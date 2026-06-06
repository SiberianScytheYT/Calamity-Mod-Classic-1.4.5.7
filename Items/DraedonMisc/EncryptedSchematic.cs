using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.DraedonMisc
{
    public class EncryptedSchematic : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Encrypted Schematic");
            //Tooltip.SetDefault("It's impossible to decipher"); // But not for long.
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.rare = ItemRarityID.Red;
            Item.Calamity().customRarity = CalamityRarity.DraedonRust;
            Item.maxStack = 999;
        }
    }
}
