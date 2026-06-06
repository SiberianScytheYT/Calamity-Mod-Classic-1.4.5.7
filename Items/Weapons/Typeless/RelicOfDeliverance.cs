using CalRD.Projectiles.Typeless;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Typeless
{
    public class RelicOfDeliverance : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Relic of Deliverance");
/*
            Tooltip.SetDefault("Summons a spear that causes you to lunge towards the mouse position\n" +
                               "The spear requires a charge-up. The longer the charge, the more strong the lunge\n" +
                               "The spear disappears immediately if you are not holding this item while charging\n" +
                               "If enough time has passed or the spear collides into a wall, the spear dies and the lunge ends");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 46;
            Item.damage = 1350;
            Item.useTime = Item.useAnimation = 25;
            Item.crit += 16;
            Item.reuseDelay = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item46;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.shoot = ModContent.ProjectileType<RelicOfDeliveranceSpear>();
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
    }
}
