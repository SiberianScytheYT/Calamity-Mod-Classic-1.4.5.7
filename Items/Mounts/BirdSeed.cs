using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Mounts
{
    public class BirdSeed : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Folly Feed");
/*
            Tooltip.SetDefault("Summons a monstrosity");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.UseSound = SoundID.NPCHit51;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<BUMBLEDOGE>();
        }
    }
}
