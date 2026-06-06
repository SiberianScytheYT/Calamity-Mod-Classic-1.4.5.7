using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Ammo
{
    public class BloodRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Rune");
/*
            Tooltip.SetDefault("Used with the Ice Barrage \n" +
                               "Found in some sort of runic landscape");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.width = 22;
            Item.height = 24;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 10f;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.shoot = ModContent.ProjectileType<IceBarrageMain>();
            Item.shootSpeed = 0f;
            Item.ammo = Item.type; // CONSIDER -- Would item.type work here instead of a self reference?
        }
    }
}
