using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class PlagueKeeper : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plague Keeper");
/*
            Tooltip.SetDefault("Fires a plague and bee cloud");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 74;
            Item.damage = 80;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 90;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<PlagueBeeDust>();
            Item.shootSpeed = 9f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VirulentKatana>());
            recipe.AddIngredient(ItemID.BeeKeeper);
            recipe.AddIngredient(ItemID.FragmentSolar, 10);
			recipe.AddIngredient(ModContent.ItemType<InfectedArmorPlating>(), 5);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 300);
            for (int i = 0; i < 3; i++)
            {
                int bee = Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center.X, player.Center.Y, 0f, 0f, player.beeType(),
                    player.beeDamage((int)(Item.damage * player.MeleeDamage()) / 3), player.beeKB(0f), player.whoAmI, 0f, 0f);
                Main.projectile[bee].penetrate = 1;
                Main.projectile[bee].Calamity().forceMelee = true;
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 300);
            for (int i = 0; i < 3; i++)
            {
                int bee = Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center.X, player.Center.Y, 0f, 0f, player.beeType(),
                    player.beeDamage((int)(Item.damage * player.MeleeDamage()) / 3), player.beeKB(0f), player.whoAmI, 0f, 0f);
                Main.projectile[bee].penetrate = 1;
                Main.projectile[bee].Calamity().forceMelee = true;
            }
		}
    }
}
