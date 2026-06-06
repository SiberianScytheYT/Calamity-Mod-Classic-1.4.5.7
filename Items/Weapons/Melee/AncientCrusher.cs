using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class AncientCrusher : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ancient Crusher");
/*
            Tooltip.SetDefault("Summons fossil spikes on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 62;
			Item.height = 62;
			Item.scale = 2f;
			Item.damage = 55;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Amber, 8);
            recipe.AddIngredient(ItemID.FossilOre, 35);
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>(), 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<FossilSpike>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, Main.myPlayer);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<FossilSpike>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, Main.myPlayer);
        }
    }
}
