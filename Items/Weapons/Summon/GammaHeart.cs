using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class GammaHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gamma Heart");
/*
            Tooltip.SetDefault("Summons radioactive heads that are bound by your body\n" +
                               "If the entity already exists, using this item again will cause it to gain more heads");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 60;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item42;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 16;
            Item.damage = 120;
            Item.knockBack = 3f;
            Item.autoReuse = true;
            Item.useTime = Item.useAnimation = 15;
            Item.shoot = ModContent.ProjectileType<GammaHead>();
            Item.shootSpeed = 10f;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), player.Center, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
            }
            return false;
        }
    }
}
