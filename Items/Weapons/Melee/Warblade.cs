using CalRD.Buffs.StatDebuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Warblade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Warblade");
/*
            Tooltip.SetDefault("Critical hits cleave enemy armor, reducing their defense by 15 and protection by 25%");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 32;
            Item.height = 48;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.25f;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                target.AddBuff(ModContent.BuffType<WarCleave>(), 450);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            /*
            if (crit)
            {
                target.AddBuff(ModContent.BuffType<WarCleave>(), 450);
            }
            */
        }
    }
}
