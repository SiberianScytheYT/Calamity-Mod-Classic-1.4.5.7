using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class AbyssalTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abyssal Tome");
/*
            Tooltip.SetDefault("Casts a slow-moving ball of dark energy");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.rare = 4;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AbyssBall>();
            Item.shootSpeed = 9f;
        }
    }
}
