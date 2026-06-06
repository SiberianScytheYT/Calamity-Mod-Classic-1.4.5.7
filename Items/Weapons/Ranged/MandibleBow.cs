using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//using TerrariaOverhaul;

namespace CalRD.Items.Weapons.Ranged
{
    public class MandibleBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mandible Bow");
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 30f;
            Item.useAmmo = AmmoID.Arrow;
        }

        /*public void OverhaulInit()
        {
            this.SetTag("bow");
        }*/
    }
}
