using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class Keelhaul : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Keelhaul");
/*
            Tooltip.SetDefault("Summons a geyser upon hitting an enemy\n" +
                               "Crumple 'em like paper");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.damage = 55;
            Item.mana = 50;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item102;
            Item.autoReuse = true;
            Item.rare = 8;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.shoot = ModContent.ProjectileType<KeelhaulBubble>();
            Item.shootSpeed = 15f;
        }
    }
}
