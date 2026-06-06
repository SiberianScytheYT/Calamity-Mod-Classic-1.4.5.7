using CalRD.Items.Materials;
using CalRD.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
    public class PulseDragon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pulse Dragon");
/*
            Tooltip.SetDefault("Heavy duty flails, each containing a powerful generator which is activated upon launch.\n" +
            "Throws two dragon heads that emit electrical fields\n" +
            "Especially effective against inorganic targets");
*/
        }

        public override void SetDefaults()
        {
            CalamityGlobalItem modItem = Item.Calamity();

            Item.damage = 420;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 30;
            Item.height = 10;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 8f;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ItemRarityID.Red;
            modItem.customRarity = CalamityRarity.DraedonRust;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<PulseDragonProjectile>();
            Item.shootSpeed = 20f;

            modItem.UsesCharge = true;
            modItem.MaxCharge = 190f;
            modItem.ChargePerUse = 0.32f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            float offsetAngle = Main.rand.NextFloat(0.2f, 0.4f);
            velocity1 += player.velocity;
            float velocityAngle = velocity1.ToRotation();
            Projectile projectile = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), position, velocity, type, damage, Item.knockBack, player.whoAmI, velocityAngle, Main.rand.NextFloat(30f, PulseDragonProjectile.MaximumPossibleOutwardness));
            projectile.localAI[0] = 1f;
            projectile = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), position, velocity.RotatedBy(offsetAngle), type, damage, Item.knockBack, player.whoAmI, velocityAngle + offsetAngle, Main.rand.NextFloat(30f, PulseDragonProjectile.MaximumPossibleOutwardness));
            projectile.localAI[0] = -1f;
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 12);
            recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 18);
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 8);
            recipe.AddIngredient(ItemID.LunarBar, 4);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
