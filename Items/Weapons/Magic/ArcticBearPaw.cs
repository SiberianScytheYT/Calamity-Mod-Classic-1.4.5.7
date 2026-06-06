using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class ArcticBearPaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Arctic Bear Paw");
/*
            Tooltip.SetDefault("The savage mauling that fits in your pocket\n"
                               +"Fires spiritual claws that ignore walls and confuse enemies");
*/
        }
        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 18;
            Item.width = 34;
            Item.height = 22;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.knockBack = 10f;
            Item.value = Item.buyPrice(0, 48, 0, 0);
            Item.rare = 6;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ArcticBearPawProj>();
            Item.shootSpeed = 27f;
        }
    }
}
