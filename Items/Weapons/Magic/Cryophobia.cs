using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class Cryophobia : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cryophobia");
/*
            Tooltip.SetDefault("Chill");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 18;
            Item.width = 56;
            Item.height = 34;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item117;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<CryoBlast>();
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}
