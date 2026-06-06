using CalRD.Buffs.DamageOverTime;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TitanArm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Titan Arm");
/*
            Tooltip.SetDefault("Slap Hand but better");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 58;
			Item.scale = 1.5f;
            Item.damage = 60;
            Item.crit += 96; //more knockback huehue
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useTurn = true;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.knockBack = 9001f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
        }
    }
}
