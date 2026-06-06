using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Ammo
{
    public class IcicleArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Icicle Arrow");
/*
            Tooltip.SetDefault("Shatters into shards on impact");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.consumable = true;
            Item.width = 14;
            Item.height = 50;
            Item.knockBack = 2.5f;
            Item.value = Item.buyPrice(0, 0, 0, 80);
            Item.rare = 6;
            Item.shoot = ModContent.ProjectileType<IcicleArrowProj>();
            Item.shootSpeed = 1.0f;
            Item.ammo = AmmoID.Arrow;
            Item.maxStack = 999;
        }
    }
}
