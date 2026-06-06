using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Magic
{
    public class Thunderstorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Thunderstorm");
/*
            Tooltip.SetDefault("Make it rain");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 360;
            Item.mana = 50;
            Item.DamageType = DamageClass.Magic;
            Item.width = 48;
            Item.height = 22;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/PlasmaBlast");
            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ModContent.ProjectileType<ThunderstormShot>();
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}
