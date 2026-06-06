using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
	public class DragonbloodDisgorger : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dragonblood Disgorger");
/*
            Tooltip.SetDefault("Summons a skeletal dragon and her two children\n" +
                               "Requires 6 minion slots to be summoned\n" +
                               "There can only be one family");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 300;
            Item.mana = 30;
            Item.width = 64;
            Item.height = 62;
            Item.useTime = Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 8;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
            Item.UseSound = SoundID.DD2_SkeletonDeath;
            Item.shoot = ModContent.ProjectileType<SkeletalDragonMother>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
            return false;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0 && player.maxMinions >= 5;

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<BloodstoneCore>(), 12);
            r.AddTile(TileID.LunarCraftingStation);
            r.Register();
        }
    }
}
