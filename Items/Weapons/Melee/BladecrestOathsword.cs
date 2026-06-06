using CalRD.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class BladecrestOathsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bladecrest Oathsword");
/*
            Tooltip.SetDefault("Sword of an ancient demon lord\n" +
                "Fires a blood scythe");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 56;
            Item.damage = 25;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.shoot = ModContent.ProjectileType<BloodScythe>();
            Item.shootSpeed = 6f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 240);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OnFire, 240);
        }
    }
}
