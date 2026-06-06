using CalRD.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class BalefulHarvester : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Baleful Harvester");
/*
            Tooltip.SetDefault("Shoots skulls that split into homing fire orbs on enemy hits\n" +
                "Summons flaming pumpkins on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 160;
            Item.width = 66;
            Item.height = 66;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.shoot = ModContent.ProjectileType<BalefulHarvesterProjectile>();
            Item.shootSpeed = 6f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TheHorsemansBlade);
            recipe.AddIngredient(ItemID.BookofSkulls);
            recipe.AddIngredient(ItemID.FragmentSolar, 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            CalamityGlobalItem.HorsemansBladeOnHit(player, target.whoAmI, (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f) * 1.5f), Item.knockBack, false);
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            CalamityGlobalItem.HorsemansBladeOnHit(player, target.whoAmI, (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f) * 1.5f), Item.knockBack, false);
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
