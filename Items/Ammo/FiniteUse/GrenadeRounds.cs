using CalRD.Projectiles.Typeless.FiniteUse;
using Terraria.ModLoader;
namespace CalRD.Items.Ammo.FiniteUse
{
    public class GrenadeRounds : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Grenade Shell");
        }

        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 9;
            Item.consumable = true;
            Item.knockBack = 10f;
            Item.value = 15000;
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<GrenadeRound>();
            Item.shootSpeed = 12f;
            Item.ammo = ModContent.ItemType<GrenadeRounds>(); // CONSIDER -- Would item.type work here instead of a self reference?
        }
    }
}
