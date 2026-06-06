using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Weapons.Magic
{
    public class EyeofMagnus : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eye of Magnus");
/*
            Tooltip.SetDefault("Fires powerful beams\n" +
                "Heals mana and health on hit");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 80;
            Item.damage = 55;
            Item.rare = 5;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2f;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/LaserCannon");
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.height = 50;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.shoot = ModContent.ProjectileType<MagnusBeam>();
            Item.shootSpeed = 12f;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }
    }
}
